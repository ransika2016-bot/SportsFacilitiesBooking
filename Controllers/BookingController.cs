using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Data;
using SportsBookingSystem.Models;

namespace SportsBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly SportsDbContext _db;

        public BookingController(SportsDbContext db)
        {
            _db = db;
        }

        public IActionResult Create(int? facilityId)
        {
            if (HttpContext.Session.GetInt32("MemberID") == null)
                return RedirectToAction("Login", "Account");

            var model = new BookingCreateModel();
            if (facilityId.HasValue)
                model.FacilityID = facilityId.Value;

            PopulateFacilities(model.FacilityID);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookingCreateModel model)
        {
            if (HttpContext.Session.GetInt32("MemberID") == null)
                return RedirectToAction("Login", "Account");

            if (!TryParseTime(model.StartTime, out var startTime))
                ModelState.AddModelError(nameof(model.StartTime), "Invalid start time.");

            if (!TryParseTime(model.EndTime, out var endTime))
                ModelState.AddModelError(nameof(model.EndTime), "Invalid end time.");

            if (TryParseTime(model.StartTime, out startTime) &&
                TryParseTime(model.EndTime, out endTime) &&
                endTime <= startTime)
            {
                ModelState.AddModelError(nameof(model.EndTime), "End time must be after start time.");
            }

            if (model.BookingDate.Date < DateTime.Today)
                ModelState.AddModelError(nameof(model.BookingDate), "Booking date cannot be in the past.");

            if (!ModelState.IsValid)
            {
                PopulateFacilities(model.FacilityID);
                return View(model);
            }

            TryParseTime(model.StartTime, out startTime);
            TryParseTime(model.EndTime, out endTime);
            var bookingDate = model.BookingDate.Date;
            var startDateTime = bookingDate.Add(startTime);
            var endDateTime = bookingDate.Add(endTime);

            bool isBooked = _db.Bookings.Any(b =>
                b.FacilityID == model.FacilityID
                && b.BookingDate.Date == bookingDate
                && b.Status != "Cancelled"
                && b.StartTime < endDateTime
                && b.EndTime > startDateTime);

            if (isBooked)
            {
                ModelState.AddModelError("", "This facility is already booked for the selected time slot.");
                PopulateFacilities(model.FacilityID);
                return View(model);
            }

            try
            {
                var booking = new Booking
                {
                    MemberID = HttpContext.Session.GetInt32("MemberID")!.Value,
                    FacilityID = model.FacilityID,
                    BookingDate = bookingDate,
                    StartTime = startDateTime,
                    EndTime = endDateTime,
                    Status = "Confirmed",
                    CreatedDate = DateTime.Now,
                    Notes = string.Empty
                };

                _db.Bookings.Add(booking);
                _db.SaveChanges();

                TempData["SuccessMessage"] = $"Booking #{booking.BookingID} confirmed successfully!";
                return RedirectToAction(nameof(MyBookings));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Could not save booking. Error: " + ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : ""));
                PopulateFacilities(model.FacilityID);
                return View(model);
            }
        }

        public IActionResult MyBookings()
        {
            var memberId = HttpContext.Session.GetInt32("MemberID");
            if (memberId == null)
                return RedirectToAction("Login", "Account");

            var now = DateTime.Now;

            var allBookings = _db.Bookings
                .Include(b => b.Facility)
                .Where(b => b.MemberID == memberId.Value)
                .ToList();

            var upcoming = allBookings
                .Where(b => b.EndTime >= now && b.Status != "Cancelled")
                .OrderBy(b => b.StartTime)
                .ToList();

            var past = allBookings
                .Where(b => b.EndTime < now || b.Status == "Cancelled")
                .OrderByDescending(b => b.StartTime)
                .ToList();

            ViewBag.MemberName = HttpContext.Session.GetString("MemberName");
            ViewBag.Upcoming = upcoming;
            ViewBag.Past = past;

            var reviewedBookingIds = _db.Reviews
                .Where(r => r.MemberID == memberId.Value)
                .Select(r => r.BookingID)
                .ToHashSet();
            ViewBag.ReviewedBookingIds = reviewedBookingIds;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            if (HttpContext.Session.GetInt32("MemberID") == null)
                return RedirectToAction("Login", "Account");

            var booking = _db.Bookings.Find(id);
            if (booking != null && booking.MemberID == HttpContext.Session.GetInt32("MemberID"))
            {
                booking.Status = "Cancelled";
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Booking cancelled successfully.";
            }
            return RedirectToAction("MyBookings");
        }

        [HttpGet]
        public IActionResult GetFacilityBookings(int facilityId)
        {
            var bookings = _db.Bookings
                .Where(b => b.FacilityID == facilityId && b.Status != "Cancelled")
                .Select(b => new
                {
                    title = "Booked",
                    start = b.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = b.EndTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    color = "#e74c3c",
                    display = "background"
                })
                .ToList();

            return Json(bookings);
        }

        private void PopulateFacilities(int? selectedId = null)
        {
            ViewBag.Facilities = new SelectList(
                _db.Facilities.Where(f => f.Status == "Active"),
                "FacilityID", "FacilityName", selectedId);
        }

        private static bool TryParseTime(string? value, out TimeSpan time)
        {
            time = default;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim();
            if (TimeSpan.TryParse(value, out time))
                return true;

            if (value.Length == 5 && TimeSpan.TryParse(value + ":00", out time))
                return true;

            return false;
        }
    }
}
