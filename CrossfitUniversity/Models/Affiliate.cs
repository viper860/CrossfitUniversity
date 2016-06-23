using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CrossfitUniversity.Models
{
    public class Affiliate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AffiliateId { get; set; }
        public string CfAffiliateId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Cfkids { get; set; }

        public virtual ICollection<Athlete> Athletes { get; set; }
    }
}