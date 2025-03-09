using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Last.Models;
using System;

namespace Last.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly LastContext _spoContext;
        public AuthorizationController(LastContext spoContext)
        {
            _spoContext = spoContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _spoContext.Users
                    .FirstOrDefault(u => u.Firstname == user.Firstname && u.Password == user.Password);

                if (existingUser != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()), // Используем existingUser.Id
                new Claim(ClaimTypes.Name, existingUser.Firstname) // Используем existingUser.Firstname
            };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("CookieAuth", principal);
                    return RedirectToAction("Index", "MainPage");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль.");
                }
            }
            return View(user);
        }
    }
}