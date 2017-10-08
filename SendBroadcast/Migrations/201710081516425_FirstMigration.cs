namespace SendBroadcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BotApplications",
                c => new
                    {
                        BotId = c.Int(nullable: false, identity: true),
                        BotName = c.String(),
                        BotIdentifier = c.String(),
                        BotAccessToken = c.String(),
                    })
                .PrimaryKey(t => t.BotId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BotApplications");
        }
    }
}
