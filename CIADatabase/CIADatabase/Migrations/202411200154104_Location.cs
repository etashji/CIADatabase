namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Location : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GWOTArticles", "Location", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GWOTArticles", "Location", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
