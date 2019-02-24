using Reviewer.Global;
using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Reviewer
{
	public partial class ReviewDateForm : Form
	{
		List<int> m_liAllDay = new List<int>();
		List<int> m_liFixedDay = new List<int>();
		List<int> m_liAfterDay = new List<int>();

		public ReviewDateForm()
		{
			InitializeComponent();
		}

		// 이벤트 콜백
		private void OnForm_Load(object sender, EventArgs e)
		{
			m_liAllDay.Clear();
			m_liFixedDay.Clear();
			m_liAfterDay.Clear();

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

				m_liAllDay.Add(val);
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

		// 이벤트 처리기
		private void OKButton_Click(object sender, EventArgs e)
		{
			m_liAllDay.Clear();
			m_liFixedDay.Clear();
			m_liAfterDay.Clear();

			try
			{
				var arS = m_uiFixedDateText.Text.Split(',');

				foreach( var s in arS )
				{
					if (string.IsNullOrWhiteSpace(s) == true) { continue; }

					m_liFixedDay.Add( int.Parse(s) );
				}

				if( m_liFixedDay.HasDuplicatedValue() == true )
				{
					MessageBox.Show(Properties.Resources.sDateStringDuplicated,
									Properties.Resources.sOK);
					return;
				}

				m_liFixedDay.Sort((a, b) => { return a.CompareTo(b); });
				m_liAllDay.AddRange(m_liFixedDay);

				arS = this.m_uiAfterDateText.Text.Split(',');
				
				foreach (var s in arS)
				{
					if( string.IsNullOrWhiteSpace(s) == true ) { continue; }

					m_liAfterDay.Add(int.Parse(s) + (int)Global.eDate.AfterDateGap);
				}

				m_liAllDay.AddRange(m_liAfterDay);
				
				if ( m_liAllDay.CheckMatch(ReviewMng.Ins.m_liDate) == false )
				{
					ReviewMng.Ins.ChangeDate(m_liAllDay);
				}

				m_uiFixedDateText.Text = "";
				m_uiAfterDateText.Text = "";

				Close();
			}
			catch
			{
				MessageBox.Show(Properties.Resources.sDateStringError,
								Properties.Resources.sOK);
			}
		}		
	}
}
