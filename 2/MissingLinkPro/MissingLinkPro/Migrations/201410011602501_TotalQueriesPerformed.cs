namespace MissingLinkPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalQueriesPerformed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "TotalQueriesPerformed", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TotalQueriesPerformed");
        }
    }
}
