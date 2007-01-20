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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace OpenPandora
{
	public class About : BaseForm
	{
		private System.ComponentModel.Container components = null;

		//
		// Constructors
		//

		#region public About()
		public About()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Text = "About  " + this.Text + "  " + Manager.CurrentVersion;
			this.StartPosition = FormStartPosition.CenterScreen;

			Label lblAbout = new Label();

			lblAbout.Dock = DockStyle.Fill;
			lblAbout.TextAlign = ContentAlignment.MiddleCenter;
			lblAbout.BackColor = Color.GhostWhite;
			lblAbout.Text = 
				"OpenPandora is a windows frontend for the Pandora™ music descovery service." + Environment.NewLine +
				Environment.NewLine +
				"Developed by Eitan Pogrebizsky" + Environment.NewLine +
				Environment.NewLine +
				"Individual contributors: Dirk Rettschlag" + Environment.NewLine +
				Environment.NewLine + Environment.NewLine +
				"Copyright (C) 2006 Eitan Pogrebizsky";

			this.Controls.Add(lblAbout);
		}
		#endregion

		//
		// Generated code
		//

		#region protected override void Dispose(bool disposing)
		protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(About));
			// 
			// About
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(272, 176);
			this.DockPadding.Bottom = 1;
			this.DockPadding.Left = 1;
			this.DockPadding.Right = 1;
			this.DockPadding.Top = 19;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "About";
			this.Text = "Open Pandora";

		}
		#endregion
	}
}
