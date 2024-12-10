namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JediArticles", "Title", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JediArticles", "Title");
        }
    }
}
