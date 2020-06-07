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
    public class MoveOutReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoveOutReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MoveOutReport
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Move_out.Include(m => m.Tenant).Include(m => m.Unit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MoveOutReport/Details/5
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

        // GET: MoveOutReport/Create
        public IActionResult Create()
        {
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name");
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID");
            return View();
        }

        // POST: MoveOutReport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MOID,Date,Carpet,Appliances,Walls,Cleaning_fee,Damage_fee,fees_paid,TenantTID,UnitUID")] Move_out move_out)
        {
            if (ModelState.IsValid)
            {
                _context.Add(move_out);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", move_out.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", move_out.UnitUID);
            return View(move_out);
        }

        // GET: MoveOutReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var move_out = await _context.Move_out.FindAsync(id);
            if (move_out == null)
            {
                return NotFound();
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", move_out.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", move_out.UnitUID);
            return View(move_out);
        }

        // POST: MoveOutReport/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MOID,Date,Carpet,Appliances,Walls,Cleaning_fee,Damage_fee,fees_paid,TenantTID,UnitUID")] Move_out move_out)
        {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", move_out.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", move_out.UnitUID);
            return View(move_out);
        }

        // GET: MoveOutReport/Delete/5
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

        // POST: MoveOutReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var move_out = await _context.Move_out.FindAsync(id);
            _context.Move_out.Remove(move_out);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Move_outExists(int id)
        {
            return _context.Move_out.Any(e => e.MOID == id);
        }
    }
}
