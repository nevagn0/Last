using Last.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Last.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Last.Controllers
{
    [Authorize] 
    public class AddPasportController : Controller
    {
        private readonly LastContext _poContext;

        public AddPasportController(LastContext poContext)
        {
            _poContext = poContext;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Authorization");
            }

            var userAnimals = await _poContext.Animals
                .Where(a => a.Userid == int.Parse(userId))
                .Select(a => new AnimalViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Age = a.Age,
                    Type = a.Type,
                    PassportSeria = a.Passport.Seria,
                    PassportNumber = a.Passport.Number,
                    Vaccines = a.Passport.Vacins.Select(v => v.Type).ToList()
                })
                .ToListAsync();

            return View(userAnimals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int animalId, string seria, string number)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Authorization");
            }

            var animal = _poContext.Animals
                .FirstOrDefault(a => a.Id == animalId && a.Userid == int.Parse(userId));

            if (animal == null)
            {
                ModelState.AddModelError(string.Empty, "Животное не найдено.");
                return View(await GetUserAnimals(int.Parse(userId)));
            }

            if (animal.Passport != null)
            {
                ModelState.AddModelError(string.Empty, "У этого животного уже есть паспорт.");
                return View(await GetUserAnimals(int.Parse(userId))); 
            }

            try
            {
                var passport = new Passport
                {
                    Id = animalId,
                    Seria = seria,
                    Number = number,
                };

                _poContext.Passports.Add(passport);
                await _poContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка при создании паспорта: " + ex.Message);
                return View(await GetUserAnimals(int.Parse(userId)));
            }

            return RedirectToAction("Index", "MainPage");
        }

        private async Task<List<AnimalViewModel>> GetUserAnimals(int userId)
        {
            return await _poContext.Animals
                .Where(a => a.Userid == userId)
                .Select(a => new AnimalViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Age = a.Age,
                    Type = a.Type,
                    PassportSeria = a.Passport.Seria,
                    PassportNumber = a.Passport.Number,
                    Vaccines = a.Passport.Vacins.Select(v => v.Type).ToList()
                })
                .ToListAsync();
        }

    }
}