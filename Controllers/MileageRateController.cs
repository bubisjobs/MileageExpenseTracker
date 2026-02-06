using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MileageExpenseTracker.Data;
using MileageExpenseTracker.Models;

namespace MileageExpenseTracker.Controllers
{
    public class MileageRateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MileageRateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MileageRate
        public async Task<IActionResult> Index()
        {
            var rates = await _context.MileageRates
                .OrderByDescending(r => r.EffectiveDate)
                .ToListAsync();
            return View(rates);
        }

        // GET: MileageRate/Edit
        public async Task<IActionResult> Edit()
        {
            var currentRate = await _context.MileageRates
                .Where(r => r.IsActive)
                .FirstOrDefaultAsync();

            if (currentRate == null)
            {
                currentRate = new MileageRate
                {
                    RatePerKm = 0.00m,
                    EffectiveDate = DateTime.Now,
                    IsActive = true
                };
            }

            return View(currentRate);
        }

        // POST: MileageRate/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MileageRate model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Deactivate all existing rates
                    var existingRates = await _context.MileageRates.ToListAsync();
                    foreach (var rate in existingRates)
                    {
                        rate.IsActive = false;
                    }

                    // Create new rate or update existing
                    if (model.Id == 0)
                    {
                        model.IsActive = true;
                        model.LastUpdated = DateTime.Now;
                        model.UpdatedBy = User.Identity?.Name;
                        _context.Add(model);
                    }
                    else
                    {
                        var existingRate = await _context.MileageRates.FindAsync(model.Id);
                        if (existingRate != null)
                        {
                            existingRate.RatePerKm = model.RatePerKm;
                            existingRate.EffectiveDate = model.EffectiveDate;
                            existingRate.LastUpdated = DateTime.Now;
                            existingRate.UpdatedBy = User.Identity?.Name;
                            existingRate.IsActive = true;
                            _context.Update(existingRate);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Mileage rate has been updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating the rate.";
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
