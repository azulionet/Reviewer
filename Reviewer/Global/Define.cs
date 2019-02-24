using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

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

		AfterDateGap = 10000, // 고정날짜는 1,2,3... ~일 후 복습은 10003, 10007, 10015, 10030...
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
				s.Append("Error][");
			}

			s.Append(System.IO.Path.GetFileName(a_sFilePath));
			s.Append(" - ");
			s.Append(a_nLineNum);
			s.Append("] : ");
			s.Append(a_sLog);

			File.Wright(Path.FileName(Path.eFile.Log), s.ToString());

			return s.ToString();
		}

		public static string LogError(string a_sLog,
							   string a_sFileName = Path.sLogFile,
								[CallerFilePath] string a_sFilePath = "",
								[CallerLineNumber] int a_nLineNum = 0)
		{
			string s = Log(a_sLog, eLog.Error, a_sFileName, a_sFilePath, a_nLineNum);
			Debug.Assert(false, s);

			return s;
		}

		// 구조체성 클래스 -----------------------------------------------------------
	}

	public static class Path
	{
		public enum eFile
		{
			Config,
			Log,
		}

		public static readonly string sSavePath = string.Empty;

		// 사용 폴더
		public const string sRootFolder		= "Reviewer\\";
		public const string sStrt			= "Start\\";
		public const string sFinish			= "Finish\\";
		public const string sLog			= "Log\\";

		// 사용 파일
		public const string sConfigFile		= "Config.json";
		public const string sLogFile		= "ToolLog.txt";
		public const string sToolLogFile	= "ToolLog.txt";

		static Path()
		{
			sSavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			sSavePath = System.IO.Path.Combine(sSavePath, sRootFolder);

			if (System.IO.File.Exists(sSavePath) == false)
			{
				System.IO.Directory.CreateDirectory(sSavePath);
			}
		}

		public static string FileName(eFile a_eFile)
		{
			string sTemp = string.Empty;

			switch (a_eFile)
			{
				case eFile.Config: { sTemp = sConfigFile; } break;
				case eFile.Log: { sTemp = sLogFile; } break;
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
					a_fpRead(s);
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
						eMode = FileMode.Open;
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
				m_nID			= a_nID;
				m_unSettingTime	= a_unMicroSec;
				m_unNowTime		= a_unMicroSec;

				m_fpCallback	= a_fpCallback;
				m_bRepeat		= a_bRepeat;
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

	// 인터페이스 -----------------------------------------------------------
	public interface IMustDeepCopy<T>
	{
		void Copy(T a_refSource);
	}
}
