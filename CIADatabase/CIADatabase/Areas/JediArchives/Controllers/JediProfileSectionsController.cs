using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIADatabase.Areas.JediArchives.Models;
using CIADatabase.Models;

namespace CIADatabase.Areas.JediArchives.Controllers
{
    public class JediProfileSectionsController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: JediArchives/JediProfileSections
        public ActionResult Index()
        {
            return View(db.JediProfilesSection.ToList());
        }

        // GET: JediArchives/JediProfileSections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediProfileSection jediProfileSection = db.JediProfilesSection.Find(id);
            if (jediProfileSection == null)
            {
                return HttpNotFound();
            }
            return View(jediProfileSection);
        }

        // GET: JediArchives/JediProfileSections/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: JediArchives/JediProfileSections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SectionId,Title")] JediProfileSection jediProfileSection)
        {
            if (ModelState.IsValid)
            {
                db.JediProfilesSection.Add(jediProfileSection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jediProfileSection);
        }

        // GET: JediArchives/JediProfileSections/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediProfileSection jediProfileSection = db.JediProfilesSection.Find(id);
            if (jediProfileSection == null)
            {
                return HttpNotFound();
            }
            return View(jediProfileSection);
        }

        // POST: JediArchives/JediProfileSections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectionId,Title")] JediProfileSection jediProfileSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jediProfileSection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jediProfileSection);
        }

        // GET: JediArchives/JediProfileSections/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediProfileSection jediProfileSection = db.JediProfilesSection.Find(id);
            if (jediProfileSection == null)
            {
                return HttpNotFound();
            }
            return View(jediProfileSection);
        }

        // POST: JediArchives/JediProfileSections/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JediProfileSection jediProfileSection = db.JediProfilesSection
                .Include(s => s.Profiles)
                .SingleOrDefault(s => s.SectionId == id);

            if (jediProfileSection == null)
            {
                return HttpNotFound();
            }
            if (jediProfileSection.Profiles != null && jediProfileSection.Profiles.Any())
            {
                db.JediProfiles.RemoveRange(jediProfileSection.Profiles);
            }
            db.JediProfilesSection.Remove(jediProfileSection);
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
