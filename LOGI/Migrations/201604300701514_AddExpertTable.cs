namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExpertTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Experts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ApplyDate = c.DateTime(nullable: false),
                        FullName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        Expertise = c.String(),
                        Education = c.String(),
                        PoliticalAffiliation = c.String(),
                        WhyToJoin = c.String(),
                        Message = c.String(),
                        ResumeURL = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Experts");
        }
    }
}
