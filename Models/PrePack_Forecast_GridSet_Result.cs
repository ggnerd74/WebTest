//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebTest.Models
{
    using System;
    
    public partial class PrePack_Forecast_GridSet_Result
    {
        public int rid { get; set; }
        public Nullable<System.DateTime> forecastdate { get; set; }
        public string productcode { get; set; }
        public string um { get; set; }
        public string ProductDescription { get; set; }
        public decimal forecastvalue { get; set; }
        public decimal actualvalue { get; set; }
        public Nullable<System.DateTime> lastmodified { get; set; }
        public string username { get; set; }
        public decimal startingqoh { get; set; }
    }
}
