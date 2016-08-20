namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVacancy : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vacancies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobNumber = c.String(),
                        DeadLine = c.DateTime(nullable: false),
                        Title = c.String(),
                        RoleSummary = c.String(),
                        EssentialResponsibilities = c.String(),
                        ProjectDeadline = c.String(),
                        DesiredCharacteristics = c.String(),
                        Qualifications = c.String(),
                        Salary = c.String(),
                        Outcome = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VacancyApplications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Email = c.String(),
                        Country = c.String(),
                        Phone = c.String(),
                        Message = c.String(),
                        ResumeURL = c.String(),
                        Vacancy_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Vacancies", t => t.Vacancy_ID)
                .Index(t => t.Vacancy_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VacancyApplications", "Vacancy_ID", "dbo.Vacancies");
            DropIndex("dbo.VacancyApplications", new[] { "Vacancy_ID" });
            DropTable("dbo.VacancyApplications");
            DropTable("dbo.Vacancies");
        }
    }
}
