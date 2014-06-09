namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RankTypeSeq : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Salary", "IX_IdAndCycleYear");
            DropIndex("dbo.Salary", new[] { "PersonId" });
            AddColumn("dbo.RankType", "Sequence", c => c.String());
            CreateIndex("dbo.Salary", new[] { "PersonId", "CycleYear" }, unique: true, name: "IX_PersonAndCycleYear");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Salary", "IX_PersonAndCycleYear");
            DropColumn("dbo.RankType", "Sequence");
            CreateIndex("dbo.Salary", "PersonId");
            CreateIndex("dbo.Salary", new[] { "Id", "CycleYear" }, unique: true, name: "IX_IdAndCycleYear");
        }
    }
}
