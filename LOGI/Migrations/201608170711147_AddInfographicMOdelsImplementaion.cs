namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInfographicMOdelsImplementaion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InfographicCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameAr = c.String(),
                        Description = c.String(),
                        DescriptionAr = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Infographics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                        ImageURL = c.String(),
                        ImageURLAr = c.String(),
                        ThumbURL = c.String(),
                        ThumbURLAr = c.String(),
                        FriendlyURL = c.String(),
                        FriendlyURLAr = c.String(),
                        InfographicCategory_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.InfographicCategories", t => t.InfographicCategory_ID)
                .Index(t => t.InfographicCategory_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Infographics", "InfographicCategory_ID", "dbo.InfographicCategories");
            DropIndex("dbo.Infographics", new[] { "InfographicCategory_ID" });
            DropTable("dbo.Infographics");
            DropTable("dbo.InfographicCategories");
        }
    }
}
