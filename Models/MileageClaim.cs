using System.ComponentModel.DataAnnotations;

namespace MileageExpenseTracker.Models
{
    public class MileageClaim
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid? ApproverId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Rate per Kilometer")]
        [Range(0.01, 10.00)]
        public decimal RatePerKm { get; set; } = 0.50m;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Draft";

        public DateTime? SubmittedAt { get; set; }

        public DateTime? DecisionAt { get; set; }

        public string DecisionComment { get; set; }

        public decimal TotalKilometers { get; set; }

        public decimal TotalReimbursement { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<MileageTrip> Trips { get; set; } = new List<MileageTrip>();
        public virtual User Employee { get; set; }
        public virtual User Approver { get; set; }
    }
}
