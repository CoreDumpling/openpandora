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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using AxSHDocVw;

namespace OpenPandora
{
	public class WebForm : System.Windows.Forms.Form
	{
		private AxSHDocVw.AxWebBrowser browser;
		private System.ComponentModel.Container components = null;

		public WebForm()
		{
			InitializeComponent();
		}

		public AxWebBrowser Browser
		{
			get { return browser; }
		}

		protected override void Dispose( bool disposing )
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WebForm));
			this.browser = new AxSHDocVw.AxWebBrowser();
			((System.ComponentModel.ISupportInitialize)(this.Browser)).BeginInit();
			this.SuspendLayout();
			// 
			// browser
			// 
			this.Browser.Enabled = true;
			this.Browser.Location = new System.Drawing.Point(0, 0);
			this.Browser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("browser.OcxState")));
			this.Browser.Size = new System.Drawing.Size(300, 150);
			this.Browser.TabIndex = 0;
			// 
			// WebForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 149);
			this.Controls.Add(this.Browser);
			this.Name = "WebForm";
			this.Text = "WebForm";
			((System.ComponentModel.ISupportInitialize)(this.Browser)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
