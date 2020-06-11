using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class PdfFile
    {
        [Key]
        public int Id { get; set; }

        public int? TID { get; set; }

        public string FileName { get; set; }
    }
}
