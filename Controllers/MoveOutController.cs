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
    public class MoveOutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoveOutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MoveOut
        public async Task<IActionResult> Index(long BuildingId)
        {
            var applicationDbContext = _context.Move_out.Include(m => m.Tenant).Include(m => m.Unit);
            var buildingMoveOuts = from m in applicationDbContext where m.Unit.BuildingId == BuildingId select m;

            ViewBag.Building = BuildingId;
            return View(await buildingMoveOuts.ToListAsync());
        }

        // GET: MoveOut/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var move_out = await _context.Move_out
                .Include(m => m.Tenant)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.MOID == id);
            if (move_out == null)
            {
                return NotFound();
            }

            return View(move_out);
        }

        // GET: MoveOut/Create
        public IActionResult Create(long UnitUID)
        {
            var mounit = from m in _context.Unit
                         where m.UID == UnitUID
                         select m;

            var motenant = from t in _context.Tenant join m in _context.Move_in on t.TID equals m.TenantTID
                           where m.UnitUID == UnitUID && t.Current.Equals("Yes") select t;

            ViewBag.TenantTID = new SelectList(motenant, "TID", "Last_name");
            ViewBag.UnitUID = new SelectList(mounit, "UID", "Unit_Number");

            return View();

        }

        // POST: MoveOut/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(long BuildingId, [Bind("MOID,Date,Carpet,Appliances,Walls,Cleaning_fee,Damage_fee,fees_paid,TenantTID,UnitUID")] Move_out move_out)
        {
            if (ModelState.IsValid)
            {
                var mounit = from m in _context.Unit where m.UID == move_out.UnitUID select m;

                var motenant = from u in mounit
                               join mi in _context.Move_in
                               on u.UID equals mi.UnitUID into temp
                               from lj in temp.DefaultIfEmpty()
                               join t in _context.Tenant
                               on lj.TenantTID equals t.TID into temp2
                               from lj2 in temp2.DefaultIfEmpty()
                               select lj2;

                ViewBag.TenantTID = new SelectList(motenant, "TID", "Last_name");
                ViewBag.UnitUID = new SelectList(mounit, "UID", "Unit_Number");

                var move_in = from m in _context.Move_in where m.TenantTID == move_out.TenantTID select m;

                foreach (var m in move_in)
                {
                    if(move_out.Date < m.Date)
                    {
                        ViewBag.Message = "Invalid move out date!";
                        return View(move_out);
                    }
                }

                if (move_out.Date > DateTime.Today.AddDays(1))
                {
                    ViewBag.Message = "Move out date cannot be in the future";
                    return View(move_out);
                }

                var tenant = await _context.Tenant.FindAsync(move_out.TenantTID);
                tenant.Current = "No";
                tenant.ReservedUnit = null;
                tenant.Lease_end_date = move_out.Date;
                _context.Add(move_out);
                _context.Update(tenant);

                var unit = await _context.Unit.FindAsync(move_out.UnitUID);
                unit.Occupied = "No";
                unit.Ready_to_rent = "No";

                _context.Update(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Buildings", new { id = unit.BuildingId });
            }
           
            return View(move_out);
        }

        // GET: MoveOut/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var move_out = await _context.Move_out.FindAsync(id);
            var tenant = from t in _context.Tenant where t.TID == move_out.TenantTID select t;
            var unit = from u in _context.Unit where u.UID == move_out.UnitUID select u;
            
            if (move_out == null)
            {
                return NotFound();
            }
            ViewData["TenantTID"] = new SelectList(tenant, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(unit, "UID", "Unit_Number");
            return View(move_out);
        }

        // POST: MoveOut/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MOID,Date,Carpet,Appliances,Walls,Cleaning_fee,Damage_fee,fees_paid,TenantTID,UnitUID")] Move_out move_out)
        {
            var tenant = from t in _context.Tenant where t.TID == move_out.TenantTID select t;
            var unit = from u in _context.Unit where u.UID == move_out.UnitUID select u;

            ViewData["TenantTID"] = new SelectList(tenant, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(unit, "UID", "Unit_Number");

            if (id != move_out.MOID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(move_out);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Move_outExists(move_out.MOID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var units = await _context.Unit.FindAsync(move_out.UnitUID);
                return RedirectToAction("Details", "Buildings", new { id = units.BuildingId });
            }
            return View(move_out);
        }

        // GET: MoveOut/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var move_out = await _context.Move_out
                .Include(m => m.Tenant)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.MOID == id);
            if (move_out == null)
            {
                return NotFound();
            }

            return View(move_out);
        }

        // POST: MoveOut/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var move_out = await _context.Move_out.FindAsync(id);

            var unit = await _context.Unit.FindAsync(move_out.UnitUID);
            

            if(unit.Occupied.Equals("Yes")) {
                 ViewBag.Message = "Unable to withdraw. Unit already occupied by another Tenant";
                 return View(move_out);
            } else if (unit.Ready_to_rent.Equals("No"))
            {
                ViewBag.Message = "Unable to withdraw. Unit is not available";
                return View(move_out);
            } else if ((DateTime.Today - move_out.Date).TotalDays > 2)
            {
                ViewBag.Message = "Record archived. Unable to withdraw.";
                return View(move_out);
            }

            unit.Occupied = "Yes";
            unit.Ready_to_rent = "No";
            _context.Unit.Update(unit);

            var tenant = await _context.Tenant.FindAsync(move_out.TenantTID);
            tenant.Current = "Yes";
            tenant.ReservedUnit = unit.UID;
            _context.Tenant.Update(tenant);

            _context.Move_out.Remove(move_out);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Buildings", new { id = unit.BuildingId });
        }

        private bool Move_outExists(int id)
        {
            return _context.Move_out.Any(e => e.MOID == id);
        }
    }
}