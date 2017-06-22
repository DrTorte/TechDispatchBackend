namespace TechDispatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class baselevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AuthId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AuthId");
        }
    }
}
