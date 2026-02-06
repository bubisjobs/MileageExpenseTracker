using System.ComponentModel.DataAnnotations;

namespace MileageExpenseTracker.Models
{
    public class MileageRate
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Rate Per Kilometer")]
        [Range(0.01, 100.00, ErrorMessage = "Rate must be between $0.01 and $100.00")]
        [DataType(DataType.Currency)]
        public decimal RatePerKm { get; set; }

        [Display(Name = "Effective Date")]
        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }

        [Display(Name = "Updated By")]
        public string? UpdatedBy { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}

