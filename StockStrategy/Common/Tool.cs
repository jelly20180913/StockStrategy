﻿using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Globalization;

namespace StockStrategy.Common
{
    public static class Tool
    {
        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="textToEncrypt">加密字串</param>
        /// <param name="publicKey">公鑰</param>
        /// <param name="secretKey">私鑰</param>
        /// <returns></returns>
        public static string Encrypt(string textToEncrypt,string publicKey,string secretKey)
        {
            try
            { 
                string ToReturn = ""; 
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretKey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publicKey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textToDecrypt">解密</param>
        /// <param name="publicKey">公鑰</param>
        /// <param name="secretKey">私鑰</param>
        /// <returns></returns>
        public static string Decrypt(string textToDecrypt, string publicKey, string secretKey)
        {
            try
            { 
                string ToReturn = ""; 
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretKey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publicKey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
        public static string GetServerName()
        {
            return Environment.MachineName; 
        }
        /// <summary>
        /// Gets the ip addresses.
        /// </summary>
        /// <returns>ip addresses</returns>
        public static string[] GetIpAddresses()
        {
            string hostName = GetServerName();
            return System.Net.Dns.GetHostAddresses(hostName).Select(i => i.ToString()).ToArray();
        }
		/// <summary>
		/// 将千分位字符串转换成数字
		/// 说明：将诸如"–111,222,333的千分位"转换成-111222333数字
		/// 若转换失败则返回-1
		/// </summary>
		/// <param name="thousandthStr">需要转换的千分位</param>
		/// <returns>数字</returns>
		public static int ParseThousandthString(this string thousandthStr)
		{
			int _value = -1;
			if (!string.IsNullOrEmpty(thousandthStr))
			{
				try
				{
					_value = int.Parse(thousandthStr, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
				}
				catch (Exception ex)
				{
					_value = -1;
					Debug.WriteLine(string.Format("将千分位字符串{0}转换成数字异常，原因:{0}", thousandthStr, ex.Message));
				}
			}
			return _value;
		} 
	}
}
