using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Management;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using USBLib;
namespace SmartSync
{
    public partial class Form2 : Form
    {
       
        DriveDetector driveDetector = null;
        List<Drive> drives = new List<Drive>();
        Drive selectedDrive = new Drive();
        object threadlock = new object();
        public string letter="";
        public Form2()
        {
            
            InitializeComponent();
            
            //create event handler for the detection of the usb devices
            driveDetector = new DriveDetector();
            driveDetector.DeviceArrived += new DriveDetectorEventHandler(OnDriveArrived);
            driveDetector.DeviceRemoved += new DriveDetectorEventHandler(OnDriveRemoved);
            selectedDrive = null;
            toolStrip1.Visible = false;
            this.Load += (s, a) => {
                
               //Load the settings file of the project
                if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"SmartSync")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SmartSync"));
                   
                    
                }
                listView1.AllowDrop = true;
               
                //Load the system directories in a treeview

                treeView1.Nodes.Clear();
                DriveInfo[] ds = DriveInfo.GetDrives();

                foreach (DriveInfo d in ds)
                {

                   
                        USB.USBDevice device = USB.FindDriveLetter(d.Name.Substring(0, 2));
                        if (device == null)
                        {
                            if (d.IsReady && d.TotalSize > 0 && IsNotReadOnly(d.ToString()) )
                            {
                                //populate the list view with the directories in the drives of the system

                                TreeNode parentNode = new TreeNode();
                                parentNode.ImageIndex = 0;
                                parentNode.SelectedImageIndex = 0;
                                parentNode.Text = d.RootDirectory.ToString();
                                //string[] dir = getDirectories(di.RootDirectory.ToString());
                                treeView1.Nodes.Add(parentNode);

                                foreach (var si in d.RootDirectory.GetDirectories())
                                {
                                    if ((si.Attributes & FileAttributes.System) == FileAttributes.System) continue;
                                    TreeNode child = new TreeNode();
                                    child.ImageIndex = 0;
                                    child.Name = si.FullName.ToString();
                                    child.SelectedImageIndex = 0;

                                    child.Text = si.Name;
                                    parentNode.Nodes.Add(child);

                                }
                                parentNode.Expand();
                            }
                        }
                }
               
                //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                
                drives = FileDetailsMgmt.getRegisteredDrives();

               
                                       statuslabel.Text = "Loading Application settings...";
                Application.DoEvents();
                                      
                                       statuslabel.Text = "Loading USB drives...";

                                       //Load the usb devices that are connected to the system
                                           LoadConnectedDevices();
                                       
                MenuItem item = new MenuItem();
                item.Text = "Open";
                item.Click += (b, y) => { Show(); };

                MenuItem item1 = new MenuItem();
                item1.Text = "Exit";
                item1.Click += (t, y) => { Application.ExitThread(); };
                ContextMenu contextMenu = new ContextMenu();
                contextMenu.MenuItems.AddRange(new MenuItem[] { item, item1 });
                notifyIcon1.ContextMenu = contextMenu;
            };
            
        }

      


        /// <summary>
        /// Loads the directories of a selected node to it
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="parent"></param>
        private void getDirectories(string dr, TreeNode parent)
        {
            
            if (Directory.Exists(dr))
            {
                DirectoryInfo drInfo = new DirectoryInfo(dr);
                foreach (var si in drInfo.GetDirectories())
                {
                    if ((si.Attributes & FileAttributes.System) == FileAttributes.System) continue;
                    TreeNode child1 = new TreeNode();
                    if (selectedDrive.source.Count > 0 || selectedDrive.source!=null)
                    {
                        foreach (var s in selectedDrive.source)
                        {
                            if (s[0] == si.FullName.ToString())
                            {

                                child1.ImageIndex = 1;

                                child1.SelectedImageIndex = 1;
                                break;
                            }
                            else
                            {
                                child1.ImageIndex = 0;

                                child1.SelectedImageIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        child1.ImageIndex = 0;

                        child1.SelectedImageIndex = 0;
                    }
                    child1.Text = si.Name;
                    child1.Name = si.FullName.ToString();
                    parent.Nodes.Add(child1);

                    

                }
            }
            else
                return;
        }
        string serial;
        void OnDriveArrived(object sender, DriveDetectorEventArgs e)
        {
            //get the information of the usb device that just arrived at the system
            DriveInfo drive = new DriveInfo(e.Drive);
            if (drive.TotalSize > 0 &&  IsNotReadOnly(e.Drive))
            {
                letter = e.Drive.Substring(0, 1);

                serial =DriveInformation.getSerial(e.Drive); //USB.FindDriveLetter(e.Drive.Substring(0, 2)).SerialNumber;
                //check if the drive have been registered before else prompt for the registration of the drive
               
                //RegisterDevice(serial);
                loadDevice(e.Drive);
                if (SmartSync.Properties.Settings.Default.notifyoniteminsert == false) return;
                Drive drv = FileDetailsMgmt.thisDrive(serial, drives);
                if (drv == null)
                {
                    DialogResult result = MessageBox.Show("A new device is inserted into your system,\n Do you want to synchronize it?", "SmartSync", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result.ToString() == "No") return;
                    else
                        RegisterDevice(serial);
                }
                NotifyDetection(e.Drive.Substring(0,1));
            }
            
            //letter = e.Drive.Substring(0, 1);
          

        }
        /// <summary>
        /// Loads a UsbDevice to the applications
        /// </summary>
        /// <param name="driveletter">The name of the drive</param>
        private void loadDevice(string driveletter)
        {
            try
            {
                usbDevice device = new usbDevice(driveletter, drives, serial);
                device.AllowDrop = true;
                device.DragEnter += device_DragEnter;
                device.DragDrop += device_DragDrop;
                device.Anchor = AnchorStyles.Left;
                device.Name = driveletter;
                int number = panel1.Controls.Count;
                
               // device.Location = new Point(2, number * 50);
                device.Margin = new System.Windows.Forms.Padding(5);
                //device.Width = 350;
                panel1.Controls.Add(device);

            }
            catch (Exception e)
            {
                Helper.WriteLog(e);
            }
        
           
        }

        void device_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] file = (string[])e.Data.GetData(DataFormats.FileDrop);
                    foreach (string f in file)
                    {

                        string[] dirs = new string[2];
                        dirs[0] = f;
                        if (!Directory.Exists(dirs[0])) return;
                        DriveInformation.sdrv_folder = f;
                        usbDevice dv = (usbDevice)sender;
                        DriveInformation.sdrv = dv.letter.Substring(0,1);
                        
                        //load the dialog form to select the drive you want to synchronize and the folder you want to do that with
                        driveBrowser browser = new driveBrowser(this);
                        if (browser.ShowDialog().ToString() == "Cancel") return;

                        //retrieve the values set from the dialog
                        //check if the device has been registered else register it
                        RegisterDevice(DriveInformation.sdrv_serial);
                        Drive drive = drives.Find(i => (i.serial) == DriveInformation.sdrv_serial);
                        if (drive == null) return;
                        if (drive.source.Count > 0)
                        {
                            //check if the folder has originally been synchronized
                            foreach (string[] s in drive.source)
                            {
                                if (s[0] == dirs[0])
                                {

                                    dirs[1] =DriveInformation.sdrv_folder;

                                    drives.Remove(drive);
                                    drive.source.Remove(s);

                                    if (drive.source != null)
                                        drive.source.Add(dirs);
                                    else
                                    {
                                        drive.source = new List<string[]>();
                                        drive.source.Add(dirs);
                                    }
                                    drives.Add(drive);
                                    FileDetailsMgmt.saveDrive(drives);
                                    drives = FileDetailsMgmt.getRegisteredDrives();
                                    statuslabel.Text = "Folder added successfully";


                                }
                            }
                        }
                        else
                        {


                            dirs[1] = DriveInformation.sdrv + ":\\" + DriveInformation.sdrv_folder;
                            drives.Remove(drive);
                            drive.autosync = true;
                            if (drive.source != null)
                                drive.source.Add(dirs);
                            else
                            {

                                drive.source = new List<string[]>();
                                drive.source.Add(dirs);
                            }
                            drives.Add(drive);
                            FileDetailsMgmt.saveDrive(drives);
                            drives = FileDetailsMgmt.getRegisteredDrives();
                            statuslabel.Text = "Folder added successfully";

                        }


                        

                    }


                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);
            }
        }

        void device_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        void OnDriveRemoved(object sender, DriveDetectorEventArgs e)
        {
           
            string name = e.Drive;
            
            UnloadDevice(name);
            NotifyRemoval(name.Substring(0, 1));
            if (letter == name.Substring(0, 1))
            {
                listView1.Items.Clear();
                foreach (TreeNode node in treeView1.Nodes)
                {
                    foreach (TreeNode n in node.Nodes)
                    {
                        n.ImageIndex = 0;
                        n.SelectedImageIndex = 0;
                    }
                }

            }
            
          
            
        }

        /// <summary>
        /// Unload the selected drive from the program
        /// </summary>
        /// <param name="driveletter">The label of the selected drive</param>
        private void UnloadDevice(string driveletter)
        {
            usbDevice device =(usbDevice)fpanel1.Controls.Find(driveletter, true)[0]; 

            if (panel1.Controls.Contains(device))
            {
                panel1.Controls.Remove(device);
                device.Dispose();
                panel1.Refresh();
            }

        }
        
       List<string[]> dList=new List<string[]>(); 

        

        /// <summary>
        /// Register a usb device to the program
        /// </summary>
        /// <param name="o">The device to be registered</param>
        private void RegisterDevice(object o)
        {
            try
            {
                string sl = o as string;
                Drive drive = new Drive();
                drives = FileDetailsMgmt.getRegisteredDrives();
                drive.autosync = true;
                drive.serial = sl;
                Drive drv = null;
                //check if the drive has already been registered
                foreach (var d in drives) if (d.serial == serial) drv = d;

                if (drv != null)
                {
                    return;
                }
                else
                {
                    List<string[]> sources = new List<string[]>();
                    List<string[]> syncdetails = new List<string[]>();
                    drive.source = sources;
                    drive.syncType = SyncType.TwoWay;
                    drive.syncDetails = syncdetails;
                    drives.Add(drive);
                    FileDetailsMgmt.saveDrive(drives);
                    drives = FileDetailsMgmt.getRegisteredDrives();
                    UnloadDevice(letter + ":\\");
                    loadDevice(letter);

                }
            }
            catch (Exception e) { Helper.WriteLog(e); }
        }
        /// <summary>
        /// Load the usb devices that are already connected to the system
        /// </summary>
        private void LoadConnectedDevices()
        {
            try
            {
                
                DriveInfo[] driveInfos = DriveInfo.GetDrives();
                foreach (var driveInfo in driveInfos)
                {
                    //check if the drive is a usb drive
                    USB.USBDevice device = USB.FindDriveLetter(driveInfo.Name.Substring(0, 2));
                    if (device != null)
                    {
                        if (driveInfo.IsReady && driveInfo.TotalSize > 0) 
                        {
                            serial = DriveInformation.getSerial(driveInfo.Name); //USB.FindDriveLetter(driveInfo.Name.Substring(0, 2)).SerialNumber;
                            
                            //RegisterDevice(serial);
                            loadDevice(driveInfo.Name);
                        }
                    }
                }
                statuslabel.Text = "Ready";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
           
        }


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileDetailsMgmt.saveDrive(drives);
            e.Cancel = true;

            Hide();
            notifyIcon1.BalloonTipText = "You can open SmartSync from the here.";
            notifyIcon1.BalloonTipClicked += (s, a) => {Show(); };
            notifyIcon1.ShowBalloonTip(5000);
        }
        FolderBrowserDialog opfd = new FolderBrowserDialog();
        

        #region notification
        /// <summary>
        /// show notification when the device is removed from the system
        /// </summary>
        /// <param name="lt">The letter of the device</param>
        void NotifyRemoval(string lt)
        {
            try
            {
                if (SmartSync.Properties.Settings.Default.Removed == false) return;
                DriveInfo drive = new DriveInfo(lt);
                notifyIcon1.BalloonTipText = lt + ":\\" + " has been removed.";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(5000);
            }
            catch(Exception e) { Helper.WriteLog(e); }
        }

        

        void NotifyDetection(string lt)
        {
            if (SmartSync.Properties.Settings.Default.Devdetect == false) return;
            DriveInfo drive = new DriveInfo(lt);
            notifyIcon1.BalloonTipText = drive.VolumeLabel + " has been inserted.";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(5000);
            notifyIcon1.BalloonTipClicked += (s, a) => {Show(); };
        }
        void NotifyUnregistered(string lt)
        {
            DriveInfo drive = new DriveInfo(lt);
            notifyIcon1.BalloonTipText = drive.VolumeLabel + " has been inserted and is not registered.";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(5000);
            notifyIcon1.BalloonTipClicked += (s, a) => { Show(); };
        }
        void NotifyCompleted(string lt)
        {
            
            DriveInfo drive = new DriveInfo(lt);
            notifyIcon1.BalloonTipText = drive.VolumeLabel + " has been successfully synchronized.";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Visible = true;
            notifyIcon1.ShowBalloonTip(5000);
        }

#endregion
        

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // List<int> ints = listView1.SelectedIndices.t;
            ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
           // ListViewItem item = (ListViewItem)items[]; //listView1.SelectedItems[(Convert.ToInt32(listView1.SelectedIndices.ToString()))];
            foreach (var item1 in items)
            {
                ListViewItem item = (ListViewItem) item1;
              string m= item.SubItems[0].Text;
                string m1 = item.SubItems[1].Text;
                string message = "Source: " + m + "\n Destination: " + m1;
          
            toolTip1.Show(message,this,5000);
             
            }
           
        }

        private void editSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opfd.Description = "Select source folder";
                             
            ListViewItem item = listView1.SelectedItems[0];
            string source="";// = item.SubItems[0].Text;
            if (opfd.ShowDialog()==DialogResult.OK)
            {
                source = opfd.SelectedPath;
                item.SubItems.Insert(0, new ListViewItem.ListViewSubItem(item, source));
            }
          
            
           
        }

        private void editDestinationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opfd.Description = "Select destination folder";
            ListViewItem item = listView1.SelectedItems[0];
            string source=""; //= item.SubItems[1].Text;
            if (opfd.ShowDialog() == DialogResult.OK)
            {
                source = opfd.SelectedPath;
                item.SubItems.Insert(1, new ListViewItem.ListViewSubItem(item, source));
            }
           
           
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            item.Remove();
            
            Drive drive = drives.Find(i => (i.serial) == label3.Text);
            

            if (drive != null)
            {
                string[] s = new string[] { item.SubItems[0].Text, item.SubItems[1].Text };
                drives.Remove(drive);
                //drive.autosync = true;
                drive.source.Remove(s);
               
                drives.Add(drive);
                FileDetailsMgmt.saveDrive(drives);
                drives = FileDetailsMgmt.getRegisteredDrives();
                // MessageBox.Show("Operation Completed", "SmartSync", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listView1_MouseHover(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedListViewItemCollection items = listView1.SelectedItems;

                foreach (var item1 in items)
                {
                    ListViewItem item = (ListViewItem)item1;
                    string m = item.SubItems[0].Text;
                    string m1 = item.SubItems[1].Text;
                    string message = "Source: " + m + "\n Destination: " + m1;

                    toolTip1.Show(message, this, 5000);

                }
            }
            catch(Exception){}
        }

        private void label2_TextChanged(object sender, EventArgs e)
        {
            letter = label2.Text;
        }
        
        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            if (listView1.Size.Width > 318 && listView1.Items.Count > 0)
            {
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            else if (listView1.Size.Width <= 318)
            {
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           
            foreach (var VARIABLE in listView1.SelectedItems)
            {
                listView1.Items.Remove((ListViewItem)VARIABLE);
            }
            Drive drive = drives.Find(i => (i.serial) == label3.Text);

            if (drive != null)
            {
                drives.Remove(drive);
                drive.autosync = true;

                if (drive.source != null)
                    drive.source.Clear();
                else drive.source = new List<string[]>();
                foreach (ListViewItem txt in listView1.Items)
                {
                    
                    drive.source.Add(new string[] { txt.SubItems[0].Text, txt.SubItems[1].Text.Substring(1, txt.SubItems[1].Text.Length - 1) });

                }


                drives.Add(drive);
                FileDetailsMgmt.saveDrive(drives);
                drives = FileDetailsMgmt.getRegisteredDrives();
                // MessageBox.Show("Operation Completed", "SmartSync", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AboutBox1 frm = new AboutBox1();
            frm.ShowDialog();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            selectedDrive = drives.Find(i => (i.serial) == label3.Text);
            if (selectedDrive == null) return;
            selectedDrive = drives.Find(i => (i.serial) == label3.Text);
            
            TreeNode node = ((TreeView)sender).SelectedNode;
            if (node.Nodes.Count == 0)
            {
                string name = node.Name;
                getDirectories(name, node);
                node.Expand();
            }
        }

       

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        bool IsNotReadOnly(string drive)
        {
            try
            {
                File.Create(drive + "name" + ".tmp");
                
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return true;
            }
            catch
            {
                return false;
            }
           
            
            
        }
       
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {

            try
            {
                //check if the device is registered already and if not, register it
                string[] dirs = new string[2];
                dirs[0] = (string)e.Data.GetData(typeof(string));
                if (!Directory.Exists(dirs[0])) return;

                DriveInformation.sdrv_folder = (string)e.Data.GetData(typeof(string));

                if (selectedDrive != null) DriveInformation.sdrv = letter;
                else DriveInformation.sdrv = "";

                //load the dialog form to select the drive you want to synchronize and the folder you want to do that with
                driveBrowser browser = new driveBrowser(this);
                if (browser.ShowDialog().ToString() == "Cancel") return;

                //progressbar.Visible = true;

                //retrieve the values set from the dialog
                string value = DriveInformation.sdrv_folder;
                //check if the device has been registered else register it
                RegisterDevice(DriveInformation.sdrv_serial);
                Drive drive = drives.Find(i => (i.serial) == DriveInformation.sdrv_serial);
                if (drive == null) return;
                //progressbar.PerformStep();
                if (drive.source.Count > 0)
                {
                    //check if the folder has originally been synchronized
                    foreach (string[] s in drive.source)
                    {
                        if (s[0] == dirs[0])
                        {

                            dirs[1] = DriveInformation.sdrv_folder;

                            drives.Remove(drive);
                            drive.source.Remove(s);

                            if (drive.source != null)
                                drive.source.Add(dirs);
                            else
                            {
                                drive.source = new List<string[]>();
                                drive.source.Add(dirs);
                            }
                            drives.Add(drive);
                            FileDetailsMgmt.saveDrive(drives);
                            drives = FileDetailsMgmt.getRegisteredDrives();
                            statuslabel.Text = "Folder added successfully";


                        }
                    }
                }
                else
                {


                    dirs[1] =  DriveInformation.sdrv_folder;
                    drives.Remove(drive);
                    drive.autosync = true;
                    if (drive.source != null)
                        drive.source.Add(dirs);
                    else
                    {

                        drive.source = new List<string[]>();
                        drive.source.Add(dirs);
                    }
                    drives.Add(drive);
                    FileDetailsMgmt.saveDrive(drives);
                    drives = FileDetailsMgmt.getRegisteredDrives();
                    statuslabel.Text = "Folder added successfully";

                }
            }
            catch (Exception ex) { Helper.WriteLog(ex); }
            finally
            {
                //progressbar.Visible = false;
            }

        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
           // if (selectedDrive == null) return;
            TreeNode node = (TreeNode)e.Item;
            DoDragDrop(node.Name, DragDropEffects.Copy);
            
          
        }

        /// <summary>
        /// Change the synchronization settings of the selected drive
        /// </summary>
        /// <param name="syncType">The type of synchronization that is selected</param>
        private void updateSyncType(SyncType syncType)
        {
            try
            {
                if (selectedDrive == null) return;
                drives.Remove(selectedDrive);
                selectedDrive.syncType = syncType;
                drives.Add(selectedDrive);
                FileDetailsMgmt.saveDrive(drives);
                drives = FileDetailsMgmt.getRegisteredDrives();
                lblSyncType.Text = syncType.ToString();
            }
            catch (Exception e) { Helper.WriteLog(e); }
        }
        private void keepSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateSyncType(SyncType.PCToUSBKeepSource);
        }

        private void label3_TextChanged(object sender, EventArgs e)
        {
            selectedDrive = drives.Find(i => (i.serial) == label3.Text);
        }

        private void PCToUSBdeleteSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateSyncType(SyncType.PCToUSBDeleteSource);
        }

        private void twoWayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateSyncType(SyncType.TwoWay);
        }

        private void USBToPCkeepSourceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            updateSyncType(SyncType.USBToPCKeepSource);
        }

        private void USBToPCdeleteSourceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            updateSyncType(SyncType.USBToPCDeleteSource);
        }


        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        } 

    }
  }
