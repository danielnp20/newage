namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class CierreLegalizacion
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
            this.uc_Periodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.lblPeriodo = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 253);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(444, 18);
            // 
            // uc_Periodo
            // 
            this.uc_Periodo.BackColor = System.Drawing.Color.Transparent;
            this.uc_Periodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.uc_Periodo.EnabledControl = true;
            this.uc_Periodo.ExtraPeriods = 0;
            this.uc_Periodo.Location = new System.Drawing.Point(191, 92);
            this.uc_Periodo.MaxValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.MinValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.Name = "uc_Periodo";
            this.uc_Periodo.Size = new System.Drawing.Size(130, 20);
            this.uc_Periodo.TabIndex = 1;
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(97, 155);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(224, 23);
            this.btnProcesar.TabIndex = 3;
            this.btnProcesar.Text = "1138_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.Location = new System.Drawing.Point(97, 94);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(76, 13);
            this.lblPeriodo.TabIndex = 2;
            this.lblPeriodo.Text = "1138_lblPeriodo";
            // 
            // FacturaEquivalente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 271);
            this.Controls.Add(this.uc_Periodo);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.lblPeriodo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FacturaEquivalente";
            this.Text = "CierreNomina";
            this.Controls.SetChildIndex(this.lblPeriodo, 0);
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.uc_Periodo, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlsUC.uc_PeriodoEdit uc_Periodo;
        private System.Windows.Forms.Button btnProcesar;
        private DevExpress.XtraEditors.LabelControl lblPeriodo;
    }
}