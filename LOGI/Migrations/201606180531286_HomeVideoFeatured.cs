namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeVideoFeatured : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeyIssues", "IsHomeVideoFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KeyIssues", "IsHomeVideoFeatured");
        }
    }
}
