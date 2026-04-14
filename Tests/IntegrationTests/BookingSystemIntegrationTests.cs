using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusBookingSystem.API.Data;
using BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.Models;
using BusBookingSystem.API.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace BusBookingSystem.Tests
{
    public class bookingSystemIntegrationTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private Mock<IConnectionMultiplexer> GetMockRedis()
        {
            var mockRedis = new Mock<IConnectionMultiplexer>();
            var mockDatabase = new Mock<IDatabase>();
            
            mockDatabase.Setup(d => d.StringSetAsync(
                It.IsAny<RedisKey>(), 
                It.IsAny<RedisValue>(), 
                It.IsAny<TimeSpan?>(), 
                It.IsAny<When>(), 
                It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(mockDatabase.Object);
            return mockRedis;
        }

        [Fact]
        public async Task FullSystemFlow_ShouldSucceed()
        {
            // --- SETUP ---
            var context = GetInMemoryDbContext();
            var mockRedis = GetMockRedis();
            var mockEmail = new Mock<IEmailService>();
            var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            var bookingService = new BookingService(context, mockRedis.Object);
            var userService = new UserService(context, mockConfig.Object, mockEmail.Object);

            // 1. Create Data (Trip, Bus, Seats)
            var bus = new Bus { Name = "Express 101", BusNumber = "TN-01-AB-1234", Capacity = 40, BusType = "AC Sleeper" };
            context.Buses.Add(bus);

            var trip = new Trip 
            { 
                Source = "Chennai", 
                Destination = "Madurai", 
                DepartureTime = DateTime.UtcNow.AddDays(1), 
                AvailableSeats = 40,
                BasePrice = 1000,
                Bus = bus
            };
            context.Trips.Add(trip);

            var seat1 = new Seat { SeatNumber = "S1", SeatType = "Sleeper", Bus = bus };
            var seat2 = new Seat { SeatNumber = "S2", SeatType = "Sleeper", Bus = bus };
            context.Seats.AddRange(seat1, seat2);
            await context.SaveChangesAsync();

            // 2. Register/User (Step 1)
            var user = new User { Name = "Adhi", Email = "adhi@gmail.com", Phone = "1234567890", PasswordHash = "hashed", IsVerified = true };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // --- TEST EXECUTION ---

            // 3. Create Booking (Step 2 & 8)
            var seatIds = new List<Guid> { seat1.Id, seat2.Id };
            var booking = await bookingService.CreateBooking(user.Id, trip.Id, seatIds, "unique-key-1");

            // --- VERIFICATION ---
            booking.Should().NotBeNull();
            booking.Status.Should().Be("Pending");
            booking.TotalAmount.Should().Be(2000); // 1000 * 2
            
            // Verify Seat Locks (Step 7 logic is inside CreateBooking)
            context.BookingSeats.Count(bs => bs.BookingId == booking.Id).Should().Be(2);

            // 4. Verify Trip Inventory Updated
            var updatedTrip = await context.Trips.FindAsync(trip.Id);
            updatedTrip.AvailableSeats.Should().Be(38);

            // 5. Test Idempotency (Duplicate Request)
            var duplicateBooking = await bookingService.CreateBooking(user.Id, trip.Id, seatIds, "unique-key-1");
            duplicateBooking.Id.Should().Be(booking.Id); // Should return same booking, not create a new one

            // 6. Test Cancellation (Step 13)
            await bookingService.CancelBooking(booking.Id);
            
            var cancelledBooking = await context.Bookings.FindAsync(booking.Id);
            cancelledBooking.Status.Should().Be("Cancelled");

            // 7. Verify Seats Released (Step 14)
            var tripAfterCancel = await context.Trips.FindAsync(trip.Id);
            tripAfterCancel.AvailableSeats.Should().Be(40);
        }

        [Fact]
        public async Task DoubleBooking_ShouldFail()
        {
            // --- SETUP ---
            var context = GetInMemoryDbContext();
            var mockRedis = GetMockRedis();
            var bookingService = new BookingService(context, mockRedis.Object);

            var trip = new Trip { Source = "A", Destination = "B", AvailableSeats = 10, BasePrice = 500 };
            context.Trips.Add(trip);
            var seat = new Seat { SeatNumber = "A1" };
            context.Seats.Add(seat);
            var user1 = new User { Name = "User1", Email = "u1@g.com", IsVerified = true };
            var user2 = new User { Name = "User2", Email = "u2@g.com", IsVerified = true };
            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            // 1. First user books seat
            await bookingService.CreateBooking(user1.Id, trip.Id, new List<Guid> { seat.Id });

            // 2. Second user tries to book same seat
            Func<Task> act = async () => await bookingService.CreateBooking(user2.Id, trip.Id, new List<Guid> { seat.Id });

            // 3. Should Fail
            await act.Should().ThrowAsync<Exception>().WithMessage("*already booked*");
        }
    }
}
