using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Rent
    {
        [Key]
        public int RID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due date")]
        [DataType(DataType.Date)]
        public DateTime Date_due { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date paid")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> Date_paid { get; set; }

        [Display(Name = "Rent amount")]
        public float Rent_amount { get; set; }

        [Display(Name = "Paid amount")]
        public float Amount_paid { get; set; }

        public float Balance
        {
            //calculate amount left to pay
            get { return Rent_amount - Amount_paid; }
            set { } 
        }

        public int TenantTID { get; set; }
        public virtual Tenant Tenant { get; set; }

        public int UnitUID { get; set; }

        public virtual Unit Unit { get; set; }

    }
}
