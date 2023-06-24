namespace StockStrategy
{
	partial class FormThreeInstitutional
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
			this.dtpDate = new System.Windows.Forms.DateTimePicker();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnGetData = new System.Windows.Forms.Button();
			this.lbPercent = new System.Windows.Forms.Label();
			this.lbMsg = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.dgvThreeInstitutional = new System.Windows.Forms.DataGridView();
			this.btnQuery = new System.Windows.Forms.Button();
			this.cbType = new System.Windows.Forms.ComboBox();
			this.cbInstitutional = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.dgvThreeInstitutional)).BeginInit();
			this.SuspendLayout();
			// 
			// dtpDate
			// 
			this.dtpDate.Location = new System.Drawing.Point(18, 10);
			this.dtpDate.Name = "dtpDate";
			this.dtpDate.Size = new System.Drawing.Size(102, 22);
			this.dtpDate.TabIndex = 42;
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(435, 10);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 41;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			// 
			// btnGetData
			// 
			this.btnGetData.Location = new System.Drawing.Point(350, 10);
			this.btnGetData.Name = "btnGetData";
			this.btnGetData.Size = new System.Drawing.Size(75, 23);
			this.btnGetData.TabIndex = 40;
			this.btnGetData.Text = "Get Data";
			this.btnGetData.UseVisualStyleBackColor = true;
			this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
			// 
			// lbPercent
			// 
			this.lbPercent.AutoSize = true;
			this.lbPercent.Location = new System.Drawing.Point(756, 17);
			this.lbPercent.Name = "lbPercent";
			this.lbPercent.Size = new System.Drawing.Size(0, 12);
			this.lbPercent.TabIndex = 39;
			// 
			// lbMsg
			// 
			this.lbMsg.AutoSize = true;
			this.lbMsg.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.lbMsg.Location = new System.Drawing.Point(99, 20);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new System.Drawing.Size(0, 12);
			this.lbMsg.TabIndex = 38;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(824, 10);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(332, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 37;
			this.progressBar1.UseWaitCursor = true;
			// 
			// dgvThreeInstitutional
			// 
			this.dgvThreeInstitutional.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvThreeInstitutional.Location = new System.Drawing.Point(18, 44);
			this.dgvThreeInstitutional.Name = "dgvThreeInstitutional";
			this.dgvThreeInstitutional.RowTemplate.Height = 24;
			this.dgvThreeInstitutional.Size = new System.Drawing.Size(1138, 743);
			this.dgvThreeInstitutional.TabIndex = 35;
			// 
			// btnQuery
			// 
			this.btnQuery.Location = new System.Drawing.Point(263, 10);
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Size = new System.Drawing.Size(75, 23);
			this.btnQuery.TabIndex = 36;
			this.btnQuery.Text = "Insert";
			this.btnQuery.UseVisualStyleBackColor = true;
			// 
			// cbType
			// 
			this.cbType.FormattingEnabled = true;
			this.cbType.Items.AddRange(new object[] {
            "買超",
            "賣超"});
			this.cbType.Location = new System.Drawing.Point(126, 10);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(58, 20);
			this.cbType.TabIndex = 43;
			// 
			// cbInstitutional
			// 
			this.cbInstitutional.FormattingEnabled = true;
			this.cbInstitutional.Items.AddRange(new object[] {
            "外資",
            "投信",
            "自營商"});
			this.cbInstitutional.Location = new System.Drawing.Point(190, 10);
			this.cbInstitutional.Name = "cbInstitutional";
			this.cbInstitutional.Size = new System.Drawing.Size(58, 20);
			this.cbInstitutional.TabIndex = 44;
			// 
			// FormThreeInstitutional
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1174, 796);
			this.Controls.Add(this.cbInstitutional);
			this.Controls.Add(this.cbType);
			this.Controls.Add(this.dtpDate);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnGetData);
			this.Controls.Add(this.lbPercent);
			this.Controls.Add(this.lbMsg);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.btnQuery);
			this.Controls.Add(this.dgvThreeInstitutional);
			this.Name = "FormThreeInstitutional";
			this.Text = "FormThreeInstitutional";
			this.Load += new System.EventHandler(this.FormThreeInstitutional_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvThreeInstitutional)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DateTimePicker dtpDate;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.Button btnGetData;
		private System.Windows.Forms.Label lbPercent;
		private System.Windows.Forms.Label lbMsg;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.DataGridView dgvThreeInstitutional;
		private System.Windows.Forms.Button btnQuery;
		private System.Windows.Forms.ComboBox cbType;
		private System.Windows.Forms.ComboBox cbInstitutional;
	}
}