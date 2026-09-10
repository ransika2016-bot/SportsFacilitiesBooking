using System;
using System.ComponentModel.DataAnnotations;

namespace SportsBookingSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        public int MemberID { get; set; }

        [Required]
        public int FacilityID { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string? Status { get; set; } = "Confirmed";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Notes { get; set; }

        public virtual Member? Member { get; set; }
        public virtual Facility? Facility { get; set; }
    }
}