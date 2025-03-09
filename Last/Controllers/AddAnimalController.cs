using Last.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Last.Models;
using System;
using System.Security.Claims;

namespace Last.Controllers
{
    [Authorize] // Только авторизованные пользователи могут добавлять животных
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
                // Получаем ID текущего пользователя из аутентификации
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не авторизован.");
                    return View(animal);
                }

                // Привязываем животное к текущему пользователю
                animal.Userid = int.Parse(userId);

                // Добавляем животное в базу данных
                _poContext.Animals.Add(animal);
                _poContext.SaveChanges();

                // Перенаправляем на главную страницу или другую страницу
                return RedirectToAction("Index", "MainPage");
            }

            // Если что-то пошло не так, возвращаем пользователя на страницу с ошибками
            return View(animal);
        }
    }
}