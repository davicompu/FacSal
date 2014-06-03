namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fakie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StatusType", "CreatedBy", c => c.String());
            AddColumn("dbo.StatusType", "CreatedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.StatusType", "RowVersion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StatusType", "RowVersion");
            DropColumn("dbo.StatusType", "CreatedDate");
            DropColumn("dbo.StatusType", "CreatedBy");
        }
    }
}
