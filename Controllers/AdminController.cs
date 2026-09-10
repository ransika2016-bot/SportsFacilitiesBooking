using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Data;
using System.Linq;

namespace SportsBookingSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly SportsDbContext _db;

        public AdminController(SportsDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");

            ViewBag.TotalMembers = _db.Members.Count();
            ViewBag.TotalFacilities = _db.Facilities.Count();
            ViewBag.TotalBookings = _db.Bookings.Count();
            ViewBag.TotalInquiries = _db.Inquiries.Count();
            
            var recentBookings = _db.Bookings
                .Include(b => b.Member)
                .Include(b => b.Facility)
                .OrderByDescending(b => b.CreatedDate)
                .Take(5)
                .ToList();

            return View(recentBookings);
        }
    }
}
