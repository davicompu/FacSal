namespace FacsalData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalaryBannerBaseNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Salary", "BannerBaseAmount", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Salary", "BannerBaseAmount", c => c.Int(nullable: false));
        }
    }
}
