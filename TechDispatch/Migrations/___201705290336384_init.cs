namespace TechDispatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "AuthId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "AuthId", c => c.Int(nullable: false));
        }
    }
}
