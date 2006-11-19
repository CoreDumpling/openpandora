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
	public class User32
	{
		//
		// Imports
		//
		
		[DllImport("user32", EntryPoint="SendMessageA")]
		public static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern int FindWindow(string lpClassName, string lpWindowName);
		
		[DllImport("user32", EntryPoint="FindWindowExA")]
		public static extern int FindWindowEx(int hwndParent, int hwndChildAfter, string lpszClass, string lpszWindow);
		
		[DllImport("user32.dll")]
		public static extern int GetWindowRect(int hwnd, ref RECT rc);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int SetWindowsHookEx(int idHook, HOOKPROC lpfn, int hMod, int dwThreadId);
		
		[DllImport("user32")]
		public static extern int GetKeyboardState(byte[] pbKeyState);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern short GetKeyState(int vKey);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode, int wParam, int lParam);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		public static extern int UnhookWindowsHookEx(int idHook);
		
		[DllImport("user32.dll")]
		public static extern int SetActiveWindow(int hwnd);
		
		//
		// Delegates
		//
		
		public delegate int HOOKPROC(int nCode, int wParam, int lParam);
		
		//
		// Structs
		//
		
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT 
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}
	}
}
