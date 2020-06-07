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
    public class ContractorsReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractorsReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContractorsReport
        public async Task<IActionResult> Index(string searchString)
        {
            var contractor = from m in _context.Contractor
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                contractor = contractor.Where(s => s.Company_name.Contains(searchString) || s.Contact_name.Contains(searchString) ||
                 s.Specialty.Contains(searchString) || s.Zip_code.Contains(searchString) || s.State.Contains(searchString)
                 || s.Street.Contains(searchString));
            }

            return View(await contractor.ToListAsync());
        }

        
        private bool ContractorExists(int id)
        {
            return _context.Contractor.Any(e => e.CID == id);
        }
    }
}
