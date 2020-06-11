using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class MoveOutViewModel
    {
        public int Unit { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Move-in date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Carpet { get; set; }

        public string Appliances { get; set; }

        public string Walls { get; set; }

        [Display(Name = "Cleaning fee")]
        public float? Cleaning_fee { get; set; }

        [Display(Name = "Damage fee")]
        public float? Damage_fee { get; set; }

        [Display(Name = "Fees Paid")]
        public float? fees_paid { get; set; }

        [Display(Name = "Tenant")]
        public string Last_name { get; set; }

        public string Property { get; set; }

    }
}
