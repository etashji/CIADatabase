namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AAR : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GWOTArticles", "AfterActionReport", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GWOTArticles", "AfterActionReport", c => c.String(nullable: false, storeType: "ntext"));
        }
    }
}
