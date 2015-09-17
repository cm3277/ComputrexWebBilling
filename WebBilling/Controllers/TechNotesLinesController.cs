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

namespace WebBilling.Controllers
{
    public class TechNotesLinesController : Controller
    {
        private CompXDbContext db = new CompXDbContext();

        // GET: TechNotesLines
        public async Task<ActionResult> Index()
        {
            var techNotesLines = db.TechNotesLines.Include(t => t.Job);
            return View(await techNotesLines.ToListAsync());
        }

        // GET: TechNotesLines/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechNotesLine techNotesLine = await db.TechNotesLines.FindAsync(id);
            if (techNotesLine == null)
            {
                return HttpNotFound();
            }
            return View(techNotesLine);
        }

        // GET: TechNotesLines/Create
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech");
            return View();
        }

        // POST: TechNotesLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,JobID,techName,line")] TechNotesLine techNotesLine)
        {
            if (ModelState.IsValid)
            {
                db.TechNotesLines.Add(techNotesLine);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", techNotesLine.JobID);
            return View(techNotesLine);
        }

        // GET: TechNotesLines/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechNotesLine techNotesLine = await db.TechNotesLines.FindAsync(id);
            if (techNotesLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", techNotesLine.JobID);
            return View(techNotesLine);
        }

        // POST: TechNotesLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,JobID,techName,line")] TechNotesLine techNotesLine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(techNotesLine).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", techNotesLine.JobID);
            return View(techNotesLine);
        }

        // GET: TechNotesLines/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TechNotesLine techNotesLine = await db.TechNotesLines.FindAsync(id);
            if (techNotesLine == null)
            {
                return HttpNotFound();
            }
            return View(techNotesLine);
        }

        // POST: TechNotesLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TechNotesLine techNotesLine = await db.TechNotesLines.FindAsync(id);
            db.TechNotesLines.Remove(techNotesLine);
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
