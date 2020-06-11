using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Contractor
    {
        [Required]
        [Key]
        public int CID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Vendor")]
        public string Company_name { get; set; }

        [MaxLength(50)]
        [Display(Name = "Contact person")]
        public string Contact_name { get; set; }

        [MaxLength(50)]
        [Display(Name ="Category")]
        public string Specialty { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Street { get; set; }

        [MaxLength(30)]
        public string City { get; set; }

        [MaxLength(15)]
        [Display(Name = "Zip code")]
        public string Zip_code { get; set; }

        [MaxLength(25)]
        public string State { get; set; }

    }
}
