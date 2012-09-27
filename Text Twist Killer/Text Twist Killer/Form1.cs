using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZUtilities;
using System.IO;
using System.Threading;
namespace Text_Twist_Killer
{
    public partial class Form1 : Form
    {
        List<string> TheDictionary = new List<string>();
        Thread GoThread;

        IntPtr myHandle;

        KeyHookManager m_KeyboardManager;

        int length = 6;

        string CurrentLetters;

        public Form1()
        {
            InitializeComponent();

            CurrentLetters = "";

            using (StringReader sr = new StringReader(Properties.Resources.words))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    TheDictionary.Add(line);
                }
            }

            myHandle = this.Handle;

            m_KeyboardManager = new KeyHookManager();
            m_KeyboardManager.KeyDown += new KeyEventHandler(m_KeyboardManager_KeyDown);
                        
            GoThread = new Thread(new ThreadStart(GO));
            GoThread.IsBackground = true;
            GoThread.Start();
        }

        void m_KeyboardManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        void GO()
        {
            while (true)
            {
                if (NativeWin32.GetForegroundWindow() == myHandle)
                {
                    while (true)
                    {
                        if (NativeWin32.GetForegroundWindow() != myHandle)
                        {
                            for (int i = 0; i < TheDictionary.Count; i++)
                            {
                                if (TheDictionary[i].Length == length)
                                {
                                    for (int j = 0; j < CurrentLetters.Length; j++)
                                    {
                                        if (TheDictionary[i][0] == CurrentLetters[j])
                                        {
                                            SendKeys.SendWait(TheDictionary[i]);
                                            SendKeys.SendWait("{ENTER}");
                                        }
                                    }
                                }
                            }
                            if (length >= 3)
                                length--;
                            else
                                return;
                        }
                    }
                }
            }
        }

        private void textBox_Main_TextChanged(object sender, EventArgs e)
        {
            CurrentLetters = textBox_Main.Text;
        }
    }
}
