using System;
using System.Windows.Forms;

using Reviewer.Global;

namespace Reviewer
{
	public partial class ReviewerForm : Form
	{
		public enum eState
		{
			StandBy,
			Wait,
		}

		eState m_eState = eState.Wait;

		public ReviewerForm()
		{
			InitializeComponent();

			// 프로그램 초기화

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

		public bool bStandByState
		{
			get
			{
				return (ReviewMng.Ins.bInit == true) &&
						(m_eState == eState.StandBy);
			}
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
				else if( result == DialogResult.Cancel )
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

			var form = new ReviewDateForm();
			form.ShowDialog();
		}
	}
}
