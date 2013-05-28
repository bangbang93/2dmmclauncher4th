using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace _2dmmclauncher
{
    public partial class downloadForm : Form
    {
        public downloadForm()
        {
            InitializeComponent();
        }
        static string gamefile=@"http://file.bangbang93.com/2dmmc4th.7z";
        static string md5file = @"http://2dmmc.bangbang93.com/cmd54th.txt";
        string cmd5;
        private void Form2_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            if (File.Exists("2dmmc.dat"))
            {
                WebClient cmd5f = new WebClient();
                cmd5 = cmd5f.DownloadString(md5file).Substring(0, 32).ToUpper();
                if (cmd5 != GetMD5HashFromFile("2dmmc.dat").ToUpper())
                {
                    MessageBox.Show("找到下载文件，但是与服务器MD5效验不同，重新下载");
                    File.Delete("2dmmc.dat");
                }
                else
                {
                    MessageBox.Show("找到下载文件，校验通过，开始解压");
                    WebClient d7z = new WebClient();
                    d7z.DownloadFile("http://image.bangbang93.com/7za.exe", "unpakcer.exe");
                    if (File.Exists("2dmmc.dat") == true)
                    {
                        Process un7z = new Process();
                        un7z.StartInfo.FileName = "unpakcer.exe";
                        un7z.StartInfo.Arguments = "x -y 2dmmc.dat";
                        un7z.Start();
                        un7z.WaitForExit();
                        File.Delete("unpakcer.exe");
                        this.Close();
                    }
                }
            }
            if (File.Exists("2dmmc.dat.cfg"))
            {
                File.Delete("2dmmc.dat.cfg");
            }
            if (File.Exists("2dmmc.dat.tfg"))
            {
                File.Delete("2dmmc.dat.tfg");
            }
        }
        downloader game = new downloader(gamefile);

        WebClient downl = new WebClient();
        private void downloadForm_Shown(object sender, EventArgs e)
        {
            game.ThreadCount = 5;   
            game.Filename = "2dmmc.dat";
            game.DirectoryName = Environment.CurrentDirectory;
            game.Progress += new downloader.ProgressEventHandler(game_Progress);
            game.Finished += new downloader.FinishedEventHandler(game_Finished);
            game.Exception += new downloader.ExceptionEventHandler(game_Exception);
            game.Speed += new downloader.SpeedHandler(game_Speed);
            game.Start();
            game.Connected += new downloader.ConnectedEventHandler(game_Connected);
            
        }

        void game_Connected(downloader sender, string filename, string contentType)
        {
            size.Text = ((double)game.ContentLength/1024.0/1024.0).ToString("f")+"MB";
        }

        void game_Exception(downloader sender, Exception e)
        {
            MessageBox.Show("下载失败，请重试或尝试手动下载"+game.Url+"\n"+e.Message ,"错误");
            if (MessageBox.Show("是否尝试手动下载?", "手动下载", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Process manual = new Process();
                manual.StartInfo.FileName = gamefile;
                manual.Start();
                MessageBox.Show("下载完成后点击确定");
                MessageBox.Show("将文件重命名为2dmmc.dat后放在启动器目录下点击确定");
                while (!File.Exists("2dmmc.dat"))
                {
                    if (File.Exists("2dmmc.7z"))
                    {
                        File.Move("2dmmc.7z", "2dmmc.dat");
                        Application.Restart();
                    }
                    if (!(MessageBox.Show("没找到2dmmc.dat", "没找到文件", MessageBoxButtons.RetryCancel) == DialogResult.Retry))
                    {
                        break;
                    }
                }
                if (File.Exists("2dmmc.dat"))
                {
                    Application.Restart();
                }
            }
            Environment.Exit(0);
        }
        
        void game_Speed(downloader sender)
        {
            speed.Text = sender.SpeedStr;
        }
        void game_Finished(downloader sender)
        {
            WebClient d7z = new WebClient();
            d7z.DownloadFile("http://image.bangbang93.com/7za.exe", "unpakcer.exe");
            
            if (File.Exists("2dmmc4th.7z"))
            {
                File.Move("2dmmc4th.7z", "2dmmc.dat");
            }
            if (File.Exists("2dmmc.dat") == true)
            {
                Process un7z = new Process();
                un7z.StartInfo.FileName = "unpakcer.exe";
                un7z.StartInfo.Arguments = "x -y 2dmmc.dat";
                //un7z.Exited += new EventHandler(un7z_Exited);
                un7z.Start();
                un7z.WaitForExit();
                File.Delete("unpakcer.exe");
                this.Close();
             }
                else
                {
                    MessageBox.Show("找不到下载的文件");
                    Environment.Exit(2);
                 }
        }

        void un7z_Exited(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
        }

        void game_Progress(downloader sender)
        {
            progressBar1.Value = sender.FinishedRate;
            prog.Text = sender.FinishedRate.ToString()+"%";
        }

        private void buttonCancal_Click(object sender, EventArgs e)
        {
            game.Stop();
            Environment.Exit(0);
            //下载器依靠自身生成的配置文件可以实现断点续传，虽然不太稳定
        }
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

    }
}
