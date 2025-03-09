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

// ��������� ��������� �� ���������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); // ������ ����������