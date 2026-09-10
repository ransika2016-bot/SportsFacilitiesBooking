using Microsoft.AspNetCore.Mvc;
using SportsBookingSystem.Data;
using SportsBookingSystem.Models;

namespace SportsBookingSystem.Controllers
{
    public class AdminSportController : Controller
    {
        private readonly SportsDbContext _db;

        public AdminSportController(SportsDbContext db)
        {
            _db = db;
        }

        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var sports = _db.Sports.ToList();
            return View(sports);
        }

        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sport sport)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            
            if (ModelState.IsValid)
            {
                _db.Sports.Add(sport);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Sport added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(sport);
        }

        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var sport = _db.Sports.Find(id);
            if (sport == null) return NotFound();
            return View(sport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Sport sport)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            
            if (ModelState.IsValid)
            {
                _db.Sports.Update(sport);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Sport updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(sport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            
            var sport = _db.Sports.Find(id);
            if (sport != null)
            {
                var preferences = _db.MemberSportPreferences.Where(p => p.SportID == id).ToList();
                _db.MemberSportPreferences.RemoveRange(preferences);
                
                _db.Sports.Remove(sport);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Sport deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
