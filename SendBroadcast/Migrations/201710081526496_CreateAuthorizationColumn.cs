namespace SendBroadcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAuthorizationColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BotApplications", "BotAuthorizationTokenApi", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BotApplications", "BotAuthorizationTokenApi");
        }
    }
}
