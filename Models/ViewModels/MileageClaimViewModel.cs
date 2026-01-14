using System.ComponentModel.DataAnnotations;

namespace MileageExpenseTracker.Models.ViewModels
{
    public class MileageClaimViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Rate per KM")]
        [Range(0.01, 10.00)]
        public decimal RatePerKm { get; set; } = 0.50m;

        public string Status { get; set; }

        public List<MileageTripViewModel> Trips { get; set; } = new List<MileageTripViewModel>();

        public decimal TotalKilometers { get; set; }

        public decimal TotalReimbursement { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public DateTime? DecisionAt { get; set; }

        public string DecisionComment { get; set; }
    }
}
