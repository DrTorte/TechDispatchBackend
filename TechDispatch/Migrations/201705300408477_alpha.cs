namespace TechDispatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alpha : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "AppointmentStateReasonID", "dbo.AppointmentStateReasons");
            DropIndex("dbo.Appointments", new[] { "AppointmentStateReasonID" });
            CreateTable(
                "dbo.AppointmentResolutionReasons",
                c => new
                    {
                        AppointmentResolutionReasonID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                        Cancel = c.Boolean(nullable: false),
                        Fail = c.Boolean(nullable: false),
                        Complete = c.Boolean(nullable: false),
                        Reschedule = c.Boolean(nullable: false),
                        RequireComment = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentResolutionReasonID);
            
            AddColumn("dbo.Appointments", "AppointmentResolutionReasonID", c => c.Int());
            CreateIndex("dbo.Appointments", "AppointmentResolutionReasonID");
            AddForeignKey("dbo.Appointments", "AppointmentResolutionReasonID", "dbo.AppointmentResolutionReasons", "AppointmentResolutionReasonID");
            DropColumn("dbo.Appointments", "AppointmentStateReasonID");
            DropTable("dbo.AppointmentStateReasons");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AppointmentStateReasons",
                c => new
                    {
                        AppointmentStateReasonID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentStateReasonID);
            
            AddColumn("dbo.Appointments", "AppointmentStateReasonID", c => c.Int());
            DropForeignKey("dbo.Appointments", "AppointmentResolutionReasonID", "dbo.AppointmentResolutionReasons");
            DropIndex("dbo.Appointments", new[] { "AppointmentResolutionReasonID" });
            DropColumn("dbo.Appointments", "AppointmentResolutionReasonID");
            DropTable("dbo.AppointmentResolutionReasons");
            CreateIndex("dbo.Appointments", "AppointmentStateReasonID");
            AddForeignKey("dbo.Appointments", "AppointmentStateReasonID", "dbo.AppointmentStateReasons", "AppointmentStateReasonID");
        }
    }
}
