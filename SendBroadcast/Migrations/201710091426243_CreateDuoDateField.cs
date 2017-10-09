namespace SendBroadcast.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDuoDateField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Broadcasts", "DuoDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Broadcasts", "DuoDate");
        }
    }
}
