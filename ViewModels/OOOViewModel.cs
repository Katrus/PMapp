using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class OOOViewModel
    {

        [Display(Name = "Unit number")]
        public int Unit_Number { get; set; }

        [Display(Name = "Rent amount")]
        public float Rent_Amount { get; set; }

        public string Property { get; set; }

        [Display(Name = "Sqft")]
        public int? Square_footage { get; set; }

        public string Occupied { get; set; }

        [Display(Name = "Available to rent")]
        public string Ready_to_rent { get; set; }

        [Display(Name = "Work description")]
        public string Work_description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Date_due
        {
            get; set;
        }

    }
}
