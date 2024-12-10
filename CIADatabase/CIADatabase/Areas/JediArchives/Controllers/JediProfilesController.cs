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
using CIADatabase.Areas.JediArchives.Models;
using CIADatabase.Models;

namespace CIADatabase.Areas.JediArchives.Controllers
{
    public class JediProfilesController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: JediArchives/JediProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediProfile jediProfile = db.JediProfiles.Find(id);
            if (jediProfile == null)
            {
                return HttpNotFound();
            }
            return View(jediProfile);
        }

        // GET: JediArchives/JediProfiles/Create
        [Authorize]
        public ActionResult Create(int? sectionId)
        {
            if (sectionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var section = db.JediProfilesSection.Find(sectionId);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionTitle = section.Title;
            ViewBag.SectionId = sectionId;
            return View();
        }

        // POST: JediArchives/JediProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ProfileId,ProfilePic,FirstName,LastName,Alias,Profile,ProfileSectionId")] JediProfile jediProfile,
            HttpPostedFileBase ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                if (ProfilePicture != null)
                {
                    using (var binaryReader = new BinaryReader(ProfilePicture.InputStream))
                    {
                        jediProfile.ProfilePic = binaryReader.ReadBytes(ProfilePicture.ContentLength);
                    }
                }
                var section = db.JediProfilesSection.Find(jediProfile.ProfileSectionId);
                section.Profiles.Add(jediProfile);
                db.JediProfiles.Add(jediProfile);
                db.SaveChanges();
                return RedirectToAction("Details", "JediProfileSections", new { id = jediProfile.ProfileSectionId });
            }

            return View(jediProfile);
        }

        // GET: JediArchives/JediProfiles/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediProfile jediProfile = db.JediProfiles.Find(id);
            if (jediProfile == null)
            {
                return HttpNotFound();
            }
            return View(jediProfile);
        }

        // POST: JediArchives/JediProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ProfileId,FirstName,LastName,Alias,Profile,ProfileSectionId")] JediProfile jediProfile,
            HttpPostedFileBase ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                var existingProfile = db.JediProfiles.Find(jediProfile.ProfileId);
                if (existingProfile == null)
                {
                    return HttpNotFound("The profile no longer exists.");
                }
                existingProfile.FirstName = jediProfile.FirstName;
                existingProfile.LastName = jediProfile.LastName;
                existingProfile.Alias = jediProfile.Alias;
                existingProfile.Profile = jediProfile.Profile;

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

                existingProfile.ProfilePic = UpdateFile(ProfilePicture, existingProfile.ProfilePic);

                db.Entry(existingProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "JediProfileSections", new { id = jediProfile.ProfileSectionId });
            }
            return View(jediProfile);
        }

        // GET: JediArchives/JediProfiles/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JediProfile jediProfile = db.JediProfiles.Find(id);
            if (jediProfile == null)
            {
                return HttpNotFound();
            }
            return View(jediProfile);
        }

        // POST: JediArchives/JediProfiles/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JediProfile jediProfile = db.JediProfiles.Find(id);
            var section = db.JediProfilesSection.Find(jediProfile.ProfileSectionId);
            section.Profiles.Remove(jediProfile);
            db.JediProfiles.Remove(jediProfile);
            db.SaveChanges();
            return RedirectToAction("Details", "JediProfileSections", new { id = jediProfile.ProfileSectionId });
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
