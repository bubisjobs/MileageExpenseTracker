using Microsoft.AspNetCore.Identity;

namespace MileageExpenseTracker.Models
{
    public class User : IdentityUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Role { get; set; } // Employee, Manager, Admin
    }
}
