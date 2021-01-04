using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketplaceServices.Models
{
    public class UserRating
    {
        public int userId { get; set; }
        public int countComment { get; set; }
        public int totalRatePoint { get; set; }
        public double average { get; set; }
    }
}