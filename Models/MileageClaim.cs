using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MileageExpenseTracker.Models
{
    public class MileageClaim
    {  // Claim PK (OK to be Guid)
        public Guid Id { get; set; }

        // ========= Ownership / Routing =========

        // Claim owner (logged-in employee)
        [Required]
        public string? EmployeeName { get; set; } = default!;

        // Team lead selected from dropdown (first approver)
        [Required]
        public string? TeamLeadApprover { get; set; } = default!;
        public virtual User? Approver { get; set; }
        public string? FinanceApprover { get; set; } = default!;


        // House or wik
        [Required]
        public string? Wik { get; set; } = default!;

        // ========= Dates =========

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // ========= Rates / Totals =========

        [Required]
        [Display(Name = "Rate per Kilometer")]
        [Range(0.01, 10.00)]
        public decimal RatePerKm { get; set; } = 0.50m;

        public decimal? TotalKilometers { get; set; }
        public decimal? TotalReimbursement { get; set; }

        // ========= Workflow =========
        // Replace your string Status with an enum-driven workflow (strongly recommended)
        [Required]
        public ClaimStatus Status { get; set; } = ClaimStatus.Draft;

        public DateTime? SubmittedAt { get; set; }

        // Team lead review
        public DateTime? TeamLeadReviewedAt { get; set; }
        public string? TeamLeadComment { get; set; }

        // Finance review
        public DateTime? FinanceReviewedAt { get; set; }
        public string? FinanceComment { get; set; }

        // ========= Timestamps =========

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ========= Navigation =========

        public virtual ICollection<MileageTrip> Trips { get; set; } = new List<MileageTrip>();
    }
}
