# Template

A reusable template project providing common features across systems.

## Overview

This solution serves as a foundation template containing common functionality that can be shared and reused across multiple systems and applications. It provides a clean architecture structure with pre-configured authentication, authorization, localization, and other essential features.

## 🚀 Getting Started

### 1. Clone the repository


```bash
git clone git@github.com:odainasser/Template.git
cd Template
```

### 2. Restore and build

```bash
dotnet restore
dotnet build
```

### 3. Run the application

Start the API:
```bash
dotnet run --project Api/Api.csproj
```

Start the Web (in a separate terminal):
```bash
dotnet run --project Web/Web.csproj
```

### 4. Access the application

- **Web App**: https://localhost:7098
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger

## 👤 Default Users

> These accounts are for local development and testing. Do NOT use in production.

| Role | Email | Password |
|------|-------|----------|
| Administrator | admin@sma.gov.ae | Sma@123! |
| Client | client@sma.gov.ae | Sma@123! |

## 🗄️ Database Setup

The database is automatically created and seeded when you run the API for the first time. Migrations are applied automatically on application startup.

### Connection String

Update `appsettings.json` in both `Api` and `Web` projects:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TemplateDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Reset Database

If you need to reset the database during development:

```powershell
dotnet ef database drop --project Infrastructure/Infrastructure.csproj --startup-project Api/Api.csproj --force
```

Restart the API - the database will be automatically recreated and seeded.

## 📁 Project Structure

```
Template/
├── Api/                    # API project (Minimal APIs)
│   ├── Endpoints/          # API endpoint definitions
│   ├── Middleware/         # Custom middleware
│   └── Authorization/      # Authorization handlers
├── Application/            # Application layer
│   ├── Common/             # Shared interfaces and models
│   ├── Features/           # Feature-specific DTOs and requests
│   ├── Services/           # Business services
│   └── Validators/         # FluentValidation validators
├── Domain/                 # Domain layer
│   ├── Constants/          # Application constants
│   ├── Entities/           # Domain entities
│   └── Enums/              # Enumerations
├── Infrastructure/         # Infrastructure layer
│   ├── Data/               # Seeders and configurations
│   ├── Identity/           # ASP.NET Identity implementation
│   ├── Persistence/        # EF Core DbContext
│   └── Services/           # Infrastructure services
└── Web/                    # Blazor WebAssembly project
    ├── Components/         # Razor components
    │   ├── Common/         # Shared components
    │   ├── Layout/         # Layout components
    │   └── Pages/          # Page components
    ├── Services/           # Client-side services
    └── wwwroot/            # Static files and localization
```

## 🌐 Localization

The application supports English and Arabic languages. Localization files are located in:
- `Web/wwwroot/localization/en.json`
- `Web/wwwroot/localization/ar.json`

## ⚙️ Environment Configuration

Update `appsettings.json` or set environment variables for:
- `ConnectionStrings:DefaultConnection`
- `JwtSettings:Secret`
- `JwtSettings:Issuer`
- `JwtSettings:Audience`

## 📝 Notes

- Projects target .NET 10 and C# 14 features. Use the matching SDK.
- Layouts and role-based routing are implemented in the `Web` project.
- Client and Admin pages are segregated by role with automatic redirects.
- Database migrations are applied automatically on startup.

## 📄 License

This project is proprietary software developed for Sharjah Museums Authority.

## 👥 Contributors

- Odai Nasser ([@odainasser](https://github.com/odainasser))