using System;
using System.Windows.Forms;

namespace Reviewer
{
	public partial class HelpForm : Form
	{
		public HelpForm()
		{
			InitializeComponent();

			linkLabel1.Links.Add(10, 3, Properties.Resources.sOriginYoutubeURL);
			linkLabel2.Links.Add(14, 3, Properties.Resources.sExplainYotubeURL);
		}

		private void On_URLLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			LinkLabel link = sender as LinkLabel;

			if (link == null) { Global.Define.LogError("ui setting error"); return; }

			link.Links[link.Links.IndexOf(e.Link)].Visited = true;

			string s = e.Link.LinkData as string;

			if (string.IsNullOrEmpty(s) == false &&
                Uri.IsWellFormedUriString(s, UriKind.Absolute) == true )
			{
				System.Diagnostics.Process.Start(s);
			}
			else
			{
				Global.Define.LogError("resource error");
			}
		}
	}
}
