//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockRevenue
    {
        public int Id { get; set; }
        public string Revenue { get; set; }
        public string MoM { get; set; }
        public string LastYearRevenue { get; set; }
        public string YoY { get; set; }
        public string CreateDate { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
        public string SumMonthRevenue { get; set; }
        public string SumLastYearRevenue { get; set; }
        public string SumYoY { get; set; }
    }
}
