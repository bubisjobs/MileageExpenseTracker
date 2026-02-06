using Microsoft.AspNetCore.Mvc.Rendering;

namespace MileageExpenseTracker.Services
{
    public interface IMileageCreateLookupService
    {
        Task<List<SelectListItem>> GetTeamLeadsAsync();
        Task<List<SelectListItem>> GetWiksAsync();
        Task<decimal?> GetActiveRatePerKmAsync();


    }
}
