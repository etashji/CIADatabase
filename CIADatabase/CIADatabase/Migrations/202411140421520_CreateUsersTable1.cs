namespace CIADatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUsersTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "HashedPassword", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "HashedPassword", c => c.String(nullable: false));
        }
    }
}
