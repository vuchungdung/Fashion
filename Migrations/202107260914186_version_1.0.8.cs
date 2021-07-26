namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_108 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Content = c.String(nullable: false, maxLength: 600),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comments");
        }
    }
}
