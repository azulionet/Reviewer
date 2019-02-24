using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public DateTime		m_stToday;
		public List<int>	m_liDate = null;

		private ReviewMng()
		{
			// 1. 컨피그 파일이 없으면 만듬
			// 2. 컨피그 파일에 파일 경로가 없다면 저장해야됨
			// 3. 최초 경로 설정 창이 떠야함
			// 경로 설정후, 폴더가 없으면 만들어야하고, 폴더가 있다면 경로를 수집해야함
			// 


			m_stToday = DateTime.Now;

			// 기본이 1,2,3일 / 3일후, 7일후, 15일후, 30일후
			m_liDate = new List<int>
			{
				1,2,3,

				 3 + (int)Global.eDate.AfterDateGap,
				 7 + (int)Global.eDate.AfterDateGap,
				15 + (int)Global.eDate.AfterDateGap,
				30 + (int)Global.eDate.AfterDateGap,
			};
			
			bInit = true;
		}
	}
}
