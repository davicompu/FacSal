namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StatusType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatusType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Person", "StatusTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Person", "StatusTypeId");
            AddForeignKey("dbo.Person", "StatusTypeId", "dbo.StatusType", "Id", cascadeDelete: true);
            DropColumn("dbo.Person", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "IsActive", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Person", "StatusTypeId", "dbo.StatusType");
            DropIndex("dbo.Person", new[] { "StatusTypeId" });
            DropColumn("dbo.Person", "StatusTypeId");
            DropTable("dbo.StatusType");
        }
    }
}
