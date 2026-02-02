using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MileageExpenseTracker.Data;
using MileageExpenseTracker.Models;
using MileageExpenseTracker.SD;
using MileageExpenseTracker.Services;
using System.Security.Cryptography;
using System.Text;

namespace MileageExpenseTracker.Controllers
{
    [Authorize(Roles = "Admin,Finance")]
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
                var tempPassword = Password.TemporaryPassword;

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

                // ENCODE the token for safe URL transmission
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                // Send invitation email with ENCODED token
                await SendInvitationEmail(user, encodedToken);


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
        [HttpDelete("Admin/DeleteUser/{userEmail}")]
        public async Task<IActionResult> DeleteUser(string userEmail)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == userEmail);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                // Check if user has any claims
                var hasClaims = await _context.MileageClaims.AnyAsync(c => c.Email == userEmail);
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

            // BEFORE (incorrect)
            var resetUrl = $"{Request.Scheme}://{Request.Host}/Identity/Account/ResetPassword" +
                $"?code={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";


            var emailBody = EmailBody.ResetPasswordTemplate(user.FirstName, user.LastName, user.Role, resetUrl);

            await _emailService.SendEmailAsync(
                user.Email,
                "Reset your Mileage Tracker password",
                emailBody
            );

        }
    }

  
  

    
}
 