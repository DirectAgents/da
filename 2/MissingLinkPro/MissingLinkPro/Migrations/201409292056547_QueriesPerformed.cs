namespace MissingLinkPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QueriesPerformed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "QueriesPerformed", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "QueriesPerformed");
        }
    }
}
