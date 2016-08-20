namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class not_Mandatory_SourceID_AuthorID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KeyIssues", "AuthorID", "dbo.Authors");
            DropForeignKey("dbo.KeyIssues", "SourceID", "dbo.Sources");
            DropIndex("dbo.KeyIssues", new[] { "AuthorID" });
            DropIndex("dbo.KeyIssues", new[] { "SourceID" });
            AlterColumn("dbo.KeyIssues", "AuthorID", c => c.Int());
            AlterColumn("dbo.KeyIssues", "SourceID", c => c.Int());
            CreateIndex("dbo.KeyIssues", "AuthorID");
            CreateIndex("dbo.KeyIssues", "SourceID");
            AddForeignKey("dbo.KeyIssues", "AuthorID", "dbo.Authors", "ID");
            AddForeignKey("dbo.KeyIssues", "SourceID", "dbo.Sources", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KeyIssues", "SourceID", "dbo.Sources");
            DropForeignKey("dbo.KeyIssues", "AuthorID", "dbo.Authors");
            DropIndex("dbo.KeyIssues", new[] { "SourceID" });
            DropIndex("dbo.KeyIssues", new[] { "AuthorID" });
            AlterColumn("dbo.KeyIssues", "SourceID", c => c.Int(nullable: false));
            AlterColumn("dbo.KeyIssues", "AuthorID", c => c.Int(nullable: false));
            CreateIndex("dbo.KeyIssues", "SourceID");
            CreateIndex("dbo.KeyIssues", "AuthorID");
            AddForeignKey("dbo.KeyIssues", "SourceID", "dbo.Sources", "ID", cascadeDelete: true);
            AddForeignKey("dbo.KeyIssues", "AuthorID", "dbo.Authors", "ID", cascadeDelete: true);
        }
    }
}
