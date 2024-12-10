using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIADatabase.Areas.JediArchives;
using CIADatabase.Models;

namespace CIADatabase.Areas.JediArchives.Controllers
{
    public class JediSectionsController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: JediArchives/JediSections
        public ActionResult Index()
        {
            return View(db.JediSections.ToList());
        }

        // GET: JediArchives/JediSections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediSection jediSection = db.JediSections.Find(id);
            if (jediSection == null)
            {
                return HttpNotFound();
            }
            return View(jediSection);
        }

        // GET: JediArchives/JediSections/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: JediArchives/JediSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectionId,Title")] JediSection jediSection)
        {
            if (ModelState.IsValid)
            {
                db.JediSections.Add(jediSection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jediSection);
        }

        // GET: JediArchives/JediSections/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediSection jediSection = db.JediSections.Find(id);
            if (jediSection == null)
            {
                return HttpNotFound();
            }
            return View(jediSection);
        }

        // POST: JediArchives/JediSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectionId,Title")] JediSection jediSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jediSection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jediSection);
        }

        // GET: JediArchives/JediSections/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediSection jediSection = db.JediSections.Find(id);
            if (jediSection == null)
            {
                return HttpNotFound();
            }
            return View(jediSection);
        }

        // POST: JediArchives/JediSections/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JediSection jediSection = db.JediSections
                .Include(s => s.Articles)
                .SingleOrDefault(s => s.SectionId == id);

            if (jediSection == null)
            {
                return HttpNotFound();
            }

            if (jediSection.Articles != null && jediSection.Articles.Any())
            {
                db.JediArticles.RemoveRange(jediSection.Articles);
            }

            db.JediSections.Remove(jediSection);

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
