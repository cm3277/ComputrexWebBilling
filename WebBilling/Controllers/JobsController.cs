using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBilling.Models;
using CSGOSideLoungeMVCTest4.DAL;
using System.Net.Mail;
using WebBilling.ActiveDirectoryHelpers;

namespace WebBilling.Controllers
{
    public class JobsController : Controller
    {
        private CompXDbContext db = new CompXDbContext();

        // GET: /Jobs/
        public async Task<ActionResult> Index(string type, string customerName)
        {
            var jobs = db.Jobs.Where(j => j.inProgress == true).Include(j => j.Customer);
            if (type == null)
                type = "current";
            else if (type.Equals("tobebilled"))
                jobs = db.Jobs.Where(j => j.inProgress == false && j.wasBilled == false).Include(j => j.Customer);
            else if (type.Equals("archived"))
                jobs = db.Jobs.Where(j => j.inProgress == false && j.wasBilled == true).Include(j => j.Customer);
            else if (type.Equals("customer") && !string.IsNullOrWhiteSpace(customerName))
            {
                jobs = db.Jobs.Where(j => j.Customer.name == customerName).Include(j => j.Customer);
            }
            ViewBag.type = type;
            return View(await jobs.ToListAsync());
        }

        // GET: /Jobs/Details/5
        public async Task<ActionResult> BillInfo(int? id, string urlType)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            int realID = job.ID;
            ViewBag.billingInfo = calculateBillingInfo(realID);
            ViewBag.type = urlType;
            return View(job);
        }

        // POST: /Jobs/BillInfo
        [HttpPost]
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> BillInfoFinish(int jobID)
        {
            Job job = await db.Jobs.FindAsync(jobID);
            if (job == null)
            {
                return Json(new { Success = false });
            }
            job.wasBilled = true;

            db.Entry(job).State = EntityState.Modified;
            await db.SaveChangesAsync();
            string url = Url.Action("Index", new { type = "tobebilled" });
            return Json(new { redirect = url });
        }

        // GET: /Jobs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: /Jobs/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "name").OrderBy(c => c.Text);
            return View();
        }

        // POST: /Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ID,CustomerID")] Job job)
        {
            //Auto fill data on job creation
            job.startingTech = User.Identity.Name;
            job.startTime = DateTime.Now;
            job.totalPrice = 0;
            job.inProgress = true;
            job.wasBilled = false;

            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                await db.SaveChangesAsync();
                //return RedirectToAction("Index");
                return RedirectToAction("Edit", new { id = job.ID });
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "name", job.CustomerID);
            return View(job);
        }

        // GET: /Jobs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "name", job.CustomerID);
            return View(job);
        }

        // POST: /Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ID,startingTech,CustomerID,startTime,endTime,totalPrice,isFinished")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "ID", "name", job.CustomerID);
            //ViewBag.elapsedTime = (DateTime.Now - job.startTime).TotalHours;
            return View(job);
        }

        // GET: /Jobs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: /Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            db.Jobs.Remove(job);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteNarrationLine(int narID)
        {
            NarrationLine narrationLine = await db.NarrationLines.FindAsync(narID);
            db.NarrationLines.Remove(narrationLine);
            await db.SaveChangesAsync();
            return Json(new { Success = true });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTechLine(int techID)
        {
            TechNotesLine techLine = await db.TechNotesLines.FindAsync(techID);
            db.TechNotesLines.Remove(techLine);
            await db.SaveChangesAsync();
            return Json(new { Success = true });
        }

        [HttpPost]
        public async Task<ActionResult> DeletePartLine(int partID)
        {
            Parts part = await db.Parts.FindAsync(partID);
            db.Parts.Remove(part);
            await db.SaveChangesAsync();
            return Json(new { Success = true });
        }

        [HttpPost]
        public async Task<ActionResult> EditNarrationLine(int narID, string narLineText)
        {
            if (string.IsNullOrWhiteSpace(narLineText))
                return Json(new { Success = false });
            NarrationLine narLine = await db.NarrationLines.FindAsync(narID);
            if (narLine == null)
                return Json(new { Success = false });
            narLine.line = narLineText;
            db.Entry(narLine).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Json(new { Success = true });
        }

        [HttpPost]
        public async Task<ActionResult> EditTechLine(int techID, string techLineText)
        {
            if (string.IsNullOrWhiteSpace(techLineText))
                return Json(new { Success = false });
            TechNotesLine techLine = await db.TechNotesLines.FindAsync(techID);
            if (techLine == null)
                return Json(new { Success = false });
            techLine.line = techLineText;
            db.Entry(techLine).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Json(new { Success = true });
        }

        [HttpPost]
        public async Task<ActionResult> AddNarrationLine(int jobID, string narLineText)
        {
            if (string.IsNullOrWhiteSpace(narLineText))
                return Json(new { Success = false });
            NarrationLine narLine = new NarrationLine();
            narLine.JobID = jobID;
            narLine.line = narLineText;
            db.NarrationLines.Add(narLine);
            await db.SaveChangesAsync();
            //return new id for the narLine
            return Json(new { Success = true, newID = narLine.ID });
        }

        [HttpPost]
        public async Task<ActionResult> AddTechLine(int jobID, string techLineText)
        {
            if (string.IsNullOrWhiteSpace(techLineText))
                return Json(new { Success = false });
            TechNotesLine techLine = new TechNotesLine();
            techLine.JobID = jobID;
            techLine.line = techLineText;
            techLine.techName = User.Identity.Name;
            db.TechNotesLines.Add(techLine);
            await db.SaveChangesAsync();
            //return new id for the narLine
            return Json(new { Success = true, newID = techLine.ID });
        }

        [HttpPost]
        public async Task<ActionResult> AddPart(int jobID, string partDescription, int quantity, double originalPrice)
        {
            if (string.IsNullOrWhiteSpace(partDescription))
                return Json(new { Success = false, error = "No part description" });
            if (quantity <= 0)
                return Json(new { Success = false, error = "Illegal quantity" });
            if (originalPrice < 0)
                return Json(new { Success = false, error = "Illegal price" });
            
            Parts part = new Parts();
            part.JobID = jobID;
            part.description = partDescription;
            part.quantity = quantity;
            part.techName = User.Identity.Name;
            part.modifyPrice(originalPrice, db);
            db.Parts.Add(part);
            await db.SaveChangesAsync();

            return Json(new { Success = true, newID = part.ID });
        }

        [HttpPost]
        public async Task<ActionResult> SaveBillNotes(int jobID, string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
                return Json(new { Success = false });
            Job job = await db.Jobs.FindAsync(jobID);
            if (job == null)
            {
                return Json(new { Success = false, error = "Bad ID" });
            }
            job.billingNotes = notes.Trim();

            db.Entry(job).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(new { Success = true });
        }

        [HttpPost]
        public async Task<ActionResult> FinishJobInvoice(int jobID)
        {
            Job job = await db.Jobs.FindAsync(jobID);
            if (job == null)
            {
                return Json(new { Success = false, error = "Bad ID" });
            }
            if (!job.inProgress)
                return Json(new { Success = false, error = "Invoice already submitted" });
            if (job.narrationLines.Count == 0)
                return Json(new { Success = false, error = "No narration" });
            if (job.CustomerID == null)
                return Json(new { Success = false, error = "No Customer" });
            if (job.startTime == null)
                return Json(new { Success = false, error = "No start time" });
            job.endTime = DateTime.Now;

            
            //Send email
            /*string emailSubject = job.Customer.name + ":" + job.startTime.Month + "/" + job.startTime.Day + "/" + job.startTime.Year + ":INVOICE";
            string emailBody = calculateBillingInfo(jobID);*/
            
            job.inProgress = false;
            db.Entry(job).State = EntityState.Modified;
            await db.SaveChangesAsync();
            //send 
            //if (sendEmail(emailSubject, emailBody))
            //return RedirectToAction("Index");
               //return Json(new { Success = true });
            //else
                //return Json(new { Success = false, error = "Could not send email" });
            string url = Url.Action("Index");
            return Json(new { redirect = url });
        }

        private string calculateBillingInfo(int jobID)
        {
            Job job = db.Jobs.Find(jobID);
            if (job == null)
            {
                return "Error bad ID";
            }
            string emailBody = "";
            emailBody += job.startTime.Month + "/" + job.startTime.Day;
            //calculate hours
            TimeSpan elapsedTime = job.endTime.Value - job.startTime;
            double hours = 0;
            List<Tolerances> hoursTolerances = db.Tolerances.Where(tol => tol.name == Global_Vars.GlobalVars.perHourTimeName).ToList();
            double timeMax = Global_Vars.GlobalVars.perHourTimeMaxFallback;
            if (hoursTolerances.Count > 0)
            {
                timeMax = hoursTolerances[0].max;
            }
            if (elapsedTime.TotalHours < 1 || elapsedTime.TotalMinutes <= timeMax)
                hours = 1;
            else
            {
                double percentOfHour = (timeMax - 60) / 60;
                double percentOfHalfHour = (8 - 60) / 60;
                double timeOverHour = elapsedTime.TotalHours - (int)elapsedTime.TotalHours;
                hours = (int)elapsedTime.TotalHours;
                if (timeOverHour > percentOfHour)
                {
                    if ((timeOverHour - 0.5f) > percentOfHalfHour)
                        hours += 1;
                    else
                        hours += 0.5f;
                }
                else if (timeOverHour > percentOfHalfHour)
                    hours += 0.5f;
            }
            //end calculate hours
            DateTime roundedStartTime = RoundToNearest(job.startTime, new TimeSpan(0, 15, 0));
            DateTime roundedEndTime = roundedStartTime.AddHours(hours);
            emailBody += " - " + job.startingTech + " ( " + roundedStartTime.ToShortTimeString() + " - " + roundedEndTime.ToShortTimeString() + " ) ";
            
            foreach(NarrationLine narLine in job.narrationLines)
            {
                emailBody += narLine.line.Trim();
                if (!narLine.line.Trim().Substring(narLine.line.Trim().Length - 1).Equals("."))
                    emailBody += ". ";
                else
                    emailBody += " ";
                //emailBody += Environment.NewLine;
            }
            emailBody += Environment.NewLine + Environment.NewLine + "End of narration" + Environment.NewLine + Environment.NewLine;

            //Add hours + parts to email
            emailBody += "Total Billing Hours: ";
           
            emailBody += hours + Environment.NewLine + Environment.NewLine;
            //parts
            emailBody += "Parts List" + Environment.NewLine;
            if (job.parts.Count == 0)
                emailBody += "No parts." + Environment.NewLine;
            double totalPartsPrice = 0;
            foreach(Parts part in job.parts)
            {
                emailBody += "Qty: " + part.quantity + " \t " + part.description + " \t $" + part.soldPrice + " total." + Environment.NewLine;
                totalPartsPrice += part.soldPrice;
            }
            if (totalPartsPrice > 0)
                emailBody += Environment.NewLine + "Total parts price marked up: $" + totalPartsPrice + Environment.NewLine;
            //total price
            double pricePerHour = 0;
            if (job.Customer.businessPricing)
            {
                emailBody += "Total hour price (Business Pricing): $";
                pricePerHour = Global_Vars.GlobalVars.businessPricingFallback;
                List<Tolerances> businessRate = db.Tolerances.Where(tol => tol.name == Global_Vars.GlobalVars.perHourBusinessName).ToList();
                if (businessRate.Count > 0)
                    pricePerHour = businessRate[0].max;
            }
            else
            {
                emailBody += "Total hour price (Home Pricing): $";
                pricePerHour = Global_Vars.GlobalVars.homePricingFallback;
                List<Tolerances> homeRate = db.Tolerances.Where(tol => tol.name == Global_Vars.GlobalVars.perHourHomeName).ToList();
                if (homeRate.Count > 0)
                    pricePerHour = homeRate[0].max;
            }
            double totalHourPrice = hours * pricePerHour;
            emailBody += totalHourPrice;
            double totalPrice = totalHourPrice + totalPartsPrice;
            job.totalPrice = totalPrice;
            //Save total price
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();

            emailBody += Environment.NewLine + "Total price: $" + totalPrice;
            emailBody += Environment.NewLine + Environment.NewLine;

            if (!string.IsNullOrWhiteSpace(job.billingNotes))
                emailBody += "Notes to billing supervisor:" + Environment.NewLine + job.billingNotes + Environment.NewLine;

            return emailBody;
        }

        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            var delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
            return new DateTime(dt.Ticks + delta, dt.Kind);
        }

        public static DateTime RoundDown(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta, dt.Kind);
        }

        public static DateTime RoundToNearest(DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            bool roundUp = delta > d.Ticks / 2;

            return roundUp ? RoundUp(dt, d) : RoundDown(dt, d);
        }

        private bool sendEmail(string subject, string body)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress("someone@somedomain.com", "SomeOne"));
            msg.From = new MailAddress("you@yourdomain.com", "You");
            msg.Subject = "This is a Test Mail";
            msg.Body = "This is a test message using Exchange OnLine";
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("your user name", "your password");
            client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
                //lblText.Text = "Message Sent Succesfully";
            }
            catch (Exception ex)
            {
                //lblText.Text = ex.ToString();
                return false;
            }
            return true;
        }
    }
}
