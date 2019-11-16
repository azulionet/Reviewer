namespace Reviewer
{
	partial class ReviewDateForm
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
			this.button1 = new System.Windows.Forms.Button();
			this.m_uiFixedDateText = new System.Windows.Forms.TextBox();
			this.m_uiAfterDateText = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(204, 308);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(208, 39);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// m_uiFixedDateText
			// 
			this.m_uiFixedDateText.Location = new System.Drawing.Point(43, 61);
			this.m_uiFixedDateText.Name = "m_uiFixedDateText";
			this.m_uiFixedDateText.Size = new System.Drawing.Size(539, 28);
			this.m_uiFixedDateText.TabIndex = 1;
			this.m_uiFixedDateText.Text = " ";
			// 
			// m_uiAfterDateText
			// 
			this.m_uiAfterDateText.Location = new System.Drawing.Point(43, 165);
			this.m_uiAfterDateText.Name = "m_uiAfterDateText";
			this.m_uiAfterDateText.Size = new System.Drawing.Size(539, 28);
			this.m_uiAfterDateText.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(51, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(212, 18);
			this.label1.TabIndex = 3;
			this.label1.Text = "고정 날짜 ( 중복 불가능 )";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(51, 144);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(334, 18);
			this.label2.TabIndex = 4;
			this.label2.Text = "~일 후, 순서를 따집니다. 유의해주세요.";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(51, 263);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(365, 18);
			this.label3.TabIndex = 5;
			this.label3.Text = "텍스트는 1,2,3 <- 이 형태로 사용해주세요. ";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(51, 232);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(278, 18);
			this.label5.TabIndex = 7;
			this.label5.Text = "공백, 숫자, 콤마만 입력해주세요.";
			// 
			// ReviewDateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(625, 359);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_uiAfterDateText);
			this.Controls.Add(this.m_uiFixedDateText);
			this.Controls.Add(this.button1);
			this.Name = "ReviewDateForm";
			this.Text = "복습 주기를 써주세요";
			this.Load += new System.EventHandler(this.OnForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox m_uiFixedDateText;
		private System.Windows.Forms.TextBox m_uiAfterDateText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
	}
}