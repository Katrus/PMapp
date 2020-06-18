using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;

namespace PMApp.Controllers
{
    public class Tax_ParcelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Tax_ParcelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tax_Parcel
        public async Task<IActionResult> Index(long BuildingId)
        {
            var applicationDbContext = from t in _context.Tax_Parcels.Include(t => t.Building) where t.BuildingId == BuildingId
                                       select t;

            ViewBag.Building = BuildingId;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tax_Parcel/Details/5
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

        // GET: Tax_Parcel/Create
        public IActionResult Create(long BuildingId)
        {
            var building = from b in _context.Buildings where b.BuildingId == BuildingId select b;

            ViewBag.Building = BuildingId;
            ViewData["BuildingId"] = new SelectList(building, "BuildingId", "Org_name");
            return View();
        }

        // POST: Tax_Parcel/Create
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
                return RedirectToAction("Index", "Tax_Parcel", new { BuildingId = tax_Parcel.BuildingId});
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Org_name", tax_Parcel.BuildingId);
            return View(tax_Parcel);
        }

        // GET: Tax_Parcel/Edit/5
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

            var building = from b in _context.Buildings where b.BuildingId == tax_Parcel.BuildingId select b;
            ViewData["BuildingId"] = new SelectList(building, "BuildingId", "Org_name");
            return View(tax_Parcel);
        }

        // POST: Tax_Parcel/Edit/5
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

            ViewBag.Building = tax_Parcel.BuildingId;

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
                return RedirectToAction("Index", "Tax_Parcel", new { BuildingId = tax_Parcel.BuildingId });
            }

            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Org_name", tax_Parcel.BuildingId);
            return View(tax_Parcel);
        }

        // GET: Tax_Parcel/Delete/5
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

            ViewBag.Building = tax_Parcel.BuildingId;

            return View(tax_Parcel);
        }

        // POST: Tax_Parcel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tax_Parcel = await _context.Tax_Parcels.FindAsync(id);
            _context.Tax_Parcels.Remove(tax_Parcel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Tax_Parcel", new { BuildingId = tax_Parcel.BuildingId });
        }

        private bool Tax_ParcelExists(int id)
        {
            return _context.Tax_Parcels.Any(e => e.Id == id);
        }

    }
}
