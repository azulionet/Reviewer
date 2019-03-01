namespace Reviewer
{
	partial class HelpForm
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
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.DisabledLinkColor = System.Drawing.Color.Red;
			this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel1.LinkColor = System.Drawing.Color.Navy;
			this.linkLabel1.Location = new System.Drawing.Point(39, 88);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(159, 18);
			this.linkLabel1.TabIndex = 0;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "유튜브 URL : 클릭!";
			this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.On_URLLinkClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(39, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(236, 18);
			this.label1.TabIndex = 1;
			this.label1.Text = "이 유튜브를 먼저 봐주세요!!";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(39, 139);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(422, 18);
			this.label2.TabIndex = 2;
			this.label2.Text = "위 유튜브에서 사용하는 방식을 도와주는 툴입니다.";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(39, 253);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(398, 18);
			this.label3.TabIndex = 3;
			this.label3.Text = "자세한 사용방법은 아래 유튜브를 참고해주세요.";
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.DisabledLinkColor = System.Drawing.Color.Red;
			this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabel2.LinkColor = System.Drawing.Color.Navy;
			this.linkLabel2.Location = new System.Drawing.Point(39, 314);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(225, 18);
			this.linkLabel2.TabIndex = 4;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "툴 설명 유튜브 URL : 클릭!";
			this.linkLabel2.VisitedLinkColor = System.Drawing.Color.Blue;
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.On_URLLinkClicked);
			// 
			// HelpForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(478, 380);
			this.Controls.Add(this.linkLabel2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.linkLabel1);
			this.Name = "HelpForm";
			this.Text = "HelpForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel linkLabel2;
	}
}