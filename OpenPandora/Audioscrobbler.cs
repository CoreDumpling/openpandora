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
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Drawing;

namespace OpenPandora
{
	public class Audioscrobbler
	{
		#region private class Submittor
		private class Submittor
		{
			#region public Submittor(string url, string user, string response, string artist, string song, string album, string musicBrainzId, int length, WebProxy userProxy)
			public Submittor(string url, string user, string response, string artist, string song, string album, string musicBrainzId, int length, WebProxy userProxy)
			: this(url, user, response, artist, song, album, musicBrainzId, length, userProxy, 0)
			{
			}
			#endregion
			
			#region public Submittor(string url, string user, string response, string artist, string song, string album, string musicBrainzId, int length, WebProxy userProxy, int retry)
			public Submittor(string url, string user, string response, string artist, string song, string album, string musicBrainzId, int length, WebProxy userProxy, int retry)
			{
				this.url = url;
				this.user = user;
				this.response = response;
				this.artist = (retry == 0 ? HttpUtility.UrlEncode(artist) : artist);
				this.song = (retry == 0 ? HttpUtility.UrlEncode(song) : song);
				this.album = (retry == 0 ? HttpUtility.UrlEncode(album) : album);
				this.musicBrainzId = musicBrainzId;
				this.length = length;
				this.userProxy = userProxy;
				this.retry = retry;
			}
			#endregion
			
			#region public void Execute()
			public void Execute()
			{
				try
				{
					string submitPost = 
						"u=" + user + "&s=" + response + "&a[0]=" + artist + "&t[0]=" + song + "&b[0]=" + album + "&m[0]=" + musicBrainzId + "&l[0]=" + length + "&i[0]=" + DateTime.UtcNow.ToString("u").Replace("Z", "");
			
					byte[] submitPostByteArray = utf8Encoding.GetBytes(submitPost);
			
					HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
					
					if (userProxy != null)
					{			
						webRequest.Proxy = userProxy;					
					}
					
					webRequest.Method = "POST";
					webRequest.ContentLength = submitPostByteArray.Length;
					webRequest.ContentType = "application/x-www-form-urlencoded";
			
					Stream requestStream = webRequest.GetRequestStream();
					
					try
					{
						requestStream.Write(submitPostByteArray, 0, submitPostByteArray.Length);
					}
					finally
					{
						requestStream.Close();
					}
					
					Debug.WriteLine("AudioscrobblerPlugin.Submitting");
					Debug.WriteLine("	Artist - " + artist);
					Debug.WriteLine("	Song   - " + song);
					Debug.WriteLine("	Album  - " + album);
					Debug.WriteLine("	MB ID  - " + musicBrainzId);
					Debug.WriteLine("	Length - " + length);
					Debug.WriteLine("	Retry  - " + retry);
			
					WebResponse webResponse = webRequest.GetResponse();
			
					try
					{
						Stream stream = webResponse.GetResponseStream();
						StreamReader reader = new StreamReader(stream);
					
						string status = reader.ReadLine();
						
						Debug.WriteLine("AudioscrobblerPlugin.Submitted: " + status);
						
						int interval = 0;
						string intervalText = reader.ReadLine();
						
						if (intervalText != null && intervalText.StartsWith("INTERVAL"))
						{
							try
							{
								interval = Int32.Parse(intervalText.Remove(0, "INTERVAL ".Length));
							} catch {}
						}
						
						if (status.StartsWith("FAILED"))
						{
							Retry(interval);
						}
					}
					finally
					{
						webResponse.Close();
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine("AudioscrobblerPlugin.Submitting  : " + ex.Message);
					Debug.WriteLine(ex.StackTrace);
					
					Retry(1);
				}
			}
			#endregion
			
			//
			// Private methods
			//
			
			#region private void Retry(int interval)
			private void Retry(int interval)
			{
				if (retry > 3)
				{
					return;
				}
				
				++retry;

				Thread.Sleep(interval * retry * 1000);
				
				Debug.WriteLine("AudioscrobblerPlugin.Retrying");
				
				Submittor submittor = new Submittor(
					url,
					user,
					response,
					artist,
					song,
					album,
					musicBrainzId,
					length,
					userProxy,
					retry);
			
				Thread submitThread = new Thread(new ThreadStart(submittor.Execute));
				submitThread.Start();
			}
			#endregion
			
			//
			// Private data
			//
			
			private string url;
			private string user;
			private string response;
			private string artist;
			private string song;
			private string album;
			private string musicBrainzId;
			private int length;
			private WebProxy userProxy;
			private int retry;
		}
		#endregion
		
		//
		// Events
		//

		public event EventHandler Connected;

		//
		// Constructors
		//
		
		#region public Audioscrobbler(string clientId, string clientVersion, string user, string passwordMD5)
		public Audioscrobbler(string clientId, string clientVersion, string user, string passwordMD5)
		{
			this.connected = false;
			
			this.clientId = clientId;
			this.clientVersion = clientVersion;
			this.user = user;
			this.passwordMD5 = passwordMD5;		
			
			this.handshakeUrl = 
				"http://post.audioscrobbler.com/?hs=true&p=1.1&c=" + this.clientId + "&v=" + this.clientVersion + "&u=" + this.user;
		}
		#endregion
		
		//
		// Public properties
		//

		#region public string AvatarBitmap
		public Bitmap AvatarBitmap
		{
			get { return avatarBitmap; }
		}
		#endregion

		//
		// Public methods
		//
		
		#region public void Connect(string proxyHost, int proxyPort, string proxyUser, string proxyPassword)
		public void Connect(string proxyHost, int proxyPort, string proxyUser, string proxyPassword)
		{
			if (proxyPort != -1)
			{
				Debug.WriteLine("Proxy set: " + proxyHost + ":" + proxyPort);
				
				userProxy = new WebProxy(proxyHost, proxyPort);
				userProxy.Credentials = new NetworkCredential(proxyUser, proxyPassword);
			}
			else
			{
				userProxy = null;
			}
			
			Thread connectThread = new Thread(new ThreadStart(this.Connect));
			connectThread.IsBackground = true;
			connectThread.Start();
		}
		#endregion
		
		#region public void Submit(string artist, string song, string album, string musicBrainzId, int length)
		public void Submit(string artist, string song, string album, string musicBrainzId, int length)
		{
			if (!connected)
			{
				Debug.WriteLine("AudioscrobblerPlugin.Submit  : Not connected");
				return;
			}
			
			Submittor submittor = new Submittor(
				submitUrl,
				user,
				md5Response,
				artist,
				song,
				album,
				musicBrainzId,
				length,
				userProxy);
			
			Thread submitThread = new Thread(new ThreadStart(submittor.Execute));
			submitThread.Start();
		}
		#endregion
		
		#region public static string GetPasswordMD5(string password)
		public static string GetPasswordMD5(string password)
		{
			byte[] passwordHash = md5.ComputeHash(asciiEncoding.GetBytes(password));
			return BitConverter.ToString(passwordHash).Replace("-", "").ToLower();
		}
		#endregion
		
		//
		// Private data
		//
		
		#region private void Connect()
		private void Connect()
		{
			try
			{
				avatarBitmap = null;

				HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(handshakeUrl);
				
				if (userProxy != null)
				{			
					webRequest.Proxy = userProxy;					
				}
				
				WebResponse webResponse = webRequest.GetResponse();
				
				try
				{
					Stream stream = webResponse.GetResponseStream();
					StreamReader reader = new StreamReader(stream);
				
					string status = reader.ReadLine();
					
					Debug.WriteLine("AudioscrobblerPlugin.Connect : " + status);
				
					if (status == "UPTODATE" || status.StartsWith("UPDATE"))
					{
						// TODO: treat UPDATE
					
						challenge = reader.ReadLine();					
						submitUrl = reader.ReadLine();
						
						string helper = passwordMD5 + challenge;
						byte[] md5ResponseHash = md5.ComputeHash(asciiEncoding.GetBytes(helper));

						md5Response = BitConverter.ToString(md5ResponseHash).Replace("-", "").ToLower();
						
						this.connected = true;

						DownloadAvatar();
					}
					
					// TODO: treat interval
				}
				finally
				{
					webResponse.Close();
				}

				if (Connected != null)
				{
					Connected(this, new EventArgs());
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("AudioscrobblerPlugin.Connect : " + ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region private void DownloadAvatar()
		private void DownloadAvatar()
		{
			byte[] userHash = md5.ComputeHash(asciiEncoding.GetBytes(user));
			string avatar = BitConverter.ToString(userHash).Replace("-", "").ToLower();
			string avatarUrl = @"http://static.last.fm/avatar/" + avatar + ".jpg/";

			HttpWebRequest webAvatarRequest = (HttpWebRequest)HttpWebRequest.Create(avatarUrl);
				
			if (userProxy != null)
			{			
				webAvatarRequest.Proxy = userProxy;					
			}
			
			try
			{
				WebResponse webAvatarResponse = webAvatarRequest.GetResponse();

				try
				{
					Stream stream = webAvatarResponse.GetResponseStream();
					avatarBitmap = new Bitmap(stream);
				}
				finally
				{
					webAvatarResponse.Close();
				}
			}
			catch {}
		}
		#endregion

		//
		// Private data
		//
		
		private static ASCIIEncoding asciiEncoding = new ASCIIEncoding();
		private static UTF8Encoding utf8Encoding = new UTF8Encoding();
		private static MD5 md5 = new MD5CryptoServiceProvider();
		
		private bool connected;
		
		private string clientId;
		private string clientVersion;
		private string user;
		private string passwordMD5;
		
		private string handshakeUrl;
		private string challenge;
		private string submitUrl;
		private string md5Response;

		private Bitmap avatarBitmap;
		
		private WebProxy userProxy;
	}
}
