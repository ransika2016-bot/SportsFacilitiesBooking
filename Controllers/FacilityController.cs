using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Data;
using System.Collections.Generic;

namespace SportsBookingSystem.Controllers
{
    public class FacilityController : Controller
    {
        private readonly SportsDbContext _db;

        public FacilityController(SportsDbContext db)
        {
            _db = db;
        }

        public IActionResult Search(int? facilityTypeId, string location, DateTime? bookingDate, TimeSpan? startTime)
        {
            var query = _db.Facilities.Where(f => f.Status == "Active").AsQueryable();

            if (facilityTypeId.HasValue)
                query = query.Where(f => f.FacilityTypeID == facilityTypeId.Value);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(f => f.Location.Contains(location));

            if (bookingDate.HasValue && startTime.HasValue)
            {
                var searchStart = bookingDate.Value.Date.Add(startTime.Value);
                query = query.Where(f => !f.Bookings.Any(b => b.Status != "Cancelled" && b.StartTime <= searchStart && b.EndTime > searchStart));
            }

            query = query.OrderByDescending(f => f.FacilityID);

            ViewBag.FacilityTypes = new SelectList(_db.FacilityTypes, "FacilityTypeID", "TypeName");
            return View(query.ToList());
        }

        public IActionResult Details(int id, DateTime? date)
        {
            DateTime selectedDate = date ?? DateTime.Today;
            var facility = _db.Facilities
                .Include(f => f.FacilityType)
                .FirstOrDefault(f => f.FacilityID == id);

            if (facility == null)
                return NotFound();

            var bookingsForDate = _db.Bookings
                .Where(b => b.FacilityID == id && b.BookingDate == selectedDate && b.Status != "Cancelled")
                .ToList();

            var timeSlots = new List<TimeSpan>();
            for (int hour = 6; hour < 22; hour++)
            {
                timeSlots.Add(new TimeSpan(hour, 0, 0));
            }

            ViewBag.TimeSlots = timeSlots;
            ViewBag.Bookings = bookingsForDate;
            ViewBag.SelectedDate = selectedDate;

            var facilityReviews = _db.Reviews
                .Include(r => r.Member)
                .Where(r => r.FacilityID == id)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();

            ViewBag.FacilityReviews = facilityReviews;
            ViewBag.AverageRating = facilityReviews.Any()
                ? Math.Round(facilityReviews.Average(r => r.Rating), 1)
                : 0.0;
            ViewBag.ReviewCount = facilityReviews.Count;

            return View(facility);
        }
    }
}
