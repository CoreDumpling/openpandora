using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OpenPandora
{
	public class SettingsForm : OpenPandora.BaseForm
	{
		private OpenPandora.SettingsView settingsView;
		private System.ComponentModel.IContainer components = null;

		public SettingsForm()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.settingsView = new OpenPandora.SettingsView();
			this.SuspendLayout();
			// 
			// settingsView
			// 
			this.settingsView.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(49)), ((System.Byte)(49)), ((System.Byte)(49)));
			this.settingsView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.settingsView.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.settingsView.Location = new System.Drawing.Point(1, 19);
			this.settingsView.Name = "settingsView";
			this.settingsView.Size = new System.Drawing.Size(348, 220);
			this.settingsView.TabIndex = 1;
			// 
			// SettingsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(350, 240);
			this.Controls.Add(this.settingsView);
			this.DockPadding.Bottom = 1;
			this.DockPadding.Left = 1;
			this.DockPadding.Right = 1;
			this.DockPadding.Top = 19;
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "SettingsForm";
			this.Text = "Settings ";
			this.Controls.SetChildIndex(this.settingsView, 0);
			this.ResumeLayout(false);

		}
		#endregion

		//
		// Public properties
		//

		#region public SettingsView SettingsView
		public SettingsView SettingsView
		{
			get
			{
				return this.settingsView;
			} 
		}
		#endregion

		#region protected override CreateParams CreateParams
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x80; // Turn on WS_EX_TOOLWINDOW style bit
				return cp;
			}
		}
		#endregion
	}
}

