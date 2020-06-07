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
    public class MoveInController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoveInController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MoveIn
        public async Task<IActionResult> Index(long BuildingId)
        {
            var applicationDbContext = _context.Move_in.Include(m => m.Tenant).Include(m => m.Unit);
            var buildingMoveIns = from m in applicationDbContext where m.Unit.BuildingId == BuildingId select m;

            return View(await buildingMoveIns.ToListAsync());

        }

        // GET: MoveIn/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var move_in = await _context.Move_in
                .Include(m => m.Tenant)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.MIID == id);
            if (move_in == null)
            {
                return NotFound();
            }

            return View(move_in);
        }

        // GET: MoveIn/Create
        public IActionResult Create(int UnitUID, int TID)
        {
            var mounit = from m in _context.Unit
                         where m.UID == UnitUID
                         select m;

            var motenant = from t in _context.Tenant
                           where t.TID == TID
                           select t;
          
            ViewBag.TenantTID = new SelectList(motenant, "TID", "Last_name");
            ViewBag.UnitUID = new SelectList(mounit, "UID", "Unit_Number");

            return View();
        }

        // POST: MoveIn/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(long BuildingId, [Bind("MIID,UnitUID,Date,Carpet,Appliances,Walls,Refundable_deposit,Nonrefundable_deposit,Pet_deposit,TenantTID")] Move_in move_in)
        {
            var miunit = from m in _context.Unit where m.UID == move_in.UnitUID select m;
            var mitenant = from t in _context.Tenant where t.TID == move_in.TenantTID select t;

            ViewData["TenantTID"] = new SelectList(mitenant, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(miunit, "UID", "Unit_Number");


            if (move_in.Date > DateTime.Today.AddDays(1)) 
            {
                ViewBag.Message = "Move in date cannot be in the future";
                return View(move_in);
            }

            if (ModelState.IsValid)
            {
                _context.Add(move_in);

                var tenant = await _context.Tenant.FindAsync(move_in.TenantTID);
                tenant.Current = "Yes";
                tenant.Lease_start_date = move_in.Date;
                _context.Update(tenant);

                var unit = await _context.Unit.FindAsync(move_in.UnitUID);
                unit.Occupied = "Yes";
                unit.Ready_to_rent = "No";
                
                _context.Update(unit);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(move_in);
        }

        // GET: MoveIn/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var move_in = await _context.Move_in.FindAsync(id);
            var tenant = from t in _context.Tenant where t.TID == move_in.TenantTID select t;
            var unit = from u in _context.Unit where u.UID == move_in.UnitUID select u;
           

            if (move_in == null)
            {
                return NotFound();
            }

            ViewData["TenantTID"] = new SelectList(tenant, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(unit, "UID", "Unit_Number");

            
            return View(move_in);
        }

        // POST: MoveIn/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MIID,UnitUID,Date,Carpet,Appliances,Walls,Refundable_deposit,Nonrefundable_deposit,Pet_deposit,TenantTID")] Move_in move_in)
        {
            var mitenant = from t in _context.Tenant where t.TID == move_in.TenantTID select t;
            var miunit = from u in _context.Unit where u.UID == move_in.UnitUID select u;

            ViewData["TenantTID"] = new SelectList(mitenant, "TID", "Last_name");
            ViewData["UnitUID"] = new SelectList(miunit, "UID", "Unit_Number");

            if (id != move_in.MIID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var move_out = from m in _context.Move_out where m.TenantTID == move_in.TenantTID select m;
                    if (move_out != null)
                    {
                        ViewBag.Message = "Unable to change move in date. Tenant moved out.";
                        return View(move_in);
                    }
                    if (move_in.Date > DateTime.Today)
                    {
                        ViewBag.Message = "Unable to change move in to future date. Withdraw move in.";
                        return View(move_in);
                    }
                    var tenant = await _context.Tenant.FindAsync(move_in.TenantTID);
                    tenant.Lease_start_date = move_in.Date;

                    _context.Update(tenant);
                    _context.Update(move_in);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Move_inExists(move_in.MIID))
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

            return View(move_in);
        }

        // GET: MoveIn/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var move_in = await _context.Move_in
                .Include(m => m.Tenant)
                .Include(m => m.Unit)
                .FirstOrDefaultAsync(m => m.MIID == id);
            if (move_in == null)
            {
                return NotFound();
            }

            return View(move_in);
        }

        // POST: MoveIn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var move_in = await _context.Move_in.FindAsync(id);
            var move_out = from m in _context.Move_out where m.TenantTID == move_in.TenantTID select m;

            foreach (var m in move_out)
            {
                if (m != null)
                {
                    ViewBag.Message = "Unable to withdraw! Tenant moved out.";
                    return View(move_in);
                }
            }

            var unit = await _context.Unit.FindAsync(move_in.UnitUID);
            unit.Occupied = "No";
            unit.Ready_to_rent = "No";

            var tenant = await _context.Tenant.FindAsync(move_in.TenantTID);
            tenant.Current = "No";
            _context.Tenant.Update(tenant);

            _context.Unit.Update(unit);
            _context.Move_in.Remove(move_in);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Move_inExists(int id)
        {
            return _context.Move_in.Any(e => e.MIID == id);
        }

        public async Task<IActionResult> pickTenant(int UID)
        {
            var futureTenants = from t in _context.Tenant
                                where t.Lease_start_date >= DateTime.Today
                                select t;
            var currentTenants = from mi in _context.Move_in
                                 join t in _context.Tenant
                                 on mi.TenantTID equals t.TID into temp
                                 from lj in temp.DefaultIfEmpty()
                                 select lj;

            var motenant = futureTenants.Where(p => currentTenants.All(p2 => p2.TID != p.TID));
            var tenant = from m in motenant
                         select new TenantViewModel
                         {
                             TID = m.TID,
                             Last_name = m.Last_name,
                             First_name = m.First_name,
                             UnitUID = UID,
                             Lease_start_date = m.Lease_start_date,
                             Lease_end_date = m.Lease_end_date,
                             Email = m.Email,
                             Phone = m.Phone
                         };

            return View(await tenant.ToListAsync());
        }


    }
}
