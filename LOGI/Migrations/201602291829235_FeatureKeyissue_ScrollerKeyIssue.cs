namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeatureKeyissue_ScrollerKeyIssue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeyIssues", "IsFeatured", c => c.Boolean(nullable: false));
            AddColumn("dbo.KeyIssues", "InScroller", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KeyIssues", "InScroller");
            DropColumn("dbo.KeyIssues", "IsFeatured");
        }
    }
}
