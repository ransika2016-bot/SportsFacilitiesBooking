using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Data;
using System.Linq;

namespace SportsBookingSystem.Controllers
{
    public class AdminBookingController : Controller
    {
        private readonly SportsDbContext _db;

        public AdminBookingController(SportsDbContext db)
        {
            _db = db;
        }

        private bool IsAdmin => HttpContext.Session.GetString("Role") == "Admin";

        public IActionResult Index()
        {
            if (!IsAdmin) return RedirectToAction("Login", "Account");

            var bookings = _db.Bookings
                .Include(b => b.Member)
                .Include(b => b.Facility)
                .OrderByDescending(b => b.BookingDate)
                .ThenByDescending(b => b.StartTime)
                .ToList();
            
            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            if (!IsAdmin) return RedirectToAction("Login", "Account");

            var booking = _db.Bookings.Find(id);
            if (booking != null)
            {
                booking.Status = "Cancelled";
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Booking cancelled successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
