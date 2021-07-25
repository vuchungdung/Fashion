﻿namespace Fashion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_107 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Discounts", "Value", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Discounts", "Value");
        }
    }
}
