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
    
    public partial class StockInventoryLog
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<int> Qty { get; set; }
        public string Cost { get; set; }
        public string UpdateTime { get; set; }
        public string Status { get; set; }
        public string CoverPrice { get; set; }
        public Nullable<bool> Winning { get; set; }
        public string CreateDate { get; set; }
        public string StartPrice { get; set; }
        public string Profit { get; set; }
        public string Remark { get; set; }
        public string CoverDate { get; set; }
    }
}
