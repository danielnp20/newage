namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ControlForm
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
        protected virtual void InitializeComponent()
        {
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tcControl = new DevExpress.XtraTab.XtraTabControl();
            this.TbLyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcControl)).BeginInit();
            this.SuspendLayout();
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.197802F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 97.8022F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.TbLyPanel.Controls.Add(this.tcControl, 1, 0);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 2;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.2F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.8F));
            this.TbLyPanel.Size = new System.Drawing.Size(973, 500);
            this.TbLyPanel.TabIndex = 0;
            // 
            // tcControl
            // 
            this.tcControl.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.tcControl.Appearance.Options.UseBackColor = true;
            this.tcControl.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tcControl.AppearancePage.Header.Options.UseFont = true;
            this.tcControl.AppearancePage.PageClient.BackColor = System.Drawing.Color.Transparent;
            this.tcControl.AppearancePage.PageClient.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.tcControl.AppearancePage.PageClient.Options.UseBackColor = true;
            this.tcControl.AppearancePage.PageClient.Options.UseBorderColor = true;
            this.tcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcControl.Location = new System.Drawing.Point(24, 3);
            this.tcControl.Name = "tcControl";
            this.tcControl.Size = new System.Drawing.Size(937, 474);
            this.tcControl.TabIndex = 0;
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 500);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "ControlForm";
            this.Text = "tcControl";
            this.TbLyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcControl)).EndInit();
            this.ResumeLayout(false);
            
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TbLyPanel;
        protected DevExpress.XtraTab.XtraTabControl tcControl;
    }
}