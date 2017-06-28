namespace TechDispatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alpha2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FieldTeches", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.FieldTeches", "UserID");
            AddForeignKey("dbo.FieldTeches", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FieldTeches", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.FieldTeches", new[] { "UserID" });
            AlterColumn("dbo.FieldTeches", "UserID", c => c.String());
        }
    }
}
