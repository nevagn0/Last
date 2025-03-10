using Last.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Last.Controllers
{
    public class RecordController : Controller
    {
        private readonly LastContext _context;

        public RecordController(LastContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            ViewBag.VetClinics = _context.Vetcins.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = $"{v.Adress} ({v.Phone})"
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Record record)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Index", "Authorization");
                }

                record.Userid = int.Parse(userId);

                _context.Records.Add(record);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "MainPage");
            }


            ViewBag.VetClinics = _context.Vetcins.Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = $"{v.Adress} ({v.Phone})"
            }).ToList();

            return View(record);
        }
        public async Task<IActionResult> ViewComments(int? vetclinId)
        {
            var vetClinics = await _context.Vetcins.ToListAsync();

            Vetcin selectedVetclinic = null;
            if (vetclinId.HasValue)
            {
                selectedVetclinic = await _context.Vetcins
                    .Include(v => v.Records)
                    .ThenInclude(r => r.User) 
                    .FirstOrDefaultAsync(v => v.Id == vetclinId.Value);
            }

            // Передаем данные в представление
            ViewBag.VetClinics = vetClinics;
            return View(selectedVetclinic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int vetclinId, string comment)
        {
            if (string.IsNullOrEmpty(comment) || comment.Length > 500)
            {
                ModelState.AddModelError(string.Empty, "Комментарий не может быть пустым или превышать 500 символов.");
                return RedirectToAction("ViewComments", new { vetclinId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Authorization"); 
            }


            var record = new Record
            {
                Com = comment,
                Vetclinid = vetclinId,
                Userid = int.Parse(userId)
            };


            _context.Records.Add(record);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewComments", new { vetclinId });
        }
    }
}