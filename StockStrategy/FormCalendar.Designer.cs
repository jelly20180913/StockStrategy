namespace StockStrategy
{
	partial class FormCalendar
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
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.dtpStart = new System.Windows.Forms.DateTimePicker();
			this.dtpEnd = new System.Windows.Forms.DateTimePicker();
			this.btnQuery = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "一月",
            "二月",
            "三月",
            "四月",
            "五月",
            "六月",
            "七月",
            "八月",
            "九月",
            "十月",
            "十一月",
            "十二月"});
			this.comboBox1.Location = new System.Drawing.Point(12, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 20);
			this.comboBox1.TabIndex = 0;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 102);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(534, 677);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// dtpStart
			// 
			this.dtpStart.Location = new System.Drawing.Point(149, 10);
			this.dtpStart.Name = "dtpStart";
			this.dtpStart.Size = new System.Drawing.Size(121, 22);
			this.dtpStart.TabIndex = 2;
			// 
			// dtpEnd
			// 
			this.dtpEnd.Location = new System.Drawing.Point(276, 10);
			this.dtpEnd.Name = "dtpEnd";
			this.dtpEnd.Size = new System.Drawing.Size(121, 22);
			this.dtpEnd.TabIndex = 3;
			// 
			// btnQuery
			// 
			this.btnQuery.Location = new System.Drawing.Point(404, 10);
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Size = new System.Drawing.Size(75, 23);
			this.btnQuery.TabIndex = 4;
			this.btnQuery.Text = "查詢";
			this.btnQuery.UseVisualStyleBackColor = true;
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.textBox1.Enabled = false;
			this.textBox1.Font = new System.Drawing.Font("新細明體", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textBox1.Location = new System.Drawing.Point(12, 46);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 45);
			this.textBox1.TabIndex = 5;
			this.textBox1.Text = "星期一";
			this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textBox2
			// 
			this.textBox2.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.textBox2.Enabled = false;
			this.textBox2.Font = new System.Drawing.Font("新細明體", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textBox2.Location = new System.Drawing.Point(118, 46);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(100, 45);
			this.textBox2.TabIndex = 6;
			this.textBox2.Text = "星期二";
			this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textBox3
			// 
			this.textBox3.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.textBox3.Enabled = false;
			this.textBox3.Font = new System.Drawing.Font("新細明體", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textBox3.Location = new System.Drawing.Point(224, 46);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(100, 45);
			this.textBox3.TabIndex = 7;
			this.textBox3.Text = "星期三";
			this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textBox4
			// 
			this.textBox4.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.textBox4.Enabled = false;
			this.textBox4.Font = new System.Drawing.Font("新細明體", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textBox4.Location = new System.Drawing.Point(330, 46);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(100, 45);
			this.textBox4.TabIndex = 8;
			this.textBox4.Text = "星期四";
			this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textBox5
			// 
			this.textBox5.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.textBox5.Enabled = false;
			this.textBox5.Font = new System.Drawing.Font("新細明體", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.textBox5.Location = new System.Drawing.Point(436, 46);
			this.textBox5.Multiline = true;
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(100, 45);
			this.textBox5.TabIndex = 9;
			this.textBox5.Text = "星期五";
			this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// FormCalendar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(551, 791);
			this.Controls.Add(this.textBox5);
			this.Controls.Add(this.textBox4);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.btnQuery);
			this.Controls.Add(this.dtpEnd);
			this.Controls.Add(this.dtpStart);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.comboBox1);
			this.Name = "FormCalendar";
			this.Text = "FormCalendar";
			this.Load += new System.EventHandler(this.FormCalendar_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.DateTimePicker dtpStart;
		private System.Windows.Forms.DateTimePicker dtpEnd;
		private System.Windows.Forms.Button btnQuery;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox5;
	}
}