using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AntiScreenshots
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Log.Initialize(richTextBox1, this);
            AntiScreenshot.ProtectAgainstScreenshots(Process.GetCurrentProcess().MainWindowHandle);
            Log.Write("Try take a screenshot of me and paste in paint.", Color.Blue);
        }
    }
}