namespace MusicStreaming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'3b2fd881-4b72-43ac-80cc-225a6aba6e80', N'admin@admin.com', 0, N'AGOKsMEuNBx7aXxp/cny+tKTaM57e3Moh0wDm39anx9Npu8E4GGCz1YWpB1W9GPsBA==', N'0d254f40-7d77-4e21-87d5-0afd17f487b1', NULL, 0, 0, NULL, 1, 0, N'admin@admin.com')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'ce940354-c9bd-404c-a9a1-726ae7eefb57', N'Admin')
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'3b2fd881-4b72-43ac-80cc-225a6aba6e80', N'ce940354-c9bd-404c-a9a1-726ae7eefb57')

");
        }
        
        public override void Down()
        {
        }
    }
}
