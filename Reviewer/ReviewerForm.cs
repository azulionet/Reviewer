using Reviewer.Global;
using System;
using System.Windows.Forms;

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

		public bool bStandByState
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
		}

		~ReviewerForm()
		{
			// 프로그램 정리
			Global.Timer.Destroy();
		}

		// 이벤트 처리기
		private void OnFolderSettingButton_Click(object sender, EventArgs e)
		{
			if (bStandByState == false) { return; }

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

			// 데일리 폴더들이 있다면 오른쪽에 표기 & 데이터 수집
			// 없으면 냅도야지
		}

		private void OnReviewButton_Click(object sender, EventArgs e)
		{
			if (bStandByState == false) { return; }

		}

		private void OnDateApplyButton_Click(object sender, EventArgs e)
		{
			if (bStandByState == false) { return; }

		}

		private void OnDateSettingButton_Click(object sender, EventArgs e)
		{
			if (bStandByState == false) { return; }

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
	}
}
