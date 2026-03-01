using Application;
using Api.Endpoints;
using Api.Middleware;
using Api.Authorization;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Services
// =========================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add Swagger services
builder.Services.AddSwaggerGen();

// Add CORS
var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy =>
        {
            policy.WithOrigins(corsOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add custom authorization
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddAuthorization();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// =========================
// App
// =========================

var app = builder.Build();

// =========================
// Database Migrations + Seeding
// =========================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Console.WriteLine("[Startup] Ensuring database schema is up to date...");
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.EnsureCreatedAsync();
        Console.WriteLine("[Startup] Database ready.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database");
        Console.WriteLine($"[Startup][ERROR] {ex.Message}");
        throw;
    }
}

// =========================
// Middleware
// =========================

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Booking API Documentation";
    });
}

app.UseHttpsRedirection();

// Serve static files from wwwroot (uploads, assets, etc.) so media URLs like /uploads/... are reachable
app.UseStaticFiles();

app.UseCors("AllowBlazorClient");

app.UseAuthentication();

app.UseMiddleware<PermissionMiddleware>();

app.UseAuthorization();

// =========================
// Endpoints
// =========================

app.MapAuthEndpoints();
app.MapUserEndpoints();
app.MapRoleEndpoints();
app.MapPermissionEndpoints();
app.MapSettingsEndpoints();
app.MapUserLogEndpoints();
app.MapMediaEndpoints();
app.MapLookupEndpoints();
// Museum endpoints removed during feature cleanup.

// =========================
// Run
// =========================

app.Run();
