using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBilling.Models
{
    public class TechNotesLine
    {
        public int ID { get; set; }

        [ForeignKey("Job")]
        public int JobID { get; set; }
        public virtual Job Job { get; set; }

        public string techName { get; set; }

        public string line { get; set; }
    }
}
