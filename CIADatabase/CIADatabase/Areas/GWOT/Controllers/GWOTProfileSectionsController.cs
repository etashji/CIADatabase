using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIADatabase.Areas.GWOT.Models;
using CIADatabase.Migrations;
using CIADatabase.Models;

namespace CIADatabase.Areas.GWOT.Controllers
{
    public class GWOTProfileSectionsController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: GWOT/GWOTProfileSections
        public ActionResult Index()
        {
            return View(db.GWOTProfileSections.ToList());
        }

        // GET: GWOT/GWOTProfileSections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTProfileSections gWOTProfileSections = db.GWOTProfileSections.Find(id);
            if (gWOTProfileSections == null)
            {
                return HttpNotFound();
            }
            return View(gWOTProfileSections);
        }

        // GET: GWOT/GWOTProfileSections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GWOT/GWOTProfileSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "SectionId,Title")] GWOTProfileSections gWOTProfileSections)
        {
            if (ModelState.IsValid)
            {
                db.GWOTProfileSections.Add(gWOTProfileSections);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gWOTProfileSections);
        }

        // GET: GWOT/GWOTProfileSections/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTProfileSections gWOTProfileSections = db.GWOTProfileSections.Find(id);
            if (gWOTProfileSections == null)
            {
                return HttpNotFound();
            }
            return View(gWOTProfileSections);
        }

        // POST: GWOT/GWOTProfileSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "SectionId,Title")] GWOTProfileSections gWOTProfileSections)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gWOTProfileSections).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gWOTProfileSections);
        }

        // GET: GWOT/GWOTProfileSections/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTProfileSections gWOTProfileSections = db.GWOTProfileSections.Find(id);
            if (gWOTProfileSections == null)
            {
                return HttpNotFound();
            }
            return View(gWOTProfileSections);
        }

        // POST: GWOT/GWOTProfileSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            GWOTProfileSections gWOTProfileSections = db.GWOTProfileSections.Find(id);
            if (gWOTProfileSections.Profiles != null && gWOTProfileSections.Profiles.Any())
            {
                db.GWOTProfiles.RemoveRange(gWOTProfileSections.Profiles); // Assuming GWOTArticles is the DbSet for articles
            }

            db.GWOTProfileSections.Remove(gWOTProfileSections);
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
