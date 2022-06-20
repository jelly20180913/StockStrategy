using System.Collections.Generic;

namespace StockStrategy.Model
{
    public class Stock
    {
        /// <summary>
        /// [即時股價]參數
        /// </summary>
        public class GetRealtimePriceIn
        {
            public string Sample1_Symbol { get; set; }
        }

        /// <summary>
        /// [即時股價]回傳
        /// </summary>
        public class GetRealtimePriceOut
        {
            public string ErrMsg { get; set; }
            public string realPrice { get; set; }
        }
        public class GetRealtimeStockPrice
        {
            public string StockId { get; set; }
            public string StockName { get; set; }
            public string Price { get; set; }
            public string HighPrice { get; set; }
            public string DealQty { get; set; }
            public string TotalDealQty { get; set; }
            public string OpenPrice  { get; set; }
        }
        /// <summary>
        /// [每日收盤行情]參數
        /// </summary>
        public class GetDayPriceIn
        {
            public string Sample2_Date { get; set; }
        }

        /// <summary>
        /// [每日收盤行情]回傳
        /// </summary>
        public class GetDayPriceOut
        {
            public string ErrMsg { get; set; }
            public List<StockPriceRow> gridList { get; set; }
        }

        /// <summary>
        /// [當月各日成交資訊]參數
        /// </summary>
        public class GetMonthPriceIn
        {
            public string Sample3_Symbol { get; set; }
            public string Sample3_Date { get; set; }
        }

        /// <summary>
        /// [當月各日成交資訊]回傳
        /// </summary>
        public class GetMonthPriceOut
        {
            public string ErrMsg { get; set; }
            public List<StockPriceRow> gridList = new List<StockPriceRow>();
        }

        public class StockPriceRow
        {
            public string symbolCode { get; set; }
            public string symbolName { get; set; }
            public string date { get; set; }
            public string open { get; set; }
            public string high { get; set; }
            public string low { get; set; }
            public string close { get; set; }
            public string volume { get; set; }
        }

        public class TwsePriceSchema
        {
            public QueryTime queryTime { get; set; }
            public string referer { get; set; }
            public string rtmessage { get; set; }
            public string exKey { get; set; }
            public IList<MsgArray> msgArray { get; set; }
            public int userDelay { get; set; }
            public string rtcode { get; set; }
            public int cachedAlive { get; set; }
        }

        public class QueryTime
        {
            public int stockInfoItem { get; set; }
            public string sessionKey { get; set; }
            public string sessionStr { get; set; }
            public string sysDate { get; set; }
            public int sessionFromTime { get; set; }
            public int stockInfo { get; set; }
            public bool showChart { get; set; }
            public int sessionLatestTime { get; set; }
            public string sysTime { get; set; }
        }
        public class StockDayAll
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string TradeVolume { get; set; }
            public string TradeValue { get; set; }
            public string OpeningPrice { get; set; }
            public string HighestPrice { get; set; }
            public string  LowestPrice { get; set; }
            public string ClosingPrice { get; set; }
            public string Change { get; set; }
            public string Transaction { get; set; }
        }
        public class MsgArray
        {
            public string n { get; set; }
            public string g { get; set; }
            public string u { get; set; }
            public string mt { get; set; }
            public string o { get; set; }
            public string ps { get; set; }
            public string tk0 { get; set; }
            public string a { get; set; }
            public string tlong { get; set; }
            public string t { get; set; }
            public string it { get; set; }
            public string ch { get; set; }
            public string b { get; set; }
            public string f { get; set; }
            public string w { get; set; }
            public string pz { get; set; }
            public string l { get; set; }
            public string c { get; set; }
            public string v { get; set; }
            public string d { get; set; }
            public string tv { get; set; }
            public string tk1 { get; set; }
            public string ts { get; set; }
            public string nf { get; set; }
            public string y { get; set; }
            public string p { get; set; }
            public string i { get; set; }
            public string ip { get; set; }
            public string z { get; set; }
            public string s { get; set; }
            public string h { get; set; }
            public string ex { get; set; }
        }
        public class JuridicaPerson
        {
            public string queryType { get; set; }
            public string goDay { get; set; }
            public string doQuery { get; set; }
            public string dateaddcnt { get; set; }
            public string queryDate { get; set; } 
        }
        public class LineMsg
        {
            public string message { get; set; }
            
        }
    }
}
