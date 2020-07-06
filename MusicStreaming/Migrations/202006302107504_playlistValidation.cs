namespace MusicStreaming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class playlistValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Playlists", "Name", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Playlists", "Name", c => c.String());
        }
    }
}
