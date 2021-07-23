namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_102 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "Warranty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Warranty", c => c.Int());
            DropColumn("dbo.Products", "Status");
        }
    }
}
