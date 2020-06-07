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
    public class ChecklistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChecklistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checklists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Checklist.ToListAsync());
        }

        // GET: Checklists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .FirstOrDefaultAsync(m => m.checklistId == id);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        // GET: Checklists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Checklists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("checklistId,Reminder")] Checklist checklist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checklist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(checklist);
        }

        // GET: Checklists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist.FindAsync(id);
            if (checklist == null)
            {
                return NotFound();
            }
            return View(checklist);
        }

        // POST: Checklists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("checklistId,Reminder")] Checklist checklist)
        {
            if (id != checklist.checklistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checklist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChecklistExists(checklist.checklistId))
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
            return View(checklist);
        }

        // GET: Checklists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .FirstOrDefaultAsync(m => m.checklistId == id);
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        // POST: Checklists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checklist = await _context.Checklist.FindAsync(id);
            _context.Checklist.Remove(checklist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChecklistExists(int id)
        {
            return _context.Checklist.Any(e => e.checklistId == id);
        }
    }
}
