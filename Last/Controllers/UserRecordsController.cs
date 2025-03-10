using Last.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Last.Controllers
{
    public class UserRecordsController : Controller
    {
        private readonly LastContext _poContext;

        public UserRecordsController(LastContext poContext)
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

            var userRecords = await _poContext.Records
                .Include(r => r.Vetclin) 
                .Where(r => r.Userid == int.Parse(userId))
                .ToListAsync();

            return View(userRecords);
        }

        public async Task<IActionResult> Details(int id)
        {
            var record = await _poContext.Records
                .Include(r => r.Vetclin) 
                .FirstOrDefaultAsync(r => r.Id == id);

            if (record == null)
            {
                return NotFound(); 
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (record.Userid != int.Parse(userId))
            {
                return Forbid();
            }

            return View(record);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _poContext.Records
                .Include(r => r.Vetclin) 
                .FirstOrDefaultAsync(r => r.Id == id);

            if (record == null)
            {
                return NotFound(); 
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (record.Userid != int.Parse(userId))
            {
                return Forbid(); 
            }

            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await _poContext.Records.FindAsync(id);

            if (record == null)
            {
                return NotFound(); 
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (record.Userid != int.Parse(userId))
            {
                return Forbid(); 
            }

            _poContext.Records.Remove(record);
            await _poContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}