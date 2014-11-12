namespace MissingLinkPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PackageMaxLengthAnnotation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Packages", "StatementDescription", c => c.String(maxLength: 22));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Packages", "StatementDescription", c => c.String());
        }
    }
}
