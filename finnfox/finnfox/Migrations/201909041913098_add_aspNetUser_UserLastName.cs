namespace finnfox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_aspNetUser_UserLastName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserLastName", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserLastName");
        }
    }
}
