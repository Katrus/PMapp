using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMApp.Data;
using PMApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMApp.ViewModels;

namespace PMApp.ViewComponents
{
    public class FutureTenantsViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public FutureTenantsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tenants = from t in await _context.Tenant.ToListAsync()
                          where t.Current.Equals("New")
                          select new TenantViewModel
                          {
                              TID = t.TID,
                              Last_name = t.Last_name,
                              First_name = t.First_name,
                              Lease_start_date = t.Lease_start_date,
                              Lease_end_date = t.Lease_end_date,
                              Email = t.Email,
                          };

            return View(tenants);
        }
    }
}
