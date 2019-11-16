namespace Reviewer
{
	partial class ReviewerForm
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_uiReviewList = new System.Windows.Forms.CheckedListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.m_uiTextFolder = new System.Windows.Forms.TextBox();
			this.m_uiTextState = new System.Windows.Forms.Label();
			this.m_uiReview = new System.Windows.Forms.Button();
			this.menuStrip2 = new System.Windows.Forms.MenuStrip();
			this.m_menuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuShortcut = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuShortcut_ReviewFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuShortcut_ExeFile = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuEtc = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuEtc_DateSetting = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuEtc_OpenStudyFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.m_menuEtc_Help = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_uiReviewList
			// 
			this.m_uiReviewList.FormattingEnabled = true;
			this.m_uiReviewList.Location = new System.Drawing.Point(18, 87);
			this.m_uiReviewList.Margin = new System.Windows.Forms.Padding(4);
			this.m_uiReviewList.Name = "m_uiReviewList";
			this.m_uiReviewList.Size = new System.Drawing.Size(287, 533);
			this.m_uiReviewList.TabIndex = 2;
			this.m_uiReviewList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnReviewList_ItemCheck);
			this.m_uiReviewList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(337, 465);
			this.button1.Margin = new System.Windows.Forms.Padding(4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(196, 155);
			this.button1.TabIndex = 6;
			this.button1.TabStop = false;
			this.button1.Text = "복습 완료 ( 파일이 이동합니다.  )";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnReviewCompleteButton_Click);
			this.button1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(12, 36);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(137, 35);
			this.button3.TabIndex = 0;
			this.button3.TabStop = false;
			this.button3.Text = "폴더 설정";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.OnFolderSettingButton_Click);
			this.button3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
			// 
			// m_uiTextFolder
			// 
			this.m_uiTextFolder.Enabled = false;
			this.m_uiTextFolder.Location = new System.Drawing.Point(166, 43);
			this.m_uiTextFolder.Name = "m_uiTextFolder";
			this.m_uiTextFolder.Size = new System.Drawing.Size(369, 28);
			this.m_uiTextFolder.TabIndex = 1;
			this.m_uiTextFolder.TabStop = false;
			// 
			// m_uiTextState
			// 
			this.m_uiTextState.AutoSize = true;
			this.m_uiTextState.Location = new System.Drawing.Point(22, 631);
			this.m_uiTextState.Name = "m_uiTextState";
			this.m_uiTextState.Size = new System.Drawing.Size(35, 18);
			this.m_uiTextState.TabIndex = 9;
			this.m_uiTextState.Text = "---";
			// 
			// m_uiReview
			// 
			this.m_uiReview.Location = new System.Drawing.Point(339, 112);
			this.m_uiReview.Name = "m_uiReview";
			this.m_uiReview.Size = new System.Drawing.Size(196, 298);
			this.m_uiReview.TabIndex = 5;
			this.m_uiReview.TabStop = false;
			this.m_uiReview.Text = "복습";
			this.m_uiReview.UseVisualStyleBackColor = true;
			this.m_uiReview.Click += new System.EventHandler(this.OnReviewButton_Click);
			this.m_uiReview.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
			// 
			// menuStrip2
			// 
			this.menuStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuFile,
            this.m_menuShortcut,
            this.m_menuEtc});
			this.menuStrip2.Location = new System.Drawing.Point(0, 0);
			this.menuStrip2.Name = "menuStrip2";
			this.menuStrip2.Size = new System.Drawing.Size(551, 33);
			this.menuStrip2.TabIndex = 8;
			this.menuStrip2.Text = "menuStrip2";
			// 
			// m_menuFile
			// 
			this.m_menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuFile_Exit});
			this.m_menuFile.Name = "m_menuFile";
			this.m_menuFile.Size = new System.Drawing.Size(51, 29);
			this.m_menuFile.Text = "File";
			// 
			// m_menuFile_Exit
			// 
			this.m_menuFile_Exit.Name = "m_menuFile_Exit";
			this.m_menuFile_Exit.Size = new System.Drawing.Size(150, 30);
			this.m_menuFile_Exit.Text = "끝내기";
			this.m_menuFile_Exit.Click += new System.EventHandler(this.OnMenu_File_Exit_Click);
			// 
			// m_menuShortcut
			// 
			this.m_menuShortcut.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuShortcut_ReviewFolder,
            this.m_menuShortcut_ExeFile});
			this.m_menuShortcut.Name = "m_menuShortcut";
			this.m_menuShortcut.Size = new System.Drawing.Size(156, 29);
			this.m_menuShortcut.Text = "바로가기 만들기";
			// 
			// m_menuShortcut_ReviewFolder
			// 
			this.m_menuShortcut_ReviewFolder.Name = "m_menuShortcut_ReviewFolder";
			this.m_menuShortcut_ReviewFolder.Size = new System.Drawing.Size(372, 30);
			this.m_menuShortcut_ReviewFolder.Text = "바탕화면에 복습 시작 폴더 만들기";
			this.m_menuShortcut_ReviewFolder.Click += new System.EventHandler(this.OnMenu_Shortcut_ReviewFolder_Click);
			// 
			// m_menuShortcut_ExeFile
			// 
			this.m_menuShortcut_ExeFile.Name = "m_menuShortcut_ExeFile";
			this.m_menuShortcut_ExeFile.Size = new System.Drawing.Size(372, 30);
			this.m_menuShortcut_ExeFile.Text = "바탕화면에 실행 파일 만들기";
			this.m_menuShortcut_ExeFile.Click += new System.EventHandler(this.OnMenu_Shortcut_ExeFile_Click);
			// 
			// m_menuEtc
			// 
			this.m_menuEtc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuEtc_DateSetting,
            this.m_menuEtc_OpenStudyFolder,
            this.m_menuEtc_Help});
			this.m_menuEtc.Name = "m_menuEtc";
			this.m_menuEtc.Size = new System.Drawing.Size(122, 29);
			this.m_menuEtc.Text = "기타_도움말";
			// 
			// m_menuEtc_DateSetting
			// 
			this.m_menuEtc_DateSetting.Name = "m_menuEtc_DateSetting";
			this.m_menuEtc_DateSetting.Size = new System.Drawing.Size(216, 30);
			this.m_menuEtc_DateSetting.Text = "날짜 세팅";
			this.m_menuEtc_DateSetting.Click += new System.EventHandler(this.OnMenuEtc_DateSetting_Click);
			// 
			// m_menuEtc_OpenStudyFolder
			// 
			this.m_menuEtc_OpenStudyFolder.Name = "m_menuEtc_OpenStudyFolder";
			this.m_menuEtc_OpenStudyFolder.Size = new System.Drawing.Size(216, 30);
			this.m_menuEtc_OpenStudyFolder.Text = "공부 폴더 열기";
			this.m_menuEtc_OpenStudyFolder.Click += new System.EventHandler(this.OnMenuEtc_OpenStudyFolder_Click);
			// 
			// m_menuEtc_Help
			// 
			this.m_menuEtc_Help.Name = "m_menuEtc_Help";
			this.m_menuEtc_Help.Size = new System.Drawing.Size(216, 30);
			this.m_menuEtc_Help.Text = "도움말";
			this.m_menuEtc_Help.Click += new System.EventHandler(this.OnMenuEtc_Help_Click);
			// 
			// ReviewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(551, 661);
			this.Controls.Add(this.m_uiReview);
			this.Controls.Add(this.m_uiTextState);
			this.Controls.Add(this.m_uiTextFolder);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.m_uiReviewList);
			this.Controls.Add(this.menuStrip2);
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.Name = "ReviewerForm";
			this.Text = "Reviewer";
			this.menuStrip2.ResumeLayout(false);
			this.menuStrip2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox m_uiReviewList;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox m_uiTextFolder;
		private System.Windows.Forms.Label m_uiTextState;
		private System.Windows.Forms.Button m_uiReview;
		private System.Windows.Forms.MenuStrip menuStrip2;
		private System.Windows.Forms.ToolStripMenuItem m_menuFile;
		private System.Windows.Forms.ToolStripMenuItem m_menuShortcut;
		private System.Windows.Forms.ToolStripMenuItem m_menuFile_Exit;
		private System.Windows.Forms.ToolStripMenuItem m_menuShortcut_ReviewFolder;
		private System.Windows.Forms.ToolStripMenuItem m_menuShortcut_ExeFile;
		private System.Windows.Forms.ToolStripMenuItem m_menuEtc;
		private System.Windows.Forms.ToolStripMenuItem m_menuEtc_DateSetting;
		private System.Windows.Forms.ToolStripMenuItem m_menuEtc_OpenStudyFolder;
		private System.Windows.Forms.ToolStripMenuItem m_menuEtc_Help;
	}
}

