using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PMApp.ViewModels
{
    public class TenantsTabViewModel
    {
        public Tab ActiveTab { get; set; }
    }

    public enum Tab
    {
        Future,
        Current,
        Past
    }

}
