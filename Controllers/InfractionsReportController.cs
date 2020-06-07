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
    public class InfractionsReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InfractionsReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InfractionsReport
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from m in _context.Infractions.Include(i => i.Tenant)
                                       where m.Day_closed != null
                                       join r in _context.Move_in on m.TenantTID equals r.TenantTID into temp
                                       from lj in temp.DefaultIfEmpty()
                                       join u in _context.Unit on lj.UnitUID equals u.UID into temp2
                                       from lj2 in temp2.DefaultIfEmpty()
                                       join b in _context.Buildings on lj2.BuildingId equals b.BuildingId into temp3
                                       from lj3 in temp3.DefaultIfEmpty()
                                       select new InfractionsViewModel
                                       {
                                           IID = m.IID,
                                           Day_opened = m.Day_opened,
                                           Day_closed = m.Day_closed,
                                           Description = m.Description,
                                           Resolution = m.Resolution,
                                           Last_name = m.Tenant.Last_name,
                                           First_name = m.Tenant.First_name,
                                           Property = lj3.Org_name,
                                           Unit = lj2.Unit_Number
                                       };
                                       
         
            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Last_name.Contains(searchString) 
                || s.First_name.Contains(searchString) 
                || s.Description.Contains(searchString)
                || s.Resolution.Contains(searchString)
                || s.Property.Contains(searchString)
                || s.Unit.ToString().Contains(searchString));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: InfractionsReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infractions = await _context.Infractions
                .Include(i => i.Tenant)
                .FirstOrDefaultAsync(m => m.IID == id);
            if (infractions == null)
            {
                return NotFound();
            }

            return View(infractions);
        }


        private bool InfractionsExists(int id)
        {
            return _context.Infractions.Any(e => e.IID == id);
        }
    }
}
