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

			// 프로그램 초기화
			ReviewMng.Ins.Init(this);

			// UI 초기화
			m_uiTextFolder.Text = Properties.Resources.sFolderDefaultWord;
			m_uiTextState.Text = Properties.Resources.sWaitState;

			m_eState = eState.StandBy;
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

			using (var fbd = new FolderBrowserDialog())
			{
				DialogResult result = fbd.ShowDialog();

				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					sPath = fbd.SelectedPath;
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

			// 데일리 폴더들이 있다면 오른쪽에 표기 & 데이터 수집
			// 없으면 냅도야지

			m_uiCreateFolderButton.Enabled = true;
		}

		private void OnReviewButton_Click(object sender, EventArgs e)
		{
			if (bStandByState == false) { return; }

		}

		private void OnDateApplyButton_Click(object sender, EventArgs e)
		{
			if (bStandByState == false) { return; }

		}

		private void OnCreateFolderButton_Click(object sender, EventArgs e)
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
	}
}
