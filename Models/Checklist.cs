using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.Models
{
    public class Checklist
    {
        [Key]
        public int checklistId { get; set; }

        [MaxLength(70)]
        public string Reminder { get; set; }

        [DataType(DataType.Date)]
        public Nullable<DateTime> When { get; set; }

    }
}
