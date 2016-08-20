namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMetaDescriptionToINfographic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Infographics", "MetaDescription", c => c.String());
            AddColumn("dbo.Infographics", "MetaDescriptionAr", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Infographics", "MetaDescriptionAr");
            DropColumn("dbo.Infographics", "MetaDescription");
        }
    }
}
