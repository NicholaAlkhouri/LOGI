namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addViewsToKeyIssue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeyIssues", "Views", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KeyIssues", "Views");
        }
    }
}
