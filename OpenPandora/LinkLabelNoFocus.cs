using System;
using System.Windows.Forms; 
using System.Drawing;
using System.Diagnostics;

namespace OpenPandora.Windows.Forms
{
	public class LinkLabelNoFocus : LinkLabel 
	{ 
		private string url;
		private string fullText;

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

		public new string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				fullText = value;
				base.Text = value;
				BuildText(value);
				this.LinkArea = new LinkArea(0, this.Text.Length);
			}
		}


		#region private void BuildText(string text)
		private void BuildText(string text)
		{
			Graphics textGraphics = this.CreateGraphics();

			SizeF textSize = textGraphics.MeasureString(base.Text, this.Font, this.Size.Width);
			textGraphics.Dispose();

			if (Math.Round(textSize.Height - 0.5) > this.Size.Height)
			{
				string shortText = text.Substring(0, text.Length - 1);
				base.Text = shortText + "...";
				BuildText(shortText);
			}
		}
		#endregion
	}
}