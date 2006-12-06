using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace OpenPandora.Windows.Forms
{

	public class TaskbarNotifier : System.Windows.Forms.Form
	{
		[DllImport("user32.dll")]
		private static extern Boolean ShowWindow(IntPtr hWnd,Int32 nCmdShow);

		const int CS_DROPSHADOW = 0x20000;

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

		private Rectangle WorkAreaRectangle;
		private Timer timer = new Timer();
		private TaskbarStates taskbarState = TaskbarStates.hidden;
		private int ShowEvents;
		protected int HideEvents;
		private int VisibleEvents;
		private int IncrementShow;
		private int IncrementHide;

		private bool IsMouseOverPopup = false;

		System.Resources.ResourceManager resourcesBackGround = new System.Resources.ResourceManager(typeof(TaskbarNotifier));
		
		private OpenPandora.Windows.Forms.LinkLabelNoFocus linkLabelAlbum;
		private OpenPandora.Windows.Forms.LinkLabelNoFocus linkLabelArtist;
		private OpenPandora.Windows.Forms.LinkLabelNoFocus linkLabelSongName;
		private System.Windows.Forms.Label lblOnAlbum;
		private System.Windows.Forms.Label lblByArtist;
		private System.Windows.Forms.PictureBox pictureBoxAlbumArt;

		private System.Net.WebClient wc;
		private byte[] buffer;
		private OpenPandora.Windows.Forms.TransparentPanel TransparentPanelMouse;
		private System.IO.MemoryStream stream;
		private System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
		private string songsource;
		private System.Text.RegularExpressions.Regex regex;
		private System.Text.RegularExpressions.Match match;
	
		#region private void InitializeComponent()
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TaskbarNotifier));
			this.pictureBoxAlbumArt = new System.Windows.Forms.PictureBox();
			this.linkLabelAlbum = new OpenPandora.Windows.Forms.LinkLabelNoFocus();
			this.linkLabelArtist = new OpenPandora.Windows.Forms.LinkLabelNoFocus();
			this.linkLabelSongName = new OpenPandora.Windows.Forms.LinkLabelNoFocus();
			this.lblOnAlbum = new System.Windows.Forms.Label();
			this.lblByArtist = new System.Windows.Forms.Label();
			this.TransparentPanelMouse = new OpenPandora.Windows.Forms.TransparentPanel();
			this.SuspendLayout();
			// 
			// pictureBoxAlbumArt
			// 
			this.pictureBoxAlbumArt.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(204)), ((System.Byte)(204)), ((System.Byte)(204)));
			this.pictureBoxAlbumArt.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxAlbumArt.Image")));
			this.pictureBoxAlbumArt.Location = new System.Drawing.Point(13, 67);
			this.pictureBoxAlbumArt.Name = "pictureBoxAlbumArt";
			this.pictureBoxAlbumArt.Size = new System.Drawing.Size(100, 100);
			this.pictureBoxAlbumArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxAlbumArt.TabIndex = 0;
			this.pictureBoxAlbumArt.TabStop = false;
			// 
			// linkLabelAlbum
			// 
			this.linkLabelAlbum.ActiveLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelAlbum.BackColor = System.Drawing.Color.Transparent;
			this.linkLabelAlbum.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelAlbum.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.linkLabelAlbum.FullText = null;
			this.linkLabelAlbum.LinkArea = new System.Windows.Forms.LinkArea(0, 25);
			this.linkLabelAlbum.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabelAlbum.LinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelAlbum.Location = new System.Drawing.Point(32, 50);
			this.linkLabelAlbum.Name = "linkLabelAlbum";
			this.linkLabelAlbum.ShortText = null;
			this.linkLabelAlbum.Size = new System.Drawing.Size(80, 14);
			this.linkLabelAlbum.TabIndex = 1;
			this.linkLabelAlbum.TabStop = true;
			this.linkLabelAlbum.Text = "Album";
			this.linkLabelAlbum.URL = null;
			this.linkLabelAlbum.UseMnemonic = false;
			this.linkLabelAlbum.VisitedLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelAlbum.TextChanged += new System.EventHandler(this.linkLabel_TextChanged);
			// 
			// linkLabelArtist
			// 
			this.linkLabelArtist.ActiveLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelArtist.BackColor = System.Drawing.Color.Transparent;
			this.linkLabelArtist.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelArtist.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.linkLabelArtist.FullText = null;
			this.linkLabelArtist.LinkArea = new System.Windows.Forms.LinkArea(0, 25);
			this.linkLabelArtist.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabelArtist.LinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelArtist.Location = new System.Drawing.Point(32, 36);
			this.linkLabelArtist.Name = "linkLabelArtist";
			this.linkLabelArtist.ShortText = null;
			this.linkLabelArtist.Size = new System.Drawing.Size(80, 14);
			this.linkLabelArtist.TabIndex = 2;
			this.linkLabelArtist.TabStop = true;
			this.linkLabelArtist.Text = "Artist";
			this.linkLabelArtist.URL = null;
			this.linkLabelArtist.UseMnemonic = false;
			this.linkLabelArtist.VisitedLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelArtist.TextChanged += new System.EventHandler(this.linkLabel_TextChanged);
			// 
			// linkLabelSongName
			// 
			this.linkLabelSongName.ActiveLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelSongName.BackColor = System.Drawing.Color.Transparent;
			this.linkLabelSongName.DisabledLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelSongName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.linkLabelSongName.FullText = null;
			this.linkLabelSongName.LinkArea = new System.Windows.Forms.LinkArea(0, 25);
			this.linkLabelSongName.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.linkLabelSongName.LinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelSongName.Location = new System.Drawing.Point(13, 8);
			this.linkLabelSongName.Name = "linkLabelSongName";
			this.linkLabelSongName.ShortText = null;
			this.linkLabelSongName.Size = new System.Drawing.Size(100, 28);
			this.linkLabelSongName.TabIndex = 3;
			this.linkLabelSongName.TabStop = true;
			this.linkLabelSongName.Text = "Song Name";
			this.linkLabelSongName.URL = null;
			this.linkLabelSongName.UseMnemonic = false;
			this.linkLabelSongName.VisitedLinkColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.linkLabelSongName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
			this.linkLabelSongName.TextChanged += new System.EventHandler(this.linkLabelSong_TextChanged);
			// 
			// lblOnAlbum
			// 
			this.lblOnAlbum.BackColor = System.Drawing.Color.Transparent;
			this.lblOnAlbum.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.lblOnAlbum.Location = new System.Drawing.Point(13, 50);
			this.lblOnAlbum.Name = "lblOnAlbum";
			this.lblOnAlbum.Size = new System.Drawing.Size(24, 23);
			this.lblOnAlbum.TabIndex = 4;
			this.lblOnAlbum.Text = "on:";
			// 
			// lblByArtist
			// 
			this.lblByArtist.BackColor = System.Drawing.Color.Transparent;
			this.lblByArtist.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(68)), ((System.Byte)(68)), ((System.Byte)(68)));
			this.lblByArtist.Location = new System.Drawing.Point(13, 36);
			this.lblByArtist.Name = "lblByArtist";
			this.lblByArtist.Size = new System.Drawing.Size(24, 23);
			this.lblByArtist.TabIndex = 5;
			this.lblByArtist.Text = "by:";
			// 
			// TransparentPanelMouse
			// 
			this.TransparentPanelMouse.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TransparentPanelMouse.Location = new System.Drawing.Point(0, 0);
			this.TransparentPanelMouse.Name = "TransparentPanelMouse";
			this.TransparentPanelMouse.Size = new System.Drawing.Size(128, 179);
			this.TransparentPanelMouse.TabIndex = 6;
			this.TransparentPanelMouse.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Control_MouseUp);
			this.TransparentPanelMouse.MouseEnter += new System.EventHandler(this.TransparentPanelMouse_MouseEnter);
			this.TransparentPanelMouse.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_MouseMove);
			this.TransparentPanelMouse.MouseLeave += new System.EventHandler(this.TransparentPanelMouse_MouseLeave);
			// 
			// TaskbarNotifier
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(128, 179);
			this.Controls.Add(this.TransparentPanelMouse);
			this.Controls.Add(this.pictureBoxAlbumArt);
			this.Controls.Add(this.linkLabelAlbum);
			this.Controls.Add(this.lblOnAlbum);
			this.Controls.Add(this.linkLabelArtist);
			this.Controls.Add(this.linkLabelSongName);
			this.Controls.Add(this.lblByArtist);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "TaskbarNotifier";
			this.Opacity = 0;
			this.TransparencyKey = System.Drawing.Color.Magenta;
			this.Load += new System.EventHandler(this.TaskbarNotifier_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.TaskbarNotifier_Paint);
			this.ResumeLayout(false);

		}

		#endregion

		//
		// Constructor
		//
		#region TaskbarNotifier Constructor
		public TaskbarNotifier()
		{

			InitializeComponent();

			FormBorderStyle = FormBorderStyle.None;
			WindowState = FormWindowState.Minimized;
			base.Show();
			base.Hide();
			WindowState = FormWindowState.Normal;
			ShowInTaskbar = false;
			TopMost = true;
			MaximizeBox = false;
			MinimizeBox = false;
			ControlBox = false;

			wc = new System.Net.WebClient();

			timer.Enabled = true;
			timer.Tick += new EventHandler(OnTimer);

		}
		#endregion

		#region TaskbarNotifier Enums
		public enum TaskbarStates
		{
			hidden = 0,
			appearing = 1,
			visible = 2,
			disappearing = 3
		}
		#endregion

		//
		// Public Methods
		//
		#region public void Show(string songname, string artist, string album, string songarturl, string songurl, string artisturl, string albumurl, int x, int y)
		public void Show(string songname, string artist, string album, string songarturl, string songurl, string artisturl, string albumurl, int x, int y)
		{
			BuildDisplay(songname, artist, album, songarturl, songurl, artisturl, albumurl);

			this.Left = x;
			this.Top = y;	
			this.Width = 128;
			this.Height = 179;

			taskbarState = TaskbarStates.visible;
			this.Opacity = 1.0;
			this.TopMost = true;
			ShowWindow(this.Handle, 4);

		}

		#endregion

		#region public void Show(string songname, string artist, string album, string songarturl, string songurl, string artisturl, string albumurl, int x, int y, int timetoshow, int timetostay, int timetohide)
		public void Show(string songname, string artist, string album, string songarturl, string songurl, string artisturl, string albumurl, int x, int y, int timetoshow, int timetostay, int timetohide)
		{
			BuildDisplay(songname, artist, album, songarturl, songurl, artisturl, albumurl);

			WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);

			SetBounds(WorkAreaRectangle.Right - 128 - x, WorkAreaRectangle.Bottom - 179 - y, 128, 179);
			
			VisibleEvents = timetostay;

			int Events;
			if (timetoshow > 10)
			{
				Events = Math.Min((timetoshow / 10), 100);
				ShowEvents = timetoshow / Events;
				IncrementShow = 100 / Events;
			}
			else
			{
				ShowEvents = 10;
				IncrementShow = 10;
			}

			if( timetohide > 10)
			{
				Events = Math.Min((timetohide / 10), 100);
				HideEvents = timetohide / Events;
				IncrementHide = 100 / Events;
			}
			else
			{
				HideEvents = 10;
				IncrementHide = 10;
			}

			switch (taskbarState)
			{
				case TaskbarStates.hidden:
					this.Opacity = 0;
					taskbarState = TaskbarStates.appearing;
					timer.Interval = ShowEvents;
					timer.Start();
					ShowWindow(this.Handle, 4);
					break;

				case TaskbarStates.appearing:
					Refresh();
					break;

				case TaskbarStates.visible:
					timer.Stop();
					timer.Interval = VisibleEvents;
					timer.Start();
					Refresh();
					break;

				case TaskbarStates.disappearing:
					timer.Stop();
					taskbarState = TaskbarStates.visible;
					timer.Interval = VisibleEvents;
					timer.Start();
					Refresh();
					break;
			}
		}

		#endregion

		#region public new void Hide()
		public new void Hide()
		{
			if (taskbarState != TaskbarStates.hidden)
			{
				timer.Stop();
				taskbarState = TaskbarStates.hidden;
				base.Opacity = 0.0;
				base.Hide();
			}
		}

		#endregion

		#region public void Hide(int timetostay)
		public void Hide(int timetostay)
		{
			timer.Stop();
			HideEvents = 10;
			IncrementHide = 100;
			timer.Interval = timetostay;
			timer.Start();
			Refresh();
		}

		#endregion

		//
		// Private methods
		//
		#region private void BuildShortText(OpenPandora.Windows.Forms.LinkLabelNoFocus linkLabel, int maxwidth)
		private void BuildShortText(OpenPandora.Windows.Forms.LinkLabelNoFocus linkLabel, int maxwidth)
		{
			Graphics textGraphics = linkLabel.CreateGraphics();

			SizeF textSize = textGraphics.MeasureString(linkLabel.Text,linkLabel.Font);

			if (textSize.Width > maxwidth)
			{
				linkLabel.ShortText = linkLabel.ShortText.Substring(0,linkLabel.ShortText.Length-1);
				linkLabel.Text = linkLabel.ShortText + "...";
			}

			textGraphics.Dispose();
		}
		#endregion

		#region private void BuildShortText2Lines(OpenPandora.Windows.Forms.LinkLabelNoFocus linkLabel, int maxwidth)
		private void BuildShortText2Lines(OpenPandora.Windows.Forms.LinkLabelNoFocus linkLabel, int maxwidth)
		{
			Graphics textGraphics = linkLabel.CreateGraphics();

			StringFormat sf = new StringFormat();
			sf.FormatFlags = StringFormatFlags.NoWrap;

			SizeF textSize = textGraphics.MeasureString(linkLabel.Text,linkLabel.Font,maxwidth/2,sf);

			if (textSize.Width > maxwidth/2)
			{
				linkLabel.ShortText = linkLabel.ShortText.Substring(0,linkLabel.ShortText.Length-1);
				linkLabel.Text = linkLabel.ShortText + "...";
			}

			textGraphics.Dispose();
		}
		#endregion

		#region private void BuildDisplay(string songname, string artist, string album, string songarturl, string songurl, string artisturl, string albumurl)
		private void BuildDisplay(string songname, string artist, string album, string songarturl, string songurl, string artisturl, string albumurl)
		{
			ResetLinks();

			if (linkLabelSongName.URL != songurl)
			{
            
				if (songarturl == null || songarturl == "")
				{
					Debug.WriteLine("TaskbarNotifier: No ArtURL");
					this.pictureBoxAlbumArt.Image = ((System.Drawing.Image)(resourcesBackGround.GetObject("pictureBoxAlbumArt.Image")));
				}
				else
				{
            
					try
					{
						buffer = DownloadData(songarturl);

						stream = new System.IO.MemoryStream(buffer);
						pictureBoxAlbumArt.Image = Image.FromStream(stream);
					}
					catch (Exception ex)
					{
						Debug.WriteLine("TaskbarNotifier: " + ex.Message);
					}
				}

				GetAlbumAndURL(songurl);

				linkLabelSongName.FullText = songname;
				linkLabelArtist.FullText = artist;
				linkLabelSongName.URL = songurl;
			}
		}

		#endregion

		#region private byte[] DownloadData(string url)
		private byte[] DownloadData(string url)
		{
			System.IO.Stream stream;
			stream = wc.OpenRead(url);
			
			byte[] buffer = new byte[512];
			int read=0;
    
			int chunk;
			while ( (chunk = stream.Read(buffer, read, buffer.Length-read)) > 0)
			{
				read += chunk;
        
				if (read == buffer.Length)
				{
					Application.DoEvents();

					int nextByte = stream.ReadByte();
            
					if (nextByte==-1)
					{
						return buffer;
					}
            
					byte[] newBuffer = new byte[buffer.Length*2];
					Array.Copy(buffer, newBuffer, buffer.Length);
					newBuffer[read]=(byte)nextByte;
					buffer = newBuffer;
					read++;
				}
			}
			
			byte[] ret = new byte[read];
			Array.Copy(buffer, ret, read);

			stream.Close();

			return ret;


		}

		#endregion

		#region private void drawBackground()
		private void drawBackground()
		{
			GraphicsPath Path = new GraphicsPath(); 
			Bitmap b = ((System.Drawing.Bitmap)(resourcesBackGround.GetObject("$this.BackgroundImage")));

			Rectangle rect; 
			Color colorFirst = b.GetPixel(1, 1); 
			for (int x = 0; x <= b.Width - 1; x++) 
			{ 
				for (int y = 0; y <= b.Height - 1; y++) 
				{ 
					if (b.GetPixel(x, y).Equals(colorFirst)) 
					{ 
					} 
					else 
					{ 
						rect = new Rectangle(x, y, 1, 1); 
						Path.AddRectangle(rect); 
					} 
				} 
			} 
			this.Region = new Region(Path);

		}

		#endregion

		#region private void ResetLinks()
		private void ResetLinks()
		{
			this.Cursor = Cursors.Default;
                
			linkLabelSongName.LinkBehavior = LinkBehavior.HoverUnderline;
			linkLabelSongName.LinkColor = Color.FromArgb(68,68,68);
                
			linkLabelArtist.LinkBehavior = LinkBehavior.HoverUnderline;
			linkLabelArtist.LinkColor = Color.FromArgb(68,68,68);
                
			linkLabelAlbum.LinkBehavior = LinkBehavior.HoverUnderline;
			linkLabelAlbum.LinkColor = Color.FromArgb(68,68,68);
		}
		#endregion

		#region private void GetAlbumAndURL(string songurl)
		private void GetAlbumAndURL(string songurl)
		{
			try
			{
				buffer = DownloadData(songurl);
				
				songsource = encoder.GetString(buffer);

				regex = new Regex("<div id=.artist.>.*?</div>",RegexOptions.Singleline);
				match = regex.Match(songsource);
				if (match.Success)
				{
					regex = new Regex("href=.*?\".*?\"",RegexOptions.Singleline);
					match = regex.Match(match.Value);
					if (match.Success)
					{
						linkLabelArtist.URL = match.Value.Replace("href=\"","").Replace("\"","");
					}

				}

				regex = new Regex("<div id=.album.>.*?</div>",RegexOptions.Singleline);
				match = regex.Match(songsource);
				if (match.Success)
				{
					linkLabelAlbum.FullText = Regex.Replace(match.Value,"<.*?>","",RegexOptions.Singleline).Replace("\n","").Replace("\t","");

					regex = new Regex("href=.*?\".*?\"",RegexOptions.Singleline);
					match = regex.Match(match.Value);
					if (match.Success)
					{
						linkLabelAlbum.URL = match.Value.Replace("href=\"","").Replace("\"","");
					}

				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("TaskbarNotifier: " + ex.Message);
			}
			
		}

		#endregion

        //
		// Event Handlers
		//
		#region private void linkLabel_TextChanged(object sender, System.EventArgs e)
		private void linkLabel_TextChanged(object sender, System.EventArgs e)
		{
		
			BuildShortText((OpenPandora.Windows.Forms.LinkLabelNoFocus)sender, 80);

		}
		#endregion

		#region private void linkLabelSong_TextChanged(object sender, System.EventArgs e)
		private void linkLabelSong_TextChanged(object sender, System.EventArgs e)
		{
		
			BuildShortText2Lines((OpenPandora.Windows.Forms.LinkLabelNoFocus)sender, 200);

		}
		#endregion

		#region private void TaskbarNotifier_Load(object sender, System.EventArgs e)
		private void TaskbarNotifier_Load(object sender, System.EventArgs e)
		{

			drawBackground();

		}
		#endregion

		#region private void OnTimer(Object obj, EventArgs ea)
		private void OnTimer(Object obj, EventArgs ea)
		{
			switch (taskbarState)
			{
				case TaskbarStates.appearing:
					if ((this.Opacity * 100) < 100)
						this.Opacity = this.Opacity + (double)IncrementShow / 100;
					else
					{
						timer.Stop();
						timer.Interval = VisibleEvents;
						taskbarState = TaskbarStates.visible;
						timer.Start();
					}
					break;

				case TaskbarStates.visible:
					timer.Stop();
					timer.Interval = HideEvents;
					if (!IsMouseOverPopup)
					{
						taskbarState = TaskbarStates.disappearing;
					} 
					timer.Start();
					break;

				case TaskbarStates.disappearing:
					if (IsMouseOverPopup) 
					{
						taskbarState = TaskbarStates.appearing;
					} 
					else 
					{
						if (this.Opacity > 0)
							this.Opacity = this.Opacity - (double)IncrementHide / 100;
						else
							Hide();
					}
					break;
			}
			
		}

		#endregion

		#region private void TaskbarNotifier_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		private void TaskbarNotifier_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = this.CreateGraphics(); 
			Bitmap b =  ((System.Drawing.Bitmap)(resourcesBackGround.GetObject("$this.BackgroundImage")));
			g.DrawImage(b, 0, 0, b.Width, b.Height); 
			g.Dispose();
		}
		#endregion

		#region private void linkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		private void linkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			Shell32.ShellExecute(0, "Open", ((OpenPandora.Windows.Forms.LinkLabelNoFocus)sender).URL , "", Application.StartupPath, 1);
		}
		#endregion

		#region private void TransparentPanelMouse_MouseEnter(object sender, System.EventArgs e)
		private void TransparentPanelMouse_MouseEnter(object sender, System.EventArgs e)
		{
			Debug.WriteLine("MouseEnter");
			IsMouseOverPopup = true;
		}
		#endregion

		#region private void TransparentPanelMouse_MouseLeave(object sender, System.EventArgs e)
		private void TransparentPanelMouse_MouseLeave(object sender, System.EventArgs e)
		{
			Debug.WriteLine("MouseLeave");
			IsMouseOverPopup = false;
			ResetLinks();
		}
		#endregion

		#region private void Control_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		private void Control_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.X >= linkLabelSongName.Location.X) && (e.Y >= linkLabelSongName.Location.Y) && (e.X <= linkLabelSongName.Location.X + linkLabelSongName.Width) && (e.Y <= linkLabelSongName.Location.Y + linkLabelSongName.Height))
			{
				this.Cursor = Cursors.Hand;
				linkLabelSongName.LinkBehavior = LinkBehavior.AlwaysUnderline;
				linkLabelSongName.LinkColor = Color.FromArgb(204,102,0);

				linkLabelArtist.LinkBehavior = LinkBehavior.HoverUnderline;
				linkLabelArtist.LinkColor = Color.FromArgb(68,68,68);

				linkLabelAlbum.LinkBehavior = LinkBehavior.HoverUnderline;
				linkLabelAlbum.LinkColor = Color.FromArgb(68,68,68);
				
			}
			else if ((e.X >= linkLabelArtist.Location.X) && (e.Y >= linkLabelArtist.Location.Y) && (e.X <= linkLabelArtist.Location.X + linkLabelArtist.Width) && (e.Y <= linkLabelArtist.Location.Y + linkLabelArtist.Height))
			{
				linkLabelSongName.LinkBehavior = LinkBehavior.HoverUnderline;
				linkLabelSongName.LinkColor = Color.FromArgb(68,68,68);

				this.Cursor = Cursors.Hand;
				linkLabelArtist.LinkBehavior = LinkBehavior.AlwaysUnderline;
				linkLabelArtist.LinkColor = Color.FromArgb(204,102,0);

				linkLabelAlbum.LinkBehavior = LinkBehavior.HoverUnderline;
				linkLabelAlbum.LinkColor = Color.FromArgb(68,68,68);
			}
			else if ((e.X >= linkLabelAlbum.Location.X) && (e.Y >= linkLabelAlbum.Location.Y) && (e.X <= linkLabelAlbum.Location.X + linkLabelAlbum.Width) && (e.Y <= linkLabelAlbum.Location.Y + linkLabelAlbum.Height))
			{
				linkLabelSongName.LinkBehavior = LinkBehavior.HoverUnderline;
				linkLabelSongName.LinkColor = Color.FromArgb(68,68,68);

				linkLabelArtist.LinkBehavior = LinkBehavior.HoverUnderline;
				linkLabelArtist.LinkColor = Color.FromArgb(68,68,68);

				this.Cursor = Cursors.Hand;
				linkLabelAlbum.LinkBehavior = LinkBehavior.AlwaysUnderline;
				linkLabelAlbum.LinkColor = Color.FromArgb(204,102,0);
			}
			else
			{
				ResetLinks();
			}

			this.Refresh();
		}

		#endregion

		#region private void Control_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		private void Control_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if ((e.X >= linkLabelSongName.Location.X) && (e.Y >= linkLabelSongName.Location.Y) && (e.X <= linkLabelSongName.Location.X + linkLabelSongName.Width) && (e.Y <= linkLabelSongName.Location.Y + linkLabelSongName.Height))
			{
				Shell32.ShellExecute(0, "Open", linkLabelSongName.URL , "", Application.StartupPath, 1);
			}

			if ((e.X >= linkLabelAlbum.Location.X) && (e.Y >= linkLabelAlbum.Location.Y) && (e.X <= linkLabelAlbum.Location.X + linkLabelAlbum.Width) && (e.Y <= linkLabelAlbum.Location.Y + linkLabelAlbum.Height))
			{
				Shell32.ShellExecute(0, "Open", linkLabelAlbum.URL , "", Application.StartupPath, 1);
			}

			if ((e.X >= linkLabelArtist.Location.X) && (e.Y >= linkLabelArtist.Location.Y) && (e.X <= linkLabelArtist.Location.X + linkLabelArtist.Width) && (e.Y <= linkLabelArtist.Location.Y + linkLabelArtist.Height))
			{
				Shell32.ShellExecute(0, "Open", linkLabelArtist.URL , "", Application.StartupPath, 1);
			}
		}

		#endregion

		
	}
}
