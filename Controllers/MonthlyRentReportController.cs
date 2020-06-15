using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;
using PMApp.ViewModels;

namespace PMApp.Controllers
{
    public class MonthlyRentReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonthlyRentReportController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: MonthlyRentReport
        public async Task<IActionResult> Index()
        {

            var applicationDbContext = from r in _context.Rent.Include(r => r.Tenant).Include(r => r.Unit)
                                       where r.Date_due < DateTime.Today && r.Date_paid != null
                                       group r by new { r.Date_due.Year, r.Date_due.Month } into a
                                       select new RentViewModel
                                       {
                                           Month = getMonth(a.Key.Month),
                                           Year = a.Key.Year,
                                           Rent_amount = a.Sum(x => x.Rent_amount),
                                           MonthNum = a.Key.Month
                                       };
                return View(await applicationDbContext.ToListAsync());
        }


        private bool RentExists(int id)
        {
            return _context.Rent.Any(e => e.RID == id);
        }

        private static string getMonth(int m)
        {
            switch(m)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
            }

            return "Unspecified";
        }

    
    }
}
