using Reviewer.Global;
using System;
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

						m_uiReviewList.Items.Add(temp.m_sName, CheckState.Indeterminate);
						++nIndex;
					}

					m_uiReviewList.Items.Add(file);
					m_mapStudyList.Add(nIndex, file);
					++nIndex;
				}
			}
		}

		// 이벤트 처리기
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
		}

		private void OnDateApplyButton_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }

		}

		private void OnDateSettingButton_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }

			if (m_DateForm == null)
			{
				m_DateForm = new ReviewDateForm();
			}

			m_DateForm.ShowDialog();
		}

		private void OnReviewList_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (e.CurrentValue == CheckState.Indeterminate) // 구분선은 클릭되지 않도록 수정
			{
				e.NewValue = CheckState.Indeterminate;
			}
		}

		private void OnCreateShortCutButton_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }
			if (Config.bIsSetting == false) { return; }

			string sStartName = ReviewMng.Ins.m_mapSpecialFolder[eFolder.Start].m_sName_withFullPath;
			Define.MakeShortCut(Properties.Resources.sShortcutStartReview, sStartName);
		}

		private void OnCreateExeShortCutButton_Click(object sender, EventArgs e)
		{
			if (bStandByUIState == false) { return; }

			Define.MakeShortCut(Properties.Resources.sShortcutExe, Application.ExecutablePath);
		}
	}
}
