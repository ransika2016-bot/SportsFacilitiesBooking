using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportsBookingSystem.Models
{
    public class Facility
    {
        [Key]
        public int FacilityID { get; set; }

        [Required]
        public string FacilityName { get; set; }

        [Required]
        public int FacilityTypeID { get; set; }

        [Required]
        public string Location { get; set; }

        public int? Capacity { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; } = "Active";
        public string? ImageUrl { get; set; }

        public virtual FacilityType? FacilityType { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}
