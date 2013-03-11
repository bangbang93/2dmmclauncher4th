using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Net;
using System.Threading;
using System.Management;
using System.Xml;


namespace _2dmmclauncher
{
    public partial class Form1 : Form
    {
        public Process launcher = new Process();
        bool normalExit = true;
        public string playername;
        public string javaxmx;
        public string javaw;
        public string cfgfile = "2dmmccfg.xml";
        public Form1()
        {
            InitializeComponent();
        }
       
        private void GameExit(object sender, EventArgs e)
        {
            //MessageBox.Show("w");

            try
            {
                File.Delete("ver.txt");
                File.Delete(".minecraft\\bin\\ver.txt");
            }
            catch { }
            if (normalExit==true)
            {
                notifyIcon1.Visible = false;
                Environment.Exit(0);
            }
        }

        private void erdmmc(string PlayerName,string  JavaXmx,string javaw)
        {

            launcher.StartInfo.FileName = javaw;
            while (!File.Exists(launcher.StartInfo.FileName))
            {
                if (MessageBox.Show("javaw.exe路径错误", "无法启动", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    OpenFileDialog javawp = new OpenFileDialog();
                    javawp.Multiselect = false;
                    javawp.Title = "请选择javaw.exe";
                    javawp.Filter = "javaw.exe|javaw.exe";
                    if (javawp.ShowDialog() == DialogResult.OK)
                    {
                        javaw = javawp.FileName;
                        launcher.StartInfo.FileName = javaw;
                        XmlDocument cfg=new XmlDocument();
                        cfg.Load(cfgfile);
                        XmlElement root = (XmlElement)cfg.SelectSingleNode("edmmc");
                        XmlElement javainfo = (XmlElement )root.SelectSingleNode("JavaInfo");
                        javainfo.SetAttribute("javaw", javaw);
                        cfg.Save(cfgfile);
                    }
                }
                else
                {
                    Environment.Exit(0);
                }

            }
            launcher.StartInfo.UseShellExecute = false;
            launcher.StartInfo.WorkingDirectory = Environment.CurrentDirectory+"\\.minecraft\\bin";
            Environment.SetEnvironmentVariable("APPDATA", Environment.CurrentDirectory);
            launcher.StartInfo.Arguments = "-Xincgc -Xmx" + JavaXmx + "M -XX:PermSize=64m -XX:MaxPermSize=128m " + "-Dsun.java2d.noddraw=true -Dsun.java2d.pmoffscreen=false -Dsun.java2d.d3d=false -Dsun.java2d.opengl=false -cp \"" + Environment.CurrentDirectory + "\\.minecraft\\bin\\minecraft.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\lwjgl.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\lwjgl_util.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\jinput.jar\" -Djava.library.path=\"" + Environment.CurrentDirectory + "\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft " + PlayerName;

            launcher.EnableRaisingEvents = true;
            launcher.Exited += new EventHandler(GameExit);
            launcher.Start();
            Thread.Sleep(5000);
            timer1.Enabled = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void loadconfig()
        {
            #region 旧的配置文件更新
            if (File.Exists("2dmmclauncher.cfg"))
            {
                StreamReader ocfg = new StreamReader("2dmmclauncher.cfg");
                string oname = ocfg.ReadLine();
                string ojavaxmx = ocfg.ReadLine();
                string ojavaw = ocfg.ReadLine();
                XmlDocument cfg = new XmlDocument();
                XmlDeclaration xmldecl;
                xmldecl = cfg.CreateXmlDeclaration("1.0", "utf-8", null);
                cfg.AppendChild(xmldecl);
                XmlElement cfgvalue = cfg.CreateElement("edmmc");
                cfg.AppendChild(cfgvalue);
                XmlNode cfgroot = cfg.SelectSingleNode("edmmc");
                XmlElement player = cfg.CreateElement("PlayerInfo");
                player.SetAttribute("playername", oname);
                cfgvalue.AppendChild(player);
                XmlElement JavaInfo = cfg.CreateElement("JavaInfo");
                JavaInfo.SetAttribute("javaxmx", ojavaxmx);
                JavaInfo.SetAttribute("javaw", ojavaw);
                cfgvalue.AppendChild(JavaInfo);
                cfg.AppendChild(cfgvalue);
                cfg.Save(cfgfile);
                ocfg.Close();
                File.Delete("2dmmclauncher.cfg");

            }
            #endregion
            if (File.Exists(cfgfile))
            {
                XmlDocument cfg = new XmlDocument();
                cfg.Load(cfgfile);
                XmlNode cfgroot = cfg.SelectSingleNode("edmmc");
                XmlElement playerinfo = (XmlElement )cfgroot.SelectSingleNode("PlayerInfo");
                playername = playerinfo.Attributes["playername"].Value;
                XmlElement javainfo = (XmlElement)cfgroot.SelectSingleNode("JavaInfo");
                javaw = javainfo.Attributes["javaw"].Value;
                javaxmx=javainfo.Attributes["javaxmx"].Value;
            }
            else
            {
                playername = Interaction.InputBox("请输入用户名(仅影响单机模式，用于单机模式获取皮肤)", "用户名", "Player");
                XmlDocument cfg=new XmlDocument();
                XmlDeclaration xmldecl;
                xmldecl = cfg.CreateXmlDeclaration("1.0", "utf-8",null);
                cfg.AppendChild(xmldecl);
                XmlElement cfgvalue = cfg.CreateElement("edmmc");
                cfg.AppendChild(cfgvalue);
                XmlNode cfgroot = cfg.SelectSingleNode("edmmc");
                XmlElement player = cfg.CreateElement("PlayerInfo");
                player.SetAttribute("playername", playername);
                cfgvalue.AppendChild(player);
                double capacity = 0.0;
                ManagementClass cimobject1 = new ManagementClass("Win32_PhysicalMemory");
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo1 in moc1)
                {
                    capacity += ((Math.Round(Int64.Parse(mo1.Properties["Capacity"].Value.ToString()) / 1024 / 1024.0, 1)));
                }
                moc1.Dispose();
                cimobject1.Dispose();
                int qmem = Convert.ToUInt16(capacity.ToString()) / 4;
                if (qmem < 512)
                {
                    qmem = 512;
                }
                javaxmx = qmem.ToString ();
                XmlElement JavaInfo = cfg.CreateElement("JavaInfo");
                JavaInfo.SetAttribute("javaxmx", javaxmx);
                {
                    try
                    {
                        RegistryKey lm = Registry.LocalMachine;
                        RegistryKey sf = lm.OpenSubKey("SOFTWARE");
                        RegistryKey js = sf.OpenSubKey("JavaSoft");
                        RegistryKey jre = js.OpenSubKey("Java Runtime Environment");
                        RegistryKey reg = Registry.LocalMachine;
                        reg = reg.OpenSubKey("SOFTWARE").OpenSubKey("JavaSoft").OpenSubKey("Java Runtime Environment");
                        bool flag = false;
                        foreach (string ver in jre.GetSubKeyNames())
                        {
                            try
                            {
                                RegistryKey command = jre.OpenSubKey(ver);
                                string str = command.GetValue("JavaHome").ToString();
                                if (str != "")
                                {
                                    javaw = str + @"\bin\javaw.exe";
                                    flag = true;
                                    break;
                                }
                            }
                            catch { }

                        }
                        if (!flag)
                        {
                            MessageBox.Show("获取javaw.exe目录失败，请手动查找");
                            OpenFileDialog javawp = new OpenFileDialog();
                            javawp.Multiselect = false;
                            javawp.Title = "请选择javaw.exe";
                            javawp.Filter = "javaw.exe|javaw.exe";
                            if (javawp.ShowDialog() == DialogResult.OK)
                            {
                                javaw = javawp.FileName;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("获取javaw.exe目录失败，请手动查找");
                        OpenFileDialog javawp = new OpenFileDialog();
                        javawp.Multiselect = false;
                        javawp.Title = "请选择javaw.exe";
                        javawp.Filter = "javaw.exe|javaw.exe";
                        if (javawp.ShowDialog() == DialogResult.OK)
                        {
                            javaw = javawp.FileName;
                        }
                    }
                    

                }
                JavaInfo.SetAttribute("javaw", javaw);
                cfgvalue.AppendChild(JavaInfo);
                cfg.AppendChild(cfgvalue);
                cfg.Save(cfgfile);
            }

        }
        private void checkupdate()
        {
            WebClient verc = new WebClient();
            verc.DownloadFile("http://2dmmc.bangbang93.com/ver4th.txt", "ver.txt");
            Thread.Sleep(10000);
            try
            {
                StreamReader ver = new StreamReader("ver.txt");
                StreamReader cver = new StreamReader(".minecraft\\bin\\ver.txt");
                string vers = ver.ReadLine();
                string isMust = ver.ReadLine();
                string updateurl = ver.ReadLine();
                string cvers = cver.ReadLine();
                ver.Close();
                cver.Close();
                Application.DoEvents();
                if (vers != cvers)
                {
                    if (isMust == "1")
                    {
                        if (MessageBox.Show("检测到强制更新版本，点击确定更新", "检测更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.Hide();
                            normalExit = false;
                            launcher.Kill();
                            update Update = new update();
                            Update.ShowDialog();
                            Application.Restart();
                        }
                        else
                        {
                            if (MessageBox.Show("不更新可能会导致无法进入服务器，确定不更新？", "确定不更新？", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                            {
                                this.Hide();
                                normalExit = false;
                                launcher.Kill();
                                update Update = new update();
                                Update.ShowDialog();
                                Application.Restart();
                            }
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("检测到版本更新，是否更新？当前版本：" + cvers + "更新版本" + vers, "检测到更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            //this.Hide();
                            normalExit = false;
                            launcher.Kill();
                            update Update = new update();
                            Update.ShowDialog();
                            Application.Restart();
                        }
                    }
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(10, "废话二次元启动器", "已是最新版本："+vers , System.Windows.Forms.ToolTipIcon.Info);
                }
            }
            catch (FileNotFoundException )
            {
                MessageBox.Show("获取本地版本号失败");
            }
            catch (WebException)
            {
                MessageBox.Show("网络错误，获取最新版本号失败");
            }
            Application.ExitThread();
        }
        downloadForm downloader = new downloadForm();
        private void checkgame()
        {
            if (File.Exists(".minecraft\\bin\\minecraft.jar") == false)
            {
                if (MessageBox.Show("游戏不存在，是否下载？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    this.Hide();
                    downloader.ShowDialog();
                    downloader.Close();
                }
                else
                {
                    Environment.Exit(0);
                }
                
            }
            DirectoryInfo crash = new DirectoryInfo(@".minecraft\crash-reports");
            try
            {
                ocrashs = crash.GetFiles().Length;
            }
            catch 
            {
                ocrashs = 0;
            }
        }
        private void downloadcfg()
        {
            WebClient cfg = new WebClient();
            cfg.DownloadFile("http://2dmmc.bangbang93.com/RMCAClien1.0", ".minecraft\\RMCAuthServer.txt");
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            label1.Text = "检测游戏";
            label1.Refresh();
            checkgame();
            progressBar1.Value += 1;
            label1.Text = "下载游戏配置文件";
            label1.Refresh();
            downloadcfg();
            progressBar1.Value+=1;
            label1.Text = "正在加载配置文件";
            label1.Refresh();
            loadconfig();
            progressBar1.Value += 1;
            label1.Text = "正在启动游戏";
            label1.Refresh();
            erdmmc(playername, javaxmx, javaw);
            progressBar1.Value += 1;
            label1.Text = "正在检查更新";
            label1.Refresh();
            Thread tCheckUpdate = new Thread(new ThreadStart(checkupdate));
            tCheckUpdate.Start();
            progressBar1.Value += 1;
            while (tCheckUpdate.ThreadState == System.Threading.ThreadState.Running)
            {
                this.Refresh();
            }
            this.Hide();
        }

        private void About_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void runState_Click(object sender, EventArgs e)
        {
            runState rs = new runState(this );
            rs.ShowDialog();
        }
        int ocrashs;
        private void timer1_Tick(object sender, EventArgs e)
        {
            DirectoryInfo crash = new DirectoryInfo(@".minecraft\crash-reports");
            int crashs;
            try
            {
                crashs = crash.GetFiles().Length;
            }
            catch
            {
                crashs = 0;
            }
            if (crashs != ocrashs)
            {
                string filedir=null;
                DateTime dt=DateTime.MinValue ;
                foreach (FileInfo file in crash.GetFiles())
                {
                    if (file.LastWriteTime > dt)
                    {
                        filedir = file.FullName;
                        dt = file.LastWriteTime;
                    }
                }
                StreamReader erp = new StreamReader(filedir);
                errorReport er = new errorReport(erp.ReadToEnd());
                er.ShowDialog();
                
            }
        }
        public long getworkset()
        {
            launcher.Refresh();
            return launcher.WorkingSet64;
        }




    }
}
