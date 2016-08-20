namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArabicToVacancyModel2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Timelines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Period = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Timelines");
        }
    }
}
