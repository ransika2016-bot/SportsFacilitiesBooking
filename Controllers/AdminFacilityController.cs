using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Data;
using SportsBookingSystem.Models;
using System.Linq;

namespace SportsBookingSystem.Controllers
{
    public class AdminFacilityController : Controller
    {
        private readonly SportsDbContext _db;

        public AdminFacilityController(SportsDbContext db)
        {
            _db = db;
        }

        private bool IsAdmin => HttpContext.Session.GetString("Role") == "Admin";

        public IActionResult Index()
        {
            if (!IsAdmin) return RedirectToAction("Login", "Account");

            var facilities = _db.Facilities.Include(f => f.FacilityType).ToList();
            return View(facilities);
        }

        public IActionResult Create()
        {
            if (!IsAdmin) return RedirectToAction("Login", "Account");
            PopulateFacilityTypes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Facility facility)
        {
            if (!IsAdmin) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                _db.Facilities.Add(facility);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Facility created successfully!";
                return RedirectToAction(nameof(Index));
            }
            PopulateFacilityTypes(facility.FacilityTypeID);
            return View(facility);
        }

        public IActionResult Edit(int id)
        {
            if (!IsAdmin) return RedirectToAction("Login", "Account");

            var facility = _db.Facilities.Find(id);
            if (facility == null) return NotFound();

            PopulateFacilityTypes(facility.FacilityTypeID);
            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Facility facility)
        {
            if (!IsAdmin) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                _db.Facilities.Update(facility);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Facility updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            PopulateFacilityTypes(facility.FacilityTypeID);
            return View(facility);
        }

        private void PopulateFacilityTypes(int? selectedId = null)
        {
            ViewBag.FacilityTypes = new SelectList(_db.FacilityTypes, "FacilityTypeID", "TypeName", selectedId);
        }
    }
}
