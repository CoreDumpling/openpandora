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
	public class Messenger
	{
		public struct COPYDATASTRUCT
		{
			public int dwData;
			public int cbData;
			public int lpData;
		}
		
		public enum Category
		{
			Office,
			Music
		}
		
		//
		// Constructor
		//
		
		#region private Messenger()
		private Messenger()
		{
		}
		#endregion
		
		//
		// Public methods
		//

		#region public static void SetMessage(bool enable, Category category, string message)
		public static void SetMessage(bool enable, Category category, string message)
		{
			string buffer = "\\0" + category.ToString() + "\\0" + (enable ? "1" : "0") + "\\0{0}\\0" + message + "\\0\\0\\0\\0\0";
			int handle = 0;

			messengerData.dwData = 0x0547;
			messengerData.lpData = VarPtr(buffer);
			messengerData.cbData = buffer.Length * 2;

			handle = User32.FindWindowEx(0, handle, "MsnMsgrUIManager", null);
			
			if (handle > 0)
			{
				User32.SendMessage(handle, WindowsMessageCode.WM_COPYDATA, 0, VarPtr(messengerData));
			}
		}
		#endregion
		
		//
		// Private methods
		//
		
		#region private static int VarPtr(object e)
		private static int VarPtr(object e)
		{
			GCHandle gcHandle = GCHandle.Alloc(e, GCHandleType.Pinned);
			int gcAddr = gcHandle.AddrOfPinnedObject().ToInt32();
			gcHandle.Free();
			
			return gcAddr;
		}
		#endregion
		
		//
		// Private data
		//
		
		private static COPYDATASTRUCT messengerData;
	}
}
