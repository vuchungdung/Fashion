namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_104 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Discounts", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Products", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ProductOptions", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "CreatedDate", c => c.DateTime());
            DropColumn("dbo.Users", "CreatedDate");
            DropColumn("dbo.ProductOptions", "CreatedDate");
            DropColumn("dbo.Products", "CreatedDate");
            DropColumn("dbo.Discounts", "CreatedDate");
            DropColumn("dbo.Customers", "CreatedDate");
            DropColumn("dbo.Categories", "CreatedDate");
        }
    }
}
