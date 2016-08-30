namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampaign : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        MobileImageUrl = c.String(),
                        URL = c.String(),
                        Location = c.Int(nullable: false),
                        IsOnline = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Campaigns");
        }
    }
}
