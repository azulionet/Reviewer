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

        public Form m_refMainForm = null;
        public List<Folder> m_liDateFolder = new List<Folder>();
        public Dictionary<eFolder, Folder> m_mapSpecialFolder = new Dictionary<eFolder, Folder>();

        public LinkedList<File> m_liStudyList = new LinkedList<File>();

        public bool bInit { get; private set; } = false;

        public int nMaxDateOffset
        {
            get
            {
                if (m_liDateFolder.Count == 0)
                {
                    return 0;
                }
                else if (m_liDateFolder.Count == 1)
                {
                    return m_liDateFolder[0].m_nDateOffset;
                }

                return m_liDateFolder[m_liDateFolder.Count - 1].m_nDateOffset - m_liDateFolder[0].m_nDateOffset;
            }
        }

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

            int nDateOffset = 0;
            foreach (var val in liDate)
            {
                if (val < (int)eDate.AfterDateGap)
                {
                    int nTemp = val - (int)eDate.FixedDateGap;
                    nDateOffset = nTemp;
                }
                else
                {
                    int nTemp = val - (int)eDate.AfterDateGap;
                    nDateOffset += nTemp;
                }

                m_liDateFolder.Add(new Folder(eFolder.Normal, val, nDateOffset));
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

        public bool MoveFiles(List<object> a_liMove)
        {
            if (a_liMove == null || a_liMove.Count == 0) { Define.LogError("arg error"); return false; }

            bool bResult = true;

            foreach (File f in a_liMove)
            {
                if (f == null) { Define.LogError("logic error"); continue; }

                try
                {
                    MoveFile(f);
                }
                catch
                {
                    bResult = false;
                    continue;
                }

                m_liStudyList.Remove(f);
            }

            return bResult;
        }

        void MoveFile(File a_cTargetFile)
        {
            if (a_cTargetFile == null) { Define.LogError("arg error"); return; }

            Folder fDest = GetNextFolder(a_cTargetFile.m_refParent);

            if (fDest == null) { Define.LogError("logic error?"); return; }

            int nOffset = 0;

            if (fDest.IsTypedFolder(eFolder.Finish) == true)
            {
                nOffset = nMaxDateOffset;
            }
            else
            {
                nOffset = fDest.m_nDateOffset - a_cTargetFile.m_refParent.m_nDateOffset;

                if (nOffset <= 0) { Define.LogError("logic error"); return; }
            }

            // 옮기기 이전 파일명 ( 풀패스 )
            string sBefore = a_cTargetFile.sName_withFullPath;

            // 변경될 데이터 세팅
            a_cTargetFile.SetReview(nOffset);

            // 자식 위치 변경
            a_cTargetFile.m_refParent.m_liChild.Remove(a_cTargetFile);
            a_cTargetFile.m_refParent = fDest;
            fDest.m_liChild.AddLast(a_cTargetFile);

            // 옮기기 이후 파일명 ( 풀패스 )
            string sAfter = a_cTargetFile.sName_withFullPath;

            Define.Log("Before - " + sBefore);
            Define.Log("After - " + sAfter);

            // 실제 파일 이동
            try
            {
                System.IO.File.Move(sBefore, sAfter);
            }
            catch (Exception ex)
            {
                Define.LogError("File Move Error - " + ex.Message);
                throw ex;
            }

            if (fDest.IsTypedFolder(eFolder.Finish) == true)
            {
                string s = string.Format(Properties.Resources.sCongraturation_FinishReviewFile, a_cTargetFile.sUIName);
                MessageBox.Show(s);
            }
        }

        Folder GetNextFolder(Folder a_cTarget)
        {
            if (a_cTarget == null) { Define.LogError("arg error"); return null; }

            switch (a_cTarget.m_eFolder)
            {
                case eFolder.Normal:

                    for (int i = 0; i < m_liDateFolder.Count; ++i)
                    {
                        if (m_liDateFolder[i] == a_cTarget)
                        {
                            if (i == m_liDateFolder.Count - 1)
                            {
                                return m_mapSpecialFolder[eFolder.Finish];
                            }
                            else
                            {
                                return m_liDateFolder[i + 1];
                            }
                        }
                    }

                    break;
                case eFolder.UnmatchDate:

                    Folder returnFolder = null;

                    foreach (Folder f in m_liDateFolder)
                    {
                        if (f.m_nDateOffset > a_cTarget.m_nDateOffset)
                        {
                            returnFolder = f;
                        }
                    }

                    if (returnFolder == null)
                    {
                        returnFolder = m_mapSpecialFolder[eFolder.Finish];
                    }

                    return returnFolder;

                case eFolder.Start:

                    return m_liDateFolder[0];
            }

            Define.LogError("logic error or arg error");
            return null;
        }
    }
}
