namespace MusicStreaming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Artists", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Releases", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Songs", "Name", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Songs", "Name", c => c.String());
            AlterColumn("dbo.Releases", "Name", c => c.String());
            AlterColumn("dbo.Artists", "Name", c => c.String());
        }
    }
}
