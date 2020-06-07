using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Move_in
    {
        [Key]
        public int MIID { get; set; }

        [Display(Name ="Unit")]
        public int UnitUID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Move-in date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Carpet { get; set; }

        public string Appliances { get; set; }

        public string Walls { get; set; }

        [Display(Name = "Refundable deposit")]
        public float Refundable_deposit { get; set; }

        [Display(Name = "Non-refundable deposit")]
        public float Nonrefundable_deposit { get; set; }

        [Display(Name = "Pet deposit")]
        public float Pet_deposit { get; set; }

        [Display(Name ="Tenant")]
        public int TenantTID { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Unit Unit { get; set; }
    }
}
