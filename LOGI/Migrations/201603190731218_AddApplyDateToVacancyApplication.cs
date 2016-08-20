namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplyDateToVacancyApplication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VacancyApplications", "ApplyDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VacancyApplications", "ApplyDate");
        }
    }
}
