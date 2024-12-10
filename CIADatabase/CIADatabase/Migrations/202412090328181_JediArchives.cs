namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JediArchives : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JediArticles",
                c => new
                    {
                        JediArticleId = c.Int(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Planet = c.String(maxLength: 200),
                        Location = c.String(maxLength: 200),
                        Starmap1 = c.Binary(),
                        Starmap2 = c.Binary(),
                        Starmap3 = c.Binary(),
                        LocationMap1 = c.Binary(),
                        LocationMap2 = c.Binary(),
                        LocationMap3 = c.Binary(),
                        Briefing = c.String(nullable: false, storeType: "ntext"),
                        Video = c.String(),
                        AfterActionReport = c.String(storeType: "ntext"),
                        JediSectionId = c.Int(nullable: false),
                        JediSection_SectionId = c.Int(),
                    })
                .PrimaryKey(t => t.JediArticleId)
                .ForeignKey("dbo.JediSections", t => t.JediSection_SectionId)
                .Index(t => t.JediSection_SectionId);
            
            CreateTable(
                "dbo.JediSections",
                c => new
                    {
                        SectionId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SectionId);
            
            CreateTable(
                "dbo.JediProfiles",
                c => new
                    {
                        ProfileId = c.Int(nullable: false, identity: true),
                        ProfilePic = c.Binary(),
                        FirstName = c.String(maxLength: 25),
                        LastName = c.String(maxLength: 25),
                        Alias = c.String(maxLength: 25),
                        Profile = c.String(nullable: false, storeType: "ntext"),
                        ProfileSectionId = c.Int(nullable: false),
                        ProfileSection_SectionId = c.Int(),
                    })
                .PrimaryKey(t => t.ProfileId)
                .ForeignKey("dbo.JediProfileSections", t => t.ProfileSection_SectionId)
                .Index(t => t.ProfileSection_SectionId);
            
            CreateTable(
                "dbo.JediProfileSections",
                c => new
                    {
                        SectionId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SectionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JediProfiles", "ProfileSection_SectionId", "dbo.JediProfileSections");
            DropForeignKey("dbo.JediArticles", "JediSection_SectionId", "dbo.JediSections");
            DropIndex("dbo.JediProfiles", new[] { "ProfileSection_SectionId" });
            DropIndex("dbo.JediArticles", new[] { "JediSection_SectionId" });
            DropTable("dbo.JediProfileSections");
            DropTable("dbo.JediProfiles");
            DropTable("dbo.JediSections");
            DropTable("dbo.JediArticles");
        }
    }
}
