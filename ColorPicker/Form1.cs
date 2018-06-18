using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ColorPicker
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        public Form1()
        {
            InitializeComponent();
        }

        private void lblColor_MouseDown(object sender, MouseEventArgs e)
        {
            var x = e.X;
            var y = e.Y;
        }

        /// <summary>
        /// Get the pixel color once mouse button released
        /// </summary>
        /// <param name="x">x position of the cursor</param>
        /// <param name="y">y position of the cursor</param>
        /// <returns>Color</returns>
        static public System.Drawing.Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                         (int)(pixel & 0x0000FF00) >> 8,
                         (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }

        /// <summary>
        /// Load the selected pixel's color in to background label, 
        /// and code in to text box at the bottom
        /// </summary>
        private void lblColor_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);

            lblColor.BackColor = GetPixelColor(Cursor.Position.X, Cursor.Position.Y);
            txtColor.Text = HexConverter(lblColor.BackColor);
            Clipboard.SetText(txtColor.Text);
        }

        /// <summary>
        /// Converts color in to Hex Format
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}
