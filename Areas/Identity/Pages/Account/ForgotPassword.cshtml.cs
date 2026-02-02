using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MileageExpenseTracker.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System;
using MileageExpenseTracker.Services;
using Microsoft.EntityFrameworkCore;

public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailSender;

    public ForgotPasswordModel(UserManager<User> userManager, IEmailService emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public bool EmailSent { get; set; } = false; // Add this property

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == Input.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                // But still show success message for security
                EmailSent = true;
                return Page();
            }

            // Generate and encode the reset token
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code, email = user.Email },
                protocol: Request.Scheme);

            var emailBody = $@"
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
                            <h2>Hello {user.FirstName} {user.LastName},</h2>
                            <p>We received a request to reset your password for your BBI Mileage Tracker account.</p>
                            <p>To reset your password, please click the button below:</p>
                          
                            <div style='text-align: center;'>
                                <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' class='button' style='color: white;'>Reset Password</a>
                            </div>
                            <p><small>This password reset link will expire in 24 hours.</small></p>
                            <p><small>If you did not request a password reset, please ignore this email or contact support if you have concerns.</small></p>
                        </div>
                        <div class='footer'>
                            <p>&copy; {DateTime.UtcNow.Year} BBI Mileage Tracker. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await _emailSender.SendEmailAsync(
                Input.Email,
                "Reset Password - BBI Mileage Tracker",
                emailBody);

            EmailSent = true;
            return Page();
        }

        return Page();
    }
}