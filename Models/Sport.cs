using System.ComponentModel.DataAnnotations;

namespace SportsBookingSystem.Models
{
    public class Sport
    {
        [Key]
        public int SportID { get; set; }

        [Required]
        public string SportName { get; set; }

        public string Description { get; set; }
    }
}