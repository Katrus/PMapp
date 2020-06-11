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
    public class InfractionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InfractionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Infractions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Infractions.Include(i => i.Tenant);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Infractions/Details/5
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

        // GET: Infractions/Create
        public IActionResult Create(int TID)
        {
            var tenants = from t in _context.Tenant where t.TID == TID select t;
            ViewData["TenantTID"] = new SelectList(tenants, "TID", "Last_name");
            return View();
        }

        // POST: Infractions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IID,Day_opened,Day_closed,Description,Resolution,TenantTID")] Infractions infractions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(infractions);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tenants", new { id = infractions.TenantTID });
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "Last_name", infractions.TenantTID);
            return View(infractions);
        }

        // GET: Infractions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var infractions = await _context.Infractions.FindAsync(id);
            if (infractions == null)
            {
                return NotFound();
            }

            var tenants = from t in _context.Tenant where t.TID == infractions.TenantTID select t;
            ViewData["TenantTID"] = new SelectList(tenants, "TID", "Last_name");
            return View(infractions);
        }

        // POST: Infractions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IID,Day_opened,Day_closed,Description,Resolution,TenantTID")] Infractions infractions)
        {
            if (id != infractions.IID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(infractions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InfractionsExists(infractions.IID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Tenants", new { id = infractions.TenantTID });
            }
            ViewData["TenantTID"] = new SelectList(_context.Tenant, "TID", "Last_name", infractions.TenantTID);
            return View(infractions);
        }

        // GET: Infractions/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Infractions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var infractions = await _context.Infractions.FindAsync(id);
            _context.Infractions.Remove(infractions);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tenants", new { id = infractions.TenantTID });
        }

        private bool InfractionsExists(int id)
        {
            return _context.Infractions.Any(e => e.IID == id);
        }
    }
}
