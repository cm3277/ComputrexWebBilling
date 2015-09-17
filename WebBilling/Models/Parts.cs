using CSGOSideLoungeMVCTest4.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBilling.Models
{
    public class Parts
    {
        public int ID { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Quantity")]
        public int quantity { get; set; }

        [Display(Name = "Total purchased price")]
        public double purchasedPrice { get; set; }

        [Display(Name = "Total sold price")]
        public double soldPrice { get; set; }

        [ForeignKey("Job")]
        public int JobID { get; set; }
        public virtual Job Job { get; set; }

        [Display(Name = "Tech Name")]
        public string techName { get; set; }

        public void modifyPrice(double originalPrice, CompXDbContext dbContext)
        {
            //get the parts tolerances from the database
            List<Tolerances> partsTolerances = dbContext.Tolerances.Where(tol => tol.name == Global_Vars.GlobalVars.partsMarkUpTolerance).ToList();
            double partsPercent = Global_Vars.GlobalVars.partsMarkUpPercentFallback;
            double partsMin = Global_Vars.GlobalVars.partsMarkUpMinFallback;
            if (partsTolerances.Count > 0)
            {
                partsPercent = partsTolerances[0].max;
                partsMin = partsTolerances[0].min;
            }

            double markUp = originalPrice * (partsPercent/100);
            if (markUp < partsMin)
                markUp = partsMin;
            this.soldPrice = originalPrice + markUp;
            this.purchasedPrice = originalPrice;
        }

    }
}
