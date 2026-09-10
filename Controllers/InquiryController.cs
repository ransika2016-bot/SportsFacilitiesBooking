using Microsoft.AspNetCore.Mvc;
using SportsBookingSystem.Data;
using SportsBookingSystem.Models;

namespace SportsBookingSystem.Controllers
{
    public class InquiryController : Controller
    {
        private readonly SportsDbContext _db;

        public InquiryController(SportsDbContext db)
        {
            _db = db;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inquiry inquiry)
        {
            if (ModelState.IsValid)
            {
                inquiry.SubmissionDate = DateTime.Now;
                inquiry.Status = "Received";
                _db.Inquiries.Add(inquiry);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Thank you! Your inquiry has been received.";
                return RedirectToAction("Index", "Home");
            }
            return View(inquiry);
        }
    }
}