namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReclasificacionLibros
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
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.btnReclasificar = new System.Windows.Forms.Button();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.periodoEdit = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.masterTipoBal = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.SuspendLayout();
            // 
            // btnMayor
            // 
            this.btnReclasificar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReclasificar.Location = new System.Drawing.Point(116, 110);
            this.btnReclasificar.Name = "btnMayor";
            this.btnReclasificar.Size = new System.Drawing.Size(152, 33);
            this.btnReclasificar.TabIndex = 4;
            this.btnReclasificar.Text = "1107_btnReclasificar";
            this.btnReclasificar.UseVisualStyleBackColor = true;
            this.btnReclasificar.Click += new System.EventHandler(this.btnReclasificar_Click);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(29, 35);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblPeriod.TabIndex = 6;
            this.lblPeriod.Text = "1103_lblPeriod";
            // 
            // periodoEdit
            // 
            this.periodoEdit.BackColor = System.Drawing.Color.Transparent;
            this.periodoEdit.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.periodoEdit.EnabledControl = true;
            this.periodoEdit.ExtraPeriods = 0;
            this.periodoEdit.Location = new System.Drawing.Point(136, 31);
            this.periodoEdit.Name = "periodoEdit";
            this.periodoEdit.Size = new System.Drawing.Size(130, 21);
            this.periodoEdit.TabIndex = 7;
            this.periodoEdit.Enabled = false;
            // 
            // masterTipoBal
            // 
            this.masterTipoBal.BackColor = System.Drawing.Color.Transparent;
            this.masterTipoBal.Filtros = null;
            this.masterTipoBal.Location = new System.Drawing.Point(36, 65);
            this.masterTipoBal.Name = "masterTipoBal";
            this.masterTipoBal.Size = new System.Drawing.Size(291, 25);
            this.masterTipoBal.TabIndex = 8;
            this.masterTipoBal.Value = "";
            // 
            // MayorizarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 170);
            this.Controls.Add(this.masterTipoBal);
            this.Controls.Add(this.periodoEdit);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.btnReclasificar);
            this.Name = "ReclasificacionLibros";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1107";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReclasificar;
        private System.Windows.Forms.Label lblPeriod;
        private ControlsUC.uc_PeriodoEdit periodoEdit;
        private ControlsUC.uc_MasterFind masterTipoBal;
    }
}