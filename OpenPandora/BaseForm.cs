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
using System.Diagnostics;

namespace OpenPandora
{
	public class BaseForm : System.Windows.Forms.Form
	{
		private static int TITLE_HEIGHT = 19;
		private static int BORDER_WIDTH = 1;
		private System.Windows.Forms.Button btnClose;

		private System.ComponentModel.Container components = null;

		#region public BaseForm()
		public BaseForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.DockPadding.All = BORDER_WIDTH;
			this.DockPadding.Top = TITLE_HEIGHT;

			btnClose.ForeColor = Player.BACKGROUND_COLOR;
			btnClose.Location = new Point(this.Width - btnClose.Width - 2, 2);

			try
			{
				Graphics g = this.CreateGraphics();
				double xRatio = 96.0 / g.DpiX;
				double yRatio = 96.0 / g.DpiY;
				btnClose.Width = (int)(btnClose.Width * xRatio);
				btnClose.Height = (int)(btnClose.Height * yRatio);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region public BaseForm(string titleText)
		public BaseForm(string titleText) : this()
		{
			this.Text = titleText;
		}
		#endregion

		#region protected override void Dispose(bool disposing)
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BaseForm));
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.ForeColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(326, 0);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(16, 16);
			this.btnClose.TabIndex = 0;
			this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
			this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
			// 
			// BaseForm
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(51)), ((System.Byte)(102)), ((System.Byte)(153)));
			this.ClientSize = new System.Drawing.Size(350, 314);
			this.ControlBox = false;
			this.Controls.Add(this.btnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "BaseForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "BaseForm";
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BaseForm_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.BaseForm_Closing);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BaseForm_MouseUp);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.BaseForm_Paint);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BaseForm_MouseMove);
			this.ResumeLayout(false);

		}
		#endregion

		//
		// Public properties
		//

		#region public bool HideOnClose
		public bool HideOnClose
		{
			get { return this.hideOnClose; }
			set { this.hideOnClose = value; }
		}
		#endregion

		//
		// Events
		//

		#region private void BaseForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		private void BaseForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (hideOnClose)
			{
				e.Cancel = true;
				this.Hide();
			}
		}
		#endregion

		#region private void BaseForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		private void BaseForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			
			Brush backgroundBrush = new SolidBrush(Color.FromArgb(49, 49, 49));
			g.FillRectangle(backgroundBrush, 0, 0, this.Width, TITLE_HEIGHT);

			Pen outterPen = new Pen(Color.FromArgb(192, 192, 192));
			
			g.DrawPolygon(outterPen, 
				new Point[] {
								new Point(0, this.Height - 1),
								new Point(0, 0),
								new Point(this.Width - 1, 0),
								new Point(this.Width - 1, this.Height - 1)});
			
			Pen innerPen = new Pen(Color.FromArgb(128, 128, 128));
			g.DrawPolygon(innerPen, 
				new Point[] {
								new Point(1, TITLE_HEIGHT - 1),
								new Point(1, 1),
								new Point(this.Width - 2, 1),
								new Point(this.Width - 2, TITLE_HEIGHT - 1)});

			g.DrawString(this.Text, new Font("Tahoma", 8), Brushes.WhiteSmoke, new PointF(3, 3));
		}
		#endregion

		#region private void BaseForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		private void BaseForm_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.mouseOffset = new Point(-e.X, -e.Y);
			}
		}
		#endregion

		#region private void BaseForm_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		private void BaseForm_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.Cursor = Cursors.Hand;
				Point mousePosition = Control.MousePosition;
				
				if (Math.Abs(e.X + this.mouseOffset.X) > 1 ||
					Math.Abs(e.Y + this.mouseOffset.Y) > 1)
				{
					mousePosition.Offset(this.mouseOffset.X, this.mouseOffset.Y);
					this.Location = mousePosition;
				}
			}
		}
		#endregion

		#region private void BaseForm_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		private void BaseForm_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.mouseOffset = new Point(0, 0);
			this.Cursor = Cursors.Default;
		}
		#endregion

		#region private void btnClose_Click(object sender, System.EventArgs e)
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		#endregion

		#region private void btnClose_MouseEnter(object sender, System.EventArgs e)
		private void btnClose_MouseEnter(object sender, System.EventArgs e)
		{
			btnClose.ForeColor = Player.PANDORA_COLOR;
		}
		#endregion

		#region private void btnClose_MouseLeave(object sender, System.EventArgs e)
		private void btnClose_MouseLeave(object sender, System.EventArgs e)
		{
			btnClose.ForeColor = Player.BACKGROUND_COLOR;
		}
		#endregion

		//
		// Private data
		//

		private Point mouseOffset;
		private bool hideOnClose = false;
	}
}
