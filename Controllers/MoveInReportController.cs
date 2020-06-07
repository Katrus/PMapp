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
    public class MoveInReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoveInReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MoveInReport
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Move_in.Include(m => m.Tenant).Include(m => m.Unit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MoveInReport/Details/5
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

        // GET: MoveInReport/Create
        public IActionResult Create()
        {
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name");
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID");
            return View();
        }

        // POST: MoveInReport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MIID,UnitUID,Date,Carpet,Appliances,Walls,Refundable_deposit,Nonrefundable_deposit,Pet_deposit,TenantTID")] Move_in move_in)
        {
            if (ModelState.IsValid)
            {
                _context.Add(move_in);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", move_in.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", move_in.UnitUID);
            return View(move_in);
        }

        // GET: MoveInReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var move_in = await _context.Move_in.FindAsync(id);
            if (move_in == null)
            {
                return NotFound();
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", move_in.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", move_in.UnitUID);
            return View(move_in);
        }

        // POST: MoveInReport/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MIID,UnitUID,Date,Carpet,Appliances,Walls,Refundable_deposit,Nonrefundable_deposit,Pet_deposit,TenantTID")] Move_in move_in)
        {
            if (id != move_in.MIID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "First_name", move_in.TenantTID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", move_in.UnitUID);
            return View(move_in);
        }

        // GET: MoveInReport/Delete/5
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

        // POST: MoveInReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var move_in = await _context.Move_in.FindAsync(id);
            _context.Move_in.Remove(move_in);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Move_inExists(int id)
        {
            return _context.Move_in.Any(e => e.MIID == id);
        }
    }
}
