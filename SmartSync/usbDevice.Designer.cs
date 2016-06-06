namespace SmartSync
{
    partial class usbDevice
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblname = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblmessage = new System.Windows.Forms.Label();
            this.progressBar1 = new MBGlassStyleProgressBar.MBProgressBar();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.synchronizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblname
            // 
            this.lblname.AutoSize = true;
            this.lblname.BackColor = System.Drawing.Color.Transparent;
            this.lblname.Font = new System.Drawing.Font("Candara", 9F);
            this.lblname.Location = new System.Drawing.Point(35, 0);
            this.lblname.Name = "lblname";
            this.lblname.Size = new System.Drawing.Size(83, 14);
            this.lblname.TabIndex = 2;
            this.lblname.Text = "USB Device E:\\\\";
            this.lblname.Click += new System.EventHandler(this.usbDevice_Click);
           
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.BackColor = System.Drawing.Color.Transparent;
            this.lblSize.Font = new System.Drawing.Font("Candara", 9F);
            this.lblSize.Location = new System.Drawing.Point(163, 1);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(50, 14);
            this.lblSize.TabIndex = 3;
            this.lblSize.Text = "3GB/4GB";
            this.lblSize.Click += new System.EventHandler(this.usbDevice_Click);
           
            // 
            // lblmessage
            // 
            this.lblmessage.AutoSize = true;
            this.lblmessage.BackColor = System.Drawing.Color.Transparent;
            this.lblmessage.Font = new System.Drawing.Font("Candara", 9F);
            this.lblmessage.Location = new System.Drawing.Point(35, 30);
            this.lblmessage.Name = "lblmessage";
            this.lblmessage.Size = new System.Drawing.Size(119, 14);
            this.lblmessage.TabIndex = 4;
            this.lblmessage.Text = "Unregistered Device...";
            this.lblmessage.Click += new System.EventHandler(this.usbDevice_Click);
            
            // 
            // progressBar1
            // 
            this.progressBar1.Animate = true;
            this.progressBar1.BackColor = System.Drawing.Color.Transparent;
            this.progressBar1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(201)))));
            this.progressBar1.Color = System.Drawing.Color.DeepSkyBlue;
            this.progressBar1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.progressBar1.HighlightColor = System.Drawing.Color.White;
            this.progressBar1.Location = new System.Drawing.Point(30, 15);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(217, 15);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Value = 50;
            this.progressBar1.Click += new System.EventHandler(this.usbDevice_Click);
            
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Candara", 10F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.synchronizeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 26);
            // 
            // synchronizeToolStripMenuItem
            // 
            this.synchronizeToolStripMenuItem.Name = "synchronizeToolStripMenuItem";
            this.synchronizeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.synchronizeToolStripMenuItem.Text = "Synchronize";
            this.synchronizeToolStripMenuItem.Click += new System.EventHandler(this.synchronizeToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::SmartSync.Properties.Resources.usb_connected;
            this.pictureBox1.Location = new System.Drawing.Point(2, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(26, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.usbDevice_Click);
            
            // 
            // usbDevice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.lblmessage);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblname);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(255, 49);
            this.Name = "usbDevice";
            this.Size = new System.Drawing.Size(252, 49);
            this.Click += new System.EventHandler(this.usbDevice_Click);
            
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblname;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblmessage;
        private MBGlassStyleProgressBar.MBProgressBar progressBar1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem synchronizeToolStripMenuItem;
    }
}
