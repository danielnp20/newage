namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReprocesoDepreciacion
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
            this.btnProcesar = new System.Windows.Forms.Button();
            this.lblPeriodIni = new System.Windows.Forms.Label();
            this.uc_PeriodoIni = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.uc_PeriodoFin = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriodFin = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 175);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(334, 18);
            // 
            // btnProcesar
            // 
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.Location = new System.Drawing.Point(77, 110);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 33);
            this.btnProcesar.TabIndex = 4;
            this.btnProcesar.Text = "1118_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // lblPeriodIni
            // 
            this.lblPeriodIni.AutoSize = true;
            this.lblPeriodIni.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodIni.Location = new System.Drawing.Point(43, 35);
            this.lblPeriodIni.Name = "lblPeriodIni";
            this.lblPeriodIni.Size = new System.Drawing.Size(100, 14);
            this.lblPeriodIni.TabIndex = 6;
            this.lblPeriodIni.Text = "1134_lblPeriodIni";
            // 
            // uc_PeriodoIni
            // 
            this.uc_PeriodoIni.BackColor = System.Drawing.Color.Transparent;
            this.uc_PeriodoIni.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.uc_PeriodoIni.EnabledControl = true;
            this.uc_PeriodoIni.ExtraPeriods = 0;
            this.uc_PeriodoIni.Location = new System.Drawing.Point(147, 33);
            this.uc_PeriodoIni.MaxValue = new System.DateTime(((long)(0)));
            this.uc_PeriodoIni.MinValue = new System.DateTime(((long)(0)));
            this.uc_PeriodoIni.Name = "uc_PeriodoIni";
            this.uc_PeriodoIni.Size = new System.Drawing.Size(130, 21);
            this.uc_PeriodoIni.TabIndex = 7;
            // 
            // uc_PeriodoFin
            // 
            this.uc_PeriodoFin.BackColor = System.Drawing.Color.Transparent;
            this.uc_PeriodoFin.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.uc_PeriodoFin.EnabledControl = true;
            this.uc_PeriodoFin.ExtraPeriods = 0;
            this.uc_PeriodoFin.Location = new System.Drawing.Point(147, 65);
            this.uc_PeriodoFin.MaxValue = new System.DateTime(((long)(0)));
            this.uc_PeriodoFin.MinValue = new System.DateTime(((long)(0)));
            this.uc_PeriodoFin.Name = "uc_PeriodoFin";
            this.uc_PeriodoFin.Size = new System.Drawing.Size(130, 21);
            this.uc_PeriodoFin.TabIndex = 9;
            // 
            // lblPeriodFin
            // 
            this.lblPeriodFin.AutoSize = true;
            this.lblPeriodFin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodFin.Location = new System.Drawing.Point(43, 67);
            this.lblPeriodFin.Name = "lblPeriodFin";
            this.lblPeriodFin.Size = new System.Drawing.Size(102, 14);
            this.lblPeriodFin.TabIndex = 8;
            this.lblPeriodFin.Text = "1134_lblPeriodFin";
            // 
            // ReprocesoDepreciacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 193);
            this.Controls.Add(this.uc_PeriodoFin);
            this.Controls.Add(this.lblPeriodFin);
            this.Controls.Add(this.uc_PeriodoIni);
            this.Controls.Add(this.lblPeriodIni);
            this.Controls.Add(this.btnProcesar);
            this.Name = "ReprocesoDepreciacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1132";
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.lblPeriodIni, 0);
            this.Controls.SetChildIndex(this.uc_PeriodoIni, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.lblPeriodFin, 0);
            this.Controls.SetChildIndex(this.uc_PeriodoFin, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Label lblPeriodIni;
        private ControlsUC.uc_PeriodoEdit uc_PeriodoIni;
        private ControlsUC.uc_PeriodoEdit uc_PeriodoFin;
        private System.Windows.Forms.Label lblPeriodFin;
    }
}