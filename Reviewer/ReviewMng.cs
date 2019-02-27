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

		public Form					m_refMainForm = null;

		public List<Folder>			m_liDateFolder = new List<Folder>();
		public Folder[]				m_arSpecialFolder = new Folder[(int)eFolder.CountMax];

		public void Init(Form a_refForm)
		{
			m_refMainForm = a_refForm;

			Global.Config.Init();

			// 폴더가 없다면 생성, 있다면 파일들을 취합 
			ResearchFoldersAndFiles();

			bInit = true;
		}

		public void ResearchFoldersAndFiles()
		{
			if( Config.bIsSetting == false ) { return; }

			var sRoot = Config.sFolderPath;
			var liDate = Config.liDate;

			m_liDateFolder.Clear();
			
			foreach ( var val in liDate )
			{
				m_liDateFolder.Add(new Folder(eFolder.Normal, val));
			}

			for ( int i=(int)eFolder.Start; i<=(int)eFolder.Finish; ++i )
			{
				m_arSpecialFolder[i] = new Folder((eFolder)i);
			}			
		}
		
		public void ChangeDate(List<int> m_refAllDay)
		{
			if( Config.SetDate(m_refAllDay) == true )
			{
				// todo : 날짜리스트 변경되면 폴더안에 파일들이 다 바뀜 할 일 개마늠

				ResearchFoldersAndFiles();
			}
		}
	}
}
