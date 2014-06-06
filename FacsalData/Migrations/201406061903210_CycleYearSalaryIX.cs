namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CycleYearSalaryIX : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Salary", new[] { "Id", "CycleYear" }, unique: true, name: "IX_IdAndCycleYear");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Salary", "IX_IdAndCycleYear");
        }
    }
}
