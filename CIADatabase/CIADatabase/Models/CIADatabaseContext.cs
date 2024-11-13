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
        public CIADatabaseContext() : base("name=CIADatabaseContext") { }

        // Add any other DbSet properties as needed (for example, for user roles, etc.)

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Other custom configurations can be added here.
        }
    }

}