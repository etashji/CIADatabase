namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileEdit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GWOTProfiles", "ProfilePic", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GWOTProfiles", "ProfilePic", c => c.Binary(nullable: false));
        }
    }
}
