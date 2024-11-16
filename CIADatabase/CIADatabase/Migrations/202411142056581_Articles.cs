namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Articles : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GWOTArticles", "GWOTSections_SectionId", "dbo.GWOTSections");
            DropIndex("dbo.GWOTArticles", new[] { "GWOTSections_SectionId" });
            RenameColumn(table: "dbo.GWOTArticles", name: "GWOTSections_SectionId", newName: "SectionId");
            AddColumn("dbo.GWOTArticles", "LocalTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.GWOTArticles", "ZuluTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.GWOTArticles", "Location", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.GWOTArticles", "PoliticalMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "MilitaryMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "PresidentRegionMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "CountryMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "RegionMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "LocalMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "CityMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "Briefing", c => c.String(nullable: false, storeType: "ntext"));
            AddColumn("dbo.GWOTArticles", "Video", c => c.String());
            AddColumn("dbo.GWOTArticles", "UpdatedPoliticalMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "UpdatedMilitaryMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "UpdatedRegionMap", c => c.Binary());
            AddColumn("dbo.GWOTArticles", "AfterActionReport", c => c.String(nullable: false, storeType: "ntext"));
            AlterColumn("dbo.GWOTArticles", "SectionId", c => c.Int(nullable: false));
            CreateIndex("dbo.GWOTArticles", "SectionId");
            AddForeignKey("dbo.GWOTArticles", "SectionId", "dbo.GWOTSections", "SectionId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GWOTArticles", "SectionId", "dbo.GWOTSections");
            DropIndex("dbo.GWOTArticles", new[] { "SectionId" });
            AlterColumn("dbo.GWOTArticles", "SectionId", c => c.Int());
            DropColumn("dbo.GWOTArticles", "AfterActionReport");
            DropColumn("dbo.GWOTArticles", "UpdatedRegionMap");
            DropColumn("dbo.GWOTArticles", "UpdatedMilitaryMap");
            DropColumn("dbo.GWOTArticles", "UpdatedPoliticalMap");
            DropColumn("dbo.GWOTArticles", "Video");
            DropColumn("dbo.GWOTArticles", "Briefing");
            DropColumn("dbo.GWOTArticles", "CityMap");
            DropColumn("dbo.GWOTArticles", "LocalMap");
            DropColumn("dbo.GWOTArticles", "RegionMap");
            DropColumn("dbo.GWOTArticles", "CountryMap");
            DropColumn("dbo.GWOTArticles", "PresidentRegionMap");
            DropColumn("dbo.GWOTArticles", "MilitaryMap");
            DropColumn("dbo.GWOTArticles", "PoliticalMap");
            DropColumn("dbo.GWOTArticles", "Location");
            DropColumn("dbo.GWOTArticles", "ZuluTime");
            DropColumn("dbo.GWOTArticles", "LocalTime");
            RenameColumn(table: "dbo.GWOTArticles", name: "SectionId", newName: "GWOTSections_SectionId");
            CreateIndex("dbo.GWOTArticles", "GWOTSections_SectionId");
            AddForeignKey("dbo.GWOTArticles", "GWOTSections_SectionId", "dbo.GWOTSections", "SectionId");
        }
    }
}
