namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFinancialcolor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinancialReportEntries", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinancialReportEntries", "Color");
        }
    }
}
