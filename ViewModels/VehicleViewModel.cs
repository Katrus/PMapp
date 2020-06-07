using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class VehicleViewModel
    {
        public int VID { get; set; }

        [Display(Name = "License plate")]
        public string License_plate { get; set; }

        public string Model { get; set; }

        public string Make { get; set; }

        public string Year { get; set; }

        public string Color { get; set; }

        [Display(Name = "Parking stall")]
        public int stall_number { get; set; }

        [Display(Name = "Last name")]
        public string Last_name { get; set; }

        [Display(Name = "First name")]
        public string First_name { get; set; }

        public string Property { get; set; }

        public int Unit { get; set; }

        [Display(Name = "Lease start date")]
        public DateTime Lease_start_date { get; set; }

        [Display(Name = "Lease end date")]
        public DateTime Lease_end_date { get; set; }
    }
}
