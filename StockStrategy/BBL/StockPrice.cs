﻿using Newtonsoft.Json;
using StockStrategy.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NLog;
namespace StockStrategy.BBL
{
    public  class StockPrice
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 即時股價
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public static List<Stock.GetRealtimeStockPrice> GetRealtimePrice(Stock.GetRealtimePriceIn inModel)
        {
            string _Log = "";
            Stock.GetRealtimePriceOut outModel = new Stock.GetRealtimePriceOut();
            List<Stock.GetRealtimeStockPrice> _PriceList = new List<Stock.GetRealtimeStockPrice>();
            // 檢查輸入參數
            if (string.IsNullOrEmpty(inModel.Sample1_Symbol))
            {
                outModel.ErrMsg = "請輸入股票代碼";
            }
            try
            {
                StringBuilder ExCode = new StringBuilder();
                string[] symbols = inModel.Sample1_Symbol.Split(';');
                foreach (string symbol in symbols)
                {
                    ExCode.Append("tse_" + symbol + ".tw|");
                }

                // 呼叫網址
                string url = "https://mis.twse.com.tw/stock/api/getStockInfo.jsp";
                url += "?json=1&delay=0&ex_ch=" + ExCode;

                string downloadedData = "";
                using (WebClient wClient = new WebClient())
                {
                    // 取得網頁資料
                    wClient.Encoding = Encoding.UTF8;
                    downloadedData = wClient.DownloadString(url);
                }
                Stock.TwsePriceSchema jsonPrice = null;
                if (downloadedData.Trim().Length > 0)
                {
                    jsonPrice = JsonConvert.DeserializeObject<Stock.TwsePriceSchema>(downloadedData);
                    if (jsonPrice.rtcode != "0000")
                    {
                        throw new Exception("取商品價格失敗: " + jsonPrice.rtmessage);
                    }
                }

                StringBuilder sbRealPrice = new StringBuilder();
                for (int i = 0; i < jsonPrice.msgArray.Count; i++)
                {
                    Stock.GetRealtimeStockPrice _Price = new Stock.GetRealtimeStockPrice();
                    // 代碼
                    _Price.StockId = jsonPrice.msgArray[i].c;

                    // z = 收盤價
                    
                    if (jsonPrice.msgArray[i].z != "-")
                    {
                        _Price.Price = Math.Round(Convert.ToDecimal(jsonPrice.msgArray[i].z), 2).ToString();
                    }
                    else
                    {
                        string[] arrPrice = jsonPrice.msgArray[i].b.Split('_');
                        _Price.Price = arrPrice.Length>0? Math.Round(Convert.ToDecimal(arrPrice[0]), 2).ToString():"0";
                    }
                    _Price.StockName = jsonPrice.msgArray[i].n;
                    _Price.HighPrice = jsonPrice.msgArray[i].h;
                    _Price.DealQty = jsonPrice.msgArray[i].tv;
                    _Price.TotalDealQty = jsonPrice.msgArray[i].v;
                    _Price.OpenPrice = jsonPrice.msgArray[i].o;
                    _PriceList.Add(_Price); 
                } 
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetMonthPrice:" + "\r\n" + ex.Message;
                logger.Error(_Log);
            }
            return _PriceList;
        }
        /// <summary>
        /// 取得昨日股票清單 證交所
        /// </summary>
        /// <returns></returns>
        public static List<Stock.StockDayAll> GetStockDayAll( )
        {
            string _Log = "";
            List<Stock.StockDayAll> _PriceList = new List<Stock.StockDayAll>();
            try
            { 
                // 呼叫網址
                string url = "https://openapi.twse.com.tw/v1/exchangeReport/STOCK_DAY_ALL"; 

                string downloadedData = "";
                using (WebClient wClient = new WebClient())
                { 
                    wClient.Encoding = Encoding.UTF8;
                    downloadedData = wClient.DownloadString(url);
                } 
                if (downloadedData.Trim().Length > 0)
                {
                    _PriceList = JsonConvert.DeserializeObject<List<Stock.StockDayAll>>(downloadedData);
                   
                } 
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetStockDayAll:" + "\r\n" + ex.Message;
                logger.Error(_Log);
            }
            return _PriceList;
        }
        //public static List<object>  GetStockByDate(string date,string code)
        //{
        //    string _Log = "";
        //List<object>  _PriceList  = new List<object>();
        //    try
        //    {
        //        // 呼叫網址
        //        string url = "https://www.twse.com.tw/exchangeReport/STOCK_DAY?date="+date+"&stockNo="+code;

        //        string downloadedData = "";
        //        using (WebClient wClient = new WebClient())
        //        {
        //            wClient.Encoding = Encoding.UTF8;
        //            downloadedData = wClient.DownloadString(url);
        //        }
        //        if (downloadedData.Trim().Length > 0)
        //        {
        //            _PriceList = JsonConvert.DeserializeObject<List<object>>(downloadedData);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetStockByDate:" + "\r\n" + ex.Message;
        //        logger.Error(_Log);
        //    }
        //    return _PriceList;
        //}
        /// <summary>
        /// 當月各日成交資訊
        /// </summary>
        /// <param name="inModel"></param>
        /// <returns></returns>
        public static Stock.GetMonthPriceOut GetMonthPrice(Stock.GetMonthPriceIn inModel)
        {

            //List<Stock.GetMonthPriceOut> _GetMonthPriceOutList = new List<Stock.GetMonthPriceOut>();
            string _Log = "";
            Stock.GetMonthPriceOut outModel = new Stock.GetMonthPriceOut();
            try
            {

                // 呼叫網址
                 string download_url = "http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=csv&date=" + inModel.Sample3_Date + "&stockNo=" + inModel.Sample3_Symbol; 
                string downloadedData = "";
                using (WebClient wClient = new WebClient())
                {
                    // 網頁回傳
                    downloadedData = wClient.DownloadString(download_url);
                }
                if (downloadedData.Trim().Length > 0)
                {
                    outModel.gridList = new List<Stock.StockPriceRow>();
                    string[] lineStrs = downloadedData.Split('\n');
                    for (int i = 0; i < lineStrs.Length; i++)
                    {
                        string strline = lineStrs[i];
                        if (i == 0 || i == 1 || strline.Trim().Length == 0)
                        {
                            continue;
                        }
                        // 排除非價格部份
                        if (strline.IndexOf("說明") > -1 || strline.IndexOf("符號") > -1 || strline.IndexOf("統計") > -1 || strline.IndexOf("ETF") > -1)
                        {
                            continue;
                        }

                        ArrayList resultLine = new ArrayList();
                        // 解析資料
                        ParseCSVData(resultLine, strline);
                        string[] datas = (string[])resultLine.ToArray(typeof(string));
                        if (datas.Length < 6) continue;
                        //檢查資料內容
                        if (Convert.ToInt32(datas[1].Replace(",", "")) == 0 || datas[3] == "--" || datas[4] == "--" || datas[5] == "--" || datas[6] == "--")
                        {
                            continue;
                        }

                        // 輸出資料
                        Stock.StockPriceRow row = new Stock.StockPriceRow();
                        row.date = datas[0]; //日期
                        row.open = datas[3]; //開盤價
                        row.high = datas[4]; //最高價
                        row.low = datas[5]; //最低價
                        row.close = datas[6]; //收盤價
                        row.volume = datas[1]; //成交量
                        outModel.gridList.Add(row);

                    }
                }
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetMonthPrice:" + "\r\n" + ex.Message;
                logger.Error(_Log);
            }
            return outModel;


        }
        /// <summary>
        /// 取得股票三大法人(已失效)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<Stock.StockJuridical> GetJuridical(string date)
        { 
            string _Log = "";
            List<Stock.StockJuridical> _ListStockJuridical = new List<Stock.StockJuridical>();
            try
            { 
                // 呼叫網址 
                string download_url = "https://www.twse.com.tw/fund/T86?response=csv&date="+ date + "&selectType=ALL";
                string downloadedData = "";
                using (WebClient wClient = new WebClient())
                {
                    wClient.Encoding = Encoding.UTF8;
					// 網頁回傳
					downloadedData = wClient.DownloadString(download_url);
                }
                if (downloadedData.Trim().Length > 0)
                { 
                    string[] lineStrs = downloadedData.Split('\n');
                    for (int i = 0; i < lineStrs.Length; i++)
                    {
                        string strline = lineStrs[i];
                        if (i == 0 || i == 1 || strline.Trim().Length == 0)
                        {
                            continue;
                        }
                        // 排除非價格部份
                        if (strline.IndexOf("說明") > -1 || strline.IndexOf("符號") > -1 || strline.IndexOf("統計") > -1 || strline.IndexOf("ETF") > -1)
                        {
                            continue;
                        } 
                        ArrayList resultLine = new ArrayList();
                        // 解析資料
                        ParseCSVData(resultLine, strline);
                        string[] datas = (string[])resultLine.ToArray(typeof(string));
                        if (datas.Length < 6) continue;
                        Stock.StockJuridical _StockJuridical = new Stock.StockJuridical();
                        _StockJuridical.Code = datas[0];
                        _StockJuridical.Date = date;
                        _StockJuridical.ForeignInvestment= datas[4];
                        _StockJuridical.Investment = datas[10];
                        _StockJuridical.Dealer = datas[11];
                        _ListStockJuridical.Add(_StockJuridical);
                    }
                }
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetJuridical:" + "\r\n" + ex.Message;
                logger.Error(_Log);
            }
            return _ListStockJuridical; 
        }
        private static void ParseCSVData(ArrayList result, string data)
        {
            int position = -1;
            while (position < data.Length)
                result.Add(ParseCSVField(ref data, ref position));
        }

        private static string ParseCSVField(ref string data, ref int StartSeperatorPos)
        {
            if (StartSeperatorPos == data.Length - 1)
            {
                StartSeperatorPos++;
                return "";
            }

            int fromPos = StartSeperatorPos + 1;
            if (data[fromPos] == '"')
            {
                int nextSingleQuote = GetSingleQuote(data, fromPos + 1);
                StartSeperatorPos = nextSingleQuote + 1;
                string tempString = data.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
                tempString = tempString.Replace("'", "''");
                return tempString.Replace("\"\"", "\"");
            }

            int nextComma = data.IndexOf(',', fromPos);
            if (nextComma == -1)
            {
                StartSeperatorPos = data.Length;
                return data.Substring(fromPos);
            }
            else
            {
                StartSeperatorPos = nextComma;
                return data.Substring(fromPos, nextComma - fromPos);
            }
        }
        private static int GetSingleQuote(string data, int SFrom)
        {
            int i = SFrom - 1;
            while (++i < data.Length)
                if (data[i] == '"')
                {
                    if (i < data.Length - 1 && data[i + 1] == '"')
                    {
                        i++;
                        continue;
                    }
                    else
                        return i;
                }
            return -1;
        }
    }
}
