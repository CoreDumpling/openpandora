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

namespace OpenPandora
{
	public class Song
	{
		#region public Song(string id, string name, string artist, string url, string artUrl)
		public Song(string id, string name, string artist, string url, string artUrl)
			: this(id, name, artist)
		{
			this.url = url;
			this.artUrl = artUrl;
		}
		#endregion

		#region public Song(string id, string name, string artist)
		public Song(string id, string name, string artist)
		{
			this.id = id;
			this.name = name;
			this.artist = artist;
		}
		#endregion

		//
		// Public properties
		//

		#region public string Name
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}
		#endregion

		#region public string Artist
		public string Artist
		{
			get { return this.artist; }
			set { this.artist = value; }
		}
		#endregion

		#region public string Url
		public string Url
		{
			get { return this.url; }
			set { this.url = value; }
		}
		#endregion

		#region public string ArtUrl
		public string ArtUrl
		{
			get { return this.artUrl; }
			set { this.artUrl = value; }
		}
		#endregion

		//
		// Public methods
		//

		#region public override int GetHashCode()
		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}
		#endregion

		#region public override bool Equals(object o)
		public override bool Equals(object o)
		{
			if (o is Song && this.id == ((Song)o).id)
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

		private string name;
		private string artist;
		private string id;
		private string url;
		private string artUrl;
	}
}
