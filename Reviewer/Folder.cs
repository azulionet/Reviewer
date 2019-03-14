using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reviewer.Global;

namespace Reviewer
{
	public class Folder
	{
		public eFolder		m_eFolder;
		public int			m_nData;
		public int			m_nDateOffset;
		public string		m_sName;
		public string		m_sName_withFullPath;
	
		public LinkedList<File>	m_liChild = new LinkedList<File>();
		
		public bool			bIsSpecialFolder=> !((m_eFolder == eFolder.Normal || m_eFolder == eFolder.UnmatchDate));

		public string sUIName // 폼에서 보일 폴더명
		{
			get
			{
				if( bIsSpecialFolder == true )
				{
					return m_sName;
				}
				
				string sFormat = string.Empty;		

				if( m_nData < (int)Global.eDate.AfterDateGap )
				{
					sFormat = Properties.Resources.sFixedDateFoderAdd_UI;
				}
				else
				{
					sFormat = Properties.Resources.sAfterDateFolderAdd;
				}
				
				return string.Format(sFormat, m_nData % 10000);
			}
		}

		public string sFolderName
		{
			get
			{
				if (Config.bIsSetting == false) { Define.LogError("logic error"); return string.Empty; }

				return System.IO.Path.Combine(Config.sFolderPath, m_sName);
			}
		}

		public int nFileCount { get { return m_liChild.Count; } }

		public Folder(eFolder a_eFolder, int a_nData = 0, int a_nDateOffset = 0)
		{
			m_eFolder				= a_eFolder;
			m_nData					= a_nData;
			m_nDateOffset			= a_nDateOffset;

			m_sName = Global.Path.FolderName(m_eFolder, m_nData);
			m_sName_withFullPath = System.IO.Path.Combine(Config.sFolderPath, m_sName);

			if (string.IsNullOrEmpty(m_sName_withFullPath) == true) { Define.LogError("logic error"); return; }

			if (System.IO.Directory.Exists(m_sName_withFullPath) == true) // 폴더가 있다면 안의 파일, 폴더를 취합
			{
				var arName = System.IO.Directory.GetFiles(m_sName_withFullPath);

				for( int i=0; i<arName.Length; ++i )
				{
					arName[i] = System.IO.Path.GetFileName(arName[i]);
					m_liChild.AddLast(new File(arName[i], this));
				}
			}
			else // 폴더가 없었다면 ~일 폴더들을 만듬
			{
				System.IO.Directory.CreateDirectory(sFolderName);
			}
		}

		public bool IsTypedFolder(eFolder a_eType) { return m_eFolder == a_eType; }
	}

}
