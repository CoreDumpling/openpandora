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

namespace OpenPandora
{
	public class Xfire
	{
		private static readonly string XFIRE_PROTOCOL = @"xfire:status?text=";

		//
		// Constructor
		//

		#region private Xfire()
		private Xfire()
		{
		}
		#endregion

		//
		// Public methods
		//

		#region public static void SetMessage(string message)
		public static void SetMessage(string message)
		{
			string url = XFIRE_PROTOCOL + message;
			Shell32.ShellExecute(0, "Open", url, "", Application.StartupPath, 1);
		}
		#endregion

	}
}
