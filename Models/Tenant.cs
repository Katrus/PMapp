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
        [MaxLength(50)]
        [Display(Name = "Last name")]
        public string Last_name { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First name")]
        public string First_name { get; set; }

        [MaxLength(50)]
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

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email {get; set; }

        [MaxLength(100)]
        public string Pets { get; set; }

        public string Current { get; set; }

        public ICollection<Infractions> Infractions { get; set; }
        
        public ICollection<Vehicle> Vehicles { get; set; }

        public ICollection<Rent> Rents { get; set; }

    }
}
