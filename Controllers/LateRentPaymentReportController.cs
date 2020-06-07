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
    public class LateRentPaymentReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LateRentPaymentReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LateRentPaymentReport
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from r in _context.Rent.Include(r => r.Tenant).Include(r => r.Unit)
                                       where r.Date_due < DateTime.Today && r.Date_paid > r.Date_due && r.Date_paid != null
                                       join u in _context.Unit on r.UnitUID equals u.UID into temp2
                                       from lj2 in temp2.DefaultIfEmpty()
                                       join b in _context.Buildings on lj2.BuildingId equals b.BuildingId into temp3
                                       from lj3 in temp3.DefaultIfEmpty()
                                       select new RentViewModel
                                       {
                                           Last_name = r.Tenant.Last_name,
                                           First_name = r.Tenant.First_name,
                                           Property = lj3.Org_name,
                                           Unit = lj2.Unit_Number,
                                           Phone = r.Tenant.Phone,
                                           Email = r.Tenant.Email,
                                           Date_due = r.Date_due,
                                           Date_paid = r.Date_paid,
                                           Rent_amount = r.Rent_amount,
                                           DaysLate = getDays(r.Date_due, r.Date_paid)
                                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Last_name.Contains(searchString)
                || s.First_name.Contains(searchString)
                || s.Property.Contains(searchString)
                || s.Unit.ToString().Contains(searchString));
            }


            return View(await applicationDbContext.ToListAsync());
        }


        private bool RentExists(int id)
        {
            return _context.Rent.Any(e => e.RID == id);
        }

        private static int getDays (DateTime first, DateTime? second)
        {
            DateTime updated = (DateTime)second;
            TimeSpan span = updated.Subtract(first);

            return span.Days;
        }
    }
}
