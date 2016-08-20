namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class authernameisstringnotinteger : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Authors", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Authors", "FullName", c => c.Int(nullable: false));
        }
    }
}
