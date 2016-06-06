using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using USBLib;
using System.Runtime.InteropServices;
namespace SmartSync
{
    
    public partial class usbDevice : UserControl
    {
        bool synchronizing = false;
        List<string[]> syncdetails;
        bool dirSpace=true;
        public string name; public string letter; public string message; public string details;
        public string deviceType;
        TreeView treeview = new TreeView();
        private ListView lb=new ListView();
        private object threadlock = new object();
        ToolStrip ts = new ToolStrip();
        ToolStripLabel lblSyncType = new ToolStripLabel();
        List<Drive> drives = new List<Drive>();
        Label label2=new Label();
        Form frm; Drive drive = new Drive();
        Button regBtn = new Button();
        string serial;
        bool synchronized=false;
        Panel panel1 = new Panel(); Label label3 = new Label(); 
        public usbDevice(string letter,List<Drive> drives,string serial)
        {

            InitializeComponent();
            this.serial = serial;
            this.drives = drives;
            this.letter = letter;
            syncdetails = new List<string[]>();
           // this.usbserial = usbserial;
            this.Load += (s, a) =>
                             {
                                 this.ContextMenuStrip = contextMenuStrip1;
                frm = this.FindForm();
               
              lb = (ListView) frm.Controls.Find("listView1", true)[0];

              ts = (ToolStrip)frm.Controls.Find("toolStrip1", true)[0];
              lblSyncType = (ToolStripLabel)ts.Items.Find("lblSyncType", true)[0];
                panel1=(Panel)frm.Controls.Find("panel1",true)[0];
                label3 = (Label)frm.Controls.Find("label3", true)[0];
                label2 = (Label)frm.Controls.Find("label2", true)[0];
               // regBtn = (Button)frm.Controls.Find("btnRegister", true)[0];
                treeview = (TreeView)frm.Controls.Find("treeView1", true)[0];
                string lt = letter.Substring(0, 1);
                serial = DriveInformation.getSerial(lt + ":\\"); //USB.FindDriveLetter(lt + ":").SerialNumber;
                //Thread secondaryThread=new Thread(new ThreadStart(getDeviceData));
                //secondaryThread.Start();

                LoadDevice();
            };
        }


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CopyFile(string lpExistingFileName, string lpNewFileName, bool bFailIfExists);
       // [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int MoveFile([In, MarshalAs(UnmanagedType.LPTStr)] string lpExistingFileName, [In, MarshalAs(UnmanagedType.LPTStr)] string lpNewFileName);


        /// <summary>
        /// Unselect if the device is selected.
        /// </summary>
        /// <param name="device">The device that is to be unselected</param>
        public void deSelectDevice(usbDevice device)
        {
            device.BorderStyle=BorderStyle.None;
           // device.BackgroundImage = SmartSync.Properties.Resources.usbDevicebg;
        }



        /// <summary>
        /// Load the information of the drive to the parent form
        /// </summary>
        private void loadDeviceInfo()
        {
            try
            {
                DriveInfo drive = new DriveInfo(letter);


                label3.Text = serial;

                Drive drv = FileDetailsMgmt.thisDrive(serial, drives);

                if (drv == null)
                {
                    lb.Items.Clear();
                    printMessage("Unregistered device");

                    foreach (TreeNode node in treeview.Nodes)
                    {
                        foreach (TreeNode n in node.Nodes)
                        {
                            
                                n.ImageIndex = 0;

                                n.SelectedImageIndex = 0;
                            
                        }
                    }
                    pictureBox1.Image = Properties.Resources.usb_disconnected;
                }
                else
                {
                    this.drive = drv;
                    lb.Items.Clear();
                    lblSyncType.Text = drv.syncType.ToString();
                    //lb.Columns.Clear();
                   
                    int i=0;
                    if (drv.source.Count > 0)
                    {
                        foreach (string[] s in drv.source)
                        {
                            if (drv.syncDetails.Count > 0)
                            {
                                string[] s1 = new string[] { s[0], s[1], drv.syncDetails[i][0], drv.syncDetails[i][0] };
                                lb.Items.Add(new ListViewItem(s1));
                                lb.Items[0].ImageIndex = 1;
                            }
                            else {
                                string[] s1 = new string[] { s[0], s[1], "Not synchronized", "Not synchronized" };
                                lb.Items.Add(new ListViewItem(s1));
                                lb.Items[0].ImageIndex = 1;
                            }
                           // lb.Items.Add(new ListViewItem(s));
                        }
                    }



                    foreach (TreeNode node in treeview.Nodes)
                    {
                        foreach (TreeNode n in node.Nodes)
                        {
                            if (drv.source != null && drv.source.Count > 0)
                            {
                                foreach (var s in drv.source[0])
                                {
                                    if (s == n.Name)
                                    {

                                        n.ImageIndex = 1;

                                        n.SelectedImageIndex = 1;
                                        break;
                                    }
                                    else
                                    {
                                        n.ImageIndex = 0;

                                        n.SelectedImageIndex = 0;
                                    }
                                }
                            }
                            else
                            {
                                n.ImageIndex = 0;

                                n.SelectedImageIndex = 0;
                            }
                        }
                    }



                    if (drv.autosync && synchronized == false)
                    {
                        if (drv.source.Count > 0)
                        {
                            printMessage("Synchronizing...");
                            //backgroundWorker1.RunWorkerAsync();
                            executeSyn();
                            progressBar1.Color = Color.DeepSkyBlue;
                            //Thread thread = new Thread(new ThreadStart(executeSync));
                            //thread.Start();
                        }
                    }

                    printMessage("Device Ready");

                }
            }
            catch (Exception e)
            {
                Helper.WriteLog(e);
            }

            

        }


        private void LoadDevice()
        {
            try
            {
                DriveInfo drive = new DriveInfo(letter);
                this.name = drive.VolumeLabel;
                lblname.Text = name + " " + letter;
                //printMessage("Checking drive.....");
                decimal totalSize = Math.Round(Convert.ToDecimal(drive.TotalSize) / 1000000000, 1);
                decimal availableSize = Math.Round(Convert.ToDecimal(drive.TotalFreeSpace) / 1000000000, 1);
                lblSize.Text = availableSize.ToString() + "GB" + "/" + totalSize.ToString() + "GB";
                progressBar1.Value = Convert.ToInt32(((totalSize - availableSize) / totalSize) * 100);
                if (availableSize < (Convert.ToDecimal(0.1) * totalSize))
                {
                    progressBar1.Color = Color.Red;
                }

                Drive drv = FileDetailsMgmt.thisDrive(serial, drives);

                if (drv == null)
                {


                    printMessage("Unregistered device");

                    pictureBox1.Image = Properties.Resources.usb_disconnected;
                }
                else
                {
                    lblSyncType.Text = drv.syncType.ToString();
                    this.drive = drv;
                    printMessage("Device Ready");
                }

                if (drv.autosync && synchronized == false)
                {
                    if (drv.source.Count > 0)
                    {
                        if (SmartSync.Properties.Settings.Default.autosync == false) return;
                        executeSyn();
                        progressBar1.Color = Color.DeepSkyBlue;

                    }
                }
            }
            catch(Exception e) { Helper.WriteLog(e); }

        }


        public void printMessage(string message)
        {
            lblmessage.Text = message;
            Application.DoEvents();
        }

        /// <summary>
        /// Gets the number of files in a directory
        /// </summary>
        /// <param name="dir">Name of the directory</param>
        /// <returns></returns>
        public int directoryFiles(string dir)
        {
            int fileNumber=0;
            //DirectoryInfo dirInfo = new DirectoryInfo(dir);
            //FileInfo[] files = dirInfo.GetFiles();
            //fileNumber = files.Length;
            //DirectoryInfo[] dirs = dirInfo.GetDirectories();
            //foreach (DirectoryInfo dr in dirs)
            //{
            //    fileNumber += directoryFiles(dr.FullName);
            //}
            return fileNumber;
        }


       /// <summary>
       /// Checks if the file in a drive is enough for the synchronisation
       /// </summary>
       /// <param name="drive">Letter of the drive</param>
       /// <param name="source">List of the source folders</param>
       /// <param name="destDir">Destination folder</param>
       /// <returns></returns>
        private void checkDriveSize(object o)
        {
            try
            {
                lock (threadlock)
                {

                    object[] os = o as object[];
                    string drive = os[0] as string;
                    List<string[]> source = os[1] as List<string[]>;
                    string destDir = os[2] as string;
                    //string drive, List<string[]> source, string destDir
                    //get the amount of data that you will be transferring


                    if (this.drive.syncType == SyncType.PCToUSBKeepSource || this.drive.syncType == SyncType.PCToUSBDeleteSource)
                    {
                        long sourceSize = 0;
                        foreach (string[] src in source)
                        {
                            sourceSize += directorySize(src[0]);
                        }
                        DriveInfo driveinfo = new DriveInfo(drive);
                        long driveSize = driveinfo.AvailableFreeSpace;

                        if (sourceSize < driveSize)
                        {
                            dirSpace = true;
                        }
                        else
                        {
                            dirSpace = false;
                        }

                    }

                    else if (this.drive.syncType == SyncType.TwoWay)
                    {
                        long sourceSize = 0;
                        long dirSizee = 0;
                        foreach (string[] src in source)
                        {
                            sourceSize += directorySize(src[0]);
                            dirSizee += directorySize(src[1]);
                        }

                        long difference = sourceSize - dirSizee;//directorySize(destDir);
                        DriveInfo driveinfo = new DriveInfo(drive);
                        long driveSize = driveinfo.AvailableFreeSpace;

                        if (difference < driveSize)
                        {
                            dirSpace = true;
                        }
                        else
                        {
                            dirSpace = false;
                        }
                    }
                    else if (this.drive.syncType == SyncType.USBToPCDeleteSource || this.drive.syncType == SyncType.USBToPCKeepSource)
                    {

                        long sourceSize = 0;
                        foreach (string[] src in source)
                        {
                            sourceSize += directorySize(src[1]);
                        }

                        long difference = sourceSize - directorySize(destDir);

                        DriveInfo driveinfo = new DriveInfo(drive);
                        long driveSize = driveinfo.AvailableFreeSpace;

                        if (difference < driveSize)
                        {
                            dirSpace = true;
                        }
                        else
                        {
                            dirSpace = false;
                        }
                    }
                }
            }
            catch (Exception e) { Helper.WriteLog(e); }
        }

        /// <summary>
        /// Get the size of the files in a directory
        /// </summary>
        /// <param name="dir">Path of the directory</param>
        /// <returns></returns>
        public long directorySize(string dir)
        {
            try
            {
                long size = 0;
                if (Directory.Exists(dir))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    FileInfo[] files = dirInfo.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        size += file.Length;

                    }
                    //using recursive programming we then iterate through the subdirectories of the directory that you want to get the size
                    DirectoryInfo[] dirs = dirInfo.GetDirectories();
                    foreach (DirectoryInfo dire in dirs)
                    {
                        size += directorySize(dire.FullName);
                    }

                }
                return size;
            }
            catch (Exception e) { Helper.WriteLog(e); return 0; }
        }


        /// <summary>
        /// Checks if a directory exits or tries to create it, returns true if successful or else false
        /// </summary>
        /// <param name="o">Path of the folder</param>
        /// <returns></returns>
        private void DirExists(object o)
        {
           
                string[] path = o as string[];
                if (!Directory.Exists(path[0]))
                {
                    Directory.CreateDirectory(path[0]);
                    //return true;
                }
            
            
        }

        /// <summary>
        /// Synchronizes the files in the source and the destination 
        /// </summary>
        /// <param name="sourcePath">Path of the source</param>
        /// <param name="destinationPath">path of the destination</param>
        public void Synchronize(string sourcePath, string destinationPath,SyncType syncType)
        {
            //count the number of files in the directories
            int nfiles = 0;
            //foreach (string[] directory in drive.source)
            //{
            //    nfiles += directoryFiles(directory[0]);
            //}

            if (syncType == SyncType.USBToPCDeleteSource)
            {
                MoveFolderAsync(destinationPath, sourcePath);
            }
            else if (syncType == SyncType.USBToPCKeepSource)
            {
                CopyFolderAsync(destinationPath, sourcePath);
            }
            else if (syncType == SyncType.PCToUSBDeleteSource)
            {
                MoveFolderAsync(sourcePath, destinationPath);
            }
            else if (syncType == SyncType.PCToUSBKeepSource)
            {
                CopyFolderAsync(sourcePath, destinationPath);
            }
            else if (syncType == SyncType.TwoWay)
            {
                CopyFolderAsync(sourcePath, destinationPath);
            }
            
            string[] dirs = Directory.GetDirectories(sourcePath);
            foreach (String dir in dirs)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                Synchronize(dir, Path.Combine(destinationPath, dirInfo.Name),syncType);
                string value = Path.Combine(destinationPath, dirInfo.Name);
            }


        }

        private void threadCopy(object o)
        {
            try
            {
                string[] files = o as string[];
                CopyFile(files[0], files[1], true);
            }
            catch { }
        }

        private void threadMove(object o)
        {
            try
            {
                string[] files = o as string[];
                MoveFile(files[0], files[1]);
            }
            catch { }
        }

        /// <summary>
        /// Copies files between folders asynchronously
        /// </summary>
        /// <param name="source">The source folder</param>
        /// <param name="destination">The destination folder</param>
        public void CopyFolderAsync(string source, string destination)
        {
            try
            {
                Thread td = new Thread(DirExists);
                td.Start(new[] { destination });

                string[] srcFiles = Directory.GetFiles(source);
                progressBar1.Value = 0;
                progressBar1.Color = Color.Green;
                int count = srcFiles.Length;
                string[] s = new string[] { count.ToString(), DateTime.Now.Date.ToShortDateString() };
                syncdetails.Add(s);
                int step = 1;
                if (count != 0)
                {
                    step = 100 / count;
                }
                else
                    count = 1;

                foreach (string filename in srcFiles)
                {
                    
                    progressBar1.Value += step;
                    FileInfo sourceFile = new FileInfo(filename);
                    FileInfo destinationFile = new FileInfo(destination + "\\" + sourceFile.Name);

                    if (destinationFile.Exists)
                    {
                        if (sourceFile.Length > destinationFile.Length)
                        {
                            if (sourceFile.LastWriteTime.CompareTo(destinationFile.LastWriteTime) == 1)
                            {
                                printMessage("Copying " + sourceFile.FullName + "...");
                                Thread t = new Thread(threadCopy);
                                t.Start(new[] { sourceFile.FullName, destinationFile.FullName });
                                //updatedFiles.Add(new string[] {sourceFile.Name,sourceFile.FullName,sourceFile.Length.ToString() });
                            }
                            else if (sourceFile.LastWriteTime.CompareTo(destinationFile.LastWriteTime) == -1 && this.drive.syncType == SyncType.TwoWay)
                            {
                                printMessage("Copying " + destinationFile.FullName + "...");
                                Thread t = new Thread(threadCopy);
                                t.Start(new[] { destinationFile.FullName, sourceFile.FullName });
                                //updatedFiles.Add(new string[] {destinationFile.Name,destinationFile.FullName,destinationFile.Length.ToString() });
                            }

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Thread t = new Thread(threadCopy);
                        t.Start(new[] { sourceFile.FullName, destinationFile.FullName });
                        //updatedFiles.Add(new string[] { sourceFile.Name, sourceFile.FullName, sourceFile.Length.ToString() });

                    }
                }

                if (this.drive.syncType == SyncType.TwoWay)
                {
                    foreach (string filename in Directory.GetFiles(destination))
                    {

                        progressBar1.Value += step;
                        FileInfo sourceFile = new FileInfo(filename);
                        FileInfo destinationFile = new FileInfo(source + "\\" + sourceFile.Name);

                        if (destinationFile.Exists)
                        {
                            if (sourceFile.Length > destinationFile.Length)
                            {
                                if (sourceFile.LastWriteTime.CompareTo(destinationFile.LastWriteTime) == 1)
                                {
                                    printMessage("Copying " + sourceFile.FullName + "...");
                                    Thread t = new Thread(threadCopy);
                                    t.Start(new[] { sourceFile.FullName, destinationFile.FullName });
                                    //          updatedFiles.Add(new string[] { sourceFile.Name, sourceFile.FullName, sourceFile.Length.ToString() });
                                }
                                else if (sourceFile.LastWriteTime.CompareTo(destinationFile.LastWriteTime) == -1 && this.drive.syncType == SyncType.TwoWay)
                                {
                                    printMessage("Copying " + destinationFile.FullName + "...");
                                    Thread t = new Thread(threadCopy);
                                    t.Start(new[] { destinationFile.FullName, sourceFile.FullName });
                                    //        updatedFiles.Add(new string[] { destinationFile.Name, destinationFile.FullName, destinationFile.Length.ToString() });
                                }

                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            printMessage("Copying " + sourceFile.FullName + "...");
                            Thread t = new Thread(threadCopy);
                            t.Start(new[] { sourceFile.FullName, destinationFile.FullName });
                            //updatedFiles.Add(new string[] { sourceFile.Name, sourceFile.FullName, sourceFile.Length.ToString() });

                        }
                    }
                }
            }
            catch (Exception e) { Helper.WriteLog(e); }
        }


        /// <summary>
        /// Moves files between folders asynchronously
        /// </summary>
        /// <param name="source">The source folder</param>
        /// <param name="destination">The destination folder</param>
        public void MoveFolderAsync(string source, string destination)
        {
            try
            {
                Thread td = new Thread(DirExists);
                td.Start(new[] { destination });

                string[] srcFiles = Directory.GetFiles(source);
                progressBar1.Value = 0;
                progressBar1.Color = Color.Green;
                int count = srcFiles.Length;
                string[] s = new string[] { count.ToString(), DateTime.Now.Date.ToShortDateString() };
                syncdetails.Add(s);
                int step = 1;
                if (count != 0)
                {
                    step = 100 / count;
                }
                else
                    count = 1;

                foreach (string filename in srcFiles)
                {
                    progressBar1.Value += step;
                    FileInfo sourceFile = new FileInfo(filename);
                    FileInfo destinationFile = new FileInfo(destination + "\\" + sourceFile.Name);

                    if (destinationFile.Exists)
                    {
                        if (sourceFile.Length > destinationFile.Length)
                        {
                            printMessage("Checking " + sourceFile.FullName + "...");
                            if (sourceFile.LastWriteTime.CompareTo(destinationFile.LastWriteTime) == 1)
                            {
                                printMessage("Moving " + sourceFile.FullName + "...");
                                Thread t = new Thread(threadMove);
                                t.Start(new[] { sourceFile.FullName, destinationFile.FullName });
                            }
                            else if (sourceFile.LastWriteTime.CompareTo(destinationFile.LastWriteTime) == -1 && this.drive.syncType == SyncType.TwoWay)
                            {
                                printMessage("Moving " + destinationFile.FullName + "...");
                                Thread t = new Thread(threadMove);
                                t.Start(new[] { destinationFile.FullName, sourceFile.FullName });
                            }
                            //using (FileStream sourcestream = File.Open(filename, FileMode.Open,FileAccess.Read))
                            //{
                            //    using (FileStream destinationstream = File.Create(destination + sourceFile.Name)
                            //    {
                            //        printMessage("Appending " + filename + "...");
                            //        await sourcestream.CopyToAsync(destinationstream);
                            //    }
                            //}
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        printMessage("Moving " + sourceFile.FullName + "...");
                        Thread t = new Thread(threadMove);
                        t.Start(new[] { sourceFile.FullName, destinationFile.FullName });
                    }
                }
            }
            catch (Exception e) { Helper.WriteLog(e); }
        }

      
       


        private void usbDevice_Click(object sender, EventArgs e)
        {
            if (synchronizing == true) return;
            loadDeviceInfo();
            //List<NonSyncList> list = FileDetailsMgmt.getNonSyncList();
            //NonSyncList itm = list.Find(i => (i.serial) == serial);
            Drive drv = FileDetailsMgmt.thisDrive(serial, drives);
            if (drv !=null)
            {
              
                ts.Visible = true;
                pictureBox1.Image = Properties.Resources.usb_connected;
            }
            else
            {
                
                ts.Visible = true;
                pictureBox1.Image = Properties.Resources.usb_disconnected;
            }
            List<usbDevice> devices = new List<usbDevice>();
            foreach (Control ctrl in panel1.Controls)
            {
                devices.Add((usbDevice)ctrl);

            }

            foreach (usbDevice dv in devices)
            {
                if (dv.Name != this.Name) deSelectDevice(dv);
            }
            label2.Text = this.Name.Substring(0, 1); //this.Name.Length - 1);
            this.BorderStyle = BorderStyle.FixedSingle;
            DriveInformation.sdrv_serial = serial;
            this.FindForm().Controls.Find("label2", true)[0].Text = this.Name.Substring(0, 1);

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            executeSyn();
        }

        /// <summary>
        /// This is the method that does that synchronises the folders in the device
        /// 
        /// </summary>
        public void executeSyn()
        {
           
            try
            {
                synchronizing = true;
                    //updatedFiles.Clear();
                    //drive.syncDetails.Clear();
                    int number = drive.source.Count;
                    int step;
                    if (number != 0)
                    {
                        step = 100 / number;
                    }
                    else
                    {
                        step = 1;
                    }
                    progressBar1.Value = 0;
                    foreach (string[] directory in drive.source)
                    {

                        printMessage("Synchronizing " + directory[0] + " ...");
                        progressBar1.Value += step;
                        //check if the folder synchronises by comparing their sizes. If the sizes are the same then it skips the folder and goes on to another folder.
                        Synchronize(directory[0], letter.Substring(0, 1) +":\\"+ directory[1], drive.syncType);
                    }
                    progressBar1.Value = 0;
                    
                    synchronized = true;
                    
                    printMessage("Synchronization complete.");
                    drive.syncDetails = syncdetails;
                    Drive d = FileDetailsMgmt.thisDrive(serial, drives);

                    drives.Remove(d);
                    drives.Add(drive);
                    FileDetailsMgmt.saveDrive(drives);
                    

                }

            
            catch (IOException e)
            {
                
                printMessage("An error occured.");
                Helper.WriteLog(e);
            }
            

            finally
                {
                    LoadDevice();
                    loadDeviceInfo();
                    synchronizing = false;
                }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            executeSyn();
           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            printMessage("Sync Completed...");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 5;
            }
            else if (progressBar1.Value == 100)
                progressBar1.Value = 0;
            printMessage("Synchronzing...");
        }

        private void synchronizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            executeSyn();               
        }

       


    }
}
