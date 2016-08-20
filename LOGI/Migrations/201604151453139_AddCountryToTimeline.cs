namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryToTimeline : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimelineCountries",
                c => new
                    {
                        Timeline_ID = c.Int(nullable: false),
                        Country_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Timeline_ID, t.Country_ID })
                .ForeignKey("dbo.Timelines", t => t.Timeline_ID, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.Country_ID, cascadeDelete: true)
                .Index(t => t.Timeline_ID)
                .Index(t => t.Country_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimelineCountries", "Country_ID", "dbo.Countries");
            DropForeignKey("dbo.TimelineCountries", "Timeline_ID", "dbo.Timelines");
            DropIndex("dbo.TimelineCountries", new[] { "Country_ID" });
            DropIndex("dbo.TimelineCountries", new[] { "Timeline_ID" });
            DropTable("dbo.TimelineCountries");
        }
    }
}
