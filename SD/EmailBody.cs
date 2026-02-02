using MileageExpenseTracker.Models;

namespace MileageExpenseTracker.SD
{
    public static class EmailBody
    {

        public static string ResetPasswordTemplate(string firstName, string lastName, string role, string link)
        {
           return $@"
    <html>
    <head>
        <style>
            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
            .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 8px 8px 0 0; }}
            .content {{ background: #f9fafb; padding: 30px; border-radius: 0 0 8px 8px; }}
            .button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 12px 30px; text-decoration: none; border-radius: 6px; margin: 20px 0; }}
            .footer {{ text-align: center; color: #6b7280; font-size: 12px; margin-top: 20px; }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <h1>Password Reset Request</h1>
            </div>
            <div class='content'>
                <h2>Hello {firstName} {lastName},</h2>
                <p>You have been invited to join the BBI Mileage Tracker as a <strong>{role}</strong>. You are receiving this request to reset your password for your Mileage Tracker account.</p>
                <p>To reset your password, please click the button below:</p>
              

                <div style='text-align: center;'>
                    <a href='{link}' class='button' style='color: white;'>Reset Password</a>
                </div>
                <p><small>This password reset link will expire in 7 days or after it is used.</small></p>
            </div>
            <div class='footer'>
                <p>&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
            </div>
        </div>
    </body>
    </html>
";
        }
       
    }
}
