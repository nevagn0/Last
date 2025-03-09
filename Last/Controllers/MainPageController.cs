using System;
using Microsoft.AspNetCore.Mvc;
using Last.Models;

namespace Last.Controllers
{
    public class MainPageController : Controller
    {
        private readonly LastContext _contextAccessor;
        public MainPageController(LastContext contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}