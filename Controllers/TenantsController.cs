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
    public class TenantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TenantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tenants
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from t in _context.Tenant
                                       join r in _context.Move_in on t.TID
              equals r.TenantTID into temp
                                       from lj in temp.DefaultIfEmpty()
                                       join u in _context.Unit on lj.UnitUID equals u.UID into temp2
                                       from lj2 in temp2.DefaultIfEmpty()
                                       join b in _context.Buildings on lj2.BuildingId equals b.BuildingId into temp3
                                       from lj3 in temp3.DefaultIfEmpty()
                                       select new TenantViewModel
                                       {
                                           TID = t.TID,
                                           Last_name = t.Last_name,
                                           First_name = t.First_name,
                                           Lease_start_date = t.Lease_start_date,
                                           Lease_end_date = t.Lease_end_date,
                                           Property = lj3.Org_name,
                                           Unit = lj2.Unit_Number,
                                           Employer = t.Employer,
                                           Salary = t.Salary,
                                           Phone = t.Phone,
                                           Email = t.Email,
                                           Pets = t.Pets
                                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Last_name.Contains(searchString)
                || s.First_name.Contains(searchString)
                || s.Lease_start_date.ToString().Contains(searchString)
                || s.Lease_end_date.ToString().Contains(searchString)
                || s.Property.Contains(searchString)
                || s.Unit.ToString().Contains(searchString));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tenants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenant
                .FirstOrDefaultAsync(m => m.TID == id);
            if (tenant == null)
            {
                return NotFound();
            }

            tenant.Vehicles = _context.Vehicle.Where(m => m.TenantTID == id).ToList();
            tenant.Rents = _context.Rent.Where(m => m.TenantTID == id).ToList();
            tenant.Infractions = _context.Infractions.Where(m => m.TenantTID == id).ToList();
       
            return View(tenant);
        }

        // GET: Tenants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TID,Last_name,First_name,Employer,Salary,Lease_start_date,Lease_end_date,Phone,Email,Pets")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                if (tenant.Lease_end_date < tenant.Lease_start_date)
                {
                    ViewBag.Message = "Invalid Lease ending date.";
                    return View(tenant);
                } else if (tenant.Lease_start_date < DateTime.Today)
                {
                    ViewBag.Message = "Invalid Lease start date.";
                    return View(tenant);
                }

                tenant.Current = "No";
                _context.Add(tenant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tenant);
        }

        // GET: Tenants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenant.FindAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TID,Last_name,First_name,Employer,Salary,Lease_start_date,Lease_end_date,Phone,Email,Pets")] Tenant tenant)
        {
            if (id != tenant.TID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tenant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantExists(tenant.TID))
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
            return View(tenant);
        }

        // GET: Tenants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tenant = await _context.Tenant
                .FirstOrDefaultAsync(m => m.TID == id);
            if (tenant == null)
            {
                return NotFound();
            }

            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ten = await _context.Tenant.FindAsync(id);

            if (ten.Current.Equals("Yes"))
            {
                ViewBag.Message = "Tenant is still moved in. Please move out the Tenant before deleting!";
                return View(ten);

            }

            var tenant = await _context.Tenant.FindAsync(id);
            _context.Tenant.Remove(tenant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TenantExists(int id)
        {
            return _context.Tenant.Any(e => e.TID == id);
        }

    }
}
