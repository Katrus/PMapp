using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class RentViewModel
    {
        public int RID { get; set; }

        [Display(Name = "Due date")]
        public DateTime Date_due { get; set; }

        [Display(Name = "Date paid")]
        public Nullable<DateTime> Date_paid { get; set; }

        [Display(Name = "Rent amount")]
        public float Rent_amount { get; set; }

        [Display(Name = "Pet fee")]
        public float Pet_fee { get; set; }

        [Display(Name = "Parking fee")]
        public float Parking_fee { get; set; }

        public int TenantTID { get; set; }

        public int UnitUID { get; set; }

        public string Property { get; set; }

        public int Unit { get; set; }

        [Display(Name = "Last name")]
        public string Last_name { get; set; }

        [Display(Name = "First name")]
        public string First_name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Month { get; set; }

        public int MonthNum { get; set; }

        public int Year { get; set; }

        [Display(Name ="Days Late")]
        public int DaysLate { get; set; }
    }
}
 