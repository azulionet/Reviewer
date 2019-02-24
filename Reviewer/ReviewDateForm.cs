using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Reviewer
{
	public partial class ReviewDateForm : Form
	{
		public ReviewDateForm()
		{
			InitializeComponent();
		}

		// 이벤트 콜백
		private void OnForm_Load(object sender, EventArgs e)
		{
			List<int> m_liFixedDay = new List<int>();
			List<int> m_liAfterDay = new List<int>();

			var liSavedData = ReviewMng.Ins.m_liDate;

			foreach( var val in liSavedData)
			{
				if( val < (int)Global.eDate.AfterDateGap )
				{
					m_liFixedDay.Add(val);
				}
				else
				{
					m_liAfterDay.Add(val - (int)Global.eDate.AfterDateGap);
				}
			}

			StringBuilder s = new StringBuilder();
			
			for( int i=0; i<m_liFixedDay.Count; ++i )
			{
				s.Append(m_liFixedDay[i]);

				if( i < (m_liFixedDay.Count - 1) )
				{
					s.Append(", ");
				}
			}
		
			m_uiFixedDateText.Text = s.ToString();
		
			s.Clear();
			for (int i = 0; i < m_liAfterDay.Count; ++i)
			{
				s.Append(m_liAfterDay[i]);

				if (i < (m_liAfterDay.Count - 1))
				{
					s.Append(", ");
				}
			}

			m_uiAfterDateText.Text = s.ToString();
		}

		private void OnForm_Close(object sender, FormClosedEventArgs e)
		{

		}

		// 이벤트 처리기
		private void OKButton_Click(object sender, EventArgs e)
		{

		}

		
	}
}
