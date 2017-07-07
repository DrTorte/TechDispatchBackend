namespace TechDispatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alpha3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppointmentSubReasons", "Install", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppointmentSubReasons", "Repair", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppointmentSubReasons", "Misc", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppointmentSubReasons", "RequireComment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppointmentSubReasons", "RequireComment");
            DropColumn("dbo.AppointmentSubReasons", "Misc");
            DropColumn("dbo.AppointmentSubReasons", "Repair");
            DropColumn("dbo.AppointmentSubReasons", "Install");
        }
    }
}
