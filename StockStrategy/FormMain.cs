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
namespace HtsApiDemo
{
    public partial class FormMain : Form
    {
        [DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSOrder", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void HTSOrder([MarshalAs(UnmanagedType.LPStr)]StringBuilder APChar);
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
            string _BuySell1 = "S";
            string _BuySell = "S";
            string _CashMarginShort = "S";
            string _Units = "0";
            string _Price = "0";
            string _StockId = "";
            Button _Btn = sender as Button;
            if (_Btn.Tag.ToString() == "")
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
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=L,Price=" + _Price + ",CashMarginShort=" + _CashMarginShort;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你融券賣出" + _StockId + "共" + _Units + "張,成本" + _Price + "元";
            Color color = Color.DarkGreen;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            TextBox _TxtPrice = new TextBox();
            _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
            double _Price = _TxtPrice.Text != "" ? Convert.ToDouble(_TxtPrice.Text) : 0.0;
            double _Value = tick(_TxtPrice.Text);
            _Price -= _Value;
            _TxtPrice.Text = _Price.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            TextBox _TxtPrice = new TextBox();
            _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
            double _Price = _TxtPrice.Text != "" ? Convert.ToDouble(_TxtPrice.Text) : 0.0;
            double _Value = tick(_TxtPrice.Text);
            _Price += _Value;
            _TxtPrice.Text = _Price.ToString();
        }

        private void btnMarginTrading_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
            TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
            Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
            _BtnDeal.Text = "資買";
            string _BuySell = "B";
            string _CashMarginShort = "M";
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _Btn.Tag + ",BuySell=" + _BuySell + ",Units=" + Convert.ToString(_NupQty.Value) + ",OrderType=L,Price=" + _TxtPrice.Text + ",CashMarginShort=" + _CashMarginShort;
            // MessageBox.Show(_Parameter);
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你融資買進" + _Btn.Tag + "共" + Convert.ToString(_NupQty.Value) + "張,成本" + _TxtPrice.Text + "元";
            Color color = Color.DarkRed;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnOverSell_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
            TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
            Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
            _BtnDeal.Text = "沖賣";
            string _BuySell = "S";
            string _CashMarginShort = "C";
            string _OverSell = ",OverSell=Y";
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _Btn.Tag + ",BuySell=" + _BuySell + ",Units=" + Convert.ToString(_NupQty.Value) + ",OrderType=L,Price=" + _TxtPrice.Text + ",CashMarginShort=" + _CashMarginShort + _OverSell;
            //MessageBox.Show(_Parameter);
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你當沖賣出" + _Btn.Tag + "共" + Convert.ToString(_NupQty.Value) + "張,成本" + _TxtPrice.Text + "元";
            Color color = Color.PeachPuff;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
            TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
            Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
            _BtnDeal.Text = "現買";
            string _BuySell = "B";
            string _CashMarginShort = "C";
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _Btn.Tag + ",BuySell=" + _BuySell + ",Units=" + Convert.ToString(_NupQty.Value) + ",OrderType=L,Price=" + _TxtPrice.Text + ",CashMarginShort=" + _CashMarginShort;
            //MessageBox.Show(_Parameter);
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你現股買進" + _Btn.Tag + "共" + Convert.ToString(_NupQty.Value) + "張,成本" + _TxtPrice.Text + "元";
            Color color = Color.Red;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            Button _Btn = sender as Button;
            NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
            TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
            Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
            _BtnDeal.Text = "現賣";
            string _BuySell = "S";
            string _CashMarginShort = "C";
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _Btn.Tag + ",BuySell=" + _BuySell + ",Units=" + Convert.ToString(_NupQty.Value) + ",OrderType=L,Price=" + _TxtPrice.Text + ",CashMarginShort=" + _CashMarginShort;
            //MessageBox.Show(_Parameter);
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你現股賣出" + _Btn.Tag + "共" + Convert.ToString(_NupQty.Value) + "張,成本" + _TxtPrice.Text + "元";
            Color color = Color.Green;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.txtStockId.Text = Oberservers[1]; 
            this.txtOberserver.Text = ConfigurationManager.AppSettings["Observer"];
        }
        #region method
        private double tick(string strPrice)
        {
            double _Value = 0.0;
            double _Price = strPrice != "" ? Convert.ToDouble(strPrice) : 0.0;
            if (_Price > 100) { _Value = 0.5; }
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
            txtStockId.Size = new System.Drawing.Size(81, 22);
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
            // btnOverSell
            // 
            Button btnOverSell = new Button();
            btnOverSell.Location = new System.Drawing.Point(432, 6);
            btnOverSell.Name = "btnOverSell" + s;
            btnOverSell.Size = new System.Drawing.Size(39, 23);
            btnOverSell.TabIndex = 4;
            btnOverSell.Text = "沖賣";
            btnOverSell.UseVisualStyleBackColor = true;
            btnOverSell.Tag = s;
            btnOverSell.BackColor = Color.PeachPuff;
            btnOverSell.Click += new System.EventHandler(this.btnOverSell_Click);
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
            panel.Controls.Add(btnOverSell);
            panel.Controls.Add(nupQty);
            panel.Controls.Add(btnBuy);
            panel.Controls.Add(btnSell);
            panel.Controls.Add(btnDeal);
            panel.Controls.Add(btnMarginTradingSell);
            panel.Controls.Add(btnLending);
            panel.Location = new System.Drawing.Point(3, 3);
            panel.Name = "panel" + s;
            panel.Size = new System.Drawing.Size(770, 34);
            //panel1.TabIndex = 12;
            this.flowLayoutPanel1.Controls.Add(panel);

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
            _BtnBuy.Enabled = false;
            _BtnSell.Enabled = false;
            _BtnMarginTrading.Enabled = false;
            _BtnOverSell.Enabled = false;
            _BtnSellingShort.Enabled = false;

            _BtnBuy.BackColor = Color.Gainsboro;
            _BtnSell.BackColor = Color.Gainsboro;
            _BtnMarginTrading.BackColor = Color.Gainsboro;
            _BtnOverSell.BackColor = Color.Gainsboro;
            _BtnSellingShort.BackColor = Color.Gainsboro;
            switch (_Btn.Text)
            {
                case "券賣":
                    _BtnMarginTrading.Enabled = true;
                    _BtnMarginTrading.BackColor = Color.IndianRed;
                    break;
                case "資買":
                    //_BtnMarginTrading.Enabled = true;
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
            Button _Btn = sender as Button;
            NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
            TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
            Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
            _BtnDeal.Text = "資賣";
            string _BuySell = "S";
            string _CashMarginShort = "S";
            string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _Btn.Tag + ",BuySell=" + _BuySell + ",Units=" + Convert.ToString(_NupQty.Value) + ",OrderType=L,Price=" + _TxtPrice.Text + ",CashMarginShort=" + _CashMarginShort;
            StringBuilder sb = new StringBuilder(_Parameter);
            HTSOrder(sb);
            string _Msg = "你融券賣出" + _Btn.Tag + "共" + Convert.ToString(_NupQty.Value) + "張,成本" + _TxtPrice.Text + "元";

            Color color = Color.DarkGreen;
            richTxtInfo.SelectionColor = color;
            richTxtInfo.AppendText(_Msg + Environment.NewLine);
        }

        private void btnLending_Click(object sender, EventArgs e)
        {

        }
    }
}
