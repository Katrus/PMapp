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
    public class VehiclesReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: VehiclesReport
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from v in _context.Vehicle.Include(v => v.Tenant)
                                       where v.Tenant.Lease_end_date > DateTime.Today && v.Tenant.Lease_start_date < DateTime.Today
                                       join r in _context.Rent on v.TenantTID equals r.TenantTID into temp
                                       from lj in temp.DefaultIfEmpty()
                                       join u in _context.Unit on lj.UnitUID equals u.UID into temp2
                                       from lj2 in temp2.DefaultIfEmpty()
                                       join b in _context.Buildings on lj2.BuildingId equals b.BuildingId into temp3
                                       from lj3 in temp3.DefaultIfEmpty()
                                       select new VehicleViewModel
                                       {
                                           VID = v.VID,
                                           Last_name = v.Tenant.Last_name,
                                           First_name = v.Tenant.First_name,
                                           Lease_start_date = v.Tenant.Lease_start_date,
                                           Lease_end_date = v.Tenant.Lease_end_date,
                                           Property = lj3.Org_name,
                                           Unit = lj2.Unit_Number,
                                           License_plate = v.License_plate,
                                           Make = v.Make,
                                           Model = v.Model,
                                           Year = v.Year,
                                           Color = v.Color,
                                           stall_number = v.stall_number
                                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Last_name.Contains(searchString)
                || s.First_name.Contains(searchString)
                || s.Lease_start_date.ToString().Contains(searchString)
                || s.Lease_end_date.ToString().Contains(searchString)
                || s.Property.Contains(searchString)
                || s.Unit.ToString().Contains(searchString)
                || s.stall_number.ToString().Contains(searchString)
                || s.Color.Contains(searchString)
                || s.Model.Contains(searchString)
                || s.Make.Contains(searchString)
                || s.License_plate.Contains(searchString));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: VehiclesReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Tenant)
                .FirstOrDefaultAsync(m => m.VID == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.VID == id);
        }
    }
}
