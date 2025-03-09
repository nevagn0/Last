using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Last.Models;

var builder = WebApplication.CreateBuilder(args);

// ���������� �������� � ���������
builder.Services.AddControllersWithViews();

// ����������� LastContext ��� ������� � �������������� SQL Server
builder.Services.AddDbContext<LastContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� �������������� � �������������� cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "YourAppCookie"; // ��� cookie
        options.LoginPath = "/Authorization/Index"; // ���� � �������� �����
        options.AccessDeniedPath = "/Home/AccessDenied"; // ���� � �������� ������ � �������
    });

var app = builder.Build();

// ��������� ��������� HTTP-��������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // ��������� ������ � production
    app.UseHsts(); // ��������� HSTS (HTTP Strict Transport Security)
}

app.UseHttpsRedirection(); // ��������������� HTTP �� HTTPS
app.UseStaticFiles(); // ��������� ����������� ������

app.UseRouting(); // �������������

app.UseAuthentication(); // ��������� ��������������
app.UseAuthorization(); // ��������� �����������

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

// ��������� ��������� �� ���������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run(); // ������ ����������