namespace StockStrategy
{
	partial class FormMaintain
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
			this.dtpStart = new System.Windows.Forms.DateTimePicker();
			this.dtpEnd = new System.Windows.Forms.DateTimePicker();
			this.btnAddPicking = new System.Windows.Forms.Button();
			this.txtErrMsg = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// dtpStart
			// 
			this.dtpStart.Location = new System.Drawing.Point(12, 12);
			this.dtpStart.Name = "dtpStart";
			this.dtpStart.Size = new System.Drawing.Size(105, 22);
			this.dtpStart.TabIndex = 0;
			this.dtpStart.Value = new System.DateTime(2023, 4, 17, 0, 0, 0, 0);
			// 
			// dtpEnd
			// 
			this.dtpEnd.Location = new System.Drawing.Point(123, 12);
			this.dtpEnd.Name = "dtpEnd";
			this.dtpEnd.Size = new System.Drawing.Size(105, 22);
			this.dtpEnd.TabIndex = 1;
			this.dtpEnd.Value = new System.DateTime(2023, 4, 18, 0, 0, 0, 0);
			// 
			// btnAddPicking
			// 
			this.btnAddPicking.Location = new System.Drawing.Point(234, 11);
			this.btnAddPicking.Name = "btnAddPicking";
			this.btnAddPicking.Size = new System.Drawing.Size(75, 23);
			this.btnAddPicking.TabIndex = 2;
			this.btnAddPicking.Text = "Add";
			this.btnAddPicking.UseVisualStyleBackColor = true;
			this.btnAddPicking.Click += new System.EventHandler(this.btnAddPicking_Click);
			// 
			// txtErrMsg
			// 
			this.txtErrMsg.Location = new System.Drawing.Point(799, 11);
			this.txtErrMsg.Multiline = true;
			this.txtErrMsg.Name = "txtErrMsg";
			this.txtErrMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtErrMsg.Size = new System.Drawing.Size(363, 120);
			this.txtErrMsg.TabIndex = 4;
			// 
			// FormMaintain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1174, 796);
			this.Controls.Add(this.txtErrMsg);
			this.Controls.Add(this.btnAddPicking);
			this.Controls.Add(this.dtpEnd);
			this.Controls.Add(this.dtpStart);
			this.Name = "FormMaintain";
			this.Text = "FormMaintain";
			this.Load += new System.EventHandler(this.FormMaintain_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DateTimePicker dtpStart;
		private System.Windows.Forms.DateTimePicker dtpEnd;
		private System.Windows.Forms.Button btnAddPicking;
		private System.Windows.Forms.TextBox txtErrMsg;
	}
}