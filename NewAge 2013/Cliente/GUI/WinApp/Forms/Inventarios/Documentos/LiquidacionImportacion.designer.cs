namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class LiquidacionImportacion
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.gcDetail = new DevExpress.XtraGrid.GridControl();
            this.gvDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtNro = new System.Windows.Forms.TextBox();
            this.lblNroDocInv = new System.Windows.Forms.Label();
            this.masterAgenteAduanaProv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtTasaImport = new DevExpress.XtraEditors.TextEdit();
            this.lblTasaCambio = new DevExpress.XtraEditors.LabelControl();
            this.lblResumenMvtoImport = new System.Windows.Forms.Label();
            this.txtDocTransporte = new System.Windows.Forms.TextBox();
            this.lblDocTransporte = new System.Windows.Forms.Label();
            this.txtDeclarImportacion = new System.Windows.Forms.TextBox();
            this.lblDeclarImportacion = new System.Windows.Forms.Label();
            this.cmbTipoTransporte = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoTransporte = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaImportacion = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaImportacion = new DevExpress.XtraEditors.DateEdit();
            this.txtDocImportadora = new System.Windows.Forms.TextBox();
            this.lblDocImportadora = new System.Windows.Forms.Label();
            this.lblDocMvtoZonaFranca = new System.Windows.Forms.Label();
            this.txtDocMvtoZonaFranca = new System.Windows.Forms.TextBox();
            this.lblFacturas = new System.Windows.Forms.Label();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.cmbModalidad = new DevExpress.XtraEditors.LookUpEdit();
            this.lblModalidad = new DevExpress.XtraEditors.LabelControl();
            this.grpboxDetail.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaImport.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoTransporte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaImportacion.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaImportacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbModalidad.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.lblFacturas);
            this.grpboxDetail.Controls.Add(this.gcDetail);
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
            this.grpboxHeader.Controls.Add(this.cmbModalidad);
            this.grpboxHeader.Controls.Add(this.lblModalidad);
            this.grpboxHeader.Controls.Add(this.btnQueryDoc);
            this.grpboxHeader.Controls.Add(this.txtObservacion);
            this.grpboxHeader.Controls.Add(this.lblObservacion);
            this.grpboxHeader.Controls.Add(this.txtDocMvtoZonaFranca);
            this.grpboxHeader.Controls.Add(this.lblDocMvtoZonaFranca);
            this.grpboxHeader.Controls.Add(this.txtDocImportadora);
            this.grpboxHeader.Controls.Add(this.lblDocImportadora);
            this.grpboxHeader.Controls.Add(this.lblFechaImportacion);
            this.grpboxHeader.Controls.Add(this.dtFechaImportacion);
            this.grpboxHeader.Controls.Add(this.lblTipoTransporte);
            this.grpboxHeader.Controls.Add(this.cmbTipoTransporte);
            this.grpboxHeader.Controls.Add(this.txtDeclarImportacion);
            this.grpboxHeader.Controls.Add(this.lblDeclarImportacion);
            this.grpboxHeader.Controls.Add(this.txtDocTransporte);
            this.grpboxHeader.Controls.Add(this.lblDocTransporte);
            this.grpboxHeader.Controls.Add(this.txtNro);
            this.grpboxHeader.Controls.Add(this.lblNroDocInv);
            this.grpboxHeader.Controls.Add(this.masterAgenteAduanaProv);
            this.grpboxHeader.Controls.Add(this.txtTasaImport);
            this.grpboxHeader.Controls.Add(this.lblTasaCambio);
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
            // gcDetail
            // 
            this.gcDetail.AllowDrop = true;
            this.gcDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetail.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetail.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetail.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetail.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetail.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetail.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gcDetail.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDetail.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetail.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDetail.Location = new System.Drawing.Point(12, 19);
            this.gcDetail.LookAndFeel.SkinName = "Dark Side";
            this.gcDetail.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetail.MainView = this.gvDetail;
            this.gcDetail.Margin = new System.Windows.Forms.Padding(2);
            this.gcDetail.Name = "gcDetail";
            this.gcDetail.Size = new System.Drawing.Size(998, 142);
            this.gcDetail.TabIndex = 12;
            this.gcDetail.UseEmbeddedNavigator = true;
            this.gcDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetail});
            // 
            // gvDetail
            // 
            this.gvDetail.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetail.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetail.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetail.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetail.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetail.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetail.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetail.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetail.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetail.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetail.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetail.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetail.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetail.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetail.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetail.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetail.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetail.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetail.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetail.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetail.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetail.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetail.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetail.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetail.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetail.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetail.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetail.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetail.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetail.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetail.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetail.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.Row.Options.UseBackColor = true;
            this.gvDetail.Appearance.Row.Options.UseForeColor = true;
            this.gvDetail.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetail.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetail.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetail.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetail.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetail.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetail.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetail.GridControl = this.gcDetail;
            this.gvDetail.HorzScrollStep = 50;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetail.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetail.OptionsBehavior.Editable = false;
            this.gvDetail.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetail.OptionsCustomization.AllowFilter = false;
            this.gvDetail.OptionsCustomization.AllowSort = false;
            this.gvDetail.OptionsMenu.EnableColumnMenu = false;
            this.gvDetail.OptionsMenu.EnableFooterMenu = false;
            this.gvDetail.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetail.OptionsView.ShowGroupPanel = false;
            this.gvDetail.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetail.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetail_CustomUnboundColumnData);
            // 
            // txtNro
            // 
            this.txtNro.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNro.Location = new System.Drawing.Point(294, 12);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(46, 21);
            this.txtNro.TabIndex = 2;
            this.txtNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNro_KeyPress);
            this.txtNro.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // lblNroDocInv
            // 
            this.lblNroDocInv.AutoSize = true;
            this.lblNroDocInv.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroDocInv.Location = new System.Drawing.Point(241, 15);
            this.lblNroDocInv.Name = "lblNroDocInv";
            this.lblNroDocInv.Size = new System.Drawing.Size(58, 14);
            this.lblNroDocInv.TabIndex = 17;
            this.lblNroDocInv.Text = "55_lblNro";
            // 
            // masterProveedor
            // 
            this.masterAgenteAduanaProv.BackColor = System.Drawing.Color.Transparent;
            this.masterAgenteAduanaProv.Filtros = null;
            this.masterAgenteAduanaProv.Location = new System.Drawing.Point(379, 11);
            this.masterAgenteAduanaProv.Name = "masterProveedor";
            this.masterAgenteAduanaProv.Size = new System.Drawing.Size(291, 25);
            this.masterAgenteAduanaProv.TabIndex = 6;
            this.masterAgenteAduanaProv.Value = "";
            this.masterAgenteAduanaProv.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // txtTasaImport
            // 
            this.txtTasaImport.EditValue = 0;
            this.txtTasaImport.Location = new System.Drawing.Point(898, 36);
            this.txtTasaImport.Name = "txtTasaImport";
            this.txtTasaImport.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaImport.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaImport.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaImport.Properties.Mask.EditMask = "c2";
            this.txtTasaImport.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaImport.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaImport.Size = new System.Drawing.Size(87, 20);
            this.txtTasaImport.TabIndex = 11;
            this.txtTasaImport.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // lblTasaCambio
            // 
            this.lblTasaCambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambio.Location = new System.Drawing.Point(794, 38);
            this.lblTasaCambio.Name = "lblTasaCambio";
            this.lblTasaCambio.Size = new System.Drawing.Size(94, 14);
            this.lblTasaCambio.TabIndex = 18;
            this.lblTasaCambio.Text = "55_lblTasaImport";
            // 
            // lblResumenMvtoImport
            // 
            this.lblResumenMvtoImport.AutoSize = true;
            this.lblResumenMvtoImport.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResumenMvtoImport.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumenMvtoImport.Location = new System.Drawing.Point(12, -3);
            this.lblResumenMvtoImport.Name = "lblResumenMvtoImport";
            this.lblResumenMvtoImport.Size = new System.Drawing.Size(173, 14);
            this.lblResumenMvtoImport.TabIndex = 51;
            this.lblResumenMvtoImport.Text = "55_lblResumenMovimiento";
            // 
            // txtDocTransporte
            // 
            this.txtDocTransporte.BackColor = System.Drawing.Color.LightBlue;
            this.txtDocTransporte.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocTransporte.Location = new System.Drawing.Point(150, 11);
            this.txtDocTransporte.Name = "txtDocTransporte";
            this.txtDocTransporte.Size = new System.Drawing.Size(91, 21);
            this.txtDocTransporte.TabIndex = 1;
            this.txtDocTransporte.Leave += new System.EventHandler(this.txtDocTransporte_Leave);
            // 
            // lblDocTransporte
            // 
            this.lblDocTransporte.AutoSize = true;
            this.lblDocTransporte.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocTransporte.Location = new System.Drawing.Point(13, 14);
            this.lblDocTransporte.Name = "lblDocTransporte";
            this.lblDocTransporte.Size = new System.Drawing.Size(120, 14);
            this.lblDocTransporte.TabIndex = 16;
            this.lblDocTransporte.Text = "55_lblDocTransporte";
            // 
            // txtDeclarImportacion
            // 
            this.txtDeclarImportacion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeclarImportacion.Location = new System.Drawing.Point(150, 34);
            this.txtDeclarImportacion.Name = "txtDeclarImportacion";
            this.txtDeclarImportacion.Size = new System.Drawing.Size(91, 21);
            this.txtDeclarImportacion.TabIndex = 3;
            this.txtDeclarImportacion.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // lblDeclarImportacion
            // 
            this.lblDeclarImportacion.AutoSize = true;
            this.lblDeclarImportacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeclarImportacion.Location = new System.Drawing.Point(13, 36);
            this.lblDeclarImportacion.Name = "lblDeclarImportacion";
            this.lblDeclarImportacion.Size = new System.Drawing.Size(137, 14);
            this.lblDeclarImportacion.TabIndex = 15;
            this.lblDeclarImportacion.Text = "55_lblDeclarImportacion";
            // 
            // cmbTipoTransporte
            // 
            this.cmbTipoTransporte.Location = new System.Drawing.Point(480, 36);
            this.cmbTipoTransporte.Name = "cmbTipoTransporte";
            this.cmbTipoTransporte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoTransporte.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoTransporte.Properties.DisplayMember = "Value";
            this.cmbTipoTransporte.Properties.ValueMember = "Key";
            this.cmbTipoTransporte.Size = new System.Drawing.Size(76, 20);
            this.cmbTipoTransporte.TabIndex = 8;
            this.cmbTipoTransporte.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // lblTipoTransporte
            // 
            this.lblTipoTransporte.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoTransporte.Location = new System.Drawing.Point(379, 38);
            this.lblTipoTransporte.Name = "lblTipoTransporte";
            this.lblTipoTransporte.Size = new System.Drawing.Size(116, 14);
            this.lblTipoTransporte.TabIndex = 14;
            this.lblTipoTransporte.Text = "55_lblTipoTransporte";
            // 
            // lblFechaImportacion
            // 
            this.lblFechaImportacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaImportacion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaImportacion.Location = new System.Drawing.Point(794, 16);
            this.lblFechaImportacion.Margin = new System.Windows.Forms.Padding(4);
            this.lblFechaImportacion.Name = "lblFechaImportacion";
            this.lblFechaImportacion.Size = new System.Drawing.Size(129, 14);
            this.lblFechaImportacion.TabIndex = 13;
            this.lblFechaImportacion.Text = "55_lblFechaImportacion";
            // 
            // dtFechaImportacion
            // 
            this.dtFechaImportacion.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaImportacion.Location = new System.Drawing.Point(898, 13);
            this.dtFechaImportacion.Name = "dtFechaImportacion";
            this.dtFechaImportacion.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaImportacion.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaImportacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaImportacion.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaImportacion.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaImportacion.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaImportacion.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaImportacion.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaImportacion.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaImportacion.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaImportacion.Size = new System.Drawing.Size(87, 20);
            this.dtFechaImportacion.TabIndex = 10;
            this.dtFechaImportacion.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // txtDocImportadora
            // 
            this.txtDocImportadora.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocImportadora.Location = new System.Drawing.Point(150, 57);
            this.txtDocImportadora.Name = "txtDocImportadora";
            this.txtDocImportadora.Size = new System.Drawing.Size(91, 21);
            this.txtDocImportadora.TabIndex = 4;
            this.txtDocImportadora.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // lblDocImportadora
            // 
            this.lblDocImportadora.AutoSize = true;
            this.lblDocImportadora.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocImportadora.Location = new System.Drawing.Point(13, 60);
            this.lblDocImportadora.Name = "lblDocImportadora";
            this.lblDocImportadora.Size = new System.Drawing.Size(127, 14);
            this.lblDocImportadora.TabIndex = 12;
            this.lblDocImportadora.Text = "55_lblDocImportadora";
            // 
            // lblDocMvtoZonaFranca
            // 
            this.lblDocMvtoZonaFranca.AutoSize = true;
            this.lblDocMvtoZonaFranca.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocMvtoZonaFranca.Location = new System.Drawing.Point(13, 84);
            this.lblDocMvtoZonaFranca.Name = "lblDocMvtoZonaFranca";
            this.lblDocMvtoZonaFranca.Size = new System.Drawing.Size(149, 14);
            this.lblDocMvtoZonaFranca.TabIndex = 11;
            this.lblDocMvtoZonaFranca.Text = "55_lblDocMvtoZonaFranca";
            // 
            // txtDocMvtoZonaFranca
            // 
            this.txtDocMvtoZonaFranca.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocMvtoZonaFranca.Location = new System.Drawing.Point(150, 80);
            this.txtDocMvtoZonaFranca.Name = "txtDocMvtoZonaFranca";
            this.txtDocMvtoZonaFranca.Size = new System.Drawing.Size(91, 21);
            this.txtDocMvtoZonaFranca.TabIndex = 5;
            this.txtDocMvtoZonaFranca.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // lblFacturas
            // 
            this.lblFacturas.AutoSize = true;
            this.lblFacturas.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblFacturas.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFacturas.Location = new System.Drawing.Point(18, 2);
            this.lblFacturas.Name = "lblFacturas";
            this.lblFacturas.Size = new System.Drawing.Size(96, 14);
            this.lblFacturas.TabIndex = 0;
            this.lblFacturas.Text = "55_lblFacturas";
            // 
            // lblObservacion
            // 
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.Location = new System.Drawing.Point(379, 60);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(105, 14);
            this.lblObservacion.TabIndex = 10;
            this.lblObservacion.Text = "55_lblObservacion";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacion.Location = new System.Drawing.Point(480, 59);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(508, 43);
            this.txtObservacion.TabIndex = 9;
            this.txtObservacion.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(342, 12);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(32, 21);
            this.btnQueryDoc.TabIndex = 25;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // cmbModalidad
            // 
            this.cmbModalidad.Location = new System.Drawing.Point(662, 35);
            this.cmbModalidad.Name = "cmbModalidad";
            this.cmbModalidad.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbModalidad.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbModalidad.Properties.DisplayMember = "Value";
            this.cmbModalidad.Properties.ValueMember = "Key";
            this.cmbModalidad.Size = new System.Drawing.Size(126, 20);
            this.cmbModalidad.TabIndex = 26;
            this.cmbModalidad.Leave += new System.EventHandler(this.txtControlHeader_Leave);
            // 
            // lblModalidad
            // 
            this.lblModalidad.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModalidad.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblModalidad.Location = new System.Drawing.Point(564, 39);
            this.lblModalidad.Margin = new System.Windows.Forms.Padding(4);
            this.lblModalidad.Name = "lblModalidad";
            this.lblModalidad.Size = new System.Drawing.Size(117, 14);
            this.lblModalidad.TabIndex = 27;
            this.lblModalidad.Text = "55_lblModalidadImpor";
            // 
            // LiquidacionImportacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1044, 430);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "LiquidacionImportacion";
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaImport.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoTransporte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaImportacion.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaImportacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbModalidad.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        protected DevExpress.XtraGrid.GridControl gcDetail;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetail;
        private System.Windows.Forms.TextBox txtNro;
        private System.Windows.Forms.Label lblNroDocInv;
        private ControlsUC.uc_MasterFind masterAgenteAduanaProv;
        private DevExpress.XtraEditors.TextEdit txtTasaImport;
        private DevExpress.XtraEditors.LabelControl lblTasaCambio;
        private System.Windows.Forms.Label lblResumenMvtoImport;
        private System.Windows.Forms.TextBox txtDeclarImportacion;
        private System.Windows.Forms.Label lblDeclarImportacion;
        private System.Windows.Forms.TextBox txtDocTransporte;
        private System.Windows.Forms.Label lblDocTransporte;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoTransporte;
        private DevExpress.XtraEditors.LabelControl lblTipoTransporte;
        private System.Windows.Forms.TextBox txtDocImportadora;
        private System.Windows.Forms.Label lblDocImportadora;
        private DevExpress.XtraEditors.LabelControl lblFechaImportacion;
        protected DevExpress.XtraEditors.DateEdit dtFechaImportacion;
        private System.Windows.Forms.TextBox txtDocMvtoZonaFranca;
        private System.Windows.Forms.Label lblDocMvtoZonaFranca;
        private System.Windows.Forms.Label lblFacturas;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Label lblObservacion;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.LookUpEdit cmbModalidad;
        private DevExpress.XtraEditors.LabelControl lblModalidad;
    }
}