using Reviewer.Global;
using System;

namespace Reviewer
{
    // 어떠한 경우에도 경로에대한 정보는 가지지 않도록 작업, 경로는 부모 폴더가 캐싱하는 형태로 관리
    public class File
    {
        public ReviewFile m_cReview = null;

        public string m_sName_noPath;
        public Folder m_refParent;

        public string sUIName // 폼에서 보일 파일명
        {
            get
            {
                return m_cReview?.m_sName_NoExt ?? m_sName_noPath;
            }
        }

        public bool bIsTodayReviewFile
        {
            get
            {
                if (m_cReview == null) { return false; }

                DateTime now = DateTime.Parse(DateTime.Now.ToShortDateString());
                DateTime targetDate = DateTime.Parse(m_cReview.m_sReviewDate);

                return ((now - targetDate).Days >= 0);
            }
        }

        public string sName_withFullPath
        {
            get
            {
                string sName = m_cReview?.sName ?? m_sName_noPath;
                return System.IO.Path.Combine(m_refParent.m_sName_withFullPath, sName);
            }
        }

        public File(string a_sFileName, Folder a_refParent)
        {
            m_refParent = a_refParent;
            m_sName_noPath = a_sFileName;

            CheckAndParsingName(a_sFileName);
        }

        private void CheckAndParsingName(string a_sFileName)
        {
            if (m_refParent == null) { Define.LogError("logic error"); return; }

            switch (m_refParent.m_eFolder)
            {
                case eFolder.Normal:
                case eFolder.UnmatchDate:

                    m_cReview = Global.Path.ParsingReviewFileData(a_sFileName);

                    if (m_cReview == null)
                    {
                        Define.Log("filename not matched" + a_sFileName);
                        System.Windows.Forms.MessageBox.Show(string.Format(Properties.Resources.sNotMatchedFileName, a_sFileName));
                        return;
                    }

                    break;

                case eFolder.Start: // 얘네는 할게 없음
                case eFolder.Finish:
                case eFolder.Study:
                    break;

                default:
                    Define.LogError("arg error"); return;
            }
        }

        // CheckListItem 에서 표기하기 위해 재정의
        public override string ToString()
        {
            return sUIName;
        }

        public void SetReview(int a_nOffset)
        {
            if (m_cReview != null)
            {
                ++m_cReview.m_nStudyCount;

                var d = DateTime.Now + new TimeSpan(a_nOffset, 0, 0, 0);
                m_cReview.m_sReviewDate = d.ToShortDateString();
            }
            else // 스타트 폴더에서 옮겨지는 경우
            {
                if (this.m_refParent.m_eFolder != eFolder.Start)
                {
                    Define.LogError("?? this is not revied file - {0}", m_refParent.m_eFolder.ToString());
                    return;
                }

                m_cReview = Global.Path.MakeReviewFileData(this.m_sName_noPath, a_nOffset);
                m_sName_noPath = m_cReview.sName;
            }
        }
    }
}
