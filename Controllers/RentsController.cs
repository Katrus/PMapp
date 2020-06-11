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
    public class RentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rent.Include(r => r.Tenant).Include(r => r.Unit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rents/Details/5
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

        // GET: Rents/Create
        public IActionResult Create(int TID)
        {
            var tenants = from t in _context.Tenant where t.TID == TID select t;
            var units = from u in _context.Unit
                        join m in _context.Move_in
                        on u.UID equals m.UnitUID
                        where m.TenantTID == TID
                        select u;

            ViewData["TenantTID"] = new SelectList(tenants, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(units, "UID", "Unit_Number");
            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RID,Date_due,Date_paid,Rent_amount,Pet_fee,Parking_fee,TenantTID,UnitUID")] Rent rent)
        {
            var tenants = from t in _context.Tenant where t.TID == rent.TenantTID select t;
            var unit = from u in _context.Unit where u.UID == rent.UnitUID select u;

            var move_out = from m in _context.Move_out where m.TenantTID == rent.TenantTID select m;

            ViewData["TenantTID"] = new SelectList(tenants, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(unit, "UID", "Unit_number");

            if (ModelState.IsValid)
            {
                rent.Amount_paid = 0;
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tenants", new { id = rent.TenantTID });
            }
          
            return View(rent);
        }

        // GET: Rents/Edit/5
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

            var tenants = from t in _context.Tenant where t.TID == rent.TenantTID select t;
            var unit = from u in _context.Unit where u.UID == rent.UnitUID select u;

            ViewData["TenantTID"] = new SelectList(tenants, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(unit, "UID", "Unit_Number");
          
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RID,Date_due,Date_paid,Rent_amount,Amount_paid,Pet_fee,Parking_fee,TenantTID,UnitUID")] Rent rent)
        {
            if (id != rent.RID)
            {
                return NotFound();
            }

            var tenants = from t in _context.Tenant where t.TID == rent.TenantTID select t;
            var unit = from u in _context.Unit where u.UID == rent.UnitUID select u;

            ViewData["TenantTID"] = new SelectList(tenants, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(unit, "UID", "Unit_Number");

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
                return RedirectToAction("Details", "Tenants", new { id = rent.TenantTID });
            }

            return View(rent);
        }

        // GET: Rents/Delete/5
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

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rent.FindAsync(id);
            _context.Rent.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tenants", new { id = rent.TenantTID });
        }

        private bool RentExists(int id)
        {
            return _context.Rent.Any(e => e.RID == id);
        }
    }
}
