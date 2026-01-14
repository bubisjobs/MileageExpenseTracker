using System.ComponentModel.DataAnnotations;

namespace MileageExpenseTracker.Models
{
    public class MileageTrip
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ClaimId { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime TripDate { get; set; }

        [MaxLength(20)]
        [Display(Name = "Time")]
        public string TripTime { get; set; }

        [MaxLength(300)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Starting Location")]
        public string StartLocation { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "End Location")]
        public string EndLocation { get; set; }

        [Required]
        [Range(0.1, 10000)]
        [Display(Name = "Kilometers")]
        public decimal Kilometers { get; set; }

        [Display(Name = "Reimbursement")]
        public decimal Reimbursement { get; set; }

        // Navigation property
        public virtual MileageClaim Claim { get; set; }
    }
}
