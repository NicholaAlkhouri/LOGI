namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addarabictosomeentities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "FullNameAr", c => c.String());
            AddColumn("dbo.Sources", "NameAr", c => c.String());
            AddColumn("dbo.Tags", "Language", c => c.String());
            AddColumn("dbo.Topics", "NameAr", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topics", "NameAr");
            DropColumn("dbo.Tags", "Language");
            DropColumn("dbo.Sources", "NameAr");
            DropColumn("dbo.Authors", "FullNameAr");
        }
    }
}
