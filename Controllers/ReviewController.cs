using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Data;
using SportsBookingSystem.Models;

namespace SportsBookingSystem.Controllers
{
    public class ReviewController : Controller
    {
        private readonly SportsDbContext _db;

        public ReviewController(SportsDbContext db)
        {
            _db = db;
        }

        public IActionResult Create(int bookingId)
        {
            var memberId = HttpContext.Session.GetInt32("MemberID");
            if (memberId == null)
                return RedirectToAction("Login", "Account");

            var booking = _db.Bookings
                .Include(b => b.Facility)
                .FirstOrDefault(b => b.BookingID == bookingId);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction("MyBookings", "Booking");
            }

            if (booking.MemberID != memberId.Value)
            {
                TempData["ErrorMessage"] = "You can only review your own bookings.";
                return RedirectToAction("MyBookings", "Booking");
            }

            if (booking.EndTime >= DateTime.Now)
            {
                TempData["ErrorMessage"] = "You can only review after your booking time has passed.";
                return RedirectToAction("MyBookings", "Booking");
            }

            if (booking.Status == "Cancelled")
            {
                TempData["ErrorMessage"] = "Cancelled bookings cannot be reviewed.";
                return RedirectToAction("MyBookings", "Booking");
            }

            if (_db.Reviews.Any(r => r.BookingID == bookingId))
            {
                TempData["ErrorMessage"] = "You have already submitted a review for this booking.";
                return RedirectToAction("MyBookings", "Booking");
            }

            var review = new Review
            {
                BookingID = booking.BookingID,
                MemberID = memberId.Value,
                FacilityID = booking.FacilityID
            };

            ViewBag.FacilityName = booking.Facility?.FacilityName ?? "Facility";
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            var memberId = HttpContext.Session.GetInt32("MemberID");
            if (memberId == null)
                return RedirectToAction("Login", "Account");

            var booking = _db.Bookings
                .Include(b => b.Facility)
                .FirstOrDefault(b => b.BookingID == review.BookingID);

            if (booking == null || booking.MemberID != memberId.Value)
            {
                TempData["ErrorMessage"] = "Invalid booking.";
                return RedirectToAction("MyBookings", "Booking");
            }

            if (booking.EndTime >= DateTime.Now)
            {
                TempData["ErrorMessage"] = "You can only review after your booking time has passed.";
                return RedirectToAction("MyBookings", "Booking");
            }

            if (booking.Status == "Cancelled")
            {
                TempData["ErrorMessage"] = "Cancelled bookings cannot be reviewed.";
                return RedirectToAction("MyBookings", "Booking");
            }

            if (_db.Reviews.Any(r => r.BookingID == review.BookingID))
            {
                TempData["ErrorMessage"] = "A review already exists for this booking.";
                return RedirectToAction("MyBookings", "Booking");
            }

            review.MemberID = memberId.Value;
            review.FacilityID = booking.FacilityID;
            review.ReviewDate = DateTime.Now;

            ModelState.Remove("Booking");
            ModelState.Remove("Facility");
            ModelState.Remove("Member");

            if (ModelState.IsValid)
            {
                _db.Reviews.Add(review);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Review submitted successfully! Thank you for your feedback.";
                return RedirectToAction("Details", "Facility", new { id = review.FacilityID });
            }

            ViewBag.FacilityName = booking.Facility?.FacilityName ?? "Facility";
            return View(review);
        }

        public IActionResult Index()
        {
            var reviews = _db.Reviews
                .Include(r => r.Facility)
                .Include(r => r.Member)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();
            return View(reviews);
        }
    }
}
