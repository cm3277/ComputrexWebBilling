using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBilling.Models
{
    public class Tolerances
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string name { get; set; }

        [Display(Name = "Min")]
        public double min { get; set; }

        [Display(Name = "Max")]
        public double max { get; set; }

        [Display(Name = "Notes")]
        public string notes { get; set; }
    }
}