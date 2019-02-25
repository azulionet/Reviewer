using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reviewer.Global;

namespace Reviewer
{
	public abstract class Folder
	{
		public int			m_nData;
		public string		m_sName;
		public string		m_sName_withFullPath;
		public List<string>	m_liChild = new List<string>();

		public string sFolderName
		{
			get
			{
				if (Config.bIsSetting == false) { Define.LogError("logic error"); return string.Empty; }

				return System.IO.Path.Combine(Config.sFolderPath, m_sName);
			}
		}

		public int nFileCount { get { return m_liChild.Count; } }

		public Folder(int a_nData)
		{
			m_nData = a_nData;

			SetName();

			if (string.IsNullOrEmpty(m_sName_withFullPath) == true) { Define.LogError("logic error"); return; }

			if (System.IO.File.Exists(m_sName_withFullPath) == true) // 폴더가 있다면 안의 파일, 폴더를 취합
			{
				var arName = System.IO.Directory.GetFiles(sFolderName);
				m_liChild.AddRange(arName);
			}
			else // 폴더가 없었다면 ~일 폴더들을 만듬
			{
				System.IO.Directory.CreateDirectory(sFolderName);
			}
		}

		public abstract void SetName();
	}
	
	public class DateFolder : Folder // 날짜 폴더 하나, 내부의 파일들
	{
		public DateFolder(int a_nData) : base(a_nData) { }
		
		public override void SetName()
		{
			string sAdd = string.Empty;
			int nName = m_nData;

			if (nName < (int)Global.eDate.AfterDateGap)
			{
				sAdd = Properties.Resources.sFixedDateForderAdd;
			}
			else
			{
				sAdd = Properties.Resources.sAfterDateFolderAdd;
				nName -= (int)Global.eDate.AfterDateGap;
			}

			m_sName = string.Format("{0}{1}", nName, sAdd); // 0일, 1일, 1일 후 등이 됨 
			m_sName_withFullPath = System.IO.Path.Combine(Config.sFolderPath, m_sName);
		}
	}

	public class SpecialFolder : Folder // 날짜 제외 특수 폴더
	{
		public eFolder eFolder => (eFolder)m_nData;

		public SpecialFolder(int a_nData) : base(a_nData) { }

		public override void SetName()
		{
			m_sName = Global.Path.FolderName(eFolder);
			m_sName_withFullPath = System.IO.Path.Combine(Config.sFolderPath, m_sName);
		}
	}
}
