using System;
using System.ComponentModel.DataAnnotations;

namespace SportsBookingSystem.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [Required]
        public int BookingID { get; set; }

        [Required]
        public int MemberID { get; set; }

        [Required]
        public int FacilityID { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? CommentText { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        public bool IsAnonymous { get; set; } = false;

        public virtual Booking? Booking { get; set; }
        public virtual Member? Member { get; set; }
        public virtual Facility? Facility { get; set; }
    }
}
