using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Diamond_Dash_Killer
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        Thread m_KeyboardThread;
        Point InitialPosition;
        public Form1()
        {
            InitializeComponent();

            m_KeyboardThread = new Thread(new ThreadStart(watch));
            m_KeyboardThread.IsBackground = true;
            m_KeyboardThread.Start();
        }

        void watch()
        {
            while (true)
            {
                while (GetAsyncKeyState((int)Keys.F2) == -32767)
                {
                    InitialPosition.X = Cursor.Position.X;
                    InitialPosition.Y = Cursor.Position.Y;

                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            Microsoft.Xna.Framework.Input.Mouse.SetPosition((i * 40) + InitialPosition.X, (j * 40) + InitialPosition.Y);
                            mouse_event(0x02 | 0x04, (uint)(i * 40), (uint)(j * 40), 0, 0);
                    	}
                    }
                    Microsoft.Xna.Framework.Input.Mouse.SetPosition(InitialPosition.X, InitialPosition.Y);
                }
            }
        }
    }
}
