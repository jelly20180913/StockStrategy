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
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            MessageBox.Show(_Parameter);
            string _Msg = "你融券賣出" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
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
                _Price -= _Value;
                txtPrice.Text = _Price.ToString();
            }
            else
            {
                TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
                _Price = _TxtPrice.Text != "" ? Convert.ToDouble(_TxtPrice.Text) : 0.0;
                _Value = tick(_Price);
                _Price -= _Value;
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
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            MessageBox.Show(_Parameter);
            string _Msg = "你融資買進" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
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
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + _Price + ",CashMarginShort=" + _CashMarginShort + _OverSell + _MarketLimit; ;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你當沖賣出" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
            MessageBox.Show(_Parameter);
            Color color = Color.PeachPuff;
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
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你現股買進" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
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
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你現股賣出" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
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
            dicOrderType.Add("漲停", "C");
            dicOrderType.Add("跌停", "F");
        }
        #region method
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
            // label4
            // 
            Label label4 = new Label();
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(315, 12);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(11, 12);
            label4.TabIndex = 47;
            label4.Text = "0";
            // 
            // label6
            // 
            Label label6 = new Label();
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(10, 10);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(41, 12);
            label6.TabIndex = 13;
            label6.Text = s;
            // 
            // btnMinus
            // 
            Button btnMinus = new Button();
            btnMinus.Font = new System.Drawing.Font("新細明體", 12F);
            btnMinus.Location = new System.Drawing.Point(257, 5);
            btnMinus.Name = "btnMinus";
            btnMinus.Size = new System.Drawing.Size(22, 23);
            btnMinus.TabIndex = 10;
            btnMinus.Text = "-";
            btnMinus.Tag = s;
            btnMinus.UseVisualStyleBackColor = true;
            btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnAdd
            // 
            Button btnAdd = new Button();
            btnAdd.Font = new System.Drawing.Font("新細明體", 12F);
            btnAdd.Location = new System.Drawing.Point(285, 5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(22, 24);
            btnAdd.TabIndex = 11;
            btnAdd.Text = "+";
            btnAdd.Tag = s;
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtStockId
            //  
            TextBox txtStockId = new TextBox();
            txtStockId.Location = new System.Drawing.Point(80, 6);
            txtStockId.Name = "txtStockId";
            txtStockId.Size = new System.Drawing.Size(73, 25);
            txtStockId.TabIndex = 1;
            txtStockId.TextChanged += new System.EventHandler(this.txtStockId_TextChanged);
            // 
            // btnSellingShort
            // 
            Button btnSellingShort = new Button();
            btnSellingShort.Location = new System.Drawing.Point(350, 6);
            btnSellingShort.Name = "btnSellingShort" + s;
            btnSellingShort.Size = new System.Drawing.Size(39, 23);
            btnSellingShort.TabIndex = 2;
            btnSellingShort.Text = "券賣";
            btnSellingShort.UseVisualStyleBackColor = true;
            btnSellingShort.Tag = s;
            btnSellingShort.BackColor = Color.ForestGreen;
            btnSellingShort.Click += new System.EventHandler(this.btnSellingShort_Click);

            Button btnMarginTrading = new Button();

            btnMarginTrading.Location = new System.Drawing.Point(391, 6);
            btnMarginTrading.Name = "btnMarginTrading" + s;
            btnMarginTrading.Size = new System.Drawing.Size(39, 23);
            btnMarginTrading.TabIndex = 3;
            btnMarginTrading.Text = "資買";
            btnMarginTrading.UseVisualStyleBackColor = true;
            btnMarginTrading.Tag = s;
            btnMarginTrading.BackColor = Color.IndianRed;
            btnMarginTrading.Click += new System.EventHandler(this.btnMarginTrading_Click);
           
            // 
            // btnBuy
            // 
            Button btnBuy = new Button();
            btnBuy.Location = new System.Drawing.Point(471, 6);
            btnBuy.Name = "btnBuy" + s;
            btnBuy.Size = new System.Drawing.Size(39, 23);
            btnBuy.TabIndex = 5;
            btnBuy.Text = "現買";
            btnBuy.UseVisualStyleBackColor = true;
            btnBuy.Tag = s;
            btnBuy.BackColor = Color.Red;
            btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
            // 
            // btnSell
            // 
            Button btnSell = new Button();
            btnSell.Location = new System.Drawing.Point(512, 6);
            btnSell.Name = "btnSell" + s;
            btnSell.Size = new System.Drawing.Size(39, 23);
            btnSell.TabIndex = 6;
            btnSell.Text = "現賣";
            btnSell.UseVisualStyleBackColor = true;
            btnSell.Tag = s;
            btnSell.BackColor = Color.LightGreen;
            btnSell.Click += new System.EventHandler(this.btnSell_Click);
                 // 
            // btnOverSell
            // 
            Button btnOverSell = new Button();
            btnOverSell.Location = new System.Drawing.Point(532, 6);
            btnOverSell.Name = "btnOverSell" + s;
            btnOverSell.Size = new System.Drawing.Size(39, 23);
            btnOverSell.TabIndex = 4;
            btnOverSell.Text = "沖賣";
            btnOverSell.UseVisualStyleBackColor = true;
            btnOverSell.Tag = s;
            btnOverSell.BackColor = Color.PeachPuff;
            btnOverSell.Click += new System.EventHandler(this.btnOverSell_Click);
            // 
            // nupQty
            // 
            NumericUpDown nupQty = new NumericUpDown();
            nupQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            nupQty.Location = new System.Drawing.Point(167, 6);
            nupQty.Name = "nupQty" + s;
            nupQty.Size = new System.Drawing.Size(36, 22);
            nupQty.TabIndex = 8;
            nupQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtPrice
            // 
            TextBox txtPrice = new TextBox();
            txtPrice.Location = new System.Drawing.Point(209, 6);
            txtPrice.Name = "txtPrice" + s;
            txtPrice.Size = new System.Drawing.Size(42, 22);
            txtPrice.TabIndex = 9;
            txtPrice.Tag = s;
            txtPrice.Text = "0";

            // 
            // btnDeal
            // 
            Button btnDeal = new Button();
            btnDeal.BackColor = System.Drawing.Color.ForestGreen;
            btnDeal.Location = new System.Drawing.Point(633, 7);
            btnDeal.Name = "btnDeal" + s;
            btnDeal.Size = new System.Drawing.Size(39, 23);
            btnDeal.TabIndex = 48;
            btnDeal.Text = "成交";
            btnDeal.UseVisualStyleBackColor = true;
            btnDeal.Tag = s;
            btnDeal.Click += new System.EventHandler(this.btnDeal_Click);

            // 
            // btnMarginTradingSell
            // 
            Button btnMarginTradingSell = new Button();
            btnMarginTradingSell.BackColor = System.Drawing.Color.Aquamarine;
            btnMarginTradingSell.Location = new System.Drawing.Point(553, 6);
            btnMarginTradingSell.Name = "btnMarginTradingSell" + s;
            btnMarginTradingSell.Size = new System.Drawing.Size(39, 23);
            btnMarginTradingSell.TabIndex = 49;
            btnMarginTradingSell.Text = "資賣";
            btnMarginTradingSell.UseVisualStyleBackColor = false;
            btnMarginTradingSell.Tag = s;
            this.btnMarginTradingSell.Click += new System.EventHandler(this.btnMarginTradingSell_Click);
            // 
            // btnLending
            // 
            Button btnLending = new Button();
            btnLending.BackColor = System.Drawing.Color.DeepPink;
            btnLending.Location = new System.Drawing.Point(592, 6);
            btnLending.Name = "btnLending" + s;
            btnLending.Size = new System.Drawing.Size(39, 23);
            btnLending.TabIndex = 50;
            btnLending.Text = "券買";
            btnLending.UseVisualStyleBackColor = false;
            btnLending.Tag = s;
            this.btnLending.Click += new System.EventHandler(this.btnLending_Click);

            // 
            // txtMa60
            // 
            TextBox txtMa60 = new TextBox();
             txtMa60.Location = new System.Drawing.Point(663, 9);
             txtMa60.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
             txtMa60.Name = "txtMa60";
             txtMa60.ReadOnly = true;
             txtMa60.Size = new System.Drawing.Size(45, 25);
             txtMa60.TabIndex = 56;
            // 
            // txtMa20
            // 
            TextBox txtMa20 = new TextBox();
            txtMa20.Location = new System.Drawing.Point(593, 9);
            txtMa20.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            txtMa20.Name = "txtMa20";
            txtMa20.ReadOnly = true;
            txtMa20.Size = new System.Drawing.Size(45, 25);
            txtMa20.TabIndex = 57;
            // 
            // txtMa5
            // 
            TextBox txtMa5 = new TextBox();
             txtMa5.Location = new System.Drawing.Point(433, 9);
             txtMa5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
             txtMa5.Name = "txtMa5";
             txtMa5.ReadOnly = true;
             txtMa5.Size = new System.Drawing.Size(45, 25);
             txtMa5.TabIndex = 55;  // 
            // txtMa10
            // 
            TextBox txtMa10 = new TextBox();
            txtMa10.Location = new System.Drawing.Point(524, 9);
            txtMa10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            txtMa10.Name = "txtMa10";
            txtMa10.ReadOnly = true;
            txtMa10.Size = new System.Drawing.Size(45, 25);
            txtMa10.TabIndex = 59;
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
            // flowLayoutPanel1
            // 
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
             flowLayoutPanel1.Location = new System.Drawing.Point(20, 271);
             flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
             flowLayoutPanel1.Name = "flowLayoutPanel1";
             flowLayoutPanel1.Size = new System.Drawing.Size(1524, 253);
             flowLayoutPanel1.TabIndex = 13;
            Panel panel = new Panel();
            panel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            panel.Controls.Add(label4);
            panel.Controls.Add(label6);
            panel.Controls.Add(txtStockId);
            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnSellingShort);
            panel.Controls.Add(btnMinus);
            panel.Controls.Add(btnMarginTrading);
            panel.Controls.Add(txtPrice);
           
            panel.Controls.Add(nupQty);
            panel.Controls.Add(btnBuy);
            panel.Controls.Add(btnSell);
            panel.Controls.Add(btnOverSell);
            panel.Controls.Add(btnDeal);
            panel.Controls.Add(btnMarginTradingSell);
            panel.Controls.Add(btnLending);
            panel.Controls.Add(label13);
            panel.Controls.Add(label12);
            panel.Controls.Add(label11);
            panel.Controls.Add(label10);
            panel.Controls.Add(txtMa5);
            panel.Controls.Add(txtMa10);
            panel.Controls.Add(txtMa20);
            panel.Controls.Add(txtMa60);
                
            panel.Location = new System.Drawing.Point(3, 3);
            panel.Name = "panel" + s;
            panel.Size = new System.Drawing.Size(770, 34);
            //panel1.TabIndex = 12;
           flowLayoutPanel1.Controls.Add(panel);

        }
        #endregion

        private void txtStockId_TextChanged(object sender, EventArgs e)
        {
            nupQty.Value = 1;
            txtPrice.Text = "0";
        }

        private void btnClick_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Controls.Clear();
            string[] _Oberservers = txtOberserver.Text.Split(';');
            foreach (string s in _Oberservers)
            {
                dynamicOberserver(s);
            }
        }

        private void btnDeal_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            _Btn.BackColor = Color.Yellow;
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
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你融資賣出" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
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
                _StockId = _Btn.Tag.ToString();
            }
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _MarketLimit;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你融券買進" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
            MessageBox.Show(_Parameter);
            Color color = Color.DeepPink;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

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
               
                string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort  ;
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

        private void btnChangePrice_Click(object sender, EventArgs e)
        {
            string _OrderID = "";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            Button _Btn = sender as Button;
            if (Convert.ToString( _Btn.Tag) == "")
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
            string _Msg = "你委託改價" + _StockId + "共" + _Units + "張,價格" + _Price + "元,委託編號:"+_OrderID;
            Color color = Color.PaleGoldenrod;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }
    }
}
