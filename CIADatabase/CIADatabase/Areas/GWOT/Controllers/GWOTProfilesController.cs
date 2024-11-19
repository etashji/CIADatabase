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

namespace CIADatabase.Areas.GWOT.Controllers
{
    public class GWOTProfilesController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: GWOT/GWOTProfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTProfile gWOTProfile = db.GWOTProfiles.Find(id);
            if (gWOTProfile == null)
            {
                return HttpNotFound();
            }
            return View(gWOTProfile);
        }

        [Authorize]
        // GET: GWOT/GWOTProfiles/Create
        public ActionResult Create(int? sectionId)
        {
            if (sectionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var section = db.GWOTProfileSections.Find(sectionId);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionTitle = section.Title;
            ViewBag.SectionId = sectionId;
            return View();
        }

        // POST: GWOT/GWOTProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ProfileId,DOB,FirstName,LastName,Alias,Profile,ProfileSectionId")] GWOTProfile gWOTProfile, 
            HttpPostedFileBase ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                if (ProfilePicture != null)
                {
                    using (var binaryReader = new BinaryReader(ProfilePicture.InputStream))
                    {
                        gWOTProfile.ProfilePic = binaryReader.ReadBytes(ProfilePicture.ContentLength);
                    }
                }
                var section = db.GWOTProfileSections.Find(gWOTProfile.ProfileSectionId);
                section.Profiles.Add(gWOTProfile);
                db.GWOTProfiles.Add(gWOTProfile);
                db.SaveChanges();
                return RedirectToAction("Details", "GWOTProfileSections", new { id = gWOTProfile.ProfileSectionId });
            }

            return View(gWOTProfile);
        }

        // GET: GWOT/GWOTProfiles/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTProfile gWOTProfile = db.GWOTProfiles.Find(id);
            if (gWOTProfile == null)
            {
                return HttpNotFound();
            }
            return View(gWOTProfile);
        }

        // POST: GWOT/GWOTProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileId,ProfilePic,DOB,FirstName,LastName,Alias,Profile,ProfileSectionId")] GWOTProfile gWOTProfile,
            HttpPostedFileBase ProfilePicture)
        {
            if (ModelState.IsValid)
            {
                var existingProfile = db.GWOTProfiles.Find(gWOTProfile.ProfileId);
                if (existingProfile == null)
                {
                    return HttpNotFound("The article no longer exists.");
                }

                existingProfile.FirstName = gWOTProfile.FirstName;
                existingProfile.LastName = gWOTProfile.LastName;
                existingProfile.Alias = gWOTProfile.Alias;
                existingProfile.Profile = gWOTProfile.Profile;

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
                return RedirectToAction("Details", "GWOTProfileSections", new { id = gWOTProfile.ProfileSectionId });
            }
            return View(gWOTProfile);
        }

        // GET: GWOT/GWOTProfiles/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GWOTProfile gWOTProfile = db.GWOTProfiles.Find(id);
            if (gWOTProfile == null)
            {
                return HttpNotFound();
            }
            return View(gWOTProfile);
        }

        // POST: GWOT/GWOTProfiles/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GWOTProfile gWOTProfile = db.GWOTProfiles.Find(id);
            var section = db.GWOTProfileSections.Find(gWOTProfile.ProfileSectionId);
            section.Profiles.Remove(gWOTProfile);
            db.GWOTProfiles.Remove(gWOTProfile);
            db.SaveChanges();
            return RedirectToAction("Details", "GWOTProfileSections", new { id = gWOTProfile.ProfileSectionId });
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
