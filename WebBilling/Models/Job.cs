using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBilling.Models
{
    public class Job
    {
        public int ID { get; set; }

        [Display(Name = "Starting Tech")]
        public string startingTech { get; set; }

        [Display(Name = "Customer")]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Display(Name = "Start Time")]
        public DateTime startTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime? endTime { get; set; }

        [Display(Name = "Total Price")]
        public double totalPrice { get; set; }

        [Display(Name = "In Progress")]
        public bool inProgress { get; set; }

        [Display(Name = "Billed")]
        public bool wasBilled { get; set; }

        [Display(Name = "Notes to Billing supervisor")]
        public string billingNotes { get; set; }

        public virtual ICollection<NarrationLine> narrationLines { get; set; }

        public virtual ICollection<TechNotesLine> techNotesLines { get; set; }

        public virtual ICollection<Parts> parts { get; set; }
    }
}