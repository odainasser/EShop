using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;
using Web.Services;
using Web.Authentication;
using Web.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Blazored.LocalStorage;
using System.Globalization;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
// If web app settings were moved to wwwroot, attempt to load them into configuration
try
{
    var httpForConfig = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    var baseResp = await httpForConfig.GetAsync("appsettings.json");
    if (baseResp.IsSuccessStatusCode)
    {
        using var stream = await baseResp.Content.ReadAsStreamAsync();
        builder.Configuration.AddJsonStream(stream);
    }

    var env = builder.HostEnvironment.Environment;
    var envResp = await httpForConfig.GetAsync($"appsettings.{env}.json");
    if (envResp.IsSuccessStatusCode)
    {
        using var stream2 = await envResp.Content.ReadAsStreamAsync();
        builder.Configuration.AddJsonStream(stream2);
    }
}
catch
{
    // ignore - fall back to existing configuration sources
}
builder.RootComponents.Add<Web.Components.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient with an authorization handler that reads token from local storage
builder.Services.AddTransient<AuthorizationHeaderHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    var apiBaseUrl = builder.Configuration["AppUrl"] ?? builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthorizationHeaderHandler>();

// Add HttpClient for localization files (without auth header)
builder.Services.AddHttpClient("LocalizationClient", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

// Provide IHttpClientFactory-created client for DI (used by services)
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

// JSON-based Localization
builder.Services.AddSingleton<JsonStringLocalizerFactory>();
builder.Services.AddScoped<IJsonStringLocalizer>(sp =>
{
    var factory = sp.GetRequiredService<JsonStringLocalizerFactory>();
    return factory.CreateSync();
});

// Add authentication and authorization
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Register Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Add custom authorization
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddAuthorizationCore();

// Add management services
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IRoleManagementService, RoleManagementService>();
builder.Services.AddScoped<IPermissionManagementService, PermissionManagementService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Register client-side services (all communicate via HTTP API)
builder.Services.AddScoped<IUserLogService, ClientUserLogService>();
builder.Services.AddScoped<ISystemSettingService, ClientSystemSettingService>();
builder.Services.AddScoped<IMediaService, ClientMediaService>();
builder.Services.AddScoped<ILookupService, ClientLookupService>();
// Museum client service removed

var host = builder.Build();

try 
{
    var js = host.Services.GetRequiredService<IJSRuntime>();
    var cultureName = await js.InvokeAsync<string>("AppUtils.initializeCulture");
    
    if (!string.IsNullOrEmpty(cultureName))
    {
        var culture = new CultureInfo(cultureName);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
    
    // Preload localizations
    var localizerFactory = host.Services.GetRequiredService<JsonStringLocalizerFactory>();
    await localizerFactory.CreateAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Initialization error: {ex.Message}");
}

await host.RunAsync();
