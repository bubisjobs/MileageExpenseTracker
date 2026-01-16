using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MileageExpenseTracker.Data;
using MileageExpenseTracker.Models.ViewModels;
using MileageExpenseTracker.Models;

namespace MileageExpenseTracker.Controllers
{
    public class MileageController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public MileageController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            
        }
        // Helper method to get or create current user
        private async Task<Guid> GetOrCreateCurrentUserAsync()
        {
            // TODO: Replace with actual authentication - User.Identity.Name or User.FindFirst(ClaimTypes.NameIdentifier)
            var userEmail = "employee@mileagetracker.com";

            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Demo Employee",
                    LastName = "User",
                    Email = userEmail,
                    Role = "Employee"
                };

                _applicationDbContext.Users.Add(user);
                await _applicationDbContext.SaveChangesAsync();
            }

            return user.Id;
        }

        public async Task<IActionResult> Index()
        {
            // TODO: Get current user ID from authentication
            //var currentUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var currentUserId = User.Identity?.Name;


            var claims = await _applicationDbContext.MileageClaims
                .Include(x => x.Trips)
                ////.Where(c => c.EmployeeId == currentUserId)
                //.OrderByDescending(c => c.CreatedAt)
                //.Select(c => new
                //{
                //    c.Id,
                //    c.StartDate,
                //    c.EndDate,
                //    c.Status,
                //    c.TotalKilometers,
                //    c.TotalReimbursement,
                //    c.SubmittedAt,
                //    //c.DecisionAt,
                //    TripCount = c.Trips.Count
                //})
                .ToListAsync();

            return View(claims);
        }

        // GET: Mileage/Create
        public IActionResult Create()
        {
            var model = new MileageClaim
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                RatePerKm = 0.50m,
                Status = ClaimStatus.Draft
            };

            return View(model);
        }

        // POST: Mileage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MileageClaim mileageClaim)
        {
            //if (ModelState.IsValid)
            //{
            var currentUserId = User.Identity?.Name;

            var claim = new MileageClaim
            {
                    Id = Guid.NewGuid(),
                    EmployeeName = currentUserId,
                    TeamLeadApprover = mileageClaim.TeamLeadApprover,
                    //FinanceApprover = mileageClaim.FinanceApprover,
                    Wik = mileageClaim.Wik,
                    StartDate = mileageClaim.StartDate,
                    EndDate = mileageClaim.EndDate,
                    RatePerKm = mileageClaim.RatePerKm,
                    Status = ClaimStatus.SubmittedToTeamLead,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    SubmittedAt = DateTime.UtcNow,
                    TotalReimbursement = mileageClaim.TotalReimbursement,
                
            };

             
            if (mileageClaim.Trips.Any())
            {
                if (mileageClaim.Trips.Any())
                {
                    var totalKm = 0.0m;
                    foreach (var trip in mileageClaim.Trips)
                    {
                        totalKm += trip.Kilometers; 
                        claim.Trips.Add(trip);
                    }
                    claim.TotalReimbursement = totalKm * claim.RatePerKm;
                    claim.TotalKilometers = totalKm;
                }
                
            }
            await _applicationDbContext.MileageClaims.AddAsync(claim);
            await _applicationDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
        //}

        //    return View(model);
        }

        // GET: Mileage/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var claim = await _applicationDbContext.MileageClaims
                .Include(c => c.Trips)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (claim == null)
            {
                return NotFound();
            }
            if (claim.Trips.Any())
            {
                var model = new MileageClaimViewModel
                {
                    Id = claim.Id,
                    StartDate = claim.StartDate,
                    EndDate = claim.EndDate,
                    RatePerKm = claim.RatePerKm,
                    Status = claim.Status.ToString(),
                    TotalKilometers = (decimal)claim.TotalKilometers,
                    TotalReimbursement = (decimal)claim.TotalReimbursement,
                    SubmittedAt = claim.SubmittedAt,
                    //DecisionAt = claim.DecisionAt,
                    //DecisionComment = claim.DecisionComment,
                    Trips = claim.Trips.Select(t => new MileageTripViewModel
                    {
                        Id = t.Id,
                        TripDate = t.TripDate,
                        //TripTime = t.TripTime,
                        Description = t.Description,
                        StartLocation = t.StartLocation,
                        EndLocation = t.EndLocation,
                        Kilometers = t.Kilometers,
                        Reimbursement = t.Reimbursement
                    }).ToList()
                };

                return View(model);
            }
            
            

               

            return View(claim);
        }

        // POST: Mileage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MileageClaimViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var claim = await _applicationDbContext.MileageClaims
                    .Include(c => c.Trips)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (claim == null)
                {
                    return NotFound();
                }

                // Only allow editing if status is Draft
                //if (claim.Status != "Draft")
                //{
                //    TempData["Error"] = "Cannot edit a claim that has been submitted.";
                //    return RedirectToAction(nameof(Edit), new { id });
                //}

                claim.StartDate = model.StartDate;
                claim.EndDate = model.EndDate;
                claim.RatePerKm = model.RatePerKm;
                claim.UpdatedAt = DateTime.UtcNow;

                await _applicationDbContext.SaveChangesAsync();

                TempData["Success"] = "Claim updated successfully.";
                return RedirectToAction(nameof(Edit), new { id });
            }

            return View(model);
        }

        // POST: Mileage/AddTrip
        [HttpPost]
        public async Task<IActionResult> AddTrip(Guid claimId, MileageTrip model)
        {
            var claim = await _applicationDbContext.MileageClaims.FindAsync(claimId);

            //if (claim == null || claim.Status != "Draft")
            //{
            //    return Json(new { success = false, message = "Cannot add trip to this claim." });
            //}

            var trip = new MileageTrip
            {
                Id = Guid.NewGuid(),
                ClaimId = claimId,
                TripDate = model.TripDate,
                //TripTime = model.TripTime,
                Description = model.Description,
                StartLocation = model.StartLocation,
                EndLocation = model.EndLocation,
                Kilometers = model.Kilometers,
                Reimbursement = model.Kilometers * claim.RatePerKm
            };

            _applicationDbContext.MileageTrips.Add(trip);

            // Update totals
            claim.TotalKilometers += trip.Kilometers;
            claim.TotalReimbursement += trip.Reimbursement;
            claim.UpdatedAt = DateTime.UtcNow;

            await _applicationDbContext.SaveChangesAsync();

            return Json(new { success = true, tripId = trip.Id });
        }

        // POST: Mileage/UpdateTrip
        [HttpPost]
        public async Task<IActionResult> UpdateTrip(Guid id, MileageTripViewModel model)
        {
            var trip = await _applicationDbContext.MileageTrips
                .Include(t => t.Claim)
                .FirstOrDefaultAsync(t => t.Id == id);

            //if (trip == null || trip.Claim.Status != "Draft")
            //{
            //    return Json(new { success = false, message = "Cannot update this trip." });
            //}

            var oldKilometers = trip.Kilometers;
            var oldReimbursement = trip.Reimbursement;

            trip.TripDate = model.TripDate;
            //trip.TripTime = model.TripTime;
            trip.Description = model.Description;
            trip.StartLocation = model.StartLocation;
            trip.EndLocation = model.EndLocation;
            trip.Kilometers = model.Kilometers;
            trip.Reimbursement = model.Kilometers * trip.Claim.RatePerKm;

            // Update claim totals
            trip.Claim.TotalKilometers = trip.Claim.TotalKilometers - oldKilometers + trip.Kilometers;
            trip.Claim.TotalReimbursement = trip.Claim.TotalReimbursement - oldReimbursement + trip.Reimbursement;
            trip.Claim.UpdatedAt = DateTime.UtcNow;

            await _applicationDbContext.SaveChangesAsync();

            return Json(new { success = true });
        }

        // POST: Mileage/DeleteTrip
        [HttpPost]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var trip = await _applicationDbContext.MileageTrips
                .Include(t => t.Claim)
                .FirstOrDefaultAsync(t => t.Id == id);

            //if (trip == null || trip.Claim.Status != "Draft")
            //{
            //    return Json(new { success = false, message = "Cannot delete this trip." });
            //}

            // Update claim totals
            trip.Claim.TotalKilometers -= trip.Kilometers;
            trip.Claim.TotalReimbursement -= trip.Reimbursement;
            trip.Claim.UpdatedAt = DateTime.UtcNow;

            _applicationDbContext.MileageTrips.Remove(trip);
            await _applicationDbContext.SaveChangesAsync();

            return Json(new { success = true });
        }

        // POST: Mileage/Submit
        [HttpPost]
        public async Task<IActionResult> Submit(Guid id)
        {
            var claim = await _applicationDbContext.MileageClaims
                .Include(c => c.Trips)
                .FirstOrDefaultAsync(c => c.Id == id);

            //if (claim == null || claim.Status != "Draft")
            //{
            //    return Json(new { success = false, message = "Cannot submit this claim." });
            //}

            //if (!claim.Trips.Any())
            //{
            //    return Json(new { success = false, message = "Cannot submit a claim with no trips." });
            //}

            //claim.Status = "Submitted";
            claim.SubmittedAt = DateTime.UtcNow;
            claim.UpdatedAt = DateTime.UtcNow;

            await _applicationDbContext.SaveChangesAsync();

            TempData["Success"] = "Claim submitted successfully.";
            return Json(new { success = true });
        }
    }
}

