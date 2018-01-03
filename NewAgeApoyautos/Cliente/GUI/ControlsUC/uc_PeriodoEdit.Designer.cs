namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    partial class uc_PeriodoEdit
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
            this.lblExtra = new System.Windows.Forms.Label();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.PeriodControl();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriod.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriod.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblExtra
            // 
            this.lblExtra.AutoSize = true;
            this.lblExtra.Location = new System.Drawing.Point(106, 4);
            this.lblExtra.Name = "lblExtra";
            this.lblExtra.Size = new System.Drawing.Size(13, 13);
            this.lblExtra.TabIndex = 1;
            this.lblExtra.Text = "1";
            // 
            // dtPeriod
            // 
            this.dtPeriod.DateTimePeriodo = new System.DateTime(((long)(0)));
            this.dtPeriod.EditValue = null;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(1, 0);
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.PeriodAccept = null;
            this.dtPeriod.PeriodSelect = null;
            this.dtPeriod.PeriodTitFrm = null;
            this.dtPeriod.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dtPeriod.Properties.Appearance.Options.UseFont = true;
            this.dtPeriod.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPeriod.Properties.DisplayFormat.FormatString = "yyyy/MM";
            this.dtPeriod.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtPeriod.Properties.EditFormat.FormatString = "yyyy/MM";
            this.dtPeriod.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtPeriod.Properties.Mask.EditMask = "yyyy/MM";
            this.dtPeriod.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtPeriod.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtPeriod.Size = new System.Drawing.Size(100, 20);
            this.dtPeriod.TabIndex = 0;
            this.dtPeriod.EditValueChanged += new System.EventHandler(this.dtPeriod_EditValueChanged);
            // 
            // uc_PeriodoEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lblExtra);
            this.Controls.Add(this.dtPeriod);
            this.Name = "uc_PeriodoEdit";
            this.Size = new System.Drawing.Size(130, 20);
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriod.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriod.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PeriodControl dtPeriod;
        private System.Windows.Forms.Label lblExtra;
    }
}
