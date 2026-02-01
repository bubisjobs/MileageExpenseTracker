using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MileageExpenseTracker.Data;
using MileageExpenseTracker.Models;
using MileageExpenseTracker.Services;
using System.Security.Cryptography;

namespace MileageExpenseTracker.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AdminController(ApplicationDbContext context,
    IEmailService emailService,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        // POST: Admin/CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User UserRequest)
        {
            //try
            //{
            //    // Validate email doesn't already exist
            //    if (await _context.Users.AnyAsync(u => u.Email == UserRequest.Email))
            //    {
            //        return Json(new { success = false, message = "A user with this email already exists" });
            //    }

            //    // Create new user
            //    var user = new User
            //    {
            //        Id = Guid.NewGuid(),
            //        FirstName = UserRequest.FirstName,
            //        LastName = UserRequest.LastName,
            //        Email = UserRequest.Email,
            //        Role = UserRequest.Role,
            //        IsActive = true,
            //        CreatedAt = DateTime.UtcNow,
            //        EmailVerified = false
            //    };

            //    // Generate invitation token
            //    var invitationToken = Guid.NewGuid().ToString();
            //    user.InvitationToken = invitationToken;
            //    user.InvitationTokenExpiry = DateTime.UtcNow.AddDays(7); // 7 days to accept

            //    _context.Users.Add(user);
            //    await _context.SaveChangesAsync();

            //    // Send invitation email
            //    await SendInvitationEmail(user, invitationToken);

            //    return Json(new { success = true, message = "User created and invitation sent successfully" });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { success = false, message = $"Error: {ex.Message}" });
            //}

            try
            {
                // Check if user already exists
                if (await _userManager.FindByEmailAsync(UserRequest.Email) != null)
                {
                    return Json(new { success = false, message = "A user with this email already exists" });
                }

                // Ensure role exists
                if (!await _roleManager.RoleExistsAsync(UserRequest.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRequest.Role));
                }

                // Generate a temporary password
                var tempPassword = SD.Password.TemporaryPassword;

                // Create user
                var user = new User
                {
                    UserName = UserRequest.Email,
                    Email = UserRequest.Email,
                    FirstName = UserRequest.FirstName,
                    LastName = UserRequest.LastName,
                    Role = UserRequest.Role,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    EmailVerified = false
                    
                };

                var result = await _userManager.CreateAsync(user, tempPassword);

                if (!result.Succeeded)
                {
                    return Json(new { success = false, message = string.Join("; ", result.Errors.Select(e => e.Description)) });
                }

                // Assign role
                await _userManager.AddToRoleAsync(user, UserRequest.Role);

                // Generate password reset token for invitation
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var inviteUrl = Url.Action("AcceptInvitation", "Account", new { userId = user.Id, token }, Request.Scheme);

                // Send invitation email
                var emailBody = $@"
            <html>
            <body>
                <h2>Hello {user.FirstName} {user.LastName},</h2>
                <p>You've been invited to join as a <strong>{user.Role}</strong>.</p>
                <p>
                    <a href='{inviteUrl}' style='background:#667eea;color:#fff;padding:10px 20px;border-radius:5px;text-decoration:none;'>Set Your Password</a>
                </p>
                <p>This link will expire soon. If you did not expect this, please ignore this email.</p>
            </body>
            </html>
        ";
                await _emailService.SendEmailAsync(user.Email, "Invitation to Mileage Tracker", emailBody);

                return Json(new { success = true, message = "User created and invitation sent successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // GET: Admin/GetUser/{id}
        [HttpGet("Admin/GetUser/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                return Json(new
                {
                    success = true,
                    user = new
                    {
                        id = user.Id,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        role = user.Role,
                        isActive = user.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/UpdateUser
        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User userRequest)
        {
            try
            {
                //var tt = userRequest.Id;
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == userRequest.Email);

                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                // Check if email is being changed and if it's already taken
                if (user.Email != userRequest.Email)
                {
                    if (await _context.Users.AnyAsync(u => u.Email == userRequest.Email && u.Id != userRequest.Id))
                    {
                        return Json(new { success = false, message = "Email is already taken by another user" });
                    }
                }

                user.FirstName = userRequest.FirstName;
                user.LastName = userRequest.LastName;
                user.Email = userRequest.Email;
                user.Role = userRequest.Role;
                user.IsActive = userRequest.IsActive;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // DELETE: Admin/DeleteUser/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                // Check if user has any claims
                var hasClaims = await _context.MileageClaims.AnyAsync(c => c.EmployeeName == id);
                if (hasClaims)
                {
                    return Json(new { success = false, message = "Cannot delete user with existing mileage claims. Deactivate instead." });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Send Invitation Email
        private async Task SendInvitationEmail(User user, string token)
        {
            var inviteUrl = Url.Action("AcceptInvitation", "Account", new { token }, Request.Scheme);

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
                            <h1>Welcome to Mileage Tracker</h1>
                        </div>
                        <div class='content'>
                            <h2>Hello {user.FirstName} {user.LastName},</h2>
                            <p>You've been invited to join our Mileage Tracker system as a <strong>{user.Role}</strong>.</p>
                            <p>To get started, please click the button below to set up your account and create your password:</p>
                            <div style='text-align: center;'>
                                <a href='{inviteUrl}' class='button'>Accept Invitation</a>
                            </div>
                            <p><small>This invitation link will expire in 7 days.</small></p>
                            <p>If you have any questions, please contact your administrator.</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; 2024 Mileage Tracker. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await _emailService.SendEmailAsync(
                user.Email,
                "Invitation to Mileage Tracker",
                emailBody
            );
        }
    }

  
  

    
}
 