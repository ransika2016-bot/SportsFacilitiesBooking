using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Data;
using SportsBookingSystem.Models;

namespace SportsBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly SportsDbContext _db;

        public HomeController(SportsDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.FeaturedFacilities = _db.Facilities
                .Where(f => f.Status == "Active")
                .OrderByDescending(f => f.FacilityID)
                .Take(6)
                .ToList();
            ViewBag.LatestReviews = _db.Reviews
                .Include(r => r.Facility)
                .Include(r => r.Member)
                .OrderByDescending(r => r.ReviewDate)
                .Take(5)
                .ToList();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}