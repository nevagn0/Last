using Last.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Last.Models;
using System;
using System.Security.Claims;

namespace Last.Controllers
{
    [Authorize]
    public class AddAnimalController : Controller
    {
        private readonly LastContext _poContext;

        public AddAnimalController(LastContext poContext)
        {
            _poContext = poContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Animal animal)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не авторизован.");
                    return View(animal);
                }

                animal.Userid = int.Parse(userId);

                _poContext.Animals.Add(animal);
                _poContext.SaveChanges();

                return RedirectToAction("Index", "MainPage");
            }

            return View(animal);
        }
    }
}