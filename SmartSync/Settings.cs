using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace SmartSync
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            this.Load += (s, a) =>
            {
                //Load the application settings
                startup();
                
                chkDevDetected.Checked = SmartSync.Properties.Settings.Default.Devdetect;
                chkDeviceRemoved.Checked = SmartSync.Properties.Settings.Default.Removed;
                chkAutoSync.Checked = SmartSync.Properties.Settings.Default.autosync;
                chkpromptnewdevice.Checked = SmartSync.Properties.Settings.Default.notifyoniteminsert;
                chkUpdate.Checked = SmartSync.Properties.Settings.Default.autoupdate;
               
                //chkUnregistered.Checked = SmartSync.Properties.Settings.Default.Unregistered;
                chksyncComplete.Checked = SmartSync.Properties.Settings.Default.synccomplete;
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //save the setting of the applications
                saveSettings();
                MessageBox.Show("Your settings were save successfully", "SmartSync", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("There was error saving the settings.", "Try again", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                throw;
            }
        }

        /// <summary>
        /// This method saves the program settings into the setting file
        /// </summary>
        private void saveSettings()
        {
           
            SmartSync.Properties.Settings.Default.Devdetect = chkDevDetected.Checked;
            SmartSync.Properties.Settings.Default.Removed = chkDeviceRemoved.Checked;
            //SmartSync.Properties.Settings.Default.Unregistered = chkUnregistered.Checked;
            SmartSync.Properties.Settings.Default.autosync = chkAutoSync.Checked;
            SmartSync.Properties.Settings.Default.autoupdate = chkUpdate.Checked;
            SmartSync.Properties.Settings.Default.notifyoniteminsert = chkpromptnewdevice.Checked;
            SmartSync.Properties.Settings.Default.synccomplete = chksyncComplete.Checked;
            if (chkstartup.Checked)
                setStartup();
            else
            {
                removeStartup();
            }
            SmartSync.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Set the application to start up with the computer by adding it to the appropriate registry
        /// </summary>
        private void setStartup()
        {
            try
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rkApp.SetValue("SmartSync", Application.ExecutablePath.ToString());
            }
            catch (Exception)
            {

                MessageBox.Show("Operation failed");
            }


        }
        /// <summary>
        /// Remove the application from the start up
        /// </summary>
        private void removeStartup()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkApp.DeleteValue("SmartSync", false);
        }
        /// <summary>
        /// Check the startup status of the application
        /// </summary>
        void startup()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkApp.GetValue("SmartSync") == null)
            {

                // The value doesn't exist, the application is not set to run at startup

                chkstartup.Checked = false;

            }

            else
            {

                // The value exists, the application is set to run at startup

                chkstartup.Checked = true;

            }
        }

        private void shieldButton1_Click(object sender, EventArgs e)
        {
            su_pdate update = new su_pdate();
            update.ShowDialog();
        }
    }
}
