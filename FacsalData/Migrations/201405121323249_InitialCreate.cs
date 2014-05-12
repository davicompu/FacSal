namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdjustmentType",
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
                "dbo.AppointmentType",
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
                "dbo.DepartmentModification",
                c => new
                    {
                        ModificationId = c.Long(nullable: false, identity: true),
                        UpdatedBy = c.String(),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Department_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ModificationId)
                .ForeignKey("dbo.Department", t => t.Department_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Sequence = c.String(),
                        UnitID = c.String(maxLength: 128),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unit", t => t.UnitID)
                .Index(t => t.UnitID);
            
            CreateTable(
                "dbo.Employment",
                c => new
                    {
                        PersonId = c.String(nullable: false, maxLength: 128),
                        DepartmentId = c.String(nullable: false, maxLength: 128),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.DepartmentId })
                .ForeignKey("dbo.Department", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Pid = c.String(),
                        LastName = c.String(),
                        FirstName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        FullName = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonModification",
                c => new
                    {
                        ModificationId = c.Long(nullable: false, identity: true),
                        UpdatedBy = c.String(),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Person_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ModificationId)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.Salary",
                c => new
                    {
                        PersonID = c.String(nullable: false, maxLength: 128),
                        CycleYear = c.Int(nullable: false),
                        Title = c.String(maxLength: 128),
                        FacultyTypeId = c.Int(nullable: false),
                        RankTypeId = c.Int(nullable: false),
                        AppointmentTypeId = c.Int(nullable: false),
                        FullTimeEquivalent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BannerBaseAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdminAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EminentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PromotionAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MeritIncrease = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SpecialIncrease = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EminentIncrease = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseAdjustReason = c.String(maxLength: 128),
                        BaseAdjustNote = c.String(maxLength: 1024),
                        MeritAdjustReason = c.String(maxLength: 128),
                        MeritAdjustNote = c.String(maxLength: 1024),
                        SpecialAdjustNote = c.String(maxLength: 1024),
                        Comments = c.String(maxLength: 1024),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonID, t.CycleYear })
                .ForeignKey("dbo.AppointmentType", t => t.AppointmentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.FacultyType", t => t.FacultyTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .ForeignKey("dbo.RankType", t => t.RankTypeId, cascadeDelete: true)
                .Index(t => t.PersonID)
                .Index(t => t.FacultyTypeId)
                .Index(t => t.RankTypeId)
                .Index(t => t.AppointmentTypeId);
            
            CreateTable(
                "dbo.FacultyType",
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
                "dbo.SalaryModification",
                c => new
                    {
                        ModificationId = c.Long(nullable: false, identity: true),
                        UpdatedBy = c.String(),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Salary_PersonID = c.String(maxLength: 128),
                        Salary_CycleYear = c.Int(),
                    })
                .PrimaryKey(t => t.ModificationId)
                .ForeignKey("dbo.Salary", t => new { t.Salary_PersonID, t.Salary_CycleYear })
                .Index(t => new { t.Salary_PersonID, t.Salary_CycleYear });
            
            CreateTable(
                "dbo.RankType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Unit",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 1024),
                        Sequence = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        RowVersion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UnitModification",
                c => new
                    {
                        ModificationId = c.Long(nullable: false, identity: true),
                        UpdatedBy = c.String(),
                        UpdatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Unit_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ModificationId)
                .ForeignKey("dbo.Unit", t => t.Unit_Id)
                .Index(t => t.Unit_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Department", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.UnitModification", "Unit_Id", "dbo.Unit");
            DropForeignKey("dbo.DepartmentModification", "Department_Id", "dbo.Department");
            DropForeignKey("dbo.Employment", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Salary", "RankTypeId", "dbo.RankType");
            DropForeignKey("dbo.Salary", "PersonID", "dbo.Person");
            DropForeignKey("dbo.SalaryModification", new[] { "Salary_PersonID", "Salary_CycleYear" }, "dbo.Salary");
            DropForeignKey("dbo.Salary", "FacultyTypeId", "dbo.FacultyType");
            DropForeignKey("dbo.Salary", "AppointmentTypeId", "dbo.AppointmentType");
            DropForeignKey("dbo.PersonModification", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.Employment", "DepartmentId", "dbo.Department");
            DropIndex("dbo.UnitModification", new[] { "Unit_Id" });
            DropIndex("dbo.SalaryModification", new[] { "Salary_PersonID", "Salary_CycleYear" });
            DropIndex("dbo.Salary", new[] { "AppointmentTypeId" });
            DropIndex("dbo.Salary", new[] { "RankTypeId" });
            DropIndex("dbo.Salary", new[] { "FacultyTypeId" });
            DropIndex("dbo.Salary", new[] { "PersonID" });
            DropIndex("dbo.PersonModification", new[] { "Person_Id" });
            DropIndex("dbo.Employment", new[] { "DepartmentId" });
            DropIndex("dbo.Employment", new[] { "PersonId" });
            DropIndex("dbo.Department", new[] { "UnitID" });
            DropIndex("dbo.DepartmentModification", new[] { "Department_Id" });
            DropTable("dbo.UnitModification");
            DropTable("dbo.Unit");
            DropTable("dbo.RankType");
            DropTable("dbo.SalaryModification");
            DropTable("dbo.FacultyType");
            DropTable("dbo.Salary");
            DropTable("dbo.PersonModification");
            DropTable("dbo.Person");
            DropTable("dbo.Employment");
            DropTable("dbo.Department");
            DropTable("dbo.DepartmentModification");
            DropTable("dbo.AppointmentType");
            DropTable("dbo.AdjustmentType");
        }
    }
}
