namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DeterioroRevalorizacionInv
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.txtNro = new System.Windows.Forms.TextBox();
            this.lblNro = new System.Windows.Forms.Label();
            this.lblResumenMvtoImport = new System.Windows.Forms.Label();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblTipoDoc = new DevExpress.XtraEditors.LabelControl();
            this.cmbTipoDoc = new DevExpress.XtraEditors.LookUpEdit();
            this.pnHeader = new DevExpress.XtraEditors.PanelControl();
            this.btnGetDoc = new DevExpress.XtraEditors.SimpleButton();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.lblEstado = new DevExpress.XtraEditors.LabelControl();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.masterCosteoGrupo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbMdaOrigen = new DevExpress.XtraEditors.LookUpEdit();
            this.lblMdaOrigen = new DevExpress.XtraEditors.LabelControl();
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
            this.gbGridDocument.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnHeader)).BeginInit();
            this.pnHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMdaOrigen.Properties)).BeginInit();
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
            this.gbGridDocument.Size = new System.Drawing.Size(720, 219);
            this.gbGridDocument.Controls.SetChildIndex(this.lblResumenMvtoImport, 0);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(720, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 219);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.cmbMdaOrigen);
            this.grpboxHeader.Controls.Add(this.lblMdaOrigen);
            this.grpboxHeader.Controls.Add(this.pnHeader);
            this.grpboxHeader.Controls.Add(this.lblTipoDoc);
            this.grpboxHeader.Controls.Add(this.cmbTipoDoc);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(1012, 110);
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
            this.txtNro.Location = new System.Drawing.Point(352, 7);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(42, 21);
            this.txtNro.TabIndex = 1;
            this.txtNro.Text = "0";
            this.txtNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNro_KeyPress);
            // 
            // lblNro
            // 
            this.lblNro.AutoSize = true;
            this.lblNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNro.Location = new System.Drawing.Point(309, 10);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(58, 14);
            this.lblNro.TabIndex = 17;
            this.lblNro.Text = "57_lblNro";
            // 
            // lblResumenMvtoImport
            // 
            this.lblResumenMvtoImport.AutoSize = true;
            this.lblResumenMvtoImport.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResumenMvtoImport.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumenMvtoImport.Location = new System.Drawing.Point(12, -3);
            this.lblResumenMvtoImport.Name = "lblResumenMvtoImport";
            this.lblResumenMvtoImport.Size = new System.Drawing.Size(101, 14);
            this.lblResumenMvtoImport.TabIndex = 51;
            this.lblResumenMvtoImport.Text = "57_lblResumen";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(8, 5);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(302, 25);
            this.masterPrefijo.TabIndex = 0;
            this.masterPrefijo.Value = "";
            // 
            // lblTipoDoc
            // 
            this.lblTipoDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoDoc.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTipoDoc.Location = new System.Drawing.Point(21, 17);
            this.lblTipoDoc.Margin = new System.Windows.Forms.Padding(4);
            this.lblTipoDoc.Name = "lblTipoDoc";
            this.lblTipoDoc.Size = new System.Drawing.Size(77, 14);
            this.lblTipoDoc.TabIndex = 13;
            this.lblTipoDoc.Text = "57_lblTipoDoc";
            // 
            // cmbTipoDoc
            // 
            this.cmbTipoDoc.Location = new System.Drawing.Point(121, 14);
            this.cmbTipoDoc.Name = "cmbTipoDoc";
            this.cmbTipoDoc.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cmbTipoDoc.Properties.Appearance.Options.UseFont = true;
            this.cmbTipoDoc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoDoc.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoDoc.Properties.DisplayMember = "Value";
            this.cmbTipoDoc.Properties.ValueMember = "Key";
            this.cmbTipoDoc.Size = new System.Drawing.Size(159, 20);
            this.cmbTipoDoc.TabIndex = 2;
            this.cmbTipoDoc.EditValueChanged += new System.EventHandler(this.cmbTipoDoc_EditValueChanged);
            // 
            // pnHeader
            // 
            this.pnHeader.Controls.Add(this.masterCosteoGrupo);
            this.pnHeader.Controls.Add(this.btnGetDoc);
            this.pnHeader.Controls.Add(this.masterPrefijo);
            this.pnHeader.Controls.Add(this.lblNro);
            this.pnHeader.Controls.Add(this.txtNro);
            this.pnHeader.Controls.Add(this.btnQueryDoc);
            this.pnHeader.Controls.Add(this.lblEstado);
            this.pnHeader.Controls.Add(this.cmbEstado);
            this.pnHeader.Location = new System.Drawing.Point(13, 38);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(975, 60);
            this.pnHeader.TabIndex = 22;
            // 
            // btnGetDoc
            // 
            this.btnGetDoc.Location = new System.Drawing.Point(888, 9);
            this.btnGetDoc.Name = "btnGetDoc";
            this.btnGetDoc.Size = new System.Drawing.Size(81, 23);
            this.btnGetDoc.TabIndex = 26;
            this.btnGetDoc.Text = "57_btnGetDoc";
            this.btnGetDoc.Visible = false;
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(395, 7);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(26, 21);
            this.btnQueryDoc.TabIndex = 23;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // lblEstado
            // 
            this.lblEstado.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblEstado.Location = new System.Drawing.Point(8, 38);
            this.lblEstado.Margin = new System.Windows.Forms.Padding(4);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(69, 14);
            this.lblEstado.TabIndex = 25;
            this.lblEstado.Text = "57_lblEstado";
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(108, 35);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cmbEstado.Properties.Appearance.Options.UseFont = true;
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbEstado.Properties.DisplayMember = "Value";
            this.cmbEstado.Properties.ValueMember = "Key";
            this.cmbEstado.Size = new System.Drawing.Size(145, 20);
            this.cmbEstado.TabIndex = 24;
            // 
            // masterCosteoGrupo
            // 
            this.masterCosteoGrupo.BackColor = System.Drawing.Color.Transparent;
            this.masterCosteoGrupo.Filtros = null;
            this.masterCosteoGrupo.Location = new System.Drawing.Point(311, 33);
            this.masterCosteoGrupo.Name = "masterCosteoGrupo";
            this.masterCosteoGrupo.Size = new System.Drawing.Size(302, 25);
            this.masterCosteoGrupo.TabIndex = 28;
            this.masterCosteoGrupo.Value = "";
            // 
            // cmbMdaOrigen
            // 
            this.cmbMdaOrigen.Location = new System.Drawing.Point(874, 14);
            this.cmbMdaOrigen.Name = "cmbMdaOrigen";
            this.cmbMdaOrigen.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cmbMdaOrigen.Properties.Appearance.Options.UseFont = true;
            this.cmbMdaOrigen.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbMdaOrigen.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbMdaOrigen.Properties.DisplayMember = "Value";
            this.cmbMdaOrigen.Properties.ValueMember = "Key";
            this.cmbMdaOrigen.Size = new System.Drawing.Size(114, 20);
            this.cmbMdaOrigen.TabIndex = 28;
            this.cmbMdaOrigen.EditValueChanged += new System.EventHandler(this.cmbMdaOrigen_EditValueChanged);
            // 
            // lblMdaOrigen
            // 
            this.lblMdaOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMdaOrigen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblMdaOrigen.Location = new System.Drawing.Point(778, 18);
            this.lblMdaOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.lblMdaOrigen.Name = "lblMdaOrigen";
            this.lblMdaOrigen.Size = new System.Drawing.Size(90, 14);
            this.lblMdaOrigen.TabIndex = 29;
            this.lblMdaOrigen.Text = "57_lblMdaOrigen";
            // 
            // DeterioroRevalorizacionInv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1044, 542);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "DeterioroRevalorizacionInv";
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
            this.gbGridDocument.ResumeLayout(false);
            this.gbGridDocument.PerformLayout();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnHeader)).EndInit();
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMdaOrigen.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.TextBox txtNro;
        private System.Windows.Forms.Label lblNro;
        private System.Windows.Forms.Label lblResumenMvtoImport;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoDoc;
        private DevExpress.XtraEditors.LabelControl lblTipoDoc;
        private DevExpress.XtraEditors.PanelControl pnHeader;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private DevExpress.XtraEditors.LabelControl lblEstado;
        private DevExpress.XtraEditors.SimpleButton btnGetDoc;
        private DevExpress.XtraEditors.LookUpEdit cmbMdaOrigen;
        private DevExpress.XtraEditors.LabelControl lblMdaOrigen;
        private ControlsUC.uc_MasterFind masterCosteoGrupo;
    }
}