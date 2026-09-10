using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Models;

namespace SportsBookingSystem.Data
{
    public class SportsDbContext : DbContext
    {
        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<MemberSportPreference> MemberSportPreferences { get; set; }
        public DbSet<FacilityType> FacilityTypes { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberSportPreference>()
                .HasKey(m => new { m.MemberID, m.SportID });

            base.OnModelCreating(modelBuilder);
        }
    }
}