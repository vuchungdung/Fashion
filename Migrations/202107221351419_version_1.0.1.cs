namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_101 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "QrCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "QrCode");
        }
    }
}
