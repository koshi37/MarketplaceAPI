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
    
    public partial class ad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ad()
        {
            this.chat = new HashSet<chat>();
        }
    
        public int ad_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image_url { get; set; }
        public double price { get; set; }
        public System.DateTime date_added { get; set; }
        public int category_id { get; set; }
        public int user_id { get; set; }
        public string city { get; set; }
        public int province_id { get; set; }
        public bool reserved { get; set; }
        public int views { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<chat> chat { get; set; }
    }
}
