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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;

namespace OpenPandora
{
	public class StationLoader
	{
		#region private StationLoader(string authToken, string listenerID, string proxyHost, int proxyPort, string proxyUser, string proxyPassword)
		private StationLoader(string authToken, string listenerID, string proxyHost, int proxyPort, string proxyUser, string proxyPassword)
		{
			this.authToken = authToken;
			this.listenerID = listenerID;
			this.proxyHost = proxyHost;
			this.proxyPort = proxyPort;
			this.proxyUser = proxyUser;
			this.proxyPassword = proxyPassword;
		}
		#endregion
			
		//
		// Events
		//
		
		public static event EventHandler Loaded;
		
		//
		// Public properties
		//
		
		#region public static Station[] Stations
		public static Station[] Stations
		{
			get { return stations; }
		}
		#endregion
		
		//
		// Public methods
		//
		
		#region public static void Load(string AuthToken, string listenerID, string proxyHost, int proxyPort, string proxyUser, string proxyPassword)
		public static void Load(string authToken, string listenerID, string proxyHost, int proxyPort, string proxyUser, string proxyPassword)
		{
			Debug.WriteLine("Stations initiate loading ...");
			
			StationLoader loader = new StationLoader(authToken, listenerID, proxyHost, proxyPort, proxyUser, proxyPassword);
			
			Thread loaderThread = new Thread(new ThreadStart(loader.Load));
			loaderThread.IsBackground = true;
			loaderThread.Start();
		}
		#endregion
		
		//
		// Private methods
		//
		
		#region private void Load()
		private void Load()
		{
			try
			{
				Debug.WriteLine("Stations loading ...");
			
				/*ArrayList stationArray = new ArrayList();
			
				string filename = "http://feeds.pandora.com/feeds/people/" + user + "/stations.xml";
				HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(filename);
			
				if (proxyPort != -1)
				{
					Debug.WriteLine("Proxy settings: " + proxyHost + ":" + proxyPort);
					
					WebProxy userProxy = new WebProxy(proxyHost, proxyPort);
					userProxy.Credentials = new NetworkCredential(proxyUser, proxyPassword);
			
					webRequest.Proxy = userProxy;
					
					Debug.WriteLine("Proxy set: " + ((WebProxy)webRequest.Proxy).Address);
				}
				else
				{
					Debug.WriteLine("Proxy used default");
				}

				WebResponse webResponse = webRequest.GetResponse();
				Stream stream = webResponse.GetResponseStream();

				try
				{
					XmlDocument xml = new XmlDocument();
					xml.Load(stream);

					XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
					nsmgr.AddNamespace("pandora", "http://www.pandora.com/rss/1.0/modules/pandora/");

					XmlNodeList itemNodes = xml.DocumentElement.SelectNodes("./channel/item");

					foreach (XmlNode itemNode in itemNodes)
					{
						string stationName = itemNode.SelectSingleNode("description").InnerText;
						string stationCode = itemNode.SelectSingleNode("pandora:stationCode", nsmgr).InnerText;
						bool isQuickMix = false;
						
						if (stationCode.StartsWith("qm"))
						{
							isQuickMix = true;
							stationCode = stationCode.Replace("qm", "");
						}
						else
						{
							stationCode = stationCode.Replace("sh", "");
						}
						
						bool isSharedStation = true;
						XmlNode listenerLinkNode = itemNode.SelectSingleNode("pandora:listenerLink", nsmgr);

						if (listenerLinkNode != null)
						{
							string listenerUrl = listenerLinkNode.InnerText.TrimEnd(new char[] {'/'});
							string listenerUser = listenerUrl.Substring(listenerUrl.LastIndexOf('/') + 1, listenerUrl.Length - listenerUrl.LastIndexOf('/') - 1);
							
							if (listenerUser.Equals(user))
							{
								isSharedStation = false;
							}
						}

						stationArray.Add(new Station(stationCode, stationName, isSharedStation, isQuickMix));
					}
				
					Debug.WriteLine("Stations loaded");
				}
				finally
				{
					stream.Close();
				}

				stations = (Station[])stationArray.ToArray(typeof(Station));
			
				if (Loaded != null)
				{
					Loaded(this, new EventArgs());
				}*/
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
		
		private static Station[] stations = new Station[0];
		private string authToken;
		private string listenerID;
		private string proxyHost;
		private int proxyPort;
		private string proxyUser;
		private string proxyPassword;
	}
}
