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

        [Display(Name = "Company")]
        public string Company_name { get; set; }

        [Display(Name = "Contact person")]
        public string Contact_name { get; set; }

        public string Specialty { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        [Display(Name = "Zip code")]
        public string Zip_code { get; set; }

        public string State { get; set; }

    }
}
