namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Update
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Update));
            this.pnlSesion = new System.Windows.Forms.Panel();
            this.lblRequireUpdate = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.pbMain = new System.Windows.Forms.PictureBox();
            this.pnlSesion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSesion
            // 
            this.pnlSesion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSesion.AutoSize = true;
            this.pnlSesion.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlSesion.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlSesion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSesion.Controls.Add(this.lblRequireUpdate);
            this.pnlSesion.Controls.Add(this.btnDownload);
            this.pnlSesion.Controls.Add(this.pbMain);
            this.pnlSesion.Location = new System.Drawing.Point(451, 249);
            this.pnlSesion.Name = "pnlSesion";
            this.pnlSesion.Size = new System.Drawing.Size(399, 152);
            this.pnlSesion.TabIndex = 11;
            // 
            // lblRequireUpdate
            // 
            this.lblRequireUpdate.AutoSize = true;
            this.lblRequireUpdate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequireUpdate.Location = new System.Drawing.Point(47, 94);
            this.lblRequireUpdate.Name = "lblRequireUpdate";
            this.lblRequireUpdate.Size = new System.Drawing.Size(109, 13);
            this.lblRequireUpdate.TabIndex = 0;
            this.lblRequireUpdate.Text = "1001_RequireUpdate";
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.Transparent;
            this.btnDownload.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.Location = new System.Drawing.Point(120, 118);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(151, 27);
            this.btnDownload.TabIndex = 3;
            this.btnDownload.Text = "1001_btnDownload";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // pbMain
            // 
            this.pbMain.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pbMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbMain.BackgroundImage")));
            this.pbMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMain.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pbMain.ErrorImage")));
            this.pbMain.Image = ((System.Drawing.Image)(resources.GetObject("pbMain.Image")));
            this.pbMain.ImageLocation = "";
            this.pbMain.Location = new System.Drawing.Point(3, 3);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(389, 85);
            this.pbMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMain.TabIndex = 10;
            this.pbMain.TabStop = false;
            // 
            // Update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(884, 482);
            this.Controls.Add(this.pnlSesion);
            this.Name = "Update";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmLogIn";
            this.pnlSesion.ResumeLayout(false);
            this.pnlSesion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSesion;
        private System.Windows.Forms.Label lblRequireUpdate;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.PictureBox pbMain;
    }
}