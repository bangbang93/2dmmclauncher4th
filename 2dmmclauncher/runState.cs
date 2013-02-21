using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Diagnostics;

namespace _2dmmclauncher
{
    public partial class runState : Form
    {
        double  totmem;
        public runState(Form1 F)
        {
            f = F;
            InitializeComponent();
        }
        Form1 f;

        private void runState_Load(object sender, EventArgs e)
        {
            string verPath = f.launcher.StartInfo.FileName.Substring(0, f.launcher.StartInfo.FileName.IndexOf("bin"))+"RELEASE";
            try
            {
                StreamReader verp = new StreamReader(verPath);
                javaVersion.Text = verp.ReadToEnd();
                verp.Close();
                verp.Dispose();
            }
            catch
            {
                javaVersion.Text = "获取失败";
            }
            double capacity = 0.0;
                ManagementClass cimobject1 = new ManagementClass("Win32_PhysicalMemory");
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo1 in moc1)
                {
                    capacity += ((Math.Round(Int64.Parse(mo1.Properties["Capacity"].Value.ToString()) / 1024 / 1024.0, 1)));
                }
                moc1.Dispose();
                cimobject1.Dispose();
                totmem = capacity;
            memLoad.Text = ((double)f.launcher.WorkingSet64 / 1024.0 / 1024.0).ToString("f") + "MB";
            memPresent.Text = ((double)f.getworkset() / 1024.0 / 1024.0 / totmem*100).ToString("f") + "%";
            runTime.Text = (DateTime.Now - f.launcher.StartTime).ToString();
            try
            {
                StreamReader ver = new StreamReader("ver.txt");
                currectVer.Text = ver.ReadLine();
                ver.Close();
                StreamReader cver = new StreamReader(".minecraft\\bin\\ver.txt");
                lastVer.Text = cver.ReadLine();
                cver.Close();
            }
            catch { }
            try
            {
                ManagementClass searcher = new ManagementClass("WIN32_Processor");
                ManagementObjectCollection moc = searcher.GetInstances();
                hardWare.Text = "";
                foreach (ManagementObject mo in moc)
                {
                    hardWare.Text += mo["Name"].ToString().Trim()+"\n";
                    hardWare.Text += mo["AddressWidth"].ToString().Trim() + "Bit系统\n";   
                }
            }
            catch { }
            try
            {
                ManagementClass searcher = new ManagementClass("Win32_VideoController");
                ManagementObjectCollection moc = searcher.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    hardWare.Text += (mo["Name"].ToString().Trim()) + "\n";
                }
            }
            catch { }
            hardWare.Text += (totmem).ToString()+"MB\n";
            try
            {
                ManagementClass searcher = new ManagementClass("Win32_OperatingSystem");
                ManagementObjectCollection moc = searcher.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    hardWare.Text += (mo["Name"].ToString().Trim()) + "\n";
                    hardWare.Text += (mo["CSDVersion"].ToString().Trim()) + "\n";
                    hardWare.Text += (mo["Version"].ToString().Trim()) + "\n";
                }
            }
            catch { }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            memLoad.Text = ((double)f.getworkset() /1024.0/1024.0).ToString("f")+"MB";
            memPresent.Text = ((double)f.getworkset()/1024.0/1024.0 / totmem*100).ToString("f") + "%";
            runTime.Text = (DateTime.Now -f.launcher.StartTime).ToString(); 
        }
    }
}
