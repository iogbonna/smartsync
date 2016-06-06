using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace SmartSync
{
    public partial class driveBrowser : Form
    {
        FlowLayoutPanel panel;
        public driveBrowser(Form2 frm)
        {
            InitializeComponent();
            panel = (FlowLayoutPanel)frm.Controls.Find("panel1", true)[0];
            foreach (Control ctrl in panel.Controls)
            {
                panel1.Controls.Add(ctrl); 
               
            }
            if (DriveInformation.sdrv != "")
            {
                usbDevice device = (usbDevice)panel1.Controls.Find(DriveInformation.sdrv+":\\", true)[0];
                device.BorderStyle = BorderStyle.FixedSingle;
                label2.Text = DriveInformation.sdrv;
                checkDirectory(DriveInformation.sdrv + DriveInformation.sdrv_folder.Substring(1, DriveInformation.sdrv_folder.Length - 1));
            }
            textBox1.Text = DriveInformation.sdrv_folder.Substring(3,DriveInformation.sdrv_folder.Length-3);
            this.FormClosing += (s, a) => {
                foreach (Control ctrl in panel1.Controls)
                {
                    panel.Controls.Add(ctrl);
                }
            };
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            DriveInformation.sdrv_folder = textBox1.Text;
            this.Close();
        }
        private void checkDirectory(string name)
        {
            if (Directory.Exists(name))
            {

                string[] files = Directory.GetFiles(name);
                if (files.Length > 0)
                {
                    lblstatus.Text = "This folder already exists and contains some files. There might be conflicts synchronizing with this folder.";
                }
                else { lblstatus.Text = "This folder exits and contains no files. You can synchronize to this folder "; }
            }
            else { lblstatus.Text = "This folder does not exist but will be created. You can synchronize to this folder "; }
        }

      

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (label2.Text != "" )
                checkDirectory(label2.Text + ":\\" + textBox1.Text);
            //if (DriveInformation.sdrv != "")
                          
        }
    }
}
