using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class TenantViewModel
    {
            public int TID { get; set; }

            [Display(Name = "Last name")]
            public string Last_name { get; set; }

            [Display(Name = "First name")]
            public string First_name { get; set; }

            public string Employer { get; set; }

            public string Current { get; set; }

            public int? Salary { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Display(Name = "Lease start date")]
            public DateTime Lease_start_date { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [DataType(DataType.Date)]
            [Display(Name = "Lease end date")]
            public DateTime Lease_end_date { get; set; }

            public string Property { get; set; }

            public int? Unit { get; set; }

            public int? UnitUID { get; set; }

            public string Phone { get; set; }

            public string Email { get; set; }

            public string Pets { get; set; }

        
    }
}
