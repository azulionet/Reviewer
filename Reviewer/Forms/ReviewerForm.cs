﻿using Reviewer.Global;
using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Reviewer
{
	public partial class ReviewerForm : Form
	{
		public enum eState
		{
			StandBy,
			Wait,
		}

		Form m_DateForm = null;
		Form m_HelpForm = null;

		eState m_eState = eState.Wait;

		Dictionary<int, File> m_mapStudyList = new Dictionary<int, File>();

		public bool bStandByUIState
		{
			get
			{
				return (ReviewMng.Ins.bInit == true) &&
						(m_eState == eState.StandBy);
			}
		}

		public ReviewerForm()
		{
			InitializeComponent();

			m_uiTextState.Text = Properties.Resources.sState_Wait;

			// 프로그램 초기화
			ReviewMng.Ins.Init(this);

			string path = Config.sFolderPath;
			m_uiTextFolder.Text = string.IsNullOrEmpty(path) ? Properties.Resources.sFolderDefaultWord : path;

			// 상태 ㅇㅋ
			m_eState = eState.StandBy;
			m_uiTextState.Text = Properties.Resources.sState_Default;

			PrintStudyList();
		}

		~ReviewerForm()
		{
			// 프로그램 정리
			Global.Timer.Destroy();
		}

		void PrintStudyList()
		{
			m_uiReviewList.Items.Clear();
			m_mapStudyList.Clear();

			var list = ReviewMng.Ins.m_liStudyList;
			int nIndex = 0;

			if( list.Count == 0 )
			{
				m_uiReviewList.Items.Add(Properties.Resources.sNoReviewFiles, CheckState.Indeterminate);
			}
			else
			{
				Folder temp = null;

				foreach (var file in list)
				{
					if( temp != file.m_refParent )
					{
						temp = file.m_refParent;

						StringBuilder sPrint = new StringBuilder("---------- ");
						sPrint.Append(temp.sUIName);
						sPrint.Append(" ----------");
						
						m_uiReviewList.Items.Add(sPrint, CheckState.Indeterminate);
						++nIndex;
					}

					m_uiReviewList.Items.Add(file);
					m_mapStudyList.Add(nIndex, file);
					++nIndex;
				}
			}
		}

		// 이벤트 처리기 ------------------------------------------------------------------------
		private void OnFolderSettingButton_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }

			string sPath = string.Empty;

			using (var dialog = new FolderBrowserDialog())
			{
				DialogResult result = dialog.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
				{
					sPath = dialog.SelectedPath;
				}
				else if (result == DialogResult.Cancel)
				{
					return;
				}
			}

			if (string.IsNullOrEmpty(sPath) == true)
			{
				MessageBox.Show(Properties.Resources.sERROR_WRONG_PATH,
								Properties.Resources.sOK);

				return;
			}

			m_uiTextFolder.Text = sPath;

			Config.SetFolderPath(sPath);
			ReviewMng.Ins.ResearchFoldersAndFiles();
		}

		bool m_bEventOpen = false;
		private void OnReviewButton_Click(object sender, EventArgs e)
		{
			int nCount = m_uiReviewList.SelectedItems.Count;

			if (nCount == 0)
			{
				MessageBox.Show(Properties.Resources.sReviewSelectFile);
				return;
			}
			else if (nCount >= 2)
			{
				MessageBox.Show(Properties.Resources.sReviewSelectMuchFile);
				return;
			}

			var file = m_uiReviewList.SelectedItem as File;

			if (file == null)
			{
				MessageBox.Show(Properties.Resources.sReviewSelectFile);
				return;
			}

			int nIndex = 0;

			foreach (var item in m_uiReviewList.Items)
			{
				if (item == m_uiReviewList.SelectedItem) { break; }

				++nIndex;
			}

			m_uiTextState.Text = Properties.Resources.sState_FileExecute;
			eTimerEvent.StateText.Start(1500, () => { m_uiTextState.Text = Properties.Resources.sState_Default; });

			m_bEventOpen = true;
			m_uiReviewList.SetItemCheckState(nIndex, CheckState.Checked);
			Define.ExecuteFile(file.sName_withFullPath);
		}

		List<object> m_liRemoveTemp = new List<object>();
		private void OnReviewCompleteButton_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }

			if ( m_uiReviewList.Items.Count == 0 ) { MessageBox.Show(Properties.Resources.sNoList_InReviewList); return; }

			m_liRemoveTemp.Clear();
			var liItems = m_uiReviewList.CheckedItems;			

			foreach( var item in liItems )
			{
				File f = item as File;

				if( f == null )
				{
					continue;
				}

				m_liRemoveTemp.Add(item);
			}

			if (m_liRemoveTemp.Count == 0)
			{
				MessageBox.Show(Properties.Resources.sNoCheckList_InReviewList);
				return;
			}

			if( ReviewMng.Ins.MoveFiles(m_liRemoveTemp) == false )
			{
				Define.LogError("file execption occured or logic error");
			}

			PrintStudyList();

			if (m_uiReviewList.Items.Count == 1) // 폴더 - 파일 이기 때문에 최소 2개. 1개라면 안내용문구 일때
			{
				MessageBox.Show(Properties.Resources.sCongraturation_FinishDailyReview);
			}
		}

		private void OnReviewList_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			// check 하는 것으론 버튼을 못 바꾸도록 수정 ( 복습 버튼을 눌러야만 체크가 됨 )
			if( e.NewValue == CheckState.Indeterminate ) { return; }

			if( m_bEventOpen == false )
			{
				switch (e.CurrentValue)
				{
					case CheckState.Unchecked:
						e.NewValue = CheckState.Unchecked;
						break;
					case CheckState.Checked:
						e.NewValue = CheckState.Checked;
						break;
					case CheckState.Indeterminate:
						e.NewValue = CheckState.Indeterminate;
						break;
				}
			}

			m_bEventOpen = false;
		}
		
		// 이벤트 처리기 - 메뉴 ------------------------------------------------------------------------
		private void OnMenu_Shortcut_ReviewFolder_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }
			if (Config.bIsSetting == false) { return; }

			string sStartName = ReviewMng.Ins.m_mapSpecialFolder[eFolder.Start].m_sName_withFullPath;
			Define.MakeShortCut(Properties.Resources.sShortcutStartReview, sStartName);
		}

		private void OnMenu_Shortcut_ExeFile_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }

			Define.MakeShortCut(Properties.Resources.sShortcutExe, Application.ExecutablePath);
		}

		private void OnMenu_File_Exit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void OnMenuEtc_DateSetting_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }

			if (m_DateForm == null)
			{
				m_DateForm = new ReviewDateForm();
			}

			m_DateForm.ShowDialog();
		}

		private void OnMenuEtc_OpenStudyFolder_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }

			if (Config.bIsSetting == false)
			{
				MessageBox.Show(Properties.Resources.sNoReviewFolder);
				return;
			}

			Define.ExecuteFile(Config.sFolderPath);
		}

		private void OnMenuEtc_Help_Click(object sender, EventArgs e)
		{
			if (m_HelpForm == null)
			{
				m_HelpForm = new HelpForm();
			}

			m_HelpForm.ShowDialog();
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if ( e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter )				
			{
				OnReviewButton_Click(null, default(EventArgs));
			}
		}
	}
}
