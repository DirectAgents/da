namespace MissingLinkPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigPackage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SearchesPerMonth = c.Int(nullable: false),
                        MaxResults = c.Int(nullable: false),
                        CostPerMonth = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "PackageId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "PackageId");
            AddForeignKey("dbo.AspNetUsers", "PackageId", "dbo.Packages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PackageId", "dbo.Packages");
            DropIndex("dbo.AspNetUsers", new[] { "PackageId" });
            DropColumn("dbo.AspNetUsers", "PackageId");
            DropTable("dbo.Packages");
        }
    }
}
