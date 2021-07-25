namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_106 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Status", c => c.Int(nullable: false));
        }
    }
}
