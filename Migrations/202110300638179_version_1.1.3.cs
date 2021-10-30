namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_113 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Customers", "Email", c => c.String(nullable: false, maxLength: 250, unicode: false));
            AlterColumn("dbo.Customers", "Address", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.Customers", "Phone", c => c.String(nullable: false, maxLength: 10, unicode: false));
            AlterColumn("dbo.Customers", "Username", c => c.String(maxLength: 250));
            AlterColumn("dbo.Products", "Content", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Tags", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Password", c => c.String(nullable: false));
            CreateIndex("dbo.Customers", "Name", unique: true);
            CreateIndex("dbo.Customers", "Email", unique: true);
            CreateIndex("dbo.Customers", "Phone", unique: true);
            CreateIndex("dbo.Customers", "Username", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Customers", new[] { "Username" });
            DropIndex("dbo.Customers", new[] { "Phone" });
            DropIndex("dbo.Customers", new[] { "Email" });
            DropIndex("dbo.Customers", new[] { "Name" });
            AlterColumn("dbo.Users", "Password", c => c.String());
            AlterColumn("dbo.Tags", "Name", c => c.String());
            AlterColumn("dbo.Products", "Content", c => c.String());
            AlterColumn("dbo.Customers", "Username", c => c.String(nullable: false));
            AlterColumn("dbo.Customers", "Phone", c => c.String());
            AlterColumn("dbo.Customers", "Address", c => c.String());
            AlterColumn("dbo.Customers", "Email", c => c.String());
            AlterColumn("dbo.Customers", "Name", c => c.String(nullable: false));
        }
    }
}
