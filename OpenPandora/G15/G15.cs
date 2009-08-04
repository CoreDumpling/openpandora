using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

using lgLcdClassLibrary;

namespace OpenPandora
{
    public class G15 
    {
        private string appName = "OpenPandora";
        
		//
		//Necessary Variables
		//

        private LCDInterface lcd;

        private Graphics lcd_graphics;
        private Brush brush;
        private Bitmap bmp;

        private int lcd_width = 160;
        private int lcd_height = 43;   

		// setup the string formatting
        private StringFormat format_left;
        private StringFormat format_right;
        private StringFormat format_center;

		//
		// Constructor
		//

        #region  public G15()
        public G15()
        {          
            lcd = new LCDInterface();
            
            // setup the string formatting
            format_left = new StringFormat();
            format_right = new StringFormat();
            format_center = new StringFormat();

            format_left.Alignment = StringAlignment.Near;
            format_right.Alignment = StringAlignment.Far;   
            format_center.Alignment = StringAlignment.Center;

            // Initialize the brush
            brush = new SolidBrush(Color.White);

            // Create the bitmap
            bmp = new Bitmap(lcd_width, lcd_height, PixelFormat.Format24bppRgb);

            // Create a graphics object from the bitmap
            lcd_graphics = Graphics.FromImage(bmp);
        }
        #endregion

        //
        // Public methods
        //

        #region public void LoadingScreen()
        public void LoadingScreen()
        {
            /* FUNCTION BY: CRACKPOT */
         
            ClearLCD();
 
            // draw the text
            DrawText("OpenPandora", "Verdana", 10.0f, FontStyle.Bold, format_left, new RectangleF(20, 0, 140, 25));
            DrawText("Version " + Application.ProductVersion, "Verdana", 7.0f, FontStyle.Italic, format_right, new RectangleF(0, 15, 160, 15));
            DrawText("Song is Loading...", "Lucida Console", 7.0f, FontStyle.Regular, format_center, new RectangleF(0, 33, 160, 14));

            // send to the lcd 
            TranslateBitmap();
        }
        #endregion
      
        #region public void OpenLCD()
        public void OpenLCD()
        {
            /* initialization was done within the constructor but since the 
             * button delegates have to be assigned to the lcd before lcd.open() is 
             * called, this function became necessary.
             */

            // open connection to the lcd
			if (!lcd.Open(appName, true))
			{
				Debug.WriteLine("ERROR: Failed to open LCD Screen");
			}

            LoadingScreen();
        }
        #endregion        

		#region public void closeLCD()
		public void CloseLCD()
		{
			try
			{
				lcd.Close();
			}
			catch
			{}
		}
		#endregion        

        #region public void RefreshBitmap()
        public void RefreshBitmap(string artist, string title, string album)
        {
            ClearLCD();
            DrawText(title, "Luicida Console", 7.0f, FontStyle.Regular, format_center, new RectangleF(20, -2, 120, 22));
            DrawText("by: " + artist, "Tahoma", 7.0f, FontStyle.Regular, format_center, new RectangleF(20, 20, 120, 20));       
            DrawButtons();
            TranslateBitmap();            
        }
        #endregion

        #region public void SetButtonDelegateImplementation(ButtonDelegate bDelegate)
        public void SetButtonDelegateImplementation(ButtonDelegate bDelegate)
        {
            lcd.AssignButtonDelegate(bDelegate);
        }

        #endregion

        #region public void SetConfigureDelegateImplementation(ConfigureDelegate cDelegate)
        public void SetConfigureDelegateImplementation(ConfigureDelegate cDelegate)
        {
            lcd.AssignConfigDelegate(cDelegate);
        }

        #endregion

        //
        // Private methods
        //

        #region private void ClearLCD()
        private void ClearLCD()
        {
            // Clear the graphics object
            lcd_graphics.Clear(Color.Black);
        }
        #endregion

        #region private void DrawText(string message, string fontName, float emSize, FontStyle stylish, StringFormat sFormat, RectangleF location)
        private void DrawText(string message, string fontName, float emSize, FontStyle stylish, StringFormat sFormat, RectangleF location)
        {
            try
            {
                using (Font font = new Font(fontName, emSize, stylish))
                {
                    lcd_graphics.DrawString(message, font, brush, location, sFormat);
                }
            }
            catch { }
        }
        #endregion

        #region private void DrawButtons()
        private void DrawButtons()
        {
            DrawText("Like", "Tahoma", 7.0f, FontStyle.Regular, format_center, new RectangleF(5, 34, 25, 10));
            DrawText("Hate", "Tahoma", 7.0f, FontStyle.Regular, format_center, new RectangleF(35, 34, 25, 10));
        }
        
        #endregion

        #region private void TranslateBitmap()
        private void TranslateBitmap()
        {
            byte[] lcdTranslate = new byte[lcd_width * lcd_height];

            //draw the open pandora icon
            Image icon = Base64ToImage(base64pandora16ico);
            lcd_graphics.DrawImage(icon, 0, 0, icon.Width, icon.Height);

            // Create temporary color and byte
            Byte pixelValue;

            //translate the bitmap to a byte array
            for (int hIndex = 0; hIndex < lcd_height; ++hIndex)
            {
                for (int wIndex = 0; wIndex < lcd_width; ++wIndex)
                {
                    // Get the green value of the current pixel
                    pixelValue = bmp.GetPixel(wIndex, hIndex).G;

                    // Place it in our translator
                    lcdTranslate[wIndex + (hIndex * lcd_width)] = pixelValue;
                }
            }

            // Send the lcd the translated information
            lcd.DisplayBitmap(ref lcdTranslate[0], LCDInterface.lglcd_PRIORITY_ALERT);
        }
        #endregion

        #region private Image Base64ToImage(string base64String)
        private Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        #endregion

        private string base64pandora16ico = "R0lGODlhEAAQAPcAAAAAAIAAAACAAICAAAAAgIAAgACAgICAgMDAwP8AAAD/AP//AAAA//8A/wD//////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMwAAZgAAmQAAzAAA/wAzAAAzMwAzZgAzmQAzzAAz/wBmAABmMwBmZgBmmQBmzABm/wCZAACZMwCZZgCZmQCZzACZ/wDMAADMMwDMZgDMmQDMzADM/wD/AAD/MwD/ZgD/mQD/zAD//zMAADMAMzMAZjMAmTMAzDMA/zMzADMzMzMzZjMzmTMzzDMz/zNmADNmMzNmZjNmmTNmzDNm/zOZADOZMzOZZjOZmTOZzDOZ/zPMADPMMzPMZjPMmTPMzDPM/zP/ADP/MzP/ZjP/mTP/zDP//2YAAGYAM2YAZmYAmWYAzGYA/2YzAGYzM2YzZmYzmWYzzGYz/2ZmAGZmM2ZmZmZmmWZmzGZm/2aZAGaZM2aZZmaZmWaZzGaZ/2bMAGbMM2bMZmbMmWbMzGbM/2b/AGb/M2b/Zmb/mWb/zGb//5kAAJkAM5kAZpkAmZkAzJkA/5kzAJkzM5kzZpkzmZkzzJkz/5lmAJlmM5lmZplmmZlmzJlm/5mZAJmZM5mZZpmZmZmZzJmZ/5nMAJnMM5nMZpnMmZnMzJnM/5n/AJn/M5n/Zpn/mZn/zJn//8wAAMwAM8wAZswAmcwAzMwA/8wzAMwzM8wzZswzmcwzzMwz/8xmAMxmM8xmZsxmmcxmzMxm/8yZAMyZM8yZZsyZmcyZzMyZ/8zMAMzMM8zMZszMmczMzMzM/8z/AMz/M8z/Zsz/mcz/zMz///8AAP8AM/8AZv8Amf8AzP8A//8zAP8zM/8zZv8zmf8zzP8z//9mAP9mM/9mZv9mmf9mzP9m//+ZAP+ZM/+ZZv+Zmf+ZzP+Z///MAP/MM//MZv/Mmf/MzP/M////AP//M///Zv//mf//zP///yH5BAEAABAALAAAAAAQABAAAAhQAKdsGUiwYEGB/xIqXLhwYEKDBBtukajQ4cOJFTFe3JiR4j+LHDmC/KgR5EiIHkMyPDlxisuEU0hSjPlvCoqYLBl2pIZypUadK1FAHLplSkAAOw==";

    }
}