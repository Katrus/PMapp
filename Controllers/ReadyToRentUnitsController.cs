using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;

namespace PMApp.Controllers
{
    public class ReadyToRentUnitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadyToRentUnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReadyToRentUnits
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from m in _context.Unit.Include(u => u.Building)
                                       where m.Occupied.ToUpper() == "NO" && m.Ready_to_rent.ToUpper() == "YES"
                                       select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Ready_to_rent.Contains(searchString)
                || s.Occupied.Contains(searchString)
                || s.Unit_Number.ToString().Contains(searchString)
                || s.Building.Org_name.Contains(searchString));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        private bool UnitExists(int id)
        {
            return _context.Unit.Any(e => e.UID == id);
        }
    }
}
