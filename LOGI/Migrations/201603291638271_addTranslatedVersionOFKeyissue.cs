namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTranslatedVersionOFKeyissue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KeyIssues", "Language", c => c.String());
            AddColumn("dbo.KeyIssues", "OriginalId", c => c.Int());
            CreateIndex("dbo.KeyIssues", "OriginalId");
            AddForeignKey("dbo.KeyIssues", "OriginalId", "dbo.KeyIssues", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KeyIssues", "OriginalId", "dbo.KeyIssues");
            DropIndex("dbo.KeyIssues", new[] { "OriginalId" });
            DropColumn("dbo.KeyIssues", "OriginalId");
            DropColumn("dbo.KeyIssues", "Language");
        }
    }
}
