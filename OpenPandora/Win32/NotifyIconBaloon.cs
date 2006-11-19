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
	}
}
