namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCompositeKeys : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employment", "DepartmentId", "dbo.Department");
            DropForeignKey("dbo.Employment", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Salary", "PersonId", "dbo.Person");
            DropForeignKey("dbo.SalaryModification", new[] { "Salary_PersonId", "Salary_CycleYear" }, "dbo.Salary");
            DropForeignKey("dbo.SpecialSalaryAdjustment", new[] { "PersonId", "CycleYear" }, "dbo.Salary");
            DropForeignKey("dbo.SalaryModification", "Salary_Id", "dbo.Salary");
            DropForeignKey("dbo.SpecialSalaryAdjustment", "SalaryId", "dbo.Salary");
            DropIndex("dbo.Employment", new[] { "PersonId" });
            DropIndex("dbo.Employment", new[] { "DepartmentId" });
            DropIndex("dbo.Salary", new[] { "PersonId" });
            DropIndex("dbo.SalaryModification", new[] { "Salary_PersonId", "Salary_CycleYear" });
            DropIndex("dbo.SpecialSalaryAdjustment", new[] { "PersonId", "CycleYear" });
            RenameColumn(table: "dbo.SalaryModification", name: "Salary_PersonId", newName: "Salary_Id");
            RenameColumn(table: "dbo.SpecialSalaryAdjustment", name: "PersonId", newName: "SalaryId");
            DropPrimaryKey("dbo.Employment");
            DropPrimaryKey("dbo.Salary");
            DropPrimaryKey("dbo.SpecialSalaryAdjustment");
            AddColumn("dbo.Employment", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.Salary", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.SpecialSalaryAdjustment", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.Employment", "PersonId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Employment", "DepartmentId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Salary", "PersonId", c => c.String(maxLength: 128));
            AlterColumn("dbo.SalaryModification", "Salary_Id", c => c.Long());
            AlterColumn("dbo.SpecialSalaryAdjustment", "SalaryId", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Employment", "Id");
            AddPrimaryKey("dbo.Salary", "Id");
            AddPrimaryKey("dbo.SpecialSalaryAdjustment", "Id");
            CreateIndex("dbo.Employment", "PersonId");
            CreateIndex("dbo.Employment", "DepartmentId");
            CreateIndex("dbo.Salary", "PersonId");
            CreateIndex("dbo.SalaryModification", "Salary_Id");
            CreateIndex("dbo.SpecialSalaryAdjustment", "SalaryId");
            AddForeignKey("dbo.Employment", "DepartmentId", "dbo.Department", "Id");
            AddForeignKey("dbo.Employment", "PersonId", "dbo.Person", "Id");
            AddForeignKey("dbo.Salary", "PersonId", "dbo.Person", "Id");
            AddForeignKey("dbo.SalaryModification", "Salary_Id", "dbo.Salary", "Id");
            AddForeignKey("dbo.SpecialSalaryAdjustment", "SalaryId", "dbo.Salary", "Id", cascadeDelete: true);
            DropColumn("dbo.SalaryModification", "Salary_CycleYear");
            DropColumn("dbo.SpecialSalaryAdjustment", "CycleYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SpecialSalaryAdjustment", "CycleYear", c => c.Int(nullable: false));
            AddColumn("dbo.SalaryModification", "Salary_CycleYear", c => c.Int());
            DropForeignKey("dbo.SpecialSalaryAdjustment", "SalaryId", "dbo.Salary");
            DropForeignKey("dbo.SalaryModification", "Salary_Id", "dbo.Salary");
            DropForeignKey("dbo.Salary", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Employment", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Employment", "DepartmentId", "dbo.Department");
            DropIndex("dbo.SpecialSalaryAdjustment", new[] { "SalaryId" });
            DropIndex("dbo.SalaryModification", new[] { "Salary_Id" });
            DropIndex("dbo.Salary", new[] { "PersonId" });
            DropIndex("dbo.Employment", new[] { "DepartmentId" });
            DropIndex("dbo.Employment", new[] { "PersonId" });
            DropPrimaryKey("dbo.SpecialSalaryAdjustment");
            DropPrimaryKey("dbo.Salary");
            DropPrimaryKey("dbo.Employment");
            AlterColumn("dbo.SpecialSalaryAdjustment", "SalaryId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.SalaryModification", "Salary_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Salary", "PersonId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Employment", "DepartmentId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Employment", "PersonId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.SpecialSalaryAdjustment", "Id");
            DropColumn("dbo.Salary", "Id");
            DropColumn("dbo.Employment", "Id");
            AddPrimaryKey("dbo.SpecialSalaryAdjustment", new[] { "PersonId", "CycleYear", "SpecialAdjustmentTypeId" });
            AddPrimaryKey("dbo.Salary", new[] { "PersonId", "CycleYear" });
            AddPrimaryKey("dbo.Employment", new[] { "PersonId", "DepartmentId" });
            RenameColumn(table: "dbo.SpecialSalaryAdjustment", name: "SalaryId", newName: "PersonId");
            RenameColumn(table: "dbo.SalaryModification", name: "Salary_Id", newName: "Salary_PersonId");
            CreateIndex("dbo.SpecialSalaryAdjustment", new[] { "PersonId", "CycleYear" });
            CreateIndex("dbo.SalaryModification", new[] { "Salary_PersonId", "Salary_CycleYear" });
            CreateIndex("dbo.Salary", "PersonId");
            CreateIndex("dbo.Employment", "DepartmentId");
            CreateIndex("dbo.Employment", "PersonId");
            AddForeignKey("dbo.SpecialSalaryAdjustment", "SalaryId", "dbo.Salary", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SalaryModification", "Salary_Id", "dbo.Salary", "Id");
            AddForeignKey("dbo.SpecialSalaryAdjustment", new[] { "PersonId", "CycleYear" }, "dbo.Salary", new[] { "PersonId", "CycleYear" }, cascadeDelete: true);
            AddForeignKey("dbo.SalaryModification", new[] { "Salary_PersonId", "Salary_CycleYear" }, "dbo.Salary", new[] { "PersonId", "CycleYear" });
            AddForeignKey("dbo.Salary", "PersonId", "dbo.Person", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employment", "PersonId", "dbo.Person", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employment", "DepartmentId", "dbo.Department", "Id", cascadeDelete: true);
        }
    }
}
