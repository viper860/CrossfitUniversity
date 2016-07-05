using CrossfitUniversity.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossfitUniversity.ViewModels
{
    public class AffiliateIndexData
    {
        public IPagedList<Affiliate> AffiliatePagedList { get; set; }
        public IEnumerable<Affiliate> Affiliates { get; set; }
        public IEnumerable<Athlete> Athletes { get; set; }
    }
}