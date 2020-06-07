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
    public class AccountingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounting
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rent.Include(r => r.Tenant).Include(r => r.Unit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accounting/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent
                .Include(r => r.Tenant)
                .Include(r => r.Unit)
                .FirstOrDefaultAsync(m => m.RID == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // GET: Accounting/Create
        public IActionResult Create()
        {
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name");
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID");
            return View();
        }

        // POST: Accounting/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RID,Date_due,Date_paid,Rent_amount,Pet_fee,Parking_fee,TenantTID,UnitUID")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", rent.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", rent.UnitUID);
            return View(rent);
        }

        // GET: Accounting/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", rent.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", rent.UnitUID);
            return View(rent);
        }

        // POST: Accounting/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RID,Date_due,Date_paid,Rent_amount,Pet_fee,Parking_fee,TenantTID,UnitUID")] Rent rent)
        {
            if (id != rent.RID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.RID))
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
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", rent.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", rent.UnitUID);
            return View(rent);
        }

        // GET: Accounting/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rent
                .Include(r => r.Tenant)
                .Include(r => r.Unit)
                .FirstOrDefaultAsync(m => m.RID == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Accounting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rent.FindAsync(id);
            _context.Rent.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rent.Any(e => e.RID == id);
        }
    }
}
