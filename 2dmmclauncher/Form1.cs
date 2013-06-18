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
        static public Process launcher = new Process();  //启动器核心，用于加载MC
        bool normalExit = true;
        public static string playername;
        public static string javaxmx;
        public static string javaw;
        public static string cfgfile = "2dmmccfg.xml";  //配置文件
        public static string gamemode ;
        public Form1()
        {
            InitializeComponent();
        }
       
        public void GameExit(object sender, EventArgs e)
        {
            try  //配合RMCAL，不过四周目已经用不到它了
            {
                File.Delete("ver.txt");
                File.Delete(".minecraft\\bin\\ver.txt");
            }
            catch { }
            if (normalExit==true) //程序正常退出时注销任务栏托盘图标
            {
                notifyIcon1.Visible = false;
                Environment.Exit(0);
            }
        }

        private void erdmmc(string PlayerName,string  JavaXmx,string javaw)    //启动器核心
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
            Environment.SetEnvironmentVariable("APPDATA", Environment.CurrentDirectory);//不设置的话会去访问系统的%appdata%目录下的.minecraft目录
            if (gamemode=="0")
                launcher.StartInfo.Arguments = "-Xincgc -Xmx" + JavaXmx + "M -XX:PermSize=64m -XX:MaxPermSize=128m " + "-Dsun.java2d.noddraw=true -Dsun.java2d.pmoffscreen=false -Dsun.java2d.d3d=false -Dsun.java2d.opengl=false -cp \"" + Environment.CurrentDirectory + "\\.minecraft\\bin\\survive.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\lwjgl.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\lwjgl_util.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\jinput.jar\" -Djava.library.path=\"" + Environment.CurrentDirectory + "\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft " + PlayerName;
            else
                launcher.StartInfo.Arguments = "-Xincgc -Xmx" + JavaXmx + "M -XX:PermSize=64m -XX:MaxPermSize=128m " + "-Dsun.java2d.noddraw=true -Dsun.java2d.pmoffscreen=false -Dsun.java2d.d3d=false -Dsun.java2d.opengl=false -cp \"" + Environment.CurrentDirectory + "\\.minecraft\\bin\\create.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\lwjgl.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\lwjgl_util.jar;" + Environment.CurrentDirectory + "\\.minecraft\\bin\\jinput.jar\" -Djava.library.path=\"" + Environment.CurrentDirectory + "\\.minecraft\\bin\\natives\" net.minecraft.client.Minecraft " + PlayerName;
            launcher.EnableRaisingEvents = true;
            launcher.Exited += new EventHandler(GameExit);
            launcher.Start();
            Thread.Sleep(5000);
            timer1.Enabled = true;  //每秒读取crash-reports目录，有变化即判断为有崩溃报告
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;  //将程序窗口定位到屏幕右下角
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
                XmlElement playerinfo = (XmlElement)cfgroot.SelectSingleNode("PlayerInfo");
                playername = playerinfo.Attributes["playername"].Value;
                try
                {
                    gamemode = playerinfo.Attributes["gamemode"].Value;
                }
                catch (NullReferenceException)
                {
                    gamemode = "0";
                }
                XmlElement javainfo = (XmlElement)cfgroot.SelectSingleNode("JavaInfo");
                javaw = javainfo.Attributes["javaw"].Value;
                javaxmx=javainfo.Attributes["javaxmx"].Value;
            }
            else
            {
                playername = Interaction.InputBox("请输入用户名(仅影响单机模式，用于单机模式获取皮肤)", "用户名", "Player");  //c#没有inputbox，调用VB的inputbox
                XmlDocument cfg=new XmlDocument();
                XmlDeclaration xmldecl;
                xmldecl = cfg.CreateXmlDeclaration("1.0", "utf-8",null);
                cfg.AppendChild(xmldecl);
                XmlElement cfgvalue = cfg.CreateElement("edmmc");
                cfg.AppendChild(cfgvalue);
                XmlNode cfgroot = cfg.SelectSingleNode("edmmc");
                XmlElement player = cfg.CreateElement("PlayerInfo");
                player.SetAttribute("playername", playername);
                player.SetAttribute("gamemode", "0");
                cfgvalue.AppendChild(player);
                //获取系统物理内存大小，支持大于4G的内存
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
                //取四分之一在一些机器上会出现无法创建java虚拟机的情况，所以这里添加一个手动输入窗口
                string ijavaxmx = Interaction.InputBox("输入java运行内存大小，默认为四分之一物理内存，如果你对此不了解，直接按确定即可", "javaxmx", javaxmx);
                if (ijavaxmx != "" && ijavaxmx != null && Convert.ToInt32(ijavaxmx) != 0)
                {
                    javaxmx = ijavaxmx;
                }
                XmlElement JavaInfo = cfg.CreateElement("JavaInfo");
                //从注册表读取java安装信息，对于64位系统，只判断目录名里是否有x86字串
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
                        bool maybe = false;
                        foreach (string ver in jre.GetSubKeyNames())
                        {
                            try
                            {
                                RegistryKey command = jre.OpenSubKey(ver);
                                string str = command.GetValue("JavaHome").ToString();
                                if (str != "")
                                {
                                    javaw = str + @"\bin\javaw.exe";
                                    if (File.Exists(javaw))
                                    {
                                        if (javaw.Contains("(x86)"))
                                        {
                                            flag = true;
                                            maybe = true;
                                        }
                                        else
                                        {
                                            flag = true;
                                            maybe = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            catch { }

                        }
                        if (maybe)
                        {
                            if (MessageBox.Show("可能您使用的是64位系统，但是只找到了32位的java，是否要手动指定java路径？64位系统使用32位java可能会带来性能下降问题", "X64", MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                OpenFileDialog javawp = new OpenFileDialog();
                                javawp.Multiselect = false;
                                javawp.Title = "请选择javaw.exe";
                                javawp.Filter = "javaw.exe|javaw.exe";
                                if (javawp.ShowDialog() == DialogResult.OK)
                                {
                                    javaw = javawp.FileName;
                                }
                                
                            }
                            flag = true;
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
            //获取服务器上的版本标识，等待10秒后读取MC输出的版本号
            Thread.Sleep(10000);
            try
            {
                StreamReader ver = new StreamReader("ver.txt");
                StreamReader cver = new StreamReader(".minecraft\\bin\\ver.txt");
                string vers = ver.ReadLine();  //版本号，字符串类型
                string isMust = ver.ReadLine();  //是否为强制更新，0=false;1=true
                string updateurl = ver.ReadLine();  //升级URL，已放弃不用
                string cvers = cver.ReadLine();  //本地版本号
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
                            launcher.EnableRaisingEvents = false;
                            launcher.Kill();
                            update Update = new update();
                            while (Update.ShowDialog() == DialogResult.Retry) { }
                            Application.Restart();
                        }
                        else
                        {
                            if (MessageBox.Show("不更新可能会导致无法进入服务器，确定不更新？", "确定不更新？", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                            {
                                this.Hide();
                                normalExit = false;
                                launcher.EnableRaisingEvents = false;
                                launcher.Kill();
                                update Update = new update();
                                while (Update.ShowDialog() == DialogResult.Retry) { }
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
                            launcher.EnableRaisingEvents = false;
                            launcher.Kill(); 
                            update Update = new update();
                            while (Update.ShowDialog() == DialogResult.Retry) { }
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
            if ((File.Exists("2dmmc.dat") || File.Exists("2dmmc4th.7z")) && !File.Exists(".minecraft\\bin\\survive.jar"))
            {
                if (File.Exists("2dmmc4th.7z"))
                {
                    File.Move("2dmmc4th.7z", "2dmmc.dat");
                }
                //this.Hide();
                downloader.ShowDialog();
                downloader.Close();
            }
            if (File.Exists(".minecraft\\bin\\survive.jar") == false)
            {
                if (MessageBox.Show("游戏不存在，是否下载？如果是下载完成后出现这个提示，点确定即可", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
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
            DirectoryInfo crash = new DirectoryInfo(@".minecraft\crash-reports");   //获取游戏启动时的崩溃报告数量
            try
            {
                ocrashs = crash.GetFiles().Length;
            }
            catch 
            {
                ocrashs = 0;
            }
        }
        private void downloadcfg()   //RMCA配置文件下载
        {
            WebClient cfg = new WebClient();
            string cfgfile;
            switch (gamemode)
            {
                case "0": 
                    cfgfile = "http://2dmmc.bangbang93.com/RMCAClien1.0";
                    创造服务器ToolStripMenuItem.Checked = false;
                    生存服务器ToolStripMenuItem.Checked = true;
                    break;
                case "1":
                    创造服务器ToolStripMenuItem.Checked = true;
                    生存服务器ToolStripMenuItem.Checked = false;
                    cfgfile = "http://2dmmc.bangbang93.com/RMCAClientCreate";
                    break;
                default :
                    cfgfile = "http://2dmmc.bangbang93.com/RMCAClien1.0";
                    break;
            }
            cfg.DownloadFile(cfgfile, ".minecraft\\RMCAuthServer.txt");
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            label1.Text = "检测游戏";
            this.Refresh();
            checkgame();
            progressBar1.Value += 1;

            label1.Text = "正在加载配置文件";
            this.Refresh();
            loadconfig();
            progressBar1.Value += 1;

            label1.Text = "下载游戏配置文件";
            this.Refresh();
            downloadcfg();
            progressBar1.Value+=1;

            label1.Text = "正在启动游戏";
            this.Refresh();
            erdmmc(playername, javaxmx, javaw);
            progressBar1.Value += 1;

            label1.Text = "正在检查更新";
            this.Refresh();
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
            runState rs = new runState(this );  //运行状态窗口
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
                StreamReader erp = new StreamReader(filedir,Encoding.Default);
                Thread.Sleep(500);  //等待半秒， 以防文件没有写完
                errorReport er = new errorReport(erp.ReadToEnd());
                timer1.Stop();
                er.ShowDialog();
                try
                {
                    ocrashs = crash.GetFiles().Length;
                }
                catch
                {
                    ocrashs = 0;
                }
                timer1.Start();
            }
        }
        public long getworkset()
        {
            launcher.Refresh();
            return launcher.WorkingSet64;
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setting Setting = new setting();
            Setting.ShowDialog();
        }

        private void 生存服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (生存服务器ToolStripMenuItem.Checked != true)
            {
                生存服务器ToolStripMenuItem.Checked = true;
                创造服务器ToolStripMenuItem.Checked = false;
                launcher.EnableRaisingEvents = false;
                launcher.Kill();
                launcher.StartInfo.Arguments = launcher.StartInfo.Arguments.Replace("create", "survive");
                string dcfgfile = "http://2dmmc.bangbang93.com/RMCAClien1.0";
                (new WebClient()).DownloadFile(dcfgfile,".minecraft\\RMCAuthServer.txt");
                launcher.Start();
                launcher.EnableRaisingEvents = true;
            }
        }

        private void 创造服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (创造服务器ToolStripMenuItem.Checked != true)
            {
                生存服务器ToolStripMenuItem.Checked = false;
                创造服务器ToolStripMenuItem.Checked = true;
                launcher.EnableRaisingEvents = false;
                launcher.Kill();
                launcher.StartInfo.Arguments = launcher.StartInfo.Arguments.Replace("survive", "create");
                string dcfgfile = "http://2dmmc.bangbang93.com/RMCAClientCreate";
                (new WebClient()).DownloadFile(dcfgfile,".minecraft\\RMCAuthServer.txt");
                launcher.Start();
                launcher.EnableRaisingEvents = true;
            }
        }




    }
}
