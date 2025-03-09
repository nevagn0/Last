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

namespace Last.Controllers
{
    [Authorize] // Только авторизованные пользователи могут добавлять паспорта
    public class AddPasportController : Controller
    {
        private readonly LastContext _poContext;

        public AddPasportController(LastContext poContext)
        {
            _poContext = poContext;
        }

        // GET: AddPasport/Index
        public async Task<IActionResult> Index()
        {
            // Получаем ID текущего пользователя из аутентификации
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Authorization"); // Перенаправляем на страницу авторизации, если пользователь не авторизован
            }

            // Получаем список животных, принадлежащих текущему пользователю
            var userAnimals = _poContext.Animals
                .Where(a => a.Userid == int.Parse(userId))
                .ToList();

            // Передаем список животных в представление
            return View(userAnimals);
        }

        // POST: AddPasport/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int animalId, string seria, string number)
        {
            // Получаем ID текущего пользователя из аутентификации
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Authorization"); // Перенаправляем на страницу авторизации, если пользователь не авторизован
            }

            // Находим выбранное животное
            var animal = _poContext.Animals
                .FirstOrDefault(a => a.Id == animalId && a.Userid == int.Parse(userId));

            if (animal == null)
            {
                ModelState.AddModelError(string.Empty, "Животное не найдено.");
                return View(_poContext.Animals.Where(a => a.Userid == int.Parse(userId)).ToList());
            }

            // Проверяем, есть ли уже паспорт у этого животного
            if (animal.Passport != null)
            {
                ModelState.AddModelError(string.Empty, "У этого животного уже есть паспорт.");
                return View(_poContext.Animals.Where(a => a.Userid == int.Parse(userId)).ToList());
            }


            var pasport = new Passport
            {
                Id = animalId,
                Seria = seria,
                Number = number,
            };

            _poContext.Passports.Add(pasport);
            await _poContext.SaveChangesAsync();

            // Перенаправляем на страницу с подтверждением или другую страницу
            return RedirectToAction("Index", "MainPage");
        }

    }
}