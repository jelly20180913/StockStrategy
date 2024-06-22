namespace StockStrategy
{
	partial class FormRobinForcast
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
			this.btnGetData = new System.Windows.Forms.Button();
			this.dgvRoginForcast = new System.Windows.Forms.DataGridView();
			this.label32 = new System.Windows.Forms.Label();
			this.dTPReport = new System.Windows.Forms.DateTimePicker();
			this.label33 = new System.Windows.Forms.Label();
			this.dtpReportStart = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.lbForcastRight = new System.Windows.Forms.Label();
			this.lbForcastWrong = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lbProfit = new System.Windows.Forms.Label();
			this.lbForcastRate = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgvRoginForcast)).BeginInit();
			this.SuspendLayout();
			// 
			// btnGetData
			// 
			this.btnGetData.Location = new System.Drawing.Point(264, 8);
			this.btnGetData.Name = "btnGetData";
			this.btnGetData.Size = new System.Drawing.Size(75, 23);
			this.btnGetData.TabIndex = 26;
			this.btnGetData.Text = "Get Data";
			this.btnGetData.UseVisualStyleBackColor = true;
			this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
			// 
			// dgvRoginForcast
			// 
			this.dgvRoginForcast.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvRoginForcast.Location = new System.Drawing.Point(6, 47);
			this.dgvRoginForcast.Name = "dgvRoginForcast";
			this.dgvRoginForcast.RowTemplate.Height = 24;
			this.dgvRoginForcast.Size = new System.Drawing.Size(642, 743);
			this.dgvRoginForcast.TabIndex = 25;
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Location = new System.Drawing.Point(4, 13);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(17, 12);
			this.label32.TabIndex = 75;
			this.label32.Text = "起";
			// 
			// dTPReport
			// 
			this.dTPReport.Location = new System.Drawing.Point(148, 9);
			this.dTPReport.Name = "dTPReport";
			this.dTPReport.Size = new System.Drawing.Size(107, 22);
			this.dTPReport.TabIndex = 73;
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Location = new System.Drawing.Point(127, 13);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(17, 12);
			this.label33.TabIndex = 76;
			this.label33.Text = "迄";
			// 
			// dtpReportStart
			// 
			this.dtpReportStart.Location = new System.Drawing.Point(21, 8);
			this.dtpReportStart.Name = "dtpReportStart";
			this.dtpReportStart.Size = new System.Drawing.Size(102, 22);
			this.dtpReportStart.TabIndex = 74;
			this.dtpReportStart.Value = new System.DateTime(2024, 6, 1, 0, 0, 0, 0);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(356, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(20, 12);
			this.label1.TabIndex = 77;
			this.label1.Text = "準:";
			// 
			// lbForcastRight
			// 
			this.lbForcastRight.AutoSize = true;
			this.lbForcastRight.Location = new System.Drawing.Point(382, 13);
			this.lbForcastRight.Name = "lbForcastRight";
			this.lbForcastRight.Size = new System.Drawing.Size(17, 12);
			this.lbForcastRight.TabIndex = 78;
			this.lbForcastRight.Text = "筆";
			// 
			// lbForcastWrong
			// 
			this.lbForcastWrong.AutoSize = true;
			this.lbForcastWrong.Location = new System.Drawing.Point(452, 13);
			this.lbForcastWrong.Name = "lbForcastWrong";
			this.lbForcastWrong.Size = new System.Drawing.Size(17, 12);
			this.lbForcastWrong.TabIndex = 80;
			this.lbForcastWrong.Text = "筆";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(418, 13);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 12);
			this.label3.TabIndex = 79;
			this.label3.Text = "不準:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(492, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 12);
			this.label2.TabIndex = 81;
			this.label2.Text = "賺幾點:";
			// 
			// lbProfit
			// 
			this.lbProfit.AutoSize = true;
			this.lbProfit.Location = new System.Drawing.Point(542, 14);
			this.lbProfit.Name = "lbProfit";
			this.lbProfit.Size = new System.Drawing.Size(44, 12);
			this.lbProfit.TabIndex = 82;
			this.lbProfit.Text = "賺幾點:";
			// 
			// lbForcastRate
			// 
			this.lbForcastRate.AutoSize = true;
			this.lbForcastRate.Location = new System.Drawing.Point(604, 15);
			this.lbForcastRate.Name = "lbForcastRate";
			this.lbForcastRate.Size = new System.Drawing.Size(44, 12);
			this.lbForcastRate.TabIndex = 83;
			this.lbForcastRate.Text = "準確率:";
			// 
			// FormRobinForcast
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(653, 798);
			this.Controls.Add(this.lbForcastRate);
			this.Controls.Add(this.lbProfit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbForcastWrong);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lbForcastRight);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label32);
			this.Controls.Add(this.dTPReport);
			this.Controls.Add(this.label33);
			this.Controls.Add(this.dtpReportStart);
			this.Controls.Add(this.btnGetData);
			this.Controls.Add(this.dgvRoginForcast);
			this.Name = "FormRobinForcast";
			this.Text = "FormRobinForcast";
			((System.ComponentModel.ISupportInitialize)(this.dgvRoginForcast)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnGetData;
		private System.Windows.Forms.DataGridView dgvRoginForcast;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.DateTimePicker dTPReport;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.DateTimePicker dtpReportStart;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbForcastRight;
		private System.Windows.Forms.Label lbForcastWrong;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbProfit;
		private System.Windows.Forms.Label lbForcastRate;
	}
}