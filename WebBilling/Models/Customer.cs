using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBilling.Models
{
    public class Customer
    {
        public int ID { get; set; }

        [Display(Name = "Client Name")]
        public string name { get; set; }

        [Display(Name = "Business Pricing")]
        public bool businessPricing { get; set; }
    }
}
