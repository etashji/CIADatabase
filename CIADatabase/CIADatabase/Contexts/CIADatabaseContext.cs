using CIADatabase.Areas.Users.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

    }

}