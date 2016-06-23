using CrossfitUniversity.Models;
using Library;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CrossfitUniversity.DAL
{

    public class CrossfitInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CrossfitContext>
    {
        protected override void Seed(CrossfitContext context)
        {
            var athletes = LoadAthletes();
            athletes.ForEach(athlete => context.Athletes.Add(athlete));
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

            var affiliates = LoadAffiliates();
            affiliates.ForEach(affiliate => context.Affiliates.Add(affiliate));
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
            CsvManager<Athlete> athletesManager = new CsvManager<Athlete>(@"D:\Users\s3803\Downloads\athletes3.csv", true);
            //athletesManager.SetField(x => x.AthleteId, 0);
            athletesManager.SetField(x => x.CfId, 0);
            athletesManager.SetField(x => x.Name, 1);
            athletesManager.SetField(x => x.Region, 2);
            athletesManager.SetField(x => x.Team, 3);
            //athletesManager.SetField(x => x.Affiliate, 4);
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
            CsvManager<Affiliate> affiliatesManager = new CsvManager<Affiliate>(@"D:\Users\s3803\Downloads\affiliates.csv", true);
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