//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockMonitor
{
    using System;
    using System.Collections.Generic;
    
    public partial class Company
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Company()
        {
            this.WatchListItems = new HashSet<WatchListItem>();
        }
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public string MarketCapital { get; set; }
        public string PriceToEarningRatio { get; set; }
        public string PriceToSalesRatio { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string CEO { get; set; }
        public byte[] Logo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WatchListItem> WatchListItems { get; set; }
    }
}
