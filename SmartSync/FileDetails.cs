using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Reflection;
using System.Runtime.InteropServices;
namespace SmartSync
{
    public enum SyncType { TwoWay,PCToUSBDeleteSource,PCToUSBKeepSource,USBToPCDeleteSource,USBToPCKeepSource}
   

    [Serializable]
    public class Drive
    {
        public string serial { set; get; }
        //public string destination { set; get; }
        public List<string[]> source { get; set; }
        public bool autosync { get; set; }
        public SyncType syncType { set; get; }
        public List<string[]> syncDetails { set; get; }

    }
    [Serializable]
    public class NonSyncList
    {
        public string serial { set; get; }
    }


    /// <summary>
    /// This class manages the details of the drives in the system. It helping in saving, processing and reading the details of drives in the system
    /// </summary>
    public static class FileDetailsMgmt
    {
        /// <summary>
        /// The location of the file where the information the drives registered for the system is stored
        /// </summary>
        public static string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SmartSync/data.ogb");

        /// <summary>
        /// Saves list of drives by serializing them
        /// </summary>
        /// <param name="details">List of the drives to be serialized</param>
        public static void saveDrive(List<Drive> details)
        {
            try
            {
                using (Stream fstream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    BinaryFormatter binformat = new BinaryFormatter();
                    binformat.Serialize(fstream, details);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Get the list of the usb devices that is registered in the application
        /// </summary>
        /// <returns>A list of all the registered devices</returns>
        public static List<Drive> getRegisteredDrives()
        {
            List<Drive> details = new List<Drive>();
            try
            {
                if (File.Exists(filename))
                {

                    using (Stream fstream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        BinaryFormatter binformat = new BinaryFormatter();
                        details = (List<Drive>)binformat.Deserialize(fstream);
                    }
                    return details;
                }
                return details;
            }
            catch (Exception)
            {
                return details;
                throw;
            }


        }

        /// <summary>
        /// Select a particular usb drive from a list of usb drives using the serial number of the usb device
        /// </summary>
        /// <param name="serial">The serial number of the drive</param>
        /// <param name="drives">The list of the drives to search</param>
        /// <returns>The drive</returns>
        public static Drive thisDrive(string serial, List<Drive> drives)
        {
            

            Drive drive = drives.Find(i => (i.serial) == serial);
            
            return drive;
        }


        public static void AddToNonSyncList(List<NonSyncList> serials)
        {
            string name = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SmartSync/nui.ogb");
            try
            {
                using (Stream fstream = new FileStream(name, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    BinaryFormatter binformat = new BinaryFormatter();
                    binformat.Serialize(fstream, serials);
                }
            }
            catch { }
        }

        public static List<NonSyncList> getNonSyncList()
        {
            string name = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"SmartSync/nui.ogb");
            List<NonSyncList> list = new List<NonSyncList>();
            try
            {
                if (File.Exists(filename))
                {

                    using (Stream fstream = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        BinaryFormatter binformat = new BinaryFormatter();
                        list = (List<NonSyncList>)binformat.Deserialize(fstream);
                    }
                    return list;
                }
                return list;
            }
            catch (Exception)
            {
                return list;
                throw;
            }

        }


    }


    /// <summary>
    /// This class is used to get information about a drive.
    /// </summary>
    public static class DriveInformation
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern bool GetVolumeInformation(string Volume,
            StringBuilder VolumeName, uint VolumeNameSize,
            out uint SerialNumber, out uint SerialNumberLength, out uint flags,
            StringBuilder fs, uint fs_size);

        /// <summary>
        /// Retrieve the serial number of the drive
        /// </summary>
        /// <param name="drive">The name of the drive that its serial number is to be retrieved</param>
        /// <returns>The serial number of the drive</returns>
        public static string getSerial(string drive)
        {
            uint serialNum, serialNumLength, flags;
            StringBuilder volumename = new StringBuilder(256);
            StringBuilder fstype = new StringBuilder(256);
            bool ok = false;

            ok = GetVolumeInformation(drive, volumename, (uint)volumename.Capacity - 1, out serialNum,
                                       out serialNumLength, out flags, fstype, (uint)fstype.Capacity - 1);
            if (ok)
                return serialNum.ToString();
            else
                return "";


        }
        public static string sdrv = "";
        public static string sdrv_folder = "";
        public static string sdrv_serial = "";
        
    }


    
}