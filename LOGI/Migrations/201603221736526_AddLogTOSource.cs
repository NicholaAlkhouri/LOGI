namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogTOSource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sources", "LogoURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sources", "LogoURL");
        }
    }
}
