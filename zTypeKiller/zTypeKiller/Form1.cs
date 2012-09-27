using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ZUtilities;
namespace zTypeKiller
{

    public partial class Form1 : Form
    {
        Thread currentThread;
        IntPtr m_MyHWND;

        IntPtr CurrentWindow;
        IntPtr LastWindow;

        Thread HWndThread;

        KeyHookManager m_KeyboardManager;

        char[] m_Alphabet;
        int index = 0;

        public Form1()
        {
            InitializeComponent();
            char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            m_Alphabet = alphabet;
            CurrentWindow = this.Handle;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HWndThread = new Thread(new ThreadStart(WatchHWNDs));
            HWndThread.IsBackground = true;
            //HWndThread.Start();

            currentThread = new Thread(new ThreadStart(killer));
            currentThread.IsBackground = true;
            //currentThread.Start();

            m_KeyboardManager = new KeyHookManager();
            m_KeyboardManager.KeyDown += new KeyEventHandler(m_KeyboardManager_KeyDown);
            m_KeyboardManager.Start();

        }

        void m_KeyboardManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.Space)
            {

                    if (NativeWin32.GetForegroundWindow() != CurrentWindow)
                    {
                        //SendKeys.SendWait(m_Alphabet[index].ToString());
                        SendKeys.SendWait("qwertyuiopasdfghjklzxcvbnm");
                        index++;
                        if (index >= 26)
                        {
                            index = 0;
                            Thread.Sleep(200);
                        }
                    }
                
            }
        }

        void killer()
        {
            int index = 0;
            while (true)
            {
                if (NativeWin32.GetForegroundWindow() != CurrentWindow)
                {                    
                    //SendKeys.SendWait(m_Alphabet[index].ToString());
                    SendKeys.SendWait("qwertyuiopasdfghjklzxcvbnm");
                    index++;
                    if (index >= 26)
                    {
                        index = 0;
                        Thread.Sleep(200);
                    }                    
                }
            }
        }

        void WatchHWNDs()
        {
            while (true)
            {
                CurrentWindow = NativeWin32.GetForegroundWindow();
                if (CurrentWindow == m_MyHWND)
                    NativeWin32.SetForegroundWindow(LastWindow);
                LastWindow = CurrentWindow;
                Thread.Sleep(10);
            }
        }
    }
}
