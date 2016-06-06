namespace SmartSync
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkstartup = new System.Windows.Forms.CheckBox();
            this.chkDeviceRemoved = new System.Windows.Forms.CheckBox();
            this.chksyncComplete = new System.Windows.Forms.CheckBox();
            this.chkDevDetected = new System.Windows.Forms.CheckBox();
            this.chkUpdate = new System.Windows.Forms.CheckBox();
            this.chkpromptnewdevice = new System.Windows.Forms.CheckBox();
            this.chkAutoSync = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(288, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 24);
            this.button1.TabIndex = 9;
            this.button1.Text = "Save settings";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Candara", 13F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(384, 27);
            this.label1.TabIndex = 10;
            this.label1.Text = "SmartSync Settings";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkstartup
            // 
            this.chkstartup.AutoSize = true;
            this.chkstartup.Location = new System.Drawing.Point(14, 105);
            this.chkstartup.Name = "chkstartup";
            this.chkstartup.Size = new System.Drawing.Size(206, 19);
            this.chkstartup.TabIndex = 15;
            this.chkstartup.Text = "Start SmartSync on system startup";
            this.chkstartup.UseVisualStyleBackColor = true;
            // 
            // chkDeviceRemoved
            // 
            this.chkDeviceRemoved.AutoSize = true;
            this.chkDeviceRemoved.Location = new System.Drawing.Point(14, 83);
            this.chkDeviceRemoved.Name = "chkDeviceRemoved";
            this.chkDeviceRemoved.Size = new System.Drawing.Size(163, 19);
            this.chkDeviceRemoved.TabIndex = 14;
            this.chkDeviceRemoved.Text = "Notify on device removed";
            this.chkDeviceRemoved.UseVisualStyleBackColor = true;
            // 
            // chksyncComplete
            // 
            this.chksyncComplete.AutoSize = true;
            this.chksyncComplete.Location = new System.Drawing.Point(14, 63);
            this.chksyncComplete.Name = "chksyncComplete";
            this.chksyncComplete.Size = new System.Drawing.Size(223, 19);
            this.chksyncComplete.TabIndex = 13;
            this.chksyncComplete.Text = "Notify on synchronization completed";
            this.chksyncComplete.UseVisualStyleBackColor = true;
            // 
            // chkDevDetected
            // 
            this.chkDevDetected.AutoSize = true;
            this.chkDevDetected.Location = new System.Drawing.Point(14, 165);
            this.chkDevDetected.Name = "chkDevDetected";
            this.chkDevDetected.Size = new System.Drawing.Size(248, 19);
            this.chkDevDetected.TabIndex = 11;
            this.chkDevDetected.Text = "Show notification when device is detected";
            this.chkDevDetected.UseVisualStyleBackColor = true;
            // 
            // chkUpdate
            // 
            this.chkUpdate.AutoSize = true;
            this.chkUpdate.Location = new System.Drawing.Point(14, 124);
            this.chkUpdate.Name = "chkUpdate";
            this.chkUpdate.Size = new System.Drawing.Size(174, 19);
            this.chkUpdate.TabIndex = 16;
            this.chkUpdate.Text = "Check update automatically";
            this.chkUpdate.UseVisualStyleBackColor = true;
            // 
            // chkpromptnewdevice
            // 
            this.chkpromptnewdevice.AutoSize = true;
            this.chkpromptnewdevice.Location = new System.Drawing.Point(14, 144);
            this.chkpromptnewdevice.Name = "chkpromptnewdevice";
            this.chkpromptnewdevice.Size = new System.Drawing.Size(309, 19);
            this.chkpromptnewdevice.TabIndex = 19;
            this.chkpromptnewdevice.Text = "Prompt me when a new usb storage device is inserted";
            this.chkpromptnewdevice.UseVisualStyleBackColor = true;
            // 
            // chkAutoSync
            // 
            this.chkAutoSync.AutoSize = true;
            this.chkAutoSync.Location = new System.Drawing.Point(14, 43);
            this.chkAutoSync.Name = "chkAutoSync";
            this.chkAutoSync.Size = new System.Drawing.Size(229, 19);
            this.chkAutoSync.TabIndex = 20;
            this.chkAutoSync.Text = "Synchronize usb devices automatically";
            this.chkAutoSync.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(205, 191);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 23);
            this.button2.TabIndex = 21;
            this.button2.Text = "Update now";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.shieldButton1_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 224);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.chkAutoSync);
            this.Controls.Add(this.chkpromptnewdevice);
            this.Controls.Add(this.chkUpdate);
            this.Controls.Add(this.chkstartup);
            this.Controls.Add(this.chkDeviceRemoved);
            this.Controls.Add(this.chksyncComplete);
            this.Controls.Add(this.chkDevDetected);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkstartup;
        private System.Windows.Forms.CheckBox chkDeviceRemoved;
        private System.Windows.Forms.CheckBox chksyncComplete;
        private System.Windows.Forms.CheckBox chkDevDetected;
        private System.Windows.Forms.CheckBox chkUpdate;
        
        private System.Windows.Forms.CheckBox chkpromptnewdevice;
        private System.Windows.Forms.CheckBox chkAutoSync;
        private System.Windows.Forms.Button button2;
    }
}