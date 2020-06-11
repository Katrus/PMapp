using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Building
    {
        [Key]
        [Display(Name = "Property")]
        public long BuildingId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Property name")]
        public string Org_name { get; set; }

        [Display(Name = "Units")]
        public int? Unit_Count { get; set; }

        [MaxLength(50)]
        public string Street { get; set; }

        [MaxLength(30)]
        public string City { get; set; }

        [MaxLength(10)]
        [Display(Name = "Zip code")]
        public string Zip_code { get; set; }

        [MaxLength(30)]
        public string State { get; set; }

        [Display(Name = "Tax Parcel")]
        public string TPID { get; set; }

        public ICollection<Tenant> Tenants { get; set; }
        public ICollection<Unit> Units { get; set; }

    }
}
