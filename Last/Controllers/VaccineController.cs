using Last.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Last.Controllers
{
    public class VaccineController : Controller
    {
        private readonly LastContext _context;

        public VaccineController(LastContext context)
        {
            _context = context;
        }

        public IActionResult AddVaccineToPassport()
        {
            ViewBag.Vacins = _context.Vacins.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Type
            }).ToList();

            ViewBag.Passports = _context.Passports.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"Паспорт: {p.Seria} {p.Number}"
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVaccineToPassport(int passportId, List<int> selectedVacins)
        {
            if (ModelState.IsValid)
            {
                var passport = await _context.Passports
                    .Include(p => p.Vacins) 
                    .FirstOrDefaultAsync(p => p.Id == passportId);

                if (passport == null)
                {
                    ModelState.AddModelError(string.Empty, "Паспорт не найден.");
                    return View();
                }


                if (selectedVacins != null)
                {
                    foreach (var vacinId in selectedVacins)
                    {
                        var vacin = await _context.Vacins.FindAsync(vacinId);
                        if (vacin != null && !passport.Vacins.Contains(vacin))
                        {
                            passport.Vacins.Add(vacin);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "MainPage"); 
            }

            ViewBag.Vacins = _context.Vacins.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.Type
            }).ToList();

            ViewBag.Passports = _context.Passports.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"Паспорт: {p.Seria} {p.Number}"
            }).ToList();

            return View();
        }
    }
}