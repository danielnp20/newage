namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class CierreDiaCartera
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
            base.InitializeComponent();
            this.btnCierre = new System.Windows.Forms.Button();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtFechaCierre = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 146);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(361, 18);
            // 
            // btnCierre
            // 
            this.btnCierre.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnCierre.Location = new System.Drawing.Point(86, 89);
            this.btnCierre.Name = "btnCierre";
            this.btnCierre.Size = new System.Drawing.Size(170, 33);
            this.btnCierre.TabIndex = 4;
            this.btnCierre.Text = "1131_btnCierre";
            this.btnCierre.UseVisualStyleBackColor = true;
            this.btnCierre.Click += new System.EventHandler(this.btnCierre_Click);
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.Enabled = false;
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(155, 12);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 20);
            this.dtPeriod.TabIndex = 73;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(59, 16);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(77, 13);
            this.lblPeriod.TabIndex = 72;
            this.lblPeriod.Text = "1111_lblPeriod";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 74;
            this.label1.Text = "1143_lblFechaCierre";
            // 
            // dtFechaCierre
            // 
            this.dtFechaCierre.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaCierre.Location = new System.Drawing.Point(156, 50);
            this.dtFechaCierre.Name = "dtFechaCierre";
            this.dtFechaCierre.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaCierre.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaCierre.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaCierre.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCierre.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCierre.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCierre.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCierre.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaCierre.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaCierre.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaCierre.Size = new System.Drawing.Size(100, 20);
            this.dtFechaCierre.TabIndex = 75;
            // 
            // CierreDia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 164);
            this.Controls.Add(this.dtFechaCierre);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.btnCierre);
            this.Name = "CierreDia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1143";
            this.Controls.SetChildIndex(this.btnCierre, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.lblPeriod, 0);
            this.Controls.SetChildIndex(this.dtPeriod, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dtFechaCierre, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCierre.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCierre;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Label label1;
        protected DevExpress.XtraEditors.DateEdit dtFechaCierre;
    }
}