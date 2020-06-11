using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Vehicle
    {
        [Key]
        public int VID { get; set; }

        [Display(Name = "License plate")]
        public string License_plate { get; set; }

        public string Model { get; set; }

        public string Make { get; set; }

        public string Year { get; set; }

        public string Color { get; set; }

        [Display(Name = "Parking stall")]
        public int? stall_number { get; set; }

        [Display(Name = "Tenant")]
        public int TenantTID { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
