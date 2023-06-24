namespace StockStrategy
{
	partial class FormHighLow
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
			this.dgvHighLow = new System.Windows.Forms.DataGridView();
			this.btnQuery = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.lbMsg = new System.Windows.Forms.Label();
			this.lbPercent = new System.Windows.Forms.Label();
			this.btnGetData = new System.Windows.Forms.Button();
			this.btnExport = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgvHighLow)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvHighLow
			// 
			this.dgvHighLow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvHighLow.Location = new System.Drawing.Point(12, 41);
			this.dgvHighLow.Name = "dgvHighLow";
			this.dgvHighLow.RowTemplate.Height = 24;
			this.dgvHighLow.Size = new System.Drawing.Size(970, 743);
			this.dgvHighLow.TabIndex = 0;
			// 
			// btnQuery
			// 
			this.btnQuery.Location = new System.Drawing.Point(12, 12);
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Size = new System.Drawing.Size(75, 23);
			this.btnQuery.TabIndex = 1;
			this.btnQuery.Text = "Insert";
			this.btnQuery.UseVisualStyleBackColor = true;
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(412, 10);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(332, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 21;
			this.progressBar1.UseWaitCursor = true;
			// 
			// lbMsg
			// 
			this.lbMsg.AutoSize = true;
			this.lbMsg.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
			this.lbMsg.Location = new System.Drawing.Point(93, 17);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new System.Drawing.Size(0, 12);
			this.lbMsg.TabIndex = 22;
			// 
			// lbPercent
			// 
			this.lbPercent.AutoSize = true;
			this.lbPercent.Location = new System.Drawing.Point(750, 14);
			this.lbPercent.Name = "lbPercent";
			this.lbPercent.Size = new System.Drawing.Size(0, 12);
			this.lbPercent.TabIndex = 23;
			// 
			// btnGetData
			// 
			this.btnGetData.Location = new System.Drawing.Point(99, 12);
			this.btnGetData.Name = "btnGetData";
			this.btnGetData.Size = new System.Drawing.Size(75, 23);
			this.btnGetData.TabIndex = 24;
			this.btnGetData.Text = "Get Data";
			this.btnGetData.UseVisualStyleBackColor = true;
			this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
			// 
			// btnExport
			// 
			this.btnExport.Location = new System.Drawing.Point(184, 12);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(75, 23);
			this.btnExport.TabIndex = 25;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// FormHighLow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(994, 796);
			this.Controls.Add(this.btnExport);
			this.Controls.Add(this.btnGetData);
			this.Controls.Add(this.lbPercent);
			this.Controls.Add(this.lbMsg);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.btnQuery);
			this.Controls.Add(this.dgvHighLow);
			this.Name = "FormHighLow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormHighLow";
			((System.ComponentModel.ISupportInitialize)(this.dgvHighLow)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvHighLow;
		private System.Windows.Forms.Button btnQuery;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label lbMsg;
		private System.Windows.Forms.Label lbPercent;
		private System.Windows.Forms.Button btnGetData;
		private System.Windows.Forms.Button btnExport;
	}
}