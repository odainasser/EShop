using Application;
using Api.Endpoints;
using Api.Middleware;
// Api.Authorization removed — permissions module deprecated
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

// Custom permission policy removed
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

// Database initializer removed during cleanup

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

// Permission middleware removed (identity service removed)
app.UseAuthorization();

// Map auth endpoints (login) restored
app.MapAuthEndpoints();

app.UseAuthorization();

// =========================
// Endpoints
// =========================

app.MapUserEndpoints();
// Museum/Role/Permission/Media/Lookup endpoints removed during feature cleanup.

// =========================
// Run
// =========================

app.Run();
