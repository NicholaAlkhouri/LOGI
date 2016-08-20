namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFinancilaandTeammembers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinancialReportEntries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Value = c.Double(nullable: false),
                        Title = c.String(),
                        FinancialReport_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FinancialReports", t => t.FinancialReport_ID)
                .Index(t => t.FinancialReport_ID);
            
            CreateTable(
                "dbo.FinancialReports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Year = c.String(),
                        FileURL = c.String(),
                        Funding = c.Double(nullable: false),
                        Expences = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        ImageURL = c.String(),
                        IsOnline = c.Boolean(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Expertise = c.String(),
                        CurrentRole = c.String(),
                        Career = c.String(),
                        Education = c.String(),
                        FullNameAr = c.String(),
                        ExpertiseAr = c.String(),
                        CurrentRoleAr = c.String(),
                        CareerAr = c.String(),
                        EducationAr = c.String(),
                        Email = c.String(),
                        Twitter = c.String(),
                        Linkedin = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FinancialReportEntries", "FinancialReport_ID", "dbo.FinancialReports");
            DropIndex("dbo.FinancialReportEntries", new[] { "FinancialReport_ID" });
            DropTable("dbo.TeamMembers");
            DropTable("dbo.FinancialReports");
            DropTable("dbo.FinancialReportEntries");
        }
    }
}
