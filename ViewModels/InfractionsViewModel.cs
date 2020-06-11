using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class InfractionsViewModel
    {
        public int IID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Opened")]
        public DateTime Day_opened { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Closed")]
        public Nullable<DateTime> Day_closed { get; set; }

        public string Description { get; set; }

        public string Resolution { get; set; }

        [Display(Name ="Last name")]
        public string Last_name{ get; set; }

        [Display(Name ="First name")]
        public string First_name { get; set; }

        public string Property { get; set; }

        public int Unit { get; set; }


    }
}
