using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Repair_History
    {
        [Key]
        public int RHID { get; set; }

        public float Cost { get; set; }

        [Display(Name = "Description")]
        public string Work_description { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ticket opened")]
        [DataType(DataType.Date)]
        public DateTime Ticket_opened { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ticket closed")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Ticket_closed { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Work started")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Work_started { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Work completed")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Work_ended { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Date_due { get; set; }

        [Display(Name ="Unit")]
        public int UnitUID { get; set; }

        public virtual Unit Unit { get; set; }

        [Display(Name ="Contractor")]
        public int ContractorCID { get; set; }

        public virtual Contractor Contractor { get; set; }

        public ICollection<Contractor> Contractors { get; set; }

    }
}
