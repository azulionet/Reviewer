using Reviewer.Global;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Reviewer
{
	public class ReviewMng
	{
		#region SINGLETON

		private static ReviewMng m_Instance = null;

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

		public Form							m_refMainForm = null;
		public List<Folder>					m_liDateFolder = new List<Folder>();
		public Dictionary<eFolder, Folder>	m_mapSpecialFolder = new Dictionary<eFolder, Folder>();

		public LinkedList<File>				m_liStudyList = new LinkedList<File>();

		public bool bInit { get; private set; } = false;

		public void Init(Form a_refForm)
		{
			// Step 0. 폼 등록
			m_refMainForm = a_refForm;

			// Step 1. 컨피그 파일 초기화
			Global.Config.Init();
			
			// Step 2. (컨피그에 폴더 세팅이 되있다면) 폴더가 없다면 생성, 있다면 파일들을 취합 
			ResearchFoldersAndFiles();

			bInit = true;
		}

		public void ResearchFoldersAndFiles()
		{
			if (Config.bIsSetting == false) { return; }

			var sRoot = Config.sFolderPath;
			var liDate = Config.liDate;

			m_liDateFolder.Clear();
			m_mapSpecialFolder.Clear();

			// 특수폴더부터 생성해야함
			for (int i = (int)eFolder.Start; i <= (int)eFolder.Finish; ++i)
			{
				eFolder eFolder = (eFolder)i;
				m_mapSpecialFolder.Add(eFolder, new Folder((eFolder)eFolder));
			}

			foreach (var val in liDate)
			{
				m_liDateFolder.Add(new Folder(eFolder.Normal, val));
			}

			// 오늘 복습할 파일들 취합 
			_CollectTodayReviewFile();

			return;

			#region LOCAL_FUNCTION

			void _CollectTodayReviewFile()
			{
				m_liStudyList.Clear();

				foreach (var file in m_mapSpecialFolder[eFolder.Start].m_liChild)
				{
					m_liStudyList.AddLast(file);
				}

				foreach (var folder in m_liDateFolder)
				{
					foreach (var file in folder.m_liChild)
					{
						if (file.bIsTodayReviewFile == true)
						{
							m_liStudyList.AddLast(file);
						}
					}
				}
			}

			#endregion LOCAL_FUNCTION
		}

		public void ChangeDate(List<int> m_refAllDay)
		{
			if (Config.SetDate(m_refAllDay) == true)
			{
				// todo : 날짜리스트 변경되면 폴더안에 파일들이 다 바뀜 할 일 개마늠

				ResearchFoldersAndFiles();
			}
		}

		public void MoveFiles(List<object> a_liMove)
		{
			if( a_liMove == null || a_liMove.Count == 0 ) { Define.LogError("arg error"); return; }

			foreach( File f in a_liMove )
			{
				if( f == null ) { Define.LogError("logic error"); continue; }

				MoveFile(f);
				m_liStudyList.Remove(f);
			}
		}

		void MoveFile(File a_cTargetFile)
		{
			if( a_cTargetFile == null ) { Define.LogError("arg error"); return; }

			Folder fDest = GetNextFolder(a_cTargetFile.m_refParent);

			// 파일이름 바꾸고
			// 실제 파일 이동

			fDest.m_liChild.AddLast(a_cTargetFile);
		}

		Folder GetNextFolder(Folder a_cTarget)
		{
			if( a_cTarget == null ) { Define.LogError("arg error"); return null; }





			return null;
		}
	}
}
