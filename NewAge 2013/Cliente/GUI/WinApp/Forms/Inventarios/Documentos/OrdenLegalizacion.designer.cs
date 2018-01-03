namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class OrdenLegalizacion
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.txtNro = new System.Windows.Forms.TextBox();
            this.lblNro = new System.Windows.Forms.Label();
            this.lblResumenMvtoImport = new System.Windows.Forms.Label();
            this.txtNroOrdenLegal = new System.Windows.Forms.TextBox();
            this.lblNroOrdenLegal = new System.Windows.Forms.Label();
            this.btnFindDocument = new DevExpress.XtraEditors.ButtonEdit();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblModalidad = new DevExpress.XtraEditors.LabelControl();
            this.cmbModalidad = new DevExpress.XtraEditors.LookUpEdit();
            this.pnHeader = new DevExpress.XtraEditors.PanelControl();
            this.lblAgenteAduana = new System.Windows.Forms.Label();
            this.masterAgenteAduanaProv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
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
            this.gbGridDocument.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFindDocument.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbModalidad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnHeader)).BeginInit();
            this.pnHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Padding = new System.Windows.Forms.Padding(12, 6, 6, 6);
            this.grpboxDetail.Size = new System.Drawing.Size(1016, 167);
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
            this.btnMark.Margin = new System.Windows.Forms.Padding(6);
            this.btnMark.Size = new System.Drawing.Size(49, 20);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(6);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(6);
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 20);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(6);
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
            this.gbGridDocument.Controls.Add(this.lblResumenMvtoImport);
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(12, 0, 12, 6);
            this.gbGridDocument.Size = new System.Drawing.Size(720, 150);
            this.gbGridDocument.Controls.SetChildIndex(this.lblResumenMvtoImport, 0);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(720, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 150);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.btnQueryDoc);
            this.grpboxHeader.Controls.Add(this.pnHeader);
            this.grpboxHeader.Controls.Add(this.btnFindDocument);
            this.grpboxHeader.Controls.Add(this.txtNro);
            this.grpboxHeader.Controls.Add(this.lblNro);
            this.grpboxHeader.Controls.Add(this.masterPrefijo);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(1012, 67);
            this.grpboxHeader.TabIndex = 0;
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtNro
            // 
            this.txtNro.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNro.Location = new System.Drawing.Point(377, 18);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(61, 21);
            this.txtNro.TabIndex = 1;
            this.txtNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNro_KeyPress);
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // lblNro
            // 
            this.lblNro.AutoSize = true;
            this.lblNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNro.Location = new System.Drawing.Point(341, 21);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(79, 14);
            this.lblNro.TabIndex = 17;
            this.lblNro.Text = "50502_lblNro";
            // 
            // lblResumenMvtoImport
            // 
            this.lblResumenMvtoImport.AutoSize = true;
            this.lblResumenMvtoImport.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResumenMvtoImport.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumenMvtoImport.Location = new System.Drawing.Point(12, -3);
            this.lblResumenMvtoImport.Name = "lblResumenMvtoImport";
            this.lblResumenMvtoImport.Size = new System.Drawing.Size(197, 14);
            this.lblResumenMvtoImport.TabIndex = 51;
            this.lblResumenMvtoImport.Text = "50502_lblResumenMovimiento";
            // 
            // txtNroOrdenLegal
            // 
            this.txtNroOrdenLegal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroOrdenLegal.Location = new System.Drawing.Point(456, 10);
            this.txtNroOrdenLegal.Name = "txtNroOrdenLegal";
            this.txtNroOrdenLegal.Size = new System.Drawing.Size(91, 21);
            this.txtNroOrdenLegal.TabIndex = 1;
            this.txtNroOrdenLegal.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // lblNroOrdenLegal
            // 
            this.lblNroOrdenLegal.AutoSize = true;
            this.lblNroOrdenLegal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroOrdenLegal.Location = new System.Drawing.Point(325, 13);
            this.lblNroOrdenLegal.Name = "lblNroOrdenLegal";
            this.lblNroOrdenLegal.Size = new System.Drawing.Size(141, 14);
            this.lblNroOrdenLegal.TabIndex = 15;
            this.lblNroOrdenLegal.Text = "50502_lblNroOrdenLegal";
            // 
            // btnFindDocument
            // 
            this.btnFindDocument.Location = new System.Drawing.Point(918, 13);
            this.btnFindDocument.Name = "btnFindDocument";
            this.btnFindDocument.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::NewAge.Properties.Resources.FindFkHierarchy, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.btnFindDocument.Size = new System.Drawing.Size(91, 20);
            this.btnFindDocument.TabIndex = 19;
            this.btnFindDocument.Visible = false;
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(21, 16);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(291, 25);
            this.masterPrefijo.TabIndex = 0;
            this.masterPrefijo.Value = "";
            // 
            // lblModalidad
            // 
            this.lblModalidad.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModalidad.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblModalidad.Location = new System.Drawing.Point(589, 13);
            this.lblModalidad.Margin = new System.Windows.Forms.Padding(4);
            this.lblModalidad.Name = "lblModalidad";
            this.lblModalidad.Size = new System.Drawing.Size(138, 14);
            this.lblModalidad.TabIndex = 13;
            this.lblModalidad.Text = "50502_lblModalidadImpor";
            // 
            // cmbModalidad
            // 
            this.cmbModalidad.Location = new System.Drawing.Point(760, 10);
            this.cmbModalidad.Name = "cmbModalidad";
            this.cmbModalidad.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbModalidad.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbModalidad.Properties.DisplayMember = "Value";
            this.cmbModalidad.Properties.ValueMember = "Key";
            this.cmbModalidad.Size = new System.Drawing.Size(159, 20);
            this.cmbModalidad.TabIndex = 2;
            this.cmbModalidad.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // pnHeader
            // 
            this.pnHeader.Controls.Add(this.lblAgenteAduana);
            this.pnHeader.Controls.Add(this.masterAgenteAduanaProv);
            this.pnHeader.Controls.Add(this.lblNroOrdenLegal);
            this.pnHeader.Controls.Add(this.cmbModalidad);
            this.pnHeader.Controls.Add(this.txtNroOrdenLegal);
            this.pnHeader.Controls.Add(this.lblModalidad);
            this.pnHeader.Location = new System.Drawing.Point(14, 47);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(974, 39);
            this.pnHeader.TabIndex = 22;
            // 
            // lblAgenteAduana
            // 
            this.lblAgenteAduana.AutoSize = true;
            this.lblAgenteAduana.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAgenteAduana.Location = new System.Drawing.Point(5, 13);
            this.lblAgenteAduana.Name = "lblAgenteAduana";
            this.lblAgenteAduana.Size = new System.Drawing.Size(142, 14);
            this.lblAgenteAduana.TabIndex = 23;
            this.lblAgenteAduana.Text = "50502_lblAgenteAduana";
            // 
            // masterAgenteAduanaProv
            // 
            this.masterAgenteAduanaProv.BackColor = System.Drawing.Color.Transparent;
            this.masterAgenteAduanaProv.Filtros = null;
            this.masterAgenteAduanaProv.Location = new System.Drawing.Point(7, 8);
            this.masterAgenteAduanaProv.Name = "masterAgenteAduanaProv";
            this.masterAgenteAduanaProv.Size = new System.Drawing.Size(291, 25);
            this.masterAgenteAduanaProv.TabIndex = 0;
            this.masterAgenteAduanaProv.Value = "";
            this.masterAgenteAduanaProv.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(441, 18);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(35, 21);
            this.btnQueryDoc.TabIndex = 23;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // OrdenLegalizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1044, 430);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "OrdenLegalizacion";
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
            this.gbGridDocument.ResumeLayout(false);
            this.gbGridDocument.PerformLayout();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFindDocument.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbModalidad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnHeader)).EndInit();
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TextBox txtNro;
        private System.Windows.Forms.Label lblNro;
        private System.Windows.Forms.Label lblResumenMvtoImport;
        private System.Windows.Forms.TextBox txtNroOrdenLegal;
        private System.Windows.Forms.Label lblNroOrdenLegal;
        private DevExpress.XtraEditors.ButtonEdit btnFindDocument;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.LookUpEdit cmbModalidad;
        private DevExpress.XtraEditors.LabelControl lblModalidad;
        private DevExpress.XtraEditors.PanelControl pnHeader;
        private ControlsUC.uc_MasterFind masterAgenteAduanaProv;
        private System.Windows.Forms.Label lblAgenteAduana;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
    }
}