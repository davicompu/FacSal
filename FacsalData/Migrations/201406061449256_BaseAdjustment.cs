namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseAdjustment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseSalaryAdjustment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SalaryId = c.Long(nullable: false),
                        BaseAdjustmentTypeId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseAdjustmentType", t => t.BaseAdjustmentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Salary", t => t.SalaryId, cascadeDelete: true)
                .Index(t => t.SalaryId)
                .Index(t => t.BaseAdjustmentTypeId);
            
            CreateTable(
                "dbo.BaseAdjustmentType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Salary", "BaseSalaryAdjustmentNote", c => c.String(maxLength: 1024));
            AddColumn("dbo.Salary", "MeritAdjustmentNote", c => c.String(maxLength: 1024));
            AddColumn("dbo.Salary", "SpecialAdjustmentNote", c => c.String(maxLength: 1024));
            AddColumn("dbo.SpecialSalaryAdjustment", "CreatedBy", c => c.String());
            AddColumn("dbo.SpecialSalaryAdjustment", "CreatedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.SpecialSalaryAdjustment", "RowVersion", c => c.Int(nullable: false));
            DropColumn("dbo.Salary", "BaseAdjustReason");
            DropColumn("dbo.Salary", "BaseAdjustNote");
            DropColumn("dbo.Salary", "MeritAdjustNote");
            DropColumn("dbo.Salary", "SpecialAdjustNote");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Salary", "SpecialAdjustNote", c => c.String(maxLength: 1024));
            AddColumn("dbo.Salary", "MeritAdjustNote", c => c.String(maxLength: 1024));
            AddColumn("dbo.Salary", "BaseAdjustNote", c => c.String(maxLength: 1024));
            AddColumn("dbo.Salary", "BaseAdjustReason", c => c.String(maxLength: 128));
            DropForeignKey("dbo.BaseSalaryAdjustment", "SalaryId", "dbo.Salary");
            DropForeignKey("dbo.BaseSalaryAdjustment", "BaseAdjustmentTypeId", "dbo.BaseAdjustmentType");
            DropIndex("dbo.BaseSalaryAdjustment", new[] { "BaseAdjustmentTypeId" });
            DropIndex("dbo.BaseSalaryAdjustment", new[] { "SalaryId" });
            DropColumn("dbo.SpecialSalaryAdjustment", "RowVersion");
            DropColumn("dbo.SpecialSalaryAdjustment", "CreatedDate");
            DropColumn("dbo.SpecialSalaryAdjustment", "CreatedBy");
            DropColumn("dbo.Salary", "SpecialAdjustmentNote");
            DropColumn("dbo.Salary", "MeritAdjustmentNote");
            DropColumn("dbo.Salary", "BaseSalaryAdjustmentNote");
            DropTable("dbo.BaseAdjustmentType");
            DropTable("dbo.BaseSalaryAdjustment");
        }
    }
}
