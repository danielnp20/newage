namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ProrrateoIVA
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
            this.lblPeriod = new System.Windows.Forms.Label();
            this.uc_Periodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.SuspendLayout();
            // 
            // btnProcesar
            // 
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.Location = new System.Drawing.Point(63, 80);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 33);
            this.btnProcesar.TabIndex = 4;
            this.btnProcesar.Text = "1118_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(43, 35);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblPeriod.TabIndex = 6;
            this.lblPeriod.Text = "1118_lblPeriod";
            // 
            // uc_Periodo
            // 
            this.uc_Periodo.BackColor = System.Drawing.Color.Transparent;
            this.uc_Periodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.uc_Periodo.EnabledControl = false;
            this.uc_Periodo.ExtraPeriods = 0;
            this.uc_Periodo.Location = new System.Drawing.Point(147, 33);
            this.uc_Periodo.MaxValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.MinValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.Name = "uc_Periodo";
            this.uc_Periodo.Size = new System.Drawing.Size(130, 21);
            this.uc_Periodo.TabIndex = 7;
            // 
            // ProrrateoIVA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 149);
            this.Controls.Add(this.uc_Periodo);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.btnProcesar);
            this.Name = "ProrrateoIVA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1118";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Label lblPeriod;
        private ControlsUC.uc_PeriodoEdit uc_Periodo;
    }
}