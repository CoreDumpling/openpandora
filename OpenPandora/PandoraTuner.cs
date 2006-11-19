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
using System.Web;

namespace OpenPandora
{
	public class PandoraTuner
	{
		public PandoraTuner(string urlText)
		{
			this.containsLogin = false;
			this.containsLogout = false;
			this.containsUser = false;
			this.containsAds = false;
			this.containsPlay = false;
			this.containsPause = false;
			this.containsSkip = false;
			this.containsOpen = false;
			this.containsStation = false;
			this.containsCreateStation = false;
			this.containsDeleteStation = false;
			this.containsSharedStation = false;
			
			this.isPayingUser = false;
			
			try
			{
				this.functions = urlText.Remove(0, "javascript:".Length).Split(';');
			
				foreach (string function in functions)
				{
					if (function == string.Empty)
					{
						continue;
					}
				
					if (function.StartsWith("launchStationFromId"))
					{
						continue;
					}
				
					string[] parameters = function.Remove(0, "onTunerEvent(".Length).TrimEnd(')').Split(',');
				
					for (int i = 0; i < parameters.Length; i++)
					{
						parameters[i] = parameters[i].Trim(new char[] {' ', '\''});
					}
				
					// onCheckConnection
					if (parameters[0].Equals("hasLoggedIn"))
					{
						containsLogin = true;
					}
					else if (parameters[0].Equals("userAuthChanged"))
					{						
						if (parameters[1].Equals("null"))
						{
							containsLogout = true;
						}
						else
						{
							containsUser = true;
						
							userUrl = HttpUtility.UrlDecode(parameters[1]);
							userUrl = userUrl.TrimEnd(new char[] {'/'});
							user = userUrl.Substring(userUrl.LastIndexOf('/') + 1, userUrl.Length - userUrl.LastIndexOf('/') - 1);
						}
					}
					else if (parameters[0].Equals("adStateChange"))
					{
						containsAds = true;
					
						if (parameters.Length == 4 &&
						    parameters[1].Equals("0") &&
						    parameters[2].Equals("0") &&
						    parameters[3].Equals("0"))
						{
							isPayingUser = true;
						}
					}
					else if (parameters[0].Equals("stationChange"))
					{
						containsStation = true;
					
						stationCode = parameters[1];
					}
					else if (parameters[0].Equals("songPlayed"))
					{
						containsPlay = true;
					
						songID = parameters[1];
					}
					else if (parameters[0].Equals("onCheckConnection"))
					{
						Debug.WriteLine("Pandora checking connection");
					}
					else if (parameters[0].Equals("userInteraction"))
					{	
						if (parameters[1].Equals("pause"))
						{
							containsPause = true;
						}
						else if (parameters[1].Equals("skip"))
						{
							containsSkip = true;
						}
						else if (parameters[1].Equals("createStationConfirmation"))
						{
							containsCreateStation = true;
						}
						else if (parameters[1].Equals("deleteStation"))
						{
							containsDeleteStation = true;
						}
						else if (parameters[1].Equals("sharedStationSearch"))
						{
							containsSharedStation = true;
						}
						else if (parameters[1].Equals("cancelCreateStation"))
						{
							// TODO
						}
						else if (parameters[1].Equals("openContentWindow"))
						{
							containsOpen = true;
						}
						else if (parameters[1].Equals("menuRatePositive"))
						{
							// TODO
						}
						else if (parameters[1].Equals("menuRateNegative"))
						{
							// TODO
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(urlText);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		
		//
		// Public properties
		//
		
		#region public bool ContainsLogin
		public bool ContainsLogin
		{
			get { return containsLogin; }
		}
		#endregion

		#region public bool ContainsUser
		public bool ContainsUser
		{
			get { return containsUser; }
		}
		#endregion

		#region public bool ContainsAds
		public bool ContainsAds
		{
			get { return containsAds; }
		}
		#endregion
		
		#region public bool ContainsPlay
		public bool ContainsPlay
		{
			get { return containsPlay; }
		}
		#endregion

		#region public bool ContainsPause
		public bool ContainsPause
		{
			get { return containsPause; }
		}
		#endregion

		#region public bool ContainsSkip
		public bool ContainsSkip
		{
			get { return containsSkip; }
		}
		#endregion

		#region public bool ContainsOpen
		public bool ContainsOpen
		{
			get { return containsOpen; }
		}
		#endregion
		
		#region public bool IsPayingUser
		public bool IsPayingUser
		{
			get { return isPayingUser; }
		}
		#endregion

		#region public string UserUrl
		public string UserUrl
		{
			get { return userUrl; }
		}
		#endregion

		#region public string User
		public string User
		{
			get { return user; }
		}
		#endregion

		#region public bool ContainsStation
		public bool ContainsStation
		{
			get { return containsStation; }
		}
		#endregion

		#region public string SongID
		public string SongID
		{
			get { return songID; }
		}
		#endregion

		#region public string StationCode
		public string StationCode
		{
			get { return stationCode; }
		}
		#endregion

		#region public bool ContainsLogout
		public bool ContainsLogout
		{
			get { return containsLogout; }
		}
		#endregion
		
		#region public bool ContainsCreateStation
		public bool ContainsCreateStation
		{
			get { return this.containsCreateStation; }
		}
		#endregion
		
		#region public bool ContainsDeleteStation
		public bool ContainsDeleteStation
		{
			get { return this.containsDeleteStation; }
		}
		#endregion
		
		#region public bool ContainsSharedStation
		public bool ContainsSharedStation
		{
			get { return this.containsSharedStation; }
		}
		#endregion
		
		//
		// Public methods
		//
		
		#region public static bool IsTunerUrl(string urlText)
		public static bool IsTunerUrl(string urlText)
		{
			if (urlText.StartsWith("javascript:"))
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
		// Private data
		//
		
		private string[] functions;
		private bool containsLogin;
		private bool containsLogout;
		private bool containsUser;
		private bool containsAds;
		private string userUrl;
		private string user;
		private bool isPayingUser;
		private bool containsPlay;
		private bool containsPause;
		private bool containsSkip;
		private bool containsOpen;
		private bool containsStation;
		private string songID;
		private string stationCode;
		private bool containsCreateStation;
		private bool containsDeleteStation;
		private bool containsSharedStation;
	}
}
