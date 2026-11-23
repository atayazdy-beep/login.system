# WasteLogin — ASP.NET Core MVC + SQLite

پروژه نمونه ورود مدیر و کاربر برای سامانه مدیریت جمع‌آوری پسماند.

## نیازمندی
- .NET 10 SDK
- اینترنت برای دریافت پکیج‌های NuGet در اولین اجرا

## اجرا در GitHub Codespaces
در ترمینال Codespaces، داخل پوشه پروژه:

```bash
dotnet restore
dotnet run --urls http://0.0.0.0:5000
```

سپس از تب **Ports** پورت `5000` را باز کنید (Open in Browser).

در اولین اجرا، فایل `wastelogin.db` و جداول ASP.NET Identity به‌صورت خودکار ساخته می‌شوند.

## حساب‌های نمایشی (فقط در محیط Development)
- مدیر: username `admin` / password `Admin@12345`
- کاربر: username `user` / password `User@12345`

برای تغییر رمزهای نمایشی، قبل از اجرای برنامه متغیرهای محیطی زیر را تنظیم کنید:
- `SEED_ADMIN_USERNAME`, `SEED_ADMIN_EMAIL`, `SEED_ADMIN_PASSWORD`
- `SEED_USER_USERNAME`, `SEED_USER_EMAIL`, `SEED_USER_PASSWORD`

## ساختار
- `Models/ApplicationUser.cs`: مدل کاربر
- `Data/ApplicationDbContext.cs`: اتصال EF Core به SQLite و Identity
- `Data/SeedData.cs`: ساخت نقش‌ها و دو حساب نمایشی
- `Controllers/AccountController.cs`: ورود و خروج با بررسی نقش
- `Controllers/AdminController.cs`: صفحه محافظت‌شده مدیر
- `Controllers/UserController.cs`: صفحه محافظت‌شده کاربر
- `Views/Account/Login.cshtml`: فرم ورود دوحالته

## نکات مهم
- این پروژه از `EnsureCreated` برای ساخت اولیه دیتابیس استفاده می‌کند تا راه‌اندازی Codespaces ساده باشد. برای توسعه بلندمدت و تغییر ساختار جداول، بهتر است به EF Core Migrations مهاجرت شود.
- فایل دیتابیس محلی `wastelogin.db` را به GitHub commit نکنید؛ این فایل می‌تواند شامل اطلاعات حساب‌ها باشد.
- رمزهای پیش‌فرض صرفاً برای نمایش در محیط توسعه‌اند و برای استفاده واقعی باید تغییر کنند.
- این نسخه فقط ورود و نقش‌های Admin/User را پیاده‌سازی می‌کند؛ ثبت‌نام عمومی و امکانات مدیریت کاربران در محدوده فعلی نیست.
