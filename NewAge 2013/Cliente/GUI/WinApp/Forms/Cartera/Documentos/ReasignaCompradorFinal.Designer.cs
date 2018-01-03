namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReasignaCompradorFinal
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterCompradorCartera = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCompradorFinal = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lbl_CompradorFinal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editBtnGrid,
            this.editCmb,
            this.editText,
            this.editSpin,
            this.editSpin4,
            this.editDate,
            this.editValue,
            this.editValue4});
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
            // editDate
            // 
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editValue4
            // 
            this.editValue4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // btnMark
            // 
            this.btnMark.Margin = new System.Windows.Forms.Padding(12);
            this.btnMark.Size = new System.Drawing.Size(98, 32);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(12);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(12);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(12);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(200, 32);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(12);
            this.txtPrefix.Size = new System.Drawing.Size(96, 35);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtAF
            // 
            this.txtAF.Margin = new System.Windows.Forms.Padding(12);
            this.txtAF.Size = new System.Drawing.Size(178, 35);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(12);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(24, 0, 24, 12);
            this.gbGridDocument.Size = new System.Drawing.Size(1558, 469);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(1558, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(12);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(32, 12, 32, 12);
            this.gbGridProvider.Size = new System.Drawing.Size(592, 469);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lbl_CompradorFinal);
            this.grpboxHeader.Controls.Add(this.masterCompradorCartera);
            this.grpboxHeader.Controls.Add(this.masterCompradorFinal);
            this.grpboxHeader.Location = new System.Drawing.Point(12, 52);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(12);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(12);
            this.grpboxHeader.Size = new System.Drawing.Size(2124, 225);
            // 
            // masterCompradorCartera
            // 
            this.masterCompradorCartera.BackColor = System.Drawing.Color.Transparent;
            this.masterCompradorCartera.Filtros = null;
            this.masterCompradorCartera.Location = new System.Drawing.Point(12, 21);
            this.masterCompradorCartera.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.masterCompradorCartera.Name = "masterCompradorCartera";
            this.masterCompradorCartera.Size = new System.Drawing.Size(594, 48);
            this.masterCompradorCartera.TabIndex = 0;
            this.masterCompradorCartera.Value = "";
            this.masterCompradorCartera.Leave += new System.EventHandler(this.masterCompradorCartera_Leave);
            // 
            // masterCompradorFinal
            // 
            this.masterCompradorFinal.BackColor = System.Drawing.Color.Transparent;
            this.masterCompradorFinal.Filtros = null;
            this.masterCompradorFinal.Location = new System.Drawing.Point(12, 102);
            this.masterCompradorFinal.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.masterCompradorFinal.Name = "masterCompradorFinal";
            this.masterCompradorFinal.Size = new System.Drawing.Size(594, 48);
            this.masterCompradorFinal.TabIndex = 1;
            this.masterCompradorFinal.Value = "";
            this.masterCompradorFinal.Leave += new System.EventHandler(this.masterCompradorFinal_Leave);
            // 
            // lbl_CompradorFinal
            // 
            this.lbl_CompradorFinal.AutoSize = true;
            this.lbl_CompradorFinal.Location = new System.Drawing.Point(7, 109);
            this.lbl_CompradorFinal.Name = "lbl_CompradorFinal";
            this.lbl_CompradorFinal.Size = new System.Drawing.Size(216, 26);
            this.lbl_CompradorFinal.TabIndex = 2;
            this.lbl_CompradorFinal.Text = "170_CompradorFinal";
            // 
            // ReasignaCompradorFinal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.ClientSize = new System.Drawing.Size(2202, 1117);
            this.Margin = new System.Windows.Forms.Padding(12);
            this.Name = "ReasignaCompradorFinal";
            this.Text = "32564";
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterCompradorCartera;
        private System.Windows.Forms.Label lbl_CompradorFinal;
        private ControlsUC.uc_MasterFind masterCompradorFinal;

    }
}