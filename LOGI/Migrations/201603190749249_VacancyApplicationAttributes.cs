namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VacancyApplicationAttributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VacancyApplications", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.VacancyApplications", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.VacancyApplications", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.VacancyApplications", "Message", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VacancyApplications", "Message", c => c.String());
            AlterColumn("dbo.VacancyApplications", "Country", c => c.String());
            AlterColumn("dbo.VacancyApplications", "Email", c => c.String());
            AlterColumn("dbo.VacancyApplications", "FullName", c => c.String());
        }
    }
}
