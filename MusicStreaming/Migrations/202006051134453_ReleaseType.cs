namespace MusicStreaming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReleaseType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReleaseTypes",
                c => new
                    {
                        ReleaseTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ReleaseTypeId);
            
            AddColumn("dbo.Releases", "ReleaseTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Releases", "ReleaseTypeId");
            AddForeignKey("dbo.Releases", "ReleaseTypeId", "dbo.ReleaseTypes", "ReleaseTypeId", cascadeDelete: true);
            Sql("INSERT INTO ReleaseTypes (Name) VALUES('Single')");
            Sql("INSERT INTO ReleaseTypes (Name) VALUES('EP')");
            Sql("INSERT INTO ReleaseTypes (Name) VALUES('Album')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Releases", "ReleaseTypeId", "dbo.ReleaseTypes");
            DropIndex("dbo.Releases", new[] { "ReleaseTypeId" });
            DropColumn("dbo.Releases", "ReleaseTypeId");
            DropTable("dbo.ReleaseTypes");
        }
    }
}
