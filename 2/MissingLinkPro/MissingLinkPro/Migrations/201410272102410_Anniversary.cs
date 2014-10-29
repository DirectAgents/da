namespace MissingLinkPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Anniversary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Anniversary", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Anniversary");
        }
    }
}
