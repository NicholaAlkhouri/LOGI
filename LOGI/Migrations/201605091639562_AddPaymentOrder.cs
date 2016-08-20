namespace LOGI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPaymentOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PaymentOrders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        Amount = c.String(),
                        Currency = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PaymentOrders");
        }
    }
}
