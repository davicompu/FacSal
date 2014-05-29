namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnitNavigationPropertyCase : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Department", new[] { "UnitID" });
            CreateIndex("dbo.Department", "UnitId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Department", new[] { "UnitId" });
            CreateIndex("dbo.Department", "UnitID");
        }
    }
}
