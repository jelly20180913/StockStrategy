using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
namespace StockStrategy.Common
{
    public class XSLXHelper
    {
        /// <summary>
        /// 產生 excel
        /// </summary>
        /// <typeparam name="T">傳入的物件型別</typeparam>
        /// <param name="data">物件資料集</param>
        /// <returns></returns>
        public XLWorkbook Export<T>(List<T> data)
        {
            //建立 excel 物件
            XLWorkbook workbook = new XLWorkbook();
            //加入 excel 工作表名為 `Report`
            var sheet = workbook.Worksheets.Add("Report");
            //欄位起啟位置
            int colIdx = 1;
            //使用 reflection 將物件屬性取出當作工作表欄位名稱
            foreach (var item in typeof(T).GetProperties())
            {
                #region - 可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱 -
                //可以使用 DescriptionAttribute 設定，找不到 DescriptionAttribute 時改用屬性名稱
                DescriptionAttribute description = item.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (description != null)
                {
                    sheet.Cell(1, colIdx++).Value = description.Description;
                    continue;
                }
                // sheet.Cell(1, colIdx++).Value = item.Name; 
                #endregion
                #region - 直接使用物件屬性名稱 -
                //或是直接使用物件屬性名稱
                // sheet.Cell(1, colIdx++).Value = item.Name;
                #endregion

            }
            //資料起始列位置
            int rowIdx = 2;
            foreach (var item in data)
            {
                //每筆資料欄位起始位置
                int conlumnIndex = 1;
                foreach (var jtem in item.GetType().GetProperties())
                {
                    //將資料內容加上 "'" 避免受到 excel 預設格式影響，並依 row 及 column 填入
                    sheet.Cell(rowIdx, conlumnIndex).Value = string.Concat("'", Convert.ToString(jtem.GetValue(item, null)));
                    conlumnIndex++;
                }
                rowIdx++;
            }
            return workbook;
        }
        public DataTable Import(string filePath, string dtName)
        {
            List<string> _ListTemp = new List<string>();
            DataTable dt = new DataTable();
            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {

                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(dtName);

                //Create a new DataTable.


                //Loop through the Worksheet rows.
                bool firstRow = true;
                int _Name = 1;
                int _Count = workSheet.Rows().Count();
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            if (_ListTemp.Contains(cell.Value.ToString()))
                            {
                                dt.Columns.Add(cell.Value.ToString() + _Name);
                                _Name++;
                            }
                            else dt.Columns.Add(cell.Value.ToString());
                            _ListTemp.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        int _CellCount = row.Cells().ToList().Count();
                        if (_CellCount > 1)
                        {
                            //below resolve issue about not work to get empty value 
                            foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }
                    }

                }
            }
            return dt;
        }
    }
}
