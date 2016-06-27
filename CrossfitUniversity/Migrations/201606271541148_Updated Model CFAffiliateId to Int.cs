namespace CrossfitUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedModelCFAffiliateIdtoInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Affiliate", "CfAffiliateId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Affiliate", "CfAffiliateId", c => c.String());
        }
    }
}
