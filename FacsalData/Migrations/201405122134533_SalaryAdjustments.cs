namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalaryAdjustments : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AdjustmentType", newName: "MeritAdjustmentType");
            DropIndex("dbo.Salary", new[] { "PersonID" });
            DropIndex("dbo.SalaryModification", new[] { "Salary_PersonID", "Salary_CycleYear" });
            CreateTable(
                "dbo.SpecialSalaryAdjustment",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 128),
                        CycleYear = c.Int(nullable: false),
                        SpecialAdjustmentTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.CycleYear, t.SpecialAdjustmentTypeId })
                .ForeignKey("dbo.SpecialAdjustmentType", t => t.SpecialAdjustmentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Salary", t => new { t.PersonId, t.CycleYear }, cascadeDelete: true)
                .Index(t => new { t.PersonId, t.CycleYear })
                .Index(t => t.SpecialAdjustmentTypeId);
            
            CreateTable(
                "dbo.SpecialAdjustmentType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Salary", "MeritAdjustmentTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Salary", "BannerBaseAmount", c => c.Int(nullable: false));
            AlterColumn("dbo.Salary", "BaseAmount", c => c.Int(nullable: false));
            AlterColumn("dbo.Salary", "AdminAmount", c => c.Int(nullable: false));
            AlterColumn("dbo.Salary", "EminentAmount", c => c.Int(nullable: false));
            AlterColumn("dbo.Salary", "PromotionAmount", c => c.Int(nullable: false));
            AlterColumn("dbo.Salary", "MeritIncrease", c => c.Int(nullable: false));
            AlterColumn("dbo.Salary", "SpecialIncrease", c => c.Int(nullable: false));
            AlterColumn("dbo.Salary", "EminentIncrease", c => c.Int(nullable: false));
            CreateIndex("dbo.Salary", "PersonId");
            CreateIndex("dbo.Salary", "MeritAdjustmentTypeId");
            CreateIndex("dbo.SalaryModification", new[] { "Salary_PersonId", "Salary_CycleYear" });
            AddForeignKey("dbo.Salary", "MeritAdjustmentTypeId", "dbo.MeritAdjustmentType", "Id", cascadeDelete: true);
            DropColumn("dbo.Salary", "MeritAdjustReason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Salary", "MeritAdjustReason", c => c.String(maxLength: 128));
            DropForeignKey("dbo.SpecialSalaryAdjustment", new[] { "PersonId", "CycleYear" }, "dbo.Salary");
            DropForeignKey("dbo.SpecialSalaryAdjustment", "SpecialAdjustmentTypeId", "dbo.SpecialAdjustmentType");
            DropForeignKey("dbo.Salary", "MeritAdjustmentTypeId", "dbo.MeritAdjustmentType");
            DropIndex("dbo.SpecialSalaryAdjustment", new[] { "SpecialAdjustmentTypeId" });
            DropIndex("dbo.SpecialSalaryAdjustment", new[] { "PersonId", "CycleYear" });
            DropIndex("dbo.SalaryModification", new[] { "Salary_PersonId", "Salary_CycleYear" });
            DropIndex("dbo.Salary", new[] { "MeritAdjustmentTypeId" });
            DropIndex("dbo.Salary", new[] { "PersonId" });
            AlterColumn("dbo.Salary", "EminentIncrease", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Salary", "SpecialIncrease", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Salary", "MeritIncrease", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Salary", "PromotionAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Salary", "EminentAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Salary", "AdminAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Salary", "BaseAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Salary", "BannerBaseAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Salary", "MeritAdjustmentTypeId");
            DropTable("dbo.SpecialAdjustmentType");
            DropTable("dbo.SpecialSalaryAdjustment");
            CreateIndex("dbo.SalaryModification", new[] { "Salary_PersonID", "Salary_CycleYear" });
            CreateIndex("dbo.Salary", "PersonID");
            RenameTable(name: "dbo.MeritAdjustmentType", newName: "AdjustmentType");
        }
    }
}
