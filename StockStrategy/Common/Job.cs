using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using DataModel.Stock;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Collections;
using HtmlAgilityPack;

namespace StockStrategy.Common
{
    public class Job
    {
        public static string GetTaiwanFutures(string url,string id,bool mode)
        {
            string _FuturePrice = "";
            try
            { 
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //req.Timeout = 30000;    //Timeout 30秒
                WebResponse resp = req.GetResponse();
                string strHtml = string.Empty;
                using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(65001)))
                {
                    strHtml = reader.ReadToEnd();//讀取指定url的HTML
                }
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument(); 
				htmlDoc.LoadHtml(strHtml);  
				if (mode)
                {
                    //   _FuturePrice = Math.Floor(Convert.ToDouble(htmlDoc.GetElementbyId(id).InnerText)).ToString();
                    _FuturePrice = htmlDoc.GetElementbyId(id).InnerText;
                }
                else
                {
                    HtmlAgilityPack.HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes(id);
                    foreach (HtmlAgilityPack.HtmlNode n in nodes)
                    {
                        _FuturePrice = n.InnerText;
                        // etc...
                    }
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return _FuturePrice;
        }
		public static HtmlAgilityPack.HtmlDocument GetHtml(string url)
		{
			HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
			try
			{
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				//req.Timeout = 30000;    //Timeout 30秒
				WebResponse resp = req.GetResponse();
				string strHtml = string.Empty;
				using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(65001)))
				{
					strHtml = reader.ReadToEnd();//讀取指定url的HTML
				}
				
				htmlDoc.LoadHtml(strHtml);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			return htmlDoc;
		}
		public static string GetValue(HtmlAgilityPack.HtmlDocument htmlDoc,string id)
        {
            string _ReturnValue = "";
			HtmlAgilityPack.HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes(id);
			foreach (HtmlAgilityPack.HtmlNode n in nodes)
			{ 
				_ReturnValue = n.InnerText;
			}
			return _ReturnValue;
		}
	}
}
