using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace MileageExpenseTracker.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                var fromEmail = smtpSettings["FromEmail"];
                var fromName = smtpSettings["FromName"];
                var host = smtpSettings["Host"];
                var port = int.Parse(smtpSettings["Port"]);
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];
                var enableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 30000 // 30 seconds timeout
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    Priority = MailPriority.Normal
                };

                mailMessage.To.Add(to);

                _logger.LogInformation($"Sending email to {to} with subject: {subject}");

                await client.SendMailAsync(mailMessage);

                _logger.LogInformation($"Email sent successfully to {to}");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"SMTP error sending email to {to}: {ex.Message}");
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {to}: {ex.Message}");
                throw;
            }
        }
    }
}
