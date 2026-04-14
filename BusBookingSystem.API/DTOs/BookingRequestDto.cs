using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.DTOs
{
    public class BookingRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TripId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one seat must be selected.")]
        public List<Guid> SeatIds { get; set; }

        public string? IdempotencyKey { get; set; }
    }
}
