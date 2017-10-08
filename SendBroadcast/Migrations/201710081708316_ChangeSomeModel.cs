namespace SendBroadcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSomeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Broadcasts",
                c => new
                    {
                        BroadcastId = c.Int(nullable: false, identity: true),
                        DistributionList = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.BroadcastId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Broadcasts");
        }
    }
}
