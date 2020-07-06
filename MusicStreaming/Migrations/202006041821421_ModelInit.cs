namespace MusicStreaming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImgUrl = c.String(),
                    })
                .PrimaryKey(t => t.ArtistId);
            
            CreateTable(
                "dbo.Releases",
                c => new
                    {
                        ReleaseId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Type = c.Byte(nullable: false),
                        ArtistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReleaseId)
                .ForeignKey("dbo.Artists", t => t.ArtistID, cascadeDelete: true)
                .Index(t => t.ArtistID);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        SongId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SongUrl = c.String(),
                        GenreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SongId)
                .ForeignKey("dbo.Genres", t => t.GenreId, cascadeDelete: true)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        GenreId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.GenreId);
            
            CreateTable(
                "dbo.Playlists",
                c => new
                    {
                        PlaylistId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ImageUrl = c.String(),
                        CreatedBy = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PlaylistId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Playlist_PlaylistId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Playlist_PlaylistId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SongArtists",
                c => new
                    {
                        Song_SongId = c.Int(nullable: false),
                        Artist_ArtistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongId, t.Artist_ArtistId })
                .ForeignKey("dbo.Songs", t => t.Song_SongId, cascadeDelete: true)
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistId, cascadeDelete: true)
                .Index(t => t.Song_SongId)
                .Index(t => t.Artist_ArtistId);
            
            CreateTable(
                "dbo.PlaylistSongs",
                c => new
                    {
                        Playlist_PlaylistId = c.Int(nullable: false),
                        Song_SongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Playlist_PlaylistId, t.Song_SongId })
                .ForeignKey("dbo.Playlists", t => t.Playlist_PlaylistId, cascadeDelete: true)
                .ForeignKey("dbo.Songs", t => t.Song_SongId, cascadeDelete: true)
                .Index(t => t.Playlist_PlaylistId)
                .Index(t => t.Song_SongId);
            
            CreateTable(
                "dbo.SongReleases",
                c => new
                    {
                        Song_SongId = c.Int(nullable: false),
                        Release_ReleaseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Song_SongId, t.Release_ReleaseId })
                .ForeignKey("dbo.Songs", t => t.Song_SongId, cascadeDelete: true)
                .ForeignKey("dbo.Releases", t => t.Release_ReleaseId, cascadeDelete: true)
                .Index(t => t.Song_SongId)
                .Index(t => t.Release_ReleaseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.SongReleases", "Release_ReleaseId", "dbo.Releases");
            DropForeignKey("dbo.SongReleases", "Song_SongId", "dbo.Songs");
            DropForeignKey("dbo.AspNetUsers", "Playlist_PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.Playlists", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Playlists", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PlaylistSongs", "Song_SongId", "dbo.Songs");
            DropForeignKey("dbo.PlaylistSongs", "Playlist_PlaylistId", "dbo.Playlists");
            DropForeignKey("dbo.Songs", "GenreId", "dbo.Genres");
            DropForeignKey("dbo.SongArtists", "Artist_ArtistId", "dbo.Artists");
            DropForeignKey("dbo.SongArtists", "Song_SongId", "dbo.Songs");
            DropForeignKey("dbo.Releases", "ArtistID", "dbo.Artists");
            DropIndex("dbo.SongReleases", new[] { "Release_ReleaseId" });
            DropIndex("dbo.SongReleases", new[] { "Song_SongId" });
            DropIndex("dbo.PlaylistSongs", new[] { "Song_SongId" });
            DropIndex("dbo.PlaylistSongs", new[] { "Playlist_PlaylistId" });
            DropIndex("dbo.SongArtists", new[] { "Artist_ArtistId" });
            DropIndex("dbo.SongArtists", new[] { "Song_SongId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Playlist_PlaylistId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Playlists", new[] { "User_Id" });
            DropIndex("dbo.Playlists", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Songs", new[] { "GenreId" });
            DropIndex("dbo.Releases", new[] { "ArtistID" });
            DropTable("dbo.SongReleases");
            DropTable("dbo.PlaylistSongs");
            DropTable("dbo.SongArtists");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Playlists");
            DropTable("dbo.Genres");
            DropTable("dbo.Songs");
            DropTable("dbo.Releases");
            DropTable("dbo.Artists");
        }
    }
}
