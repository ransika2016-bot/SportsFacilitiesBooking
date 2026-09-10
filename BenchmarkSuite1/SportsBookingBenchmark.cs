using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using SportsBookingSystem.Data;
using SportsBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VSDiagnostics;

namespace SportsBookingSystem.Benchmarks
{
    [SimpleJob(warmupCount: 3, iterationCount: 5)]
    [CPUUsageDiagnoser]
    public class SportsBookingBenchmark
    {
        private SportsDbContext _dbContext;
        [GlobalSetup]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SportsDbContext>().UseInMemoryDatabase(databaseName: $"SportsBookingDB_{Guid.NewGuid()}").Options;
            _dbContext = new SportsDbContext(options);
            // Seed test data
            var facilityTypes = new List<FacilityType>
            {
                new FacilityType
                {
                    FacilityTypeID = 1,
                    TypeName = "Tennis Court"
                },
                new FacilityType
                {
                    FacilityTypeID = 2,
                    TypeName = "Basketball Court"
                },
                new FacilityType
                {
                    FacilityTypeID = 3,
                    TypeName = "Swimming Pool"
                }
            };
            _dbContext.FacilityTypes.AddRange(facilityTypes);
            var facilities = new List<Facility>
            {
                new Facility
                {
                    FacilityID = 1,
                    FacilityName = "Court A",
                    FacilityTypeID = 1,
                    Location = "Downtown",
                    Status = "Active",
                    Capacity = 2
                },
                new Facility
                {
                    FacilityID = 2,
                    FacilityName = "Court B",
                    FacilityTypeID = 1,
                    Location = "Downtown",
                    Status = "Active",
                    Capacity = 2
                },
                new Facility
                {
                    FacilityID = 3,
                    FacilityName = "Pool",
                    FacilityTypeID = 3,
                    Location = "Uptown",
                    Status = "Active",
                    Capacity = 50
                }
            };
            _dbContext.Facilities.AddRange(facilities);
            var member = new Member
            {
                MemberID = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PasswordHash = "hash123",
                Phone = "555-1234",
                Address = "123 Main St",
                RegistrationDate = DateTime.Now,
                AccountStatus = "Active"
            };
            _dbContext.Members.Add(member);
            var bookings = new List<Booking>();
            for (int i = 1; i <= 20; i++)
            {
                bookings.Add(new Booking { BookingID = i, MemberID = 1, FacilityID = (i % 3) + 1, BookingDate = DateTime.Now.AddDays(i), StartTime = DateTime.Now.AddHours(9), EndTime = DateTime.Now.AddHours(10), Status = "Confirmed", CreatedDate = DateTime.Now });
            }

            _dbContext.Bookings.AddRange(bookings);
            _dbContext.SaveChanges();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _dbContext?.Dispose();
        }

        [Benchmark]
        public void SearchFacilities_Unoptimized()
        {
            // Current implementation: loads all data then filters
            var query = _dbContext.Facilities.Where(f => f.Status == "Active").AsQueryable();
            query = query.Where(f => f.FacilityTypeID == 1);
            query = query.Where(f => f.Location.Contains("Downtown"));
            var result = query.ToList(); // Executes query here
        }

        [Benchmark]
        public void GetMemberBookings_Unoptimized()
        {
            // Current implementation: loads all bookings then filters
            int memberId = 1;
            var bookings = _dbContext.Bookings.Where(b => b.MemberID == memberId).OrderByDescending(b => b.BookingDate).ToList();
        }

        [Benchmark]
        public void CheckDoubleBooking_Unoptimized()
        {
            // Current implementation: uses Any() with complex logic
            var newBooking = new Booking
            {
                FacilityID = 1,
                BookingDate = DateTime.Now.AddDays(5),
                StartTime = DateTime.Now.AddHours(9),
                EndTime = DateTime.Now.AddHours(10)
            };
            bool isBooked = _dbContext.Bookings.Any(b => b.FacilityID == newBooking.FacilityID && b.BookingDate == newBooking.BookingDate && b.Status != "Cancelled" && b.StartTime < newBooking.EndTime && b.EndTime > newBooking.StartTime);
        }

        [Benchmark]
        public void LoadFacilityDetails_Unoptimized()
        {
            // Current implementation: Find() may trigger lazy loading
            var facility = _dbContext.Facilities.Find(1);
        }
    }
}