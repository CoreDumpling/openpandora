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

using System.Runtime.InteropServices;

namespace OpenPandora
{
	public class Shell32
	{
		[DllImport("shell32.dll", EntryPoint="ShellExecuteA")]
		public static extern int ShellExecute(int hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

		[DllImport("shell32.dll", EntryPoint="Shell_NotifyIcon")]
		public static extern bool Shell_NotifyIcon(int dwMessage, ref NOTIFYICONDATA lpData);

		[StructLayout(LayoutKind.Sequential)]
		public struct NOTIFYICONDATA 
		{
			public int cbSize;
			public int hwnd;
			public int uID;
			public int uFlags;
			public int uCallbackMessage;
			public int hIcon;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x80)]
			public string szTip;
			public int dwState;
			public int dwStateMask;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=0xFF)]
			public string szInfo;
			public int uTimeout;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40)]
			public string szInfoTitle;
			public int dwInfoFlags;
		}

		public const int NIIF_NONE = 0x00;
		public const int NIIF_INFO = 0x01;
		public const int NIIF_WARNING = 0x02;
		public const int NIIF_ERROR = 0x03;
		public const int NIIF_NOSOUND = 0x10;

		public const int NIF_MESSAGE = 0x01;
		public const int NIF_ICON = 0x02;
		public const int NIF_TIP = 0x04;
		public const int NIF_STATE = 0x08;
		public const int NIF_INFO = 0x10;

		public const int NIM_ADD = 0x00;
		public const int NIM_MODIFY = 0x01;
		public const int NIM_DELETE = 0x02;
		public const int NIM_SETFOCUS = 0x03;
		public const int NIM_SETVERSION = 0x04;
	}
}
