namespace StockStrategy
{
	partial class FormUpdateStockIndex
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtOpen = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtHigh = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtLow = new System.Windows.Forms.TextBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.txtBondIndex = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.dtpStockIndex = new System.Windows.Forms.DateTimePicker();
			this.label6 = new System.Windows.Forms.Label();
			this.txtBondIndexPercent = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(31, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(29, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "日期";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(31, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "開盤價";
			// 
			// txtOpen
			// 
			this.txtOpen.Location = new System.Drawing.Point(129, 61);
			this.txtOpen.Name = "txtOpen";
			this.txtOpen.Size = new System.Drawing.Size(100, 22);
			this.txtOpen.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(31, 105);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "最高價";
			// 
			// txtHigh
			// 
			this.txtHigh.Location = new System.Drawing.Point(129, 98);
			this.txtHigh.Name = "txtHigh";
			this.txtHigh.Size = new System.Drawing.Size(100, 22);
			this.txtHigh.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(31, 145);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(41, 12);
			this.label4.TabIndex = 7;
			this.label4.Text = "最低價";
			// 
			// txtLow
			// 
			this.txtLow.Location = new System.Drawing.Point(129, 138);
			this.txtLow.Name = "txtLow";
			this.txtLow.Size = new System.Drawing.Size(100, 22);
			this.txtLow.TabIndex = 6;
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(90, 247);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(75, 23);
			this.btnUpdate.TabIndex = 8;
			this.btnUpdate.Text = "更新";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// txtBondIndex
			// 
			this.txtBondIndex.Location = new System.Drawing.Point(129, 170);
			this.txtBondIndex.Name = "txtBondIndex";
			this.txtBondIndex.Size = new System.Drawing.Size(100, 22);
			this.txtBondIndex.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(30, 177);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(89, 12);
			this.label5.TabIndex = 10;
			this.label5.Text = "十年公債殖利率";
			// 
			// dtpStockIndex
			// 
			this.dtpStockIndex.Location = new System.Drawing.Point(103, 25);
			this.dtpStockIndex.Name = "dtpStockIndex";
			this.dtpStockIndex.Size = new System.Drawing.Size(126, 22);
			this.dtpStockIndex.TabIndex = 92;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(31, 211);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(14, 12);
			this.label6.TabIndex = 94;
			this.label6.Text = "%";
			// 
			// txtBondIndexPercent
			// 
			this.txtBondIndexPercent.Location = new System.Drawing.Point(130, 204);
			this.txtBondIndexPercent.Name = "txtBondIndexPercent";
			this.txtBondIndexPercent.Size = new System.Drawing.Size(100, 22);
			this.txtBondIndexPercent.TabIndex = 93;
			// 
			// FormUpdateStockIndex
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(253, 282);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtBondIndexPercent);
			this.Controls.Add(this.dtpStockIndex);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtBondIndex);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtLow);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtHigh);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtOpen);
			this.Controls.Add(this.label1);
			this.Name = "FormUpdateStockIndex";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "指數更新";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtOpen;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtHigh;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtLow;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.TextBox txtBondIndex;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DateTimePicker dtpStockIndex;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtBondIndexPercent;
	}
}