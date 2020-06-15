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
    public class Repair_HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Repair_HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repair_History
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from m in _context.Repair_History.Include(r => r.Contractor).Include(r => r.Unit)
                                       join b in _context.Buildings on m.Unit.BuildingId equals b.BuildingId into temp 
                                       from lj in temp.DefaultIfEmpty()
                                       select new RepairsViewModel {
                                           RHID = m.RHID,
                                           Contractor = m.Contractor.Company_name,
                                           Unit = m.Unit.Unit_Number,
                                           Cost = m.Cost,
                                           Property = lj.Org_name,
                                           Ticket_opened = m.Ticket_opened,
                                           Ticket_closed = m.Ticket_closed,
                                           Work_description = m.Work_description,
                                           Work_started = m.Work_started,
                                           Work_ended = m.Work_ended,
                                           Date_due = m.Date_due

                                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Property.Contains(searchString) 
                                          || s.Unit.ToString().Contains(searchString)
                                          || s.Contractor.Contains(searchString));
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Repair_History/Details/5
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

        // GET: Repair_History/Create
        public IActionResult Create(long BuildingId)
        {
            var units = from u in _context.Unit where u.BuildingId == BuildingId select u;

            ViewData["ContractorCID"] = new SelectList(_context.Contractor, "CID", "Company_name");
            ViewData["UnitUID"] = new SelectList(units, "UID", "Unit_Number");
            return View();
        }

        // POST: Repair_History/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RHID,Cost,Work_description,Ticket_opened,Date_due,Ticket_closed,Work_started,Work_ended,UnitUID,ContractorCID")] Repair_History repair_History)
        {
            var units = from u in _context.Unit where u.UID == repair_History.UnitUID select u;
            ViewData["UnitUID"] = new SelectList(units, "UID", "Unit_Number");
            if (ModelState.IsValid)
            {
               
                _context.Add(repair_History);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractorCID"] = new SelectList(_context.Contractor, "CID", "Company_name", repair_History.ContractorCID);
            return View(repair_History);
        }

        // GET: Repair_History/Edit/5
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

            var units = from u in _context.Unit where u.UID == repair_History.UnitUID select u;
            var contractors = from c in _context.Contractor where c.CID == repair_History.ContractorCID select c;

            ViewData["ContractorCID"] = new SelectList(contractors, "CID", "Company_name");
            ViewData["UnitUID"] = new SelectList(units, "UID", "Unit_Number");
            return View(repair_History);
        }

        // POST: Repair_History/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RHID,Cost,Work_description,Ticket_opened,Date_due,Ticket_closed,Work_started,Work_ended,UnitUID,ContractorCID")] Repair_History repair_History)
        {
            if (id != repair_History.RHID)
            {
                return NotFound();
            }

            var units = from u in _context.Unit where u.UID == repair_History.UnitUID select u;
            var contractors = from c in _context.Contractor where c.CID == repair_History.ContractorCID select c;

            ViewData["ContractorCID"] = new SelectList(contractors, "CID", "Company_name");
            ViewData["UnitUID"] = new SelectList(units, "UID", "Unit_Number");

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
            
            return View(repair_History);
        }

        // GET: Repair_History/Delete/5
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

        // POST: Repair_History/Delete/5
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

        public async Task<IActionResult> selectBuilding(string searchString)
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

        public async Task<IActionResult> ContractorRepairs(int id)
        {
            var applicationDbContext = from m in _context.Repair_History.Include(r => r.Contractor).Include(r => r.Unit)
                                       where m.ContractorCID == id
                                       join b in _context.Buildings on m.Unit.BuildingId equals b.BuildingId into temp
                                       from lj in temp.DefaultIfEmpty()
                                       select new RepairsViewModel
                                       {
                                           RHID = m.RHID,
                                           Contractor = m.Contractor.Company_name,
                                           Unit = m.Unit.Unit_Number,
                                           Cost = m.Cost,
                                           Property = lj.Org_name,
                                           Ticket_opened = m.Ticket_opened,
                                           Ticket_closed = m.Ticket_closed,
                                           Work_description = m.Work_description,
                                           Work_started = m.Work_started,
                                           Work_ended = m.Work_ended,
                                           Date_due = m.Date_due

                                       };

            var contractor = await _context.Contractor.FindAsync(id);
            ViewBag.Contractor = contractor.Company_name;
            ViewBag.CID = contractor.CID;

            return View(await applicationDbContext.ToListAsync());
        }
    }
}
