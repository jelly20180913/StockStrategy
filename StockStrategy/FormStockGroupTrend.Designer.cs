namespace StockStrategy
{
	partial class FormStockGroupTrend
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
			this.btnExport = new System.Windows.Forms.Button();
			this.btnGetData = new System.Windows.Forms.Button();
			this.lbPercent = new System.Windows.Forms.Label();
			this.lbMsg = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.btnQuery = new System.Windows.Forms.Button();
			this.dgvGroupTrend = new System.Windows.Forms.DataGridView();
			this.dtpDate = new System.Windows.Forms.DateTimePicker();
			this.cbType = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.dgvGroupTrend)).BeginInit();
			this.SuspendLayout();
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(356, 8);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 32;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnGetData
			// 
			this.btnGetData.Location = new System.Drawing.Point(271, 8);
			this.btnGetData.Name = "btnGetData";
			this.btnGetData.Size = new System.Drawing.Size(75, 23);
			this.btnGetData.TabIndex = 31;
			this.btnGetData.Text = "Get Data";
			this.btnGetData.UseVisualStyleBackColor = true;
			this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
			// 
			// lbPercent
			// 
			this.lbPercent.AutoSize = true;
			this.lbPercent.Location = new System.Drawing.Point(750, 15);
			this.lbPercent.Name = "lbPercent";
			this.lbPercent.Size = new System.Drawing.Size(0, 12);
			this.lbPercent.TabIndex = 30;
			// 
			// lbMsg
			// 
			this.lbMsg.AutoSize = true;
			this.lbMsg.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.lbMsg.Location = new System.Drawing.Point(93, 18);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new System.Drawing.Size(0, 12);
			this.lbMsg.TabIndex = 29;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(818, 8);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(332, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 28;
			this.progressBar1.UseWaitCursor = true;
			// 
			// btnQuery
			// 
			this.btnQuery.Location = new System.Drawing.Point(184, 8);
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Size = new System.Drawing.Size(75, 23);
			this.btnQuery.TabIndex = 27;
			this.btnQuery.Text = "Insert";
			this.btnQuery.UseVisualStyleBackColor = true;
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			// 
			// dgvGroupTrend
			// 
			this.dgvGroupTrend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvGroupTrend.Location = new System.Drawing.Point(12, 42);
			this.dgvGroupTrend.Name = "dgvGroupTrend";
			this.dgvGroupTrend.RowTemplate.Height = 24;
			this.dgvGroupTrend.Size = new System.Drawing.Size(1138, 743);
			this.dgvGroupTrend.TabIndex = 26;
			// 
			// dtpDate
			// 
			this.dtpDate.Location = new System.Drawing.Point(12, 8);
			this.dtpDate.Name = "dtpDate";
			this.dtpDate.Size = new System.Drawing.Size(102, 22);
			this.dtpDate.TabIndex = 33;
			// 
			// cbType
			// 
			this.cbType.FormattingEnabled = true;
			this.cbType.Items.AddRange(new object[] {
            "強勢",
            "弱勢"});
			this.cbType.Location = new System.Drawing.Point(120, 8);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(58, 20);
			this.cbType.TabIndex = 34;
			// 
			// FormStockGroupTrend
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1174, 796);
			this.Controls.Add(this.cbType);
			this.Controls.Add(this.dtpDate);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnGetData);
			this.Controls.Add(this.lbPercent);
			this.Controls.Add(this.lbMsg);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.btnQuery);
			this.Controls.Add(this.dgvGroupTrend);
			this.Name = "FormStockGroupTrend";
			this.Text = "FormStockGroupTrend";
			this.Load += new System.EventHandler(this.FormStockGroupTrend_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvGroupTrend)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnGetData;
		private System.Windows.Forms.Label lbPercent;
		private System.Windows.Forms.Label lbMsg;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button btnQuery;
		private System.Windows.Forms.DataGridView dgvGroupTrend;
		private System.Windows.Forms.DateTimePicker dtpDate;
		private System.Windows.Forms.ComboBox cbType;
	}
}