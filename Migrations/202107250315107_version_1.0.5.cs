namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_105 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ActivePromotion", c => c.Boolean());
            DropColumn("dbo.Products", "HomeFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "HomeFlag", c => c.Boolean());
            DropColumn("dbo.Products", "ActivePromotion");
        }
    }
}
