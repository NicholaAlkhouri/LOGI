namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCountryAndType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameAr = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameAr = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CountryKeyIssues",
                c => new
                    {
                        Country_ID = c.Int(nullable: false),
                        KeyIssue_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Country_ID, t.KeyIssue_ID })
                .ForeignKey("dbo.Countries", t => t.Country_ID, cascadeDelete: true)
                .ForeignKey("dbo.KeyIssues", t => t.KeyIssue_ID, cascadeDelete: true)
                .Index(t => t.Country_ID)
                .Index(t => t.KeyIssue_ID);
            
            AddColumn("dbo.KeyIssues", "TypeID", c => c.Int());
            CreateIndex("dbo.KeyIssues", "TypeID");
            AddForeignKey("dbo.KeyIssues", "TypeID", "dbo.Types", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KeyIssues", "TypeID", "dbo.Types");
            DropForeignKey("dbo.CountryKeyIssues", "KeyIssue_ID", "dbo.KeyIssues");
            DropForeignKey("dbo.CountryKeyIssues", "Country_ID", "dbo.Countries");
            DropIndex("dbo.CountryKeyIssues", new[] { "KeyIssue_ID" });
            DropIndex("dbo.CountryKeyIssues", new[] { "Country_ID" });
            DropIndex("dbo.KeyIssues", new[] { "TypeID" });
            DropColumn("dbo.KeyIssues", "TypeID");
            DropTable("dbo.CountryKeyIssues");
            DropTable("dbo.Types");
            DropTable("dbo.Countries");
        }
    }
}
