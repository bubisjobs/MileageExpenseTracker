using Microsoft.AspNetCore.Identity.UI.Services;

namespace MileageExpenseTracker.Services
{
    public class NoOpEmailSender :IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Dev stub: do nothing (or log)
            return Task.CompletedTask;
        }
    }
}
