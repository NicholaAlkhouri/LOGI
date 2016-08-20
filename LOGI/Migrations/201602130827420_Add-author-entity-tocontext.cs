namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addauthorentitytocontext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullName = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Authors");
        }
    }
}
