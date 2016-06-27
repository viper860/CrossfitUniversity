namespace CrossfitUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Affiliate",
                c => new
                    {
                        AffiliateId = c.Int(nullable: false, identity: true),
                        CfAffiliateId = c.String(),
                        Name = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Url = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        Cfkids = c.String(),
                    })
                .PrimaryKey(t => t.AffiliateId);
            
            CreateTable(
                "dbo.Athlete",
                c => new
                    {
                        AthleteId = c.Int(nullable: false, identity: true),
                        CfId = c.Int(nullable: false),
                        Name = c.String(),
                        Region = c.String(),
                        Team = c.String(),
                        Gender = c.String(),
                        Age = c.String(),
                        Height = c.String(),
                        Weight = c.String(),
                        Fran = c.String(),
                        Helen = c.String(),
                        Grace = c.String(),
                        Filthy50 = c.String(),
                        Fgonebad = c.String(),
                        Run400 = c.String(),
                        Run5k = c.String(),
                        Candj = c.String(),
                        Snatch = c.String(),
                        Deadlift = c.String(),
                        Backsq = c.String(),
                        Pullups = c.String(),
                        Eat = c.String(),
                        Train = c.String(),
                        Background = c.String(),
                        Experience = c.String(),
                        Schedule = c.String(),
                        Howlong = c.String(),
                        RetrievedDatetime = c.String(),
                        Affiliate_AffiliateId = c.Int(),
                    })
                .PrimaryKey(t => t.AthleteId)
                .ForeignKey("dbo.Affiliate", t => t.Affiliate_AffiliateId)
                .Index(t => t.Affiliate_AffiliateId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Athlete", "Affiliate_AffiliateId", "dbo.Affiliate");
            DropIndex("dbo.Athlete", new[] { "Affiliate_AffiliateId" });
            DropTable("dbo.Athlete");
            DropTable("dbo.Affiliate");
        }
    }
}
