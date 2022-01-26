namespace HtsApiDemo
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtStockAccount = new System.Windows.Forms.TextBox();
            this.lbStockAccount = new System.Windows.Forms.Label();
            this.txtStockId = new System.Windows.Forms.TextBox();
            this.btnSellingShort = new System.Windows.Forms.Button();
            this.btnMarginTrading = new System.Windows.Forms.Button();
            this.btnOverSell = new System.Windows.Forms.Button();
            this.btnBuy = new System.Windows.Forms.Button();
            this.btnSell = new System.Windows.Forms.Button();
            this.nupQty = new System.Windows.Forms.NumericUpDown();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.richTxtInfo = new System.Windows.Forms.RichTextBox();
            this.txtOberserver = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClick = new System.Windows.Forms.Button();
            this.btnDeal = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnMarginTradingSell = new System.Windows.Forms.Button();
            this.btnLending = new System.Windows.Forms.Button();
            this.btnChangePrice = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nupQty)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStockAccount
            // 
            this.txtStockAccount.Location = new System.Drawing.Point(74, 4);
            this.txtStockAccount.Name = "txtStockAccount";
            this.txtStockAccount.Size = new System.Drawing.Size(115, 22);
            this.txtStockAccount.TabIndex = 42;
            this.txtStockAccount.Text = "050-0333866";
            // 
            // lbStockAccount
            // 
            this.lbStockAccount.AutoSize = true;
            this.lbStockAccount.Location = new System.Drawing.Point(12, 9);
            this.lbStockAccount.Name = "lbStockAccount";
            this.lbStockAccount.Size = new System.Drawing.Size(56, 12);
            this.lbStockAccount.TabIndex = 41;
            this.lbStockAccount.Text = "證券帳號:";
            // 
            // txtStockId
            // 
            this.txtStockId.Location = new System.Drawing.Point(80, 6);
            this.txtStockId.Name = "txtStockId";
            this.txtStockId.Size = new System.Drawing.Size(81, 22);
            this.txtStockId.TabIndex = 1;
            this.txtStockId.TextChanged += new System.EventHandler(this.txtStockId_TextChanged);
            // 
            // btnSellingShort
            // 
            this.btnSellingShort.BackColor = System.Drawing.Color.DarkGreen;
            this.btnSellingShort.Location = new System.Drawing.Point(350, 6);
            this.btnSellingShort.Name = "btnSellingShort";
            this.btnSellingShort.Size = new System.Drawing.Size(39, 23);
            this.btnSellingShort.TabIndex = 2;
            this.btnSellingShort.Text = "券賣";
            this.btnSellingShort.UseVisualStyleBackColor = false;
            this.btnSellingShort.Click += new System.EventHandler(this.btnSellingShort_Click);
            // 
            // btnMarginTrading
            // 
            this.btnMarginTrading.BackColor = System.Drawing.Color.IndianRed;
            this.btnMarginTrading.Location = new System.Drawing.Point(391, 6);
            this.btnMarginTrading.Name = "btnMarginTrading";
            this.btnMarginTrading.Size = new System.Drawing.Size(39, 23);
            this.btnMarginTrading.TabIndex = 3;
            this.btnMarginTrading.Text = "資買";
            this.btnMarginTrading.UseVisualStyleBackColor = false;
            this.btnMarginTrading.Click += new System.EventHandler(this.btnMarginTrading_Click);
            // 
            // btnOverSell
            // 
            this.btnOverSell.BackColor = System.Drawing.Color.PeachPuff;
            this.btnOverSell.Location = new System.Drawing.Point(432, 6);
            this.btnOverSell.Name = "btnOverSell";
            this.btnOverSell.Size = new System.Drawing.Size(39, 23);
            this.btnOverSell.TabIndex = 4;
            this.btnOverSell.Text = "沖賣";
            this.btnOverSell.UseVisualStyleBackColor = false;
            this.btnOverSell.Click += new System.EventHandler(this.btnOverSell_Click);
            // 
            // btnBuy
            // 
            this.btnBuy.BackColor = System.Drawing.Color.Red;
            this.btnBuy.Location = new System.Drawing.Point(471, 6);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(39, 23);
            this.btnBuy.TabIndex = 5;
            this.btnBuy.Text = "現買";
            this.btnBuy.UseVisualStyleBackColor = false;
            this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
            // 
            // btnSell
            // 
            this.btnSell.BackColor = System.Drawing.Color.LightGreen;
            this.btnSell.Location = new System.Drawing.Point(512, 6);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(39, 23);
            this.btnSell.TabIndex = 6;
            this.btnSell.Text = "現賣";
            this.btnSell.UseVisualStyleBackColor = false;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // nupQty
            // 
            this.nupQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.nupQty.Location = new System.Drawing.Point(167, 6);
            this.nupQty.Name = "nupQty";
            this.nupQty.Size = new System.Drawing.Size(36, 22);
            this.nupQty.TabIndex = 8;
            this.nupQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(209, 6);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(42, 22);
            this.txtPrice.TabIndex = 9;
            this.txtPrice.Tag = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 44;
            this.label1.Text = "股名/代號";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 45;
            this.label2.Text = "數量";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(229, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 46;
            this.label3.Text = "價格";
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnMinus.Location = new System.Drawing.Point(257, 5);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(22, 23);
            this.btnMinus.TabIndex = 10;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("新細明體", 12F);
            this.btnAdd.Location = new System.Drawing.Point(285, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(22, 24);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel1.Controls.Add(this.btnChangePrice);
            this.panel1.Controls.Add(this.btnLending);
            this.panel1.Controls.Add(this.btnMarginTradingSell);
            this.panel1.Controls.Add(this.btnDeal);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtStockId);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnSellingShort);
            this.panel1.Controls.Add(this.btnMinus);
            this.panel1.Controls.Add(this.btnMarginTrading);
            this.panel1.Controls.Add(this.txtPrice);
            this.panel1.Controls.Add(this.btnOverSell);
            this.panel1.Controls.Add(this.nupQty);
            this.panel1.Controls.Add(this.btnBuy);
            this.panel1.Controls.Add(this.btnSell);
            this.panel1.Location = new System.Drawing.Point(16, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 34);
            this.panel1.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(315, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 47;
            this.label4.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "光群雷";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(15, 124);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(773, 298);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(328, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 47;
            this.label5.Text = "券餘";
            // 
            // richTxtInfo
            // 
            this.richTxtInfo.Location = new System.Drawing.Point(15, 555);
            this.richTxtInfo.Name = "richTxtInfo";
            this.richTxtInfo.ReadOnly = true;
            this.richTxtInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTxtInfo.Size = new System.Drawing.Size(773, 178);
            this.richTxtInfo.TabIndex = 13;
            this.richTxtInfo.Text = "";
            // 
            // txtOberserver
            // 
            this.txtOberserver.Location = new System.Drawing.Point(257, 4);
            this.txtOberserver.Name = "txtOberserver";
            this.txtOberserver.Size = new System.Drawing.Size(478, 22);
            this.txtOberserver.TabIndex = 48;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(195, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 49;
            this.label7.Text = "觀察名單";
            // 
            // btnClick
            // 
            this.btnClick.Location = new System.Drawing.Point(741, 3);
            this.btnClick.Name = "btnClick";
            this.btnClick.Size = new System.Drawing.Size(47, 23);
            this.btnClick.TabIndex = 50;
            this.btnClick.Text = "產生";
            this.btnClick.UseVisualStyleBackColor = true;
            this.btnClick.Click += new System.EventHandler(this.btnClick_Click);
            // 
            // btnDeal
            // 
            this.btnDeal.BackColor = System.Drawing.Color.ForestGreen;
            this.btnDeal.Location = new System.Drawing.Point(633, 7);
            this.btnDeal.Name = "btnDeal";
            this.btnDeal.Size = new System.Drawing.Size(39, 23);
            this.btnDeal.TabIndex = 48;
            this.btnDeal.Text = "成交";
            this.btnDeal.UseVisualStyleBackColor = true;
            this.btnDeal.Click += new System.EventHandler(this.btnDeal_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(654, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 51;
            this.label8.Text = "狀態";
            // 
            // btnMarginTradingSell
            // 
            this.btnMarginTradingSell.BackColor = System.Drawing.Color.Aquamarine;
            this.btnMarginTradingSell.Location = new System.Drawing.Point(553, 6);
            this.btnMarginTradingSell.Name = "btnMarginTradingSell";
            this.btnMarginTradingSell.Size = new System.Drawing.Size(39, 23);
            this.btnMarginTradingSell.TabIndex = 49;
            this.btnMarginTradingSell.Text = "資賣";
            this.btnMarginTradingSell.UseVisualStyleBackColor = false;
            this.btnMarginTradingSell.Click += new System.EventHandler(this.btnMarginTradingSell_Click);
            // 
            // btnLending
            // 
            this.btnLending.BackColor = System.Drawing.Color.DeepPink;
            this.btnLending.Location = new System.Drawing.Point(592, 6);
            this.btnLending.Name = "btnLending";
            this.btnLending.Size = new System.Drawing.Size(39, 23);
            this.btnLending.TabIndex = 50;
            this.btnLending.Text = "券買";
            this.btnLending.UseVisualStyleBackColor = false;
            this.btnLending.Click += new System.EventHandler(this.btnLending_Click);
            // 
            // btnChangePrice
            // 
            this.btnChangePrice.BackColor = System.Drawing.Color.ForestGreen;
            this.btnChangePrice.Location = new System.Drawing.Point(676, 7);
            this.btnChangePrice.Name = "btnChangePrice";
            this.btnChangePrice.Size = new System.Drawing.Size(39, 23);
            this.btnChangePrice.TabIndex = 51;
            this.btnChangePrice.Text = "改價";
            this.btnChangePrice.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 745);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClick);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtOberserver);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.richTxtInfo);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStockAccount);
            this.Controls.Add(this.lbStockAccount);
            this.Name = "FormMain";
            this.Text = "下單機";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nupQty)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtStockAccount;
        private System.Windows.Forms.Label lbStockAccount;
        private System.Windows.Forms.TextBox txtStockId;
        private System.Windows.Forms.Button btnMarginTrading;
        private System.Windows.Forms.Button btnSellingShort;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Button btnBuy;
        private System.Windows.Forms.Button btnOverSell;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.NumericUpDown nupQty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTxtInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOberserver;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClick;
        private System.Windows.Forms.Button btnDeal;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnMarginTradingSell;
        private System.Windows.Forms.Button btnLending;
        private System.Windows.Forms.Button btnChangePrice;
    }
}