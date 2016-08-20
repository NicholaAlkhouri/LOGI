namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddarabictitleToKeyIssue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeyIssues", "TitleAr", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KeyIssues", "TitleAr");
        }
    }
}
