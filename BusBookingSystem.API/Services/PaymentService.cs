using BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.Models;
using BusBookingSystem.API.Data;
using BusBookingSystem.API.DTOs;
using Razorpay.Api;
using Microsoft.EntityFrameworkCore;
using Payment = BusBookingSystem.API.Models.Payment;

namespace BusBookingSystem.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _keyId;
        private readonly string _keySecret;

        public PaymentService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _keyId = _configuration["Razorpay:KeyId"];
            _keySecret = _configuration["Razorpay:KeySecret"];
        }

        public async Task<string> CreateOrder(Guid bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
                throw new Exception("Booking not found.");

            RazorpayClient client = new RazorpayClient(_keyId, _keySecret);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", (int)(booking.TotalAmount * 100)); // amount in the smallest currency unit (paise for INR)
            options.Add("currency", "INR");
            options.Add("receipt", bookingId.ToString());

            Order order = client.Order.Create(options);
            string orderId = order["id"].ToString();

            // Save transaction info in our database
            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = booking.TotalAmount,
                Status = "Pending",
                PaymentMethod = "Razorpay",
                TransactionId = orderId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return orderId;
        }

        public async Task<bool> VerifyPayment(PaymentVerificationDto verification)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(_keyId, _keySecret);

                Dictionary<string, string> attributes = new Dictionary<string, string>();
                attributes.Add("razorpay_order_id", verification.RazorpayOrderId);
                attributes.Add("razorpay_payment_id", verification.RazorpayPaymentId);
                attributes.Add("razorpay_signature", verification.RazorpaySignature);

                Utils.verifyPaymentSignature(attributes);

                // Update payment and booking record in database
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.TransactionId == verification.RazorpayOrderId);

                if (payment != null)
                {
                    payment.Status = "Success";
                    payment.TransactionId = verification.RazorpayPaymentId; // Update with final payment ID
                }

                var booking = await _context.Bookings.FindAsync(verification.BookingId);
                if (booking != null)
                {
                    booking.Status = "Confirmed";
                    booking.ConfirmedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Verification failed
                return false;
            }
        }
    }
}
