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
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OpenPandora
{
	public class WindowsHook : IDisposable
	{	
		//
		// Structs
		//
		
		#region private class KeyboardHookStruct
		[StructLayout(LayoutKind.Sequential)]
		private class KeyboardHookStruct
		{
			public int vkCode;
			public int scanCode;
			public int flags;
			public int time;
			public int dwExtraInfo;
		}
		#endregion
		
		//
		// Events
		//
		
		public event KeyEventHandler KeyDown;
		public event KeyEventHandler KeyUp;
		
		//
		// Constructors
		//
		
		#region public WindowsHook()
		public WindowsHook()
		{
			AttachKeyboardHook();
		}
		#endregion
		
		#region ~WindowsHook()
		~WindowsHook()
		{
			Dispose(false);
		}
		#endregion
		
		//
		// Public properties
		//
		
		#region public bool Control
		public bool Control
		{
			get { return controlPressed; }
		}
		#endregion
		
		#region public bool Win
		public bool Win
		{
			get { return winPressed; }
		}
		#endregion
		
		//
		// Dispose
		//
		
		#region public void Dispose()
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
		
		#region protected virtual void Dispose(bool disposing)
		protected virtual void Dispose(bool disposing)
		{
			if (!isDisposed)
			{
				if (disposing)
				{
					// Dispose managed resources.
				}

				DetachKeyboardHook(disposing);
				
				isDisposed = true;
			}
		}
		#endregion
		
		//
		// Public methods
		//
		
		#region public void Clean()
		public void Clean()
		{
			controlPressed = false;
			winPressed = false;
		}
		#endregion
		
		//
		// Private methods
		//
		
		#region private void AttachKeyboardHook()
		private void AttachKeyboardHook()
		{
			keyboardHook = new User32.HOOKPROC(KeyboardHookHandler);
			GC.KeepAlive(keyboardHook);
			
			hKeyboardHook = User32.SetWindowsHookEx(
				WindowsHookCode.WH_KEYBOARD_LL,
				keyboardHook,
				(int)Marshal.GetHINSTANCE(
				Assembly.GetExecutingAssembly().GetModules()[0]),
				0);

			if (hKeyboardHook == 0)
			{
				int errorCode = Marshal.GetLastWin32Error();
				DetachKeyboardHook(true);
				throw new Win32Exception(errorCode);
			}
		}
		#endregion
		
		#region private void DetachKeyboardHook(bool disposing)
		private void DetachKeyboardHook(bool disposing)
		{
			if (hKeyboardHook != 0)
			{
				int retKeyboard = User32.UnhookWindowsHookEx(hKeyboardHook);
				
				hKeyboardHook = 0;

				if (retKeyboard == 0 && disposing)
				{
					int errorCode = Marshal.GetLastWin32Error();
					throw new Win32Exception(errorCode);
				}
			}
		}
		#endregion
		
		#region private int KeyboardHookHandler(int nCode, int wParam, int lParam)
		private int KeyboardHookHandler(int nCode, int wParam, int lParam)
		{
			bool handled = false;
			
			if (nCode >= 0)
			{
				KeyboardHookStruct keyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure((IntPtr)lParam, typeof(KeyboardHookStruct));
				
				if (wParam == WindowsMessageCode.WM_KEYDOWN || wParam == WindowsMessageCode.WM_SYSKEYDOWN)
				{
					KeyEventArgs e = new KeyEventArgs((Keys)keyboardHookStruct.vkCode);
					OnKeyDown(e);
					
					handled = handled || e.Handled;
				}
				
				if (wParam == WindowsMessageCode.WM_KEYUP || wParam == WindowsMessageCode.WM_SYSKEYUP)
				{
					KeyEventArgs e = new KeyEventArgs((Keys)keyboardHookStruct.vkCode);
					OnKeyUp(e);
					
					handled = handled || e.Handled;
				}
			}
			
			if (handled)
			{
				return 1;
			}
			else
			{
				return User32.CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
			}
		}
		#endregion
		
		#region private void OnKeyDown(KeyEventArgs e)
		private void OnKeyDown(KeyEventArgs e)
		{
			if (KeyDown != null)
			{	
				if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
				{
					controlPressed = true;
				}
				else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
				{
					winPressed = true;
				}
				else
				{
					KeyDown(this, e);
				}
			}
		}
		#endregion
		
		#region private void OnKeyUp(KeyEventArgs e)
		private void OnKeyUp(KeyEventArgs e)
		{
			if (KeyUp != null)
			{
				if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
				{
					controlPressed = false;
				}
				else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
				{
					winPressed = false;
				}
				else
				{
					KeyUp(this, e);
				}
			}
		}
		#endregion
		
		//
		// Private data
		//
		
		private User32.HOOKPROC keyboardHook;
		private bool isDisposed = false;
		private int hKeyboardHook;
		private bool controlPressed = false;
		private bool winPressed = false;
	}
}
