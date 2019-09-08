namespace finnfox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Foreign_Key_To_IdentityUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RacunovodstvenaPromena", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.RacunovodstvenaPromena", "ApplicationUserId");
            AddForeignKey("dbo.RacunovodstvenaPromena", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RacunovodstvenaPromena", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.RacunovodstvenaPromena", new[] { "ApplicationUserId" });
            DropColumn("dbo.RacunovodstvenaPromena", "ApplicationUserId");
        }
    }
}
