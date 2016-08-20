namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addArabicTitleToReportEntry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinancialReportEntries", "TitleAr", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinancialReportEntries", "TitleAr");
        }
    }
}
