using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CrossfitUniversity.Models
{
    public class Athlete
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AthleteId { get; set; }
        public int CfId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Team { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string AffiliateName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Fran { get; set; }
        public string Helen { get; set; }
        public string Grace { get; set; }
        public string Filthy50 { get; set; }
        public string Fgonebad { get; set; }
        public string Run400 { get; set; }
        public string Run5k { get; set; }
        public string Candj { get; set; }
        public string Snatch { get; set; }
        public string Deadlift { get; set; }
        public string Backsq { get; set; }
        public string Pullups { get; set; }
        public string Eat { get; set; }
        public string Train { get; set; }
        public string Background { get; set; }
        public string Experience { get; set; }
        public string Schedule { get; set; }
        public string Howlong { get; set; }
        public string RetrievedDatetime { get; set; }
        public virtual Affiliate Affiliate { get; set; }

    }
}