//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MarketplaceDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class reported_post
    {
        public int report_id { get; set; }
        public int user_id { get; set; }
        public int ad_id { get; set; }
        public string reason { get; set; }
        public System.DateTime date_added { get; set; }
        public int status { get; set; }
    }
}