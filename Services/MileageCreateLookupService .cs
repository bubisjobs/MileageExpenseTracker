using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MileageExpenseTracker.Data;

namespace MileageExpenseTracker.Services
{
    public class MileageCreateLookupService: IMileageCreateLookupService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public MileageCreateLookupService( ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;   
        }
        public async Task<List<SelectListItem>> GetTeamLeadsAsync()
        {
            return await _applicationDbContext.Users
                .AsNoTracking()
                .Where(u => u.Role == SD.Roles.TeamLead)
                .OrderBy(u => u.FirstName)
                .Select(u => new SelectListItem
                {
                    Value = u.Email,
                    Text = u.FirstName + " " + u.LastName
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetWiksAsync()
        {
            return await _applicationDbContext.Wik
                .AsNoTracking()
                .OrderBy(w => w.Name)
                .Select(w => new SelectListItem
                {
                    Value = w.Name,
                    Text = w.Name
                })
                .ToListAsync();
        }

        public async Task<decimal?> GetActiveRatePerKmAsync()
        {
            return await _applicationDbContext.MileageRates
                .AsNoTracking()
                .Where(r => r.IsActive)
                .Select(r => (decimal?)r.RatePerKm)
                .SingleOrDefaultAsync();
        }

    }
}
