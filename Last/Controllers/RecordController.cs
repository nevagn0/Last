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
    }
}