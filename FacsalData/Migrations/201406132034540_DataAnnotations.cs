namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppointmentType", "Name", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Department", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Department", "Sequence", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.Person", "Pid", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Person", "LastName", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Person", "FirstName", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.BaseAdjustmentType", "Name", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.FacultyType", "Name", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.MeritAdjustmentType", "Name", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.RankType", "Name", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.RankType", "Sequence", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.SpecialAdjustmentType", "Name", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.StatusType", "Name", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.Unit", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Unit", "Sequence", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.Role", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.User", "Pid", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Pid", c => c.String());
            AlterColumn("dbo.Role", "Name", c => c.String());
            AlterColumn("dbo.Unit", "Sequence", c => c.String());
            AlterColumn("dbo.Unit", "Name", c => c.String(maxLength: 1024));
            AlterColumn("dbo.StatusType", "Name", c => c.String());
            AlterColumn("dbo.SpecialAdjustmentType", "Name", c => c.String());
            AlterColumn("dbo.RankType", "Sequence", c => c.String());
            AlterColumn("dbo.RankType", "Name", c => c.String());
            AlterColumn("dbo.MeritAdjustmentType", "Name", c => c.String());
            AlterColumn("dbo.FacultyType", "Name", c => c.String());
            AlterColumn("dbo.BaseAdjustmentType", "Name", c => c.String());
            AlterColumn("dbo.Person", "FirstName", c => c.String());
            AlterColumn("dbo.Person", "LastName", c => c.String());
            AlterColumn("dbo.Person", "Pid", c => c.String());
            AlterColumn("dbo.Department", "Sequence", c => c.String());
            AlterColumn("dbo.Department", "Name", c => c.String());
            AlterColumn("dbo.AppointmentType", "Name", c => c.String());
        }
    }
}
