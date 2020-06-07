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
    public class BuildingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuildingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Buildings
        public async Task<IActionResult> Index(string searchString)
        {
            var building = from m in _context.Buildings
                           select m;
         

            if (!String.IsNullOrEmpty(searchString))
            {
                building = building.Where(s => s.City.Contains(searchString) || s.State.Contains(searchString)

                || s.Org_name.Contains(searchString) || s.Zip_code.Contains(searchString) || s.TPID.Contains(searchString));
            }
            return View(await building.ToListAsync());
        }

        // GET: Buildings/Details/5
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

            building.Units = _context.Unit.Where(m => m.BuildingId == id).ToList();

            var tenantsOfBuilding = from u in _context.Unit
                                    where u.BuildingId == id && u.Occupied.Equals("Yes")
                                    join mi in _context.Move_in on u.UID equals mi.UnitUID into temp
                                    from lj in temp.DefaultIfEmpty()
                                    join t in _context.Tenant
                                    on lj.TenantTID equals t.TID into temp2
                                    from lj2 in temp2
                                    where lj2.Current.Equals("Yes")
                                    select lj2;


            building.Tenants = tenantsOfBuilding.ToList();

            return View(building);
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BuildingId,Org_name,Unit_Count,Street,City,Zip_code,State,TPID")] Building building)
        {
            if (ModelState.IsValid)
            {
                building.Unit_Count = 0;
                _context.Add(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(building);
        }

        public async Task<IActionResult> makeAvailable(int UnitUID, long BuildingId)
        {
            if (ModelState.IsValid)
            {
                var unit = await _context.Unit.FindAsync(UnitUID);
                unit.Occupied = "No";
                unit.Ready_to_rent = "Yes";

                _context.Update(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Buildings", new {id = BuildingId });
            }
            return View();
        }

        public async Task<IActionResult> makeUnavailable(int UnitUID, long BuildingId)
        {
            if (ModelState.IsValid)
            {
                var unit = await _context.Unit.FindAsync(UnitUID);
                unit.Occupied = "No";
                unit.Ready_to_rent = "No";

                _context.Update(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Buildings", new { id = BuildingId });
            }
            return View();
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }
            return View(building);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("BuildingId,Org_name,Unit_Count,Street,City,Zip_code,State,TPID")] Building building)
        {
            if (id != building.BuildingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.BuildingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(building);
        }

        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(long? id)
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

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var building = await _context.Buildings.FindAsync(id);
            var units = from u in _context.Unit where u.BuildingId == id select u;

            foreach(var u in units)
            {
                if(u.Occupied.Equals("Yes"))
                {
                    ViewBag.Message = "Move out all Tenants first!";
                    return View(building);
                }
            }

            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(long id)
        {
            return _context.Buildings.Any(e => e.BuildingId == id);
        }
    }
}
