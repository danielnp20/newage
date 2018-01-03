namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PagoFlujos
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            this.txt_VlrTotalFlujo = new DevExpress.XtraEditors.TextEdit();
            this.lbl_VlrTotalFlujo = new System.Windows.Forms.Label();
            this.dtFechaFlujo = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaFlujo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOferta = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.masterComprador = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_VlrTotalFlujo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFlujo.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFlujo.Properties)).BeginInit();
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
            this.editSpin7,
            this.editDate,
            this.editValue,
            this.editValue4,
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
            // editSpin7
            // 
            this.editSpin7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.Mask.EditMask = "c7";
            this.editSpin7.Mask.UseMaskAsDisplayFormat = true;
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
            this.btnMark.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(6);
            this.txtDocumentoID.Size = new System.Drawing.Size(58, 16);
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtDocDesc.Size = new System.Drawing.Size(217, 16);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(6);
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 16);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(6);
            this.txtPrefix.Size = new System.Drawing.Size(50, 20);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtAF
            // 
            this.txtAF.Margin = new System.Windows.Forms.Padding(6);
            this.txtAF.Size = new System.Drawing.Size(91, 20);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(12, 0, 12, 6);
            this.gbGridDocument.Size = new System.Drawing.Size(579, 209);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(579, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 209);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.txtLibranza);
            this.grpboxHeader.Controls.Add(this.masterComprador);
            this.grpboxHeader.Controls.Add(this.label3);
            this.grpboxHeader.Controls.Add(this.txtOferta);
            this.grpboxHeader.Controls.Add(this.label1);
            this.grpboxHeader.Controls.Add(this.lblFechaFlujo);
            this.grpboxHeader.Controls.Add(this.dtFechaFlujo);
            this.grpboxHeader.Controls.Add(this.txt_VlrTotalFlujo);
            this.grpboxHeader.Controls.Add(this.lbl_VlrTotalFlujo);
            this.grpboxHeader.Location = new System.Drawing.Point(6, 30);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(1062, 93);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txt_VlrTotalFlujo
            // 
            this.txt_VlrTotalFlujo.EditValue = "0";
            this.txt_VlrTotalFlujo.Enabled = false;
            this.txt_VlrTotalFlujo.Location = new System.Drawing.Point(675, 45);
            this.txt_VlrTotalFlujo.Margin = new System.Windows.Forms.Padding(1);
            this.txt_VlrTotalFlujo.Name = "txt_VlrTotalFlujo";
            this.txt_VlrTotalFlujo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_VlrTotalFlujo.Properties.Appearance.Options.UseFont = true;
            this.txt_VlrTotalFlujo.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_VlrTotalFlujo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txt_VlrTotalFlujo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txt_VlrTotalFlujo.Properties.Mask.EditMask = "c";
            this.txt_VlrTotalFlujo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txt_VlrTotalFlujo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txt_VlrTotalFlujo.Size = new System.Drawing.Size(89, 20);
            this.txt_VlrTotalFlujo.TabIndex = 51;
            // 
            // lbl_VlrTotalFlujo
            // 
            this.lbl_VlrTotalFlujo.AutoSize = true;
            this.lbl_VlrTotalFlujo.Location = new System.Drawing.Point(571, 48);
            this.lbl_VlrTotalFlujo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lbl_VlrTotalFlujo.Name = "lbl_VlrTotalFlujo";
            this.lbl_VlrTotalFlujo.Size = new System.Drawing.Size(105, 13);
            this.lbl_VlrTotalFlujo.TabIndex = 52;
            this.lbl_VlrTotalFlujo.Text = "169_lbl_VlrTotalFlujo";
            // 
            // dtFechaFlujo
            // 
            this.dtFechaFlujo.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFlujo.Location = new System.Drawing.Point(107, 15);
            this.dtFechaFlujo.Name = "dtFechaFlujo";
            this.dtFechaFlujo.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFlujo.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFlujo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFlujo.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFlujo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFlujo.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFlujo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFlujo.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFlujo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFlujo.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFlujo.Size = new System.Drawing.Size(100, 20);
            this.dtFechaFlujo.TabIndex = 53;
            // 
            // lblFechaFlujo
            // 
            this.lblFechaFlujo.AutoSize = true;
            this.lblFechaFlujo.Location = new System.Drawing.Point(3, 18);
            this.lblFechaFlujo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblFechaFlujo.Name = "lblFechaFlujo";
            this.lblFechaFlujo.Size = new System.Drawing.Size(99, 13);
            this.lblFechaFlujo.TabIndex = 54;
            this.lblFechaFlujo.Text = "169_lbl_FechaFlujo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(277, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 55;
            this.label1.Text = "169_lbl_Oferta";
            // 
            // txtOferta
            // 
            this.txtOferta.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOferta.Location = new System.Drawing.Point(377, 15);
            this.txtOferta.Multiline = true;
            this.txtOferta.Name = "txtOferta";
            this.txtOferta.Size = new System.Drawing.Size(120, 19);
            this.txtOferta.TabIndex = 97;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(570, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 98;
            this.label3.Text = "169_lbl_Libranza";
            // 
            // masterComprador
            // 
            this.masterComprador.BackColor = System.Drawing.Color.Transparent;
            this.masterComprador.Filtros = null;
            this.masterComprador.Location = new System.Drawing.Point(9, 44);
            this.masterComprador.Name = "masterComprador";
            this.masterComprador.Size = new System.Drawing.Size(309, 25);
            this.masterComprador.TabIndex = 100;
            this.masterComprador.Value = "";
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(676, 14);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(121, 22);
            this.txtLibranza.TabIndex = 101;
            this.txtLibranza.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLibranza_KeyPress);
            // 
            // PagoFlujos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(903, 476);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "PagoFlujos";
            this.Text = "32564";
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_VlrTotalFlujo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFlujo.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFlujo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txt_VlrTotalFlujo;
        private System.Windows.Forms.Label lbl_VlrTotalFlujo;
        private System.Windows.Forms.Label lblFechaFlujo;
        protected DevExpress.XtraEditors.DateEdit dtFechaFlujo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        protected System.Windows.Forms.TextBox txtOferta;
        private ControlsUC.uc_MasterFind masterComprador;
        private System.Windows.Forms.TextBox txtLibranza;

    }
}