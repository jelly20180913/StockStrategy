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
    
    public partial class StockChips
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string BrokerBuySide { get; set; }
        public string BuyQtyBuySide { get; set; }
        public string SellQtyBuySide { get; set; }
        public string BrokerSellSide { get; set; }
        public string BuyQtySellSide { get; set; }
        public string SellQtySellSide { get; set; }
        public string OverBuy { get; set; }
        public string OverSell { get; set; }
        public string Date { get; set; }
        public string CreateDate { get; set; }
    }
}