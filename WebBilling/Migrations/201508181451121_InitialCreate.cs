namespace WebBilling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        businessPricing = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Job",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        startingTech = c.String(),
                        CustomerID = c.Int(nullable: false),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(),
                        totalPrice = c.Double(nullable: false),
                        inProgress = c.Boolean(nullable: false),
                        wasBilled = c.Boolean(nullable: false),
                        billingNotes = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.NarrationLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        line = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Job", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        description = c.String(),
                        quantity = c.Int(nullable: false),
                        purchasedPrice = c.Double(nullable: false),
                        soldPrice = c.Double(nullable: false),
                        JobID = c.Int(nullable: false),
                        techName = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Job", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.TechNotesLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JobID = c.Int(nullable: false),
                        techName = c.String(),
                        line = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Job", t => t.JobID, cascadeDelete: true)
                .Index(t => t.JobID);
            
            CreateTable(
                "dbo.TechAccount",
                c => new
                    {
                        userName = c.String(nullable: false, maxLength: 128),
                        firstName = c.String(),
                        lastName = c.String(),
                        initials = c.String(),
                        hashPassword = c.String(),
                        permissionGroup = c.String(),
                    })
                .PrimaryKey(t => t.userName);
            
            CreateTable(
                "dbo.Tolerances",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        min = c.Double(nullable: false),
                        max = c.Double(nullable: false),
                        notes = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TechNotesLine", "JobID", "dbo.Job");
            DropForeignKey("dbo.Parts", "JobID", "dbo.Job");
            DropForeignKey("dbo.NarrationLine", "JobID", "dbo.Job");
            DropForeignKey("dbo.Job", "CustomerID", "dbo.Customer");
            DropIndex("dbo.TechNotesLine", new[] { "JobID" });
            DropIndex("dbo.Parts", new[] { "JobID" });
            DropIndex("dbo.NarrationLine", new[] { "JobID" });
            DropIndex("dbo.Job", new[] { "CustomerID" });
            DropTable("dbo.Tolerances");
            DropTable("dbo.TechAccount");
            DropTable("dbo.TechNotesLine");
            DropTable("dbo.Parts");
            DropTable("dbo.NarrationLine");
            DropTable("dbo.Job");
            DropTable("dbo.Customer");
        }
    }
}
