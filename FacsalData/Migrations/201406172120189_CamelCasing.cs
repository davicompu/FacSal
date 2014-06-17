namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CamelCasing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Department", "ValSeq", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.RankType", "ValSeq", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.Unit", "ValSeq", c => c.String(nullable: false, maxLength: 3));
            DropColumn("dbo.Department", "val");
            DropColumn("dbo.RankType", "val");
            DropColumn("dbo.Unit", "val");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Unit", "val", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.RankType", "val", c => c.String(nullable: false, maxLength: 3));
            AddColumn("dbo.Department", "val", c => c.String(nullable: false, maxLength: 3));
            DropColumn("dbo.Unit", "ValSeq");
            DropColumn("dbo.RankType", "ValSeq");
            DropColumn("dbo.Department", "ValSeq");
        }
    }
}
