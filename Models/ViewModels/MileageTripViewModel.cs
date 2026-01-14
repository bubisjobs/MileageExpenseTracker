using System.ComponentModel.DataAnnotations;

namespace MileageExpenseTracker.Models.ViewModels
{
    public class MileageTripViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TripDate { get; set; }

        public string TripTime { get; set; }

        public string Description { get; set; }

        [Required]
        public string StartLocation { get; set; }

        [Required]
        public string EndLocation { get; set; }

        [Required]
        [Range(0.1, 10000)]
        public decimal Kilometers { get; set; }

        public decimal Reimbursement { get; set; }
    }
}
