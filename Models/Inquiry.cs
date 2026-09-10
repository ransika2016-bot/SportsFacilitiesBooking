using System;
using System.ComponentModel.DataAnnotations;

namespace SportsBookingSystem.Models
{
    public class Inquiry
    {
        [Key]
        public int InquiryID { get; set; }

        [Required]
        public string GuestName { get; set; }

        [Required]
        [EmailAddress]
        public string GuestEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string MessageText { get; set; }

        public string? Category { get; set; }
        public string? Status { get; set; } = "Received";
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
    }
}
