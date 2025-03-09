using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Last.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddControllersWithViews();

// Регистрация LastContext как сервиса с использованием SQL Server
builder.Services.AddDbContext<LastContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка аутентификации с использованием cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "YourAppCookie"; // Имя cookie
        options.LoginPath = "/Authorization/Index"; // Путь к странице входа
        options.AccessDeniedPath = "/Home/AccessDenied"; // Путь к странице отказа в доступе
    });

var app = builder.Build();

// Настройка конвейера HTTP-запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Обработка ошибок в production
    app.UseHsts(); // Включение HSTS (HTTP Strict Transport Security)
}

app.UseHttpsRedirection(); // Перенаправление HTTP на HTTPS
app.UseStaticFiles(); // Поддержка статических файлов

app.UseRouting(); // Маршрутизация

app.UseAuthentication(); // Включение аутентификации
app.UseAuthorization(); // Включение авторизации

// Настройка маршрутов по умолчанию
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // Запуск приложения