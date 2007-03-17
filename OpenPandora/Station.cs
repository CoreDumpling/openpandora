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

namespace OpenPandora
{
	public class Station
	{
		//
		// Events
		//
		
		public static event EventHandler Loaded;
		
		//
		// Constructors
		// 
		
		#region public Station(string code, string name)
		public Station(string code, string name) : this(code, name, false)
		{
		}
		#endregion
		
		#region public Station(string code, string name, bool shared)
		public Station(string code, string name, bool shared) : this(code, name, false, false)
		{
		}
		#endregion

		#region public Station(string code, string name, bool shared)
		public Station(string code, string name, bool shared, bool quickMix)
		{
			this.code = code;
			this.name = name;
			this.shared = shared;
			this.quickMix = quickMix;

			if (!this.shared && this.quickMix)
			{
				this.name = "QuickMix";
			}
		}
		#endregion
		
		//
		// Public properties
		//
		
		#region public string Code
		public string Code
		{
			get { return code; }
		}
		#endregion

		#region public string Name
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		#endregion
		
		#region public bool Shared
		public bool Shared
		{
			get { return this.shared; }
		}
		#endregion

		#region public bool QuickMix
		public bool QuickMix
		{
			get { return quickMix; }
		}
		#endregion
		
		//
		// Public methods
		//
		
		#region public override string ToString()
		public override string ToString()
		{
			return "Station: name - " + name + ", code - " + code;
		}
		#endregion

		//
		// Private data
		//
		
		private string code;
		private string name;
		private bool shared;
		private bool quickMix;
	}
}
