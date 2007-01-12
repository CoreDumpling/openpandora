/*
 * Copyright (C) 2006 Eitan Pogrebizsky <openpandora@gmail.com>, 
 * and individual contributors.
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

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
			try
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
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion
	}
}