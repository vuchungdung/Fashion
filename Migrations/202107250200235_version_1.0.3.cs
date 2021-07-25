namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_103 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        ColorId = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Sizes", t => t.SizeId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.ColorId)
                .Index(t => t.SizeId);
            
            CreateTable(
                "dbo.Sizes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Products", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProductOptions", "SizeId", "dbo.Sizes");
            DropForeignKey("dbo.ProductOptions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductOptions", "ColorId", "dbo.Colors");
            DropIndex("dbo.ProductOptions", new[] { "SizeId" });
            DropIndex("dbo.ProductOptions", new[] { "ColorId" });
            DropIndex("dbo.ProductOptions", new[] { "ProductId" });
            DropTable("dbo.Sizes");
            DropTable("dbo.ProductOptions");
            DropTable("dbo.Colors");
        }
    }
}
