using Reviewer.Global;

namespace Reviewer
{
	class File
	{
		string	m_sName;
		Folder	m_refParent;
		eFile	m_eFileType;

		File(string a_sFileName, Folder a_refParent, eFile a_eType = eFile.Review)
		{
			m_sName		= a_sFileName;
			m_refParent	= a_refParent;
			m_eFileType	= a_eType;
		}





	}
}
