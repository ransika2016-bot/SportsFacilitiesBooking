using Microsoft.AspNetCore.Mvc;
using SportsBookingSystem.Data;
using SportsBookingSystem.Models;
using System.Security.Cryptography;
using System.Text;

namespace SportsBookingSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly SportsDbContext _db;

        public AccountController(SportsDbContext db)
        {
            _db = db;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = HashPassword(model.Password);
                var member = _db.Members
                    .FirstOrDefault(m => m.Email.ToLower() == model.Email.ToLower() && m.PasswordHash == hashedPassword);

                if (member != null && member.AccountStatus == "Active")
                {
                    HttpContext.Session.SetInt32("MemberID", member.MemberID);
                    HttpContext.Session.SetString("MemberName", member.FirstName + " " + member.LastName);
                    HttpContext.Session.SetString("Role", member.Role);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public IActionResult Register()
        {
            ViewBag.Sports = _db.Sports.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Sports = _db.Sports.ToList();
                return View(model);
            }

            if (_db.Members.Any(m => m.Email.ToLower() == model.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "This email is already registered. Please log in instead.");
                ViewBag.Sports = _db.Sports.ToList();
                return View(model);
            }

            var member = new Member
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Phone = model.Phone,
                Address = model.Address
            };

            _db.Members.Add(member);
            _db.SaveChanges();

            if (model.SelectedSportIDs != null)
            {
                foreach (var sportId in model.SelectedSportIDs)
                {
                    _db.MemberSportPreferences.Add(new MemberSportPreference
                    {
                        MemberID = member.MemberID,
                        SportID = sportId
                    });
                }
                _db.SaveChanges();
            }

            HttpContext.Session.SetInt32("MemberID", member.MemberID);
            HttpContext.Session.SetString("MemberName", member.FirstName + " " + member.LastName);
            HttpContext.Session.SetString("Role", member.Role);

            TempData["SuccessMessage"] = "Registration successful! You are now logged in.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult GuestRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuestRegister(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Name and email are required.");
                return View();
            }

            if (_db.Members.Any(m => m.Email.ToLower() == email.ToLower()))
            {
                ModelState.AddModelError("", "This email is already registered. Please log in instead.");
                return View();
            }

            var parts = name.Trim().Split(' ', 2);
            var firstName = parts[0];
            var lastName = parts.Length > 1 ? parts[1] : "";

            var member = new Member
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PasswordHash = HashPassword("default123"),
                RegistrationDate = DateTime.Now,
                AccountStatus = "Active"
            };

            _db.Members.Add(member);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Welcome! You're now a member. Please log in with your email and temporary password: default123. You can update your profile and password after logging in.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));
                return builder.ToString();
            }
        }
    }
}