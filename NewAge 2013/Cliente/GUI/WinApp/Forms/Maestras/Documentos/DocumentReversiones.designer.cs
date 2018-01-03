namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DocumentReversiones
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
            this.lkpDocumentos = new DevExpress.XtraEditors.LookUpEdit();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.LinkEdit = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.gbFilter = new DevExpress.XtraEditors.GroupControl();
            this.btnQuery = new System.Windows.Forms.Button();
            this.lblPrefijo = new System.Windows.Forms.Label();
            this.txtDocumentoNro = new System.Windows.Forms.TextBox();
            this.lblTercero = new System.Windows.Forms.Label();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtDocTercero = new System.Windows.Forms.TextBox();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaFinal = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaInicial = new System.Windows.Forms.Label();
            this.lblFechaFinal = new System.Windows.Forms.Label();
            this.dtFechaInicial = new DevExpress.XtraEditors.DateEdit();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblObservacion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpDocumentos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilter)).BeginInit();
            this.gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Size = new System.Drawing.Size(989, 153);
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
            this.editLink,
            this.editSpinPorcen,
            this.editSpin0,
            this.LinkEdit});
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
            this.btnMark.Margin = new System.Windows.Forms.Padding(2);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(2);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(2);
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 20);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(2);
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
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(2);
            this.txtPrefix.Size = new System.Drawing.Size(50, 20);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtPeriod
            // 
            this.dtPeriod.Size = new System.Drawing.Size(130, 20);
            // 
            // txtAF
            // 
            this.txtAF.Margin = new System.Windows.Forms.Padding(2);
            this.txtAF.Size = new System.Drawing.Size(91, 20);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(2);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(4, 0, 4, 2);
            this.gbGridDocument.Size = new System.Drawing.Size(741, 272);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(741, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(2);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 272);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.lblObservacion);
            this.grpboxHeader.Controls.Add(this.txtObservacion);
            this.grpboxHeader.Controls.Add(this.dtFechaFinal);
            this.grpboxHeader.Controls.Add(this.gbFilter);
            this.grpboxHeader.Controls.Add(this.lblFechaInicial);
            this.grpboxHeader.Controls.Add(this.lblFechaFinal);
            this.grpboxHeader.Controls.Add(this.lblDocumento);
            this.grpboxHeader.Controls.Add(this.dtFechaInicial);
            this.grpboxHeader.Controls.Add(this.lkpDocumentos);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Size = new System.Drawing.Size(1033, 159);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "c2";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "c0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lkpDocumentos
            // 
            this.lkpDocumentos.Location = new System.Drawing.Point(115, 16);
            this.lkpDocumentos.Name = "lkpDocumentos";
            this.lkpDocumentos.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.lkpDocumentos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpDocumentos.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Descriptivo", "Documento")});
            this.lkpDocumentos.Properties.DisplayMember = "Descriptivo";
            this.lkpDocumentos.Properties.NullText = "";
            this.lkpDocumentos.Properties.ValueMember = "ID";
            this.lkpDocumentos.Size = new System.Drawing.Size(178, 20);
            this.lkpDocumentos.TabIndex = 0;
            this.lkpDocumentos.EditValueChanged += new System.EventHandler(this.lkpDocumentos_EditValueChanged);
            // 
            // lblDocumento
            // 
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumento.Location = new System.Drawing.Point(9, 18);
            this.lblDocumento.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new System.Drawing.Size(117, 14);
            this.lblDocumento.TabIndex = 20;
            this.lblDocumento.Text = "1035_lblDocumento";
            // 
            // LinkEdit
            // 
            this.LinkEdit.Name = "LinkEdit";
            this.LinkEdit.Click += new System.EventHandler(this.LinkEdit_Click);
            // 
            // gbFilter
            // 
            this.gbFilter.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilter.Appearance.Options.UseFont = true;
            this.gbFilter.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilter.AppearanceCaption.Options.UseFont = true;
            this.gbFilter.Controls.Add(this.btnQuery);
            this.gbFilter.Controls.Add(this.lblPrefijo);
            this.gbFilter.Controls.Add(this.txtDocumentoNro);
            this.gbFilter.Controls.Add(this.lblTercero);
            this.gbFilter.Controls.Add(this.masterPrefijo);
            this.gbFilter.Controls.Add(this.txtDocTercero);
            this.gbFilter.Controls.Add(this.masterTercero);
            this.gbFilter.Location = new System.Drawing.Point(407, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(561, 116);
            this.gbFilter.TabIndex = 102;
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuery.Location = new System.Drawing.Point(439, 81);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(99, 23);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "1035_btnQuery";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lblPrefijo
            // 
            this.lblPrefijo.AutoSize = true;
            this.lblPrefijo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefijo.Location = new System.Drawing.Point(367, 27);
            this.lblPrefijo.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblPrefijo.Name = "lblPrefijo";
            this.lblPrefijo.Size = new System.Drawing.Size(106, 14);
            this.lblPrefijo.TabIndex = 104;
            this.lblPrefijo.Text = "1035_lblNroPrefijo";
            // 
            // txtDocumentoNro
            // 
            this.txtDocumentoNro.Location = new System.Drawing.Point(459, 24);
            this.txtDocumentoNro.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.txtDocumentoNro.Name = "txtDocumentoNro";
            this.txtDocumentoNro.Size = new System.Drawing.Size(78, 22);
            this.txtDocumentoNro.TabIndex = 5;
            this.txtDocumentoNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNro_KeyPress);
            // 
            // lblTercero
            // 
            this.lblTercero.AutoSize = true;
            this.lblTercero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTercero.Location = new System.Drawing.Point(367, 52);
            this.lblTercero.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblTercero.Name = "lblTercero";
            this.lblTercero.Size = new System.Drawing.Size(117, 14);
            this.lblTercero.TabIndex = 102;
            this.lblTercero.Text = "1035_lblDocTercero";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(10, 21);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(346, 24);
            this.masterPrefijo.TabIndex = 4;
            this.masterPrefijo.Value = "";
            // 
            // txtDocTercero
            // 
            this.txtDocTercero.Location = new System.Drawing.Point(459, 50);
            this.txtDocTercero.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.txtDocTercero.Name = "txtDocTercero";
            this.txtDocTercero.Size = new System.Drawing.Size(78, 22);
            this.txtDocTercero.TabIndex = 7;
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(10, 46);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(346, 24);
            this.masterTercero.TabIndex = 6;
            this.masterTercero.Value = "";
            // 
            // dtFechaFinal
            // 
            this.dtFechaFinal.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinal.Location = new System.Drawing.Point(294, 50);
            this.dtFechaFinal.Name = "dtFechaFinal";
            this.dtFechaFinal.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinal.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFinal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFinal.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFinal.Size = new System.Drawing.Size(84, 20);
            this.dtFechaFinal.TabIndex = 2;
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.AutoSize = true;
            this.lblFechaInicial.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicial.Location = new System.Drawing.Point(9, 52);
            this.lblFechaInicial.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Size = new System.Drawing.Size(98, 14);
            this.lblFechaInicial.TabIndex = 108;
            this.lblFechaInicial.Text = "1035_lblFechaIni";
            // 
            // lblFechaFinal
            // 
            this.lblFechaFinal.AutoSize = true;
            this.lblFechaFinal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFinal.Location = new System.Drawing.Point(204, 53);
            this.lblFechaFinal.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaFinal.Name = "lblFechaFinal";
            this.lblFechaFinal.Size = new System.Drawing.Size(100, 14);
            this.lblFechaFinal.TabIndex = 110;
            this.lblFechaFinal.Text = "1035_lblFechaFin";
            // 
            // dtFechaInicial
            // 
            this.dtFechaInicial.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicial.Location = new System.Drawing.Point(113, 50);
            this.dtFechaInicial.Name = "dtFechaInicial";
            this.dtFechaInicial.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaInicial.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaInicial.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaInicial.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaInicial.Size = new System.Drawing.Size(84, 20);
            this.dtFechaInicial.TabIndex = 1;
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(12, 90);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(367, 37);
            this.txtObservacion.TabIndex = 111;
            this.txtObservacion.Visible = false;
            // 
            // label1
            // 
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.Location = new System.Drawing.Point(11, 76);
            this.lblObservacion.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(73, 12);
            this.lblObservacion.TabIndex = 112;
            this.lblObservacion.Text = "Observación";
            this.lblObservacion.Visible = false;
            // 
            // DocumentReversiones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1065, 581);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DocumentReversiones";
            this.Text = "";
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpDocumentos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilter)).EndInit();
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).EndInit();
            this.ResumeLayout(false);

        }       
      

        #endregion

        private System.Windows.Forms.Label lblDocumento;
        public DevExpress.XtraEditors.LookUpEdit lkpDocumentos;
        public DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit LinkEdit;
        protected DevExpress.XtraEditors.GroupControl gbFilter;
        protected DevExpress.XtraEditors.DateEdit dtFechaFinal;
        private System.Windows.Forms.Label lblFechaInicial;
        private System.Windows.Forms.Label lblFechaFinal;
        protected DevExpress.XtraEditors.DateEdit dtFechaInicial;
        private System.Windows.Forms.Label lblPrefijo;
        protected System.Windows.Forms.TextBox txtDocumentoNro;
        private System.Windows.Forms.Label lblTercero;
        protected ControlsUC.uc_MasterFind masterPrefijo;
        protected System.Windows.Forms.TextBox txtDocTercero;
        protected ControlsUC.uc_MasterFind masterTercero;
        private System.Windows.Forms.Button btnQuery;
        protected System.Windows.Forms.TextBox txtObservacion;
        protected System.Windows.Forms.Label lblObservacion;
    }
}