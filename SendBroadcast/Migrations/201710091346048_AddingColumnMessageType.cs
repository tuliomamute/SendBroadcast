namespace SendBroadcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingColumnMessageType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Broadcasts", "ContentType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Broadcasts", "ContentType");
        }
    }
}
