using System.Windows.Forms; 

namespace OpenPandora.Windows.Forms
{
	public class LinkLabelNoFocus : LinkLabel 
	{ 

		private string _URL;
		private string _FullText;
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
				return _URL;
			}
			set
			{
				_URL = value;
			}
		}

		public string FullText
		{
			get
			{
				return _FullText;
			}
			set
			{
				_FullText = value;
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