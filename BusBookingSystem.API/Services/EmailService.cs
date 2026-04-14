using BusBookingSystem.API.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BusBookingSystem.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // PLACEHOLDER: Integrate with SendGrid/MailKit here
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"[EMAIL SENT TO: {toEmail}]");
            Console.WriteLine($"[SUBJECT: {subject}]");
            Console.WriteLine($"[MESSAGE: {message}]");
            Console.WriteLine("------------------------------------------");
            await Task.CompletedTask;
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            var sid = _configuration["Twilio:AccountSid"];
            var token = _configuration["Twilio:AuthToken"];
            var from = _configuration["Twilio:FromPhoneNumber"];

            if (string.IsNullOrEmpty(sid) || sid.Contains("YOUR_"))
            {
                // Fallback for testing when keys aren't set yet
                Console.WriteLine($"[TEST SMS] To: {phoneNumber}, Message: {message}");
                return;
            }

            try
            {
                TwilioClient.Init(sid, token);

                await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(from),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TWILIO ERROR] {ex.Message}");
                // In production, you might want to log this or throw
            }
        }
    }
}
