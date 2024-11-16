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
            return View(gWOTArticle);
        }

        // GET: GWOT/GWOTArticles/Create
        // GET: Articles/Create
        public ActionResult Create(int? sectionId)
        {
            if (sectionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.SectionId = sectionId;
            return View(new GWOTArticle { GWOTSectionId = sectionId.Value });
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleId,Briefing,AfterActionReport,LocalTime,ZuluTime,Location,GWOTSectionId")] GWOTArticle article, HttpPostedFileBase PoliticalMap, HttpPostedFileBase MilitaryMap)
        {
            if (ModelState.IsValid)
            {
                if (article.PoliticalMap != null)
                {
                    using (var binaryReader = new BinaryReader(PoliticalMap.InputStream))
                    {
                        article.PoliticalMap = binaryReader.ReadBytes(PoliticalMap.ContentLength);
                    }
                }

                if (MilitaryMap != null)
                {
                    using (var binaryReader = new BinaryReader(MilitaryMap.InputStream))
                    {
                        article.MilitaryMap = binaryReader.ReadBytes(MilitaryMap.ContentLength);
                    }
                }
                var section = db.GWOTSections.Include(s => s.Articles).SingleOrDefault(s => s.SectionId == article.GWOTSectionId);
                if (section == null)
                {
                    return HttpNotFound("Section not found");
                }
                section.Articles.Add(article);

                db.GWOTArticles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Details", "GWOTSections", new { id = article.GWOTSectionId });

            }

            ViewBag.SectionId = new SelectList(db.GWOTSections, "SectionId", "Title", article.GWOTSectionId);
            return View(article);
        }


        // GET: GWOT/GWOTArticles/Edit/5
        // GET: Articles/Edit/5
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

            // Populate ViewBag.SectionId with a dropdown of sections
            ViewBag.GWOTArticleId = id;
            ViewBag.SectionId = new SelectList(db.GWOTSections, "SectionId", "Title", article.GWOTSectionId);
            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,Briefing,AfterActionReport,LocalTime,ZuluTime,Location,GWOTSectionId,GWOTArticleId")] GWOTArticle article)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the existing article from the database
                var existingArticle = db.GWOTArticles.Find(article.GWOTArticleId);
                if (existingArticle == null)
                {
                    return HttpNotFound("The article no longer exists.");
                }

                // Update properties
                existingArticle.Briefing = article.Briefing;
                existingArticle.AfterActionReport = article.AfterActionReport;
                existingArticle.LocalTime = article.LocalTime;
                existingArticle.ZuluTime = article.ZuluTime;
                existingArticle.Location = article.Location;
                existingArticle.GWOTSectionId = article.GWOTSectionId;

                db.Entry(existingArticle).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "GWOTSections", new { id = article.GWOTSectionId });
            }

            // Repopulate ViewBag.SectionId in case of validation errors
            ViewBag.SectionId = new SelectList(db.GWOTSections, "SectionId", "Title", article.GWOTSectionId);
            return View(article);
        }



        // GET: GWOT/GWOTArticles/Delete/5
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
            return View(gWOTArticle);
        }

        // POST: GWOT/GWOTArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
