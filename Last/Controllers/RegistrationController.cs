
using Last.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using Last.Models;

namespace Last.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly LastContext _context;

        public RegistrationController(LastContext context)
        {
            _context = context;
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
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }
    }
}