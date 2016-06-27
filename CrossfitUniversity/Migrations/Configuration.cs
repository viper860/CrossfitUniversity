namespace CrossfitUniversity.Migrations
{
    using Library;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Web;
    internal sealed class Configuration : DbMigrationsConfiguration<CrossfitUniversity.DAL.CrossfitContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CrossfitUniversity.DAL.CrossfitContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            var affiliates = LoadAffiliates();
            //affiliates.ForEach(affiliate => context.Affiliates.Add(affiliate));
            context.Affiliates.AddRange(affiliates);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            //
            var athletes = LoadAthletes();
            //athletes.ForEach(athlete => context.Athletes.Add(athlete));
            foreach (Athlete athlete in athletes)
            {
              var affiliate = context.Affiliates.Where(a => a.Name == athlete.AffiliateName).FirstOrDefault();


                athlete.Affiliate = affiliate;
            }
            context.Athletes.AddRange(athletes);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }


        }
    

    private static List<Athlete> LoadAthletes()
    {
       // var fileName = HttpContext.Current.Server.MapPath("~/App_Data/athletes3.csv");
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "/../App_Data/athletes3.csv";
            System.Diagnostics.Debug.Write(fileName);

        //var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "athletes3.csv");
            CsvManager<Athlete> athletesManager = new CsvManager<Athlete>(fileName, true);
        //athletesManager.SetField(x => x.AthleteId, 0);
        athletesManager.SetField(x => x.CfId, 0);
        athletesManager.SetField(x => x.Name, 1);
        athletesManager.SetField(x => x.Region, 2);
        athletesManager.SetField(x => x.Team, 3);
        athletesManager.SetField(x => x.AffiliateName, 4);
        athletesManager.SetField(x => x.Gender, 5);
        athletesManager.SetField(x => x.Age, 6);
        athletesManager.SetField(x => x.Height, 7);
        athletesManager.SetField(x => x.Weight, 8);
        athletesManager.SetField(x => x.Fran, 9);
        athletesManager.SetField(x => x.Helen, 10);
        athletesManager.SetField(x => x.Grace, 11);
        athletesManager.SetField(x => x.Filthy50, 12);
        athletesManager.SetField(x => x.Fgonebad, 13);
        athletesManager.SetField(x => x.Run400, 14);
        athletesManager.SetField(x => x.Run5k, 15);
        athletesManager.SetField(x => x.Candj, 16);
        athletesManager.SetField(x => x.Snatch, 17);
        athletesManager.SetField(x => x.Deadlift, 18);
        athletesManager.SetField(x => x.Backsq, 19);
        athletesManager.SetField(x => x.Pullups, 20);
        athletesManager.SetField(x => x.Eat, 21);
        athletesManager.SetField(x => x.Train, 22);
        athletesManager.SetField(x => x.Background, 23);
        athletesManager.SetField(x => x.Experience, 24);
        athletesManager.SetField(x => x.Schedule, 25);
        athletesManager.SetField(x => x.Howlong, 26);
        athletesManager.SetField(x => x.RetrievedDatetime, 27);
        List<Athlete> athletesResult = athletesManager.GetObjectList();

        return athletesResult;
    }

    private static List<Affiliate> LoadAffiliates()
    {
            // var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "affiliates.csv");
            //string fileName = HttpContext.Current.Server.MapPath("~/App_Data/affiliates.csv");
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "/../App_Data/affiliates.csv";
            //var fileName = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/affiliates.csv");
            System.Diagnostics.Debug.Write("Test Output");
            Trace.TraceInformation("TEst output");
            Trace.TraceInformation(fileName);

            //string fileName = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/affiliates.csv");
            System.Diagnostics.Debug.Write(fileName);
            CsvManager<Affiliate> affiliatesManager = new CsvManager<Affiliate>(fileName, true);
        //athletesManager.SetField(x => x.AthleteId, 0);
        affiliatesManager.SetField(x => x.CfAffiliateId, 0);
        affiliatesManager.SetField(x => x.Name, 1);
        affiliatesManager.SetField(x => x.Address, 2);
        affiliatesManager.SetField(x => x.Phone, 3);
        affiliatesManager.SetField(x => x.Url, 4);
        affiliatesManager.SetField(x => x.Latitude, 5);
        affiliatesManager.SetField(x => x.Longitude, 6);
        affiliatesManager.SetField(x => x.Cfkids, 7);
        List<Affiliate> affiliatesResult = affiliatesManager.GetObjectList();

        return affiliatesResult;
    }
}
}
