using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PMApp.Data;
using PMApp.Models;

namespace PMApp.Controllers
{
    public class TaxReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaxReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaxReport
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from m in _context.Tax_Parcels.Include(t => t.Building) select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Building.Org_name.Contains(searchString)
                || s.Year.ToString().Contains(searchString) || s.amount.ToString().Contains(searchString));               
            } 
            

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TaxReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tax_Parcel = await _context.Tax_Parcels
                .Include(t => t.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tax_Parcel == null)
            {
                return NotFound();
            }

            return View(tax_Parcel);
        }

        // GET: TaxReport/Create
        public IActionResult Create()
        {
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Org_name");
            return View();
        }

        // POST: TaxReport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year,BuildingId,amount")] Tax_Parcel tax_Parcel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tax_Parcel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Org_name", tax_Parcel.BuildingId);
            return View(tax_Parcel);
        }

        // GET: TaxReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tax_Parcel = await _context.Tax_Parcels.FindAsync(id);
            if (tax_Parcel == null)
            {
                return NotFound();
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Org_name", tax_Parcel.BuildingId);
            return View(tax_Parcel);
        }

        // POST: TaxReport/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Year,BuildingId,amount")] Tax_Parcel tax_Parcel)
        {
            if (id != tax_Parcel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tax_Parcel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Tax_ParcelExists(tax_Parcel.Id))
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
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Org_name", tax_Parcel.BuildingId);
            return View(tax_Parcel);
        }

        // GET: TaxReport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tax_Parcel = await _context.Tax_Parcels
                .Include(t => t.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tax_Parcel == null)
            {
                return NotFound();
            }

            return View(tax_Parcel);
        }

        // POST: TaxReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tax_Parcel = await _context.Tax_Parcels.FindAsync(id);
            _context.Tax_Parcels.Remove(tax_Parcel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Tax_ParcelExists(int id)
        {
            return _context.Tax_Parcels.Any(e => e.Id == id);
        }
    }
}
