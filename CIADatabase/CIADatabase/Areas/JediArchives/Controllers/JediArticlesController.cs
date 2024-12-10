using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIADatabase.Areas.GWOT.Models;
using CIADatabase.Areas.JediArchives.Models;
using CIADatabase.Migrations;
using CIADatabase.Models;
using WebGrease;

namespace CIADatabase.Areas.JediArchives.Controllers
{
    public class JediArticlesController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: JediArchives/JediArticles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the current JediArticle
            JediArticle jediArticle = db.JediArticles.Find(id);
            if (jediArticle == null)
            {
                return HttpNotFound();
            }

            // Retrieve section details
            var section = db.JediSections.Find(jediArticle.JediSectionId);
            ViewBag.SectionTitle = section?.Title;

            // Fetch all articles in the same section, ordered by Order
            var articlesInSection = db.JediArticles
                .Where(a => a.JediSectionId == jediArticle.JediSectionId)
                .OrderBy(a => a.Order)
                .ToList();

            // Find the index of the current article
            var currentIndex = articlesInSection.FindIndex(a => a.JediArticleId == jediArticle.JediArticleId);

            // Determine previous and next articles based on Order
            ViewBag.PreviousArticle = currentIndex > 0 ? articlesInSection[currentIndex - 1] : null;
            ViewBag.NextArticle = currentIndex < articlesInSection.Count - 1 ? articlesInSection[currentIndex + 1] : null;

            return View(jediArticle);
        }


        // GET: JediArchives/JediArticles/Create
        [Authorize]
        public ActionResult Create(int? sectionId)
        {
            if (sectionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var section = db.JediSections.Find(sectionId);
            if (section == null)
            {
                return HttpNotFound();
            }

            // Pass the section title to the ViewBag
            ViewBag.SectionTitle = section.Title; // Assuming the section has a Title property
            ViewBag.SectionId = sectionId;

            return View(new JediArticle { JediSectionId = sectionId.Value });
        }

        // POST: JediArchives/JediArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "JediArticleId,Order,Title,Planet,Location,Briefing,Video,AfterActionReport,JediSectionId")] JediArticle jediArticle,
            HttpPostedFileBase Starmap1,
            HttpPostedFileBase Starmap2,
            HttpPostedFileBase Starmap3,
            HttpPostedFileBase LocationMap1,
            HttpPostedFileBase LocationMap2,
            HttpPostedFileBase LocationMap3)
        {
            if (ModelState.IsValid)
            {
                if (Starmap1 != null)
                {
                    using (var binaryReader = new BinaryReader(Starmap1.InputStream))
                    {
                        jediArticle.Starmap1 = binaryReader.ReadBytes(Starmap1.ContentLength);
                    }
                }

                if (Starmap2 != null)
                {
                    using (var binaryReader = new BinaryReader(Starmap2.InputStream))
                    {
                        jediArticle.Starmap2 = binaryReader.ReadBytes(Starmap2.ContentLength);
                    }
                }

                if (Starmap3 != null)
                {
                    using (var binaryReader = new BinaryReader(Starmap3.InputStream))
                    {
                        jediArticle.Starmap3 = binaryReader.ReadBytes(Starmap3.ContentLength);
                    }
                }

                if (LocationMap1 != null)
                {
                    using (var binaryReader = new BinaryReader(LocationMap1.InputStream))
                    {
                        jediArticle.LocationMap1 = binaryReader.ReadBytes(LocationMap1.ContentLength);
                    }
                }

                if (LocationMap2 != null)
                {
                    using (var binaryReader = new BinaryReader(LocationMap2.InputStream))
                    {
                        jediArticle.LocationMap2 = binaryReader.ReadBytes(LocationMap2.ContentLength);
                    }
                }


                if (LocationMap3 != null)
                {
                    using (var binaryReader = new BinaryReader(LocationMap3.InputStream))
                    {
                        jediArticle.LocationMap3 = binaryReader.ReadBytes(LocationMap3.ContentLength);
                    }
                }

                var section = db.JediSections.Find(jediArticle.JediSectionId);
                if (section == null)
                {
                    return HttpNotFound("Section not found");
                }
                section.Articles.Add(jediArticle);

                db.JediArticles.Add(jediArticle);
                db.SaveChanges();
                return RedirectToAction("Details", "JediSections", new { id = jediArticle.JediSectionId });
            }

            return View(jediArticle);
        }

        // GET: JediArchives/JediArticles/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var article = db.JediArticles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            var section = db.JediSections.Find(article.JediSectionId);
            ViewBag.SectionTitle = section.Title;

            // Populate ViewBag.SectionId with a dropdown of sections
            ViewBag.JediArticleId = id;
            ViewBag.SectionId = new SelectList(db.JediSections, "SectionId", "Title", article.JediSectionId);
            return View(article);
        }

        // POST: JediArchives/JediArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "JediArticleId,Order,Title,Planet,Location,Briefing,Video,AfterActionReport")] JediArticle jediArticle,
            HttpPostedFileBase Starmap1,
            HttpPostedFileBase Starmap2,
            HttpPostedFileBase Starmap3,
            HttpPostedFileBase LocationMap1,
            HttpPostedFileBase LocationMap2,
            HttpPostedFileBase LocationMap3)
        {
            if (ModelState.IsValid)
            {

                // Retrieve the existing article from the database
                var existingArticle = db.JediArticles.Find(jediArticle.JediArticleId);
                if (existingArticle == null)
                {
                    return HttpNotFound("The article no longer exists.");
                }

                // Update non-file properties
                existingArticle.Order = jediArticle.Order;
                existingArticle.Title = jediArticle.Title;
                existingArticle.Planet = jediArticle.Planet;
                existingArticle.Location = jediArticle.Location;
                existingArticle.Briefing = jediArticle.Briefing;
                existingArticle.Video = jediArticle.Video;
                existingArticle.AfterActionReport = jediArticle.AfterActionReport;

                // Helper function to update file fields
                byte[] UpdateFile(HttpPostedFileBase file, byte[] existingFile)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            return binaryReader.ReadBytes(file.ContentLength);
                        }
                    }
                    return existingFile;
                }

                // Update file properties
                existingArticle.Starmap1 = UpdateFile(Starmap1, existingArticle.Starmap1);
                existingArticle.Starmap2 = UpdateFile(Starmap2, existingArticle.Starmap2);
                existingArticle.Starmap3 = UpdateFile(Starmap3, existingArticle.Starmap3);
                existingArticle.LocationMap1 = UpdateFile(LocationMap1, existingArticle.LocationMap1);
                existingArticle.LocationMap2 = UpdateFile(LocationMap2, existingArticle.LocationMap2);
                existingArticle.LocationMap3 = UpdateFile(LocationMap3, existingArticle.LocationMap3);


                // Mark the entity as modified and save changes
                db.Entry(existingArticle).State = EntityState.Modified;
                db.SaveChanges();

                // Redirect to the section's Details page
                return RedirectToAction("Details", "JediSections", new { id = existingArticle.JediSectionId });
            }
            return View(jediArticle);
        }

        // GET: JediArchives/JediArticles/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediArticle jediArticle = db.JediArticles.Find(id);
            if (jediArticle == null)
            {
                return HttpNotFound();
            }
            var section = db.JediSections.Find(jediArticle.JediSectionId);
            ViewBag.SectionTitle = section.Title;
            return View(jediArticle);
        }

        // POST: JediArchives/JediArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            JediArticle jediArticle = db.JediArticles.Find(id);
            var section = db.JediSections.Include(s => s.Articles).SingleOrDefault(s => s.SectionId == jediArticle.JediSectionId);
            if (section == null) 
            {
                return HttpNotFound("Section not found");
            }
            section.Articles.Remove(jediArticle);
            db.JediArticles.Remove(jediArticle);
            db.SaveChanges();
            return RedirectToAction("Details", "JediSections", new { id = jediArticle.JediSectionId });
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
