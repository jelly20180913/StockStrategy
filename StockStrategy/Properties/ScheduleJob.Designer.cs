namespace StockStrategy.Properties
{
    partial class ScheduleJob
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
            this.components = new System.ComponentModel.Container();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnGetStock = new System.Windows.Forms.Button();
            this.txtErrMsg = new System.Windows.Forms.TextBox();
            this.btnGetIndexToInsert = new System.Windows.Forms.Button();
            this.btnGetTopIndex = new System.Windows.Forms.Button();
            this.timerInsertStockIndex = new System.Windows.Forms.Timer(this.components);
            this.btnUpdateStockIndex = new System.Windows.Forms.Button();
            this.btnInsertStockInventory = new System.Windows.Forms.Button();
            this.btnGetStockPrice = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnInsertStockLackOff = new System.Windows.Forms.Button();
            this.btnUpdateJuridicaPerson = new System.Windows.Forms.Button();
            this.btnResetLinePoint = new System.Windows.Forms.Button();
            this.btnGetGoodStock = new System.Windows.Forms.Button();
            this.txtGoodStock = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMoreGain = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPreDays2 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLessVolumn = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPreDays = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMultiple = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVolumn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nupGain = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLineGoodStock = new System.Windows.Forms.Button();
            this.btnUpdateStockGain = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lbPercent = new System.Windows.Forms.Label();
            this.lbBtnName = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupGain)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(12, 12);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnGetStock
            // 
            this.btnGetStock.Location = new System.Drawing.Point(93, 12);
            this.btnGetStock.Name = "btnGetStock";
            this.btnGetStock.Size = new System.Drawing.Size(75, 23);
            this.btnGetStock.TabIndex = 1;
            this.btnGetStock.Text = "GetStock";
            this.btnGetStock.UseVisualStyleBackColor = true;
            this.btnGetStock.Click += new System.EventHandler(this.btnGetStock_Click);
            // 
            // txtErrMsg
            // 
            this.txtErrMsg.Location = new System.Drawing.Point(1181, 12);
            this.txtErrMsg.Multiline = true;
            this.txtErrMsg.Name = "txtErrMsg";
            this.txtErrMsg.Size = new System.Drawing.Size(363, 103);
            this.txtErrMsg.TabIndex = 3;
            // 
            // btnGetIndexToInsert
            // 
            this.btnGetIndexToInsert.Location = new System.Drawing.Point(175, 12);
            this.btnGetIndexToInsert.Name = "btnGetIndexToInsert";
            this.btnGetIndexToInsert.Size = new System.Drawing.Size(97, 23);
            this.btnGetIndexToInsert.TabIndex = 4;
            this.btnGetIndexToInsert.Text = "getIndexToInsert";
            this.btnGetIndexToInsert.UseVisualStyleBackColor = true;
            this.btnGetIndexToInsert.Click += new System.EventHandler(this.btnGetIndexToInsert_Click);
            // 
            // btnGetTopIndex
            // 
            this.btnGetTopIndex.Location = new System.Drawing.Point(294, 12);
            this.btnGetTopIndex.Name = "btnGetTopIndex";
            this.btnGetTopIndex.Size = new System.Drawing.Size(75, 23);
            this.btnGetTopIndex.TabIndex = 5;
            this.btnGetTopIndex.Text = "getTopIndex";
            this.btnGetTopIndex.UseVisualStyleBackColor = true;
            this.btnGetTopIndex.Click += new System.EventHandler(this.btnGetTopIndex_Click);
            // 
            // timerInsertStockIndex
            // 
            this.timerInsertStockIndex.Enabled = true;
            this.timerInsertStockIndex.Interval = 1000;
            this.timerInsertStockIndex.Tick += new System.EventHandler(this.timerInsertStockIndex_Tick);
            // 
            // btnUpdateStockIndex
            // 
            this.btnUpdateStockIndex.Location = new System.Drawing.Point(375, 12);
            this.btnUpdateStockIndex.Name = "btnUpdateStockIndex";
            this.btnUpdateStockIndex.Size = new System.Drawing.Size(101, 23);
            this.btnUpdateStockIndex.TabIndex = 6;
            this.btnUpdateStockIndex.Text = "updateStockIndex";
            this.btnUpdateStockIndex.UseVisualStyleBackColor = true;
            this.btnUpdateStockIndex.Click += new System.EventHandler(this.btnUpdateStockIndex_Click);
            // 
            // btnInsertStockInventory
            // 
            this.btnInsertStockInventory.Enabled = false;
            this.btnInsertStockInventory.Location = new System.Drawing.Point(474, 12);
            this.btnInsertStockInventory.Name = "btnInsertStockInventory";
            this.btnInsertStockInventory.Size = new System.Drawing.Size(113, 23);
            this.btnInsertStockInventory.TabIndex = 7;
            this.btnInsertStockInventory.Text = "insertStockInventory";
            this.btnInsertStockInventory.UseVisualStyleBackColor = true;
            this.btnInsertStockInventory.Click += new System.EventHandler(this.btnInsertStockInventory_Click);
            // 
            // btnGetStockPrice
            // 
            this.btnGetStockPrice.Location = new System.Drawing.Point(593, 12);
            this.btnGetStockPrice.Name = "btnGetStockPrice";
            this.btnGetStockPrice.Size = new System.Drawing.Size(108, 23);
            this.btnGetStockPrice.TabIndex = 8;
            this.btnGetStockPrice.Text = "InsertStockDayAll";
            this.btnGetStockPrice.UseVisualStyleBackColor = true;
            this.btnGetStockPrice.Click += new System.EventHandler(this.btnGetStockPrice_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 41);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 9;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(93, 41);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(375, 22);
            this.txtFile.TabIndex = 10;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(474, 41);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 11;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnInsertStockLackOff
            // 
            this.btnInsertStockLackOff.Location = new System.Drawing.Point(707, 12);
            this.btnInsertStockLackOff.Name = "btnInsertStockLackOff";
            this.btnInsertStockLackOff.Size = new System.Drawing.Size(108, 23);
            this.btnInsertStockLackOff.TabIndex = 12;
            this.btnInsertStockLackOff.Text = "InsertStockLackOff";
            this.btnInsertStockLackOff.UseVisualStyleBackColor = true;
            this.btnInsertStockLackOff.Click += new System.EventHandler(this.btnInsertStockLackOff_Click);
            // 
            // btnUpdateJuridicaPerson
            // 
            this.btnUpdateJuridicaPerson.Enabled = false;
            this.btnUpdateJuridicaPerson.Location = new System.Drawing.Point(821, 12);
            this.btnUpdateJuridicaPerson.Name = "btnUpdateJuridicaPerson";
            this.btnUpdateJuridicaPerson.Size = new System.Drawing.Size(114, 23);
            this.btnUpdateJuridicaPerson.TabIndex = 13;
            this.btnUpdateJuridicaPerson.Text = "updateJuridicaPerson";
            this.btnUpdateJuridicaPerson.UseVisualStyleBackColor = true;
            this.btnUpdateJuridicaPerson.Click += new System.EventHandler(this.btnUpdateJuridicaPerson_Click);
            // 
            // btnResetLinePoint
            // 
            this.btnResetLinePoint.Location = new System.Drawing.Point(941, 12);
            this.btnResetLinePoint.Name = "btnResetLinePoint";
            this.btnResetLinePoint.Size = new System.Drawing.Size(114, 23);
            this.btnResetLinePoint.TabIndex = 14;
            this.btnResetLinePoint.Text = "resetLinePoint";
            this.btnResetLinePoint.UseVisualStyleBackColor = true;
            this.btnResetLinePoint.Click += new System.EventHandler(this.btnResetLinePoint_Click);
            // 
            // btnGetGoodStock
            // 
            this.btnGetGoodStock.Location = new System.Drawing.Point(950, 12);
            this.btnGetGoodStock.Name = "btnGetGoodStock";
            this.btnGetGoodStock.Size = new System.Drawing.Size(114, 23);
            this.btnGetGoodStock.TabIndex = 15;
            this.btnGetGoodStock.Text = "getGoodStock";
            this.btnGetGoodStock.UseVisualStyleBackColor = true;
            this.btnGetGoodStock.Click += new System.EventHandler(this.btnGetGoodStock_Click);
            // 
            // txtGoodStock
            // 
            this.txtGoodStock.Location = new System.Drawing.Point(12, 121);
            this.txtGoodStock.Multiline = true;
            this.txtGoodStock.Name = "txtGoodStock";
            this.txtGoodStock.Size = new System.Drawing.Size(1532, 31);
            this.txtGoodStock.TabIndex = 16;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(818, 13);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(126, 22);
            this.dateTimePicker1.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtMultiple);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtVolumn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nupGain);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnLineGoodStock);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.btnGetGoodStock);
            this.groupBox1.Location = new System.Drawing.Point(10, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1163, 51);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Good";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.txtMoreGain);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.txtPreDays2);
            this.panel2.Location = new System.Drawing.Point(445, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(246, 34);
            this.panel2.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(223, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 12);
            this.label8.TabIndex = 32;
            this.label8.Text = "%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(90, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 30;
            this.label9.Text = "日不得高於現價";
            // 
            // txtMoreGain
            // 
            this.txtMoreGain.Location = new System.Drawing.Point(182, 5);
            this.txtMoreGain.Name = "txtMoreGain";
            this.txtMoreGain.Size = new System.Drawing.Size(35, 22);
            this.txtMoreGain.TabIndex = 31;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 28;
            this.label10.Text = "前";
            // 
            // txtPreDays2
            // 
            this.txtPreDays2.Location = new System.Drawing.Point(52, 5);
            this.txtPreDays2.Name = "txtPreDays2";
            this.txtPreDays2.Size = new System.Drawing.Size(35, 22);
            this.txtPreDays2.TabIndex = 29;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtLessVolumn);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtPreDays);
            this.panel1.Location = new System.Drawing.Point(243, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 34);
            this.panel1.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(167, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 32;
            this.label7.Text = "量";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(85, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 30;
            this.label6.Text = "日小於";
            // 
            // txtLessVolumn
            // 
            this.txtLessVolumn.Location = new System.Drawing.Point(126, 5);
            this.txtLessVolumn.Name = "txtLessVolumn";
            this.txtLessVolumn.Size = new System.Drawing.Size(35, 22);
            this.txtLessVolumn.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "前";
            // 
            // txtPreDays
            // 
            this.txtPreDays.Location = new System.Drawing.Point(47, 5);
            this.txtPreDays.Name = "txtPreDays";
            this.txtPreDays.Size = new System.Drawing.Size(35, 22);
            this.txtPreDays.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "倍量";
            // 
            // txtMultiple
            // 
            this.txtMultiple.Location = new System.Drawing.Point(175, 10);
            this.txtMultiple.Name = "txtMultiple";
            this.txtMultiple.Size = new System.Drawing.Size(35, 22);
            this.txtMultiple.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(156, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "張";
            // 
            // txtVolumn
            // 
            this.txtVolumn.Location = new System.Drawing.Point(115, 10);
            this.txtVolumn.Name = "txtVolumn";
            this.txtVolumn.Size = new System.Drawing.Size(35, 22);
            this.txtVolumn.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "%";
            // 
            // nupGain
            // 
            this.nupGain.Location = new System.Drawing.Point(62, 10);
            this.nupGain.Name = "nupGain";
            this.nupGain.Size = new System.Drawing.Size(31, 22);
            this.nupGain.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 22;
            this.label1.Text = "起漲放量";
            // 
            // btnLineGoodStock
            // 
            this.btnLineGoodStock.Location = new System.Drawing.Point(1068, 11);
            this.btnLineGoodStock.Name = "btnLineGoodStock";
            this.btnLineGoodStock.Size = new System.Drawing.Size(88, 23);
            this.btnLineGoodStock.TabIndex = 18;
            this.btnLineGoodStock.Text = "LineGoodstock";
            this.btnLineGoodStock.UseVisualStyleBackColor = true;
            this.btnLineGoodStock.Click += new System.EventHandler(this.btnLineGoodStock_Click);
            // 
            // btnUpdateStockGain
            // 
            this.btnUpdateStockGain.Enabled = false;
            this.btnUpdateStockGain.Location = new System.Drawing.Point(1061, 12);
            this.btnUpdateStockGain.Name = "btnUpdateStockGain";
            this.btnUpdateStockGain.Size = new System.Drawing.Size(114, 23);
            this.btnUpdateStockGain.TabIndex = 19;
            this.btnUpdateStockGain.Text = "updateStockGain";
            this.btnUpdateStockGain.UseVisualStyleBackColor = true;
            this.btnUpdateStockGain.Click += new System.EventHandler(this.btnUpdateStockGain_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(1016, 682);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(332, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 20;
            this.progressBar1.UseWaitCursor = true;
            // 
            // lbPercent
            // 
            this.lbPercent.AutoSize = true;
            this.lbPercent.Location = new System.Drawing.Point(1357, 687);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(0, 12);
            this.lbPercent.TabIndex = 21;
            // 
            // lbBtnName
            // 
            this.lbBtnName.AutoSize = true;
            this.lbBtnName.ForeColor = System.Drawing.Color.Black;
            this.lbBtnName.Location = new System.Drawing.Point(801, 687);
            this.lbBtnName.Name = "lbBtnName";
            this.lbBtnName.Size = new System.Drawing.Size(0, 12);
            this.lbBtnName.TabIndex = 34;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(555, 40);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 35;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // ScheduleJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1556, 732);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lbBtnName);
            this.Controls.Add(this.lbPercent);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnUpdateStockGain);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtGoodStock);
            this.Controls.Add(this.btnResetLinePoint);
            this.Controls.Add(this.btnUpdateJuridicaPerson);
            this.Controls.Add(this.btnInsertStockLackOff);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnGetStockPrice);
            this.Controls.Add(this.btnInsertStockInventory);
            this.Controls.Add(this.btnUpdateStockIndex);
            this.Controls.Add(this.btnGetTopIndex);
            this.Controls.Add(this.btnGetIndexToInsert);
            this.Controls.Add(this.txtErrMsg);
            this.Controls.Add(this.btnGetStock);
            this.Controls.Add(this.btnLogin);
            this.Name = "ScheduleJob";
            this.Text = "ScheduleJob";
            this.Load += new System.EventHandler(this.ScheduleJob_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupGain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnGetStock;
        private System.Windows.Forms.TextBox txtErrMsg;
        private System.Windows.Forms.Button btnGetIndexToInsert;
        private System.Windows.Forms.Button btnGetTopIndex;
        private System.Windows.Forms.Timer timerInsertStockIndex;
        private System.Windows.Forms.Button btnUpdateStockIndex;
        private System.Windows.Forms.Button btnInsertStockInventory;
        private System.Windows.Forms.Button btnGetStockPrice;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnInsertStockLackOff;
        private System.Windows.Forms.Button btnUpdateJuridicaPerson;
        private System.Windows.Forms.Button btnResetLinePoint;
        private System.Windows.Forms.Button btnGetGoodStock;
        private System.Windows.Forms.TextBox txtGoodStock;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUpdateStockGain;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lbPercent;
        private System.Windows.Forms.Button btnLineGoodStock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nupGain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVolumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMultiple;
        private System.Windows.Forms.TextBox txtLessVolumn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPreDays;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMoreGain;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPreDays2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbBtnName;
        private System.Windows.Forms.Button btnClear;
    }
}