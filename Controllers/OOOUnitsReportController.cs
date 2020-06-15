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
    public class OOOUnitsReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OOOUnitsReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OOOUnitsReport
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from u in _context.Unit.Include(u => u.Building)
                where u.Occupied == "No" && u.Ready_to_rent == "No"
                join re in _context.Repair_History on u.UID equals re.UnitUID into temp
                from lj in temp.DefaultIfEmpty()
                select new OOOViewModel { 
                          Unit_Number = u.Unit_Number,
                          Property = u.Building.Org_name,
                          Square_footage = u.Square_footage,
                          Rent_Amount = u.Rent_Amount,
                          Work_description = lj.Work_description,
                          Date_due = lj.Date_due
                };

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Property.Contains(searchString)
                || s.Unit_Number.ToString().Contains(searchString));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        private bool UnitExists(int id)
        {
            return _context.Unit.Any(e => e.UID == id);
        }
    }
}
