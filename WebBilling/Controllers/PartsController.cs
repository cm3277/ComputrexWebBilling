using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CSGOSideLoungeMVCTest4.DAL;
using WebBilling.Models;
using WebBilling.ActiveDirectoryHelpers;

namespace WebBilling.Controllers
{
    public class PartsController : Controller
    {
        private CompXDbContext db = new CompXDbContext();

        // GET: Parts
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> Index()
        {
            var parts = db.Parts.Include(p => p.Job);
            return View(await parts.ToListAsync());
        }

        // GET: Parts/Details/5
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parts parts = await db.Parts.FindAsync(id);
            if (parts == null)
            {
                return HttpNotFound();
            }
            return View(parts);
        }

        // GET: Parts/Create
        [AuthorizeAD(Groups = "Admins,Boss")]
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech");
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> Create([Bind(Include = "ID,description,quantity,purchasedPrice,soldPrice,JobID,techName")] Parts parts)
        {
            if (ModelState.IsValid)
            {
                db.Parts.Add(parts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", parts.JobID);
            return View(parts);
        }

        // GET: Parts/Edit/5
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parts parts = await db.Parts.FindAsync(id);
            if (parts == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", parts.JobID);
            return View(parts);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> Edit([Bind(Include = "ID,description,quantity,purchasedPrice,soldPrice,JobID,techName")] Parts parts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", parts.JobID);
            return View(parts);
        }

        // GET: Parts/Delete/5
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parts parts = await db.Parts.FindAsync(id);
            if (parts == null)
            {
                return HttpNotFound();
            }
            return View(parts);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Parts parts = await db.Parts.FindAsync(id);
            db.Parts.Remove(parts);
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
    }
}
