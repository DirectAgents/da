namespace MissingLinkPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DateTimeStamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "dt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "dt", c => c.DateTime(nullable: false));
            DropColumn("dbo.AspNetUsers", "DateTimeStamp");
        }
    }
}
