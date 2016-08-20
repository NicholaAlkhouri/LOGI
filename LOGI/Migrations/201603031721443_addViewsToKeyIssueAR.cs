namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addViewsToKeyIssueAR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeyIssues", "ViewsAr", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KeyIssues", "ViewsAr");
        }
    }
}
