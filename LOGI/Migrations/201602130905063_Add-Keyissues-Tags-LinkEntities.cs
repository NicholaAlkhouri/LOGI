namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKeyissuesTagsLinkEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KeyIssues",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        IsOnline = c.Boolean(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        FriendlyURL = c.String(),
                        FileURL = c.String(),
                        FeatureImageURL = c.String(),
                        FeatureVideoLink = c.String(),
                        MetaTitle = c.String(),
                        MetaDescription = c.String(),
                        AuthorID = c.Int(nullable: false),
                        SourceID = c.Int(nullable: false),
                        TopicID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Authors", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.Sources", t => t.SourceID, cascadeDelete: true)
                .ForeignKey("dbo.Topics", t => t.TopicID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.SourceID)
                .Index(t => t.TopicID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        KeyIssueID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.KeyIssues", t => t.KeyIssueID, cascadeDelete: true)
                .Index(t => t.KeyIssueID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KeyIssues", "TopicID", "dbo.Topics");
            DropForeignKey("dbo.Tags", "KeyIssueID", "dbo.KeyIssues");
            DropForeignKey("dbo.KeyIssues", "SourceID", "dbo.Sources");
            DropForeignKey("dbo.KeyIssues", "AuthorID", "dbo.Authors");
            DropIndex("dbo.Tags", new[] { "KeyIssueID" });
            DropIndex("dbo.KeyIssues", new[] { "TopicID" });
            DropIndex("dbo.KeyIssues", new[] { "SourceID" });
            DropIndex("dbo.KeyIssues", new[] { "AuthorID" });
            DropTable("dbo.Tags");
            DropTable("dbo.KeyIssues");
        }
    }
}
