using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MileageExpenseTracker.Models.ViewModels
{
    public class MileageCreateVM
    {
        [ValidateNever]
        public List<SelectListItem> TeamLeads { get; set; }

        [ValidateNever]
        public List<SelectListItem> Wiks { get; set; }

        public MileageClaim mileageClaim { get; set; }
    }
}
