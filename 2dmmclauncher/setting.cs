using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace _2dmmclauncher
{
    public partial class setting : Form
    {
        public setting()
        {
            InitializeComponent();
        }
        public string cfgfile = "2dmmccfg.xml";
        public string playername;
        public string javaxmx;
        public string javaw;
        private void setting_Load(object sender, EventArgs e)
        {
            if (File.Exists(cfgfile))
            {
                XmlDocument cfg = new XmlDocument();
                cfg.Load(cfgfile);
                XmlNode cfgroot = cfg.SelectSingleNode("edmmc");
                XmlElement playerinfo = (XmlElement)cfgroot.SelectSingleNode("PlayerInfo");
                playername = playerinfo.Attributes["playername"].Value;
                XmlElement javainfo = (XmlElement)cfgroot.SelectSingleNode("JavaInfo");
                javaw = javainfo.Attributes["javaw"].Value;
                javaxmx = javainfo.Attributes["javaxmx"].Value;
            }
            else
            {
                MessageBox.Show("读取配置文件失败");
                this.Close();
                return;
            }
            playerNameBox.Text = playername;
            JavaXmxBox.Text = javaxmx;
            javawBox.Text = javaw;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            javawpath.Multiselect = false;
            javawpath.Title = "请选择javaw.exe";
            javawpath.Filter = "javaw.exe|javaw.exe";
            if (javawpath.ShowDialog() == DialogResult.OK)
            {
                javawBox.Text = javawpath.FileName;
            }
        }

        private void playerNameBox_TextChanged(object sender, EventArgs e)
        {
            if (playerNameBox.Text != playername)
            {
                Apply.Text = "应用(&Apply)";
            }
            else
            {
                Apply.Text = "确定(&OK)";
            }
        }

        private void JavaXmxBox_TextChanged(object sender, EventArgs e)
        {
            if (JavaXmxBox.Text!=javaxmx)
            {
                Apply.Text = "应用(&Apply)";
            }
            else
            {
                Apply.Text = "确定(&OK)";
            }
        }

        private void javawBox_TextChanged(object sender, EventArgs e)
        {
            if (javawBox.Text!=javaw)
            {
                Apply.Text = "应用(&Apply)";
            }
            else
            {
                Apply.Text = "确定(&OK)";
            }
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            if (Apply.Text == "确定(&OK)")
            {
                if (MessageBox.Show("所有设置在重新启动游戏后生效，是否立即重新启动", "设置改动", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    Form1.launcher.EnableRaisingEvents = false;
                    Form1.launcher.Kill();
                    Form1.launcher.Start();
                    Form1.launcher.EnableRaisingEvents = true;
                }
                
                this.Close();
                return;
            }
            else
            {
                XmlDocument cfg = new XmlDocument();
                XmlDeclaration xmldecl;
                xmldecl = cfg.CreateXmlDeclaration("1.0", "utf-8", null);
                cfg.AppendChild(xmldecl);
                XmlElement cfgvalue = cfg.CreateElement("edmmc");
                cfg.AppendChild(cfgvalue);
                XmlNode cfgroot = cfg.SelectSingleNode("edmmc");
                XmlElement player = cfg.CreateElement("PlayerInfo");
                player.SetAttribute("playername", playerNameBox.Text);
                cfgvalue.AppendChild(player);
                XmlElement JavaInfo = cfg.CreateElement("JavaInfo");
                JavaInfo.SetAttribute("javaxmx", JavaXmxBox.Text);
                JavaInfo.SetAttribute("javaw", javawBox.Text);
                cfgvalue.AppendChild(JavaInfo);
                cfg.AppendChild(cfgvalue);
                cfg.Save(cfgfile);
            }
        }
    }
}
