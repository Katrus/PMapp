using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PMApp.Data;
using PMApp.Models;

namespace PMApp.Controllers
{
    public class GeneralTaxReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GeneralTaxReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GeneralTaxReport
        public async Task<IActionResult> Index(string searchString)
        {
            var applicationDbContext = from m in _context.Tax_Parcels.Include(t => t.Building)
                                       group m by m.Year into a
                                       select (new Tax_Parcel { amount = a.Sum(x => x.amount), Year = a.Key });

                

           if (!String.IsNullOrEmpty(searchString))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Year.ToString().Contains(searchString));
            } 

            return View(await applicationDbContext.ToListAsync());
        }

    
        private bool Tax_ParcelExists(int id)
        {
            return _context.Tax_Parcels.Any(e => e.Id == id);
        }
    }
}
