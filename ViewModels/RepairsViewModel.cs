using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class RepairsViewModel
    {
        [Key]
        public int RHID { get; set; }
        public float Cost { get; set; }

        [Display(Name = "Description")]
        public string Work_description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Ticket opened")]
        public DateTime Ticket_opened { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Ticket closed")]
        public Nullable<DateTime> Ticket_closed { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Date_due { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Work started")]
        public Nullable<DateTime> Work_started { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Work completed")]
        public Nullable<DateTime> Work_ended { get; set; }

        public int Unit { get; set; }

        public string Property { get; set; }

        public string Contractor { get; set; }
    }
}
