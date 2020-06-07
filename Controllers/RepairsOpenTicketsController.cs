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
    public class RepairsOpenTicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepairsOpenTicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RepairsOpenTickets
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from m in _context.Repair_History.Include(r => r.Contractor).Include(r => r.Unit)
                                       where m.Ticket_closed == null 
                                       join b in _context.Buildings on m.Unit.BuildingId equals b.BuildingId into temp
                                       from lj in temp.DefaultIfEmpty() select new RepairsViewModel { 
                                             
                                           Contractor = m.Contractor.Company_name,
                                           Unit = m.Unit.Unit_Number,
                                           Cost = m.Cost,
                                           Property = lj.Org_name,
                                           Ticket_opened = m.Ticket_opened,
                                           Work_description = m.Work_description,
                                           Work_started = m.Work_started,
                                           Work_ended = m.Work_ended
                                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Work_description.Contains(searchString)
                || s.Contractor.Contains(searchString)
                || s.Unit.ToString().Contains(searchString)
                || s.Property.Contains(searchString));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RepairsOpenTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair_History = await _context.Repair_History
                .Include(r => r.Contractor)
                .Include(r => r.Unit)
                .FirstOrDefaultAsync(m => m.RHID == id);
            if (repair_History == null)
            {
                return NotFound();
            }

            return View(repair_History);
        }

        // GET: RepairsOpenTickets/Create
        public IActionResult Create()
        {
            ViewData["ContractorCID"] = new SelectList(_context.Contractor, "CID", "CID");
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID");
            return View();
        }

        // POST: RepairsOpenTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RHID,Cost,Work_description,Ticket_opened,Ticket_closed,Work_started,Work_ended,UnitUID,ContractorCID")] Repair_History repair_History)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repair_History);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractorCID"] = new SelectList(_context.Contractor, "CID", "CID", repair_History.ContractorCID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", repair_History.UnitUID);
            return View(repair_History);
        }

        // GET: RepairsOpenTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair_History = await _context.Repair_History.FindAsync(id);
            if (repair_History == null)
            {
                return NotFound();
            }
            ViewData["ContractorCID"] = new SelectList(_context.Contractor, "CID", "CID", repair_History.ContractorCID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", repair_History.UnitUID);
            return View(repair_History);
        }

        // POST: RepairsOpenTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RHID,Cost,Work_description,Ticket_opened,Ticket_closed,Work_started,Work_ended,UnitUID,ContractorCID")] Repair_History repair_History)
        {
            if (id != repair_History.RHID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repair_History);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Repair_HistoryExists(repair_History.RHID))
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
            ViewData["ContractorCID"] = new SelectList(_context.Contractor, "CID", "CID", repair_History.ContractorCID);
            ViewData["UnitUID"] = new SelectList(_context.Unit, "UID", "UID", repair_History.UnitUID);
            return View(repair_History);
        }

        // GET: RepairsOpenTickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair_History = await _context.Repair_History
                .Include(r => r.Contractor)
                .Include(r => r.Unit)
                .FirstOrDefaultAsync(m => m.RHID == id);
            if (repair_History == null)
            {
                return NotFound();
            }

            return View(repair_History);
        }

        // POST: RepairsOpenTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair_History = await _context.Repair_History.FindAsync(id);
            _context.Repair_History.Remove(repair_History);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Repair_HistoryExists(int id)
        {
            return _context.Repair_History.Any(e => e.RHID == id);
        }
    }
}
