namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageToTimeline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Timelines", "ImageURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Timelines", "ImageURL");
        }
    }
}
