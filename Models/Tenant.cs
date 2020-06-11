using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Tenant
    {
        [Key]
        [Display(Name ="Tenant ID")]
        public int TID { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string Last_name { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string First_name { get; set; }

        public string Employer { get; set; }

        public int? Salary { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Lease start date")]
        [DataType(DataType.Date)]
        public DateTime Lease_start_date { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Lease end date")]
        [DataType(DataType.Date)]
        public DateTime Lease_end_date { get; set; }

        public int? ReservedUnit { get; set; }

        public string Phone { get; set; }

        public string Email {get; set; }

        public string Pets { get; set; }

        public string Current { get; set; }

        public ICollection<Infractions> Infractions { get; set; }
        
        public ICollection<Vehicle> Vehicles { get; set; }

        public ICollection<Rent> Rents { get; set; }

    }
}
