using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UpYunLibrary;
using System.IO;
using System.Collections;
using System.Net;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Threading;

namespace _2dmmclauncher
{
    delegate byte[] DownFileDel (string filename);
    public partial class update : Form
    {
        public update()
        {
            InitializeComponent();
        }
        public static UpYun yunfile = new UpYun("judgement", "bangbang93", "cheese1steak");
        static DirectoryInfo mcDir = new DirectoryInfo(@".minecraft");
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
        #region 废弃的方法
        void getDirInfo(string remoteDir, string localDir)
        {
            //Console.WriteLine(remoteDir);
            ArrayList remoteFile = yunfile.readDir(remoteDir);
            DirectoryInfo localFile = new DirectoryInfo(localDir);
            checkDir.Text = remoteDir;
            checkDir.Refresh();
            TaskbarManager.Instance.SetProgressValue(progressBar1.Value, progressBar1.Maximum);
            if (!localFile.Exists)
            {
                Directory.CreateDirectory(localFile.FullName);
            }
            foreach (FolderItem file in remoteFile)
            {
                switch (file.filetype)
                {
                    case "F":
                        {
                            getDirInfo(remoteDir + "/" + file.filename, localDir + "\\" + file.filename);
                            break;
                        }
                    case "N":
                        {
                            checkFile.Text = file.filename;
                            checkFile.Refresh();
                            if (File.Exists(localDir + "\\" + file.filename))
                            {
                                if (file.size != (new FileInfo(localDir + "\\" + file.filename).Length))
                                {
                                    downFile.Text = "正在下载" + file.filename;
                                    downFile.Refresh();
                                    byte[] newfile = yunfile.readFile(remoteDir + "/" + file.filename);
                                    FileStream filewriter = new FileStream(localDir + "\\" + file.filename, FileMode.Create);
                                    filewriter.Write(newfile, 0, newfile.Length);
                                    filewriter.Close();
                                    downFile.Text = "";

                                    //Console.WriteLine(remoteDir + "/" + file.filename + " " + file.number);
                                }
                                //Console.WriteLine(remoteDir + "/" + file.filename + " " + file.number);
                            }
                            else
                            {
                                downFile.Text = "正在下载" + file.filename;
                                downFile.Refresh();
                                byte[] newfile = yunfile.readFile(remoteDir + "/" + file.filename);
                                FileStream filewriter = new FileStream(localDir + "\\" + file.filename, FileMode.Create);
                                filewriter.Write(newfile, 0, newfile.Length);
                                filewriter.Close();
                                downFile.Text = "";
                            }
                            progressBar1.Value += 1;
                            TaskbarManager.Instance.SetProgressValue(progressBar1.Value, progressBar1.Maximum);
                            break;
                        }

                }

            }
        }
        #endregion
        #region 下载线程
        private byte[] DownFile(string file)
        {
            byte[] downfile = yunfile.readFile(file);
            return downfile;
        }
        #endregion
        private void update_Shown(object sender, EventArgs e)
        {
            DownFileDel dfd = new DownFileDel(DownFile);
            this.Refresh();
            checkDir.Text = "正在获取校验文件";
            checkDir.Refresh();
            checkFile.Text = "md5.txt";
            checkFile.Refresh();
            WebClient md5file = new WebClient();
            md5file.DownloadFile("http://file.bangbang93.com/2dmmc4th/md5.txt", "rmd5.txt");
            StreamReader rmd5 = new StreamReader("rmd5.txt");
            checkDir.Text = "正在读取校验文件";
            checkDir.Refresh();
            string[,] md5 = new string[2000, 2];
            int i = 0;
            while (rmd5.EndOfStream == false)
            {
                string str = rmd5.ReadLine();
                string[] spliter = { "|" };
                md5[i, 0] = str.Split(spliter, StringSplitOptions.RemoveEmptyEntries)[0];
                md5[i, 1] = str.Split(spliter, StringSplitOptions.RemoveEmptyEntries)[1];
                i++;
            }
            int total = i;
            progressBar1.Maximum = total;
            for (i = 0; i <= total; i++)
            {
                try
                {
                    progressBar1.Value++;
                }
                catch { }
                if (md5[i, 0]==null )
                {
                    break;
                }
                string lmd5="";
                if (File.Exists(Environment.CurrentDirectory + @"\.minecraft" + md5[i, 0]))
                {
                    try
                    {
                        lmd5 = GetMD5HashFromFile(Environment.CurrentDirectory + @"\.minecraft" + md5[i, 0]);
                    }
                    catch
                    {
                        if (MessageBox.Show("minecraft程序文件被占用，是否尝试强行解除占用（如果此信息重复出现，请换个姿势再尝试）", "文件被占用", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            Form1.launcher.Kill();
                            this.DialogResult = DialogResult.Retry;
                            this.Close();
                            return;
                        }

                    }
                }
                checkDir.Text=md5[i,0].Substring(0,md5[i,0].LastIndexOf(@"\"));
                checkDir.Refresh();
                checkFile.Text = md5[i, 0].Substring(md5[i, 0].LastIndexOf(@"\"), md5[i, 0].Length - md5[i, 0].LastIndexOf(@"\"));
                checkFile.Refresh();
                if (lmd5 != md5[i, 1])
                {
                    downFile.Text = "正在下载" + md5[i,0]+"|"+yunfile.getFileInfo("/2dmmc4th"+md5[i,0])["size"]+"字节";
                    if (Convert.ToInt32( yunfile.getFileInfo("/2dmmc4th" + md5[i, 0])["size"]) == 0)
                    {
                        File.Delete(md5[i, 0]);
                    }
                    downFile.Refresh();
                    IAsyncResult res = dfd.BeginInvoke("/2dmmc4th" + md5[i, 0], null, null);
                    while (!res.IsCompleted)
                    {
                        Thread.Sleep(50);
                        Application.DoEvents();
                    }
                    byte[] downfile = dfd.EndInvoke(res);
                    //byte[] downfile = yunfile.readFile("/2dmmc4th" + md5[i, 0]);
                    if (!Directory.Exists(md5[i,0].Substring(0,md5[i,0].LastIndexOf('\\'))))
                    {
                        Directory.CreateDirectory(md5[i,0].Substring(0,md5[i,0].LastIndexOf('\\')));
                    }
                    FileStream writer = new FileStream(Environment.CurrentDirectory + @"\.minecraft" + md5[i, 0], FileMode.Create);
                    writer.Write(downfile, 0, downfile.Length);
                    downFile.Text = "";
                    downFile.Refresh();
                }
            }
            this.Close();
        }

        private void update_Load(object sender, EventArgs e)
        {

        }
    }
}