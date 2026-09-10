using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsBookingSystem.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public string AccountStatus { get; set; } = "Active";
        public string Role { get; set; } = "User";

        public virtual ICollection<MemberSportPreference>? SportPreferences { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}