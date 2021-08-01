namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_110 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Discounts", "Value", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Discounts", "Value", c => c.String());
        }
    }
}
