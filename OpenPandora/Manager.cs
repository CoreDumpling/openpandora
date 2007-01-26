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
using System.Diagnostics;
using System.Threading;

namespace OpenPandora
{
	public class Manager
	{
		private static readonly string loginUrl = @"http://openpandora.googlepages.com/login.htm";
		
		#region static Manager()
		static Manager()
		{
			currentVersion = Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion;
		}
		#endregion
		
		//
		// Public properties
		//
		
		#region public static string CurrentVersion
		public static string CurrentVersion
		{
			get { return currentVersion; }
		}
		#endregion
		
		//
		// Public methods
		//
		
		#region public static void Login(string user)
		public static void Login(string user)
		{
			try
			{
				webForm = new WebForm();
				
				object missing = System.Type.Missing;
				object url = CreateLoginUrl(user);
				
				webForm.Browser.Navigate2(ref url, ref missing, ref missing, ref missing, ref missing);
				
				new Timer(new TimerCallback(FinalizeLogin), null, 1000*15, -1);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region public static bool IsWindowsXpOrHigher()
		public static bool IsWindowsXpOrHigher()
		{
			System.OperatingSystem os = System.Environment.OSVersion;

			if (os.Platform == PlatformID.Win32NT && 
				((os.Version.Major == 5 && os.Version.Minor >= 1) || 
				os.Version.Major > 5))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion
		
		//
		// Private methods
		//
		
		#region private static string CreateLoginUrl(string user)
		private static string CreateLoginUrl(string user)
		{
			return loginUrl + "?utm_source=version&utm_medium=" + currentVersion + "&utm_term=user&utm_content=" + user;
		}
		#endregion
		
		#region private static void FinalizeLogin(object state)
		private static void FinalizeLogin(object state)
		{
			try
			{
				webForm.Close();
				webForm = null;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion
		
		//
		// Private data
		//
		
		public static WebForm webForm;
		private static string currentVersion;
	}
}
