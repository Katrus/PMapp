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
    public class CurrentTenantsReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurrentTenantsReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CurrentTenantsReport
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from t in _context.Tenant
                                       where t.Lease_end_date > DateTime.Today && t.Lease_start_date < DateTime.Today
                                       join r in _context.Move_in on t.TID equals r.TenantTID into temp
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
                                           Phone = t.Phone,
                                           Email = t.Email
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


        private bool TenantExists(int id)
        {
            return _context.Tenant.Any(e => e.TID == id);
        }
    }
}
