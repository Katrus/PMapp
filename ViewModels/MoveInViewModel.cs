using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class MoveInViewModel
    {
        public int Unit { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Move-in date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Carpet { get; set; }

        public string Appliances { get; set; }

        public string Walls { get; set; }

        [Display(Name = "Refundable deposit")]
        public float? Refundable_deposit { get; set; }

        [Display(Name = "Non-refundable deposit")]
        public float? Nonrefundable_deposit { get; set; }

        [Display(Name = "Pet deposit")]
        public float? Pet_deposit { get; set; }

        [Display(Name = "Tenant")]
        public string Last_name { get; set; }

        public string Property { get; set; }


    }
}

