namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleAssignment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pid = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleAssignment", "UserId", "dbo.User");
            DropForeignKey("dbo.RoleAssignment", "RoleId", "dbo.Role");
            DropIndex("dbo.RoleAssignment", new[] { "UserId" });
            DropIndex("dbo.RoleAssignment", new[] { "RoleId" });
            DropTable("dbo.User");
            DropTable("dbo.Role");
            DropTable("dbo.RoleAssignment");
        }
    }
}
