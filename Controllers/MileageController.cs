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
            if (ModelState.IsValid)
            {
                if (mileageClaim.Trips.Any())
                {

                    var currentUserId = User.Identity?.Name;
                    var EmployfullName = "";
                    if (currentUserId is not null)
                        if (currentUserId is not null)
                        {
                            User? user = await _applicationDbContext.Users.SingleOrDefaultAsync(x => x.Email == currentUserId);
                            EmployfullName = user?.FirstName + " " + user?.LastName;
                        }


                    var claim = new MileageClaim
                    {
                        Id = Guid.NewGuid(),
                        EmployeeName = EmployfullName,
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
                    var totalKm = 0.0m;
                    foreach (var trip in mileageClaim.Trips)
                    {
                        totalKm += trip.Kilometers;
                        //add reimbursement for each trip
                        trip.Reimbursement = trip.Kilometers * claim.RatePerKm;
                        claim.Trips.Add(trip);


                    }
                    claim.TotalReimbursement = totalKm * claim.RatePerKm;
                    claim.TotalKilometers = totalKm;

                    await _applicationDbContext.MileageClaims.AddAsync(claim);
                    await _applicationDbContext.SaveChangesAsync();
                    TempData["Success"] = "Claim has been sucesfully added and sent to Team Lead.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Claim has no Trips.";
                return RedirectToAction(nameof(Index));
            }

            return View();
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

            //if (ModelState.IsValid)
            //{
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
            //}

            //return View(model);
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

            if (trip == null)
            {
                return Json(new { success = false, message = "Cannot delete this trip." });
            }

            if (trip.Claim.Trips.Count == 1)
            {
                return Json(new { success = false, message = "Cannot delete this trip. You need atleast 1 trip" });
            }
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

            if (claim == null)
            {
                return Json(new { success = false, message = "Cannot submit this claim." });
            }

            if (!claim.Trips.Any())
            {
                return Json(new { success = false, message = "Cannot submit a claim with no trips." });
            }

            //claim.Status = "Submitted";
            claim.SubmittedAt = DateTime.UtcNow;
            claim.UpdatedAt = DateTime.UtcNow;

            await _applicationDbContext.SaveChangesAsync();

            TempData["Success"] = "Claim submitted successfully.";
            return Json(new { success = true });
        }


        //public async Task<IActionResult> ApproveByTeamLead(Guid id, MileageClaim mileageClaim)
        //{
        //    try
        //    {
        //        var claim = await _applicationDbContext.MileageClaims
        //        //.Include(c => c.Trips)
        //        .FirstOrDefaultAsync(c => c.Id == id);
        //        if (claim != null)
        //        {
        //            claim.Status = ClaimStatus.TeamLeadApproved;
        //            claim.TeamLeadComment = mileageClaim.TeamLeadComment;
        //            mileageClaim.TeamLeadReviewedAt = DateTime.Now;

        //            claim.SubmittedAt = DateTime.UtcNow;
        //            claim.UpdatedAt = DateTime.UtcNow;

        //            await _applicationDbContext.SaveChangesAsync();

        //            TempData["Success"] = "Claim Approved by Team Lead.";
        //            return Json(new { success = true });
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return Json(new { success = false, message = "Something went wrong." });

        //    }
        //    return Json(new { success = false, message = "Something went wrong." });
        //}

        //public async Task<IActionResult> ApproveByFinance(Guid id, MileageClaim mileageClaim)
        //{
        //    try
        //    {
        //        var claim = await _applicationDbContext.MileageClaims
        //        //.Include(c => c.Trips)
        //        .FirstOrDefaultAsync(c => c.Id == id);
        //        if (claim != null)
        //        {
        //            claim.Status = ClaimStatus.FinanceApproved;
        //            claim.FinanceComment = mileageClaim.FinanceComment;
        //            mileageClaim.FinanceReviewedAt = DateTime.Now;

        //            claim.SubmittedAt = DateTime.UtcNow;
        //            claim.UpdatedAt = DateTime.UtcNow;

        //            await _applicationDbContext.SaveChangesAsync();

        //            TempData["Success"] = "Claim Approved by Team Lead.";
        //            return Json(new { success = true });
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return Json(new { success = false, message = "Something went wrong." });

        //    }
        //    return Json(new { success = false, message = "Something went wrong." });
        //}

        //public async Task<IActionResult> RejectedByFinance(Guid id, MileageClaim mileageClaim)
        //{
        //    try
        //    {
        //        var claim = await _applicationDbContext.MileageClaims
        //        //.Include(c => c.Trips)
        //        .FirstOrDefaultAsync(c => c.Id == id);
        //        if (claim != null)
        //        {
        //            claim.Status = ClaimStatus.FinanceRejected;
        //            claim.FinanceComment = mileageClaim.FinanceComment;
        //            mileageClaim.FinanceReviewedAt = DateTime.Now;

        //            claim.SubmittedAt = DateTime.UtcNow;
        //            claim.UpdatedAt = DateTime.UtcNow;

        //            await _applicationDbContext.SaveChangesAsync();

        //            TempData["Success"] = "Claim Approved by Team Lead.";
        //            return Json(new { success = true });
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return Json(new { success = false, message = "Something went wrong." });

        //    }
        //    return Json(new { success = false, message = "Something went wrong." });
        //}

        //public async Task<IActionResult> RejectedByTeamLead(Guid id, MileageClaim mileageClaim)
        //{
        //    try
        //    {
        //        var claim = await _applicationDbContext.MileageClaims
        //        //.Include(c => c.Trips)
        //        .FirstOrDefaultAsync(c => c.Id == id);
        //        if (claim != null)
        //        {
        //            claim.Status = ClaimStatus.TeamLeadRejected;
        //            claim.TeamLeadComment = mileageClaim.FinanceComment;
        //            mileageClaim.TeamLeadReviewedAt = DateTime.Now;

        //            claim.SubmittedAt = DateTime.UtcNow;
        //            claim.UpdatedAt = DateTime.UtcNow;

        //            await _applicationDbContext.SaveChangesAsync();

        //            TempData["Success"] = "Claim Approved by Team Lead.";
        //            return Json(new { success = true });
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return Json(new { success = false, message = "Something went wrong." });

        //    }
        //    return Json(new { success = false, message = "Something went wrong." });
        //}


        
        [HttpPost]
        [Route("Mileage/Approve/{id}")]
        public async Task<IActionResult> Approve(Guid id, [FromBody] string comment)
        {
            try
            {
                var claim = await _applicationDbContext.MileageClaims.Include(x => x.Trips).SingleOrDefaultAsync(x => x.Id == id);

                if (claim == null)
                {
                    return Json(new { success = false, message = "Claim not found" });
                }

                if (claim.Trips.Count < 1)
                {
                    return Json(new { success = false, message = "Claim has no Mileage" });

                }
                //// Update claim status based on current workflow stage
                //if (claim.Status == ClaimStatus.SubmittedToTeamLead)
                //{
                //    claim.Status = ClaimStatus.TeamLeadApproved;
                //    claim.TeamLeadReviewedAt = DateTime.UtcNow;
                //    claim.TeamLeadComment = comment;
                //}
                //else if (claim.Status == ClaimStatus.SubmittedToFinance)
                //{
                claim.Status = ClaimStatus.Approved;
                claim.FinanceReviewedAt = DateTime.UtcNow;
                claim.FinanceComment = comment;
                // Set FinanceComment if needed
                //}

                claim.UpdatedAt = DateTime.UtcNow;
                await _applicationDbContext.SaveChangesAsync();

                return Json(new { success = true, message = "Claim approved successfully" });
            }
            catch (Exception)
            {


                return Json(new { success = false, message = "Something went wrong." });
            }
            
        }

        [HttpPost]
        [Route("Mileage/Reject/{id}")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] string comment)
        {
            try
            {
                var claim = await _applicationDbContext.MileageClaims.FindAsync(id);

                if (claim == null)
                {
                    return Json(new { success = false, message = "Claim not found" });
                }
               
                if (claim.Trips.Count < 1)
                {
                    return Json(new { success = false, message = "Claim has no Mileage" });

                }

                // Update claim status based on current workflow stage
                //if (claim.Status == ClaimStatus.SubmittedToTeamLead)
                //{
                //    claim.Status = ClaimStatus.TeamLeadRejected;
                //    claim.TeamLeadReviewedAt = DateTime.UtcNow;
                //    claim.TeamLeadComment = comment;
                //}
                //else
                //{
                claim.Status = ClaimStatus.Rejected;
                claim.FinanceReviewedAt = DateTime.UtcNow;
                claim.FinanceComment = comment;




                claim.UpdatedAt = DateTime.UtcNow;
                await _applicationDbContext.SaveChangesAsync();

                return Json(new { success = true, message = "Claim rejected successfully" });
            }
            catch (Exception)
            {

                return Json(new { success = false, message = "Something went wrong" });
            }
            
        }
    }
}

