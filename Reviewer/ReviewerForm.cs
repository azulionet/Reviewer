﻿using Reviewer.Global;
using System;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

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

			m_uiTextState.Text = Properties.Resources.sWaitState;

			// 프로그램 초기화
			ReviewMng.Ins.Init(this);

			string path = Config.sFolderPath;
			m_uiTextFolder.Text = string.IsNullOrEmpty(path) ? Properties.Resources.sFolderDefaultWord : path;

			// 상태 ㅇㅋ
			m_eState = eState.StandBy;
			m_uiTextState.Text = Properties.Resources.sDefaultState;

			// m_uiReviewList.Items.Add("1");
			// m_uiReviewList.Items.Add("2");
			// m_uiReviewList.Items.Add("3");
			// m_uiReviewList.Items.Add("-----------------------", CheckState.Indeterminate);

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
						sPrint.Append(temp.m_sName);
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

				m_uiTextState.Text = "Error!";
				eTimerEvent.StateText.Start(1500, () => { m_uiTextState.Text = Properties.Resources.sWaitState; });

				return;
			}

			m_uiTextFolder.Text = sPath;

			Config.SetFolderPath(sPath);
			ReviewMng.Ins.ResearchFoldersAndFiles();

			// 데일리 폴더들이 있다면 오른쪽에 표기 & 데이터 수집
			// 없으면 냅도야지
		}

		List<object> m_liRemoveTemp = new List<object>();
		private void OnReviewButton_Click(object sender, EventArgs e)
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

			foreach (var item in m_liRemoveTemp)
			{
				m_uiReviewList.Items.Remove(item);
			}

			_ClearParentList();

			return;

			// m_uiReviewList.Items.RemoveAt(0);

			#region LOCAL_FUNCTION

			void _ClearParentList()
			{
				m_liRemoveTemp.Clear();

				var _Items = m_uiReviewList.CheckedItems;

				if( _Items.Count == 0 ) { return; }

				if (_Items.Count == 1)
				{
					if (_Items[0] is File)
					{
						Define.LogError("logic error");
						return;
					}

					m_liRemoveTemp.Add(_Items[0]);
				}
				else
				{
					var _list = m_uiReviewList.Items;

					object before = null;

					foreach( var _item in _list )
					{
						if(before == null)
						{
							if( (_item is File) == false )
							{
								before = _item;
								continue;
							}
						}
						else
						{
							if ((_item is File) == false)
							{
								m_liRemoveTemp.Add(_item);
								before = _item;
							}
							else
							{
								before = null;
							}
						}
					}
				}

				foreach (var _item in m_liRemoveTemp)
				{
					m_uiReviewList.Items.Remove(_item);
				}

				if( m_uiReviewList.Items.Count == 0 )
				{
					MessageBox.Show(Properties.Resources.sCongraturation);
				}
			}

			#endregion LOCAL_FUNCTION
		}

		private void OnReviewList_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			// check 하는 것으론 버튼을 못 바꾸도록 수정 ( 복습 버튼을 눌러야만 체크가 됨 )
			if( e.NewValue == CheckState.Indeterminate ) { return; }

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

			/*

			if ( e.CurrentValue == CheckState.Unchecked )
			{
				File file = m_uiReviewList.Items[e.Index] as File;

				if (file != null)
				{
					if( m_mapRunningFile.ContainsKey(file) == false )
					{

					}


					p = Define.ExecuteFile(file.m_sName_withFullPath);

					m_mapRunningFile.Add(file, p);
					
				}
			}
			
			*/
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

		}
	}
}
