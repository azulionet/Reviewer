using System;
using Reviewer.Global;

namespace Reviewer
{
	public class File
	{
		public class Review
		{
			string		m_sOriginName;
			int			m_nStudyCount;
			DateTime	m_tStartDate;
			DateTime	m_tLastReviewDate;
		}

		public Review	m_cReview = null;

		public string	m_sName_withFullPath;
		public Folder	m_refParent;

		public bool bIsTodayReviewFile
		{
			get
			{
				return true;
			}
		}

		public File(string a_sFileName, Folder a_refParent)
		{
			m_refParent = a_refParent;
			m_sName_withFullPath = a_sFileName;

			CheckAndParsingName(a_sFileName);
		}

		private void CheckAndParsingName(string a_sFileName)
		{
			if (m_refParent == null) { Define.LogError("logic error"); return; }

			switch (m_refParent.m_eFolder)
			{
				case eFolder.Normal:
					break;
				case eFolder.UnmatchDate:
					break;
				case eFolder.Start:
					break;
				case eFolder.Log:
					break;
				case eFolder.Finish:
					break;
				default:
					Define.LogError("arg error"); return;
			}
		}

		public void MoveFile_ResetData(Folder a_refNewParent)
		{
			if(a_refNewParent == null) { Define.LogError("arg error"); return; }
			
			switch (a_refNewParent.m_eFolder)
			{
				case eFolder.Normal:
					break;
				case eFolder.UnmatchDate:
					break;
				case eFolder.Start:
					break;
				case eFolder.Log:
					break;
				case eFolder.Finish:
					break;

				default:
					break;
			}

			m_refParent = a_refNewParent;
		}

		// CheckListItem 에서 표기하기 위해 재정의
		public override string ToString()
		{
			return System.IO.Path.GetFileName(m_sName_withFullPath);
		}
	}
}
