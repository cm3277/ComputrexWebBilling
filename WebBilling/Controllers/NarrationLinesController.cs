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
    public class NarrationLinesController : Controller
    {
        private CompXDbContext db = new CompXDbContext();

        // GET: NarrationLines
        public async Task<ActionResult> Index()
        {
            var narrationLines = db.NarrationLines.Include(n => n.Job);
            return View(await narrationLines.ToListAsync());
        }

        // GET: NarrationLines/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NarrationLine narrationLine = await db.NarrationLines.FindAsync(id);
            if (narrationLine == null)
            {
                return HttpNotFound();
            }
            return View(narrationLine);
        }

        // GET: NarrationLines/Create
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech");
            return View();
        }

        // POST: NarrationLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,JobID,line")] NarrationLine narrationLine)
        {
            if (ModelState.IsValid)
            {
                db.NarrationLines.Add(narrationLine);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", narrationLine.JobID);
            return View(narrationLine);
        }

        // GET: NarrationLines/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NarrationLine narrationLine = await db.NarrationLines.FindAsync(id);
            if (narrationLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", narrationLine.JobID);
            return View(narrationLine);
        }

        // POST: NarrationLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,JobID,line")] NarrationLine narrationLine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(narrationLine).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.JobID = new SelectList(db.Jobs, "ID", "startingTech", narrationLine.JobID);
            return View(narrationLine);
        }

        // GET: NarrationLines/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NarrationLine narrationLine = await db.NarrationLines.FindAsync(id);
            if (narrationLine == null)
            {
                return HttpNotFound();
            }
            return View(narrationLine);
        }

        // POST: NarrationLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NarrationLine narrationLine = await db.NarrationLines.FindAsync(id);
            db.NarrationLines.Remove(narrationLine);
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
