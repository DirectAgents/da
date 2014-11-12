namespace MissingLinkPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PackageStatementDesc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Packages", "StatementDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Packages", "StatementDescription");
        }
    }
}
