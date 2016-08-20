namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArabicToTimelineRemoveTypar : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Timelines", "TypeAr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Timelines", "TypeAr", c => c.String());
        }
    }
}
