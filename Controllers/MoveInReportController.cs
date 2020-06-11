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
    public class MoveInReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoveInReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MoveInReport
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from m in _context.Move_in.Include(m => m.Tenant).Include(m => m.Unit) 
                                       join b in _context.Buildings on m.Unit.BuildingId equals b.BuildingId select new MoveInViewModel
                                       {
                                           Last_name = m.Tenant.Last_name,
                                           Unit = m.Unit.Unit_Number,
                                           Property = b.Org_name,
                                           Date = m.Date,
                                           Carpet = m.Carpet,
                                           Appliances = m.Appliances,
                                           Walls = m.Walls,
                                           Nonrefundable_deposit = m.Nonrefundable_deposit,
                                           Refundable_deposit = m.Refundable_deposit,
                                           Pet_deposit = m.Pet_deposit
                                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Last_name.Contains(searchString)
                || s.Unit.ToString().Contains(searchString)
                || s.Property.Contains(searchString));
            }


            return View(await applicationDbContext.ToListAsync());
        }

       
        private bool Move_inExists(int id)
        {
            return _context.Move_in.Any(e => e.MIID == id);
        }
    }
}
