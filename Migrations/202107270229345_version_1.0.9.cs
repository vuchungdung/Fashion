namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_109 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Status", c => c.Int(nullable: false));
        }
    }
}
