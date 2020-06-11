using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Unit
    {  
        [Key]
        public int UID { get; set; }

        [Display(Name = "Unit number")]
        public int Unit_Number { get; set; }

        [Display(Name ="Rent amount")]
        public float Rent_Amount { get; set; }

        [Display(Name ="Building")]
        public long BuildingId { get; set; }

        [Display(Name = "Sqft")]
        public int? Square_footage { get; set; }

        public int? Bedroom { get; set; }

        public float? Bath { get; set; }

        public string Occupied { get; set; }

        public int? ReservedBy { get; set; }

        [Display(Name = "Available to rent")]
        public string Ready_to_rent { get; set; }

        public virtual Building Building { get; set; }

        public ICollection<Repair_History> Repair_Histories { get; set; }
        public ICollection<Move_in> Move_Ins { get; set; }
        public ICollection<Move_out> Move_Outs { get; set; }
        public ICollection<Rent> Rents { get; set; }

    }
}
