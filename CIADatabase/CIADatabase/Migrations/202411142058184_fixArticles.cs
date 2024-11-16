namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixArticles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GWOTArticles", "SectionId", "dbo.GWOTSections");
            DropIndex("dbo.GWOTArticles", new[] { "SectionId" });
            RenameColumn(table: "dbo.GWOTArticles", name: "SectionId", newName: "GWOTSection_SectionId");
            AddColumn("dbo.GWOTArticles", "GWOTSectionId", c => c.Int(nullable: false));
            AlterColumn("dbo.GWOTArticles", "GWOTSection_SectionId", c => c.Int());
            CreateIndex("dbo.GWOTArticles", "GWOTSection_SectionId");
            AddForeignKey("dbo.GWOTArticles", "GWOTSection_SectionId", "dbo.GWOTSections", "SectionId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GWOTArticles", "GWOTSection_SectionId", "dbo.GWOTSections");
            DropIndex("dbo.GWOTArticles", new[] { "GWOTSection_SectionId" });
            AlterColumn("dbo.GWOTArticles", "GWOTSection_SectionId", c => c.Int(nullable: false));
            DropColumn("dbo.GWOTArticles", "GWOTSectionId");
            RenameColumn(table: "dbo.GWOTArticles", name: "GWOTSection_SectionId", newName: "SectionId");
            CreateIndex("dbo.GWOTArticles", "SectionId");
            AddForeignKey("dbo.GWOTArticles", "SectionId", "dbo.GWOTSections", "SectionId", cascadeDelete: true);
        }
    }
}
