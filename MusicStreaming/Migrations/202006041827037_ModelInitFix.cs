namespace MusicStreaming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelInitFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Playlists", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Playlist_PlaylistId", "dbo.Playlists");
            DropIndex("dbo.Playlists", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Playlist_PlaylistId" });
            RenameColumn(table: "dbo.Playlists", name: "ApplicationUser_Id", newName: "CreatedBy_Id");
            RenameIndex(table: "dbo.Playlists", name: "IX_ApplicationUser_Id", newName: "IX_CreatedBy_Id");
            CreateTable(
                "dbo.ApplicationUserPlaylists",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Playlist_PlaylistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Playlist_PlaylistId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistId, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Playlist_PlaylistId);
            
            DropColumn("dbo.Playlists", "CreatedBy");
            DropColumn("dbo.Playlists", "User_Id");
            DropColumn("dbo.AspNetUsers", "Playlist_PlaylistId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Playlist_PlaylistId", c => c.Int());
            AddColumn("dbo.Playlists", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Playlists", "CreatedBy", c => c.String());
            DropForeignKey("dbo.ApplicationUserPlaylists", "Playlist_PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.ApplicationUserPlaylists", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserPlaylists", new[] { "Playlist_PlaylistId" });
            DropIndex("dbo.ApplicationUserPlaylists", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserPlaylists");
            RenameIndex(table: "dbo.Playlists", name: "IX_CreatedBy_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Playlists", name: "CreatedBy_Id", newName: "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "Playlist_PlaylistId");
            CreateIndex("dbo.Playlists", "User_Id");
            AddForeignKey("dbo.AspNetUsers", "Playlist_PlaylistId", "dbo.Playlists", "PlaylistId");
            AddForeignKey("dbo.Playlists", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
