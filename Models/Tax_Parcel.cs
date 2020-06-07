using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Tax_Parcel
    {
        [Key]
        public int Id { get; set; }

        public int Year { set; get; }

        public long BuildingId { get; set; }

        public Building Building { get; set; }

        [Display(Name = "Amount")]
        public float amount { set; get; }

    }
}
