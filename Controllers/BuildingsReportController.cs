using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;

namespace PMApp.Controllers
{
    public class BuildingsReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuildingsReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BuildingsReport
        public async Task<IActionResult> Index(string searchString)
        {
            var building = from m in _context.Buildings
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                building = building.Where(s => s.City.Contains(searchString) || s.State.Contains(searchString) ||
                 s.Org_name.Contains(searchString) || s.Zip_code.Contains(searchString) || s.TPID.Contains(searchString));
            }

            return View(await building.ToListAsync());
        }

        // GET: BuildingsReport/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings
                .FirstOrDefaultAsync(m => m.BuildingId == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }


        private bool BuildingExists(long id)
        {
            return _context.Buildings.Any(e => e.BuildingId == id);
        }

        public IActionResult Print()
        {
            try
            {
                var buildings = from m in _context.Buildings
                               select m;

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Id,Property,Units,Street,City,State,Zip Code,Tax Parcel");

                foreach (var building in buildings)
                {
                    stringBuilder.AppendLine($"{building.BuildingId},{ building.Org_name},{ building.Unit_Count}," +
                        $"{ building.Street},{ building.City},{ building.State},{ building.Zip_code},{ building.TPID}");
                }

                return File(Encoding.UTF8.GetBytes
                (stringBuilder.ToString()), "text/csv", "properties.csv");
            }
            catch
            {
                return View(nameof(Index));
            }
        }

    }
}
