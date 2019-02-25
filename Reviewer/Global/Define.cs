﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

using LitJson;

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

	// 날짜 폴더를 제외한 특수폴더 : 폴더 날짜를 양의 정수형으로 하기 때문에 이 폴더는 음의 정수를 취함
	public enum eFolder
	{
		// Finish, Start 사이에 추가 요망 ( for문 돌리기 쉽게 하기 위함 )
		Finish	= -1,			
		Log		= -2,
		Start	= -3,
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

			File.Wright(Path.FileName_inMyDocument(Path.eFile.Log), s.ToString());

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
	}

	public static class JsonHelper
	{
		public static ConfigData GetConfig()
		{
			string sFileName = Path.FileName_inMyDocument(Path.eFile.Config);

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

			File.Wright(Path.FileName_inMyDocument(Path.eFile.Config), s, File.eWrite.OverWrite);
		}
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
		public const string sStart			= "Start\\";
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

			// 문서(윈도우 특별폴더) 에 리뷰어 폴더 생성
			if (System.IO.File.Exists(sSavePath) == false)
			{
				System.IO.Directory.CreateDirectory(sSavePath);
			}

			// 컨피그파일 생성
			string sConfig = System.IO.Path.Combine(sSavePath, sConfigFile);

			if( System.IO.File.Exists(sConfig) == false )
			{
				System.IO.File.Create(sConfig);
			}
		}

		public static string FolderName(eFolder a_eFolder)
		{
			switch (a_eFolder)
			{
				case eFolder.Finish:	{ return sFinish; }
				case eFolder.Log:		{ return sLog; }
				case eFolder.Start:		{ return sStart; }
			}

			Define.LogError("arg error");
			return string.Empty;
		}

		public static string FileName_inMyDocument(eFile a_eFile)
		{
			string sTemp = string.Empty;

			switch (a_eFile)
			{
				case eFile.Config:	{ sTemp = sConfigFile; } break;
				case eFile.Log:		{ sTemp = sLogFile; } break;

				default:			{ Define.LogError("arg error"); return string.Empty; }
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
			data = JsonHelper.GetConfig();

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
				1,2,3,

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
