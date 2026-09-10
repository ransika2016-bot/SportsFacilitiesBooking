using System;
using System.ComponentModel.DataAnnotations;

namespace SportsBookingSystem.Models
{
    public class BookingCreateModel
    {
        [Required(ErrorMessage = "Please select a facility.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a facility.")]
        [Display(Name = "Facility")]
        public int FacilityID { get; set; }

        [Required(ErrorMessage = "Please select a booking date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Please select a start time.")]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; } = "09:00";

        [Required(ErrorMessage = "Please select an end time.")]
        [Display(Name = "End Time")]
        public string EndTime { get; set; } = "10:00";
    }
}
