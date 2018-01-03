namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class TarjetaPago
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
            this.masterTarjetaCredito = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblMoneda = new System.Windows.Forms.Label();
            this.cmbMoneda = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.lblValor = new System.Windows.Forms.Label();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.txtDocumentoTercero = new DevExpress.XtraEditors.TextEdit();
            this.lblDocumentoTercero = new System.Windows.Forms.Label();
            this.masterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtDocumentoTercero = new DevExpress.XtraEditors.DateEdit();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotal = new DevExpress.XtraEditors.TextEdit();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.grpboxDetail.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDocumentoTercero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDocumentoTercero.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDocumentoTercero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.lblTotal);
            this.grpboxDetail.Controls.Add(this.txtTotal);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Size = new System.Drawing.Size(1187, 167);
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
            this.editValue.Mask.EditMask = "c4";
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
            this.btnMark.Size = new System.Drawing.Size(49, 20);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbGridDocument.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbGridDocument.Size = new System.Drawing.Size(894, 203);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(894, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(293, 185);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.txtDescripcion);
            this.grpboxHeader.Controls.Add(this.dtDocumentoTercero);
            this.grpboxHeader.Controls.Add(this.masterCentroCosto);
            this.grpboxHeader.Controls.Add(this.lblValor);
            this.grpboxHeader.Controls.Add(this.txtValor);
            this.grpboxHeader.Controls.Add(this.txtDocumentoTercero);
            this.grpboxHeader.Controls.Add(this.lblDocumentoTercero);
            this.grpboxHeader.Controls.Add(this.cmbMoneda);
            this.grpboxHeader.Controls.Add(this.lblMoneda);
            this.grpboxHeader.Controls.Add(this.masterTarjetaCredito);
            this.grpboxHeader.Controls.Add(this.lblDescripcion);
            this.grpboxHeader.Controls.Add(this.masterProyecto);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Size = new System.Drawing.Size(1183, 89);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // masterTarjetaCredito
            // 
            this.masterTarjetaCredito.BackColor = System.Drawing.Color.Transparent;
            this.masterTarjetaCredito.Filtros = null;
            this.masterTarjetaCredito.Location = new System.Drawing.Point(22, 25);
            this.masterTarjetaCredito.Name = "masterTarjetaCredito";
            this.masterTarjetaCredito.Size = new System.Drawing.Size(339, 27);
            this.masterTarjetaCredito.TabIndex = 0;
            this.masterTarjetaCredito.Value = "";
            this.masterTarjetaCredito.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(336, 56);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(339, 27);
            this.masterProyecto.TabIndex = 5;
            this.masterProyecto.Value = "";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcion.Location = new System.Drawing.Point(20, 85);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(100, 14);
            this.lblDescripcion.TabIndex = 12;
            this.lblDescripcion.Text = "25_lblDescripcion";
            // 
            // lblMoneda
            // 
            this.lblMoneda.AutoSize = true;
            this.lblMoneda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMoneda.Location = new System.Drawing.Point(629, 30);
            this.lblMoneda.Name = "lblMoneda";
            this.lblMoneda.Size = new System.Drawing.Size(82, 14);
            this.lblMoneda.TabIndex = 11;
            this.lblMoneda.Text = "25_lblMoneda";
            // 
            // cmbMoneda
            // 
            this.cmbMoneda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMoneda.FormattingEnabled = true;
            this.cmbMoneda.Location = new System.Drawing.Point(699, 26);
            this.cmbMoneda.Name = "cmbMoneda";
            this.cmbMoneda.Size = new System.Drawing.Size(99, 22);
            this.cmbMoneda.TabIndex = 2;
            this.cmbMoneda.SelectedIndexChanged += new System.EventHandler(this.cmbMoneda_SelectedIndexChanged);
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.Location = new System.Drawing.Point(20, 139);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(76, 14);
            this.lblValor.TabIndex = 8;
            this.lblValor.Text = "25_lblValor";
            // 
            // txtValor
            // 
            this.txtValor.EditValue = "0";
            this.txtValor.Location = new System.Drawing.Point(123, 138);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Properties.Appearance.Options.UseFont = true;
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.Mask.EditMask = "c";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Size = new System.Drawing.Size(193, 20);
            this.txtValor.TabIndex = 6;
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            // 
            // txtDocumentoTercero
            // 
            this.txtDocumentoTercero.Location = new System.Drawing.Point(438, 27);
            this.txtDocumentoTercero.Name = "txtDocumentoTercero";
            this.txtDocumentoTercero.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoTercero.Properties.Appearance.Options.UseFont = true;
            this.txtDocumentoTercero.Size = new System.Drawing.Size(122, 20);
            this.txtDocumentoTercero.TabIndex = 9;
            // 
            // lblDocumentoTercero
            // 
            this.lblDocumentoTercero.AutoSize = true;
            this.lblDocumentoTercero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentoTercero.Location = new System.Drawing.Point(334, 30);
            this.lblDocumentoTercero.Name = "lblDocumentoTercero";
            this.lblDocumentoTercero.Size = new System.Drawing.Size(146, 14);
            this.lblDocumentoTercero.TabIndex = 10;
            this.lblDocumentoTercero.Text = "25_lblDocumentoTercero";
            // 
            // masterCentroCosto
            // 
            this.masterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCosto.Filtros = null;
            this.masterCentroCosto.Location = new System.Drawing.Point(22, 56);
            this.masterCentroCosto.Name = "masterCentroCosto";
            this.masterCentroCosto.Size = new System.Drawing.Size(308, 27);
            this.masterCentroCosto.TabIndex = 4;
            this.masterCentroCosto.Value = "";
            // 
            // dtDocumentoTercero
            // 
            this.dtDocumentoTercero.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtDocumentoTercero.Location = new System.Drawing.Point(436, 27);
            this.dtDocumentoTercero.Name = "dtDocumentoTercero";
            this.dtDocumentoTercero.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtDocumentoTercero.Properties.Appearance.Options.UseBackColor = true;
            this.dtDocumentoTercero.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtDocumentoTercero.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtDocumentoTercero.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtDocumentoTercero.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtDocumentoTercero.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtDocumentoTercero.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtDocumentoTercero.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtDocumentoTercero.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtDocumentoTercero.Size = new System.Drawing.Size(124, 20);
            this.dtDocumentoTercero.TabIndex = 1;
            this.dtDocumentoTercero.Leave += new System.EventHandler(this.dtDocumentoTercero_Leave);
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(605, 13);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(76, 14);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "25_lblTotal";
            // 
            // txtTotal
            // 
            this.txtTotal.EditValue = "0";
            this.txtTotal.Location = new System.Drawing.Point(668, 10);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.Properties.Appearance.Options.UseFont = true;
            this.txtTotal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotal.Properties.Mask.EditMask = "c";
            this.txtTotal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotal.Properties.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(170, 20);
            this.txtTotal.TabIndex = 1;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.BackColor = System.Drawing.Color.White;
            this.txtDescripcion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescripcion.Location = new System.Drawing.Point(123, 88);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(675, 39);
            this.txtDescripcion.TabIndex = 13;
            // 
            // TarjetaPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 487);
            this.Name = "TarjetaPago";
            this.Text = "25";
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDocumentoTercero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDocumentoTercero.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDocumentoTercero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterTarjetaCredito;
        private ControlsUC.uc_MasterFind masterProyecto;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblMoneda;
        private Clases.ComboBoxEx cmbMoneda;
        private System.Windows.Forms.Label lblValor;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private DevExpress.XtraEditors.TextEdit txtDocumentoTercero;
        private System.Windows.Forms.Label lblDocumentoTercero;
        private ControlsUC.uc_MasterFind masterCentroCosto;
        protected DevExpress.XtraEditors.DateEdit dtDocumentoTercero;
        private System.Windows.Forms.Label lblTotal;
        private DevExpress.XtraEditors.TextEdit txtTotal;
        private System.Windows.Forms.TextBox txtDescripcion;

    }
}