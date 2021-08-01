namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_111 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "AddressMore", c => c.String());
            AddColumn("dbo.OrderDetails", "ColorId", c => c.Int(nullable: false));
            AddColumn("dbo.OrderDetails", "SizeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "SizeId");
            DropColumn("dbo.OrderDetails", "ColorId");
            DropColumn("dbo.Customers", "AddressMore");
        }
    }
}
