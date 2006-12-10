using System.Windows.Forms; 

namespace OpenPandora.Windows.Forms
{
	public class LinkLabelNoFocus : LinkLabel 
	{ 
		private string url;
		private string fullText;
		private string _ShortText;

		protected override bool ShowFocusCues 
		{ 
			get 
			{ 
				return false; 
			} 
		} 

		public string URL
		{
			get
			{
				return url;
			}
			set
			{
				url = value;
			}
		}

		public string FullText
		{
			get
			{
				return fullText;
			}
			set
			{
				fullText = value;
				_ShortText = value;
				this.Text = value;
				this.LinkArea = new LinkArea(0,this.Text.Length);
			}
		}
		public string ShortText
		{
			get
			{
				return _ShortText;
			}
			set
			{
				_ShortText = value;
			}
		}


	}
}