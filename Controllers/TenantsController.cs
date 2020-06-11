using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;
using PMApp.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace PMApp.Controllers
{
    public class TenantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _env;

        public TenantsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
                                           Pets = t.Pets,
                                           Current = t.Current
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

            if(tenant.ReservedUnit != null)
            {
                var unit = await _context.Unit.FindAsync(tenant.ReservedUnit);
                var building = await _context.Buildings.FindAsync(unit.BuildingId);
                ViewBag.UnitNumber = unit.Unit_Number;
                ViewBag.Property = building.Org_name;
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

                tenant.Current = "New";
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
        public async Task<IActionResult> Edit(int id, [Bind("TID,Last_name,First_name,Employer,Salary,Current,ReservedUnit,Lease_start_date,Lease_end_date,Phone,Email,Pets")] Tenant tenant)
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

            if (ten.ReservedUnit != null)
            {
                ViewBag.Message = "Release the unit before deleting the tenant!";
                return View(ten);

            }

            var tenant = await _context.Tenant.FindAsync(id);
            _context.Tenant.Remove(tenant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> selectBuilding(string searchString, int TID)
        {
            var building = from m in _context.Buildings
                           select m;

            ViewBag.Tenant = TID;

            if (!String.IsNullOrEmpty(searchString))
            {
                building = building.Where(s => s.City.Contains(searchString) || s.State.Contains(searchString)

                || s.Org_name.Contains(searchString) || s.Zip_code.Contains(searchString) || s.TPID.Contains(searchString));
            }
            return View(await building.ToListAsync());
        }

        public async Task<IActionResult> selectUnit(long BuildingId, int TID)
        {
            ViewBag.Tenant = TID;
            var units = from u in _context.Unit where u.BuildingId == BuildingId && u.Occupied.Equals("No") select u;
            var building = await _context.Buildings.FindAsync(BuildingId);
            ViewBag.Bname = building.Org_name;

            return View(await units.ToListAsync());
        }

        public async Task<IActionResult> reserveUnit(int UID, int TID)
        {
            try
            {
                var unit = await _context.Unit.FindAsync(UID);
                var tenant = await _context.Tenant.FindAsync(TID);
                unit.Occupied = "Reserved";
                unit.ReservedBy = TID;
                tenant.ReservedUnit = UID;
                _context.Update(unit);
                _context.Update(tenant);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tenants", new { id = TID });
            }

            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<IActionResult> releaseUnit(int UID, int TID)
        {
            try
            {
                var unit = await _context.Unit.FindAsync(UID);
                var tenant = await _context.Tenant.FindAsync(TID);
                unit.Occupied = "No";
                unit.ReservedBy = null;
                tenant.ReservedUnit = null;
                _context.Update(unit);
                _context.Update(tenant);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tenants", new { id = TID });
            }

            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

        }

        private bool TenantExists(int id)
        {
            return _context.Tenant.Any(e => e.TID == id);
        }

        public IActionResult UploadFile(IFormFile file)
        {
            var dir = _env.ContentRootPath;
            using(var fileStream = new FileStream(Path.Combine(dir, "file.pdf"), FileMode.Create, FileAccess.Write))
            {
                file.CopyTo(fileStream);
            }

            return RedirectToAction("Index");
        }

    }
}
