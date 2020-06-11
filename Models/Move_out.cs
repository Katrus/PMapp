using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Move_out
    {
        [Key]
        public int MOID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Move-out date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [MaxLength(100)]
        public string Carpet { get; set; }

        [MaxLength(100)]
        public string Appliances { get; set; }

        [MaxLength(100)]
        public string Walls { get; set; }

        [Display(Name = "Cleaning fee")]
        public float? Cleaning_fee { get; set; }

        [Display(Name = "Damage fee")]
        public float? Damage_fee { get; set; }

        [Display(Name = "Fees paid")]
        public float? fees_paid { get; set; }

        [Display(Name ="Tenant")]
        public int TenantTID { get; set; }

        public virtual Tenant Tenant { get; set; }

        [Display(Name ="Unit")]
        public int UnitUID { get; set; }

        public virtual Unit Unit { get; set; }

   

    }
}
