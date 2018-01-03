namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalQueryOrdCompra
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
            this.masterProveedor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).BeginInit();
            this.pnGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnDocument)).BeginInit();
            this.pnDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnGrid)).BeginInit();
            this.pnGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGeneral)).BeginInit();
            this.gbFilterGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterByDocumento)).BeginInit();
            this.gbFilterByDocumento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dtFechaInicial
            // 
            this.dtFechaInicial.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicial.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaInicial.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaInicial.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtFechaFinal
            // 
            this.dtFechaFinal.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinal.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinal.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinal.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkSelectAll.Properties.Appearance.Options.UseFont = true;
            // 
            // gbFilterGeneral
            // 
            this.gbFilterGeneral.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterGeneral.Appearance.Options.UseFont = true;
            this.gbFilterGeneral.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterGeneral.AppearanceCaption.Options.UseFont = true;
            // 
            // gbFilterByDocumento
            // 
            this.gbFilterByDocumento.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterByDocumento.Appearance.Options.UseFont = true;
            this.gbFilterByDocumento.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterByDocumento.AppearanceCaption.Options.UseFont = true;
            this.gbFilterByDocumento.Controls.Add(this.masterProveedor);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editSpinPorcen});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin4
            // 
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // masterProveedor
            // 
            this.masterProveedor.BackColor = System.Drawing.Color.Transparent;
            this.masterProveedor.Filtros = null;
            this.masterProveedor.Location = new System.Drawing.Point(7, 29);
            this.masterProveedor.Name = "masterProveedor";
            this.masterProveedor.Size = new System.Drawing.Size(349, 25);
            this.masterProveedor.TabIndex = 2;
            this.masterProveedor.Value = "";
            // 
            // ModalQueryOrdCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 417);
            this.Name = "ModalQueryOrdCompra";
            this.Text = "ModalQueryOrdCompra";
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).EndInit();
            this.pnGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnDocument)).EndInit();
            this.pnDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnGrid)).EndInit();
            this.pnGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGeneral)).EndInit();
            this.gbFilterGeneral.ResumeLayout(false);
            this.gbFilterGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterByDocumento)).EndInit();
            this.gbFilterByDocumento.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstate.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private ControlsUC.uc_MasterFind masterProveedor;

    }
}