using Microsoft.EntityFrameworkCore;
using BusBookingSystem.API.Models;

namespace BusBookingSystem.API.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<BusOperator> BusOperators { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusRoute> BusRoutes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // FIX: BookingSeat → Booking
            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
            // FIX: BookingSeat → Seat
            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Seat)
                .WithMany()
                .HasForeignKey(bs => bs.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // FIX: BookingSeat → Trip
            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Trip)
                .WithMany()
                .HasForeignKey(bs => bs.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            // CRITICAL: Prevent double booking at DB level
            modelBuilder.Entity<BookingSeat>()
                .HasIndex(bs => new { bs.SeatId, bs.TripId })
                .IsUnique();
        }
    }
}
