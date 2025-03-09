using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Last.Models;

namespace Last.Controllers
{
    public class ListAnimalController : Controller
    {
        private readonly LastContext _poContext;
        public ListAnimalController(LastContext poContext)
        {
            _poContext = poContext;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userAnimals = _poContext.Animals
                .Where(a => a.Userid == int.Parse(userId))
                .ToList();


            return View(userAnimals);
        }
    }
}