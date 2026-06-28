# CityInfo API

> A production-ready, secure, and robust ASP.NET Core 8.0 Web API designed for managing and exploring city data and local points of interest.

---

## 📌 Project Overview / معرفی پروژه

پروژه **CityInfo API** یک وب‌سایت سرویس‌محور (Web API) با کارایی بالا است که با استفاده از سی‌شارپ و **NET 8.0.** توسعه یافته است. این پروژه نمایانگر اصول پیشرفته توسعه بک‌اند از جمله معماری داده امن، جداسازی لایه‌ها و رعایت استانداردهای REST است. ویژگی‌های واقعی و کاربردی مانند احراز هویت با JWT، مدیریت پیشرفته لاگ‌ها و به‌روزرسانی‌های جزئی (Partial Updates) در این پروژه پیاده‌سازی شده‌اند تا یک سرویس تحت وب در مقیاس سازمانی را شبیه‌سازی کنند.

---

## 🚀 Key Features & Architecture / ویژگی‌ها و معماری پروژه

- **Target Framework:** .NET 8.0 (Modern, Fast, and Cross-platform)
- **Database & ORM:** Entity Framework Core with **SQLite** for efficient, file-based relational data management.
- **Authentication & Security:** Fully integrated **JWT (JSON Web Token) Authentication** to secure API endpoints.
- **Object Mapping:** **AutoMapper** integration for seamless and clean entity-to-DTO (Data Transfer Object) conversions.
- **Advanced HTTP Methods:** Support for **JSON Patch (HTTP PATCH)** to perform efficient partial resources updates.
- **Structured Logging:** Powered by **Serilog** to log system events and diagnostics to the Console and external files.
- **API Documentation:** Interactive **Swagger / OpenAPI** interface for rapid endpoint testing and exploration.
- **Custom Services:** Includes custom components like `FileExtentionTypeProvider` for handling specialized file MIME types.

---

## 📁 Project Structure / ساختار فایل‌ها

```text
├── DbContexts/               # Database Context configuration
├── Entities/                 # Core database models (City, PointOfInterest)
├── Models/                   # Data Transfer Objects (DTOs) for secure data exposure
├── Controllers/              # API Controllers managing endpoints
├── Services/                 # Repository layer and business logic
├── Cityinfo.db               # Local SQLite Database file
├── FileExtentionTypeProvider.cs # Custom MIME type provider for file management
├── Program.cs                # Application bootstrap and DI container configuration
└── appsettings.json          # Application configuration profiles
```

⚡ API Endpoints / مسیرهای اصلی API
This API supports secure and standard HTTP methods:

GET /api/cities - Fetch all cities (Supports filtering/pagination).

GET /api/cities/{id} - Fetch details of a specific city.

GET /api/cities/{cityId}/pointsofinterest - Fetch attractions for a given city.

POST /api/cities/{cityId}/pointsofinterest - Add a new point of interest (Authorized).

PUT /api/cities/{cityId}/pointsofinterest/{id} - Replace an entire attraction (Authorized).

PATCH /api/cities/{cityId}/pointsofinterest/{id} - Partially update an attraction via JSON Patch (Authorized).

DELETE /api/cities/{cityId}/pointsofinterest/{id} - Remove an attraction (Authorized).


1.Clone this repository:
```bash
git clone https://github.com/AliFani1382/Cityinfo.API.git
```

2.Navigate to the API folder:
```bash
cd Cityinfo.API
```

3.Restore packages and build the project:
```bash
dotnet restore
dotnet build
```

4.Run the API:
```bash
dotnet run --project Cityinfo.API
```

5.Test via Swagger: Open http://localhost:5142/swagger (یا هر پورتی که روی سیستم شما فعال است) to test the endpoints and experience the interactive UI.

🔒 Security Notice / هشدار امنیتی
Note: The SecretForKey used for JWT token generation in appsettings.Development.json is configured for development purposes. In a production environment, this key should be injected securely using Environment Variables or Azure Key Vault.

👤 Developer / توسعه‌دهنده
Name: Ali Fani (علی فانی)

Role: Backend Developer / .NET Software Engineer

GitHub: @AliFani1382
