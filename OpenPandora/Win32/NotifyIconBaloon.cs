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

using System;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OpenPandora
{
	public class NotifyIconBaloon
	{
		//
		// Public methods
		//

		#region public static void Show(string title, string text, NotifyIcon notifyIcon)
		public static void Show(string title, string text, NotifyIcon notifyIcon)
		{	
			try
			{
				if (title.Length > 60)
				{
					title = title.Substring(0, 58) + " ...";
				}
				
				if (text.Length > 250)
				{
					text = text.Substring(0, 248) + " ...";
				}
				
				Shell32.NOTIFYICONDATA notifyIconData = new Shell32.NOTIFYICONDATA();

				notifyIconData.cbSize = Marshal.SizeOf(typeof(Shell32.NOTIFYICONDATA));
				notifyIconData.hwnd = GetNotifyIconHandle(notifyIcon);
				notifyIconData.uID = GetNotifyIconID(notifyIcon);
				notifyIconData.uFlags = Shell32.NIF_INFO;
				notifyIconData.uTimeout = 3000;
				notifyIconData.dwInfoFlags = Shell32.NIIF_NONE | Shell32.NIIF_NOSOUND;
				notifyIconData.szInfoTitle = title;
				notifyIconData.szInfo = text;

				Shell32.Shell_NotifyIcon(Shell32.NIM_MODIFY, ref notifyIconData);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		//
		// Public methods
		//

		#region public static void AnimateMinimizeToTray(Form form)
		public static void AnimateMinimizeToTray(Form form)
		{
			if (UseWindowAnimation())
			{
				int hwnd = (int)form.Handle;
				User32.RECT rectFrom = new User32.RECT();
				User32.RECT rectTo = new User32.RECT();

				User32.GetWindowRect(hwnd, ref rectFrom);
				GetTrayWndRect(ref rectTo);

				User32.DrawAnimatedRects(hwnd, User32.IDANI_CAPTION, ref rectFrom, ref rectTo);
			}
		}
		#endregion

		#region public static void AnimateRestoreFromTray(Form form)
		public static void AnimateRestoreFromTray(Form form)
		{
			if (UseWindowAnimation())
			{
				int hwnd = (int)form.Handle;

				User32.RECT rectTo = new User32.RECT();
				User32.GetWindowRect(hwnd, ref rectTo);

				User32.RECT rectFrom = new User32.RECT();
				GetTrayWndRect(ref rectFrom);

				User32.DrawAnimatedRects(hwnd, User32.IDANI_CAPTION, ref rectFrom, ref rectTo);
			}
		}
		#endregion

		//
		// Private methods
		//

		#region private static object GetField(object o, string fieldName, Type type)
		private static object GetField(object o, string fieldName, Type type)
		{
			MemberInfo[] memberInfo = type.GetMember(fieldName,BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

			if (memberInfo.Length == 0)
			{
				return -1;
			}
			else
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo[0];
				return fieldInfo.GetValue(o);
			}
		}
		#endregion

		#region private static object GetField(object o, string fieldName)
		private static object GetField(object o, string fieldName)
		{
			return GetField(o, fieldName, o.GetType());
		}
		#endregion

		#region private static int GetNotifyIconID(NotifyIcon notifyIcon)
		private static int GetNotifyIconID(NotifyIcon notifyIcon)
		{
			return (int)GetField(notifyIcon, "id");
		}
		#endregion

		#region private static int GetNotifyIconHandle(NotifyIcon notifyIcon)
		private static int GetNotifyIconHandle(NotifyIcon notifyIcon)
		{
			object notifyIconWindow = GetField(notifyIcon, "window");
			
			if (notifyIconWindow == null)
			{
				return 0;
			}
			else
			{
				return ((IntPtr)GetField(notifyIconWindow, "handle", notifyIconWindow.GetType().BaseType)).ToInt32();
			}
		}
		#endregion

		#region private static void GetTrayWndRect(ref User32.RECT lpRect)
		private static void GetTrayWndRect(ref User32.RECT lpRect)
		{
			int defaultWidth = 150;
			int defaultHeight = 30;

			int hwndShellTray = User32.FindWindow("Shell_TrayWnd", null);

			if (hwndShellTray != 0)
			{	
				User32.GetWindowRect(hwndShellTray, ref lpRect);

				User32.EnumChildProc callback = new User32.EnumChildProc(FindTrayWindow);
				User32.EnumChildWindows(hwndShellTray, callback, ref lpRect);
			}
			else
			{
				// OK. Haven't found a thing. Provide a default rect based on the current work
				// area
			
				System.Drawing.Rectangle workArea = SystemInformation.WorkingArea;
				lpRect.right = workArea.Right;
				lpRect.bottom = workArea.Bottom;
				lpRect.left = workArea.Right - defaultWidth;
				lpRect.top  = workArea.Bottom - defaultHeight;
			}
		}
		#endregion

		#region private static bool FindTrayWindow(int hwnd, ref User32.RECT lParam)
		private static bool FindTrayWindow(int hwnd, ref User32.RECT lParam)
		{
			// Initialize a string to the max class name length. 
			string szClassName = new string(' ', 256);
			
			User32.GetClassName(hwnd, szClassName, szClassName.Length - 1);
			
			// Did we find the Main System Tray? If so, then get its size and keep going
			if (szClassName.StartsWith("TrayNotifyWnd"))
			{
				User32.GetWindowRect(hwnd, ref lParam);

				// We aren't done yet. Keep looking for children windows
				return true;
			}
			
			// Did we find the System Clock? If so, then adjust the size of the rectangle
			// so our rectangle will be between the outside edge of the tray and the edge
			// of the clock. In other words, our rectangle will not include the clock's 
			// rectangle. This works because the clock window will be found after the 
			// System Tray window. Once we have our new size can quit looking for remaining
			// children.
			if (szClassName.StartsWith("TrayClockWClass"))
			{
				User32.RECT rectClock = new User32.RECT();
				User32.GetWindowRect(hwnd, ref rectClock);

				// if clock is above system tray adjust accordingly
				if (rectClock.bottom < lParam.bottom - 5) // 10 = random fudge factor.
				{
					lParam.top = rectClock.bottom;
				}
				else
				{
					lParam.right = rectClock.left;
				}

				// Quit looking for children
				return false;
			}
			
			// We aren't done yet. Keep looking for children windows
			return true;
		}
		#endregion

		#region private static bool UseWindowAnimation()
		private static bool UseWindowAnimation()
		{
			User32.ANIMATIONINFO animationInfo = new User32.ANIMATIONINFO();
			
			animationInfo.cbSize = Marshal.SizeOf(animationInfo);

			User32.SystemParametersInfo(User32.SPI_GETANIMATION, animationInfo.cbSize, 
				ref animationInfo, 0);

			bool animateMinimize = (animationInfo.iMinAnimate != 0);
			return animateMinimize;
		}
		#endregion
	}
}
