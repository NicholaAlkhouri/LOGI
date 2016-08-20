namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArabicToTimeline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Timelines", "TypeAr", c => c.String());
            AddColumn("dbo.Timelines", "PeriodAr", c => c.String());
            AddColumn("dbo.Timelines", "TitleAR", c => c.String());
            AddColumn("dbo.Timelines", "DescriptionAr", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Timelines", "DescriptionAr");
            DropColumn("dbo.Timelines", "TitleAR");
            DropColumn("dbo.Timelines", "PeriodAr");
            DropColumn("dbo.Timelines", "TypeAr");
        }
    }
}
