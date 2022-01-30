using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
namespace HtsApiDemo
{
    public partial class FormMain : Form
    {
        [DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSOrder", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void HTSOrder([MarshalAs(UnmanagedType.LPStr)]StringBuilder APChar);
        /*9.改價*/
        [DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSOrderChgPrice", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void HTSOrderChgPrice([MarshalAs(UnmanagedType.LPStr)]StringBuilder APChar);
        private Dictionary<string, string> dicOrderType =
    new Dictionary<string, string>();


        public FormMain()
        {
            InitializeComponent();
        }
        private string[] Oberservers = ConfigurationManager.AppSettings["Observer"].Split(';');
        /// <summary>
        /// 動態券賣按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSellingShort_Click(object sender, EventArgs e)
        {
            string _BuySell = "S";
            string _CashMarginShort = "S";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
            string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
            string _PriceType = this.cbPriceType.SelectedItem.ToString();
            Button _Btn = sender as Button;
            if (Convert.ToString(_Btn.Tag) == "")
            {
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                btnDeal.Text = "券賣";
                _StockId = txtStockId.Text;
            }
            else
            {
                NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
                _Units = Convert.ToString(_NupQty.Value);
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text;
                Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
                _BtnDeal.Text = "券賣";
                ComboBox _CbPriceType = (ComboBox)flowLayoutPanel1.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
                _OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
                _MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
                _PriceType = _CbPriceType.SelectedItem.ToString();
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            MessageBox.Show(_Parameter);
            string _Msg = "你融券賣出" + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元";
            Color color = Color.DarkGreen;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            double _Price = 0.0;
            double _Value = 0.0;
            Button _Btn = sender as Button;
            if (_Btn.Tag.ToString() == "")
            {
                _Price = txtPrice.Text != "" ? Convert.ToDouble(txtPrice.Text) : 0.0;
                _Value = tick(_Price);
                if (_Price != 0) _Price -= _Value;
                txtPrice.Text = _Price.ToString();
            }
            else
            {
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text != "" ? Convert.ToDouble(_TxtPrice.Text) : 0.0;
                _Value = tick(_Price);
                if (_Price != 0) _Price -= _Value;
                _TxtPrice.Text = _Price.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            double _Price = 0.0;
            double _Value = 0.0;
            Button _Btn = sender as Button;
            if (_Btn.Tag.ToString() == "")
            {
                _Price = txtPrice.Text != "" ? Convert.ToDouble(txtPrice.Text) : 0.0;
                _Value = tick(_Price);
                _Price += _Value;
                txtPrice.Text = _Price.ToString();
            }
            else
            {
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text != "" ? Convert.ToDouble(_TxtPrice.Text) : 0.0;
                _Value = tick(_Price);
                _Price += _Value;
                _TxtPrice.Text = _Price.ToString();
            }
        }

        private void btnMarginTrading_Click(object sender, EventArgs e)
        {
            string _BuySell = "B";
            string _CashMarginShort = "M";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
            string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
            string _PriceType = this.cbPriceType.SelectedItem.ToString();
            Button _Btn = sender as Button;
            if (Convert.ToString(_Btn.Tag) == "")
            {
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                btnDeal.Text = "資買";
                _StockId = txtStockId.Text;
            }
            else
            {
                NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
                _Units = Convert.ToString(_NupQty.Value);
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text;
                Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
                _BtnDeal.Text = "資買";
                ComboBox _CbPriceType = (ComboBox)flowLayoutPanel1.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
                _OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
                _MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
                _PriceType = _CbPriceType.SelectedItem.ToString();
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            MessageBox.Show(_Parameter);
            string _Msg = "你融資買進" + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元";
            Color color = Color.DarkRed;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnOverSell_Click(object sender, EventArgs e)
        {
            string _BuySell = "S";
            string _CashMarginShort = "C";
            string _OverSell = ",OverSell=Y";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
            string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
            string _PriceType = this.cbPriceType.SelectedItem.ToString();
            Button _Btn = sender as Button;
            if (Convert.ToString(_Btn.Tag) == "")
            {
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                btnDeal.Text = "沖賣";
                _StockId = txtStockId.Text;
            }
            else
            {
                NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
                _Units = Convert.ToString(_NupQty.Value);
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text;
                Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
                _BtnDeal.Text = "沖賣";
                ComboBox _CbPriceType = (ComboBox)flowLayoutPanel1.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
                _OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
                _MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
                _PriceType = _CbPriceType.SelectedItem.ToString();
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _OverSell + _MarketLimit; ;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你當沖賣出" + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元";
            MessageBox.Show(_Parameter);
            Color color = Color.Salmon;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            string _BuySell = "B";
            string _CashMarginShort = "C";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
            string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
            string _PriceType = this.cbPriceType.SelectedItem.ToString();
            Button _Btn = sender as Button;
            if (Convert.ToString(_Btn.Tag) == "")
            {
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                btnDeal.Text = "現買";
                _StockId = txtStockId.Text;
            }
            else
            {
                NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
                _Units = Convert.ToString(_NupQty.Value);
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text;
                Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
                _BtnDeal.Text = "現買";
                ComboBox _CbPriceType = (ComboBox)flowLayoutPanel1.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
                _OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
                _MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
                _PriceType = _CbPriceType.SelectedItem.ToString();
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你現股買進" + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元";
            MessageBox.Show(_Parameter);
            Color color = Color.Red;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            string _BuySell = "S";
            string _CashMarginShort = "C";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
            string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
            string _PriceType = this.cbPriceType.SelectedItem.ToString();
            Button _Btn = sender as Button;
            if (Convert.ToString(_Btn.Tag) == "")
            {
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                btnDeal.Text = "現賣";
                _StockId = txtStockId.Text;
            }
            else
            {
                NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
                _Units = Convert.ToString(_NupQty.Value);
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text;
                Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
                _BtnDeal.Text = "現賣";
                ComboBox _CbPriceType = (ComboBox)flowLayoutPanel1.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
                _OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
                _MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
                _PriceType = _CbPriceType.SelectedItem.ToString();
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你現股賣出" + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元";
            MessageBox.Show(_Parameter);
            Color color = Color.Green;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.txtStockId.Text = Oberservers[1];
            this.txtOberserver.Text = ConfigurationManager.AppSettings["Observer"];
            this.Text = "股票策略下單機：V" + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
            this.cbPriceType.SelectedIndex = 0;

            dicOrderType.Add("限價", "L");
            dicOrderType.Add("市價", "L");//待確認
            dicOrderType.Add("漲停", "C");
            dicOrderType.Add("跌停", "F");
            this.WindowState = FormWindowState.Maximized;
        }
        #region method
        /// <summary>
        /// 股價>100,一檔0.5
        /// 其餘為0.1
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        private double tick(double price)
        {
            double _Value = 0.0;
            if (price > 100) { _Value = 0.5; }
            else { _Value = 0.1; }
            return _Value;
        }
        private void tick(TextBox txtPrice, bool symbol)
        {
            double _Price = txtPrice.Text != "" ? Convert.ToDouble(txtPrice.Text) : 0.0;
            double _Value = 0.0;
            if (_Price > 100) { _Value = 0.5; }
            else { _Value = 0.1; }
            if (symbol) _Price += _Value;
            else _Price -= _Value;
            txtPrice.Text = _Price.ToString();

        }
        private void dynamicOberserver(string s)
        {
            // 
            // btnSell
            // 
            Button btnSell = new Button();
            btnSell.BackColor = System.Drawing.Color.LightGreen;
            btnSell.Location = new System.Drawing.Point(1017, 8);
            btnSell.Margin = new System.Windows.Forms.Padding(4);
            btnSell.Name = "btnSell" + s;
            btnSell.Size = new System.Drawing.Size(52, 29);
            btnSell.TabIndex = 6;
            btnSell.Tag = s;
            btnSell.Text = "現賣";
            btnSell.UseVisualStyleBackColor = false;
            btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnBuy
            // 
            Button btnBuy = new Button();
            btnBuy.BackColor = System.Drawing.Color.Red;
            btnBuy.Location = new System.Drawing.Point(964, 8);
            btnBuy.Margin = new System.Windows.Forms.Padding(4);
            btnBuy.Name = "btnBuy" + s;
            btnBuy.Size = new System.Drawing.Size(52, 29);
            btnBuy.TabIndex = 5;
            btnBuy.Tag = s;
            btnBuy.Text = "現買";
            btnBuy.UseVisualStyleBackColor = false;
            btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
            // 
            // nupQty
            // 
            NumericUpDown nupQty = new NumericUpDown();
            nupQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            nupQty.Location = new System.Drawing.Point(141, 8);
            nupQty.Margin = new System.Windows.Forms.Padding(4);
            nupQty.Name = "nupQty" + s;
            nupQty.Size = new System.Drawing.Size(48, 25);
            nupQty.TabIndex = 8;
            nupQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnOverSell
            // 
            Button btnOverSell = new Button();
            btnOverSell.BackColor = System.Drawing.Color.PeachPuff;
            btnOverSell.Location = new System.Drawing.Point(911, 8);
            btnOverSell.Margin = new System.Windows.Forms.Padding(4);
            btnOverSell.Name = "btnOverSell" + s;
            btnOverSell.Size = new System.Drawing.Size(52, 29);
            btnOverSell.TabIndex = 4;
            btnOverSell.Tag = s;
            btnOverSell.Text = "沖賣";
            btnOverSell.UseVisualStyleBackColor = false;
            btnOverSell.Click += new System.EventHandler(this.btnOverSell_Click);
            // 
            // txtPrice
            // 
            TextBox txtPrice = new TextBox();
            txtPrice.Location = new System.Drawing.Point(277, 8);
            txtPrice.Margin = new System.Windows.Forms.Padding(4);
            txtPrice.Name = "txtPrice" + s;
            txtPrice.Size = new System.Drawing.Size(55, 25);
            txtPrice.TabIndex = 9;
            txtPrice.Tag = s;
            txtPrice.Text = "0";
            // 
            // btnMarginTrading
            // 
            Button btnMarginTrading = new Button();
            btnMarginTrading.BackColor = System.Drawing.Color.IndianRed;
            btnMarginTrading.Location = new System.Drawing.Point(803, 8);
            btnMarginTrading.Margin = new System.Windows.Forms.Padding(4);
            btnMarginTrading.Name = "btnMarginTrading" + s;
            btnMarginTrading.Size = new System.Drawing.Size(52, 29);
            btnMarginTrading.TabIndex = 3;
            btnMarginTrading.Tag = s;
            btnMarginTrading.Text = "資買";
            btnMarginTrading.UseVisualStyleBackColor = false;
            btnMarginTrading.Click += new System.EventHandler(this.btnMarginTrading_Click);
            // 
            // btnMinus
            // 
            Button btnMinus = new Button();
            btnMinus.Font = new System.Drawing.Font("新細明體", 12F);
            btnMinus.Location = new System.Drawing.Point(341, 6);
            btnMinus.Margin = new System.Windows.Forms.Padding(4);
            btnMinus.Name = "btnMinus";
            btnMinus.Size = new System.Drawing.Size(29, 29);
            btnMinus.TabIndex = 10;
            btnMinus.Tag = s;
            btnMinus.Text = "-";
            btnMinus.UseVisualStyleBackColor = true;
            btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnSellingShort
            // 
            Button btnSellingShort = new Button();
            btnSellingShort.BackColor = System.Drawing.Color.DarkGreen;
            btnSellingShort.Location = new System.Drawing.Point(748, 8);
            btnSellingShort.Margin = new System.Windows.Forms.Padding(4);
            btnSellingShort.Name = "btnSellingShort" + s;
            btnSellingShort.Size = new System.Drawing.Size(52, 29);
            btnSellingShort.TabIndex = 2;
            btnSellingShort.Tag = s;
            btnSellingShort.Text = "券賣";
            btnSellingShort.UseVisualStyleBackColor = false;
            btnSellingShort.Click += new System.EventHandler(this.btnSellingShort_Click);
            // 
            // btnAdd
            // 
            Button btnAdd = new Button();
            btnAdd.Font = new System.Drawing.Font("新細明體", 12F);
            btnAdd.Location = new System.Drawing.Point(379, 6);
            btnAdd.Margin = new System.Windows.Forms.Padding(4);
            btnAdd.Name = "btnAdd" + s;
            btnAdd.Size = new System.Drawing.Size(29, 30);
            btnAdd.TabIndex = 11;
            btnAdd.Tag = s;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtStockId
            // 
            TextBox txtStockId = new TextBox();
            txtStockId.Location = new System.Drawing.Point(61, 8);
            txtStockId.Margin = new System.Windows.Forms.Padding(4);
            txtStockId.Name = "txtStockId" + s;
            txtStockId.Size = new System.Drawing.Size(73, 25);
            txtStockId.TabIndex = 1;
            txtStockId.Text = s;
            txtStockId.TextChanged += new System.EventHandler(this.txtStockId_TextChanged);
            // 
            // label6
            // 
            Label label6 = new Label();
            label6.Location = new System.Drawing.Point(5, 12);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(22, 15);
            label6.TabIndex = 13;
            label6.Text = s;
            // 
            // label4
            // 
            Label label4 = new Label();
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(731, 15);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(14, 15);
            label4.TabIndex = 47;
            label4.Text = "0";
            // 
            // btnDeal
            //  
            Button btnDeal = new Button();
            btnDeal.BackColor = System.Drawing.Color.ForestGreen;
            btnDeal.Enabled = true;
            btnDeal.Location = new System.Drawing.Point(1125, 9);
            btnDeal.Margin = new System.Windows.Forms.Padding(4);
            btnDeal.Name = "btnDeal" + s;
            btnDeal.Size = new System.Drawing.Size(52, 29);
            btnDeal.TabIndex = 48;
            btnDeal.Text = "成交";
            btnDeal.Tag = s;
            btnDeal.UseVisualStyleBackColor = true;
            btnDeal.Click += new System.EventHandler(this.btnDeal_Click);
            // 
            // btnMarginTradingSell
            // 
            Button btnMarginTradingSell = new Button();
            btnMarginTradingSell.BackColor = System.Drawing.Color.Aquamarine;
            btnMarginTradingSell.Location = new System.Drawing.Point(856, 8);
            btnMarginTradingSell.Margin = new System.Windows.Forms.Padding(4);
            btnMarginTradingSell.Name = "btnMarginTradingSell" + s;
            btnMarginTradingSell.Size = new System.Drawing.Size(52, 29);
            btnMarginTradingSell.TabIndex = 49;
            btnMarginTradingSell.Text = "資賣";
            btnMarginTradingSell.Tag = s;
            btnMarginTradingSell.UseVisualStyleBackColor = false;
            btnMarginTradingSell.Click += new System.EventHandler(this.btnMarginTradingSell_Click);
            // 
            // btnLending
            // 
            Button btnLending = new Button();
            btnLending.BackColor = System.Drawing.Color.DeepPink;
            btnLending.Location = new System.Drawing.Point(1071, 8);
            btnLending.Margin = new System.Windows.Forms.Padding(4);
            btnLending.Name = "btnLending" + s;
            btnLending.Size = new System.Drawing.Size(52, 29);
            btnLending.TabIndex = 50;
            btnLending.Text = "券買";
            btnLending.Tag = s;
            btnLending.UseVisualStyleBackColor = false;
            btnLending.Click += new System.EventHandler(this.btnLending_Click);
            // 
            // btnChangePrice
            // 
            Button btnChangePrice = new Button();
            btnChangePrice.BackColor = System.Drawing.Color.ForestGreen;
            btnChangePrice.Location = new System.Drawing.Point(1183, 9);
            btnChangePrice.Margin = new System.Windows.Forms.Padding(4);
            btnChangePrice.Name = "btnChangePrice" + s;
            btnChangePrice.Size = new System.Drawing.Size(52, 29);
            btnChangePrice.TabIndex = 51;
            btnChangePrice.Text = "改價";
            btnChangePrice.Tag = s;
            btnChangePrice.UseVisualStyleBackColor = true;
            btnChangePrice.Click += new System.EventHandler(this.btnChangePrice_Click);
            // 
            // cbPriceType
            // 
            ComboBox cbPriceType = new ComboBox();
            cbPriceType.FormattingEnabled = true;
            cbPriceType.Items.AddRange(new object[] {
            "限價",
            "市價",
            "漲停",
            "跌停"});
            cbPriceType.Location = new System.Drawing.Point(195, 9);
            cbPriceType.Margin = new System.Windows.Forms.Padding(4);
            cbPriceType.Name = "cbPriceType" + s;
            cbPriceType.Size = new System.Drawing.Size(77, 23);
            cbPriceType.TabIndex = 0;
            cbPriceType.SelectedIndex = 0;
            cbPriceType.Tag = s;
            cbPriceType.SelectedIndexChanged += new System.EventHandler(this.cbPriceType_SelectedIndexChanged);
            // 
            // label10
            // 
            Label label10 = new Label();
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(412, 15);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(22, 15);
            label10.TabIndex = 52;
            label10.Text = "日";
            // 
            // label11
            // 
            Label label11 = new Label();
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(572, 15);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(22, 15);
            label11.TabIndex = 53;
            label11.Text = "月";
            // 
            // label12
            // 
            Label label12 = new Label();
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(640, 15);
            label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(22, 15);
            label12.TabIndex = 54;
            label12.Text = "季";
            // 
            // txtMa5
            // 
            TextBox txtMa5 = new TextBox();
            txtMa5.Location = new System.Drawing.Point(433, 9);
            txtMa5.Margin = new System.Windows.Forms.Padding(4);
            txtMa5.Name = "txtMa5" + s;
            txtMa5.ReadOnly = true;
            txtMa5.Size = new System.Drawing.Size(45, 25);
            txtMa5.TabIndex = 55;
            // 
            // txtMa20
            // 
            TextBox txtMa20 = new TextBox();
            txtMa20.Location = new System.Drawing.Point(593, 9);
            txtMa20.Margin = new System.Windows.Forms.Padding(4);
            txtMa20.Name = "txtMa20" + s;
            txtMa20.ReadOnly = true;
            txtMa20.Size = new System.Drawing.Size(45, 25);
            txtMa20.TabIndex = 57;
            // 
            // txtMa60
            // 
            TextBox txtMa60 = new TextBox();
            txtMa60.Location = new System.Drawing.Point(663, 9);
            txtMa60.Margin = new System.Windows.Forms.Padding(4);
            txtMa60.Name = "txtMa60" + s;
            txtMa60.ReadOnly = true;
            txtMa60.Size = new System.Drawing.Size(45, 25);
            txtMa60.TabIndex = 56;
            // 
            // label13
            // 
            Label label13 = new Label();
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(481, 14);
            label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(36, 15);
            label13.TabIndex = 58;
            label13.Text = "10日";
            // 
            // txtMa10
            // 
            TextBox txtMa10 = new TextBox();
            txtMa10.Location = new System.Drawing.Point(524, 9);
            txtMa10.Margin = new System.Windows.Forms.Padding(4);
            txtMa10.Name = "txtMa10" + s;
            txtMa10.ReadOnly = true;
            txtMa10.Size = new System.Drawing.Size(45, 25);
            txtMa10.TabIndex = 59;
            // 
            // btnReset
            // 
            Button btnReset = new Button();
            btnReset.BackColor = System.Drawing.Color.Gray;
            btnReset.Location = new System.Drawing.Point(1241, 8);
            btnReset.Margin = new System.Windows.Forms.Padding(4);
            btnReset.Name = "btnReset" + s;
            btnReset.Size = new System.Drawing.Size(52, 29);
            btnReset.TabIndex = 60;
            btnReset.Text = "重置";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Tag = s;
            btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // panel1
            // 
            Panel panel1 = new Panel();
            panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            panel1.Controls.Add(txtMa10);
            panel1.Controls.Add(label13);
            panel1.Controls.Add(txtMa60);
            panel1.Controls.Add(txtMa20);
            panel1.Controls.Add(txtMa5);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(cbPriceType);
            panel1.Controls.Add(btnChangePrice);
            panel1.Controls.Add(btnLending);
            panel1.Controls.Add(btnMarginTradingSell);
            panel1.Controls.Add(btnDeal);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(txtStockId);
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(btnSellingShort);
            panel1.Controls.Add(btnMinus);
            panel1.Controls.Add(btnMarginTrading);
            panel1.Controls.Add(txtPrice);
            panel1.Controls.Add(btnOverSell);
            panel1.Controls.Add(nupQty);
            panel1.Controls.Add(btnBuy);
            panel1.Controls.Add(btnSell);
            panel1.Controls.Add(btnReset);
            panel1.Location = new System.Drawing.Point(23, 103);
            panel1.Margin = new System.Windows.Forms.Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1509, 44);
            panel1.TabIndex = 12;
            flowLayoutPanel1.Controls.Add(panel1);
        }
        #endregion

        private void txtStockId_TextChanged(object sender, EventArgs e)
        {
            nupQty.Value = 1;
            txtPrice.Text = "0";
        }
        /// <summary>
        /// 根據分號動態產生觀察名單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClick_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Controls.Clear();
            string[] _Oberservers = txtOberserver.Text.Split(';');
            foreach (string s in _Oberservers)
            {
                dynamicOberserver(s);
            }
        }
        /// <summary>
        /// 成交按鈕壓上後,代筆該筆委託單已成交只會顯示可沖按鈕
        /// 券賣:顯示[券買][資買]
        /// 沖賣:顯示[現買]
        /// 資買:顯示[資賣]
        /// 現買:顯示[現賣]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeal_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            _Btn.BackColor = Color.Yellow;
            _Btn.Enabled = false;
            Button _BtnSell = (Button)flowLayoutPanel1.Controls.Find("btnSell" + _Btn.Tag, true)[0];
            Button _BtnBuy = (Button)flowLayoutPanel1.Controls.Find("btnBuy" + _Btn.Tag, true)[0];
            Button _BtnOverSell = (Button)flowLayoutPanel1.Controls.Find("btnOverSell" + _Btn.Tag, true)[0];
            Button _BtnMarginTrading = (Button)flowLayoutPanel1.Controls.Find("btnMarginTrading" + _Btn.Tag, true)[0];
            Button _BtnSellingShort = (Button)flowLayoutPanel1.Controls.Find("btnSellingShort" + _Btn.Tag, true)[0];
            Button _BtnMarginTradingSell = (Button)flowLayoutPanel1.Controls.Find("btnMarginTradingSell" + _Btn.Tag, true)[0];
            Button _BtnLending = (Button)flowLayoutPanel1.Controls.Find("btnLending" + _Btn.Tag, true)[0];
            _BtnLending.Enabled = false;
            _BtnBuy.Enabled = false;
            _BtnSell.Enabled = false;
            _BtnMarginTrading.Enabled = false;
            _BtnOverSell.Enabled = false;
            _BtnSellingShort.Enabled = false;
            _BtnMarginTradingSell.Enabled = false;

            _BtnBuy.BackColor = Color.Gainsboro;
            _BtnSell.BackColor = Color.Gainsboro;
            _BtnMarginTrading.BackColor = Color.Gainsboro;
            _BtnOverSell.BackColor = Color.Gainsboro;
            _BtnSellingShort.BackColor = Color.Gainsboro;
            _BtnMarginTradingSell.BackColor = Color.Gainsboro;
            _BtnLending.BackColor = Color.Gainsboro;
            switch (_Btn.Text)
            {
                case "券賣":
                    _BtnMarginTrading.Enabled = true;
                    _BtnMarginTrading.BackColor = Color.IndianRed;
                    _BtnLending.Enabled = true;
                    _BtnLending.BackColor = Color.DeepPink;
                    break;
                case "資買":
                    _BtnMarginTradingSell.Enabled = true;
                    _BtnMarginTradingSell.BackColor = Color.Aquamarine;
                    break;
                case "沖賣":
                    _BtnBuy.Enabled = true;
                    _BtnBuy.BackColor = Color.Red;
                    break;
                case "現買":
                    _BtnSell.Enabled = true;
                    _BtnSell.BackColor = Color.LightGreen;
                    break;
            }
        }

        private void btnMarginTradingSell_Click(object sender, EventArgs e)
        {
            string _BuySell = "S";
            string _CashMarginShort = "M";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
            string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
            string _PriceType = this.cbPriceType.SelectedItem.ToString();
            Button _Btn = sender as Button;
            if (Convert.ToString(_Btn.Tag) == "")
            {
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                btnDeal.Text = "資賣";
                _StockId = txtStockId.Text;
            }
            else
            {
                NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
                _Units = Convert.ToString(_NupQty.Value);
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text;
                Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
                _BtnDeal.Text = "資賣";
                ComboBox _CbPriceType = (ComboBox)flowLayoutPanel1.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
                _OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
                _MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
                _PriceType = _CbPriceType.SelectedItem.ToString();
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你融資賣出" + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元";
            MessageBox.Show(_Parameter);
            Color color = Color.DarkGreen;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnLending_Click(object sender, EventArgs e)
        {
            string _BuySell = "B";
            string _CashMarginShort = "S";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
            string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
            string _PriceType = this.cbPriceType.SelectedItem.ToString();
            Button _Btn = sender as Button;
            if (Convert.ToString(_Btn.Tag) == "")
            {
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                btnDeal.Text = "券買";
                _StockId = txtStockId.Text;
            }
            else
            {
                NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
                _Units = Convert.ToString(_NupQty.Value);
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text;
                Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
                _BtnDeal.Text = "券買";
                ComboBox _CbPriceType = (ComboBox)flowLayoutPanel1.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
                _OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
                _MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
                _PriceType = _CbPriceType.SelectedItem.ToString();
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你融券買進" + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元";
            MessageBox.Show(_Parameter);
            Color color = Color.DeepPink;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }
        /// <summary>
        /// 一鍵將所有庫存出清,用於黑天鵝降臨崩盤的時候
        /// 執行前須先填入密碼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllStopLoss_Click(object sender, EventArgs e)
        {
            string _Password = ConfigurationManager.AppSettings["Password"];
            if (txtPassword.Text == _Password)
            {
                MessageBox.Show("出清庫存");
                string _BuySell = "S";
                string _CashMarginShort = "C";
                string _Units = "0";
                string _Price = "0";
                string _StockId = "";
                string _OrderType = "F";//跌停出
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                _StockId = txtStockId.Text;

                string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort;
                StringBuilder sb = new StringBuilder(_Parameter);
                HTSOrder(sb);
                string _Msg = "你現股賣出" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
                MessageBox.Show(_Parameter);
                Color color = Color.Green;
                richTxtInfo.SelectionColor = color;
                richTxtInfo.AppendText(_Msg + Environment.NewLine);
            }
            else
            {
                MessageBox.Show("Enter Your Password", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 更改該委託編號的價格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePrice_Click(object sender, EventArgs e)
        {
            string _OrderID = "";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            Button _Btn = sender as Button;
            if (Convert.ToString(_Btn.Tag) == "")
            {
                _Units = Convert.ToString(nupQty.Value);
                _Price = txtPrice.Text;
                btnDeal.Text = "改價";
                _StockId = txtStockId.Text;
            }
            else
            {
                NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
                _Units = Convert.ToString(_NupQty.Value);
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text;
                Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
                _BtnDeal.Text = "改價";
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",OrderID=" + _OrderID + ",Price=" + _Price + ",Units=" + _Units;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrderChgPrice(sb);
            MessageBox.Show(_Parameter);
            string _Msg = "你委託改價" + _StockId + "共" + _Units + "張,價格" + _Price + "元,委託編號:" + _OrderID;
            Color color = Color.PaleGoldenrod;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }
        /// <summary>
        /// 價格類型非限價的價格都設定為0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbPriceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox _CbPriceType = sender as ComboBox;
            if (Convert.ToString(_CbPriceType.Tag) == "")
            {
                if (this.cbPriceType.SelectedItem.ToString() != "限價") this.txtPrice.Text = "0";
            }
            else
            {
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _CbPriceType.Tag, true)[0];
                if (_CbPriceType.SelectedItem.ToString() != "限價") _TxtPrice.Text = "0";
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            this.cbPriceType.SelectedItem = "限價";
            this.nupQty.Value = 1;
            Button _BtnSell = (Button)flowLayoutPanel1.Controls.Find("btnSell" + _Btn.Tag, true)[0];
            Button _BtnBuy = (Button)flowLayoutPanel1.Controls.Find("btnBuy" + _Btn.Tag, true)[0];
            Button _BtnOverSell = (Button)flowLayoutPanel1.Controls.Find("btnOverSell" + _Btn.Tag, true)[0];
            Button _BtnMarginTrading = (Button)flowLayoutPanel1.Controls.Find("btnMarginTrading" + _Btn.Tag, true)[0];
            Button _BtnSellingShort = (Button)flowLayoutPanel1.Controls.Find("btnSellingShort" + _Btn.Tag, true)[0];
            Button _BtnMarginTradingSell = (Button)flowLayoutPanel1.Controls.Find("btnMarginTradingSell" + _Btn.Tag, true)[0];
            Button _BtnLending = (Button)flowLayoutPanel1.Controls.Find("btnLending" + _Btn.Tag, true)[0];
            Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
            _BtnLending.Enabled = true;
            _BtnBuy.Enabled = true;
            _BtnSell.Enabled = true;
            _BtnMarginTrading.Enabled = true;
            _BtnOverSell.Enabled = true;
            _BtnSellingShort.Enabled = true;
            _BtnMarginTradingSell.Enabled = true;
            _BtnDeal.Enabled = true;
            _BtnBuy.BackColor = Color.Red;
            _BtnSell.BackColor = Color.LightGreen;
            _BtnMarginTrading.BackColor = Color.IndianRed;
            _BtnOverSell.BackColor = Color.Salmon;
            _BtnSellingShort.BackColor = Color.DarkGreen;
            _BtnMarginTradingSell.BackColor = Color.Aquamarine;
            _BtnLending.BackColor = Color.DeepPink;
            _BtnDeal.Text = "成交";
            _BtnDeal.BackColor = SystemColors.Control;
            
        }
    }
}
