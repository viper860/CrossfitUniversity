using CrossfitUniversity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace CrossfitUniversity.DAL
{
    public class CrossfitContext : DbContext
    {
        public CrossfitContext() : base("CrossfitContext")
        {
        }
        public DbSet<Athlete> Athletes { get; set; }

        public DbSet<Affiliate> Affiliates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}