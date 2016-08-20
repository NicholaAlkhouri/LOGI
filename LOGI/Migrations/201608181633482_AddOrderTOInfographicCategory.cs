namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderTOInfographicCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InfographicCategories", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InfographicCategories", "Order");
        }
    }
}
