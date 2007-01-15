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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace OpenPandora
{
	// Size 294:190
	public class SettingsView : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.CheckBox chbShowCloseButton;
		private System.Windows.Forms.CheckBox chbMinimizeToTrayOnClose;
		private System.Windows.Forms.CheckBox chbOpenLinksInDefaultBrowser;
		private System.Windows.Forms.RadioButton rdbTitleTemplate1;
		private System.Windows.Forms.RadioButton rdbTitleTemplate2;
		private System.Windows.Forms.RadioButton rdbTitleTemplate3;
		private System.Windows.Forms.TextBox txtTitleTemplate;
		private System.Windows.Forms.GroupBox grbTitleFormat;
		private System.Windows.Forms.TabPage tabPageDisplay;
		private System.Windows.Forms.TabPage tabPageLastFM;
		private System.Windows.Forms.TabPage tabPageGeneral;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtLastFmUser;
		private System.Windows.Forms.TextBox txtLastFmPassword;
		private System.Windows.Forms.CheckBox chbLastFmSubmit;
		private System.Windows.Forms.RadioButton rdbAutomatic;
		private System.Windows.Forms.RadioButton rdbManual;
		private System.Windows.Forms.CheckBox chbSubmitSkipped;
		private System.Windows.Forms.CheckBox chbKeepOnTop;
		private System.Windows.Forms.CheckBox chbPartyMode;
		private System.Windows.Forms.TabPage tabProxy;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton rdbProxyAuto;
		private System.Windows.Forms.RadioButton rdbProxyManual;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtProxyPort;
		private System.Windows.Forms.TextBox txtProxyHost;
		private System.Windows.Forms.TextBox txtProxyPassword;
		private System.Windows.Forms.TextBox txtProxyUser;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabPageControl;
		private System.Windows.Forms.CheckBox chbKeyboardMediaKeys;
		private System.Windows.Forms.CheckBox chbGlobalShortcuts;
		private System.Windows.Forms.LinkLabel linkHelpGlobalShortcuts;
		private System.Windows.Forms.PictureBox pictureBoxLastFmAvatar;
		private System.Windows.Forms.CheckBox chbPopupNotificationWindow;
		private System.Windows.Forms.TabPage tabPageSendSongInfo;
		private System.Windows.Forms.CheckBox chbSendToSkype;
		private System.Windows.Forms.CheckBox chbSendToXfire;
		private System.Windows.Forms.CheckBox chbSendToMessenger;
		private System.Windows.Forms.Label labelFormats;
		private System.Windows.Forms.CheckBox chbNotificationBalloon;
		private System.ComponentModel.Container components = null;

		//
		// Constructors
		//
		
		#region SettingsView()
		public SettingsView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			labelFormats.Text = "%s - song" + Environment.NewLine + "%a - artist" + Environment.NewLine + "%r - radio";
		}
		#endregion

		//
		// Generated code
		//
		
		#region protected override void Dispose( bool disposing )
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
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnApply = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.chbShowCloseButton = new System.Windows.Forms.CheckBox();
			this.chbMinimizeToTrayOnClose = new System.Windows.Forms.CheckBox();
			this.chbOpenLinksInDefaultBrowser = new System.Windows.Forms.CheckBox();
			this.rdbTitleTemplate1 = new System.Windows.Forms.RadioButton();
			this.rdbTitleTemplate2 = new System.Windows.Forms.RadioButton();
			this.rdbTitleTemplate3 = new System.Windows.Forms.RadioButton();
			this.txtTitleTemplate = new System.Windows.Forms.TextBox();
			this.grbTitleFormat = new System.Windows.Forms.GroupBox();
			this.labelFormats = new System.Windows.Forms.Label();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabPageDisplay = new System.Windows.Forms.TabPage();
			this.tabProxy = new System.Windows.Forms.TabPage();
			this.txtProxyPassword = new System.Windows.Forms.TextBox();
			this.txtProxyUser = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.rdbProxyManual = new System.Windows.Forms.RadioButton();
			this.rdbProxyAuto = new System.Windows.Forms.RadioButton();
			this.txtProxyPort = new System.Windows.Forms.TextBox();
			this.txtProxyHost = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tabPageGeneral = new System.Windows.Forms.TabPage();
			this.chbPopupNotificationWindow = new System.Windows.Forms.CheckBox();
			this.chbPartyMode = new System.Windows.Forms.CheckBox();
			this.chbKeepOnTop = new System.Windows.Forms.CheckBox();
			this.tabPageSendSongInfo = new System.Windows.Forms.TabPage();
			this.chbSendToSkype = new System.Windows.Forms.CheckBox();
			this.chbSendToXfire = new System.Windows.Forms.CheckBox();
			this.chbSendToMessenger = new System.Windows.Forms.CheckBox();
			this.tabPageControl = new System.Windows.Forms.TabPage();
			this.linkHelpGlobalShortcuts = new System.Windows.Forms.LinkLabel();
			this.chbGlobalShortcuts = new System.Windows.Forms.CheckBox();
			this.chbKeyboardMediaKeys = new System.Windows.Forms.CheckBox();
			this.tabPageLastFM = new System.Windows.Forms.TabPage();
			this.pictureBoxLastFmAvatar = new System.Windows.Forms.PictureBox();
			this.chbSubmitSkipped = new System.Windows.Forms.CheckBox();
			this.rdbManual = new System.Windows.Forms.RadioButton();
			this.rdbAutomatic = new System.Windows.Forms.RadioButton();
			this.chbLastFmSubmit = new System.Windows.Forms.CheckBox();
			this.txtLastFmPassword = new System.Windows.Forms.TextBox();
			this.txtLastFmUser = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.chbNotificationBalloon = new System.Windows.Forms.CheckBox();
			this.grbTitleFormat.SuspendLayout();
			this.tabs.SuspendLayout();
			this.tabPageDisplay.SuspendLayout();
			this.tabProxy.SuspendLayout();
			this.tabPageGeneral.SuspendLayout();
			this.tabPageSendSongInfo.SuspendLayout();
			this.tabPageControl.SuspendLayout();
			this.tabPageLastFM.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnApply.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btnApply.Location = new System.Drawing.Point(8, 168);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(48, 21);
			this.btnApply.TabIndex = 0;
			this.btnApply.Text = "Apply";
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			this.btnApply.MouseEnter += new System.EventHandler(this.btnApply_MouseEnter);
			this.btnApply.MouseLeave += new System.EventHandler(this.btnApply_MouseLeave);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btnClose.Location = new System.Drawing.Point(274, 168);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(48, 21);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			this.btnClose.MouseEnter += new System.EventHandler(this.btnClose_MouseEnter);
			this.btnClose.MouseLeave += new System.EventHandler(this.btnClose_MouseLeave);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.btnSave.Location = new System.Drawing.Point(64, 168);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(48, 21);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnSave.MouseEnter += new System.EventHandler(this.btnSave_MouseEnter);
			this.btnSave.MouseLeave += new System.EventHandler(this.btnSave_MouseLeave);
			// 
			// chbShowCloseButton
			// 
			this.chbShowCloseButton.Location = new System.Drawing.Point(8, 8);
			this.chbShowCloseButton.Name = "chbShowCloseButton";
			this.chbShowCloseButton.Size = new System.Drawing.Size(176, 24);
			this.chbShowCloseButton.TabIndex = 5;
			this.chbShowCloseButton.Text = "Show Close Button";
			this.chbShowCloseButton.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// chbMinimizeToTrayOnClose
			// 
			this.chbMinimizeToTrayOnClose.Location = new System.Drawing.Point(8, 32);
			this.chbMinimizeToTrayOnClose.Name = "chbMinimizeToTrayOnClose";
			this.chbMinimizeToTrayOnClose.Size = new System.Drawing.Size(208, 24);
			this.chbMinimizeToTrayOnClose.TabIndex = 6;
			this.chbMinimizeToTrayOnClose.Text = "Minimize To Tray On Close";
			this.chbMinimizeToTrayOnClose.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// chbOpenLinksInDefaultBrowser
			// 
			this.chbOpenLinksInDefaultBrowser.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chbOpenLinksInDefaultBrowser.Location = new System.Drawing.Point(208, 416);
			this.chbOpenLinksInDefaultBrowser.Name = "chbOpenLinksInDefaultBrowser";
			this.chbOpenLinksInDefaultBrowser.Size = new System.Drawing.Size(176, 24);
			this.chbOpenLinksInDefaultBrowser.TabIndex = 8;
			this.chbOpenLinksInDefaultBrowser.Text = "Open Links In Default Browser";
			this.chbOpenLinksInDefaultBrowser.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// rdbTitleTemplate1
			// 
			this.rdbTitleTemplate1.Location = new System.Drawing.Point(8, 16);
			this.rdbTitleTemplate1.Name = "rdbTitleTemplate1";
			this.rdbTitleTemplate1.Size = new System.Drawing.Size(72, 24);
			this.rdbTitleTemplate1.TabIndex = 0;
			this.rdbTitleTemplate1.Text = "%s - %a";
			this.rdbTitleTemplate1.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// rdbTitleTemplate2
			// 
			this.rdbTitleTemplate2.Location = new System.Drawing.Point(80, 16);
			this.rdbTitleTemplate2.Name = "rdbTitleTemplate2";
			this.rdbTitleTemplate2.Size = new System.Drawing.Size(72, 24);
			this.rdbTitleTemplate2.TabIndex = 1;
			this.rdbTitleTemplate2.Text = "%a - %s";
			this.rdbTitleTemplate2.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// rdbTitleTemplate3
			// 
			this.rdbTitleTemplate3.Location = new System.Drawing.Point(152, 16);
			this.rdbTitleTemplate3.Name = "rdbTitleTemplate3";
			this.rdbTitleTemplate3.Size = new System.Drawing.Size(64, 24);
			this.rdbTitleTemplate3.TabIndex = 2;
			this.rdbTitleTemplate3.Text = "Custom";
			this.rdbTitleTemplate3.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// txtTitleTemplate
			// 
			this.txtTitleTemplate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(177)));
			this.txtTitleTemplate.Location = new System.Drawing.Point(8, 40);
			this.txtTitleTemplate.Name = "txtTitleTemplate";
			this.txtTitleTemplate.Size = new System.Drawing.Size(200, 21);
			this.txtTitleTemplate.TabIndex = 3;
			this.txtTitleTemplate.Text = "";
			// 
			// grbTitleFormat
			// 
			this.grbTitleFormat.Controls.Add(this.rdbTitleTemplate1);
			this.grbTitleFormat.Controls.Add(this.rdbTitleTemplate2);
			this.grbTitleFormat.Controls.Add(this.rdbTitleTemplate3);
			this.grbTitleFormat.Controls.Add(this.txtTitleTemplate);
			this.grbTitleFormat.Controls.Add(this.labelFormats);
			this.grbTitleFormat.Location = new System.Drawing.Point(8, 64);
			this.grbTitleFormat.Name = "grbTitleFormat";
			this.grbTitleFormat.Size = new System.Drawing.Size(304, 64);
			this.grbTitleFormat.TabIndex = 15;
			this.grbTitleFormat.TabStop = false;
			this.grbTitleFormat.Text = "Title Format";
			// 
			// labelFormats
			// 
			this.labelFormats.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(59)), ((System.Byte)(59)), ((System.Byte)(59)));
			this.labelFormats.Location = new System.Drawing.Point(224, 16);
			this.labelFormats.Name = "labelFormats";
			this.labelFormats.Size = new System.Drawing.Size(72, 40);
			this.labelFormats.TabIndex = 16;
			this.labelFormats.Text = "%s - song %a - artist %r - radio";
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.tabPageDisplay);
			this.tabs.Controls.Add(this.tabProxy);
			this.tabs.Controls.Add(this.tabPageGeneral);
			this.tabs.Controls.Add(this.tabPageSendSongInfo);
			this.tabs.Controls.Add(this.tabPageControl);
			this.tabs.Controls.Add(this.tabPageLastFM);
			this.tabs.Location = new System.Drawing.Point(0, 0);
			this.tabs.Name = "tabs";
			this.tabs.Padding = new System.Drawing.Point(0, 0);
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(328, 160);
			this.tabs.TabIndex = 17;
			// 
			// tabPageDisplay
			// 
			this.tabPageDisplay.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.tabPageDisplay.Controls.Add(this.grbTitleFormat);
			this.tabPageDisplay.Controls.Add(this.chbShowCloseButton);
			this.tabPageDisplay.Controls.Add(this.chbMinimizeToTrayOnClose);
			this.tabPageDisplay.Location = new System.Drawing.Point(4, 22);
			this.tabPageDisplay.Name = "tabPageDisplay";
			this.tabPageDisplay.Size = new System.Drawing.Size(320, 134);
			this.tabPageDisplay.TabIndex = 0;
			this.tabPageDisplay.Text = "Display";
			// 
			// tabProxy
			// 
			this.tabProxy.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.tabProxy.Controls.Add(this.txtProxyPassword);
			this.tabProxy.Controls.Add(this.txtProxyUser);
			this.tabProxy.Controls.Add(this.label6);
			this.tabProxy.Controls.Add(this.label7);
			this.tabProxy.Controls.Add(this.rdbProxyManual);
			this.tabProxy.Controls.Add(this.rdbProxyAuto);
			this.tabProxy.Controls.Add(this.txtProxyPort);
			this.tabProxy.Controls.Add(this.txtProxyHost);
			this.tabProxy.Controls.Add(this.label2);
			this.tabProxy.Controls.Add(this.label5);
			this.tabProxy.Location = new System.Drawing.Point(4, 22);
			this.tabProxy.Name = "tabProxy";
			this.tabProxy.Size = new System.Drawing.Size(320, 134);
			this.tabProxy.TabIndex = 3;
			this.tabProxy.Text = "Proxy";
			// 
			// txtProxyPassword
			// 
			this.txtProxyPassword.Location = new System.Drawing.Point(72, 112);
			this.txtProxyPassword.Name = "txtProxyPassword";
			this.txtProxyPassword.PasswordChar = '*';
			this.txtProxyPassword.Size = new System.Drawing.Size(120, 20);
			this.txtProxyPassword.TabIndex = 23;
			this.txtProxyPassword.Text = "";
			// 
			// txtProxyUser
			// 
			this.txtProxyUser.Location = new System.Drawing.Point(72, 84);
			this.txtProxyUser.Name = "txtProxyUser";
			this.txtProxyUser.Size = new System.Drawing.Size(120, 20);
			this.txtProxyUser.TabIndex = 22;
			this.txtProxyUser.Text = "";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 112);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 16);
			this.label6.TabIndex = 21;
			this.label6.Text = "Password";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(16, 84);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 16);
			this.label7.TabIndex = 20;
			this.label7.Text = "Username";
			// 
			// rdbProxyManual
			// 
			this.rdbProxyManual.Location = new System.Drawing.Point(8, 28);
			this.rdbProxyManual.Name = "rdbProxyManual";
			this.rdbProxyManual.Size = new System.Drawing.Size(176, 24);
			this.rdbProxyManual.TabIndex = 19;
			this.rdbProxyManual.Text = "Manual Configuration";
			this.rdbProxyManual.CheckedChanged += new System.EventHandler(this.rdbProxy_CheckedChanged);
			// 
			// rdbProxyAuto
			// 
			this.rdbProxyAuto.Location = new System.Drawing.Point(8, 8);
			this.rdbProxyAuto.Name = "rdbProxyAuto";
			this.rdbProxyAuto.Size = new System.Drawing.Size(208, 24);
			this.rdbProxyAuto.TabIndex = 18;
			this.rdbProxyAuto.Text = "Automatic Proxy Detection";
			this.rdbProxyAuto.CheckedChanged += new System.EventHandler(this.rdbProxy_CheckedChanged);
			// 
			// txtProxyPort
			// 
			this.txtProxyPort.Location = new System.Drawing.Point(232, 56);
			this.txtProxyPort.Name = "txtProxyPort";
			this.txtProxyPort.Size = new System.Drawing.Size(48, 20);
			this.txtProxyPort.TabIndex = 17;
			this.txtProxyPort.Text = "";
			// 
			// txtProxyHost
			// 
			this.txtProxyHost.Location = new System.Drawing.Point(72, 56);
			this.txtProxyHost.Name = "txtProxyHost";
			this.txtProxyHost.Size = new System.Drawing.Size(120, 20);
			this.txtProxyHost.TabIndex = 16;
			this.txtProxyHost.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(200, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 16);
			this.label2.TabIndex = 15;
			this.label2.Text = "Port";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 16);
			this.label5.TabIndex = 14;
			this.label5.Text = "Host";
			// 
			// tabPageGeneral
			// 
			this.tabPageGeneral.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.tabPageGeneral.Controls.Add(this.chbNotificationBalloon);
			this.tabPageGeneral.Controls.Add(this.chbPopupNotificationWindow);
			this.tabPageGeneral.Controls.Add(this.chbPartyMode);
			this.tabPageGeneral.Controls.Add(this.chbKeepOnTop);
			this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
			this.tabPageGeneral.Name = "tabPageGeneral";
			this.tabPageGeneral.Size = new System.Drawing.Size(320, 134);
			this.tabPageGeneral.TabIndex = 2;
			this.tabPageGeneral.Text = "General";
			// 
			// chbPopupNotificationWindow
			// 
			this.chbPopupNotificationWindow.Location = new System.Drawing.Point(8, 32);
			this.chbPopupNotificationWindow.Name = "chbPopupNotificationWindow";
			this.chbPopupNotificationWindow.Size = new System.Drawing.Size(192, 24);
			this.chbPopupNotificationWindow.TabIndex = 19;
			this.chbPopupNotificationWindow.Text = "Popup Notification Window";
			this.chbPopupNotificationWindow.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// chbPartyMode
			// 
			this.chbPartyMode.Location = new System.Drawing.Point(8, 80);
			this.chbPartyMode.Name = "chbPartyMode";
			this.chbPartyMode.Size = new System.Drawing.Size(280, 24);
			this.chbPartyMode.TabIndex = 18;
			this.chbPartyMode.Text = "Party Mode (keep playing if left for too long)";
			this.chbPartyMode.Visible = false;
			this.chbPartyMode.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// chbKeepOnTop
			// 
			this.chbKeepOnTop.Location = new System.Drawing.Point(8, 8);
			this.chbKeepOnTop.Name = "chbKeepOnTop";
			this.chbKeepOnTop.Size = new System.Drawing.Size(240, 24);
			this.chbKeepOnTop.TabIndex = 17;
			this.chbKeepOnTop.Text = "Keep On Top Of Other Windows";
			this.chbKeepOnTop.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// tabPageSendSongInfo
			// 
			this.tabPageSendSongInfo.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.tabPageSendSongInfo.Controls.Add(this.chbSendToSkype);
			this.tabPageSendSongInfo.Controls.Add(this.chbSendToXfire);
			this.tabPageSendSongInfo.Controls.Add(this.chbSendToMessenger);
			this.tabPageSendSongInfo.Location = new System.Drawing.Point(4, 22);
			this.tabPageSendSongInfo.Name = "tabPageSendSongInfo";
			this.tabPageSendSongInfo.Size = new System.Drawing.Size(320, 134);
			this.tabPageSendSongInfo.TabIndex = 5;
			this.tabPageSendSongInfo.Text = "Plugins";
			// 
			// chbSendToSkype
			// 
			this.chbSendToSkype.Location = new System.Drawing.Point(8, 56);
			this.chbSendToSkype.Name = "chbSendToSkype";
			this.chbSendToSkype.Size = new System.Drawing.Size(192, 24);
			this.chbSendToSkype.TabIndex = 24;
			this.chbSendToSkype.Text = "Send Song Info To Skype";
			// 
			// chbSendToXfire
			// 
			this.chbSendToXfire.Location = new System.Drawing.Point(8, 32);
			this.chbSendToXfire.Name = "chbSendToXfire";
			this.chbSendToXfire.Size = new System.Drawing.Size(192, 24);
			this.chbSendToXfire.TabIndex = 23;
			this.chbSendToXfire.Text = "Send Song Info To Xfire";
			// 
			// chbSendToMessenger
			// 
			this.chbSendToMessenger.Location = new System.Drawing.Point(8, 8);
			this.chbSendToMessenger.Name = "chbSendToMessenger";
			this.chbSendToMessenger.Size = new System.Drawing.Size(248, 24);
			this.chbSendToMessenger.TabIndex = 22;
			this.chbSendToMessenger.Text = "Send Song Info To Messenger";
			// 
			// tabPageControl
			// 
			this.tabPageControl.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.tabPageControl.Controls.Add(this.linkHelpGlobalShortcuts);
			this.tabPageControl.Controls.Add(this.chbGlobalShortcuts);
			this.tabPageControl.Controls.Add(this.chbKeyboardMediaKeys);
			this.tabPageControl.Location = new System.Drawing.Point(4, 22);
			this.tabPageControl.Name = "tabPageControl";
			this.tabPageControl.Size = new System.Drawing.Size(320, 134);
			this.tabPageControl.TabIndex = 4;
			this.tabPageControl.Text = "Control";
			// 
			// linkHelpGlobalShortcuts
			// 
			this.linkHelpGlobalShortcuts.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
			this.linkHelpGlobalShortcuts.Location = new System.Drawing.Point(128, 38);
			this.linkHelpGlobalShortcuts.Name = "linkHelpGlobalShortcuts";
			this.linkHelpGlobalShortcuts.Size = new System.Drawing.Size(40, 16);
			this.linkHelpGlobalShortcuts.TabIndex = 21;
			this.linkHelpGlobalShortcuts.TabStop = true;
			this.linkHelpGlobalShortcuts.Text = "?";
			this.linkHelpGlobalShortcuts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHelpGlobalShortcuts_LinkClicked);
			// 
			// chbGlobalShortcuts
			// 
			this.chbGlobalShortcuts.Location = new System.Drawing.Point(8, 32);
			this.chbGlobalShortcuts.Name = "chbGlobalShortcuts";
			this.chbGlobalShortcuts.Size = new System.Drawing.Size(128, 24);
			this.chbGlobalShortcuts.TabIndex = 10;
			this.chbGlobalShortcuts.Text = "Global Shortcuts";
			this.chbGlobalShortcuts.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// chbKeyboardMediaKeys
			// 
			this.chbKeyboardMediaKeys.Location = new System.Drawing.Point(8, 8);
			this.chbKeyboardMediaKeys.Name = "chbKeyboardMediaKeys";
			this.chbKeyboardMediaKeys.Size = new System.Drawing.Size(200, 24);
			this.chbKeyboardMediaKeys.TabIndex = 9;
			this.chbKeyboardMediaKeys.Text = "Multimedia Keyboard";
			this.chbKeyboardMediaKeys.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// tabPageLastFM
			// 
			this.tabPageLastFM.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.tabPageLastFM.Controls.Add(this.pictureBoxLastFmAvatar);
			this.tabPageLastFM.Controls.Add(this.chbSubmitSkipped);
			this.tabPageLastFM.Controls.Add(this.rdbManual);
			this.tabPageLastFM.Controls.Add(this.rdbAutomatic);
			this.tabPageLastFM.Controls.Add(this.chbLastFmSubmit);
			this.tabPageLastFM.Controls.Add(this.txtLastFmPassword);
			this.tabPageLastFM.Controls.Add(this.txtLastFmUser);
			this.tabPageLastFM.Controls.Add(this.label4);
			this.tabPageLastFM.Controls.Add(this.label3);
			this.tabPageLastFM.Location = new System.Drawing.Point(4, 22);
			this.tabPageLastFM.Name = "tabPageLastFM";
			this.tabPageLastFM.Size = new System.Drawing.Size(320, 134);
			this.tabPageLastFM.TabIndex = 1;
			this.tabPageLastFM.Text = "Last.FM";
			// 
			// pictureBoxLastFmAvatar
			// 
			this.pictureBoxLastFmAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxLastFmAvatar.Location = new System.Drawing.Point(208, 38);
			this.pictureBoxLastFmAvatar.Name = "pictureBoxLastFmAvatar";
			this.pictureBoxLastFmAvatar.Size = new System.Drawing.Size(50, 50);
			this.pictureBoxLastFmAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxLastFmAvatar.TabIndex = 18;
			this.pictureBoxLastFmAvatar.TabStop = false;
			// 
			// chbSubmitSkipped
			// 
			this.chbSubmitSkipped.Location = new System.Drawing.Point(112, 94);
			this.chbSubmitSkipped.Name = "chbSubmitSkipped";
			this.chbSubmitSkipped.Size = new System.Drawing.Size(160, 16);
			this.chbSubmitSkipped.TabIndex = 17;
			this.chbSubmitSkipped.Text = "Submit Skipped Tracks";
			this.chbSubmitSkipped.Visible = false;
			this.chbSubmitSkipped.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// rdbManual
			// 
			this.rdbManual.Location = new System.Drawing.Point(24, 112);
			this.rdbManual.Name = "rdbManual";
			this.rdbManual.Size = new System.Drawing.Size(80, 16);
			this.rdbManual.TabIndex = 16;
			this.rdbManual.Text = "Manual";
			this.rdbManual.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// rdbAutomatic
			// 
			this.rdbAutomatic.Location = new System.Drawing.Point(24, 94);
			this.rdbAutomatic.Name = "rdbAutomatic";
			this.rdbAutomatic.Size = new System.Drawing.Size(80, 16);
			this.rdbAutomatic.TabIndex = 15;
			this.rdbAutomatic.Text = "Automatic";
			this.rdbAutomatic.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// chbLastFmSubmit
			// 
			this.chbLastFmSubmit.Location = new System.Drawing.Point(8, 8);
			this.chbLastFmSubmit.Name = "chbLastFmSubmit";
			this.chbLastFmSubmit.Size = new System.Drawing.Size(152, 24);
			this.chbLastFmSubmit.TabIndex = 14;
			this.chbLastFmSubmit.Text = "Submit To Last.FM";
			this.chbLastFmSubmit.CheckedChanged += new System.EventHandler(this.SettingChanged);
			// 
			// txtLastFmPassword
			// 
			this.txtLastFmPassword.Location = new System.Drawing.Point(88, 64);
			this.txtLastFmPassword.Name = "txtLastFmPassword";
			this.txtLastFmPassword.PasswordChar = '*';
			this.txtLastFmPassword.Size = new System.Drawing.Size(96, 20);
			this.txtLastFmPassword.TabIndex = 13;
			this.txtLastFmPassword.Text = "";
			this.txtLastFmPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtLastFmPassword_Validating);
			// 
			// txtLastFmUser
			// 
			this.txtLastFmUser.Location = new System.Drawing.Point(88, 40);
			this.txtLastFmUser.Name = "txtLastFmUser";
			this.txtLastFmUser.Size = new System.Drawing.Size(96, 20);
			this.txtLastFmUser.TabIndex = 12;
			this.txtLastFmUser.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 16);
			this.label4.TabIndex = 11;
			this.label4.Text = "Password";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.TabIndex = 10;
			this.label3.Text = "User";
			// 
			// chbNotificationBalloon
			// 
			this.chbNotificationBalloon.Location = new System.Drawing.Point(32, 56);
			this.chbNotificationBalloon.Name = "chbNotificationBalloon";
			this.chbNotificationBalloon.Size = new System.Drawing.Size(208, 24);
			this.chbNotificationBalloon.TabIndex = 20;
			this.chbNotificationBalloon.Text = "Show Classic Balloon";
			// 
			// SettingsView
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.Controls.Add(this.tabs);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.chbOpenLinksInDefaultBrowser);
			this.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.Name = "SettingsView";
			this.Size = new System.Drawing.Size(328, 192);
			this.VisibleChanged += new System.EventHandler(this.SettingsView_VisibleChanged);
			this.grbTitleFormat.ResumeLayout(false);
			this.tabs.ResumeLayout(false);
			this.tabPageDisplay.ResumeLayout(false);
			this.tabProxy.ResumeLayout(false);
			this.tabPageGeneral.ResumeLayout(false);
			this.tabPageSendSongInfo.ResumeLayout(false);
			this.tabPageControl.ResumeLayout(false);
			this.tabPageLastFM.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		//
		// Properties
		//
		
		#region internal Player Player
		internal Player Player
		{
			set { this.player = value; }
		}
		#endregion

		//
		// Public methods
		//

		#region public void SetLastFmAvatar(Bitmap avatarBitmap)
		public void SetLastFmAvatar(Bitmap avatarBitmap)
		{
			pictureBoxLastFmAvatar.Image = avatarBitmap;
		}
		#endregion

		//
		// Event handlers
		//
		
		#region private void SettingsView_VisibleChanged(object sender, System.EventArgs e)
		private void SettingsView_VisibleChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (this.Visible)
				{
					this.configuration = this.player.configuration.Clone();
				
					this.loading = true;
				
					RefreshSettings();
					Apply();
				
					this.loading = false;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				Debug.WriteLine(ex.StackTrace);
			}
		}
		#endregion
		
		#region private void btnSave_MouseEnter(object sender, System.EventArgs e)
		private void btnSave_MouseEnter(object sender, System.EventArgs e)
		{
			btnSave.BackColor = Color.FromArgb(BackColor.R + 30, BackColor.G + 30, BackColor.B + 30);
		}
		#endregion

		#region private void btnSave_MouseLeave(object sender, System.EventArgs e)
		private void btnSave_MouseLeave(object sender, System.EventArgs e)
		{
			btnSave.BackColor = BackColor;
		}
		#endregion

		#region private void btnApply_MouseEnter(object sender, System.EventArgs e)
		private void btnApply_MouseEnter(object sender, System.EventArgs e)
		{
			btnApply.BackColor = Color.FromArgb(BackColor.R + 30, BackColor.G + 30, BackColor.B + 30);
		}
		#endregion

		#region private void btnApply_MouseLeave(object sender, System.EventArgs e)
		private void btnApply_MouseLeave(object sender, System.EventArgs e)
		{
			btnApply.BackColor = BackColor;
		}
		#endregion

		#region private void btnClose_MouseEnter(object sender, System.EventArgs e)
		private void btnClose_MouseEnter(object sender, System.EventArgs e)
		{
			btnClose.BackColor = Color.FromArgb(BackColor.R + 30, BackColor.G + 30, BackColor.B + 30);
		}
		#endregion

		#region private void btnClose_MouseLeave(object sender, System.EventArgs e)
		private void btnClose_MouseLeave(object sender, System.EventArgs e)
		{
			btnClose.BackColor = BackColor;
		}
		#endregion
		
		#region private void btnApply_Click(object sender, System.EventArgs e)
		private void btnApply_Click(object sender, System.EventArgs e)
		{
			Apply();
		}
		#endregion

		#region private void btnClose_Click(object sender, System.EventArgs e)
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.ParentForm.Close();
		}
		#endregion

		#region private void btnSave_Click(object sender, System.EventArgs e)
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			this.Apply();
			this.ParentForm.Close();
		}
		#endregion
		
		#region private void rdbProxy_CheckedChanged(object sender, System.EventArgs e)
		private void rdbProxy_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rdbProxyManual.Checked)
			{
				txtProxyHost.Enabled = true;
				txtProxyPort.Enabled = true;
				txtProxyUser.Enabled = true;
				txtProxyPassword.Enabled = true;
			}
			else
			{
				configuration.ProxyHost = string.Empty;
				configuration.ProxyPort = -1;
				configuration.ProxyUser = string.Empty;
				configuration.ProxyPassword = string.Empty;
				
				txtProxyHost.Enabled = false;
				txtProxyPort.Enabled = false;
				txtProxyUser.Enabled = false;
				txtProxyPassword.Enabled = false;
			}
			
			UpdateSettings(this, new EventArgs());
		}
		#endregion
		
		#region private void SettingChanged(object sender, System.EventArgs e)
		private void SettingChanged(object sender, System.EventArgs e)
		{
			if (!loading)
			{
				UpdateSettings(sender, e);
			}
		}
		#endregion
		
		#region private void txtLastFmPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		private void txtLastFmPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.password = txtLastFmPassword.Text;
		}
		#endregion

		#region private void linkHelpGlobalShortcuts_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		private void linkHelpGlobalShortcuts_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				string helpUrl = @"http://openpandora.googlepages.com/help#1";
				Shell32.ShellExecute(0, "Open", helpUrl, "", Application.StartupPath, 1);
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
		
		#region private void Apply()
		private void Apply()
		{
			UpdateSettings(this, new EventArgs());
			
			if (password != string.Empty)
			{
				configuration.LastFmPassword = Audioscrobbler.GetPasswordMD5(this.password);
			}
			
			player.ApplyConfiguration(configuration, true);
		}
		#endregion
		
		#region private void RefreshSettings()
		private void RefreshSettings()
		{
			chbShowCloseButton.Checked = this.configuration.CloseButtonVisible;
			chbMinimizeToTrayOnClose.Checked = this.configuration.CloseButtonMinimizeToTray;
			chbSendToMessenger.Checked = this.configuration.SendToMessenger;
			chbSendToXfire.Checked = this.configuration.SendToXfire;
			chbSendToSkype.Checked = this.configuration.SendToSkype;
			chbOpenLinksInDefaultBrowser.Checked = this.configuration.OpenInDefaultBrowser;

			chbShowCloseButton.Enabled = this.configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.CloseButtonVisible);
			chbMinimizeToTrayOnClose.Enabled = this.configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.CloseButtonMinimizeToTray);
			chbSendToMessenger.Enabled = this.configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.SendToMessenger);
			chbOpenLinksInDefaultBrowser.Enabled = this.configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.OpenInDefaultBrowser);

			if (this.configuration.TitleTemplate == rdbTitleTemplate1.Text &&
			    !rdbTitleTemplate3.Checked)
			{
				rdbTitleTemplate1.Checked = true;
				txtTitleTemplate.Enabled = false;
				txtTitleTemplate.Text = rdbTitleTemplate1.Text;
			}
			else if (this.configuration.TitleTemplate == rdbTitleTemplate2.Text &&
			         !rdbTitleTemplate3.Checked)
			{
				rdbTitleTemplate2.Checked = true;
				txtTitleTemplate.Enabled = false;
				txtTitleTemplate.Text = rdbTitleTemplate2.Text;
			}
			else
			{
				rdbTitleTemplate3.Checked = true;
				txtTitleTemplate.Enabled = true;
				
				txtTitleTemplate.Text = this.configuration.TitleTemplate;
			}
			
			chbKeepOnTop.Checked = this.configuration.KeepOnTop;
			chbPartyMode.Checked = this.configuration.PartyMode;
			chbPopupNotificationWindow.Checked = this.configuration.NotificationWindow;
			chbNotificationBalloon.Checked = this.configuration.NotificationWindowBalloon;
			chbNotificationBalloon.Enabled = this.configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.NotificationWindowBalloon);
			
			
			//
			// Last.fm
			
			chbLastFmSubmit.Checked = configuration.LastFmSubmit;
			txtLastFmUser.Text = configuration.LastFmUser;
			
			if (configuration.LastFmPassword != string.Empty)
			{
				txtLastFmPassword.Text = "password";
			}
			else
			{
				txtLastFmPassword.Text = string.Empty;
			}
			
			rdbAutomatic.Checked = configuration.LastFmSubmitAutomatic;
			rdbManual.Checked = configuration.LastFmSubmitManual;
			chbSubmitSkipped.Checked = configuration.LastFmSubmitSkipped;
			
			txtLastFmUser.Enabled = configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.LastFmUser);
			txtLastFmPassword.Enabled = configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.LastFmPassword);
			rdbAutomatic.Enabled = configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.LastFmSubmitAutomatic);
			rdbManual.Enabled = configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.LastFmSubmitManual);
			chbSubmitSkipped.Enabled = configuration.IsConfigurationItemEnabled(Configuration.ConfigurationItemType.LastFmSubmitSkipped);
			
			//
			// Keyboard
			
			chbKeyboardMediaKeys.Checked = configuration.KeyboardMediaKeys;
			chbGlobalShortcuts.Checked = configuration.GlobalShortcuts;
			
			//
			// Proxy
			
			txtProxyHost.Text = configuration.ProxyHost;
			txtProxyPort.Text = (configuration.ProxyPort == -1 ? string.Empty : configuration.ProxyPort.ToString());
			txtProxyUser.Text = configuration.ProxyUser;
			txtProxyPassword.Text = configuration.ProxyPassword;
			
			if (!rdbProxyAuto.Checked && !rdbProxyManual.Checked)
			{
				if (configuration.ProxyPort == -1)
				{
					rdbProxyAuto.Checked = true;
				}
				else
				{
					rdbProxyManual.Checked = true;
				}
			}
		}
		#endregion
		
		#region private void UpdateSettings()
		private void UpdateSettings(object sender, System.EventArgs e)
		{
			configuration.CloseButtonVisible = chbShowCloseButton.Checked;
			configuration.CloseButtonMinimizeToTray = chbMinimizeToTrayOnClose.Checked;
			configuration.SendToMessenger = chbSendToMessenger.Checked;
			configuration.SendToXfire = chbSendToXfire.Checked;
			configuration.SendToSkype = chbSendToSkype.Checked;
			configuration.OpenInDefaultBrowser = chbOpenLinksInDefaultBrowser.Checked;
			
			if (rdbTitleTemplate1.Checked)
			{
				configuration.TitleTemplate = rdbTitleTemplate1.Text;
			}
			else if (rdbTitleTemplate2.Checked)
			{
				configuration.TitleTemplate = rdbTitleTemplate2.Text;
			}
			else if (rdbTitleTemplate3.Checked)
			{
				if (txtTitleTemplate.Text.Trim() != string.Empty)
				{
					configuration.TitleTemplate = txtTitleTemplate.Text;
				}
			}
			
			configuration.LastFmSubmit = chbLastFmSubmit.Checked;
			configuration.LastFmUser = txtLastFmUser.Text;
			
			configuration.LastFmSubmitAutomatic = rdbAutomatic.Checked;
			configuration.LastFmSubmitManual = rdbManual.Checked;
			configuration.LastFmSubmitSkipped = chbSubmitSkipped.Checked;
			
			configuration.KeyboardMediaKeys = chbKeyboardMediaKeys.Checked;
			configuration.GlobalShortcuts = chbGlobalShortcuts.Checked;
			configuration.KeepOnTop = chbKeepOnTop.Checked;
			configuration.PartyMode = chbPartyMode.Checked;
			configuration.NotificationWindow = chbPopupNotificationWindow.Checked;
			configuration.NotificationWindowBalloon = chbNotificationBalloon.Checked;
			
			if (rdbProxyManual.Checked)
			{
				configuration.ProxyHost = txtProxyHost.Text;
				configuration.ProxyPort = -1;
				
				try
				{
					configuration.ProxyPort = Int32.Parse(txtProxyPort.Text);
				}catch {}
				
				configuration.ProxyUser = txtProxyUser.Text;
				configuration.ProxyPassword = txtProxyPassword.Text;
			}
			else
			{
				configuration.ProxyHost = string.Empty;
				configuration.ProxyPort = -1;
				configuration.ProxyUser = string.Empty;
				configuration.ProxyPassword = string.Empty;
			}
			
			RefreshSettings();
		}
		#endregion
				
		//
		// Private data
		//
		
		private Player player;
		private Configuration configuration;
		private bool loading = false;
		private string password = string.Empty;
	}
}
