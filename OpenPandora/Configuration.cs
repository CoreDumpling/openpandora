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
using System.Reflection;
using System.Xml.Serialization;

namespace OpenPandora
{
	[XmlRoot("openPandora")]
	public class Configuration
	{
		#region public enum ConfigurationItemType
		public enum ConfigurationItemType
		{
			MinimizeToTray,
			CloseButtonMinimizeToTray,
			CloseButtonVisible,
			MinimizeButtonVisible,
			SendToMessenger,
			OpenInDefaultBrowser,
			SilentBookmark,
			TitleTemplate,
			LastFmSubmit,
			LastFmUser,
			LastFmPassword,
			LastFmSubmitAutomatic,
			LastFmSubmitManual,
			LastFmSubmitSkipped,
			KeyboardMediaKeys,
			KeyboardVolumeKeys,
			PayingUser,
			KeepOnTop,
			PartyMode,
			NotificationWindow,
			NotificationWindowBalloon,
			ProxyHost,
			ProxyPort,
			ProxyUser,
			ProxyPassword,
			GlobalShortcuts,
			OffsetLeft,
			OffsetTop,
			Location,
			MiniPlayerLocation,
			SendToXfire,
            SendToG15,
			SendToSkype,
			NotificationLocation,
			NewVersion,
			NewVersionReport
		}
		#endregion

		#region public class ConfigurationItem
		public class ConfigurationItem
		{
			#region public ConfigurationItem(string name)
			public ConfigurationItem(string name) : this(name, null)
			{
			}
			#endregion

			#region public ConfigurationItem(string name, object value) : this(name, value, true)
			public ConfigurationItem(string name, object value) : this(name, value, true)
			{
				
			}
			#endregion

			#region public ConfigurationItem(string name, object value, bool enabled)
			public ConfigurationItem(string name, object value, bool enabled)
			{
				this.name = name;
				this.value = value;
				this.enabled = enabled;
			}
			#endregion

			//
			// Public properties
			//

			#region public string Name
			public string Name
			{
				get { return this.name; }
			}
			#endregion

			#region public object Value
			public object Value
			{
				get { return value; }
				set { this.value = value; }
			}
			#endregion

			#region public bool Enabled
			public bool Enabled
			{
				get { return enabled; }
				set { enabled = value; }
			}
			#endregion
			
			//
			// Internal methods
			//
			
			#region internal ConfigurationItem Clone()
			internal ConfigurationItem Clone()
			{
				return new ConfigurationItem(name, value, enabled);
			}
			#endregion

			//
			// Private data
			//

			private string name;
			private object value;
			private bool enabled;
		}
		#endregion

		#region public class ConfigurationItemCollection : DictionaryBase
		public class ConfigurationItemCollection : DictionaryBase
		{
			#region public ConfigurationItem this[ConfigurationItemType item]
			public ConfigurationItem this[ConfigurationItemType item]
			{
				get { return (ConfigurationItem)Dictionary[item]; }
				set { Dictionary[item] = value; }
			}
			#endregion
			
			#region public ConfigurationItemCollection Clone()
			public ConfigurationItemCollection Clone()
			{
				ConfigurationItemCollection cloned = new ConfigurationItemCollection();
				
				foreach (ConfigurationItemType itemType in Dictionary.Keys)
				{
					cloned[itemType] = this[itemType].Clone();
				}
				
				return cloned;
			}
			#endregion
		}
		#endregion
		
		private static readonly string FILE_NAME = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) + ".config";
		
		//
		// Public properties
		//

		[XmlElement("minimizeToTray")]
		#region public bool MinimizeToTray
		public bool MinimizeToTray
		{
			get { return (bool)items[ConfigurationItemType.MinimizeToTray].Value; }
			set
			{
				items[ConfigurationItemType.MinimizeToTray].Value = value;

				Refresh();
			}
		}
		#endregion

		[XmlElement("closeButtonMinimizeToTray")]
		#region public bool CloseButtonMinimizeToTray
		public bool CloseButtonMinimizeToTray
		{
			get { return (bool)items[ConfigurationItemType.CloseButtonMinimizeToTray].Value; }
			set
			{
				items[ConfigurationItemType.CloseButtonMinimizeToTray].Value = value;
				items[ConfigurationItemType.MinimizeButtonVisible].Value = !value;

				Refresh();
			}
		}
		#endregion

		[XmlElement("closeButtonVisible")]
		#region public bool CloseButtonVisible
		public bool CloseButtonVisible
		{
			get { return (bool)items[ConfigurationItemType.CloseButtonVisible].Value; }
			set
			{
				items[ConfigurationItemType.CloseButtonVisible].Value = value;
				
				if (!(bool)items[ConfigurationItemType.MinimizeButtonVisible].Value)
				{
					items[ConfigurationItemType.CloseButtonMinimizeToTray].Value = value;
				}
				
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("minimizeButtonVisible")]
		#region public bool MinimizeButtonVisible
		public bool MinimizeButtonVisible
		{
			get { return (bool)items[ConfigurationItemType.MinimizeButtonVisible].Value; }
			set
			{
				items[ConfigurationItemType.MinimizeButtonVisible].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("sendToMessenger")]
		#region public bool SendToMessenger
		public bool SendToMessenger
		{
			get { return (bool)items[ConfigurationItemType.SendToMessenger].Value; }
			set
			{
				items[ConfigurationItemType.SendToMessenger].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("openInDefaultBrowser")]
		#region public bool OpenInDefaultBrowser
		public bool OpenInDefaultBrowser
		{
			get { return (bool)items[ConfigurationItemType.OpenInDefaultBrowser].Value; }
			set
			{
				items[ConfigurationItemType.OpenInDefaultBrowser].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("silentBookmark")]
		#region public bool SilentBookmark
		public bool SilentBookmark
		{
			get { return (bool)items[ConfigurationItemType.SilentBookmark].Value; }
			set
			{
				items[ConfigurationItemType.SilentBookmark].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("titleTemplate")]
		#region public string TitleTemplate
		public string TitleTemplate
		{
			get { return items[ConfigurationItemType.TitleTemplate].Value as string; }
			set
			{
				items[ConfigurationItemType.TitleTemplate].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("lastFmSubmit")]
		#region public bool LastFmSubmit
		public bool LastFmSubmit
		{
			get { return (bool)items[ConfigurationItemType.LastFmSubmit].Value; }
			set
			{
				items[ConfigurationItemType.LastFmSubmit].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("lastFmUser")]
		#region public string LastFmUser
		public string LastFmUser
		{
			get { return items[ConfigurationItemType.LastFmUser].Value as string; }
			set
			{
				items[ConfigurationItemType.LastFmUser].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("lastFmPassword")]
		#region public string LastFmPassword
		public string LastFmPassword
		{
			get { return items[ConfigurationItemType.LastFmPassword].Value as string; }
			set
			{
				items[ConfigurationItemType.LastFmPassword].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("lastFmSubmitAutomatic")]
		#region public bool LastFmSubmitAutomatic
		public bool LastFmSubmitAutomatic
		{
			get { return (bool)items[ConfigurationItemType.LastFmSubmitAutomatic].Value; }
			set
			{
				items[ConfigurationItemType.LastFmSubmitAutomatic].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("lastFmSubmitManual")]
		#region public bool LastFmSubmitManual
		public bool LastFmSubmitManual
		{
			get { return (bool)items[ConfigurationItemType.LastFmSubmitManual].Value; }
			set
			{
				items[ConfigurationItemType.LastFmSubmitManual].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("lastFmSubmitSkipped")]
		#region public bool LastFmSubmitSkipped
		public bool LastFmSubmitSkipped
		{
			get { return (bool)items[ConfigurationItemType.LastFmSubmitSkipped].Value; }
			set
			{
				items[ConfigurationItemType.LastFmSubmitSkipped].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("keyboardMediaKeys")]
		#region public bool KeyboardMediaKeys
		public bool KeyboardMediaKeys
		{
			get { return (bool)items[ConfigurationItemType.KeyboardMediaKeys].Value; }
			set
			{
				items[ConfigurationItemType.KeyboardMediaKeys].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("keyboardVolumeKeys")]
		#region public bool KeyboardVolumeKeys
		public bool KeyboardVolumeKeys
		{
			get { return (bool)items[ConfigurationItemType.KeyboardVolumeKeys].Value; }
			set
			{
				items[ConfigurationItemType.KeyboardVolumeKeys].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("payingUser")]
		#region public bool PayingUser
		public bool PayingUser
		{
			get { return (bool)items[ConfigurationItemType.PayingUser].Value; }
			set
			{
				items[ConfigurationItemType.PayingUser].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("keepOnTop")]
		#region public bool KeepOnTop
		public bool KeepOnTop
		{
			get { return (bool)items[ConfigurationItemType.KeepOnTop].Value; }
			set
			{
				items[ConfigurationItemType.KeepOnTop].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("partyMode")]
		#region public bool PartyMode
		public bool PartyMode
		{
			get { return (bool)items[ConfigurationItemType.PartyMode].Value; }
			set
			{
				items[ConfigurationItemType.PartyMode].Value = value;
				Refresh();
			}
		}
		#endregion

		[XmlElement("notificationWindow")]
		#region public bool NotificationWindow
		public bool NotificationWindow
		{
			get { return (bool)items[ConfigurationItemType.NotificationWindow].Value; }
			set
			{
				items[ConfigurationItemType.NotificationWindow].Value = value;
				Refresh();
			}
		}
		#endregion

		[XmlElement("notificationWindowBalloon")]
		#region public bool NotificationWindowBalloon
		public bool NotificationWindowBalloon
		{
			get { return (bool)items[ConfigurationItemType.NotificationWindowBalloon].Value; }
			set
			{
				items[ConfigurationItemType.NotificationWindowBalloon].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("proxyHost")]
		#region public string ProxyHost
		public string ProxyHost
		{
			get { return items[ConfigurationItemType.ProxyHost].Value as string; }
			set
			{
				items[ConfigurationItemType.ProxyHost].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("proxyPort")]
		#region public int ProxyPort
		public int ProxyPort
		{
			get { return (int)items[ConfigurationItemType.ProxyPort].Value; }
			set
			{
				items[ConfigurationItemType.ProxyPort].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("proxyUser")]
		#region public string ProxyUser
		public string ProxyUser
		{
			get { return items[ConfigurationItemType.ProxyUser].Value as string; }
			set
			{
				items[ConfigurationItemType.ProxyUser].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("proxyPassword")]
		#region public string ProxyPassword
		public string ProxyPassword
		{
			get { return items[ConfigurationItemType.ProxyPassword].Value as string; }
			set
			{
				items[ConfigurationItemType.ProxyPassword].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("globalShortcuts")]
		#region public bool GlobalShortcuts
		public bool GlobalShortcuts
		{
			get { return (bool)items[ConfigurationItemType.GlobalShortcuts].Value; }
			set
			{
				items[ConfigurationItemType.GlobalShortcuts].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("offsetLeft")]
		#region public int OffsetLeft
		public int OffsetLeft
		{
			get { return (int)items[ConfigurationItemType.OffsetLeft].Value; }
			set
			{
				items[ConfigurationItemType.OffsetLeft].Value = value;
				Refresh();
			}
		}
		#endregion
		
		[XmlElement("offsetTop")]
		#region public int OffsetTop
		public int OffsetTop
		{
			get { return (int)items[ConfigurationItemType.OffsetTop].Value; }
			set
			{
				items[ConfigurationItemType.OffsetTop].Value = value;
				Refresh();
			}
		}
		#endregion

		[XmlElement("location")]
		#region public string Location
		public string Location
		{
			get { return items[ConfigurationItemType.Location].Value as string; }
			set
			{
				items[ConfigurationItemType.Location].Value = value;
				Refresh();
			}
		}
		#endregion

		[XmlElement("miniPlayerLocation")]
		#region public string MiniPlayerLocation
		public string MiniPlayerLocation
		{
			get { return items[ConfigurationItemType.MiniPlayerLocation].Value as string; }
			set
			{
				items[ConfigurationItemType.MiniPlayerLocation].Value = value;
				Refresh();
			}
		}
		#endregion

		[XmlElement("sendToXfire")]
		#region public bool SendToXfire
		public bool SendToXfire
		{
			get { return (bool)items[ConfigurationItemType.SendToXfire].Value; }
			set
			{
				items[ConfigurationItemType.SendToXfire].Value = value;
				Refresh();
			}
		}
		#endregion

        [XmlElement("sendToG15")]
        #region public bool SendToG15
        public bool SendToG15
        {
            get { return (bool)items[ConfigurationItemType.SendToG15].Value; }
            set
            {
                items[ConfigurationItemType.SendToG15].Value = value;
                Refresh();
            }
        }
        #endregion

		[XmlElement("sendToSkype")]
		#region public bool SendToSkype
		public bool SendToSkype
		{
			get { return (bool)items[ConfigurationItemType.SendToSkype].Value; }
			set
			{
				items[ConfigurationItemType.SendToSkype].Value = value;
				Refresh();
			}
		}
		#endregion

		[XmlElement("notificationLocation")]
		#region public string NotificationLocation
		public string NotificationLocation
		{
			get { return items[ConfigurationItemType.NotificationLocation].Value as string; }
			set
			{
				items[ConfigurationItemType.NotificationLocation].Value = value;
				Refresh();
			}
		}
		#endregion

		[XmlElement("newVersion")]
		#region public string NewVersion
		public string NewVersion
		{
			get { return items[ConfigurationItemType.NewVersion].Value as string; }
			set
			{
				items[ConfigurationItemType.NewVersion].Value = value;
				Refresh();
			}
		}
		#endregion

		[XmlElement("newVersionReport")]
		#region public bool NewVersionReport
		public bool NewVersionReport
		{
			get { return (bool)items[ConfigurationItemType.NewVersionReport].Value; }
			set
			{
				items[ConfigurationItemType.NewVersionReport].Value = value;
				Refresh();
			}
		}
		#endregion

		//
		// Public methods
		//

		#region public bool IsConfigurationItemEnabled(ConfigurationItemType item)
		public bool IsConfigurationItemEnabled(ConfigurationItemType item)
		{
			return items[item].Enabled;
		}
		#endregion

		#region public void Save()
		public void Save()
		{
			try
			{
				lock (this)
				{
					StreamWriter textWriter = new StreamWriter(FILE_NAME);

					try 
					{
						XmlSerializer serializer = new XmlSerializer(this.GetType());

						serializer.Serialize(textWriter, this);
					}
					finally
					{
						textWriter.Close();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region public Configuration Clone()
		public Configuration Clone()
		{
			Configuration cloned = new Configuration();
			cloned.items = this.items.Clone();
			
			return cloned;
		}
		#endregion
		
		#region public void Apply(Configuration configuration)
		public void Apply(Configuration configuration)
		{	
			this.items = configuration.items.Clone();
		}
		#endregion
		
		//
		// Private methods
		//

		#region public static Configuration Load()
		public static Configuration Load()
		{
			try
			{
				Debug.WriteLine("Configuration loading ...");
				
				StreamReader reader = new StreamReader(FILE_NAME);

				try
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
					
					Configuration configuration = (Configuration) serializer.Deserialize(reader);
					
					Debug.WriteLine("Configuration loaded");
					
					return configuration;
				}
				finally
				{
					reader.Close();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return new Configuration();
			}
		}
		#endregion

		#region private void Refresh()
		private void Refresh()
		{
			if ((bool)items[ConfigurationItemType.CloseButtonVisible].Value)
			{
				items[ConfigurationItemType.CloseButtonMinimizeToTray].Enabled = true;
				items[ConfigurationItemType.MinimizeButtonVisible].Enabled = true;
			}
			else
			{
				items[ConfigurationItemType.CloseButtonMinimizeToTray].Value = false;
				items[ConfigurationItemType.MinimizeButtonVisible].Value = false;

				items[ConfigurationItemType.CloseButtonMinimizeToTray].Enabled = false;
				items[ConfigurationItemType.MinimizeButtonVisible].Enabled = false;
			}

			if ((bool)items[ConfigurationItemType.CloseButtonMinimizeToTray].Value)
			{
				items[ConfigurationItemType.MinimizeButtonVisible].Value = false;

				items[ConfigurationItemType.MinimizeButtonVisible].Enabled = false;
			}
			else if ((bool)items[ConfigurationItemType.CloseButtonVisible].Value)
			{
				items[ConfigurationItemType.MinimizeButtonVisible].Enabled = true;
			}
			else
			{
				items[ConfigurationItemType.MinimizeButtonVisible].Enabled = false;
			}
			
			//
			// Last.fm
			
			if ((bool)items[ConfigurationItemType.LastFmSubmit].Value)
			{
				items[ConfigurationItemType.LastFmUser].Enabled = true;
				items[ConfigurationItemType.LastFmPassword].Enabled = true;
				items[ConfigurationItemType.LastFmSubmitAutomatic].Enabled = true;
				items[ConfigurationItemType.LastFmSubmitManual].Enabled = true;
				items[ConfigurationItemType.LastFmSubmitSkipped].Enabled = true;
			}
			else
			{
				items[ConfigurationItemType.LastFmUser].Enabled = false;
				items[ConfigurationItemType.LastFmPassword].Enabled = false;
				items[ConfigurationItemType.LastFmSubmitAutomatic].Enabled = false;
				items[ConfigurationItemType.LastFmSubmitManual].Enabled = false;
				items[ConfigurationItemType.LastFmSubmitSkipped].Enabled = false;
			}
			
			if (!(bool)items[ConfigurationItemType.LastFmSubmitAutomatic].Value)
			{
				items[ConfigurationItemType.LastFmSubmitSkipped].Enabled = false;
			}

			if (!(bool)items[ConfigurationItemType.NotificationWindow].Value)
			{
				items[ConfigurationItemType.NotificationWindowBalloon].Enabled = false;
			}
			else
			{
				items[ConfigurationItemType.NotificationWindowBalloon].Enabled = true;
			}
		}
		#endregion

		#region private static ConfigurationItemCollection GetDefaultConfiguration()
		private static ConfigurationItemCollection GetDefaultConfiguration()
		{
			ConfigurationItemCollection items = new ConfigurationItemCollection();

			items[ConfigurationItemType.CloseButtonVisible] = 
				new ConfigurationItem("Show Close button", true);
			items[ConfigurationItemType.CloseButtonMinimizeToTray] = 
				new ConfigurationItem("Close button minimize to tray", false);
			items[ConfigurationItemType.MinimizeToTray] = 
				new ConfigurationItem("Minimize to tray", false);
			items[ConfigurationItemType.MinimizeButtonVisible] = 
				new ConfigurationItem("Show Minimize button", true, true);
			items[ConfigurationItemType.SendToMessenger] = 
				new ConfigurationItem("Send song information to Messenger", true, true);
			items[ConfigurationItemType.OpenInDefaultBrowser] = 
				new ConfigurationItem("Open links in default browser", true, true);
			items[ConfigurationItemType.SilentBookmark] = 
				new ConfigurationItem("Do not open browser on song bookmark", true, true);
			items[ConfigurationItemType.TitleTemplate] = 
				new ConfigurationItem("Title template", "%s - %a", true);
			items[ConfigurationItemType.LastFmUser] = 
				new ConfigurationItem("Last.FM User", string.Empty, true);
			items[ConfigurationItemType.LastFmPassword] = 
				new ConfigurationItem("Last.FM Password", string.Empty, true);
			items[ConfigurationItemType.LastFmSubmit] = 
				new ConfigurationItem("Last.FM submit", false, true);
			items[ConfigurationItemType.LastFmSubmitAutomatic] = 
				new ConfigurationItem("Last.FM Automatic submit", true, true);
			items[ConfigurationItemType.LastFmSubmitSkipped] = 
				new ConfigurationItem("Last.FM submit skipped", false, true);
			items[ConfigurationItemType.LastFmSubmitManual] = 
				new ConfigurationItem("Last.FM Manual submit", false, true);
			items[ConfigurationItemType.KeyboardMediaKeys] = 
				new ConfigurationItem("Listen to keyboard media keys", false, true);
			items[ConfigurationItemType.KeyboardVolumeKeys] = 
				new ConfigurationItem("Listen to keyboard volume keys", false, true);
			items[ConfigurationItemType.PayingUser] = 
				new ConfigurationItem("Is Pandora paying user", false, false);
			items[ConfigurationItemType.KeepOnTop] = 
				new ConfigurationItem("Keep on top of other windows", false, true);
			items[ConfigurationItemType.PartyMode] = 
				new ConfigurationItem("Party Mode (prevent from stop playing if left alone)", false, true);
			items[ConfigurationItemType.NotificationWindow] = 
				new ConfigurationItem("Show notification window", false, true);
			items[ConfigurationItemType.NotificationWindowBalloon] = 
				new ConfigurationItem("Show notification window bolloon", false, false);
			items[ConfigurationItemType.ProxyHost] = 
				new ConfigurationItem("Proxy host name", string.Empty, true);
			items[ConfigurationItemType.ProxyPort] = 
				new ConfigurationItem("Proxy host port", -1, true);
			items[ConfigurationItemType.ProxyUser] = 
				new ConfigurationItem("Proxy host user name", string.Empty, true);
			items[ConfigurationItemType.ProxyPassword] = 
				new ConfigurationItem("Proxy host user password", string.Empty, true);
			items[ConfigurationItemType.GlobalShortcuts] = 
				new ConfigurationItem("Listen to global shortcuts", false, true);
			items[ConfigurationItemType.OffsetLeft] = 
				new ConfigurationItem("Radio left offset", 131, true);
			items[ConfigurationItemType.OffsetTop] = 
				new ConfigurationItem("Radio top offset", 306, true);
			items[ConfigurationItemType.Location] = 
				new ConfigurationItem("Window location", string.Empty, true);
			items[ConfigurationItemType.MiniPlayerLocation] = 
				new ConfigurationItem("MiniPlayer location", string.Empty, true);
			items[ConfigurationItemType.SendToXfire] = 
				new ConfigurationItem("Send song info to Xfire", false, true);
            items[ConfigurationItemType.SendToG15] =
                new ConfigurationItem("Send song info to Logitech G15", false, true);
			items[ConfigurationItemType.SendToSkype] = 
				new ConfigurationItem("Send song info to Skype", false, true);
			items[ConfigurationItemType.NotificationLocation] = 
				new ConfigurationItem("Notification window location", string.Empty, true);
			items[ConfigurationItemType.NewVersion] = 
				new ConfigurationItem("Last run version", "0.0.0.0", true);
			items[ConfigurationItemType.NewVersionReport] = 
				new ConfigurationItem("Report about new version", true, true);

			return items;
		}
		#endregion

		//
		// Private data
		//

		private ConfigurationItemCollection items = GetDefaultConfiguration();
	}
}
