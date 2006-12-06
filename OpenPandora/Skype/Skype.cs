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
using System.Data;
using System.Security;
using System.Threading;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace OpenPandora
{
	/// <summary>
	/// Summary description for Skype.
	/// </summary>
	public class Skype : System.Windows.Forms.Form
	{

		#region Data
		private const int HWND_BROADCAST = 0xffff;
		private const int WM_COPYDATA = 0x4a;
		private const string skypeControlAPIDiscover = "SkypeControlAPIDiscover";
		private const string skypeControlAPIAttach = "SkypeControlAPIAttach";
		private const int WM_FAILURE = 0;
		private const int WM_SUCCESS = 1;

		[SuppressUnmanagedCodeSecurityAttribute()]
		[DllImport("user32.dll", EntryPoint="RegisterWindowMessage")] 
		public static extern int RegisterWindowMessageA(string lpString);

		[SuppressUnmanagedCodeSecurityAttribute()]
		[DllImport("User32.dll")] 
		public static extern int SendMessage( int hwnd, int uMsg, int wParam, ref COPYDATASTRUCT lParam);

		[StructLayout(LayoutKind.Sequential)]
		public struct COPYDATASTRUCT
		{
			public IntPtr dwData;
			public int cbData;
			public IntPtr lpData;
		}

		private const int  SKYPECONTROLAPI_ATTACH_SUCCESS = 0;               // Client is successfully attached and API window handle can be found in wParam parameter;
		private const int  SKYPECONTROLAPI_ATTACH_PENDING_AUTHORIZATION = 1; // Skype has acknowledged connection request and is waiting for confirmation from the user.
		private const int  SKYPECONTROLAPI_ATTACH_REFUSED = 2;               // User has explicitly denied access to client;
		private const int  SKYPECONTROLAPI_ATTACH_NOT_AVAILABLE = 3;         // API is not available at the moment. For example, this happens when no user is currently logged in.

		public int skypeHandler = 0 ;

		private int WM_SKYPECONTROLAPIDISCOVER = 0; 
		private int WM_SKYPECONTROLAPIATTACH = 0;
		#endregion

		//
		// Constructor
		//
		#region public Skype()
		public Skype()
		{
			WM_SKYPECONTROLAPIDISCOVER = RegisterWindowMessageA(skypeControlAPIDiscover);
			WM_SKYPECONTROLAPIATTACH = RegisterWindowMessageA(skypeControlAPIAttach);
		}
		#endregion

		//
		// Private methods
		//
		#region private void InitializeComponent()
		private void InitializeComponent()
		{
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 271);
			this.Name = "Skype";
			this.ShowInTaskbar = false;
		}
		#endregion

		//
		// Public methods
		//
		#region public bool Connect()
		public bool Connect()
		{
			int reply = 0;
			string param = string.Empty;

			skypeHandler = 0;

			IntPtr ptr = Marshal.StringToHGlobalAnsi(param);            
			COPYDATASTRUCT cds = new COPYDATASTRUCT();            
			cds.dwData = IntPtr.Zero;            
			cds.cbData = param.Length;
			cds.lpData = ptr;				
		
			reply = SendMessage(HWND_BROADCAST, WM_SKYPECONTROLAPIDISCOVER, this.Handle.ToInt32(),ref cds );

			if (reply == WM_FAILURE) 
				return false;
			else
			{
				return true;
			}
		}
		#endregion

		#region public bool SetMessage(string message)
		public bool SetMessage(string message)
		{
			int reply = 0;

			if (message != string.Empty)
			{

				message = "SET PROFILE MOOD_TEXT " + message;

				IntPtr ptr = Marshal.StringToHGlobalAnsi(message);            
				COPYDATASTRUCT cds = new COPYDATASTRUCT();            
				cds.dwData = IntPtr.Zero;            
				cds.cbData = message.Length + 1;
				cds.lpData = ptr;

				if (skypeHandler == 0)
					return false;
				else
				{			
					reply = SendMessage(skypeHandler, WM_COPYDATA, this.Handle.ToInt32(),ref cds);

					if (reply == WM_FAILURE)
					{
						skypeHandler = 0;
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			else
			{
				return false;
			}
		}
		#endregion

		//
		// Protected Override methods
		//
		#region protected override void WndProc(ref System.Windows.Forms.Message m)
		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			if (m.Msg == WM_SKYPECONTROLAPIDISCOVER)
			{
				m.Result = (IntPtr)1;
			}
			else if (m.Msg == WM_SKYPECONTROLAPIATTACH)
			{
				switch (m.LParam.ToInt32())
				{
					case SKYPECONTROLAPI_ATTACH_SUCCESS:
						if (m.WParam.ToInt32() != 0)
							skypeHandler = m.WParam.ToInt32();
						break;
					case SKYPECONTROLAPI_ATTACH_PENDING_AUTHORIZATION:
						break;
					case SKYPECONTROLAPI_ATTACH_REFUSED:
						break;
					case SKYPECONTROLAPI_ATTACH_NOT_AVAILABLE:
						break;
				}
				m.Result = (IntPtr)1;
			}

			else if (m.Msg == WM_COPYDATA)
			{
				string newMessage = string.Empty;
				COPYDATASTRUCT cds = new COPYDATASTRUCT(); 

				cds = (COPYDATASTRUCT) Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));                    
				newMessage = "<- " + Marshal.PtrToStringAnsi(cds.lpData);
				m.Result = (IntPtr)1;
			}
			else
				base.WndProc(ref m); 	
		}
		#endregion

	}
}