namespace finnfox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodata_KolicinaNovca_i_Valuta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RacunovodstvenaPromena", "KolicinaNovca", c => c.Double(nullable: false));
            AddColumn("dbo.RacunovodstvenaPromena", "Valuta", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RacunovodstvenaPromena", "Valuta");
            DropColumn("dbo.RacunovodstvenaPromena", "KolicinaNovca");
        }
    }
}
