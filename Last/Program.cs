using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Last.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LastContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка аутентификации с использованием cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "YourAppCookie"; 
        options.LoginPath = "/Authorization/Index"; 
        options.AccessDeniedPath = "/Home/AccessDenied"; 
    });

var app = builder.Build();

// Настройка конвейера HTTP-запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); 
}

app.UseHttpsRedirection(); 
app.UseStaticFiles(); 

app.UseRouting(); 

app.UseAuthentication(); 
app.UseAuthorization(); 

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
    }
    if (context.Vacins.Any())
    {
        var vetClinics = new List<Vetcin>
    {
        new Vetcin { Adress = "ул. Ленина, 10", Phone = "+7 (123) 456-7890" },
        new Vetcin { Adress = "ул. Пушкина, 25", Phone = "+7 (234) 567-8901" },
        new Vetcin { Adress = "ул. Гагарина, 5", Phone = "+7 (345) 678-9012" },
        new Vetcin { Adress = "ул. Советская, 15", Phone = "+7 (456) 789-0123" },
        new Vetcin { Adress = "ул. Мира, 30", Phone = "+7 (567) 890-1234" },
        new Vetcin { Adress = "ул. Садовая, 12", Phone = "+7 (678) 901-2345" },
        new Vetcin { Adress = "ул. Лесная, 8", Phone = "+7 (789) 012-3456" },
        new Vetcin { Adress = "ул. Центральная, 1", Phone = "+7 (890) 123-4567" }
    };

        context.Vetcins.AddRange(vetClinics);
    }
    context.SaveChanges();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run(); 