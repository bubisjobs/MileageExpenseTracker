using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MileageExpenseTracker.Data;
using MileageExpenseTracker.Models;

namespace MileageExpenseTracker.Controllers
{
    [Authorize(Roles = "Admin,Finance,TeamLead")]
    public class WiksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WiksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wiks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Wik.ToListAsync());
        }

        // GET: Wiks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wik = await _context.Wik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wik == null)
            {
                return NotFound();
            }

            return View(wik);
        }

        // GET: Wiks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wiks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Wik wik)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var checkForWik = _context.Wik.Any(x => x.Name == wik.Name);
                    if (checkForWik) {

                        TempData["ErrorMessage"] = "Wik already exisitng..";
                        return View();
                    }
                    wik.Name = wik.Name.ToUpper();
                    _context.Add(wik);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Wik has been created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while creating the Wik.";
                    return View(wik);
                }
            }
            return View(wik);
        }

        // GET: Wiks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wik = await _context.Wik.FindAsync(id);
            if (wik == null)
            {
                return NotFound();
            }
            return View(wik);
        }

        // POST: Wiks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Wik wik)
        {
            if (id != wik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WikExists(wik.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(wik);
        }

        // GET: Wiks/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var wik = await _context.Wik
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (wik == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(wik);
        //}

        [HttpPost("Wik/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var wik = await _context.Wik.FindAsync(id);
                if (wik != null)
                {
                    _context.Wik.Remove(wik);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Wik has been deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Wik not found.";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Wik.";
            }

            return RedirectToAction(nameof(Index));
        }



        private bool WikExists(int id)
        {
            return _context.Wik.Any(e => e.Id == id);
        }
    }
}
