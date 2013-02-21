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

namespace _2dmmclauncher
{
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

        void getDirInfo(string remoteDir, string localDir)
        {
            //Console.WriteLine(remoteDir);
            ArrayList remoteFile = yunfile.readDir(remoteDir);
            DirectoryInfo localFile = new DirectoryInfo(localDir);
            checkDir.Text = remoteDir;
            checkDir.Refresh();
            progressBar1.Maximum += remoteFile.Count;
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
                //if(file.filetype=="D")
                //{
                //    getDirInfo(remoteDir+"/"+file.filename, mcDir+"/" + file.filename);
                //}

            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            //Console.WriteLine(yunfile.getBucketUsage());
        }

        private void update_Shown(object sender, EventArgs e)
        {
            ArrayList remoteFile = yunfile.readDir("/2dmmc4th");
            progressBar1.Maximum = remoteFile.Count;
            getDirInfo("/2dmmc4th", mcDir.FullName);
            this.Close();
        }
    }
}