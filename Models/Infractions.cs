using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Infractions
    {
        [Key]
        public int IID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Opened")]
        [DataType(DataType.Date)]
        public DateTime Day_opened { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Closed")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Day_closed { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Resolution { get; set; }

        public int TenantTID { get; set; }

        public virtual Tenant Tenant { get; set; }

    }
}
