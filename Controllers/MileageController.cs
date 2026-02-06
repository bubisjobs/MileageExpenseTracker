using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MileageExpenseTracker.Data;
using MileageExpenseTracker.Models.ViewModels;
using MileageExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MileageExpenseTracker.Services;
using MileageExpenseTracker.SD;
using NuGet.Common;
using System.Security.Claims;

namespace MileageExpenseTracker.Controllers
{
    [Authorize]
    public class MileageController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMileageCreateLookupService _mileageCreateLookupService;
        private readonly IEmailService _emailService;

        public MileageController(ApplicationDbContext applicationDbContext, IMileageCreateLookupService mileageCreateLookupService, IEmailService emailService)
        {
            _applicationDbContext = applicationDbContext;
            _mileageCreateLookupService = mileageCreateLookupService;
            _emailService = emailService;
        }
        // Helper method to get or create current user

        public async Task<IActionResult> Index()
        {
            var currentUserId = User.Identity?.Name;
            var claims = new List<MileageClaim>();

            if (User.IsInRole(SD.Roles.Finance))
            {

                ///show only their own claim and any claim approved by team lead
                claims = await _applicationDbContext.MileageClaims
                    .Include(x => x.Trips)
                    //.Where(c => c.Status == ClaimStatus.TeamLeadApproved ||)
                    .ToListAsync();
            }
            else
            {
                claims = await _applicationDbContext.MileageClaims
                    .Include(x => x.Trips)
                    .Where(c => c.Email == currentUserId || c.TeamLeadApprover == currentUserId)
                    .ToListAsync();
            }

            return View(claims);
        }

        // GET: Mileage/Create
        public async Task<IActionResult> Create()
        {

            var ratePerKm = await _mileageCreateLookupService.GetActiveRatePerKmAsync();
            var vm = new MileageCreateVM
            {
                TeamLeads = await _mileageCreateLookupService.GetTeamLeadsAsync(),
                Wiks = await _mileageCreateLookupService.GetWiksAsync(),
                mileageClaim = new MileageClaim
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today,
                    RatePerKm = ratePerKm.Value,
                    Status = ClaimStatus.Draft
                }
            };

            return View(vm);
        }

        // POST: Mileage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MileageCreateVM mileageCreateVM)
        {

            //mileageCreateVM.TeamLeads = await _mileageCreateLookupService.GetTeamLeadsAsync();
            //mileageCreateVM.Wiks = await _mileageCreateLookupService.GetWiksAsync();
            if (ModelState.IsValid)
            {

                if (mileageCreateVM.mileageClaim.StartDate > mileageCreateVM.mileageClaim.EndDate)
                {
                    TempData["Error"] = "Start Date can't be greater than End date";
                    return View();
                }
                if (mileageCreateVM.mileageClaim.Trips.Any())
                {

                    var currentUserId = User.Identity?.Name;
                    var EmployfullName = "";
                    if (currentUserId is not null)
                        if (currentUserId is not null)
                        {
                            User? user = await _applicationDbContext.Users.SingleOrDefaultAsync(x => x.Email == currentUserId);
                            EmployfullName = user?.FirstName + " " + user?.LastName;
                        }

                    //var currentMileageRate = await _applicationDbContext.MileageRates
                                                                         //.SingleOrDefaultAsync(r => r.IsActive);
                                                                         

                    var claim = new MileageClaim
                    {
                        Id = Guid.NewGuid(),
                        EmployeeName = EmployfullName,
                        Email = currentUserId,
                        TeamLeadApprover = mileageCreateVM.mileageClaim.TeamLeadApprover,
                        //FinanceApprover = mileageClaim.FinanceApprover,
                        Wik = mileageCreateVM.mileageClaim.Wik,
                        StartDate = mileageCreateVM.mileageClaim.StartDate,
                        EndDate = mileageCreateVM.mileageClaim.EndDate,
                        RatePerKm = mileageCreateVM.mileageClaim.RatePerKm,

                        Status = ClaimStatus.SubmittedToTeamLead,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        SubmittedAt = DateTime.UtcNow,
                        TotalReimbursement = mileageCreateVM.mileageClaim.TotalReimbursement,

                    };
                    var totalKm = 0.0m;
                    foreach (var trip in mileageCreateVM.mileageClaim.Trips)
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

                    var link = $"{Request.Scheme}://{Request.Host}/Mileage/Edit/{claim.Id}";


                    var emailbody = EmailBody.emailforReview(EmployfullName, link);
                    _emailService.SendEmailAsync(mileageCreateVM.mileageClaim.TeamLeadApprover, "A new Mileage Claim requires your attention", emailbody);
                    TempData["Success"] = "Claim has been sucesfully added and sent to Team Lead.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Claim has no Trips.";
                return RedirectToAction(nameof(Index));
            }

         
            var vm = new MileageCreateVM
            {
                TeamLeads = await _mileageCreateLookupService.GetTeamLeadsAsync(),
                Wiks = await _mileageCreateLookupService.GetWiksAsync(),
                mileageClaim = new MileageClaim
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today,
                    RatePerKm = (decimal)await _mileageCreateLookupService.GetActiveRatePerKmAsync(),
                    Status = ClaimStatus.Draft
                }
            };
            return View(vm);
        }

        // GET: Mileage/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var claim = await _applicationDbContext.MileageClaims
                .Include(c => c.Trips)
                .FirstOrDefaultAsync(c => c.Id == id);
               //claim?.RatePerKm = 0.50m;

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
            var currentMileageRate = await _applicationDbContext.MileageRates
                                                                  .SingleOrDefaultAsync(r => r.IsActive);

            claim.StartDate = model.StartDate;
                claim.EndDate = model.EndDate;
                claim.RatePerKm = currentMileageRate.RatePerKm;
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

            if (claim == null )
            {
                return Json(new { success = false, message = "Cannot add trip to this claim." });
            }
            var currentMileageRate = await _applicationDbContext.MileageRates
                                                                      .SingleOrDefaultAsync(r => r.IsActive);
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
                Reimbursement = model.Kilometers * currentMileageRate.RatePerKm
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

                if (!claim.Trips.Any())
                {
                    return Json(new { success = false, message = "Claim has no Mileage" });

                }
                // to check if it's not yet approved by team lead 
                if(claim.Status != ClaimStatus.TeamLeadApproved)
                {
                    claim.Status = ClaimStatus.TeamLeadApproved;
                    claim.TeamLeadComment = comment;
                    claim.TeamLeadReviewedAt = DateTime.UtcNow;

                    var financeUsers = await _applicationDbContext.Users.AsNoTracking().Where(x => x.Role == SD.Roles.Finance).ToListAsync();
                    var teamLead = await _applicationDbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Email == claim.TeamLeadApprover);
                   
                    foreach(var financeUser in financeUsers)
                    {
                        var link = $"{Request.Scheme}://{Request.Host}/Mileage/Edit/{claim.Id}";
                        var emailbody = EmailBody.TeamLeadApproved($"{teamLead.FirstName} {teamLead.LastName}", link);
                        _emailService.SendEmailAsync(financeUser.Email, "A new Mileage Claim requires your attention", emailbody);

                    }

                }
                else
                {


                    claim.Status = ClaimStatus.FinanceApproved;
                    claim.FinanceReviewedAt = DateTime.UtcNow;
                    claim.FinanceComment = comment;
                    var link = $"{Request.Scheme}://{Request.Host}/Mileage/Edit/{claim.Id}";
                    var emailbody = EmailBody.ClaimFullyApproved($"{claim.EmployeeName}", claim.TotalReimbursement.Value, link, claim.FinanceComment);
                    _emailService.SendEmailAsync(claim.Email, "Mileage Claim Approved", emailbody);
                }
              

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
               
                //if (claim.Trips.Count < 1)
                //{
                //    return Json(new { success = false, message = "Claim has no Mileage" });

                //}

                if(claim.Status != ClaimStatus.TeamLeadRejected)
                {
                    claim.Status = ClaimStatus.TeamLeadRejected;
                    claim.TeamLeadReviewedAt = DateTime.UtcNow;
                    claim.TeamLeadComment = comment;
                    var link = $"{Request.Scheme}://{Request.Host}/Mileage/Edit/{claim.Id}";
                    var emailbody = EmailBody.ClaimRejected($"{claim.EmployeeName}", link, comment);
                    _emailService.SendEmailAsync(claim.Email, "Mileage Claim Rejected", emailbody);
                }
                else
                {
                    claim.Status = ClaimStatus.FinanceRejected;
                    claim.FinanceReviewedAt = DateTime.UtcNow;
                    claim.FinanceComment = comment;
                    var link = $"{Request.Scheme}://{Request.Host}/Mileage/Edit/{claim.Id}";
                    var emailbody = EmailBody.ClaimRejected($"{claim.EmployeeName}", link, comment);
                    _emailService.SendEmailAsync(claim.Email, "Mileage Claim Rejected", emailbody);
                }
                



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

