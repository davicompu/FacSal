namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseAdjustmentAmounts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseSalaryAdjustment", "StartingBaseAmount", c => c.Int(nullable: false));
            AddColumn("dbo.BaseSalaryAdjustment", "NewBaseAmount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseSalaryAdjustment", "NewBaseAmount");
            DropColumn("dbo.BaseSalaryAdjustment", "StartingBaseAmount");
        }
    }
}
