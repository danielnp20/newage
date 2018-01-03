namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class CierreMensual
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
            this.btnCierrePeriodo = new System.Windows.Forms.Button();
            this.lblTipoBalance = new System.Windows.Forms.Label();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.periodoEdit = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.cmbModulo = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.SuspendLayout();
            // 
            // btnCierrePeriodo
            // 
            this.btnCierrePeriodo.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCierrePeriodo.Location = new System.Drawing.Point(88, 85);
            this.btnCierrePeriodo.Name = "btnCierrePeriodo";
            this.btnCierrePeriodo.Size = new System.Drawing.Size(152, 33);
            this.btnCierrePeriodo.TabIndex = 4;
            this.btnCierrePeriodo.Text = "1105_btnClosePeriod";
            this.btnCierrePeriodo.UseVisualStyleBackColor = true;
            this.btnCierrePeriodo.Click += new System.EventHandler(this.btnCierre_Click);
            // 
            // lblTipoBalance
            // 
            this.lblTipoBalance.AutoSize = true;
            this.lblTipoBalance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoBalance.Location = new System.Drawing.Point(28, 30);
            this.lblTipoBalance.Name = "lblTipoBalance";
            this.lblTipoBalance.Size = new System.Drawing.Size(92, 14);
            this.lblTipoBalance.TabIndex = 5;
            this.lblTipoBalance.Text = "1105_lblModulo";
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(177, 30);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblPeriod.TabIndex = 6;
            this.lblPeriod.Text = "1105_lblPeriod";
            // 
            // periodoEdit
            // 
            this.periodoEdit.BackColor = System.Drawing.Color.Transparent;
            this.periodoEdit.DateTime = new System.DateTime(((long)(0)));
            this.periodoEdit.EnabledControl = false;
            this.periodoEdit.ExtraPeriods = 0;
            this.periodoEdit.Location = new System.Drawing.Point(180, 52);
            this.periodoEdit.Name = "periodoEdit";
            this.periodoEdit.Size = new System.Drawing.Size(130, 21);
            this.periodoEdit.TabIndex = 7;
            // 
            // cmbModulo
            // 
            this.cmbModulo.BackColor = System.Drawing.Color.White;
            this.cmbModulo.FormattingEnabled = true;
            this.cmbModulo.Location = new System.Drawing.Point(31, 51);
            this.cmbModulo.Name = "cmbModulo";
            this.cmbModulo.Size = new System.Drawing.Size(121, 21);
            this.cmbModulo.TabIndex = 8;
            this.cmbModulo.SelectedValueChanged += new System.EventHandler(this.cmbModulo_SelectedValueChanged);
            // 
            // CierreMensual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 149);
            this.Controls.Add(this.cmbModulo);
            this.Controls.Add(this.periodoEdit);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lblTipoBalance);
            this.Controls.Add(this.btnCierrePeriodo);
            this.Name = "CierreMensual";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1105";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCierrePeriodo;
        private System.Windows.Forms.Label lblTipoBalance;
        private System.Windows.Forms.Label lblPeriod;
        private ControlsUC.uc_PeriodoEdit periodoEdit;
        private NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx cmbModulo;
    }
}