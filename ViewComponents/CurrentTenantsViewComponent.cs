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
    public class CurrentTenantsViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CurrentTenantsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tenants = from t in await _context.Tenant.ToListAsync()
                          where t.Current.Equals("Yes")
                          join r in _context.Move_in on t.TID
                          equals r.TenantTID into temp
                          from lj in temp.DefaultIfEmpty()
                          join u in _context.Unit on lj.UnitUID equals u.UID into temp2
                          from lj2 in temp2.DefaultIfEmpty()
                          join b in _context.Buildings on lj2.BuildingId equals b.BuildingId into temp3
                          from lj3 in temp3.DefaultIfEmpty() select new TenantViewModel
                          {
                              TID = t.TID,
                              Last_name = t.Last_name,
                              First_name = t.First_name,
                              Lease_start_date = t.Lease_start_date,
                              Lease_end_date = t.Lease_end_date,
                              Property = lj3.Org_name,
                              Unit = lj2.Unit_Number,
                              Email = t.Email,
                          };
        

            return View(tenants);
        }
    }
}
