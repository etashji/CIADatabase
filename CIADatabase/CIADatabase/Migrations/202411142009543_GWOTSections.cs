namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GWOTSections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GWOTSections",
                c => new
                    {
                        SectionId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SectionId);
            
            CreateTable(
                "dbo.GWOTArticles",
                c => new
                    {
                        GWOTArticleId = c.Int(nullable: false, identity: true),
                        GWOTSections_SectionId = c.Int(),
                    })
                .PrimaryKey(t => t.GWOTArticleId)
                .ForeignKey("dbo.GWOTSections", t => t.GWOTSections_SectionId)
                .Index(t => t.GWOTSections_SectionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GWOTArticles", "GWOTSections_SectionId", "dbo.GWOTSections");
            DropIndex("dbo.GWOTArticles", new[] { "GWOTSections_SectionId" });
            DropTable("dbo.GWOTArticles");
            DropTable("dbo.GWOTSections");
        }
    }
}
