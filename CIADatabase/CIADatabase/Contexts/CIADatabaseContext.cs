using CIADatabase.Areas.GWOT;
using CIADatabase.Areas.Users.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CIADatabase.Areas.GWOT.Models;

namespace CIADatabase.Models
{
    public class CIADatabaseContext : DbContext
    {
        public class ApplicationUser : IdentityUser
        {
            // Additional custom properties can go here, if needed.
        }

        public CIADatabaseContext() : base("name=CIADatabaseContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GWOTSections> GWOTSections { get; set; }
        public DbSet<GWOTArticle> GWOTArticles { get; set; }
        public DbSet<GWOTProfileSections> GWOTProfileSections { get; set; }
        public DbSet<GWOTProfile> GWOTProfiles { get; set; }


    }

}