using System.ComponentModel.DataAnnotations;

namespace SportsBookingSystem.Models
{
    public class FacilityType
    {
        [Key]
        public int FacilityTypeID { get; set; }

        [Required]
        public string TypeName { get; set; }

        public string Description { get; set; }
    }
}