﻿namespace Reviewer
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
			this.m_uiDateTime = new System.Windows.Forms.DateTimePicker();
			this.button2 = new System.Windows.Forms.Button();
			this.m_uiFolderTree = new System.Windows.Forms.TreeView();
			this.button3 = new System.Windows.Forms.Button();
			this.m_uiTextFolder = new System.Windows.Forms.TextBox();
			this.m_uiTextState = new System.Windows.Forms.Label();
			this.button4 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_uiReviewList
			// 
			this.m_uiReviewList.FormattingEnabled = true;
			this.m_uiReviewList.Location = new System.Drawing.Point(18, 64);
			this.m_uiReviewList.Margin = new System.Windows.Forms.Padding(4);
			this.m_uiReviewList.Name = "m_uiReviewList";
			this.m_uiReviewList.Size = new System.Drawing.Size(287, 556);
			this.m_uiReviewList.TabIndex = 0;
			this.m_uiReviewList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnReviewList_ItemCheck);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(333, 557);
			this.button1.Margin = new System.Windows.Forms.Padding(4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(146, 63);
			this.button1.TabIndex = 1;
			this.button1.Text = "복습";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnReviewButton_Click);
			// 
			// m_uiDateTime
			// 
			this.m_uiDateTime.Location = new System.Drawing.Point(333, 69);
			this.m_uiDateTime.Margin = new System.Windows.Forms.Padding(4);
			this.m_uiDateTime.Name = "m_uiDateTime";
			this.m_uiDateTime.Size = new System.Drawing.Size(472, 28);
			this.m_uiDateTime.TabIndex = 2;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(333, 117);
			this.button2.Margin = new System.Windows.Forms.Padding(4);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(472, 38);
			this.button2.TabIndex = 3;
			this.button2.Text = "날짜 적용";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.OnDateApplyButton_Click);
			// 
			// m_uiFolderTree
			// 
			this.m_uiFolderTree.Location = new System.Drawing.Point(823, 64);
			this.m_uiFolderTree.Name = "m_uiFolderTree";
			this.m_uiFolderTree.Size = new System.Drawing.Size(312, 556);
			this.m_uiFolderTree.TabIndex = 4;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(18, 11);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(287, 35);
			this.button3.TabIndex = 6;
			this.button3.Text = "폴더 설정";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.OnFolderSettingButton_Click);
			// 
			// m_uiTextFolder
			// 
			this.m_uiTextFolder.Enabled = false;
			this.m_uiTextFolder.Location = new System.Drawing.Point(311, 16);
			this.m_uiTextFolder.Name = "m_uiTextFolder";
			this.m_uiTextFolder.Size = new System.Drawing.Size(824, 28);
			this.m_uiTextFolder.TabIndex = 7;
			// 
			// m_uiTextState
			// 
			this.m_uiTextState.AutoSize = true;
			this.m_uiTextState.Location = new System.Drawing.Point(22, 631);
			this.m_uiTextState.Name = "m_uiTextState";
			this.m_uiTextState.Size = new System.Drawing.Size(35, 18);
			this.m_uiTextState.TabIndex = 8;
			this.m_uiTextState.Text = "---";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(333, 175);
			this.button4.Margin = new System.Windows.Forms.Padding(4);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(472, 38);
			this.button4.TabIndex = 10;
			this.button4.Text = "날짜 세팅";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.OnDateSettingButton_Click);
			// 
			// ReviewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1160, 661);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.m_uiTextState);
			this.Controls.Add(this.m_uiTextFolder);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.m_uiFolderTree);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.m_uiDateTime);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.m_uiReviewList);
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.Name = "ReviewerForm";
			this.Text = "Reviewer";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox m_uiReviewList;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.DateTimePicker m_uiDateTime;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TreeView m_uiFolderTree;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox m_uiTextFolder;
		private System.Windows.Forms.Label m_uiTextState;
		private System.Windows.Forms.Button button4;
	}
}

