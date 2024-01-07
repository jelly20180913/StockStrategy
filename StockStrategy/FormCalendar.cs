using NLog.Fluent;
using NLog;
using StockStrategy.BBL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApiService.Models;

namespace StockStrategy
{
	public partial class FormCalendar : Form
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public FormCalendar()
		{
			InitializeComponent();
		}

		private void FormCalendar_Load(object sender, EventArgs e)
		{
			
		}
		/// <summary>
		/// 漲跌差:100 1分
		///        300 2分
		///        500 3分
		///        700 4分
		///        1000 5分
		///        -100 -1分
		///        -200 -2分
		///        -300 -3分
		///        -400 -4分
		///        -500 -5分
		///        -600 -6分
		///        -700 -7分
		///        -800 -10分
		/// 半根差 20   1分
		///        30   2分
		///        40   3分
		///        50   4分
		///        60   5分
		///        -20  -2分
		///        -30  -4分
		///        -40  -6分
		///        -50  -8分
		///        -60  -10分
		/// 大跌:-10以下 
		/// 小跌:小於-10
		/// 小漲:小於10
		/// 大漲:10以上
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnQuery_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				this.flowLayoutPanel1.Controls.Clear();
				DataAccess _DataAccess = new DataAccess();
				List<StockIndex> _ListStockAll = _DataAccess.getStockIndexList().Where(x=> Convert.ToInt32(x.Date) >= Convert.ToInt32(this.dtpStart.Value.ToString("yyyyMMdd"))&& Convert.ToInt32(x.Date) <= Convert.ToInt32(this.dtpEnd.Value.ToString("yyyyMMdd"))).ToList();
				int _Count = getControls(_ListStockAll.First().DayOfWeek.Trim());
				for (int i = 0; i<_Count;i++)
				{
					TextBox _TextBox = new TextBox(); 
					_TextBox.Multiline = true;
					_TextBox.Name = $"_TextBox{i}";
					_TextBox.Size = new System.Drawing.Size(100, 45); 
					_TextBox.Enabled = false;
					_TextBox.BackColor =Color.White;
					this.flowLayoutPanel1.Controls.Add(_TextBox);
				}
				foreach (StockIndex si in _ListStockAll) {
					TextBox _TextBox = new TextBox();
					int _Pt = 0, _HalfPt = 0;
					_TextBox.Location = new System.Drawing.Point(3, 3);
					_TextBox.Multiline = true;
					string _Date=Convert.ToInt32(si.Date).ToString("MM/dd");
					_TextBox.Name = $"_TextBox{si.Date}";
					_TextBox.Size = new System.Drawing.Size(100, 45);
					int _TotalPoint = 0;
					if (si.Ups != null) {
						  _Pt = getPoint(Convert.ToInt32(si.Ups) - Convert.ToInt32(si.Downs));
						 _HalfPt= getHalfPoint(Convert.ToInt32(si.UpsHalf) - Convert.ToInt32(si.DownsHalf));
					}
					_TotalPoint = _Pt + _HalfPt;
					_TextBox.Text = $" {si.Date.Substring(4,4)}  {_TotalPoint}";
					_TextBox.Enabled = false;
					_TextBox.BackColor = getColor(_TotalPoint);
					_TextBox.ForeColor = Color.Black;
					_TextBox.Font = new Font(_TextBox.Font.FontFamily, 14, _TextBox.Font.Style); 
					this.flowLayoutPanel1.Controls.Add(_TextBox); 
				}
				//因為stock index計算的漲跌都是昨天的所以需退一格
				this.flowLayoutPanel1.Controls.RemoveAt(0);
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " FormCalendar_Load:" + $"訊息:{ex.Message}|行號{ex.StackTrace}" + "\r\n";
				logger.Error(_Log);
			}
		}
		private int getControls(string dayOfWeek) {
			int _No = 0;
			if (dayOfWeek =="五")
			{
				_No = 4;
			}
			else if (dayOfWeek == "四")
			{
				_No = 3;
			}
			else if (dayOfWeek == "三")
			{
				_No = 2;
			}
			else if (dayOfWeek == "二")
			{
				_No = 1;
			}
			return _No;
		}
		private Color getColor(int input) {
			if (input > 9)
			{
				return Color.DarkRed;
			}
			else if (input > 0)
			{
				return Color.Red;
			}
			else if (input < -9)
			{
				return Color.DarkGreen;
			}
			else if (input < 0)
			{
				return Color.LightGreen;
			}
			return Color.Gray;
		}
		private int getPoint(int input) {
			int _Pt = 0;
			if (input > 1000) {
				_Pt = 5;
			}
			else if (input > 700)
			{
				_Pt = 4;
			}
			else if (input > 500)
			{
				_Pt = 3;
			}
			else if (input > 300)
			{
				_Pt = 2;
			} 
			else if (input > 100)
			{
				_Pt = 1;
			}  
			else if (input < -800)
			{
				_Pt = -10;
			}
			else if (input < -700)
			{
				_Pt = -7;
			}
			else if (input < -600)
			{
				_Pt = -6;
			}
			else if (input < -500)
			{
				_Pt = -5;
			}
			else if (input < -400)
			{
				_Pt = -4;
			}
			else if (input < -300)
			{
				_Pt = -3;
			}
			else if (input < -200)
			{
				_Pt = -2;
			}
			else if (input < -100)
			{
				_Pt = -1;
			}
			return _Pt;
		}
		private int getHalfPoint(int input)
		{
			int _Pt = 0;
			if (input > 60)
			{
				_Pt = 5;
			}
			else if (input > 50)
			{
				_Pt = 4;
			}
			else if (input > 40)
			{
				_Pt = 3;
			}
			else if (input > 30)
			{
				_Pt = 2;
			}
			else if (input > 20)
			{
				_Pt = 1;
			}  
			else if(input < -60)
			{
				_Pt = -10;
			}
			else if (input < -50)
			{
				_Pt = -8;
			}
			else if (input < -40)
			{
				_Pt = -6;
			}
			else if (input < -30)
			{
				_Pt = -4;
			}
			else if (input < -20)
			{
				_Pt = -2;
			}
			else if (input < -10)
			{
				_Pt = -1;
			}
			return _Pt;
		} 
	}
}
