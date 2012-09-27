using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ZUtilities;

namespace Bejeweled_Blitz_Killer
{
    public struct TwoPoint
    {
        public Point a;
        public Point b;
        public TwoPoint(Point a, Point b)
        {
            this.a = a;
            this.b = b;
        }
    }

    public partial class Form1 : Form
    {
        Point StartPos;
        List<List<Color>> ColourArray;
        int Stride = 40;
        Point InitialPosition;

        public Form1()
        {
            InitializeComponent();
            ColourArray = new List<List<Color>>();
        }

        Bitmap GetScreenBitmap(int GrabWidth, int GrabHeight, int X, int Y)
        {
            Bitmap bmpScreenShot = new Bitmap(GrabWidth, GrabHeight);
            Graphics gfx = Graphics.FromImage((Image)bmpScreenShot);
            gfx.CopyFromScreen(X, Y, 0, 0, new Size(GrabWidth, GrabHeight));

            return bmpScreenShot;
        }

        Bitmap getPixelBitmap(Bitmap ScreenShot, int Width, int Height, int SpaceingX, int SpaceingY, int NumberOfXPixels, int NumberOfYPixels)
        {
            Bitmap Bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(Bmp);

            for (int i = 0; i < NumberOfYPixels; i++)
            {
                ColourArray.Add(new List<Color>());
                for (int j = 0; j < NumberOfXPixels; j++)
                {
                    ColourArray[i].Add(ScreenShot.GetPixel(j * SpaceingX, i * SpaceingY));
                    SolidBrush brush = new SolidBrush(Color.FromArgb(ColourArray[i][j].R, ColourArray[i][j].G, ColourArray[i][j].B));
                    g.FillRectangle(brush, j * Width / NumberOfXPixels, i * Height / NumberOfYPixels, Width, Height);
                }
            }

            return Bmp;
        }

        TwoPoint Start()
        {
            InitialPosition.X = Cursor.Position.X;
            InitialPosition.Y = Cursor.Position.Y;

            ColourArray.Clear();
            this.pictureBox_Main.Image = getPixelBitmap(GetScreenBitmap(318, 318, Cursor.Position.X, Cursor.Position.Y), 240, 240, Stride, Stride, 8, 8);
            this.pictureBox_Main.Refresh();

            Color temp1 = new Color();
            Color temp2 = new Color();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    temp1 = ColourArray[y][x];
                    temp2 = ColourArray[y][x + 1];

                    ColourArray[y][x] = temp2;
                    ColourArray[y][x + 1] = temp1;

                    if (CheckBoard(ColourArray))
                    {
                        return new TwoPoint(new Point(x, y), new Point(x + 1, y));
                    }

                    ColourArray[y][x] = temp1;
                    ColourArray[y][x + 1] = temp2;

                }
            }

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    temp1 = ColourArray[y][x];
                    temp2 = ColourArray[y + 1][x];

                    ColourArray[y][x] = temp2;
                    ColourArray[y + 1][x] = temp1;

                    if (CheckBoard(ColourArray))
                        return new TwoPoint(new Point(x, y), new Point(x, y + 1));

                    ColourArray[y][x] = temp1;
                    ColourArray[y + 1][x] = temp2;

                }
            }

            return new TwoPoint(new Point(0, 0), new Point(0, 0));
        }

        bool CheckBoard(List<List<Color>> ColourArray)
        {

            //for (int y = 0; y < 8; y++)
            //{
            //    for (int x = 0; x < 6; x++)
            //    {
            //        if (ColourClosenessChecker(ColourArray[y][x], ColourArray[y][x + 1]) &&
            //            ColourClosenessChecker(ColourArray[y][x + 1], ColourArray[y][x + 2]) &&
            //            ColourClosenessChecker(ColourArray[y][x + 2], ColourArray[y][x + 3]) &&
            //            ColourClosenessChecker(ColourArray[y][x + 3], ColourArray[y][x + 4]) &&
            //            ColourClosenessChecker(ColourArray[y][x + 4], ColourArray[y][x + 5]))
            //            return true;
            //    }
            //}
            //for (int x = 0; x < 8; x++)
            //{
            //    for (int y = 0; y < 6; y++)
            //    {
            //        if (ColourClosenessChecker(ColourArray[y][x], ColourArray[y + 1][x]) &&
            //            ColourClosenessChecker(ColourArray[y + 1][x], ColourArray[y + 2][x]) &&
            //            ColourClosenessChecker(ColourArray[y + 2][x], ColourArray[y + 3][x]) &&
            //            ColourClosenessChecker(ColourArray[y + 3][x], ColourArray[y + 4][x]) &&
            //            ColourClosenessChecker(ColourArray[y + 3][x], ColourArray[y + 4][x]))
            //            return true;
            //    }
            //}

            //five
            try
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        if (ColourClosenessChecker(ColourArray[y][x], ColourArray[y][x + 1]) &&
                            ColourClosenessChecker(ColourArray[y][x + 1], ColourArray[y][x + 2]) &&
                            ColourClosenessChecker(ColourArray[y][x + 2], ColourArray[y][x + 3]) &&
                            ColourClosenessChecker(ColourArray[y][x + 3], ColourArray[y][x + 4]))
                            return true;
                    }
                }
            }
            catch { }

            try{
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (ColourClosenessChecker(ColourArray[y][x], ColourArray[y + 1][x]) &&
                        ColourClosenessChecker(ColourArray[y + 1][x], ColourArray[y + 2][x]) &&
                        ColourClosenessChecker(ColourArray[y + 2][x], ColourArray[y + 3][x]) &&
                        ColourClosenessChecker(ColourArray[y + 3][x], ColourArray[y + 4][x]))
                        return true;
                }
            }
            }
            catch { }

            //four
            try{
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (ColourClosenessChecker(ColourArray[y][x], ColourArray[y][x + 1]) &&
                        ColourClosenessChecker(ColourArray[y][x + 1], ColourArray[y][x + 2]) &&
                        ColourClosenessChecker(ColourArray[y][x + 2], ColourArray[y][x + 3]))
                        return true;
                }
            }
            }
            catch { }
            try{
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (ColourClosenessChecker(ColourArray[y][x], ColourArray[y + 1][x]) &&
                        ColourClosenessChecker(ColourArray[y + 1][x], ColourArray[y + 2][x])&&
                        ColourClosenessChecker(ColourArray[y + 2][x], ColourArray[y + 3][x]))
                        return true;
                }
            }
            }
            catch { }
            //three
            
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (ColourClosenessChecker(ColourArray[y][x], ColourArray[y][x + 1]) &&
                        ColourClosenessChecker(ColourArray[y][x + 1], ColourArray[y][x + 2]))
                        return true;
                }
            }
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (ColourClosenessChecker(ColourArray[y][x], ColourArray[y + 1][x]) &&
                        ColourClosenessChecker(ColourArray[y + 1][x], ColourArray[y + 2][x]))
                        return true;
                }
            }
            return false;
        }

        public bool ColourClosenessChecker(Color c1, Color c2, int tolerance = 25)
        {
            return Math.Abs(c1.R - c2.R) < tolerance && Math.Abs(c1.G - c2.G) < tolerance && Math.Abs(c1.B - c2.B) < tolerance;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                TwoPoint temp = Start();
                Microsoft.Xna.Framework.Input.Mouse.SetPosition((temp.a.X * Stride) + InitialPosition.X, (temp.a.Y * Stride) + InitialPosition.Y);
                MouseHookManager.mouse_event(0x02 | 0x04, (uint)temp.a.X, (uint)temp.a.Y, 0, 0);

                //System.Threading.Thread.Sleep(500);

                Microsoft.Xna.Framework.Input.Mouse.SetPosition((temp.b.X * Stride) + InitialPosition.X, (temp.b.Y * Stride) + InitialPosition.Y);
                MouseHookManager.mouse_event(0x02 | 0x04, (uint)temp.b.X, (uint)temp.b.Y, 0, 0);

                //System.Threading.Thread.Sleep(500);

                Microsoft.Xna.Framework.Input.Mouse.SetPosition(InitialPosition.X, InitialPosition.Y);

                System.Threading.Thread.Sleep(50);
                NativeWin32.SetForegroundWindow(this.Handle);
            }
        }
    }
}
