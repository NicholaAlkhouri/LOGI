namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsInNewsColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeyIssues", "IsInNews", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KeyIssues", "IsInNews");
        }
    }
}
