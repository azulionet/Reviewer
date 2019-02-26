using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using LitJson;

// 바로가기를 위함
using IWshRuntimeLibrary; // COM의 Windows Script Host Object Model 참조 필요, 참조 설정 Interop 형식 true -> false로 해야함
using System.Windows.Forms;

// 대다수의 클래스는 멀티쓰레드에서 사용하는 걸 고려하지 않고 제작
namespace Reviewer.Global
{
	public enum eTimerEvent
	{
		StateText,

	}

	public enum eDate
	{
		FixedDate,	// 1,2,3일
		AfterDate,	// 3일 후, 7일 후, 15일 후, 30일 후
		
		FixedDateGap = 10000, // 고정날짜가 컨피그에 저장될 때, 1,2,3일 => 10001, 10002, 10003
		AfterDateGap = 20000, // ~일 후 날짜 컨피그에 저장될 때, 3일후, 7일후, 15일후 => 20003, 20007, 20015
	}

	// 날짜 폴더를 제외한 특수폴더
	public enum eFolder
	{
		// Start, Finish 사이에 enum 추가요망( for 문 돌리기 용이하도록 )
		Start,
		Log,
		Finish,

		CountMax,
	}

	// 일반 복습용 파일을 제외한 특수 파일
	public enum eFile
	{
		Review,
		NoReviewed,
		Finished,
		Log,
	}

	public static partial class Timer
	{
		public static void Start(this eTimerEvent a_eType, uint a_nMicroSec, System.Action a_fpCallback)
		{
			Add((int)a_eType, a_nMicroSec, a_fpCallback);
		}
	}






	public static partial class Define
	{
		public enum eLog
		{
			Error,
			Log,
		}

		// 유틸 함수 -----------------------------------------------------------

		// Log
		public static string Log(string a_sLog,
								eLog a_eLogType = eLog.Log,
								string a_sFileName = Path.sLogFile,
								[CallerFilePath] string a_sFilePath = "",
								[CallerLineNumber] int a_nLineNum = 0)
		{
			StringBuilder s = new StringBuilder("[");
			s.Append(DateTime.Now);
			s.Append("]");

			if (a_eLogType == eLog.Error)
			{
				s.Append("[Error][");
			}

			s.Append(System.IO.Path.GetFileName(a_sFilePath));
			s.Append(" - ");
			s.Append(a_nLineNum);
			s.Append("] : ");
			s.Append(a_sLog);

			File.Wright(Path.FileName_inMyDocument(Path.eDocumentFile.Log), s.ToString());

			return s.ToString();
		}

		// assert도 겸함
		public static string LogError(string a_sLog,
							   string a_sFileName = Path.sLogFile,
								[CallerFilePath] string a_sFilePath = "",
								[CallerLineNumber] int a_nLineNum = 0)
		{
			string s = Log(a_sLog, eLog.Error, a_sFileName, a_sFilePath, a_nLineNum);
			Debug.Assert(false, s);

			return s;
		}

		public static void MakeShortCut(string a_sMakeShortcutyName, string a_sOringinFileExecutablePath)
		{
			var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			DirectoryInfo info = new DirectoryInfo(desktopDir);
			
			string sFileName = info.FullName + "\\" + a_sMakeShortcutyName + ".lnk";

			FileInfo fileInfo = new FileInfo(sFileName);

			try
			{
				WshShell shell = new WshShell();
				IWshShortcut link = (IWshShortcut)shell.CreateShortcut(fileInfo.FullName);
				link.TargetPath = a_sOringinFileExecutablePath;
				link.Save();
			}
			catch(Exception ex)
			{
				Define.LogError(string.Format("Make Shortcut Error - {0}", ex.Message));
			}
		}
	}

	public static class JsonHelper
	{
		public static ConfigData GetConfig()
		{
			string sFileName = Path.FileName_inMyDocument(Path.eDocumentFile.Config);

			if (System.IO.File.Exists(sFileName) == false)
			{
				return null;
			}

			string s = File.Read(sFileName, null);

			if( string.IsNullOrEmpty(s) == true )
			{
				return null;
			}
			
			return LitJson.JsonMapper.ToObject<ConfigData>(s);
		}

		public static void SaveConfig(ConfigData a_refData)
		{
			string s = LitJson.JsonMapper.ToJson(a_refData);

			File.Wright(Path.FileName_inMyDocument(Path.eDocumentFile.Config), s, File.eWrite.OverWrite);
		}
	}

	public static class Path
	{
		public enum eDocumentFile
		{
			Config,
			Log,
		}

		public static readonly string sSavePath = string.Empty;

		// 사용 폴더
		public const string sRootFolder		= "Reviewer\\";

		// 사용 파일
		public const string sConfigFile		= "Config.json";
		public const string sLogFile		= "ToolLog.txt";
		public const string sToolLogFile	= "ToolLog.txt";

		static Path()
		{
			sSavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			sSavePath = System.IO.Path.Combine(sSavePath, sRootFolder);

			// 문서(윈도우 특별폴더) 에 리뷰어 폴더 생성
			if (System.IO.File.Exists(sSavePath) == false)
			{
				System.IO.Directory.CreateDirectory(sSavePath);
			}
		}

		public static string FolderName(eFolder a_eFolder)
		{
			string sName = string.Empty;

			switch (a_eFolder)
			{
				case eFolder.Start:		{ return sName = Properties.Resources.sFolderName_Start; }
				case eFolder.Log:		{ return sName = Properties.Resources.sFolderName_Log; }
				case eFolder.Finish:	{ return sName = Properties.Resources.sFolderName_Finish; }
			}

			if( string.IsNullOrEmpty(sName) == true )
			{
				Define.LogError("arg error");
				return string.Empty;
			}

			sName += "\\";			
			return string.Empty;
		}

		public static string FileName_inMyDocument(eDocumentFile a_eFile)
		{
			string sTemp = string.Empty;

			switch (a_eFile)
			{
				case eDocumentFile.Config:	{ sTemp = sConfigFile; } break;
				case eDocumentFile.Log:		{ sTemp = sLogFile; } break;

				default:					{ Define.LogError("arg error"); return string.Empty; }
			}

			return System.IO.Path.Combine(sSavePath, sTemp);
		}
	}

	public delegate bool fpRead(string read);
	public delegate bool fpWright(string write);

	public static class File
	{
		public enum eWrite
		{
			AddLast,
			OverWrite,
		}

		private static bool IsLocked(string a_sFileName, FileAccess a_eFileAccess = FileAccess.ReadWrite)
		{
			try
			{
				FileStream fs = new FileStream(a_sFileName, FileMode.Open, a_eFileAccess);
				fs.Close();
				return false;
			}
			catch (IOException)
			{
				return true;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static string Read(string a_sFile, fpRead a_fpRead)
		{
			string s = string.Empty;

			// 모든 파일을 읽음
			using (FileStream fs = new FileStream(a_sFile, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs))
				{
					string temp = null;
					while ( (temp = sr.ReadLine() ) != null)
					{
						s += temp;
					}

					a_fpRead?.Invoke(s);
				}
			}

			return s;
		}

		public static void Wright(string a_sFileName, string a_sWrite, eWrite a_eWrite = eWrite.AddLast)
		{
			string s = string.Empty;
			FileMode eMode = FileMode.Truncate;

			if (System.IO.File.Exists(a_sFileName) == false)
			{
				eMode = FileMode.CreateNew;
			}
			else
			{
				switch (a_eWrite)
				{
					case eWrite.AddLast:
						eMode = FileMode.Append;
						break;
					case eWrite.OverWrite:
						eMode = FileMode.Create;
						break;
					default:
						break;
				}
			}

			// 한 줄 마지막 쓰기, 전체 데이터 덮어쓰기
			using (FileStream fs = new FileStream(a_sFileName, eMode))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					sw.WriteLine(a_sWrite);
				}
			}
		}
	}

	public static partial class Timer // ExtensionMethod 로만 접근을 권장
	{
		class TimerEvent
		{
			public int				m_nID;
			public uint				m_unSettingTime;
			public uint				m_unNowTime;

			public System.Action	m_fpCallback;
			public bool				m_bRepeat;

			public TimerEvent(int a_nID, uint a_unMicroSec, System.Action a_fpCallback, bool a_bRepeat)
			{
				SetData(a_nID, a_unMicroSec, a_fpCallback, a_bRepeat);
			}

			public void SetData(int a_nID, uint a_unMicroSec, System.Action a_fpCallback, bool a_bRepeat)
			{
				m_nID = a_nID;
				m_unSettingTime = a_unMicroSec;
				m_unNowTime = a_unMicroSec;

				m_fpCallback = a_fpCallback;
				m_bRepeat = a_bRepeat;
			}
		}

		static System.Windows.Forms.Timer m_Timer = null;
		static LinkedList<TimerEvent> m_liTimerEvent = new LinkedList<TimerEvent>();
		static List<TimerEvent> m_liTempRemove = new List<TimerEvent>(); 
		const int nCALL_TIME = 100; // 0.1초에 한 번씩 불리도록 설정

		public static void Destroy()
		{
			m_liTimerEvent.Clear();

			if (m_Timer != null)
			{
				m_Timer.Stop();
				m_Timer.Dispose();
			}
		}

		static void Add(int a_nID, uint a_unMicroSec, System.Action a_fpCallback, bool a_bRepeat = false)
		{
			foreach(var val in m_liTimerEvent)
			{
				if( val.m_nID == a_nID ) // 동일 아이디가 있다면 덮어쓰기
				{
					val.SetData(a_nID, a_unMicroSec, a_fpCallback, a_bRepeat);
					return;
				}
			}
			
			if (m_Timer == null)
			{
				m_Timer = new System.Windows.Forms.Timer();
				m_Timer.Interval = nCALL_TIME;
				m_Timer.Tick += OnTimerEvent;
				m_Timer.Start();
			}

			m_liTimerEvent.AddLast(new TimerEvent(a_nID, a_unMicroSec, a_fpCallback, a_bRepeat));
		}

		static void OnTimerEvent(object sender, EventArgs e)
		{
			foreach (var val in m_liTimerEvent)
			{
				val.m_unNowTime -= nCALL_TIME;

				if( val.m_unNowTime == 0 )
				{
					val.m_fpCallback();

					if ( val.m_bRepeat == true )
					{
						val.m_unNowTime = val.m_unSettingTime;
					}
					else
					{
						m_liTempRemove.Add(val);
					}
				}
			}

			foreach( var remove in m_liTempRemove )
			{
				m_liTimerEvent.Remove(remove);
			}
		}
	}

	// 구조체성 클래스
	public class ConfigData
	{
		public string sDateString = string.Empty;
		public string sFolderPath = string.Empty;
		public List<int> liDate = new List<int>();

		public bool IsSetting()
		{
			bool bSetting = false;

			bSetting |= string.IsNullOrEmpty(sDateString);
			bSetting |= string.IsNullOrEmpty(sFolderPath);
			bSetting |= liDate.Count == 0;

			// 위에 꺼중 하나라도 true면 안됨
			return !bSetting;
		}
	}

	public static class Config
	{
		private static ConfigData	data = null;

		public static void Init()
		{
			// 컨피그파일 없다면 생성
			string sConfig = System.IO.Path.Combine(Path.sSavePath, Path.sConfigFile);

			if (System.IO.File.Exists(sConfig) == false)
			{
				System.IO.File.Create(sConfig);
			}
			else
			{
				data = JsonHelper.GetConfig();
			}
			
			if( data == null )
			{
				data = new ConfigData();
			}

			if( data.IsSetting() == false )
			{
				SetDefaultValue();
			}
		}

		public static void SetDefaultValue()
		{
			data.sDateString = DateTime.Now.ToShortDateString();

			data.liDate = new List<int>()
			{
				// 1일, 2일, 3일
				 1 + (int)Global.eDate.FixedDateGap,
				 2 + (int)Global.eDate.FixedDateGap,
				 3 + (int)Global.eDate.FixedDateGap,

				// 3일후, 7일후, 15일후, 30일후
				 3 + (int)Global.eDate.AfterDateGap,
				 7 + (int)Global.eDate.AfterDateGap,
				15 + (int)Global.eDate.AfterDateGap,
				30 + (int)Global.eDate.AfterDateGap,
			};

			// 폴더는 실제 세팅 UI에서 세팅 해야 적용
		}

		public static void SetFolderPath(string s)
		{
			data.sFolderPath = s;

			SaveConfig();
		}

		public static bool SetDate(List<int> a_liDate)
		{
			if (data.liDate.CheckMatch(a_liDate) == true)
			{
				return false;
			}
			
			data.liDate.Clear();
			data.liDate.AddRange(a_liDate);

			if (bIsSetting == true)
			{
				Config.SaveConfig();
			}

			return true;
		}

		public static void SaveConfig()
		{
			if( bIsSetting == false )
			{
				Define.LogError("logic error! - config value not setted");
				return;
			}

			JsonHelper.SaveConfig(data);
		}

		public static bool			bIsSetting	=> data.IsSetting();
		public static string		sDateString	=> data.sDateString;
		public static string		sFolderPath	=> data.sFolderPath;
		public static List<int>		liDate		=> data.liDate;
	}

}
