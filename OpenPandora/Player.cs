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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Data;
using AxSHDocVw;
using mshtml;

namespace OpenPandora
{
	public class Player : System.Windows.Forms.Form
	{
		const int CS_DROPSHADOW = 0x20000;

		public static readonly Color BACKGROUND_COLOR = Color.FromArgb(49, 49, 49);
		public static readonly Color PANDORA_COLOR = Color.FromArgb(51, 102, 153);

		private static readonly string DEFAULT_TITLE = "OpenPandora";
		private static readonly string PAUSED = "[Paused]";
		private static readonly string STARTUP_URL = @"http://www.pandora.com/?cmd=mini";
		private static readonly string STARTUP_URL2 = @"http://openpandora.googlepages.com/pandoraevents6.htm";
		public static readonly string CONTROLLER_URL = @"http://openpandora.googlepages.com/controller.xml";
		private static readonly string WEBSITE_URL = @"http://openpandora.googlepages.com/";
		private static readonly string INSTALLER_URL = @"http://openpandora.googlepages.com/openpandora.zip";
		private static readonly string INSTALLER_BETA_URL = @"http://openpandora.googlepages.com/openpandorabeta.zip";
		
		private static readonly int MEMORYTIMER_DELAY = 30000;
		private static readonly int MEMORYTIMER_PAUSE = 300000;

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.PictureBox pictureBoxFill;
		private AxSHDocVw.AxWebBrowser browser;
		private System.Windows.Forms.Panel panelBrowser;
		private System.Windows.Forms.PictureBox pictureBoxTitle;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ToolTip toolTip;
		private SettingsView settingsView;
		private BaseForm settingsForm;
		private AxSHDocVw.AxWebBrowser browser2;
		private System.Windows.Forms.Button btnMinimize;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private OpenPandora.Windows.Forms.TaskbarNotifier taskbarNotifier;
		private Skype skype;
		
		//
		// Constructor
		//

		#region public Player()
		public Player()
		{			
			InitializeComponent();

			this.SuspendLayout();

			try
			{
				aboutForm = new About();
				aboutForm.HideOnClose = true;
			
				this.VScroll = false;
				this.HScroll = false;
			
				this.pandora16 = new Bitmap(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType().Namespace + ".Pandora16.bmp"));
				this.pandora16.MakeTransparent(Color.Black);
			
				//
				// Initialize
				//
			
				InitializeMenus();
				InitializeTimers();
				InitializeNotifyIcon();
				InitializeTaskbarNotifier();
			
				//
				// Colors
				//

				btnClose.ForeColor = BACKGROUND_COLOR;
				btnMinimize.ForeColor = BACKGROUND_COLOR;

				//
				// Tool tip
				//

				toolTip.SetToolTip(this.btnMinimize, "Minimize");
				toolTip.SetToolTip(this.btnClose, "Close");
			
				//
				// Settings
				//
			
				settingsView = new SettingsView();
				settingsView.Player = this;
				settingsView.Dock = DockStyle.Fill;

				settingsForm = new BaseForm("Settings");
				settingsForm.HideOnClose = true;
				settingsForm.Size = new Size(settingsView.Width + 2, settingsView.Height + 19);
				settingsForm.Controls.Add(settingsView);
				settingsForm.Show();
				settingsForm.Hide();
				
				//
				// Configuration
				//

				this.configuration = Configuration.Load();
				ApplyConfiguration(this.configuration, false);
			
				isPayingUser = this.configuration.PayingUser;
						
				//
				// Window
				//
			
				this.Size = new Size(640, 268);

				this.windowHeight = this.Height;
				this.windowWidth = this.Width;
			
				if (configuration.Location != string.Empty)
				{
					try
					{
						string[] coordinates = configuration.Location.Split(new char[] {','});

						this.Location = new Point(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
						Debug.WriteLine(ex.StackTrace);
					}
				}

				MoveToWorkingArea();

				//
				// Browser
				//

				browser.Silent = true;
				browser.Size = new Size(this.Size.Width + configuration.OffsetLeft + 40, this.Size.Height + configuration.OffsetTop + 40);

				this.Width = pictureBoxFill.Image.Width;
			
				//
				// Windows hook
				//
			
				try
				{
					windowsHook = new WindowsHook();
					windowsHook.KeyDown += new KeyEventHandler(this.GlobalKeyDown);
					windowsHook.KeyUp += new KeyEventHandler(this.GlobalKeyUp);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					Debug.WriteLine(ex.StackTrace);
				}
			
				//
				// Stations
				//
			
				StationLoader.Loaded += new EventHandler(this.StationsLoaded);
			
				//
				// Title
				//
			
				title = DEFAULT_TITLE;
				this.Text = title;
				notifyIcon.Text = title;
				pictureBoxTitle.Refresh();

				//
				// Refresh browser, somehow form Load event start before ctor finishes
				//

				Debug.WriteLine("Loading radio ...");

				try
				{
					object url2 = STARTUP_URL2;
					browser2.Navigate2(ref url2, ref missing, ref missing, ref missing, ref missing);
			
					object url = STARTUP_URL;
					browser.Navigate2(ref url, ref missing, ref missing, ref missing, ref missing);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					Debug.WriteLine(ex.StackTrace);
				}
			}
			finally
			{
				this.ResumeLayout();

				this.panelBrowser.Left = 0;
				this.pictureBoxFill.Left = 0;
			}
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
				if (components != null) 
				{
					components.Dispose();
				}

				if (notifyIcon != null)
				{
					notifyIcon.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region protected override CreateParams CreateParams
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ClassStyle = CS_DROPSHADOW;
				return cp;
			}
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Player));
			this.pictureBoxFill = new System.Windows.Forms.PictureBox();
			this.browser = new AxSHDocVw.AxWebBrowser();
			this.panelBrowser = new System.Windows.Forms.Panel();
			this.browser2 = new AxSHDocVw.AxWebBrowser();
			this.pictureBoxTitle = new System.Windows.Forms.PictureBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnMinimize = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.browser)).BeginInit();
			this.panelBrowser.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.browser2)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBoxFill
			// 
			this.pictureBoxFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBoxFill.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxFill.Image")));
			this.pictureBoxFill.Location = new System.Drawing.Point(0, 18);
			this.pictureBoxFill.Name = "pictureBoxFill";
			this.pictureBoxFill.Size = new System.Drawing.Size(650, 300);
			this.pictureBoxFill.TabIndex = 2;
			this.pictureBoxFill.TabStop = false;
			this.pictureBoxFill.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFill_MouseUp);
			this.pictureBoxFill.DoubleClick += new System.EventHandler(this.pictureBoxFill_DoubleClick);
			this.pictureBoxFill.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFill_MouseMove);
			this.pictureBoxFill.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFill_MouseDown);
			// 
			// browser
			// 
			this.browser.ContainingControl = this;
			this.browser.Enabled = true;
			this.browser.Location = new System.Drawing.Point(-2, -2);
			this.browser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("browser.OcxState")));
			this.browser.Size = new System.Drawing.Size(800, 300);
			this.browser.TabIndex = 3;
			this.browser.NewWindow3 += new AxSHDocVw.DWebBrowserEvents2_NewWindow3EventHandler(this.browser_NewWindow3);
			this.browser.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.browser_DocumentComplete);
			this.browser.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.browser_BeforeNavigate2);
			// 
			// panelBrowser
			// 
			this.panelBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.panelBrowser.Controls.Add(this.browser);
			this.panelBrowser.Controls.Add(this.browser2);
			this.panelBrowser.Location = new System.Drawing.Point(0, 18);
			this.panelBrowser.Name = "panelBrowser";
			this.panelBrowser.Size = new System.Drawing.Size(800, 464);
			this.panelBrowser.TabIndex = 4;
			// 
			// browser2
			// 
			this.browser2.ContainingControl = this;
			this.browser2.Enabled = true;
			this.browser2.Location = new System.Drawing.Point(96, 72);
			this.browser2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("browser2.OcxState")));
			this.browser2.Size = new System.Drawing.Size(312, 163);
			this.browser2.TabIndex = 10;
			this.browser2.StatusTextChange += new AxSHDocVw.DWebBrowserEvents2_StatusTextChangeEventHandler(this.browser2_StatusTextChange);
			this.browser2.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.browser2_DocumentComplete);
			this.browser2.BeforeNavigate2 += new AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.browser2_BeforeNavigate2);
			// 
			// pictureBoxTitle
			// 
			this.pictureBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBoxTitle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.pictureBoxTitle.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxTitle.Name = "pictureBoxTitle";
			this.pictureBoxTitle.Size = new System.Drawing.Size(640, 18);
			this.pictureBoxTitle.TabIndex = 6;
			this.pictureBoxTitle.TabStop = false;
			this.pictureBoxTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxTitle_Paint);
			this.pictureBoxTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFill_MouseUp);
			this.pictureBoxTitle.DoubleClick += new System.EventHandler(this.pictureBoxFill_DoubleClick);
			this.pictureBoxTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFill_MouseMove);
			this.pictureBoxTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxFill_MouseDown);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.BackColor = System.Drawing.Color.White;
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.ForeColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(622, 2);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(16, 16);
			this.btnClose.TabIndex = 7;
			this.btnClose.TabStop = false;
			this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
			this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
			// 
			// btnMinimize
			// 
			this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMinimize.BackColor = System.Drawing.Color.White;
			this.btnMinimize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMinimize.BackgroundImage")));
			this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMinimize.ForeColor = System.Drawing.Color.White;
			this.btnMinimize.Location = new System.Drawing.Point(606, 2);
			this.btnMinimize.Name = "btnMinimize";
			this.btnMinimize.Size = new System.Drawing.Size(16, 16);
			this.btnMinimize.TabIndex = 8;
			this.btnMinimize.TabStop = false;
			this.btnMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
			this.btnMinimize.MouseEnter += new System.EventHandler(this.btnMinimize_MouseEnter);
			this.btnMinimize.MouseLeave += new System.EventHandler(this.btnMinimize_MouseLeave);
			// 
			// Player
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(639, 267);
			this.Controls.Add(this.btnMinimize);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.pictureBoxTitle);
			this.Controls.Add(this.pictureBoxFill);
			this.Controls.Add(this.panelBrowser);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Player";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Open Pandora";
			this.Resize += new System.EventHandler(this.formPandora_Resize);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.formPandora_Closing);
			this.Load += new System.EventHandler(this.formPandora_Load);
			this.Activated += new System.EventHandler(this.formPandora_Activated);
			((System.ComponentModel.ISupportInitialize)(this.browser)).EndInit();
			this.panelBrowser.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.browser2)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		
		//
		// Window
		//

		#region protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.Alt | Keys.F4))
			{
				btnClose_Click(this, new EventArgs());

				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion

		//
		// notifyIcon event handlers
		//

		#region private void notifyIcon_DoubleClick(object Sender, EventArgs e)
		private void notifyIcon_DoubleClick(object Sender, EventArgs e)
		{
			if (this.Visible)
			{
				MinimizeToTray();
			}
			else
			{
				RestoreFromTray();
			}
		}
		#endregion

		#region private void notifyIcon_Click(object Sender, EventArgs e)
		private void notifyIcon_Click(object Sender, EventArgs e)
		{
			if (this.Visible)
			{
				this.Activate();
			}
		}
		#endregion

		//
		// formPandora event handlers
		//

		#region private void formPandora_Load(object sender, System.EventArgs e)
		private void formPandora_Load(object sender, System.EventArgs e)
		{
			/*Debug.WriteLine("Loading radio ...");

			try
			{
				object url2 = STARTUP_URL2;
				browser2.Navigate2(ref url2, ref missing, ref missing, ref missing, ref missing);
			
				object url = STARTUP_URL;
				browser.Navigate2(ref url, ref missing, ref missing, ref missing, ref missing);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}*/
		}
		#endregion

		#region private void formPandora_Resize(object sender, System.EventArgs e)
		private void formPandora_Resize(object sender, System.EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized &&
				this.Visible)
			{
				MinimizeToTray();
			}
			else if (this.WindowState != FormWindowState.Minimized &&
				!this.Visible)
			{
				RestoreFromTray();
			}
		}
		#endregion

		#region private void formPandora_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		private void formPandora_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();

			if (menuMiniPlayer.Checked)
			{
				configuration.MiniPlayerLocation = this.Location.X + "," + this.Location.Y;
			}
			else
			{
				configuration.Location = this.Location.X + "," + this.Location.Y;
			}

			configuration.Save();
			
			try
			{
				if (windowsHook != null)
				{
					windowsHook.Dispose();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region private void formPandora_Activated(object sender, System.EventArgs e)
		private void formPandora_Activated(object sender, System.EventArgs e)
		{
			if (loaded)
			{
				panelBrowser.Show();
				panelBrowser.BringToFront();
				settingsView.BringToFront();
			}
		}
		#endregion

		//
		// pictureBoxFill event handlers
		//

		#region private void pictureBoxFill_DoubleClick(object sender, System.EventArgs e)
		private void pictureBoxFill_DoubleClick(object sender, System.EventArgs e)
		{
			if (menuMiniPlayer.Checked)
			{
				menuMiniPlayer_Click(sender, e);
			}
			else
			{
				MinimizeToTray();
			}
		}
		#endregion

		#region private void pictureBoxFill_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		private void pictureBoxFill_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.mouseOffset = new Point(-e.X, -e.Y);
			}
		}
		#endregion

		#region private void pictureBoxFill_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		private void pictureBoxFill_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
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

				if (menuMiniPlayer.Checked)
				{
					configuration.MiniPlayerLocation = this.Location.X + "," + this.Location.Y;

					if (taskbarNotifier != null && taskbarNotifier.Visible)
					{
						taskbarNotifier.Hide();
					}
				}
				else
				{
					configuration.Location = this.Location.X + "," + this.Location.Y;
				}
			}
		}
		#endregion

		#region private void pictureBoxFill_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		private void pictureBoxFill_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.mouseOffset = new Point(0, 0);
			this.Cursor = Cursors.Default;
		}
		#endregion

		//
		// pictureBoxTitle event handlers
		//

		#region private void pictureBoxTitle_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		private void pictureBoxTitle_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics titleGraphics = e.Graphics;

			Pen outterPen = new Pen(Color.FromArgb(192, 192, 192));
			Pen innerPen = new Pen(Color.FromArgb(128, 128, 128));
			Pen fillerPen = new Pen(BACKGROUND_COLOR);

			titleGraphics.DrawLines(outterPen, 
				new Point[] {
								new Point(0, pictureBoxTitle.Height - 1),
								new Point(0, 0),
								new Point(pictureBoxTitle.Width - 1, 0),
								new Point(pictureBoxTitle.Width - 1, pictureBoxTitle.Height - 1)});

			titleGraphics.DrawLines(innerPen, 
				new Point[] {
								new Point(1, pictureBoxTitle.Height - 1),
								new Point(1, 1),
								new Point(pictureBoxTitle.Width - 2, 1),
								new Point(pictureBoxTitle.Width - 2, pictureBoxTitle.Height - 1)});

			if (hideTitle)
			{
				hideTitle = false;
				titleTimer.Interval = 120;
				titleGraphics.DrawRectangle(fillerPen, 0, 0, pictureBoxTitle.Width, pictureBoxTitle.Height);
				titleTimer.Start();
				return;
			}

			string text;
				
			if (message != string.Empty)
			{
				text = message;
			}
			else
			{
				text = title;
			}

			if (text == null)
			{
				return;
			}

			SizeF textSize = titleGraphics.MeasureString(text, new Font("Tahoma", 8));

			if (pictureBoxTitle.Width - 3 * 2 < textSize.Width)
			{
				text = text + "    ";

				if (titleTimer.Enabled == false)
				{
					titlePosition = 3;
					titleTimer.Interval = 4000;
					titleTimer.Start();
				}
				else if (titlePosition == 3)
				{
					titleTimer.Interval = 4000;
				}
			}
			else
			{
				titlePosition = 3;
				titleTimer.Stop();
			}

			titleGraphics.DrawString(text, new Font("Tahoma", 8), Brushes.WhiteSmoke, new PointF(titlePosition, 3));
		}
		#endregion

		//
		// browser event handlers
		//

		#region private void browser_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		private void browser_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			try
			{
				if (!loaded)
				{
					Debug.WriteLine("Radio: timer");
					browserTimer.Start();					
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region private void browser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		private void browser_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{
			try
			{	
				string urlText = HttpUtility.UrlDecode(e.uRL as string);

				//Debug.WriteLine("Loading ... " + urlText);

				try
				{
					if (!loaded && browserTimer != null)
					{
						Debug.WriteLine("Radio: timer");
						browserTimer.Interval = 10000;
						browserTimer.Start();
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					Debug.WriteLine("Url: " + urlText);
					Debug.WriteLine(ex.StackTrace);
				}

				if (PandoraTuner.IsTunerUrl(urlText))
				{
					PandoraTuner tuner = new PandoraTuner(urlText);
					
					if (tuner.ContainsCreateStation ||
						tuner.ContainsDeleteStation ||
						tuner.ContainsLogin ||
						tuner.ContainsOpen ||
						tuner.ContainsPause ||
						tuner.ContainsSharedStation ||
						tuner.ContainsSkip ||
						tuner.ContainsStation)
					{
						continuesPlayCounter = 0;
					}
					
					if ((loginTime - DateTime.Now).TotalHours == 6)
					{
						loginTime = DateTime.Now;
						Manager.Login(user);
					}
					
					if (tuner.ContainsLogin)
					{
						loaded = true;
						loginTime = DateTime.Now;
						
						if (pandora == null)
						{
							pandora = new Pandora(browser);
						}
						
						panelBrowser.BringToFront();
						settingsView.BringToFront();
					}
					
					if (tuner.ContainsLogout)
					{
						RestartPlayer();
					}
					
					if (tuner.ContainsUser)
					{
						userUrl = tuner.UserUrl;
						user = tuner.User;

						Manager.Login(user);						
						LoadStations();
					}
					
					if (tuner.ContainsCreateStation)
					{
						LoadStations();
					}
					
					if (!tuner.ContainsDeleteStation && 
						deleteStation)
					{
						if (tuner.ContainsStation)
						{
							LoadStations();
						}
						
						deleteStation = false;
					}
					
					if (tuner.ContainsDeleteStation)
					{
						deleteStation = true;
					}
					
					if (!tuner.ContainsSharedStation && 
						sharedStation)
					{
						if (tuner.ContainsStation)
						{
							LoadStations();
						}
						
						sharedStation = false;
					}
					
					if (tuner.ContainsSharedStation)
					{
						sharedStation = true;
					}
					
					if (tuner.ContainsAds)
					{	
						if (isPayingUser != tuner.IsPayingUser)
						{
							isPayingUser = tuner.IsPayingUser;
							
							configuration.PayingUser = tuner.IsPayingUser;
							configuration.Save();
						}
					}
					
					if (tuner.ContainsStation)
					{
						Debug.WriteLine("Changed station to " + tuner.StationCode);
						song = new Song(string.Empty, string.Empty, string.Empty);
					}
					
					if (tuner.ContainsOpen)
					{
						lastBookmark = DateTime.Now;
					}
					
					if (tuner.ContainsSkip)
					{
						Debug.WriteLine("Skip");
					}
					
					if (tuner.ContainsPlay)
					{
						/*Debug.WriteLine("Play");
						
						OnPlayStart();
						
						++continuesPlayCounter;
								
						memoryTimer.Interval = MEMORYTIMER_DELAY;
						memoryTimer.Start();
								
						if (taskbarNotifier.Visible)
							taskbarNotifier.Hide();

						if (!paused)
						{
							playedLength += (int)(DateTime.Now - playedStartTime).TotalSeconds;
									
							if ((!tuner.ContainsSkip && !tuner.ContainsStation) ||
							    configuration.LastFmSubmitSkipped)
							{
								SubmitSongToLastFM(song.Artist, song.Name, playedLength);
							}
									
							playedLength = 0;
						}
								
						playedStartTime = DateTime.Now;
						
						this.song = new Song(tuner.SongID, string.Empty, string.Empty);
						
						if (nextSong != null)
						{
							song.Name = nextSong.Name;
							song.Artist = nextSong.Artist;
							
							nextSong = null;
						}
								
						refreshMessenger = !paused;
						refreshXfire = !paused;
						refreshSkype = !paused;
								
						paused = false;*/
					}
					else if (tuner.ContainsPause)
					{
						/*Debug.WriteLine("Pause");
						this.menuPlayerPlayPause.Text = "Play";
								
						paused = true;
						refreshMessenger = false;
						refreshXfire = false;
						refreshSkype = false;
									
						playedLength += (int)(DateTime.Now - playedStartTime).TotalSeconds;
									
						memoryTimer.Interval = MEMORYTIMER_PAUSE;
						memoryTimer.Start();
									
						return;*/
					}
					
					if (configuration.PartyMode && continuesPlayCounter > 30)
					{
						Debug.WriteLine("Party !!!!!!!!!!!!!!!!!");
						pandora.NextTrack();

						continuesPlayCounter = 0;
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
		
		#region private void browser_NewWindow3(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow3Event e)
		private void browser_NewWindow3(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow3Event e)
		{
			try
			{
				e.cancel = true;
				Shell32.ShellExecute(0, "Open", e.bstrUrl, "", Application.StartupPath, 1);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine("Url: " + e.bstrUrl);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region private void browser2_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		private void browser2_BeforeNavigate2(object sender, AxSHDocVw.DWebBrowserEvents2_BeforeNavigate2Event e)
		{	
			string url = (string)e.uRL;
			
			try
			{	
				if (url.StartsWith("javascript:"))
				{
					string[] scripts = url.Split(';');

					foreach (string script in scripts)
					{
						if (script.LastIndexOf("SongPlayed") > 0)
						{
							Debug.WriteLine("Play");

							const string SONG_NAME_MARK = "songName:unescape";
							const string ARTIST_NAME_MARK = "artistName:unescape";
							const string SONG_URL_MARK = "songURL:unescape";
							const string ART_URL_MARK = "artURL:unescape";
							const string END_MARK = "}";
						
							int songMark = script.IndexOf(SONG_NAME_MARK) + SONG_NAME_MARK.Length;
							int songMarkLength = script.LastIndexOf(ARTIST_NAME_MARK) - songMark;
							int artistMark = script.IndexOf(ARTIST_NAME_MARK) + ARTIST_NAME_MARK.Length;
							int artistMarkLength = script.LastIndexOf(SONG_URL_MARK) - artistMark;
							int songUrlMark = script.IndexOf(SONG_URL_MARK) + SONG_URL_MARK.Length;
							int songUrlMarkLength = script.LastIndexOf(ART_URL_MARK) - songUrlMark;
							int artUrlMark = script.IndexOf(ART_URL_MARK) + ART_URL_MARK.Length;
							int artUrlMarkLength = script.LastIndexOf(END_MARK) - artUrlMark;

							string songNamePart = script.Substring(songMark, songMarkLength);
							string artistNamePart = script.Substring(artistMark, artistMarkLength);
							string songUrlPart = script.Substring(songUrlMark, songUrlMarkLength);
							string artUrlPart = script.Substring(artUrlMark, artUrlMarkLength);
											
							string songName = songNamePart.Substring(songNamePart.IndexOf("'") + 1, songNamePart.LastIndexOf("'") - songNamePart.IndexOf("'") - 1);
							string artistName = artistNamePart.Substring(artistNamePart.IndexOf("'") + 1, artistNamePart.LastIndexOf("'") - artistNamePart.IndexOf("'") - 1);
							string songUrl = songUrlPart.Substring(songNamePart.IndexOf("'") + 1, songUrlPart.LastIndexOf("'") - songUrlPart.IndexOf("'") - 1);
							string artUrl = artUrlPart.Substring(artUrlPart.IndexOf("'") + 1, artUrlPart.LastIndexOf("'") - artUrlPart.IndexOf("'") - 1);
					
							songName = HttpUtility.UrlDecode(songName.Replace("%25%32%37", "%27"));
							artistName = HttpUtility.UrlDecode(artistName.Replace("%25%32%37", "%27"));
							songUrl = HttpUtility.UrlDecode(songUrl);
							artUrl = HttpUtility.UrlDecode(artUrl);

							OnPlayStart();
						
							++continuesPlayCounter;
								
							memoryTimer.Interval = MEMORYTIMER_DELAY;
							memoryTimer.Start();
								
							if (taskbarNotifier != null && taskbarNotifier.Visible)
							{
								taskbarNotifier.Hide();
							}

							if (!paused)
							{
								playedLength += (int)(DateTime.Now - playedStartTime).TotalSeconds;
									
								SubmitSongToLastFM(song.Artist, song.Name, playedLength);
									
								playedLength = 0;
							}
								
							playedStartTime = DateTime.Now;
								
							refreshMessenger = !paused;
							refreshXfire = !paused;
							refreshSkype = !paused;

							paused = false;

							song.Name = songName;
							song.Artist = artistName;
							song.Url = songUrl;
							song.ArtUrl = artUrl;
					
							Debug.WriteLine(song.Name + " ~by~ " + song.Artist);
					
							submittedToLastFm = false;
							menuLastFmSubmit.Enabled = true;

							RefreshPlayer(true);
						}
						else if (script.LastIndexOf("SongPaused") > 0)
						{
							Debug.WriteLine("Pause");
							this.menuPlayerPlayPause.Text = "Play";
								
							paused = true;
							refreshMessenger = false;
							refreshXfire = false;
							refreshSkype = false;
									
							playedLength += (int)(DateTime.Now - playedStartTime).TotalSeconds;
									
							memoryTimer.Interval = MEMORYTIMER_PAUSE;
							memoryTimer.Start();
						}
						else if (script.LastIndexOf("StationPlayed") > 0)
						{
							const string STATION_NAME_MARK = "stationName:unescape";
							const string STATION_ID_MARK = "stationId:unescape";
							const string IS_SHARED_MARK = "isShared:";

							int stationNameMark = script.IndexOf(STATION_NAME_MARK) + STATION_NAME_MARK.Length;
							int stationNameMarkLength = script.LastIndexOf(STATION_ID_MARK) - stationNameMark;
							int stationIdMark = script.IndexOf(STATION_ID_MARK) + STATION_ID_MARK.Length;
							int stationIdMarkLength = script.LastIndexOf(IS_SHARED_MARK) - stationIdMark;

							string stationNamePart = script.Substring(stationNameMark, stationNameMarkLength);
							string stationIdPart = script.Substring(stationIdMark, stationIdMarkLength);
											
							string stationName = stationNamePart.Substring(stationNamePart.IndexOf("'") + 1, stationNamePart.LastIndexOf("'") - stationNamePart.IndexOf("'") - 1);
							string stationIdText = stationIdPart.Substring(stationIdPart.IndexOf("'") + 1, stationIdPart.LastIndexOf("'") - stationIdPart.IndexOf("'") - 1);
					
							stationName = HttpUtility.UrlDecode(stationName.Replace("%25%32%37", "%27"));

							if (currentStationCode != stationIdText)
							{
								Station currentStation = null;
						
								foreach (Station station in stations)
								{
									if (station.Code.Equals(stationIdText))
									{
										currentStation = station;
										break;
									}
								}
						
								if (currentStation != null)
								{
									foreach (MenuItem menuItem in menuPlayerStations.MenuItems)
									{
										if (menuItem.Text.Equals(currentStation.Name))
										{
											title = "Playing ... " + currentStation.Name;
											ShowMessage(title);
											menuItem.Checked = true;
										}
										else
										{
											menuItem.Checked = false;
										}
									}
								}

								currentStationCode = stationIdText;
							}
						}
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
		
		#region private void browser2_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		private void browser2_DocumentComplete(object sender, AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEvent e)
		{
			try
			{
				IHTMLDocument2 document = (IHTMLDocument2)browser2.Document;
				IHTMLElement element = (IHTMLElement)document.all.item("version", 0);

				latestVersion = element.innerText;
				
				int currentVersionNumber = int.Parse(Manager.CurrentVersion.Replace(".", ""));
				int latestVersionNumber = int.Parse(latestVersion.Replace(".", ""));
				int lastNewVersionNumber = int.Parse(configuration.NewVersion.Replace(".", ""));

				if (latestVersionNumber > lastNewVersionNumber)
				{
					configuration.NewVersion = latestVersion;
					configuration.NewVersionReport = true;
					configuration.Save();
				}

				latestVersionNumber = latestVersionNumber * (int)Math.Pow(10, 4 - latestVersion.Split(new char[] {'.'}).Length);
			
				if (currentVersionNumber == latestVersionNumber)
				{
					isLatestVersion = true;
				}
				else if (currentVersionNumber > latestVersionNumber)
				{
					isBetaVersion = true;

					IHTMLElement betaElement = (IHTMLElement)document.all.item("betaVersion", 0);

					if (betaElement != null)
					{
						int latestBetaVersionNumber = int.Parse(betaElement.innerText.Replace(".", ""));

						if (latestBetaVersionNumber > currentVersionNumber)
						{
							if (MessageBox.Show(
								"New BETA " + betaElement.innerText + " is available." + Environment.NewLine + Environment.NewLine +
								"Do you want to download the new BETA now?", "OpenPandora", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
							{
								Shell32.ShellExecute(0, "Open", INSTALLER_BETA_URL, "", Application.StartupPath, 1);
								this.Close();
							}
						}
					}
				}
				else if (configuration.NewVersionReport)
				{
					if (MessageBox.Show(
						"New version " + latestVersion + " is available." + Environment.NewLine + Environment.NewLine +
						"Do you want to download the new version now?", "OpenPandora", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						Shell32.ShellExecute(0, "Open", INSTALLER_URL, "", Application.StartupPath, 1);
						this.Close();
					}
					else
					{
						if (MessageBox.Show(
							"New version " + latestVersion + " is available." + Environment.NewLine + Environment.NewLine +
							"Remind you later?", "OpenPandora", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						{
							configuration.NewVersionReport = false;
							configuration.Save();
						}
					}
				}
				
				loaded2 = true;

				//browser2.Refresh2();
				
				RefreshPlayer(false);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		//
		// btnClose event handlers
		//

		#region private void btnClose_MouseEnter(object sender, System.EventArgs e)
		private void btnClose_MouseEnter(object sender, System.EventArgs e)
		{
			btnClose.ForeColor = PANDORA_COLOR;
		}
		#endregion

		#region private void btnClose_MouseLeave(object sender, System.EventArgs e)
		private void btnClose_MouseLeave(object sender, System.EventArgs e)
		{
			btnClose.ForeColor = BACKGROUND_COLOR;
		}
		#endregion

		#region private void btnClose_Click(object sender, System.EventArgs e)
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			if (this.configuration.CloseButtonMinimizeToTray ||
				!this.configuration.CloseButtonVisible)
			{
				MinimizeToTray();
			}
			else
			{
				if (MessageBox.Show(
					"Are you sure that you want to exit?", 
					DEFAULT_TITLE, 
					MessageBoxButtons.YesNo, 
					MessageBoxIcon.Question, 
					MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				{
					this.Close();
				}
			}
		}
		#endregion

		//
		// btnMinimize event handlers
		//

		#region private void btnMinimize_MouseEnter(object sender, System.EventArgs e)
		private void btnMinimize_MouseEnter(object sender, System.EventArgs e)
		{
			btnMinimize.ForeColor = PANDORA_COLOR;
		}
		#endregion

		#region private void btnMinimize_MouseLeave(object sender, System.EventArgs e)
		private void btnMinimize_MouseLeave(object sender, System.EventArgs e)
		{
			btnMinimize.ForeColor = BACKGROUND_COLOR;
		}
		#endregion

		#region private void btnMinimize_Click(object sender, System.EventArgs e)
		private void btnMinimize_Click(object sender, System.EventArgs e)
		{
			MinimizeToTray();
		}
		#endregion

		//
		// Timers event handlers
		//
		
		#region private void messageTimer_Tick(object sender, System.EventArgs e)
		private void messageTimer_Tick(object sender, System.EventArgs e)
		{
			messageTimer.Stop();
			message = string.Empty;
			pictureBoxTitle.Refresh();
		}
		#endregion
		
		#region private void browserTimer_Tick(object sender, System.EventArgs e)
		private void browserTimer_Tick(object sender, System.EventArgs e)
		{
			try
			{
				browserTimer.Stop();
				browserTimer.Interval = 500;

				IHTMLDocument2 document = (IHTMLDocument2)browser.Document;

				if (document == null)
				{
					Debug.WriteLine("Radio: document not loaded");
					browserRefreshTimer.Interval = 2000;
					browserRefreshTimer.Start();

					return;
				}

				IHTMLElement element = (IHTMLElement)document.all.item("radio", 0);

				if (element == null)
				{
					Debug.WriteLine("Radio: missing");

					browserRefreshTimer.Interval = 30000;
					browserRefreshTimer.Start();

					return;
				}
				
				radioSize.Height = element.offsetHeight;
				radioSize.Width = element.offsetWidth;

				int left = element.offsetLeft;
				int top = element.offsetTop;
				IHTMLElement parent = element.offsetParent;

				while (parent != null)
				{
					left += parent.offsetLeft;
					top += parent.offsetTop;
					parent = parent.offsetParent;
				}

				int x = -2 - left;
				int y = -2 - top;

				browser.Size = new Size(element.offsetWidth + left + 40, element.offsetHeight + top + 40);
				browser.Location = new Point(x, y);
					
				Debug.WriteLine("Radio: left " + left + ", top " + top);

				panelBrowser.BringToFront();
				settingsView.BringToFront();

				// Navigate to events page
				//object url2 = STARTUP_URL2;
				//browser2.Navigate2(ref url2, ref missing, ref missing, ref missing, ref missing);
					
				configuration.OffsetLeft = left;
				configuration.OffsetTop = top;
				configuration.Save();
					
				loaded = true;	
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region private void browserRefreshTimer_Tick(object sender, System.EventArgs e)
		private void browserRefreshTimer_Tick(object sender, System.EventArgs e)
		{
			try
			{
				browserRefreshTimer.Stop();

				if (!loaded && browser.Document != null)
				{
					Debug.WriteLine("Loading radio ...");

					try
					{
						/*object url2 = STARTUP_URL2;
						browser2.Navigate2(ref url2, ref missing, ref missing, ref missing, ref missing);
			
						object url = STARTUP_URL;
						browser.Navigate2(ref url, ref missing, ref missing, ref missing, ref missing);*/

						browser2.Refresh2(ref missing);
						browser.Refresh2(ref missing);
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
						Debug.WriteLine(ex.StackTrace);
					}
				
					if (pandora != null)
					{
						pandora.Refresh();
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

		#region private void memoryTimer_Tick(object sender, System.EventArgs e)
		private void memoryTimer_Tick(object sender, System.EventArgs e)
		{
			try
			{
				Debug.WriteLine("Memory Purge");

				if (memoryTimer.Interval == MEMORYTIMER_DELAY)
				{
					memoryTimer.Stop();
				}
				
				GC.Collect();
				GC.WaitForPendingFinalizers();

				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					Kernel32.SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region private void titleTimer_Tick(object sender, System.EventArgs e)
		private void titleTimer_Tick(object sender, System.EventArgs e)
		{
			try
			{
				if (hideTitle)
				{
					pictureBoxTitle.Refresh();
					return;
				}

				titleTimer.Interval = 100;

				string text;
				
				if (message != string.Empty)
				{
					text = message;
				}
				else
				{
					text = title;
				}

				if (text == null)
				{
					return;
				}

				Graphics titleGraphics = pictureBoxTitle.CreateGraphics();
				SizeF textSize = titleGraphics.MeasureString(text, new Font("Tahoma", 8));

				if ((titlePosition + textSize.Width + 4 >=  pictureBoxTitle.Width) && titleBounce == false)
				{
					titleDirection = -1;
				}
				else 
				{
					titleBounce = true;
				}
					
				if (titlePosition <= 2 && titleBounce == true)
				{
					titleDirection = 1;
				}
				else
				{
					titleBounce = false;
				}

				if (titleDirection == 1)
				{
					titlePosition = 5;
					titleTimer.Interval = 2500;
					titleBounce = false;
					hideTitle = true;
				}
				else
				{
					titlePosition = titlePosition + 2 * titleDirection;
					pictureBoxTitle.Refresh();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion
		
		//
		// Menu event handlers
		//

		#region private void menuExit_Click(object sender, System.EventArgs e)
		private void menuExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		#endregion
		
		#region private void menuAbout_Click(object sender, System.EventArgs e)
		private void menuAbout_Click(object sender, System.EventArgs e)
		{
			aboutForm.Show();

			/*if (!this.Visible)
			{
				RestoreFromTray();
			}
			
			ShowMessage(DEFAULT_TITLE + " " + Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion);
			*/
		}
		#endregion

		#region private void menuRefresh_Click(object sender, System.EventArgs e)
		private void menuRefresh_Click(object sender, System.EventArgs e)
		{
			if (true)/*MessageBox.Show(
					"Do you want to refresh Pandora?", 
					DEFAULT_TITLE, 
					MessageBoxButtons.YesNo, 
					MessageBoxIcon.Question, 
					MessageBoxDefaultButton.Button2) == DialogResult.Yes)*/
			{
				Debug.WriteLine("Refreshing ...");
				
				try
				{
					/*RestartPlayer();
				
					this.panelBrowser.SendToBack();
					this.pictureBoxFill.BringToFront();*/
				
					/*object url2 = STARTUP_URL2;
					browser2.Navigate2(ref url2, ref missing, ref missing, ref missing, ref missing);
				
					object url = STARTUP_URL;
					browser.Navigate2(ref url, ref missing, ref missing, ref missing, ref missing);*/
				
					browser2.Refresh2(ref missing);
					//browser.Refresh2(ref missing);
				
					/*if (pandora != null)
					{
						pandora.Refresh();
					}*/
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					Debug.WriteLine(ex.StackTrace);

					Debug.WriteLine("Reloading ...");

					try
					{
						object url2 = STARTUP_URL2;
						browser2.Navigate2(ref url2, ref missing, ref missing, ref missing, ref missing);
					}
					catch (Exception ex2)
					{
						Debug.WriteLine(ex2.Message);
						Debug.WriteLine(ex2.StackTrace);
					}
				}
			}
		}
		#endregion
		
		#region private void menuSettings_Click(object sender, System.EventArgs e)
		private void menuSettings_Click(object sender, System.EventArgs e)
		{
			settingsForm.Show();
			settingsForm.Activate();
		}
		#endregion

		#region private void menuOpenHide_Click(object sender, System.EventArgs e)
		private void menuOpenHide_Click(object sender, System.EventArgs e)
		{
			notifyIcon_DoubleClick(sender, e);
		}
		#endregion
		
		#region private void menuWebsite_Click(object sender, EventArgs e)
		private void menuWebsite_Click(object sender, EventArgs e)
		{
			Shell32.ShellExecute(0, "Open", WEBSITE_URL, "", Application.StartupPath, 1);
		}
		#endregion

		#region private void menuMiniPlayer_Click(object sender, EventArgs e)
		private void menuMiniPlayer_Click(object sender, EventArgs e)
		{
			this.SuspendLayout();

			menuMiniPlayer.Checked = !menuMiniPlayer.Checked;

			try
			{
				if (!menuMiniPlayer.Checked)
				{
					this.Size = new Size(windowWidth, windowHeight);

					this.TopMost = this.configuration.KeepOnTop;

					if (configuration.Location != string.Empty)
					{
						try
						{
							string[] coordinates = configuration.Location.Split(new char[] {','});

							this.Location = new Point(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
						}
						catch (Exception ex)
						{
							Debug.WriteLine(ex.Message);
							Debug.WriteLine(ex.StackTrace);
						}
					}

					MoveToWorkingArea();

					ApplyConfiguration(configuration, false);
				}
				else
				{
					this.Size = new Size(240, pictureBoxTitle.Height + 22);

					if (configuration.MiniPlayerLocation != string.Empty)
					{
						try
						{
							string[] coordinates = configuration.MiniPlayerLocation.Split(new char[] {','});

							this.Location = new Point(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
						}
						catch (Exception ex)
						{
							Debug.WriteLine(ex.Message);
							Debug.WriteLine(ex.StackTrace);
						}
					}

					MoveToWorkingArea();

					this.TopMost = true;
					btnMinimize.Hide();
					btnClose.Hide();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
			finally
			{
				this.ResumeLayout();
			}

			pictureBoxTitle.Refresh();
		}
		#endregion
		
		#region private void menuLastFmSubmit_Click(object sender, EventArgs e)
		private void menuLastFmSubmit_Click(object sender, EventArgs e)
		{
			SubmitSongToLastFM(song.Artist, song.Name, 121);
		}
		#endregion
		
		#region private void menuToolsCopyToClipboard_Click(object sender, EventArgs e)
		private void menuToolsCopyToClipboard_Click(object sender, EventArgs e)
		{
			Clipboard.SetDataObject(title, true);
		}
		#endregion
		
		#region private void menuToolsLyrics_Click(object sender, EventArgs e)
		private void menuToolsLyrics_Click(object sender, EventArgs e)
		{
			string artistName = HttpUtility.UrlEncode(song.Artist);
			string songName = HttpUtility.UrlEncode(song.Name);
			
			string url = "http://www.leoslyrics.com/advanced.php?artistmode=0&artist=" + artistName + "&albummode=0&album=&songmode=0&song=" + songName + "&mode=0";
			Shell32.ShellExecute(0, "Open", url, "", Application.StartupPath, 1);
		}
		#endregion

		#region private void menuToolsLocateRadio_Click(object sender, EventArgs e)
		private void menuToolsLocateRadio_Click(object sender, EventArgs e)
		{
			browserTimer.Start();
		}
		#endregion
		
		//
		// Player menus
		//
		
		#region private void menuPlayerPlayPause_Click(object sender, EventArgs e)
		private void menuPlayerPlayPause_Click(object sender, EventArgs e)
		{
			try
			{
				pandora.PlayPause();
			} 
			catch {}
		}
		#endregion
		
		#region private void menuPlayerSkip_Click(object sender, EventArgs e)
		private void menuPlayerSkip_Click(object sender, EventArgs e)
		{
			try
			{
				pandora.NextTrack();
			} 
			catch {}
		}
		#endregion
		
		#region private void menuPlayerLike_Click(object sender, EventArgs e)
		private void menuPlayerLike_Click(object sender, EventArgs e)
		{
			try
			{
				pandora.Like();
			} 
			catch {}
		}
		#endregion
		
		#region private void menuPlayerHate_Click(object sender, EventArgs e)
		private void menuPlayerHate_Click(object sender, EventArgs e)
		{
			try
			{
				pandora.Hate();
			} 
			catch {}
		}
		#endregion
		
		#region private void menuPlayerStations_Click(object sender, EventArgs e)
		private void menuPlayerStations_Click(object sender, EventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			
			if (menuItem.Checked)
			{
				return;
			}
			
			Station selectedStation = null;
			string selectedStationName = menuItem.Text;
			
			foreach (Station station in stations)
			{
				if (station.Name.Equals(selectedStationName))
				{
					selectedStation = station;
					break;
				}
			}
			
			if (selectedStation != null)
			{
				ChangeStation(selectedStation);
			}
		}
		#endregion
		
		#region private void menuPlayerStations_Popup(object sender, EventArgs e)
		private void menuPlayerStations_Popup(object sender, EventArgs e)
		{
			if (deleteStation)
			{
				LoadStations();
				deleteStation = false;
			}
		}
		#endregion

		//
		// Audioscrobbler events
		//

		#region private void AudioscrobblerConnected(object sender, EventArgs e)
		private void AudioscrobblerConnected(object sender, EventArgs e)
		{
			settingsView.SetLastFmAvatar(audioscrobbler.AvatarBitmap);
		}
		#endregion

		#region private void AudioscrobblerConnected(object sender, EventArgs e)
		private void AudioscrobblerConnectionFailed(object sender, EventArgs e)
		{
			menuRefresh_Click(sender, e);
		}
		#endregion

		//
		// TaskbarNotifier events
		//
		
		#region private void taskbarNotifier_OnLocationChanged(Point Location)
		private void taskbarNotifier_OnLocationChanged(Point Location)
		{
			configuration.NotificationLocation = taskbarNotifier.Location.X + "," + taskbarNotifier.Location.Y;
		}
		#endregion

		//
		// Internal methods
		//
		
		#region internal void ApplyConfiguration(Configuration aplliedConfiguration, bool refreshPlayer)
		internal void ApplyConfiguration(Configuration appliedConfiguration, bool refreshPlayer)
		{
			try
			{
				bool isAudioscrobblerUpdated = false;
				bool isProxyUpdated = false;
			
				if (this.configuration.LastFmSubmit != appliedConfiguration.LastFmSubmit ||
				    this.configuration.LastFmUser != appliedConfiguration.LastFmUser ||
				    this.configuration.LastFmPassword != appliedConfiguration.LastFmPassword)
				{
					isAudioscrobblerUpdated = true;
				}
				
				if (configuration.ProxyHost != appliedConfiguration.ProxyHost ||
					configuration.ProxyPort != appliedConfiguration.ProxyPort ||
					configuration.ProxyUser != appliedConfiguration.ProxyUser ||
					configuration.ProxyPassword != appliedConfiguration.ProxyPassword)
				{
					isProxyUpdated = true;
				}
			
				//
				// Apply
			
				this.configuration.Apply(appliedConfiguration);
			
				try
				{
					if (!menuMiniPlayer.Checked)
					{
						//
						// Title buttons
			
						this.btnClose.Visible = this.configuration.CloseButtonVisible;
						this.btnMinimize.Visible = this.configuration.MinimizeButtonVisible;
					}
					
					//
					// Window
					
					this.TopMost = this.configuration.KeepOnTop;
					settingsForm.TopMost = this.configuration.KeepOnTop;


					//
					// Tool tip

					if (this.configuration.CloseButtonMinimizeToTray)
					{
						toolTip.SetToolTip(this.btnClose, "Close (Minimize to Tray)");
					}
					else
					{
						toolTip.SetToolTip(this.btnClose, "Close (Exit)");
					}

					toolTip.SetToolTip(this.btnMinimize, "Minimize");
			
					//
					// Refresh player
			
					if (song.Name != string.Empty)
					{
						refreshMessenger = true;
						refreshXfire = true;
						refreshSkype = true;
					}
			
					if (refreshPlayer)
					{
						RefreshPlayer(false);
					}
			
					//
					// Audioscrobbler

					if ((isAudioscrobblerUpdated || isProxyUpdated || audioscrobbler == null) &&
					    configuration.LastFmSubmit && 
					    configuration.LastFmUser != string.Empty)
					{
						if (audioscrobbler != null)
						{
							audioscrobbler.Connected -= new EventHandler(this.AudioscrobblerConnected);
						}

						audioscrobbler = new Audioscrobbler("opa", "0.1", configuration.LastFmUser, configuration.LastFmPassword);

						audioscrobbler.Connected += new EventHandler(this.AudioscrobblerConnected);
						audioscrobbler.ConnectionFailed += new EventHandler(this.AudioscrobblerConnectionFailed);
						
						audioscrobbler.Connect(
							configuration.ProxyHost, 
							configuration.ProxyPort,
							configuration.ProxyUser,
							configuration.ProxyPassword);
					}
			
					if (configuration.LastFmSubmit && configuration.LastFmSubmitManual)
					{
						menuLastFmSubmit.Visible = true;
					}
					else
					{
						menuLastFmSubmit.Visible = false;
					}
					
					//
					// Proxy
					
					if (isProxyUpdated)
					{
						LoadStations();
					}
				}
				finally
				{
					this.configuration.Save();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion
		
		//
		// Private methods
		//

		#region private void InitializeMenus()
		private void InitializeMenus()
		{
			this.menuExit = new MenuItem("Exit", new EventHandler(this.menuExit_Click));
			this.menuAbout = new MenuItem("About", new EventHandler(this.menuAbout_Click));
			this.menuSettings = new MenuItem("Settings", new EventHandler(this.menuSettings_Click));
			this.menuRefresh = new MenuItem("Refresh", new EventHandler(this.menuRefresh_Click));
			this.menuOpenHide = new MenuItem("Open/Hide", new EventHandler(this.menuOpenHide_Click));
			this.menuOpenHide.DefaultItem = true;

			this.menuMiniPlayer = new MenuItem("Mini Player", new EventHandler(this.menuMiniPlayer_Click));
			
			this.menuLastFmSubmit = new MenuItem("Submit to Last.fm", new EventHandler(this.menuLastFmSubmit_Click));
			this.menuLastFmSubmit.Visible = false; 
			
			this.menuPlayerPlayPause = new MenuItem("Play/Pause", new EventHandler(this.menuPlayerPlayPause_Click));
			this.menuPlayerSkip = new MenuItem("Skip", new EventHandler(this.menuPlayerSkip_Click));
			this.menuPlayerLike = new MenuItem("I like it", new EventHandler(this.menuPlayerLike_Click));
			this.menuPlayerHate = new MenuItem("I don't like it", new EventHandler(this.menuPlayerHate_Click));
			this.menuPlayerStations = new MenuItem("Stations");
			this.menuPlayerStationQuickmix = new MenuItem("QuickMix");
			
			this.menuPlayerPlayPause.Enabled = false;
			this.menuPlayerSkip.Enabled = false;
			this.menuPlayerLike.Enabled = false;
			this.menuPlayerHate.Enabled = false;
			this.menuPlayerStations.Enabled = false;
			
			this.menuWebsite = new MenuItem("Website ...", new EventHandler(this.menuWebsite_Click));
			
			this.menuToolsCopyToClipboard = new MenuItem("Copy info to clipboard", new EventHandler(this.menuToolsCopyToClipboard_Click));			
			this.menuToolsCopyToClipboard.Enabled = false;
			this.menuToolsLyrics = new MenuItem("Lyrics ...", new EventHandler(this.menuToolsLyrics_Click));			
			this.menuToolsLyrics.Enabled = false;
			this.menuToolsLocateRadio = new MenuItem("Focus on Pandora Radio", new EventHandler(this.menuToolsLocateRadio_Click));
			this.menuTools = new MenuItem("Tools");
			this.menuTools.MenuItems.Add(menuToolsLocateRadio);
			this.menuTools.MenuItems.Add(menuToolsCopyToClipboard);
			this.menuTools.MenuItems.Add(menuToolsLyrics);

			//
			// Construct menu
			
			this.contextMenu = new ContextMenu();
			this.contextMenu.MenuItems.Add(menuOpenHide);
			this.contextMenu.MenuItems.Add(menuRefresh);
			this.contextMenu.MenuItems.Add(menuLastFmSubmit);
			this.contextMenu.MenuItems.Add("-");
			this.contextMenu.MenuItems.Add(menuPlayerPlayPause);
			this.contextMenu.MenuItems.Add(menuPlayerSkip);
			this.contextMenu.MenuItems.Add(menuPlayerLike);
			this.contextMenu.MenuItems.Add(menuPlayerHate);
			this.contextMenu.MenuItems.Add(menuPlayerStations);
			this.contextMenu.MenuItems.Add("-");
			this.contextMenu.MenuItems.Add(menuMiniPlayer);
			this.contextMenu.MenuItems.Add(menuSettings);
			this.contextMenu.MenuItems.Add(menuTools);
			this.contextMenu.MenuItems.Add("-");
			this.contextMenu.MenuItems.Add(menuWebsite);
			this.contextMenu.MenuItems.Add(menuAbout);
			this.contextMenu.MenuItems.Add("-");
			this.contextMenu.MenuItems.Add(menuExit);
			
			this.contextMenu.Popup += new EventHandler(menuPlayerStations_Popup);
			
			this.pictureBoxTitle.ContextMenu = this.contextMenu;
		}
		#endregion
		
		#region private void InitializeTimers()
		private void InitializeTimers()
		{
			messageTimer = new System.Windows.Forms.Timer();
			messageTimer.Interval = 7000;
			messageTimer.Tick += new EventHandler(this.messageTimer_Tick);
			messageTimer.Enabled = false;
			
			browserTimer = new System.Windows.Forms.Timer();
			browserTimer.Interval = 1000;
			browserTimer.Tick += new EventHandler(this.browserTimer_Tick);
			browserTimer.Enabled = false;

			browserRefreshTimer = new System.Windows.Forms.Timer();
			browserRefreshTimer.Interval = 2500;
			browserRefreshTimer.Tick += new EventHandler(this.browserRefreshTimer_Tick);
			browserRefreshTimer.Enabled = false;

			memoryTimer = new System.Windows.Forms.Timer();
			memoryTimer.Interval = 10000;
			memoryTimer.Tick += new EventHandler(this.memoryTimer_Tick);
			memoryTimer.Enabled = false;

			titleTimer = new System.Windows.Forms.Timer();
			titleTimer.Interval = 100;
			titleTimer.Tick += new EventHandler(this.titleTimer_Tick);
			titleTimer.Enabled = false;
		}
		#endregion
		
		#region private void InitializeNotifyIcon()
		private void InitializeNotifyIcon()
		{
			notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			notifyIcon.Text = this.Text;
			notifyIcon.Icon = System.Drawing.Icon.FromHandle(pandora16.GetHicon());
			notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
			notifyIcon.Click += new EventHandler(this.notifyIcon_Click);
			notifyIcon.ContextMenu = this.contextMenu;
			notifyIcon.Visible = true;
		}
		#endregion

		#region private void InitializeTaskbarNotifier(
		private void InitializeTaskbarNotifier()
		{
			try
			{
				this.taskbarNotifier = new OpenPandora.Windows.Forms.TaskbarNotifier();	

				this.taskbarNotifier.OnLocationChanged += new OpenPandora.Windows.Forms.TaskbarNotifier.LocationChangedEventHandler(taskbarNotifier_OnLocationChanged);

				//this.taskbarNotifier.Show();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region private void RefreshPlayer(bool newSong)
		private void RefreshPlayer(bool newSong)
		{
			BuildTitle();

			bool showNotification;

			if (configuration.NotificationWindow &&
				newSong &&
				this.Text != title &&
				song.Name != string.Empty &&
				!this.Focused &&
				this.Text.IndexOf(PAUSED) == -1 && 
				title.IndexOf(PAUSED) == -1)
			{
				showNotification = true;
			}
			else
			{
				showNotification = false;
			}
			
			this.Text = title;
			this.notifyIcon.Text = BuildToolTipSongTitle();

			title = string.Empty;
			pictureBoxTitle.Refresh();
			
			title = this.Text;
			pictureBoxTitle.Refresh();

			if (showNotification)
			{
				if (configuration.NotificationWindowBalloon || taskbarNotifier == null)
				{
					NotifyIconBaloon.Show(song.Name, "by: " + song.Artist, notifyIcon);
				}
				else
				{
					string[] coordinates;
				
					if (configuration.NotificationLocation != string.Empty)
					{
						coordinates = configuration.NotificationLocation.Split(new char[] {','});
					}
					else
					{
						coordinates = new string[] {"-1", "-1"};
					}

					taskbarNotifier.Show(
						this.song.Name,
						this.song.Artist,
						string.Empty,
						this.song.ArtUrl,
						this.song.Url,
						string.Empty,
						string.Empty,
						int.Parse(coordinates[0]), 
						int.Parse(coordinates[1]), 
						500, 
						10000, 
						500);
				}
			}
			
			RefreshMessenger();
			RefreshXfire();
			RefreshSkype();
		}
		#endregion
		
		#region private void RefreshMessenger()
		private void RefreshMessenger()
		{
			if (this.configuration.SendToMessenger)
			{
				if (refreshMessenger)
				{
					if (song.Name != string.Empty)
					{
						Messenger.SetMessage(true, Messenger.Category.Music, BuildShortSongTitle());
						sentOnceToMessenger = true;
					}
					else
					{
						Messenger.SetMessage(false, Messenger.Category.Office, "");
						sentOnceToMessenger = false;
					}
				}
			}
			else if (sentOnceToMessenger)
			{
				Messenger.SetMessage(false, Messenger.Category.Office, "");
				sentOnceToMessenger = false;
			}
		}
		#endregion

		#region private void RefreshXfire()
		private void RefreshXfire()
		{
			// TODO: Check if Xfire installed

			if (this.configuration.SendToXfire)
			{
				if (refreshXfire)
				{
					if (song.Name != string.Empty)
					{
						Xfire.SetMessage(HttpUtility.UrlEncode(BuildShortSongTitle()));
						sentOnceToXfire = true;
					}
					else
					{
						Xfire.SetMessage(string.Empty);
						sentOnceToXfire = false;
					}
				}
			}
			else if (sentOnceToXfire)
			{
				Xfire.SetMessage(string.Empty);
				sentOnceToXfire = false;
			}
		}
		#endregion
		
		#region private void RefreshSkype()
		private void RefreshSkype()
		{
			if (skype == null)
				skype = new Skype();

			if (this.configuration.SendToSkype)
			{
				if (refreshSkype)
				{
					if (skype.skypeHandler == 0)
					{
						if (skype.Connect())
						{
							if (song.Name != string.Empty)
							{
								skype.SetMessage(BuildShortSongTitle());
								sentOnceToSkype = true;
							}
							else
							{
								skype.SetMessage("");
								sentOnceToSkype = false;
							}
						}
					}
					else
					{
						if (song.Name != string.Empty)
						{
							skype.SetMessage(BuildShortSongTitle());
							sentOnceToSkype = true;
						}
						else
						{
							skype.SetMessage("");
							sentOnceToSkype = false;
						}
					}
				}
			}
			else if (sentOnceToSkype)
			{
				if (skype.skypeHandler == 0)
				{
					if (skype.Connect())
					{
						skype.SetMessage("");
						sentOnceToSkype = false;
					}
				}
				else
				{
					skype.SetMessage("");
					sentOnceToSkype = false;
				}
			}
		}
		#endregion
		
		#region private void BuildTitle()
		private void BuildTitle()
		{
			if (song.Name == string.Empty)
			{
				if (!loaded2)
				{
					title = DEFAULT_TITLE;
				}
				else if (isLatestVersion)
				{
					title = "OpenPandora ~ Open your music and enjoy it!";
				}
				else if (isBetaVersion)
				{
					title = DEFAULT_TITLE + ": ~~~ BETA " + Manager.CurrentVersion + " ~~~";
				}
				else
				{
					title = DEFAULT_TITLE + ": " + " ~~~ New version " + latestVersion + " is available ~~~";
				}
			}
			else
			{
				title = BuildSongTitle();
			}
		}
		#endregion
		
		#region private string BuildSongTitle()
		private string BuildSongTitle()
		{
			const string BLANK5 = "     ";
			const string BLANK4 = "    ";
			const string BLANK3 = "   ";
			const string BLANK2 = "  ";
			
			string songTitle = BuildShortSongTitle();
			
			string blank;
			
			if (configuration.TitleTemplate.IndexOf(BLANK5) != -1)
			{
				blank = BLANK5;
			}
			else if (configuration.TitleTemplate.IndexOf(BLANK4) != -1)
			{
				blank = BLANK4;
			}
			else if (configuration.TitleTemplate.IndexOf(BLANK3) != -1)
			{
				blank = BLANK3;
			}
			else if (configuration.TitleTemplate.IndexOf(BLANK2) != -1)
			{
				blank = BLANK2;
			}
			else
			{
				blank = " ";
			}
			
			if (paused)
			{
				songTitle = songTitle + blank + PAUSED;
			}
			
			return songTitle;
		}
		#endregion
		
		#region private string BuildShortSongTitle()
		private string BuildShortSongTitle()
		{		
			string songTitle = configuration.TitleTemplate;
			
			songTitle = songTitle.Replace("%s", song.Name);
			songTitle = songTitle.Replace("%a", song.Artist);

			if (stations != null)
			{
				Station currentStation = null;
			
				foreach (Station station in stations)
				{
					if (station.Code.Equals(currentStationCode))
					{
						currentStation = station;
						break;
					}
				}

				if (currentStation != null)
				{
					songTitle = songTitle.Replace("%r", currentStation.Name);
				}
			}
			
			return songTitle;
		}
		#endregion
		
		#region private string BuildToolTipSongTitle()
		private string BuildToolTipSongTitle()
		{
			if (song.Name != string.Empty)
			{
				string songShortName;
				
				if (song.Name.Length + song.Artist.Length > 48)
				{
					int songNameLength = 48 - song.Artist.Length < 20 ? 20 : 48 - song.Artist.Length;
					songShortName = song.Name.Substring(0, songNameLength);
					songShortName = songShortName.PadRight(3 + songNameLength, '.');
				}
				else
				{
					songShortName = song.Name;
				}
				
				string toolTipTitle = songShortName + Environment.NewLine + song.Artist;

				if (toolTipTitle.Length > 51)
				{
					toolTipTitle = toolTipTitle.Substring(0, 51 - 3);
					toolTipTitle = toolTipTitle.PadRight(51, '.');
				}
				
				if (paused)
				{
					toolTipTitle = toolTipTitle + Environment.NewLine + PAUSED;
				}
				
				return toolTipTitle;
			}
			else
			{
				return DEFAULT_TITLE;
			}
		}
		#endregion
		
		#region private void SubmitSongToLastFM(string artistName, string songName, int length)
		private void SubmitSongToLastFM(string artistName, string songName, int length)
		{
			submittedToLastFm = true;
			menuLastFmSubmit.Enabled = false;
			
			if (songName == string.Empty ||
				artistName == string.Empty)
			{
				return;
			}
			
			if (configuration.LastFmSubmit && 
			    audioscrobbler != null && 
			    length > 120)
			{
				audioscrobbler.Submit(artistName, songName, string.Empty, string.Empty, length);
			}
		}
		#endregion
		
		#region private void StationsLoaded(object sender, EventArgs e)
		private void StationsLoaded(object sender, EventArgs e)
		{	
			this.stations = StationLoader.Stations;
			
			menuPlayerStations.MenuItems.Clear();
			menuPlayerStations.MenuItems.Add("-");
			int sharedIndex = 0;
			Station quickMixStation = null;
			
			foreach (Station station in stations)
			{
				MenuItem menuItem = new MenuItem(station.Name, new EventHandler(menuPlayerStations_Click));
				
				if (station.Shared)
				{
					menuPlayerStations.MenuItems.Add(sharedIndex, menuItem);
					++sharedIndex;
				}
				else if (station.QuickMix)
				{
					quickMixStation = station;
				}
				else
				{
					menuPlayerStations.MenuItems.Add(menuItem);
				}
			}

			if (quickMixStation != null)
			{
				menuPlayerStations.MenuItems.Add("-");

				MenuItem menuItem = new MenuItem(quickMixStation.Name, new EventHandler(menuPlayerStations_Click));
				menuPlayerStations.MenuItems.Add(menuItem);
			}
			
			if (sharedIndex == 0)
			{
				menuPlayerStations.MenuItems.RemoveAt(0);
			}
			
			Station currentStation = null;
			
			foreach (Station station in stations)
			{
				if (station.Code.Equals(currentStationCode))
				{
					currentStation = station;
					break;
				}
			}
						
			if (currentStation != null)
			{
				foreach (MenuItem menuItem in menuPlayerStations.MenuItems)
				{
					if (menuItem.Text.Equals(currentStation.Name))
					{
						menuItem.Checked = true;
					}
					else
					{
						menuItem.Checked = false;
					}
				}
			}
			
			menuPlayerStations.Enabled = true;
		}
		#endregion
		
		#region private void ChangeStation(Station station)
		private void ChangeStation(Station station)
		{
			try
			{
				IHTMLDocument2 document = (IHTMLDocument2)browser.Document;

				if (station.QuickMix)
				{
					document.parentWindow.execScript("Pandora.launchQuickMixFromId('" + station.Code + "')", "javascript");
				}
				else
				{
					document.parentWindow.execScript("Pandora.launchStationFromId('" + station.Code + "')", "javascript");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(station);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion
		
		#region private void OnPlayStart()
		private void OnPlayStart()
		{
			this.menuPlayerPlayPause.Enabled = true;
			this.menuPlayerSkip.Enabled = true;
			this.menuPlayerLike.Enabled = true;
			this.menuPlayerHate.Enabled = true;
			this.menuToolsCopyToClipboard.Enabled = true;
			this.menuToolsLyrics.Enabled = true;
						
			this.menuPlayerPlayPause.Text = "Pause";

			if (this.isFirstTrack)
			{
				browserTimer.Start();
			}

			this.isFirstTrack = false;
		}
		#endregion
		
		#region private void LoadStations()
		private void LoadStations()
		{
			StationLoader.Load(
				user, 
				configuration.ProxyHost, 
				configuration.ProxyPort,
				configuration.ProxyUser,
				configuration.ProxyPassword);
		}
		#endregion
		
		#region private void RestartPlayer()
		private void RestartPlayer()
		{
			song = new Song(string.Empty, string.Empty, string.Empty);
			paused = false;
			refreshMessenger = true;
			refreshXfire = true;
			refreshSkype = true;
						
			this.menuPlayerPlayPause.Enabled = false;
			this.menuPlayerSkip.Enabled = false;
			this.menuPlayerLike.Enabled = false;
			this.menuPlayerHate.Enabled = false;
			this.menuPlayerStations.Enabled = false;
			this.menuToolsCopyToClipboard.Enabled = false;
						
			RefreshPlayer(false);
		}
		#endregion
		
		#region private void MoveStationUp()
		private void MoveStationUp()
		{
			const int minimumStationChangeInterval = 5;
			
			if ((DateTime.Now - lastStationFromKeyboardChange).TotalSeconds < minimumStationChangeInterval)
			{
				Debug.WriteLine("Change station from keyboard less than " + minimumStationChangeInterval + " seconds.");
				return;
			}
			
			for (int i = 0; i < stations.Length; i++)
			{
				if (stations[i].Code.Equals(currentStationCode))
				{
					if (i > 0)
					{
						ChangeStation(stations[i - 1]);
						lastStationFromKeyboardChange = DateTime.Now;
					}
					
					break;
				}
			}
		}
		#endregion
		
		#region private void MoveStationDown()
		private void MoveStationDown()
		{
			const int minimumStationChangeInterval = 5;
			
			if ((DateTime.Now - lastStationFromKeyboardChange).TotalSeconds < minimumStationChangeInterval)
			{
				Debug.WriteLine("Change station from keyboard less than " + minimumStationChangeInterval + " seconds.");
				return;
			}
			
			for (int i = 0; i < stations.Length; i++)
			{
				if (stations[i].Code.Equals(currentStationCode))
				{
					if (i + 1 < stations.Length)
					{
						ChangeStation(stations[i + 1]);
						lastStationFromKeyboardChange = DateTime.Now;
					}
					
					break;
				}
			}
		}
		#endregion

		#region private void SetQuickMixStation()
		private void SetQuickMixStation()
		{
			const int minimumStationChangeInterval = 5;
			
			if ((DateTime.Now - lastStationFromKeyboardChange).TotalSeconds < minimumStationChangeInterval)
			{
				Debug.WriteLine("Change station from keyboard less than " + minimumStationChangeInterval + " seconds.");
				return;
			}

			for (int i = 0; i < stations.Length; i++)
			{
				if (!stations[i].Shared && stations[i].QuickMix)
				{
					ChangeStation(stations[i]);
					lastStationFromKeyboardChange = DateTime.Now;
					
					break;
				}
			}
		}
		#endregion

		#region private void MoveToWorkingArea()
		private void MoveToWorkingArea()
		{
			if (this.Location.X < Screen.PrimaryScreen.WorkingArea.Left)
			{
				this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Left, this.Location.Y);
			}
			else if (this.Location.X + this.Width > Screen.PrimaryScreen.WorkingArea.Right)
			{
				this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - this.Width, this.Location.Y);
			}

			if (this.Location.Y < Screen.PrimaryScreen.WorkingArea.Top)
			{
				this.Location = new Point(this.Location.X, Screen.PrimaryScreen.WorkingArea.Top);
			}
			else if (this.Location.Y + this.Height > Screen.PrimaryScreen.WorkingArea.Bottom)
			{
				this.Location = new Point(this.Location.X, Screen.PrimaryScreen.WorkingArea.Bottom - this.Height);
			}
		}
		#endregion

		#region private void ShowMessage(string text)
		private void ShowMessage(string text)
		{
			message = text;
			messageTimer.Start();
			
			pictureBoxTitle.Refresh();
		}
		#endregion
		
		#region private void MinimizeToTray()
		private void MinimizeToTray()
		{
			try
			{
				this.Hide();

				if (taskbarNotifier != null)
				{
					NotifyIconBaloon.AnimateMinimizeToTray(this);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		#region private void RestoreFromTray()
		private void RestoreFromTray()
		{
			try
			{
				if (taskbarNotifier != null)
				{
					NotifyIconBaloon.AnimateRestoreFromTray(this);
				}

				this.Show();
				this.Activate();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion

		//
		// Shortcuts
		//
		
		#region private void GlobalKeyDown(object sender, KeyEventArgs e)
		private void GlobalKeyDown(object sender, KeyEventArgs e)
		{	
			try
			{
				if (!configuration.KeyboardVolumeKeys)
				{
					/*if (e.KeyCode == Keys.VolumeDown)
					{
						pandora.VolumeDown();
						e.Handled = true;
					}
					else if (e.KeyCode == Keys.VolumeUp)
					{
						pandora.VolumeUp();
						e.Handled = true;
					}*/
				}

				if (windowsHook != null && configuration.GlobalShortcuts && windowsHook.Control && windowsHook.Win)
				{
					if (pandora != null)
					{
						if (e.KeyCode == Keys.Space)
						{
							pandora.PlayPause();
							e.Handled = true;
						}
						else if (e.KeyCode == Keys.Right)
						{
							pandora.NextTrack();
							e.Handled = true;
						}
						else if (e.KeyCode == Keys.Up)
						{
							pandora.VolumeUp();
							e.Handled = true;
						}
						else if (e.KeyCode == Keys.Down)
						{
							pandora.VolumeDown();
							e.Handled = true;
						}
						else if (e.KeyCode == Keys.Add)
						{
							pandora.Like();
							e.Handled = true;
						}
						else if (e.KeyCode == Keys.Subtract)
						{
							pandora.Hate();
							e.Handled = true;
						}
						else if (e.KeyCode == Keys.PageUp)
						{
							MoveStationUp();
							e.Handled = true;
						}
						else if (e.KeyCode == Keys.PageDown)
						{
							MoveStationDown();
							e.Handled = true;
						}
						else if (e.KeyCode == Keys.Q)
						{
							SetQuickMixStation();
							e.Handled = true;
						}
					}

					if (e.KeyCode == Keys.Home)
					{
						notifyIcon_DoubleClick(this, new EventArgs());
						e.Handled = true;
					}
					
					// Release pressed keys
					if (e.Handled && e.KeyCode != Keys.Up && e.KeyCode != Keys.Down)
					{
						windowsHook.Clean();
					}
				}
			}
			catch
			{
				e.Handled = false;
			}
		}
		#endregion
		
		#region private void GlobalKeyUp(object sender, KeyEventArgs e)
		private void GlobalKeyUp(object sender, KeyEventArgs e)
		{
			if (pandora == null)
			{
				return;
			}
			
			try
			{				
				if (configuration.KeyboardMediaKeys)
				{
					if (e.KeyCode == Keys.MediaPlayPause)
					{
						pandora.PlayPause();
						e.Handled = true;
					}
					else if (e.KeyCode == Keys.MediaNextTrack)
					{
						pandora.NextTrack();
						e.Handled = true;
					}
				}
			}
			catch
			{
				e.Handled = false;
			}
		}
		#endregion

		//
		// Main
		//

		[STAThread]
		#region static void Main()
		static void Main()
		{
			bool exclusive;
			
			using (Mutex mutex = new Mutex(true, "OpenPandoraWindowForm", out exclusive))
			{
				if (exclusive)
				{
					//Application.EnableVisualStyles();
				
					try
					{
						AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
						Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

						Application.Run(new Player());
					}
					catch (Exception ex)
					{
						Debug.WriteLine("Application Loop exception - " + ex.Message);
						Debug.WriteLine(ex.StackTrace);
					}
					finally
					{
						try
						{
							Messenger.SetMessage(false, Messenger.Category.Office, string.Empty);
						} 
						catch {}

						try
						{
							Process[] processes = Process.GetProcessesByName("Xfire");

							if (processes.Length>0)
							Xfire.SetMessage(string.Empty);
						} 
						catch {}
					}
				}
				else
				{
					Process current = Process.GetCurrentProcess();
					Process[] processes = Process.GetProcessesByName(current.ProcessName);

					foreach (Process process in processes)
					{
						if (process.Id != current.Id)
						{
							User32.SwitchToProcess(process);
							return;
						}
					}
				}
			}
		}
		#endregion

		#region static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Debug.WriteLine("CurrentDomain_UnhandledException: " + e.ExceptionObject.ToString() + ". Terminating:" + e.IsTerminating);
		}
		#endregion

		#region private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Debug.WriteLine("Application_ThreadException: " + e.Exception.Message);
			Debug.WriteLine(e.Exception.StackTrace);
		}
		#endregion

		//
		// Private data
		//

		#region data
		private ContextMenu contextMenu;
		
		private MenuItem menuExit;
		private MenuItem menuRefresh;
		private MenuItem menuOpenHide;
		private MenuItem menuAbout;
		private MenuItem menuSettings;
		private MenuItem menuWebsite;
		private MenuItem menuMiniPlayer;
		private MenuItem menuLastFmSubmit;
		private MenuItem menuPlayerPlayPause;
		private MenuItem menuPlayerSkip;
		private MenuItem menuPlayerLike;
		private MenuItem menuPlayerHate;
		private MenuItem menuPlayerStations;
		private MenuItem menuPlayerStationQuickmix;
		
		private MenuItem menuTools;
		private MenuItem menuToolsCopyToClipboard;
		private MenuItem menuToolsLyrics;
		private MenuItem menuToolsLocateRadio;

		private object missing = System.Type.Missing;

		private Bitmap pandora16;
		private Point mouseOffset;
		private bool loaded = false;
		private bool loaded2 = false;
		internal Configuration configuration;
		private bool isLatestVersion = false;
		private bool isBetaVersion = false;
		private bool sentOnceToMessenger = false;
		private bool sentOnceToXfire = false;
        private bool sentOnceToSkype = false;

		private bool hideTitle = false;
		private bool isFirstTrack = true;
		private string userUrl;
		private string user = "Unknown";
		private string currentStationCode = string.Empty;
		private Station[] stations = new Station[0];
		private bool isPayingUser = false;
		private DateTime lastBookmark = DateTime.MinValue;
		private Song song = new Song(string.Empty, string.Empty, string.Empty);
		private Song nextSong = null;
		private bool paused = false;
		private bool refreshMessenger = false;
		private bool refreshXfire = false;
        private bool refreshSkype = false;
		private string browserTitle = string.Empty;
		private string message = string.Empty;
		private string title = string.Empty;
		private System.Windows.Forms.Timer messageTimer;
		private System.Windows.Forms.Timer browserTimer;
		private System.Windows.Forms.Timer browserRefreshTimer;
		private System.Windows.Forms.Timer memoryTimer;
        private System.Windows.Forms.Timer titleTimer;
		private Audioscrobbler audioscrobbler;
		private bool submittedToLastFm = false;
		private Pandora pandora;
		private WindowsHook windowsHook;
		private bool deleteStation = false;
		private bool sharedStation = false;
		private int continuesPlayCounter = 0;
		private DateTime loginTime = DateTime.MaxValue;
		private string latestVersion = string.Empty;
		private DateTime lastStationFromKeyboardChange = DateTime.MinValue;
		private int windowHeight;
		private int windowWidth;
		private Size radioSize = new Size(0, 0);
		private About aboutForm;
		
		private int playedLength = 0;
		private DateTime playedStartTime;
		private int titlePosition = 0;
		private int titleDirection = 1;
		private bool titleBounce = false;

		#endregion

		private void browser2_StatusTextChange(object sender, DWebBrowserEvents2_StatusTextChangeEvent e)
		{
			if (e.text.StartsWith("Pandora222222222222222"))
			{
				IHTMLDocument2 document = (IHTMLDocument2)browser2.Document;

				IHTMLElement songNameElement = (IHTMLElement)document.all.item("songName", 0);
				IHTMLElement artistNameElement = (IHTMLElement)document.all.item("artistName", 0);
				IHTMLElement songURLElement = (IHTMLElement)document.all.item("songURL", 0);
				IHTMLElement artURLElement = (IHTMLElement)document.all.item("artURL", 0);

				IHTMLElement stationNameElement = (IHTMLElement)document.all.item("stationName", 0);
				IHTMLElement stationIdElement = (IHTMLElement)document.all.item("stationId", 0);
				IHTMLElement stationIsSharedElement = (IHTMLElement)document.all.item("stationIsShared", 0);
				IHTMLElement stationIsQuickMixElement = (IHTMLElement)document.all.item("stationIsQuickMix", 0);

				string songName = (songNameElement != null ? songNameElement.innerText : string.Empty);
				string artistName = (artistNameElement != null ? artistNameElement.innerText : string.Empty);
				string songURL = (songURLElement != null ? songURLElement.innerText : string.Empty);
				string artURL = (artURLElement != null ? artURLElement.innerText : string.Empty);

				string stationName = (stationNameElement != null ? stationNameElement.innerText : string.Empty);
				string stationId = (stationIdElement != null ? stationIdElement.innerText : string.Empty);
				string stationIsShared = (stationIsSharedElement != null ? stationIsSharedElement.innerText : string.Empty);
				string stationIsQuickMix = (stationIsQuickMixElement != null ? stationIsQuickMixElement.innerText : string.Empty);

				if (e.text.StartsWith("Pandora.SongPlayed"))
				{
					if (song.Name == string.Empty)
					{
						song.Name = songName;
						song.Artist = artistName;
					}
					else
					{
						nextSong = new Song(string.Empty, songName, artistName);
					}
				}
				else if (e.text.StartsWith("Pandora.StationPlayed"))
				{
					Debug.WriteLine("Changed station to " + stationName);

					currentStationCode = stationId;

					foreach (MenuItem menuItem in menuPlayerStations.MenuItems)
					{
						menuItem.Checked = false;
					}
						
					if (bool.Parse(stationIsQuickMix))
					{
						menuPlayerStationQuickmix.Checked = true;
					}
					else
					{
						Station currentStation = null;
						
						foreach (Station station in stations)
						{
							if (station.Code.Equals(currentStationCode))
							{
								currentStation = station;
								break;
							}
						}
						
						if (currentStation != null)
						{
							foreach (MenuItem menuItem in menuPlayerStations.MenuItems)
							{
								if (menuItem.Text.Equals(currentStation.Name))
								{
									menuItem.Checked = true;
									break;
								}
							}
						}
					}
				}
			}
		}
	}
}
