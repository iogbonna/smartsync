using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Synchronization;
//using Dolinay;
using System.Threading;
using Microsoft.Synchronization.Files;
//using LibUsbDotNet.DeviceNotify;
using System.IO;
namespace SmartSync
{
    public partial class Form1 : Form
    {
       // private FileDetailsMgmt mgmt = new FileDetailsMgmt();
        private List<FileDetails> details=new List<FileDetails>();
       // public static IDeviceNotifier usbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();
        DriveDetector driveDetector = null;
        private string drive;
        public Form1()
        {
            InitializeComponent();
            driveDetector = new DriveDetector();
            driveDetector.DeviceArrived += new DriveDetectorEventHandler(OnDriveArrived);
            driveDetector.DeviceRemoved += new DriveDetectorEventHandler(OnDriveRemoved);
      //   usbDeviceNotifier.OnDeviceNotify +=new EventHandler<DeviceNotifyEventArgs>(usbDeviceNotifier_OnDeviceNotify);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DriveInfo drv = new DriveInfo("E");
           
        }


        //private  void usbDeviceNotifier_OnDeviceNotify(object sender, DeviceNotifyEventArgs e)
        
            
        //{
           
        //    string value = e.Volume.Letter;
        //    DriveInfo info = new DriveInfo(value);
        //   // string serial = e.Device.SerialNumber;
        //  //  string vendor = e.Device.IdVendor.ToString();
        //    MessageBox.Show(e.Device.SerialNumber);
           
        //    MessageBox.Show(e.DeviceType.ToString());
        //    MessageBox.Show(e.EventType.ToString());
        //   // MessageBox.Show("yes");
        //   //DriveDetectorEventArgs m = new DriveDetectorEventArgs();
        //   //OnDriveArrived(sender, m);

        //    devices dv = new devices();
        //    dv.Name = drive;
        //    loadDevice(dv);
        //    //calls up the method that will check if the device has been configured for synchronization
        //    checkDevice(e.Device.IdProduct.ToString(), e.Device.IdVendor.ToString(), e.Device.SerialNumber, drive);
        //}

        private void OnDriveArrived(object sender, DriveDetectorEventArgs e)
        {
            
            drive = e.Drive;
         
          //  DeviceNotifyEventArgs ar = (DeviceNotifyEventArgs)e;
          //  usbDeviceNotifier_OnDeviceNotify(sender, ar);
           
          //  MessageBox.Show(e.Drive.ToString() + " " + e.ToString());
        }

        private void OnDriveRemoved(object sender, DriveDetectorEventArgs e)
        {
   
        }


        private void checkAll()
        { 
            //get all the devices in the system
        }





        private void checkDevice(string productid, string vendorid, string serial,string mdrive)
        {
            //gets the details of the device
            //FileDetails filedetails = mgmt.getDetail(vendorid,productid, serial);
            //if (filedetails != null)
            //{
            //    devices device = new devices();
            //    device.AutoSync = filedetails.settings.AutoSync;
            //    device.keepsource = filedetails.settings.KeepSource;
            //    device.folders = filedetails.sources;
            //    loadDevice(device);
            //    ProgressCtrl progress=new ProgressCtrl();
            //    //start the process of synchronisation using the user settings specified by the user
            //    Action act = new Action(productid,vendorid,serial,progress);
            //    Thread secondaryThread = new Thread(new ThreadStart(act.executeSync));
               
            //    secondaryThread.Start();
            //}
            //else
            //{
            //    devices device = new devices();
                
            //    //somehow I have to differentiate between a device that is configured already and the one that is yet to be configured
            //    //there should be a slight difference on how this different devices are loaded
            //    loadDevice(device);
            //    //using the application settings, prompt the user for the sync of the new device
                
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MenuItem item=new MenuItem();
            item.Text = "how";
            item.Click += (s, a) => {this.Show(); };
        
            ContextMenu menu=new ContextMenu();
            menu.MenuItems.AddRange(new MenuItem[] {item});
           // notifyIcon1.Icon=new Icon("");
            //notifyIcon1 = new NotifyIcon(this.components);
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.ContextMenu = menu;
            notifyIcon1.Text = "Notification example";
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(5000);
            notifyIcon1.BalloonTipClicked += (s, a) => {this.Show(); };
            this.Hide();
            //devices dv=new devices();
            //loadDevice(dv);
        }
        private void loadDevice(devices dv)
        {
           // devices dv = new devices();
            int number = panel1.Controls.Count;
            dv.Location = new Point(0, number * 60);
            panel1.Controls.Add(dv);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opfd = new OpenFileDialog();
            string source="";
            string destination="";
            
            opfd.Title = "Select the source of the files";
            if (opfd.ShowDialog() == DialogResult.OK || opfd.ShowDialog() == DialogResult.Yes)
            {
                
                FileInfo file = new FileInfo(opfd.FileName);
                source =file.DirectoryName ;
            }
            opfd.Title = "Select the destination of the files";
            if (opfd.ShowDialog() == DialogResult.OK || opfd.ShowDialog() == DialogResult.Yes)
            {
                FileInfo file = new FileInfo(opfd.FileName);
                destination = file.DirectoryName;
            }
            Sync sync = new Sync();
            sync.Synchronize(source, destination);
        }

   
    }
   
}
