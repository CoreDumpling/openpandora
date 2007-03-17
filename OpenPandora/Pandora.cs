/*
 * Copyright (C) 2007 Eitan Pogrebizsky <openpandora@gmail.com>, 
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

using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System;
using System.Drawing;
using AxSHDocVw;
using mshtml;

namespace OpenPandora
{
	public class Pandora
	{
		#region public Pandora(AxSHDocVw.AxWebBrowser browser)
		public Pandora(AxSHDocVw.AxWebBrowser browser)
		{
			this.browser = browser;
			this.radioSize = new Size(0, 0);
			
			this.located = false;
			this.hwndPandora = 0;

			ThreadPool.QueueUserWorkItem(new WaitCallback(this.LocateRadio));
		}
		#endregion
		
		//
		// Public methods
		//
		
		#region public void Refresh()
		public void Refresh()
		{
			located = false;
		}
		#endregion
		
		#region public void PlayPause()
		public void PlayPause()
		{
			Debug.WriteLine("User trying PlayPause");

			LocateRadio();
			
			if (hwndPandora != 0)
			{
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)Keys.Space, 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)Keys.Space, 0);
			}
		}
		#endregion

		#region public void NextTrack()
		public void NextTrack()
		{
			Debug.WriteLine("User trying NextTrack");

			LocateRadio();
			
			if (hwndPandora != 0)
			{
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)Keys.Right, 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)Keys.Right, 0);
			}
		}
		#endregion
		
		#region public void Like()
		public void Like()
		{
			Debug.WriteLine("User trying Like");

			LocateRadio();
			
			if (hwndPandora != 0)
			{
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)Keys.Add, 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)Keys.Add, 0);
			}
		}
		#endregion
		
		#region public void Hate()
		public void Hate()
		{
			Debug.WriteLine("User trying Hate");

			LocateRadio();
			
			if (hwndPandora != 0)
			{
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)Keys.Subtract, 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)Keys.Subtract, 0);
			}
		}
		#endregion
		
		#region public void VolumeUp()
		public void VolumeUp()
		{
			if (IsVolumeSkipped())
			{
				return;
			}

			lastVolumeChange = DateTime.Now;

			Debug.WriteLine("User trying VolumeUp");

			LocateRadio();
			
			if (hwndPandora != 0)
			{
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)Keys.Up, 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)Keys.Up, 0);
			}
		}
		#endregion
		
		#region public void VolumeDown()
		public void VolumeDown()
		{
			if (IsVolumeSkipped())
			{
				return;
			}

			lastVolumeChange = DateTime.Now;

			Debug.WriteLine("User trying VolumeDown");

			LocateRadio();
			
			if (hwndPandora != 0)
			{
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)Keys.Down, 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)Keys.Down, 0);
			}
		}
		#endregion
		
		#region public void FullVolume()
		public void FullVolume()
		{
			LocateRadio();
			
			if (hwndPandora != 0)
			{
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)(Keys.Shift), 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)(Keys.Up), 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)(Keys.Up), 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)(Keys.Shift), 0);
			}
		}
		#endregion
		
		#region public void Mute()
		public void Mute()
		{
			LocateRadio();
			
			if (hwndPandora != 0)
			{
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)(Keys.Shift), 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYDOWN, (int)(Keys.Down), 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)(Keys.Down), 0);
				User32.SendMessage(hwndPandora, WindowsMessageCode.WM_KEYUP, (int)(Keys.Shift), 0);
			}
		}
		#endregion

		//
		// Private methods
		//
		
		#region private void LocateRadio(object o)
		private void LocateRadio(object o)
		{
			LocateRadio();
		}
		#endregion

		#region private void LocateRadio()
		private void LocateRadio()
		{
			if (located)
			{
				return;
			}

			if (radioSize.Height == 0 || radioSize.Width == 0)
			{
				radioSize = GetRadioSize(browser);

				if (radioSize.Height == 0 || radioSize.Width == 0)
				{
					return;
				}
			}
			
			hwndPandora = 0;
			
			int hShellDocObjectView = User32.FindWindowEx((int)browser.Handle, 0, "Shell DocObject View", null);
			int hInternetExplorerServer = User32.FindWindowEx(hShellDocObjectView, 0, "Internet Explorer_Server", null);
				
			int height = 0;
			int width = 0;
				
			do
			{
				hwndPandora = User32.FindWindowEx(hInternetExplorerServer, hwndPandora, "MacromediaFlashPlayerActiveX", null);

				if (hwndPandora != 0)
				{
					User32.RECT rc = new User32.RECT();
					User32.GetWindowRect(hwndPandora, ref rc);

					height = rc.bottom - rc.top;
					width = rc.right - rc.left;
				}
			} while (!(height == radioSize.Height && width == radioSize.Width) && hwndPandora != 0);
			
			Debug.WriteLine("hwndRadio: " + hwndPandora);

			// Setting located true even though we didn't find, to prevent repeated
			// call to located on each user event.
			located = true;
		}
		#endregion
		
		#region private bool IsVolumeSkipped()
		private bool IsVolumeSkipped()
		{
			return ((DateTime.Now - lastVolumeChange).TotalMilliseconds < 250);
		}
		#endregion

		#region private static Size GetRadioSize(AxSHDocVw.AxWebBrowser browser)
		private static Size GetRadioSize(AxSHDocVw.AxWebBrowser browser)
		{
			IHTMLDocument2 document = (IHTMLDocument2)browser.Document;

			if (document == null)
			{
				Debug.WriteLine("Radio Size: document not loaded");

				return new Size(0, 0);
			}

			IHTMLElement element = (IHTMLElement)document.all.item("radio", 0);

			if (element == null)
			{
				Debug.WriteLine("Radio Size: missing");

				return new Size(0, 0);
			}
			
			return new Size(element.offsetWidth, element.offsetHeight);
		}
		#endregion

		//
		// Private data
		//
		
		private AxSHDocVw.AxWebBrowser browser;
		private int hwndPandora;
		private bool located;
		private Size radioSize;

		private DateTime lastVolumeChange = DateTime.MinValue;
	}
}
