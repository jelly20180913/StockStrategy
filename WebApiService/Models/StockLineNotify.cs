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
    
    public partial class StockLineNotify
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public Nullable<bool> Enable { get; set; }
        public Nullable<int> Point { get; set; }
        public Nullable<int> PointBear { get; set; }
        public string Token { get; set; }
        public string NotifyClass { get; set; }
    }
}