using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;
namespace SmartSync
{
    public partial class su_pdate : Form
    {   
        string update_location;
        string filename = @"C:\Users\Public\Files\smartsync.xml";
        string fullname; 
        public su_pdate()
        {
            InitializeComponent();
            
        }

        private void su_pdate_Load(object sender, EventArgs e)
        {
            
            string[] results =checkUpdate().Split(':');
            if (results.GetUpperBound(0) >2)
            {
                label1.Text ="An update for SmartSync is available. Version" + results[0];
                string[] features = results[1].Split(',');
                listBox1.Items.Clear();
                listBox1.Items.Add("----------------------------------------------");
                listBox1.Items.Add("New features");
                listBox1.Items.Add("----------------------------------------------");
                foreach (string f in features)
                {
                    listBox1.Items.Add(f);
                }
                listBox1.Items.Add("");
                listBox1.Items.Add("----------------------------------------------");
                listBox1.Items.Add("Update fixes");
                listBox1.Items.Add("----------------------------------------------");
                foreach (string f in results[2].Split(','))
                {
                    listBox1.Items.Add(f);
                }
                update_location = results[3];

            }
            else
            {
                label1.Text = results[0];
                listBox1.Visible = false;
            }
        }

       
        /// <summary>
        /// Checks if the the application has any update
        /// </summary>
        /// 
        public string checkUpdate()
        {
            try
            {
                //get the current version of the application
                string version = "1.0.0.0";
                //string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNodeList nver = doc.GetElementsByTagName("Program_Version");
                string newVersion = nver[0].ChildNodes[0].Value;
                fullname = doc.GetElementsByTagName("Program_Name")[0].ChildNodes[0].Value;
                if (version == newVersion)
                {
                    return "You are using the latest version of SmartSync";
                }
                else
                {
                    string features = doc.GetElementsByTagName("Features")[0].ChildNodes[0].Value;
                    string fixes = doc.GetElementsByTagName("fixes")[0].ChildNodes[0].Value;
                    string location = doc.GetElementsByTagName("location")[0].ChildNodes[0].Value;

                    return string.Format(" {0}:{1}:{2}:{3}", newVersion, features, fixes, location);
                }
            }
            catch
            {
                return "There was an error checking your update. Please check your internet connection and try again.";
            }



        }


        public  void getUpdate(string location)
        {
            string destination = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SmartSync");
            try
            {
                WebClient client = new WebClient();
                Uri uri = new Uri(location);
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;
                listBox1.Items.Clear();
                listBox1.Items.Add("Downloading update...");
                client.DownloadFileAsync(uri, destination + @"/" + fullname);
               
            }
            catch {
                MessageBox.Show("There was an error downloading the update.");
            }
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            listBox1.Items.Add("Download completed...");
            listBox1.Items.Add("Installing update....");
           
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            listBox1.Items.Add((e.BytesReceived / 1024).ToString() + "KB of " + (e.TotalBytesToReceive / 1024).ToString() + "KB downloaded. " + e.ProgressPercentage.ToString() + "% ...");
            progressBar1.Maximum = 100;
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
