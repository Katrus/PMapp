using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;

namespace PMApp.Controllers
{
    public class UnitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Units
        public async Task<IActionResult> Index(string id)
        {
            ViewBag.Bname = id;
            var applicationDbContext = _context.Unit.Include(u => u.Building);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Units/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Unit
                .Include(u => u.Building)
                .FirstOrDefaultAsync(m => m.UID == id);
            if (unit == null)
            {
                return NotFound();
            }

            unit.Repair_Histories = _context.Repair_History.Where(m => m.UnitUID == id).ToList();
            unit.Move_Ins = _context.Move_in.Where(m => m.UnitUID == id).ToList();
            unit.Move_Outs = _context.Move_out.Where(m => m.UnitUID == id).ToList();

            return View(unit);
        }

        // GET: Units/Create
        public IActionResult Create(long BuildingId)
        {
            var building = from b in _context.Buildings
                         where b.BuildingId == BuildingId
                         select b;

            ViewData["BuildingId"] = new SelectList(building, "BuildingId", "Org_name");
            return View() ;
        }

        // POST: Units/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UID,Unit_Number,Rent_Amount,BuildingId,Square_footage, Bedroom,Bath,Occupied,Ready_to_rent")] Unit unit, long BuildingId)
        {
            var build = from b in _context.Buildings
                           where b.BuildingId == BuildingId
                           select b;

            ViewData["BuildingId"] = new SelectList(build, "BuildingId", "Org_name");

            if (ModelState.IsValid)
            {
                var units = from u in _context.Unit where u.BuildingId == unit.BuildingId select u;
                foreach (var u in units)
                {
                    if(u.Unit_Number == unit.Unit_Number)
                    {
                        ViewBag.Message = "This unit already exists!";
                        return View(unit);
                    }
                }

                unit.Occupied = "No";
                unit.Ready_to_rent = "No";
                unit.ReservedBy = null;
                _context.Add(unit);

                var building = await _context.Buildings.FindAsync(BuildingId);
                building.Unit_Count += 1;
                _context.Update(building);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Buildings", new { id = unit.BuildingId });
            }
           
            return View(unit);
        }

        // GET: Units/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Unit.FindAsync(id);
            var building = from b in _context.Buildings where b.BuildingId == unit.BuildingId select b;

            if (unit == null)
            {
                return NotFound();
            }
            ViewData["BuildingId"] = new SelectList(building, "BuildingId", "Org_name");

            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UID,Unit_Number,Rent_Amount,BuildingId,Square_footage,Bedroom,Bath,ReservedBy,Occupied,Ready_to_rent")] Unit unit)
        {
            if (id != unit.UID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                { 
                    _context.Update(unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(unit.UID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Buildings", new { id = unit.BuildingId });
            }
            ViewData["BuildingId"] = new SelectList(_context.Buildings, "BuildingId", "Org_name", unit.BuildingId);
            return View(unit);
        }

        // GET: Units/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var unit = await _context.Unit
                .Include(u => u.Building)
                .FirstOrDefaultAsync(m => m.UID == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unit = await _context.Unit.FindAsync(id);

            if (unit.Occupied.Equals("Yes"))
            {
                ViewBag.Message = "Unit is occupied. Move out the Tenant before deleting!";
                return View(unit);
            }

            if (unit.ReservedBy != null)
            {
                ViewBag.Message = "Unit is Reserved. Release the Unit before deleting!";
                return View(unit);
            }

            _context.Unit.Remove(unit);

            var building = await _context.Buildings.FindAsync(unit.BuildingId);
            building.Unit_Count -= 1;

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Buildings", new { id = unit.BuildingId });
        }

        private bool UnitExists(int id)
        {
            return _context.Unit.Any(e => e.UID == id);
        }
    }
}
