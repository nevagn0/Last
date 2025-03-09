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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LastContext>();
    if (!context.Vacins.Any())
    {
        var vacins = new List<Vacin>
        {
            new Vacin { Type = "Pfizer" },
            new Vacin { Type = "Moderna" },
            new Vacin { Type = "AstraZeneca" },
            new Vacin { Type = "Johnson & Johnson" },
            new Vacin { Type = "Sputnik V" },
            new Vacin { Type = "Sinopharm" },
            new Vacin { Type = "Sinovac" },
            new Vacin { Type = "Novavax" },
            new Vacin { Type = "Covaxin" },
            new Vacin { Type = "Covishield" },
            new Vacin { Type = "CanSino" },
            new Vacin { Type = "EpiVacCorona" },
            new Vacin { Type = "ZyCoV-D" },
            new Vacin { Type = "Abdala" },
            new Vacin { Type = "Soberana" }
        };

        context.Vacins.AddRange(vacins);
        context.SaveChanges();
    }
}

// Настройка маршрутов по умолчанию
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run(); // Запуск приложения