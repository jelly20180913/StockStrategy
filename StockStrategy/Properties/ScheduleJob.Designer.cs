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
            this.txtErrMsg.Location = new System.Drawing.Point(12, 97);
            this.txtErrMsg.Multiline = true;
            this.txtErrMsg.Name = "txtErrMsg";
            this.txtErrMsg.Size = new System.Drawing.Size(1240, 234);
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
            this.btnUpdateJuridicaPerson.Location = new System.Drawing.Point(821, 12);
            this.btnUpdateJuridicaPerson.Name = "btnUpdateJuridicaPerson";
            this.btnUpdateJuridicaPerson.Size = new System.Drawing.Size(114, 23);
            this.btnUpdateJuridicaPerson.TabIndex = 13;
            this.btnUpdateJuridicaPerson.Text = "updateJuridicaPerson";
            this.btnUpdateJuridicaPerson.UseVisualStyleBackColor = true;
            this.btnUpdateJuridicaPerson.Click += new System.EventHandler(this.btnUpdateJuridicaPerson_Click);
            // 
            // ScheduleJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 732);
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
    }
}