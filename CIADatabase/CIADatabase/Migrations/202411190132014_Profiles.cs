namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Profiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GWOTProfiles",
                c => new
                    {
                        ProfileId = c.Int(nullable: false, identity: true),
                        ProfilePic = c.Binary(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        FirstName = c.String(maxLength: 25),
                        LastName = c.String(maxLength: 25),
                        Alias = c.String(maxLength: 25),
                        Profile = c.String(nullable: false, storeType: "ntext"),
                        ProfileSectionId = c.Int(nullable: false),
                        ProfileSection_SectionId = c.Int(),
                    })
                .PrimaryKey(t => t.ProfileId)
                .ForeignKey("dbo.GWOTProfileSections", t => t.ProfileSection_SectionId)
                .Index(t => t.ProfileSection_SectionId);
            
            CreateTable(
                "dbo.GWOTProfileSections",
                c => new
                    {
                        SectionId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.SectionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GWOTProfiles", "ProfileSection_SectionId", "dbo.GWOTProfileSections");
            DropIndex("dbo.GWOTProfiles", new[] { "ProfileSection_SectionId" });
            DropTable("dbo.GWOTProfileSections");
            DropTable("dbo.GWOTProfiles");
        }
    }
}
