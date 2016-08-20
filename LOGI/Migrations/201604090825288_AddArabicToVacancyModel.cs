namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArabicToVacancyModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vacancies", "TitleAr", c => c.String());
            AddColumn("dbo.Vacancies", "RoleSummaryAr", c => c.String());
            AddColumn("dbo.Vacancies", "EssentialResponsibilitiesAr", c => c.String());
            AddColumn("dbo.Vacancies", "ProjectDeadlineAr", c => c.String());
            AddColumn("dbo.Vacancies", "DesiredCharacteristicsAr", c => c.String());
            AddColumn("dbo.Vacancies", "QualificationsAr", c => c.String());
            AddColumn("dbo.Vacancies", "SalaryAr", c => c.String());
            AddColumn("dbo.Vacancies", "OutcomeAr", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vacancies", "OutcomeAr");
            DropColumn("dbo.Vacancies", "SalaryAr");
            DropColumn("dbo.Vacancies", "QualificationsAr");
            DropColumn("dbo.Vacancies", "DesiredCharacteristicsAr");
            DropColumn("dbo.Vacancies", "ProjectDeadlineAr");
            DropColumn("dbo.Vacancies", "EssentialResponsibilitiesAr");
            DropColumn("dbo.Vacancies", "RoleSummaryAr");
            DropColumn("dbo.Vacancies", "TitleAr");
        }
    }
}
