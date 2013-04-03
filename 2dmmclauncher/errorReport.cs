using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2dmmclauncher
{
    public partial class errorReport : Form
    {
        string log;
        public errorReport(string Log)
        {
            log = Log;
            InitializeComponent();
        }

        private void errorReport_Load(object sender, EventArgs e)
        {
            textBox1.Text = log;
            try
            {
                Clipboard.SetText(log);
            }
            catch { };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(log);
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            Form1.launcher.EnableRaisingEvents = false;
            Form1.launcher.Kill();
            Form1.launcher.Start();
            Form1.launcher.EnableRaisingEvents = true;
            this.Close();
            return;
        }
    }
}
