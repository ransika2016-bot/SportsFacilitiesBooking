using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsBookingSystem.Models
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }
        public List<int>? SelectedSportIDs { get; set; }
    }
}
