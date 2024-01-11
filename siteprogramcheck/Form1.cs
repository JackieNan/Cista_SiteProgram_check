using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Diagnostics;

namespace siteprogramcheck
{
    public partial class Form1 : Form
    {
        public string ip = "";
        public string selectedOption = "";
        private string user = "";
        private string pass = "";
        private int site_cont = 0;
        public Form1()
        {
            InitializeComponent();
            string[] options =
            {
                "HT01", "HT02", "HT03", "HT04", "HT05", "HT06",
                "Chroma01", "Chroma02", "Chroma03", "Chroma04", "Chroma05", "Chroma06", "Chroma07", "Chroma08",
                "UTL01", "UTL02"
            };
            comboBox1.Items.AddRange(options);
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            comboBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            dataGridView1.RowHeadersVisible = false;
        }
        public string file_content(string path)
        {

            string content = File.ReadAllText(path);
            string[] lines = content.Split(new[] { "\r\n" }, StringSplitOptions.None);
            string targetLine = lines.FirstOrDefault(line => line.StartsWith("Test Progarm = "));
            string testprogram = "";
            if (targetLine != null)
            {
                testprogram = targetLine.Substring("Test Program =".Length).Trim();
            }
            return testprogram;
        }
        public class SiteProgram
        {
            public int Site { get; set; }
            public string Program { get; set; }
        }
       


        private List<string> dataList = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = false;
            int site = 1;
            dataList.Clear();
            //读选择机台的型号前两位以分配凭证的用户名密码
            string firstTwoLetters = selectedOption.Substring(0, 2);
            string lastOneLetters = selectedOption.Substring(selectedOption.Length - 1, 1);//机台末两位
            int num = int.Parse(lastOneLetters);//机台末位
            if (firstTwoLetters == "HT")
            {
                user = "it";
                pass = "admin123";
                site_cont = 4;
                    for (; site <= site_cont; site++)
                    {
                        ip = @"\\192.168.105." + num + site;
                        if (!Directory.Exists(ip + @"\d\PGM\2599_FT_NORMAL_RWMIPI_DT0606")) 
                        {connectState(ip, user, pass);}
                        if (!Directory.Exists(ip + @"\d\PGM\2599_FT_NORMAL_RWMIPI_DT0606"))
                        {
                            string not_existed = "未查询到相关数据";
                            dataList.Add(not_existed);

                        }
                        else
                        {
                            string content = file_content(ip + @"\d\PGM\2599_FT_NORMAL_RWMIPI_DT0606\config.ini");
                            dataList.Add(content);
                        }
                    }

                    List<SiteProgram> dataSource = new List<SiteProgram>();
                    int siteCounter = 1;
                    foreach (var program in dataList)
                    {
                        if (string.IsNullOrEmpty(program))
                        {
                            dataSource.Add(new SiteProgram { Site = siteCounter++, Program = "未找到程序" });
                        }
                        else
                        {
                            dataSource.Add(new SiteProgram { Site = siteCounter++, Program = program });

                        }
                    }

                    dataGridView1.DataSource = dataSource;

            }
            else if (firstTwoLetters == "Ch")
            {
                user = "user";
                site_cont = 16;

                if (num != 5)
                {
                    for (; site <= site_cont; site++)
                    {
                        ip = @"\\192.168.10" + num + "." + site ;
                        if (!Directory.Exists(ip + @"\prod\Prod"))
                        {connectState(ip, user, site.ToString()); }

                        if (!Directory.Exists(ip + @"\prod\Prod"))
                        {
                            string not_existed = "未查询到相关数据";
                            dataList.Add(not_existed);

                        }
                        else
                        {
                            string content = file_content(ip + @"\prod\Prod\config.ini");
                            dataList.Add(content);
                        }



                    }
                }
                else if (num == 5)
                {
                    for (; site <= site_cont; site++)
                    {
                        ip = @"\\192.168.100." + site;
                        if (!Directory.Exists(ip + @"\prod\Prod"))
                        {
                            connectState(ip, user, site.ToString());
                        }
                        if (!Directory.Exists(ip + @"\prod\Prod"))
                        {
                            string not_existed = "未查询到相关数据";
                            dataList.Add(not_existed);

                        }
                        else
                        {
                            string content = file_content(ip + @"\prod\Prod\config.ini");
                            dataList.Add(content);
                        }
                    }
                }
                List<SiteProgram> dataSource = new List<SiteProgram>();
                int siteCounter = 1;
                foreach (var program in dataList)
                {
                    if (string.IsNullOrEmpty(program))
                    {
                        dataSource.Add(new SiteProgram { Site = siteCounter++, Program = "未找到程序" });
                    }
                    else
                    {
                        dataSource.Add(new SiteProgram { Site = siteCounter++, Program = program });

                    }
                }
                dataGridView1.DataSource = dataSource;
                
            }

            else if (firstTwoLetters == "UT")
            {
                user = "admin";
                pass = "";
                site_cont = 8;
                    if (num == 1)
                    {
                        for (; site <= site_cont; site++)
                        {
                            ip = @"\\192.168.109." + site;
                            if (!Directory.Exists(ip + @"\d\prod\Prod\Site1")||!Directory.Exists(ip + @"\d\prod\Prod\Site2"))
                            { connectState(ip, user, pass); }
                            if (!Directory.Exists(ip + @"\d\prod\Prod\Site1") || !Directory.Exists(ip + @"\d\prod\Prod\Site2"))
                            {
                                string not_existed = "未查询到相关数据";
                                dataList.Add(not_existed);
                            }
                            else
                            {
                                string content1 = file_content(ip + @"\d\prod\Prod\Site1" + @"\config.ini");
                                dataList.Add(content1);
                                string content2 = file_content(ip + @"\d\prod\Prod\Site2" + @"\config.ini");
                                dataList.Add(content2);
                            }
                        }
                        }
                    else if (num == 2)
                    {
                        for (; site <= site_cont; site++)
                        {
                            ip = @"\\192.168.111." + site;
                            if (!Directory.Exists(ip + @"\d\prod\Prod\Site1") || !Directory.Exists(ip + @"\d\prod\Prod\Site2"))
                            { connectState(ip, user, pass); }
                            if (!Directory.Exists(ip + @"\d\prod\Prod\Site1") || !Directory.Exists(ip + @"\d\prod\Prod\Site2"))
                            {
                                string not_existed = "未查询到相关数据";
                                dataList.Add(not_existed);

                            }
                            else
                            {
                                string content1 = file_content(ip + @"\d\prod\Prod\Site1" + @"\config.ini");
                                dataList.Add(content1);
                                string content2 = file_content(ip + @"\d\prod\Prod\Site2" + @"\config.ini");
                                dataList.Add(content2);
                            }

                        }
                    }
                    List<SiteProgram> dataSource = new List<SiteProgram>();
                    int siteCounter = 1;
                    foreach (var program in dataList)
                    {
                        if (string.IsNullOrEmpty(program))
                        {
                            dataSource.Add(new SiteProgram { Site = siteCounter++, Program = "未找到程序" });
                        }
                        else
                        {
                            dataSource.Add(new SiteProgram { Site = siteCounter++, Program = program });

                        }
                    }
                    dataGridView1.DataSource = dataSource;
            }
            else
            {
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedOption = comboBox1.SelectedItem.ToString();
        }
        static bool connectState(string path, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use " + path + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataList.Clear();
            dataGridView1.DataSource = null;
        }
    }
}
