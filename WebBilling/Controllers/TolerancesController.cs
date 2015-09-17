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
using WebBilling.ActiveDirectoryHelpers;

namespace WebBilling.Controllers
{
    public class TolerancesController : Controller
    {
        private CompXDbContext db = new CompXDbContext();

        // GET: /Tolerances/
        public async Task<ActionResult> Index()
        {
            return View(await db.Tolerances.ToListAsync());
        }

        // GET: /Tolerances/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tolerances tolerances = await db.Tolerances.FindAsync(id);
            if (tolerances == null)
            {
                return HttpNotFound();
            }
            return View(tolerances);
        }

        // GET: /Tolerances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Tolerances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> Create([Bind(Include="ID,name,min,max,notes")] Tolerances tolerances)
        {
            if (ModelState.IsValid)
            {
                db.Tolerances.Add(tolerances);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tolerances);
        }

        // GET: /Tolerances/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tolerances tolerances = await db.Tolerances.FindAsync(id);
            if (tolerances == null)
            {
                return HttpNotFound();
            }
            return View(tolerances);
        }

        // POST: /Tolerances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> Edit([Bind(Include="ID,name,min,max,notes")] Tolerances tolerances)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tolerances).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tolerances);
        }

        // GET: /Tolerances/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tolerances tolerances = await db.Tolerances.FindAsync(id);
            if (tolerances == null)
            {
                return HttpNotFound();
            }
            return View(tolerances);
        }

        // POST: /Tolerances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeAD(Groups = "Admins,Boss")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tolerances tolerances = await db.Tolerances.FindAsync(id);
            db.Tolerances.Remove(tolerances);
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
