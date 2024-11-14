using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIADatabase.Areas.GWOT;
using CIADatabase.Models;

namespace CIADatabase.Areas.GWOT.Controllers
{
    public class GWOTSectionsController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: GWOT/GWOTSections
        public ActionResult Index()
        {
            return View(db.GWOTSections.ToList());
        }

        // GET: GWOT/GWOTSections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTSections gWOTSections = db.GWOTSections.Find(id);
            if (gWOTSections == null)
            {
                return HttpNotFound();
            }
            return View(gWOTSections);
        }

        // GET: GWOT/GWOTSections/Create 
        public ActionResult Create()
        {
            return View();
        }

        // POST: GWOT/GWOTSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectionId,Title")] GWOTSections gWOTSections)
        {
            if (ModelState.IsValid)
            {
                db.GWOTSections.Add(gWOTSections);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gWOTSections);
        }

        // GET: GWOT/GWOTSections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTSections gWOTSections = db.GWOTSections.Find(id);
            if (gWOTSections == null)
            {
                return HttpNotFound();
            }
            return View(gWOTSections);
        }

        // POST: GWOT/GWOTSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectionId,Title")] GWOTSections gWOTSections)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gWOTSections).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gWOTSections);
        }

        // GET: GWOT/GWOTSections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTSections gWOTSections = db.GWOTSections.Find(id);
            if (gWOTSections == null)
            {
                return HttpNotFound();
            }
            return View(gWOTSections);
        }

        // POST: GWOT/GWOTSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GWOTSections gWOTSections = db.GWOTSections.Find(id);
            db.GWOTSections.Remove(gWOTSections);
            db.SaveChanges();
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
