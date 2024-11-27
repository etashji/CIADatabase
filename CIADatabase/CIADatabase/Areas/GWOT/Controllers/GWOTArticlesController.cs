using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIADatabase.Areas.GWOT.Models;
using CIADatabase.Migrations;
using CIADatabase.Models;
using WebGrease;

namespace CIADatabase.Areas.GWOT.Controllers
{
    public class GWOTArticlesController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: GWOT/GWOTArticles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GWOTArticle gWOTArticle = db.GWOTArticles.Find(id);
            if (gWOTArticle == null)
            {
                return HttpNotFound();
            }

            // Retrieve section details
            var section = db.GWOTSections.Find(gWOTArticle.GWOTSectionId);
            ViewBag.SectionTitle = section?.Title;

            // Fetch all articles in the same section, ordered by ID
            var articlesInSection = db.GWOTArticles
                .Where(a => a.GWOTSectionId == gWOTArticle.GWOTSectionId)
                .OrderBy(a => a.GWOTArticleId)
                .ToList();

            // Find the index of the current article
            var currentIndex = articlesInSection.IndexOf(gWOTArticle);

            // Determine previous and next articles
            ViewBag.PreviousArticle = currentIndex > 0 ? articlesInSection[currentIndex - 1] : null;
            ViewBag.NextArticle = currentIndex < articlesInSection.Count - 1 ? articlesInSection[currentIndex + 1] : null;

            return View(gWOTArticle);
        }


        // GET: GWOT/GWOTArticles/Create
        // GET: Articles/Create
        [Authorize]
        public ActionResult Create(int? sectionId)
        {
            if (sectionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Fetch the section based on the sectionId
            var section = db.GWOTSections.Find(sectionId);
            if (section == null)
            {
                return HttpNotFound();
            }

            // Pass the section title to the ViewBag
            ViewBag.SectionTitle = section.Title; // Assuming the section has a Title property
            ViewBag.SectionId = sectionId;

            return View(new GWOTArticle { GWOTSectionId = sectionId.Value });
        }


        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create(
        [Bind(Include = "ArticleId,Briefing,Video,AfterActionReport,LocalTime,ZuluTime,Location,GWOTSectionId")] GWOTArticle article,
             HttpPostedFileBase PoliticalMap,
             HttpPostedFileBase MilitaryMap,
             HttpPostedFileBase PresidentRegionMap,
             HttpPostedFileBase CountryMap,
             HttpPostedFileBase RegionMap,
             HttpPostedFileBase CityMap,
             HttpPostedFileBase LocalMap,
             HttpPostedFileBase UpdatedPoliticalMap,
             HttpPostedFileBase UpdatedMilitaryMap,
             HttpPostedFileBase UpdatedRegionMap)
        {
            if (ModelState.IsValid)
            {
                // Save PoliticalMap
                if (PoliticalMap != null)
                {
                    using (var binaryReader = new BinaryReader(PoliticalMap.InputStream))
                    {
                        article.PoliticalMap = binaryReader.ReadBytes(PoliticalMap.ContentLength);
                    }
                }

                // Save MilitaryMap
                if (MilitaryMap != null)
                {
                    using (var binaryReader = new BinaryReader(MilitaryMap.InputStream))
                    {
                        article.MilitaryMap = binaryReader.ReadBytes(MilitaryMap.ContentLength);
                    }
                }

                // Save PresidentRegionMap
                if (PresidentRegionMap != null)
                {
                    using (var binaryReader = new BinaryReader(PresidentRegionMap.InputStream))
                    {
                        article.PresidentRegionMap = binaryReader.ReadBytes(PresidentRegionMap.ContentLength);
                    }
                }

                // Save CountryMap
                if (CountryMap != null)
                {
                    using (var binaryReader = new BinaryReader(CountryMap.InputStream))
                    {
                        article.CountryMap = binaryReader.ReadBytes(CountryMap.ContentLength);
                    }
                }

                // Save RegionMap
                if (RegionMap != null)
                {
                    using (var binaryReader = new BinaryReader(RegionMap.InputStream))
                    {
                        article.RegionMap = binaryReader.ReadBytes(RegionMap.ContentLength);
                    }
                }

                // Save CityMap
                if (CityMap != null)
                {
                    using (var binaryReader = new BinaryReader(CityMap.InputStream))
                    {
                        article.CityMap = binaryReader.ReadBytes(CityMap.ContentLength);
                    }
                }

                // Save LocalMap
                if (LocalMap != null)
                {
                    using (var binaryReader = new BinaryReader(LocalMap.InputStream))
                    {
                        article.LocalMap = binaryReader.ReadBytes(LocalMap.ContentLength);
                    }
                }

                // Save UpdatedPoliticalMap
                if (UpdatedPoliticalMap != null)
                {
                    using (var binaryReader = new BinaryReader(UpdatedPoliticalMap.InputStream))
                    {
                        article.UpdatedPoliticalMap = binaryReader.ReadBytes(UpdatedPoliticalMap.ContentLength);
                    }
                }

                // Save UpdatedMilitaryMap
                if (UpdatedMilitaryMap != null)
                {
                    using (var binaryReader = new BinaryReader(UpdatedMilitaryMap.InputStream))
                    {
                        article.UpdatedMilitaryMap = binaryReader.ReadBytes(UpdatedMilitaryMap.ContentLength);
                    }
                }

                // Save UpdatedRegionMap
                if (UpdatedRegionMap != null)
                {
                    using (var binaryReader = new BinaryReader(UpdatedRegionMap.InputStream))
                    {
                        article.UpdatedRegionMap = binaryReader.ReadBytes(UpdatedRegionMap.ContentLength);
                    }
                }

                // Attach the article to its section
                var section = db.GWOTSections.Find(article.GWOTSectionId);
                if (section == null)
                {
                    return HttpNotFound("Section not found");
                }
                section.Articles.Add(article);

                // Save the article to the database
                db.GWOTArticles.Add(article);
                db.SaveChanges();

                // Redirect to the section's Details page
                return RedirectToAction("Details", "GWOTSections", new { id = article.GWOTSectionId });
            }

            // If model state is invalid, return the view with the current data
            ViewBag.SectionId = new SelectList(db.GWOTSections, "SectionId", "Title", article.GWOTSectionId);
            return View(article);
        }



        // GET: GWOT/GWOTArticles/Edit/5
        // GET: Articles/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var article = db.GWOTArticles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            var section = db.GWOTSections.Find(article.GWOTSectionId);
            ViewBag.SectionTitle = section.Title;
            // Populate ViewBag.SectionId with a dropdown of sections
            ViewBag.GWOTArticleId = id;
            ViewBag.SectionId = new SelectList(db.GWOTSections, "SectionId", "Title", article.GWOTSectionId);
            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Edit(
        [Bind(Include = "ArticleId,Briefing,Video,AfterActionReport,LocalTime,ZuluTime,Location,GWOTSectionId,GWOTArticleId")] GWOTArticle article,
            HttpPostedFileBase PoliticalMap,
            HttpPostedFileBase MilitaryMap,
            HttpPostedFileBase PresidentRegionMap,
            HttpPostedFileBase CountryMap,
            HttpPostedFileBase RegionMap,
            HttpPostedFileBase CityMap,
            HttpPostedFileBase LocalMap,
            HttpPostedFileBase UpdatedPoliticalMap,
            HttpPostedFileBase UpdatedMilitaryMap,
            HttpPostedFileBase UpdatedRegionMap)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the existing article from the database
                var existingArticle = db.GWOTArticles.Find(article.GWOTArticleId);
                if (existingArticle == null)
                {
                    return HttpNotFound("The article no longer exists.");
                }

                // Update non-file properties
                existingArticle.Briefing = article.Briefing;
                existingArticle.AfterActionReport = article.AfterActionReport;
                existingArticle.LocalTime = article.LocalTime;
                existingArticle.ZuluTime = article.ZuluTime;
                existingArticle.Location = article.Location;
                existingArticle.Video = article.Video;  

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
                existingArticle.PoliticalMap = UpdateFile(PoliticalMap, existingArticle.PoliticalMap);
                existingArticle.MilitaryMap = UpdateFile(MilitaryMap, existingArticle.MilitaryMap);
                existingArticle.PresidentRegionMap = UpdateFile(PresidentRegionMap, existingArticle.PresidentRegionMap);
                existingArticle.CountryMap = UpdateFile(CountryMap, existingArticle.CountryMap);
                existingArticle.RegionMap = UpdateFile(RegionMap, existingArticle.RegionMap);
                existingArticle.CityMap = UpdateFile(CityMap, existingArticle.CityMap);
                existingArticle.LocalMap = UpdateFile(LocalMap, existingArticle.LocalMap);
                existingArticle.UpdatedPoliticalMap = UpdateFile(UpdatedPoliticalMap, existingArticle.UpdatedPoliticalMap);
                existingArticle.UpdatedMilitaryMap = UpdateFile(UpdatedMilitaryMap, existingArticle.UpdatedMilitaryMap);
                existingArticle.UpdatedRegionMap = UpdateFile(UpdatedRegionMap, existingArticle.UpdatedRegionMap);

                // Mark the entity as modified and save changes
                db.Entry(existingArticle).State = EntityState.Modified;
                db.SaveChanges();

                // Redirect to the section's Details page
                return RedirectToAction("Details", "GWOTSections", new { id = existingArticle.GWOTSectionId });
            }

            // Repopulate ViewBag.SectionId in case of validation errors
            ViewBag.SectionId = new SelectList(db.GWOTSections, "SectionId", "Title", article.GWOTSectionId);
            return View(article);
        }

        // GET: GWOT/GWOTArticles/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTArticle gWOTArticle = db.GWOTArticles.Find(id);
            if (gWOTArticle == null)
            {
                return HttpNotFound();
            }
            var section = db.GWOTSections.Find(gWOTArticle.GWOTSectionId);
            ViewBag.SectionTitle = section.Title;
            return View(gWOTArticle);
        }

        // POST: GWOT/GWOTArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            GWOTArticle gWOTArticle = db.GWOTArticles.Find(id);
            var section = db.GWOTSections.Include(s => s.Articles).SingleOrDefault(s => s.SectionId == gWOTArticle.GWOTSectionId);
            if (section == null)
            {
                return HttpNotFound("Section not found");
            }
            section.Articles.Remove(gWOTArticle);
            db.GWOTArticles.Remove(gWOTArticle);
            db.SaveChanges();
            return RedirectToAction("Details", "GWOTSections", new { id = gWOTArticle.GWOTSectionId });
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
