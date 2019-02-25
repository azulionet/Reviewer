using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Reviewer.Global;

namespace Reviewer
{
	public class ReviewMng
	{
		#region SINGLETON

		public static ReviewMng Ins
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = new ReviewMng();
				}

				return m_Instance;
			}
		}

		#endregion

		private static ReviewMng m_Instance = null;

		public bool bInit { get; private set; } = false;

		public Form m_refMainForm = null;
		  
		public void Init(Form a_refForm)
		{
			m_refMainForm = a_refForm;

			Global.Config.Init();

			bInit = true;
		}

		public void ChangeDate(List<int> m_refAllDay)
		{
			if( Config.SetDate(m_refAllDay) == true )
			{
				// 날짜리스트 변경되면 폴더안에 파일들이 다 바뀜 개마늠

			}
		}
	}
}
