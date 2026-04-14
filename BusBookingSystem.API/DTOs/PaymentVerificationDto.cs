using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.DTOs
{
    public class PaymentVerificationDto
    {
        [Required]
        public string RazorpayOrderId { get; set; }

        [Required]
        public string RazorpayPaymentId { get; set; }

        [Required]
        public string RazorpaySignature { get; set; }

        [Required]
        public Guid BookingId { get; set; }
    }
}
