using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsBookingSystem.Models
{
    public class MemberSportPreference
    {
        [Key, Column(Order = 0)]
        public int MemberID { get; set; }

        [Key, Column(Order = 1)]
        public int SportID { get; set; }

        public DateTime PreferenceDate { get; set; } = DateTime.Now;

        public virtual Member Member { get; set; }
        public virtual Sport Sport { get; set; }
    }
}
