namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInfographicMOdelsImplementaion2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Infographics", "InfographicCategory_ID", "dbo.InfographicCategories");
            DropIndex("dbo.Infographics", new[] { "InfographicCategory_ID" });
            RenameColumn(table: "dbo.Infographics", name: "InfographicCategory_ID", newName: "InfographicCategoryID");
            AddColumn("dbo.Infographics", "NameAr", c => c.String());
            AlterColumn("dbo.Infographics", "InfographicCategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Infographics", "InfographicCategoryID");
            AddForeignKey("dbo.Infographics", "InfographicCategoryID", "dbo.InfographicCategories", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Infographics", "InfographicCategoryID", "dbo.InfographicCategories");
            DropIndex("dbo.Infographics", new[] { "InfographicCategoryID" });
            AlterColumn("dbo.Infographics", "InfographicCategoryID", c => c.Int());
            DropColumn("dbo.Infographics", "NameAr");
            RenameColumn(table: "dbo.Infographics", name: "InfographicCategoryID", newName: "InfographicCategory_ID");
            CreateIndex("dbo.Infographics", "InfographicCategory_ID");
            AddForeignKey("dbo.Infographics", "InfographicCategory_ID", "dbo.InfographicCategories", "ID");
        }
    }
}
