namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeARFieldsfromKeyissue : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.KeyIssues", "ViewsAr");
            DropColumn("dbo.KeyIssues", "TitleAr");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KeyIssues", "TitleAr", c => c.String());
            AddColumn("dbo.KeyIssues", "ViewsAr", c => c.Int(nullable: false));
        }
    }
}
