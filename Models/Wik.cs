using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MileageExpenseTracker.Models
{
    public class Wik
    {
       
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
