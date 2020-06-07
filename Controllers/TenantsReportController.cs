﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;
using PMApp.ViewModels;

namespace PMApp.Controllers
{
    public class TenantsReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TenantsReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TenantsReport
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

        // GET: TenantsReport/Details/5
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

            return View(tenant);
        }
    }
}
