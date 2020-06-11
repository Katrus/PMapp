using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;
using PMApp.ViewModels;

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
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from m in _context.Rent.Include(r => r.Tenant).Include(r => r.Unit) 
                                       join b in _context.Buildings on m.Unit.BuildingId equals b.BuildingId select
                                       new RentViewModel
                                       {
                                           RID = m.RID,
                                           Property = b.Org_name,
                                           Unit = m.Unit.Unit_Number,
                                           Last_name = m.Tenant.Last_name,
                                           Rent_amount = m.Rent_amount,
                                           Date_due = m.Date_due,
                                           Date_paid = m.Date_paid,
                                           Amount_paid = m.Amount_paid,
                                           Balance = m.Balance
                                           
                                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Last_name.Contains(searchString)
                || s.Property.Contains(searchString)
                || s.Unit.ToString().Contains(searchString));
            }

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

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date_due")] Rent rent)
        {

            if (ModelState.IsValid)
            {
                if (rent.Date_due < DateTime.Today)
                {
                    ViewBag.Message = "Batch invoice cannot be done for the past date!";
                    return View(rent);
                }
                var units = from u in _context.Unit where u.Occupied.Equals("Yes") select u;

                foreach (var u in units)
                {
                    var tenant = from t in _context.Tenant
                                 join m in _context.Move_in on t.TID equals m.TenantTID
                                 where t.Current.Equals("Yes") && m.UnitUID == u.UID
                                 select t;
                    foreach (var t in tenant)
                    {
                        Rent rents = new Rent
                        {
                            UnitUID = u.UID,
                            Rent_amount = u.Rent_Amount,
                            Date_due = rent.Date_due,
                            TenantTID = t.TID
                        };
                        _context.Add<Rent>(rents);
                    }


                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

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
            var tenant = from t in _context.Tenant where t.TID == rent.TenantTID select t;
            var unit = from u in _context.Unit where u.UID== rent.UnitUID select u;

            if (rent == null)
            {
                return NotFound();
            }
            ViewData["TenantTID"] = new SelectList(tenant, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(unit, "UID", "Unit_Number");
            return View(rent);
        }

        // POST: Accounting/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RID,Date_due,Date_paid,Rent_amount,Amount_paid,TenantTID,UnitUID")] Rent rent)
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
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "Last_name", rent.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "Unit_Number", rent.UnitUID);
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
