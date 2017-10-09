namespace SendBroadcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRelationshipBroadcast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Broadcasts", "BotId", c => c.Int(nullable: false));
            CreateIndex("dbo.Broadcasts", "BotId");
            AddForeignKey("dbo.Broadcasts", "BotId", "dbo.BotApplications", "BotId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Broadcasts", "BotId", "dbo.BotApplications");
            DropIndex("dbo.Broadcasts", new[] { "BotId" });
            DropColumn("dbo.Broadcasts", "BotId");
        }
    }
}
