namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class GarantiaControl
    {

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
        protected virtual void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.RichText = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupDetalle = new DevExpress.XtraEditors.GroupControl();
            this.pnDatosBasicos = new System.Windows.Forms.Panel();
            this.masterGarantia = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkNuevo = new DevExpress.XtraEditors.CheckEdit();
            this.lblFechaInicio = new System.Windows.Forms.Label();
            this.lblFechaVto = new System.Windows.Forms.Label();
            this.lblTipoGarantia = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaInicio = new DevExpress.XtraEditors.DateEdit();
            this.lblVlrAsegurado = new System.Windows.Forms.Label();
            this.dtFechaVto = new DevExpress.XtraEditors.DateEdit();
            this.txtVlrAsegurado = new DevExpress.XtraEditors.TextEdit();
            this.cmbTipoGarantia = new DevExpress.XtraEditors.LookUpEdit();
            this.lblVlrFuente = new System.Windows.Forms.Label();
            this.txtVlrFuente = new DevExpress.XtraEditors.TextEdit();
            this.gbGarantiaPrendaria = new System.Windows.Forms.GroupBox();
            this.txtPrenda = new System.Windows.Forms.TextBox();
            this.lblPrenda = new DevExpress.XtraEditors.LabelControl();
            this.cmbFuentePre = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbTipoVehiculo = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoVehiculo = new DevExpress.XtraEditors.LabelControl();
            this.txtMotor = new System.Windows.Forms.TextBox();
            this.lblMotor = new DevExpress.XtraEditors.LabelControl();
            this.txtSerie = new System.Windows.Forms.TextBox();
            this.lblSerie = new DevExpress.XtraEditors.LabelControl();
            this.txtChasis = new System.Windows.Forms.TextBox();
            this.lblChasis = new DevExpress.XtraEditors.LabelControl();
            this.lblDatoAdd2 = new DevExpress.XtraEditors.LabelControl();
            this.lblCodigoGaranPre = new DevExpress.XtraEditors.LabelControl();
            this.txtClaseFasecolda = new System.Windows.Forms.TextBox();
            this.masterFaseColda = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDatoAdd1 = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigoGaranPre = new System.Windows.Forms.TextBox();
            this.txtMarcaFasecolda = new System.Windows.Forms.TextBox();
            this.txtModeloPre = new System.Windows.Forms.TextBox();
            this.lblFuentePre = new DevExpress.XtraEditors.LabelControl();
            this.lblModeloPre = new DevExpress.XtraEditors.LabelControl();
            this.gbGarantiaHipotecaria = new System.Windows.Forms.GroupBox();
            this.txtEscritura = new System.Windows.Forms.TextBox();
            this.lblEscritura = new DevExpress.XtraEditors.LabelControl();
            this.cmbTipoInmueble = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoInmueble = new DevExpress.XtraEditors.LabelControl();
            this.cmbFuenteHip = new DevExpress.XtraEditors.LookUpEdit();
            this.lblFuenteHip = new DevExpress.XtraEditors.LabelControl();
            this.lblModeloHip = new DevExpress.XtraEditors.LabelControl();
            this.txtAnoHip = new System.Windows.Forms.TextBox();
            this.lblCodigoGaranHip = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigoGaranHip = new System.Windows.Forms.TextBox();
            this.lblDireccion = new DevExpress.XtraEditors.LabelControl();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gcGarantia = new DevExpress.XtraGrid.GridControl();
            this.gvGarantia = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnDetails = new System.Windows.Forms.Panel();
            this.pnGarantia = new DevExpress.XtraEditors.PanelControl();
            this.lblCorreoGar = new DevExpress.XtraEditors.LabelControl();
            this.txtCorreoGar = new System.Windows.Forms.TextBox();
            this.lblDireccionGar = new DevExpress.XtraEditors.LabelControl();
            this.txtDireccionGar = new System.Windows.Forms.TextBox();
            this.lblTelefonoGar = new DevExpress.XtraEditors.LabelControl();
            this.txtTelefonoGar = new System.Windows.Forms.TextBox();
            this.gbFilterGar = new DevExpress.XtraEditors.GroupControl();
            this.chkActivo = new DevExpress.XtraEditors.CheckEdit();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.lblDocNroGar = new DevExpress.XtraEditors.LabelControl();
            this.lblEstado = new DevExpress.XtraEditors.LabelControl();
            this.txtDocNroGar = new System.Windows.Forms.TextBox();
            this.masterPrefijoGar = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterDocumentoGar = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTerceroGar = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.TbLyPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupDetalle)).BeginInit();
            this.groupDetalle.SuspendLayout();
            this.pnDatosBasicos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkNuevo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVto.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrAsegurado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoGarantia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrFuente.Properties)).BeginInit();
            this.gbGarantiaPrendaria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFuentePre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoVehiculo.Properties)).BeginInit();
            this.gbGarantiaHipotecaria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoInmueble.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFuenteHip.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcGarantia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGarantia)).BeginInit();
            this.pnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnGarantia)).BeginInit();
            this.pnGarantia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGar)).BeginInit();
            this.gbFilterGar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkActivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RichText,
            this.riPopup,
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editLink});
            // 
            // RichText
            // 
            this.RichText.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.WordML;
            this.RichText.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.RichText.Name = "RichText";
            this.RichText.ShowCaretInReadOnly = false;
            // 
            // riPopup
            // 
            this.riPopup.AutoHeight = false;
            this.riPopup.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riPopup.Name = "riPopup";
            this.riPopup.PopupControl = this.PopupContainerControl;
            this.riPopup.PopupFormMinSize = new System.Drawing.Size(500, 0);
            this.riPopup.PopupFormSize = new System.Drawing.Size(500, 300);
            // 
            // PopupContainerControl
            // 
            this.PopupContainerControl.Controls.Add(this.richEditControl);
            this.PopupContainerControl.Location = new System.Drawing.Point(3, 3);
            this.PopupContainerControl.Name = "PopupContainerControl";
            this.PopupContainerControl.Size = new System.Drawing.Size(2, 40);
            this.PopupContainerControl.TabIndex = 5;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(2, 40);
            this.richEditControl.TabIndex = 2;
            this.richEditControl.Text = "myRichEditControl";
            // 
            // editChkBox
            // 
            this.editChkBox.DisplayValueChecked = "True";
            this.editChkBox.DisplayValueUnchecked = "False";
            this.editChkBox.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editChkBox.Name = "editChkBox";
            this.editChkBox.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // editSpin
            // 
            this.editSpin.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editSpin4
            // 
            this.editSpin4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin4.Name = "editSpin4";
            // 
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.SingleClick = true;
            this.editLink.Click += new System.EventHandler(this.linkVer_Click);
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.269841F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.73016F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.TbLyPanel.Controls.Add(this.panel4, 1, 2);
            this.TbLyPanel.Controls.Add(this.panel2, 1, 1);
            this.TbLyPanel.Controls.Add(this.PopupContainerControl, 0, 0);
            this.TbLyPanel.Controls.Add(this.pnDetails, 1, 0);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 3;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 139F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 212F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TbLyPanel.Size = new System.Drawing.Size(1089, 662);
            this.TbLyPanel.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupDetalle);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(15, 353);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1057, 307);
            this.panel4.TabIndex = 11;
            // 
            // groupDetalle
            // 
            this.groupDetalle.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupDetalle.Appearance.Options.UseFont = true;
            this.groupDetalle.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupDetalle.AppearanceCaption.Options.UseFont = true;
            this.groupDetalle.Controls.Add(this.pnDatosBasicos);
            this.groupDetalle.Controls.Add(this.gbGarantiaPrendaria);
            this.groupDetalle.Controls.Add(this.gbGarantiaHipotecaria);
            this.groupDetalle.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupDetalle.Location = new System.Drawing.Point(0, 0);
            this.groupDetalle.Name = "groupDetalle";
            this.groupDetalle.Size = new System.Drawing.Size(1054, 307);
            this.groupDetalle.TabIndex = 113;
            this.groupDetalle.Text = "2_gbDetalle";
            // 
            // pnDatosBasicos
            // 
            this.pnDatosBasicos.BackColor = System.Drawing.Color.Transparent;
            this.pnDatosBasicos.Controls.Add(this.masterGarantia);
            this.pnDatosBasicos.Controls.Add(this.chkNuevo);
            this.pnDatosBasicos.Controls.Add(this.lblFechaInicio);
            this.pnDatosBasicos.Controls.Add(this.lblFechaVto);
            this.pnDatosBasicos.Controls.Add(this.lblTipoGarantia);
            this.pnDatosBasicos.Controls.Add(this.dtFechaInicio);
            this.pnDatosBasicos.Controls.Add(this.lblVlrAsegurado);
            this.pnDatosBasicos.Controls.Add(this.dtFechaVto);
            this.pnDatosBasicos.Controls.Add(this.txtVlrAsegurado);
            this.pnDatosBasicos.Controls.Add(this.cmbTipoGarantia);
            this.pnDatosBasicos.Controls.Add(this.lblVlrFuente);
            this.pnDatosBasicos.Controls.Add(this.txtVlrFuente);
            this.pnDatosBasicos.Enabled = false;
            this.pnDatosBasicos.Location = new System.Drawing.Point(5, 25);
            this.pnDatosBasicos.Name = "pnDatosBasicos";
            this.pnDatosBasicos.Size = new System.Drawing.Size(347, 256);
            this.pnDatosBasicos.TabIndex = 128;
            // 
            // masterGarantia
            // 
            this.masterGarantia.BackColor = System.Drawing.Color.Transparent;
            this.masterGarantia.Filtros = null;
            this.masterGarantia.Location = new System.Drawing.Point(3, 3);
            this.masterGarantia.Name = "masterGarantia";
            this.masterGarantia.Size = new System.Drawing.Size(352, 28);
            this.masterGarantia.TabIndex = 0;
            this.masterGarantia.Value = "";
            this.masterGarantia.Leave += new System.EventHandler(this.masterGarantia_Leave);
            // 
            // chkNuevo
            // 
            this.chkNuevo.EditValue = true;
            this.chkNuevo.Location = new System.Drawing.Point(255, 35);
            this.chkNuevo.Name = "chkNuevo";
            this.chkNuevo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkNuevo.Properties.Appearance.Options.UseFont = true;
            this.chkNuevo.Properties.Caption = "2_chkNuevo";
            this.chkNuevo.Size = new System.Drawing.Size(93, 19);
            this.chkNuevo.TabIndex = 127;
            this.chkNuevo.CheckedChanged += new System.EventHandler(this.chkNuevo_CheckedChanged);
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.AutoSize = true;
            this.lblFechaInicio.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaInicio.Location = new System.Drawing.Point(2, 62);
            this.lblFechaInicio.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(77, 14);
            this.lblFechaInicio.TabIndex = 109;
            this.lblFechaInicio.Text = "2_lblFechaIni";
            // 
            // lblFechaVto
            // 
            this.lblFechaVto.AutoSize = true;
            this.lblFechaVto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaVto.Location = new System.Drawing.Point(2, 86);
            this.lblFechaVto.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblFechaVto.Name = "lblFechaVto";
            this.lblFechaVto.Size = new System.Drawing.Size(84, 14);
            this.lblFechaVto.TabIndex = 111;
            this.lblFechaVto.Text = "2_lblFechaVto";
            // 
            // lblTipoGarantia
            // 
            this.lblTipoGarantia.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTipoGarantia.Location = new System.Drawing.Point(2, 38);
            this.lblTipoGarantia.Name = "lblTipoGarantia";
            this.lblTipoGarantia.Size = new System.Drawing.Size(93, 14);
            this.lblTipoGarantia.TabIndex = 107;
            this.lblTipoGarantia.Text = "2_lblTipoGarantia";
            // 
            // dtFechaInicio
            // 
            this.dtFechaInicio.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicio.Location = new System.Drawing.Point(120, 59);
            this.dtFechaInicio.Name = "dtFechaInicio";
            this.dtFechaInicio.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaInicio.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dtFechaInicio.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaInicio.Properties.Appearance.Options.UseFont = true;
            this.dtFechaInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaInicio.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaInicio.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicio.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicio.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaInicio.Size = new System.Drawing.Size(126, 20);
            this.dtFechaInicio.TabIndex = 2;
            this.dtFechaInicio.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblVlrAsegurado
            // 
            this.lblVlrAsegurado.AutoSize = true;
            this.lblVlrAsegurado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrAsegurado.Location = new System.Drawing.Point(2, 139);
            this.lblVlrAsegurado.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblVlrAsegurado.Name = "lblVlrAsegurado";
            this.lblVlrAsegurado.Size = new System.Drawing.Size(104, 14);
            this.lblVlrAsegurado.TabIndex = 121;
            this.lblVlrAsegurado.Text = "2_lblVlrAsegurado";
            // 
            // dtFechaVto
            // 
            this.dtFechaVto.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaVto.Location = new System.Drawing.Point(120, 83);
            this.dtFechaVto.Name = "dtFechaVto";
            this.dtFechaVto.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaVto.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dtFechaVto.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaVto.Properties.Appearance.Options.UseFont = true;
            this.dtFechaVto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaVto.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaVto.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaVto.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaVto.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaVto.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaVto.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaVto.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaVto.Size = new System.Drawing.Size(126, 20);
            this.dtFechaVto.TabIndex = 3;
            this.dtFechaVto.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // txtVlrAsegurado
            // 
            this.txtVlrAsegurado.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtVlrAsegurado.Location = new System.Drawing.Point(122, 136);
            this.txtVlrAsegurado.Name = "txtVlrAsegurado";
            this.txtVlrAsegurado.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtVlrAsegurado.Properties.Appearance.Options.UseFont = true;
            this.txtVlrAsegurado.Properties.Mask.EditMask = "c2";
            this.txtVlrAsegurado.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrAsegurado.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrAsegurado.Size = new System.Drawing.Size(124, 20);
            this.txtVlrAsegurado.TabIndex = 5;
            this.txtVlrAsegurado.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // cmbTipoGarantia
            // 
            this.cmbTipoGarantia.Location = new System.Drawing.Point(120, 35);
            this.cmbTipoGarantia.Name = "cmbTipoGarantia";
            this.cmbTipoGarantia.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.cmbTipoGarantia.Properties.Appearance.Options.UseFont = true;
            this.cmbTipoGarantia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoGarantia.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoGarantia.Properties.DisplayMember = "Value";
            this.cmbTipoGarantia.Properties.ReadOnly = true;
            this.cmbTipoGarantia.Properties.ValueMember = "Key";
            this.cmbTipoGarantia.Size = new System.Drawing.Size(126, 20);
            this.cmbTipoGarantia.TabIndex = 1;
            this.cmbTipoGarantia.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblVlrFuente
            // 
            this.lblVlrFuente.AutoSize = true;
            this.lblVlrFuente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrFuente.Location = new System.Drawing.Point(2, 112);
            this.lblVlrFuente.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblVlrFuente.Name = "lblVlrFuente";
            this.lblVlrFuente.Size = new System.Drawing.Size(98, 14);
            this.lblVlrFuente.TabIndex = 119;
            this.lblVlrFuente.Text = "2_lblValorFuente";
            // 
            // txtVlrFuente
            // 
            this.txtVlrFuente.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtVlrFuente.Location = new System.Drawing.Point(121, 109);
            this.txtVlrFuente.Name = "txtVlrFuente";
            this.txtVlrFuente.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtVlrFuente.Properties.Appearance.Options.UseFont = true;
            this.txtVlrFuente.Properties.Mask.EditMask = "c2";
            this.txtVlrFuente.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrFuente.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrFuente.Size = new System.Drawing.Size(124, 20);
            this.txtVlrFuente.TabIndex = 4;
            this.txtVlrFuente.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // gbGarantiaPrendaria
            // 
            this.gbGarantiaPrendaria.Controls.Add(this.txtPrenda);
            this.gbGarantiaPrendaria.Controls.Add(this.lblPrenda);
            this.gbGarantiaPrendaria.Controls.Add(this.cmbFuentePre);
            this.gbGarantiaPrendaria.Controls.Add(this.cmbTipoVehiculo);
            this.gbGarantiaPrendaria.Controls.Add(this.lblTipoVehiculo);
            this.gbGarantiaPrendaria.Controls.Add(this.txtMotor);
            this.gbGarantiaPrendaria.Controls.Add(this.lblMotor);
            this.gbGarantiaPrendaria.Controls.Add(this.txtSerie);
            this.gbGarantiaPrendaria.Controls.Add(this.lblSerie);
            this.gbGarantiaPrendaria.Controls.Add(this.txtChasis);
            this.gbGarantiaPrendaria.Controls.Add(this.lblChasis);
            this.gbGarantiaPrendaria.Controls.Add(this.lblDatoAdd2);
            this.gbGarantiaPrendaria.Controls.Add(this.lblCodigoGaranPre);
            this.gbGarantiaPrendaria.Controls.Add(this.txtClaseFasecolda);
            this.gbGarantiaPrendaria.Controls.Add(this.masterFaseColda);
            this.gbGarantiaPrendaria.Controls.Add(this.lblDatoAdd1);
            this.gbGarantiaPrendaria.Controls.Add(this.txtCodigoGaranPre);
            this.gbGarantiaPrendaria.Controls.Add(this.txtMarcaFasecolda);
            this.gbGarantiaPrendaria.Controls.Add(this.txtModeloPre);
            this.gbGarantiaPrendaria.Controls.Add(this.lblFuentePre);
            this.gbGarantiaPrendaria.Controls.Add(this.lblModeloPre);
            this.gbGarantiaPrendaria.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbGarantiaPrendaria.Location = new System.Drawing.Point(358, 28);
            this.gbGarantiaPrendaria.Name = "gbGarantiaPrendaria";
            this.gbGarantiaPrendaria.Size = new System.Drawing.Size(404, 227);
            this.gbGarantiaPrendaria.TabIndex = 122;
            this.gbGarantiaPrendaria.TabStop = false;
            this.gbGarantiaPrendaria.Text = "2_gbGarantiaPre";
            // 
            // txtPrenda
            // 
            this.txtPrenda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrenda.Location = new System.Drawing.Point(307, 22);
            this.txtPrenda.Name = "txtPrenda";
            this.txtPrenda.Size = new System.Drawing.Size(91, 22);
            this.txtPrenda.TabIndex = 133;
            this.txtPrenda.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblPrenda
            // 
            this.lblPrenda.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrenda.Location = new System.Drawing.Point(244, 26);
            this.lblPrenda.Name = "lblPrenda";
            this.lblPrenda.Size = new System.Drawing.Size(63, 14);
            this.lblPrenda.TabIndex = 134;
            this.lblPrenda.Text = "2_lblPrenda";
            // 
            // cmbFuentePre
            // 
            this.cmbFuentePre.Location = new System.Drawing.Point(131, 67);
            this.cmbFuentePre.Name = "cmbFuentePre";
            this.cmbFuentePre.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cmbFuentePre.Properties.Appearance.Options.UseFont = true;
            this.cmbFuentePre.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFuentePre.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbFuentePre.Properties.DisplayMember = "Value";
            this.cmbFuentePre.Properties.ValueMember = "Key";
            this.cmbFuentePre.Size = new System.Drawing.Size(105, 20);
            this.cmbFuentePre.TabIndex = 2;
            this.cmbFuentePre.EditValueChanged += new System.EventHandler(this.cmbFuentePre_EditValueChanged);
            this.cmbFuentePre.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // cmbTipoVehiculo
            // 
            this.cmbTipoVehiculo.Location = new System.Drawing.Point(130, 163);
            this.cmbTipoVehiculo.Name = "cmbTipoVehiculo";
            this.cmbTipoVehiculo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cmbTipoVehiculo.Properties.Appearance.Options.UseFont = true;
            this.cmbTipoVehiculo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoVehiculo.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoVehiculo.Properties.DisplayMember = "Value";
            this.cmbTipoVehiculo.Properties.ValueMember = "Key";
            this.cmbTipoVehiculo.Size = new System.Drawing.Size(105, 20);
            this.cmbTipoVehiculo.TabIndex = 6;
            this.cmbTipoVehiculo.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblTipoVehiculo
            // 
            this.lblTipoVehiculo.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTipoVehiculo.Location = new System.Drawing.Point(14, 166);
            this.lblTipoVehiculo.Name = "lblTipoVehiculo";
            this.lblTipoVehiculo.Size = new System.Drawing.Size(95, 14);
            this.lblTipoVehiculo.TabIndex = 132;
            this.lblTipoVehiculo.Text = "2_lblTipoVehiculo";
            // 
            // txtMotor
            // 
            this.txtMotor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMotor.Location = new System.Drawing.Point(308, 187);
            this.txtMotor.Name = "txtMotor";
            this.txtMotor.Size = new System.Drawing.Size(91, 22);
            this.txtMotor.TabIndex = 9;
            this.txtMotor.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblMotor
            // 
            this.lblMotor.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMotor.Location = new System.Drawing.Point(245, 190);
            this.lblMotor.Name = "lblMotor";
            this.lblMotor.Size = new System.Drawing.Size(57, 14);
            this.lblMotor.TabIndex = 131;
            this.lblMotor.Text = "2_lblMotor";
            // 
            // txtSerie
            // 
            this.txtSerie.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerie.Location = new System.Drawing.Point(308, 163);
            this.txtSerie.Name = "txtSerie";
            this.txtSerie.Size = new System.Drawing.Size(91, 22);
            this.txtSerie.TabIndex = 8;
            this.txtSerie.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblSerie
            // 
            this.lblSerie.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSerie.Location = new System.Drawing.Point(245, 167);
            this.lblSerie.Name = "lblSerie";
            this.lblSerie.Size = new System.Drawing.Size(52, 14);
            this.lblSerie.TabIndex = 129;
            this.lblSerie.Text = "2_lblSerie";
            // 
            // txtChasis
            // 
            this.txtChasis.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChasis.Location = new System.Drawing.Point(130, 185);
            this.txtChasis.Name = "txtChasis";
            this.txtChasis.Size = new System.Drawing.Size(105, 22);
            this.txtChasis.TabIndex = 7;
            this.txtChasis.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblChasis
            // 
            this.lblChasis.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChasis.Location = new System.Drawing.Point(15, 189);
            this.lblChasis.Name = "lblChasis";
            this.lblChasis.Size = new System.Drawing.Size(57, 14);
            this.lblChasis.TabIndex = 127;
            this.lblChasis.Text = "2_lblChasis";
            // 
            // lblDatoAdd2
            // 
            this.lblDatoAdd2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatoAdd2.Location = new System.Drawing.Point(13, 143);
            this.lblDatoAdd2.Name = "lblDatoAdd2";
            this.lblDatoAdd2.Size = new System.Drawing.Size(80, 14);
            this.lblDatoAdd2.TabIndex = 124;
            this.lblDatoAdd2.Text = "2_lblDatoAdd2";
            // 
            // lblCodigoGaranPre
            // 
            this.lblCodigoGaranPre.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoGaranPre.Location = new System.Drawing.Point(13, 22);
            this.lblCodigoGaranPre.Name = "lblCodigoGaranPre";
            this.lblCodigoGaranPre.Size = new System.Drawing.Size(111, 14);
            this.lblCodigoGaranPre.TabIndex = 114;
            this.lblCodigoGaranPre.Text = "2_lblCodigoGaranPre";
            // 
            // txtClaseFasecolda
            // 
            this.txtClaseFasecolda.Enabled = false;
            this.txtClaseFasecolda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClaseFasecolda.Location = new System.Drawing.Point(131, 139);
            this.txtClaseFasecolda.Name = "txtClaseFasecolda";
            this.txtClaseFasecolda.Size = new System.Drawing.Size(106, 22);
            this.txtClaseFasecolda.TabIndex = 5;
            // 
            // masterFaseColda
            // 
            this.masterFaseColda.BackColor = System.Drawing.Color.Transparent;
            this.masterFaseColda.Filtros = null;
            this.masterFaseColda.Location = new System.Drawing.Point(14, 88);
            this.masterFaseColda.Name = "masterFaseColda";
            this.masterFaseColda.Size = new System.Drawing.Size(409, 26);
            this.masterFaseColda.TabIndex = 3;
            this.masterFaseColda.Value = "";
            this.masterFaseColda.Leave += new System.EventHandler(this.masterGarantia_Leave);
            // 
            // lblDatoAdd1
            // 
            this.lblDatoAdd1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatoAdd1.Location = new System.Drawing.Point(13, 119);
            this.lblDatoAdd1.Name = "lblDatoAdd1";
            this.lblDatoAdd1.Size = new System.Drawing.Size(80, 14);
            this.lblDatoAdd1.TabIndex = 122;
            this.lblDatoAdd1.Text = "2_lblDatoAdd1";
            // 
            // txtCodigoGaranPre
            // 
            this.txtCodigoGaranPre.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoGaranPre.Location = new System.Drawing.Point(131, 18);
            this.txtCodigoGaranPre.Name = "txtCodigoGaranPre";
            this.txtCodigoGaranPre.Size = new System.Drawing.Size(106, 22);
            this.txtCodigoGaranPre.TabIndex = 0;
            this.txtCodigoGaranPre.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // txtMarcaFasecolda
            // 
            this.txtMarcaFasecolda.Enabled = false;
            this.txtMarcaFasecolda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMarcaFasecolda.Location = new System.Drawing.Point(131, 115);
            this.txtMarcaFasecolda.Name = "txtMarcaFasecolda";
            this.txtMarcaFasecolda.Size = new System.Drawing.Size(106, 22);
            this.txtMarcaFasecolda.TabIndex = 4;
            // 
            // txtModeloPre
            // 
            this.txtModeloPre.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtModeloPre.Location = new System.Drawing.Point(131, 42);
            this.txtModeloPre.Name = "txtModeloPre";
            this.txtModeloPre.Size = new System.Drawing.Size(106, 22);
            this.txtModeloPre.TabIndex = 1;
            this.txtModeloPre.TextChanged += new System.EventHandler(this.txt_TextChanged);
            this.txtModeloPre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // lblFuentePre
            // 
            this.lblFuentePre.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFuentePre.Location = new System.Drawing.Point(13, 69);
            this.lblFuentePre.Name = "lblFuentePre";
            this.lblFuentePre.Size = new System.Drawing.Size(82, 14);
            this.lblFuentePre.TabIndex = 118;
            this.lblFuentePre.Text = "2_lblFuentePre";
            // 
            // lblModeloPre
            // 
            this.lblModeloPre.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModeloPre.Location = new System.Drawing.Point(13, 46);
            this.lblModeloPre.Name = "lblModeloPre";
            this.lblModeloPre.Size = new System.Drawing.Size(82, 14);
            this.lblModeloPre.TabIndex = 116;
            this.lblModeloPre.Text = "2_lblModeloPre";
            // 
            // gbGarantiaHipotecaria
            // 
            this.gbGarantiaHipotecaria.Controls.Add(this.txtEscritura);
            this.gbGarantiaHipotecaria.Controls.Add(this.lblEscritura);
            this.gbGarantiaHipotecaria.Controls.Add(this.cmbTipoInmueble);
            this.gbGarantiaHipotecaria.Controls.Add(this.lblTipoInmueble);
            this.gbGarantiaHipotecaria.Controls.Add(this.cmbFuenteHip);
            this.gbGarantiaHipotecaria.Controls.Add(this.lblFuenteHip);
            this.gbGarantiaHipotecaria.Controls.Add(this.lblModeloHip);
            this.gbGarantiaHipotecaria.Controls.Add(this.txtAnoHip);
            this.gbGarantiaHipotecaria.Controls.Add(this.lblCodigoGaranHip);
            this.gbGarantiaHipotecaria.Controls.Add(this.txtCodigoGaranHip);
            this.gbGarantiaHipotecaria.Controls.Add(this.lblDireccion);
            this.gbGarantiaHipotecaria.Controls.Add(this.txtDireccion);
            this.gbGarantiaHipotecaria.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbGarantiaHipotecaria.Location = new System.Drawing.Point(764, 28);
            this.gbGarantiaHipotecaria.Name = "gbGarantiaHipotecaria";
            this.gbGarantiaHipotecaria.Size = new System.Drawing.Size(240, 227);
            this.gbGarantiaHipotecaria.TabIndex = 123;
            this.gbGarantiaHipotecaria.TabStop = false;
            this.gbGarantiaHipotecaria.Text = "2_gbGarantiaHip";
            // 
            // txtEscritura
            // 
            this.txtEscritura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEscritura.Location = new System.Drawing.Point(122, 143);
            this.txtEscritura.Name = "txtEscritura";
            this.txtEscritura.Size = new System.Drawing.Size(105, 22);
            this.txtEscritura.TabIndex = 137;
            this.txtEscritura.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblEscritura
            // 
            this.lblEscritura.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEscritura.Location = new System.Drawing.Point(6, 146);
            this.lblEscritura.Name = "lblEscritura";
            this.lblEscritura.Size = new System.Drawing.Size(71, 14);
            this.lblEscritura.TabIndex = 138;
            this.lblEscritura.Text = "2_lblEscritura";
            // 
            // cmbTipoInmueble
            // 
            this.cmbTipoInmueble.Location = new System.Drawing.Point(122, 46);
            this.cmbTipoInmueble.Name = "cmbTipoInmueble";
            this.cmbTipoInmueble.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cmbTipoInmueble.Properties.Appearance.Options.UseFont = true;
            this.cmbTipoInmueble.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoInmueble.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoInmueble.Properties.DisplayMember = "Value";
            this.cmbTipoInmueble.Properties.ValueMember = "Key";
            this.cmbTipoInmueble.Size = new System.Drawing.Size(105, 20);
            this.cmbTipoInmueble.TabIndex = 1;
            this.cmbTipoInmueble.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblTipoInmueble
            // 
            this.lblTipoInmueble.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoInmueble.Location = new System.Drawing.Point(6, 50);
            this.lblTipoInmueble.Name = "lblTipoInmueble";
            this.lblTipoInmueble.Size = new System.Drawing.Size(100, 14);
            this.lblTipoInmueble.TabIndex = 136;
            this.lblTipoInmueble.Text = "2_lblTipoInmueble";
            // 
            // cmbFuenteHip
            // 
            this.cmbFuenteHip.Location = new System.Drawing.Point(122, 119);
            this.cmbFuenteHip.Name = "cmbFuenteHip";
            this.cmbFuenteHip.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.cmbFuenteHip.Properties.Appearance.Options.UseFont = true;
            this.cmbFuenteHip.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFuenteHip.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbFuenteHip.Properties.DisplayMember = "Value";
            this.cmbFuenteHip.Properties.ValueMember = "Key";
            this.cmbFuenteHip.Size = new System.Drawing.Size(105, 20);
            this.cmbFuenteHip.TabIndex = 4;
            this.cmbFuenteHip.EditValueChanged += new System.EventHandler(this.txtFuenteHip_EditValueChanged);
            this.cmbFuenteHip.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblFuenteHip
            // 
            this.lblFuenteHip.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFuenteHip.Location = new System.Drawing.Point(5, 124);
            this.lblFuenteHip.Name = "lblFuenteHip";
            this.lblFuenteHip.Size = new System.Drawing.Size(81, 14);
            this.lblFuenteHip.TabIndex = 134;
            this.lblFuenteHip.Text = "2_lblFuenteHip";
            // 
            // lblModeloHip
            // 
            this.lblModeloHip.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModeloHip.Location = new System.Drawing.Point(5, 98);
            this.lblModeloHip.Name = "lblModeloHip";
            this.lblModeloHip.Size = new System.Drawing.Size(81, 14);
            this.lblModeloHip.TabIndex = 132;
            this.lblModeloHip.Text = "2_lblModeloHip";
            // 
            // txtAnoHip
            // 
            this.txtAnoHip.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAnoHip.Location = new System.Drawing.Point(122, 94);
            this.txtAnoHip.Name = "txtAnoHip";
            this.txtAnoHip.Size = new System.Drawing.Size(106, 22);
            this.txtAnoHip.TabIndex = 3;
            this.txtAnoHip.TextChanged += new System.EventHandler(this.txt_TextChanged);
            this.txtAnoHip.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            // 
            // lblCodigoGaranHip
            // 
            this.lblCodigoGaranHip.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoGaranHip.Location = new System.Drawing.Point(6, 25);
            this.lblCodigoGaranHip.Name = "lblCodigoGaranHip";
            this.lblCodigoGaranHip.Size = new System.Drawing.Size(110, 14);
            this.lblCodigoGaranHip.TabIndex = 130;
            this.lblCodigoGaranHip.Text = "2_lblCodigoGaranHip";
            // 
            // txtCodigoGaranHip
            // 
            this.txtCodigoGaranHip.Location = new System.Drawing.Point(122, 21);
            this.txtCodigoGaranHip.Name = "txtCodigoGaranHip";
            this.txtCodigoGaranHip.Size = new System.Drawing.Size(106, 22);
            this.txtCodigoGaranHip.TabIndex = 0;
            this.txtCodigoGaranHip.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // lblDireccion
            // 
            this.lblDireccion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccion.Location = new System.Drawing.Point(6, 73);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(74, 14);
            this.lblDireccion.TabIndex = 128;
            this.lblDireccion.Text = "2_lblDireccion";
            // 
            // txtDireccion
            // 
            this.txtDireccion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDireccion.Location = new System.Drawing.Point(122, 69);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(106, 22);
            this.txtDireccion.TabIndex = 2;
            this.txtDireccion.TextChanged += new System.EventHandler(this.txt_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gcGarantia);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(15, 141);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1057, 208);
            this.panel2.TabIndex = 11;
            // 
            // gcGarantia
            // 
            this.gcGarantia.AllowDrop = true;
            this.gcGarantia.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcGarantia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcGarantia.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcGarantia.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcGarantia.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcGarantia.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcGarantia.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcGarantia.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcGarantia.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcGarantia.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcGarantia.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcGarantia.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcGarantia.EmbeddedNavigator.Buttons.NextPage.Enabled = false;
            this.gcGarantia.EmbeddedNavigator.Buttons.PrevPage.Enabled = false;
            this.gcGarantia.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.gcGarantia.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcGarantia.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcGarantia.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcGarantia.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcGarantia.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gvGarantia_EmbeddedNavigator_ButtonClick);
            this.gcGarantia.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcGarantia.Location = new System.Drawing.Point(0, 0);
            this.gcGarantia.LookAndFeel.SkinName = "Dark Side";
            this.gcGarantia.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcGarantia.MainView = this.gvGarantia;
            this.gcGarantia.Margin = new System.Windows.Forms.Padding(4);
            this.gcGarantia.Name = "gcGarantia";
            this.gcGarantia.Size = new System.Drawing.Size(1057, 208);
            this.gcGarantia.TabIndex = 1;
            this.gcGarantia.UseEmbeddedNavigator = true;
            this.gcGarantia.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvGarantia});
            // 
            // gvGarantia
            // 
            this.gvGarantia.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGarantia.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvGarantia.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvGarantia.Appearance.Empty.Options.UseBackColor = true;
            this.gvGarantia.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvGarantia.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvGarantia.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvGarantia.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvGarantia.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvGarantia.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvGarantia.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvGarantia.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvGarantia.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvGarantia.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvGarantia.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGarantia.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvGarantia.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvGarantia.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvGarantia.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvGarantia.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvGarantia.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvGarantia.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvGarantia.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvGarantia.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvGarantia.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvGarantia.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvGarantia.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGarantia.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvGarantia.Appearance.Row.Options.UseBackColor = true;
            this.gvGarantia.Appearance.Row.Options.UseFont = true;
            this.gvGarantia.Appearance.Row.Options.UseForeColor = true;
            this.gvGarantia.Appearance.Row.Options.UseTextOptions = true;
            this.gvGarantia.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvGarantia.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvGarantia.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvGarantia.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvGarantia.GridControl = this.gcGarantia;
            this.gvGarantia.HorzScrollStep = 50;
            this.gvGarantia.Name = "gvGarantia";
            this.gvGarantia.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvGarantia.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvGarantia.OptionsCustomization.AllowColumnMoving = false;
            this.gvGarantia.OptionsCustomization.AllowFilter = false;
            this.gvGarantia.OptionsCustomization.AllowSort = false;
            this.gvGarantia.OptionsFind.AllowFindPanel = false;
            this.gvGarantia.OptionsMenu.EnableColumnMenu = false;
            this.gvGarantia.OptionsMenu.EnableFooterMenu = false;
            this.gvGarantia.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvGarantia.OptionsView.ColumnAutoWidth = false;
            this.gvGarantia.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvGarantia.OptionsView.ShowGroupPanel = false;
            this.gvGarantia.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvGarantia.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvGarantia_FocusedRowChanged);
            this.gvGarantia.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvGarantia_CellValueChanged);
            this.gvGarantia.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvGarantia_BeforeLeaveRow);
            this.gvGarantia.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvGarantia_CustomUnboundColumnData);
            this.gvGarantia.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvGarantia_CustomColumnDisplayText);
            // 
            // pnDetails
            // 
            this.pnDetails.Controls.Add(this.pnGarantia);
            this.pnDetails.Location = new System.Drawing.Point(16, 3);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(920, 133);
            this.pnDetails.TabIndex = 6;
            // 
            // pnGarantia
            // 
            this.pnGarantia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnGarantia.Controls.Add(this.lblCorreoGar);
            this.pnGarantia.Controls.Add(this.txtCorreoGar);
            this.pnGarantia.Controls.Add(this.lblDireccionGar);
            this.pnGarantia.Controls.Add(this.txtDireccionGar);
            this.pnGarantia.Controls.Add(this.lblTelefonoGar);
            this.pnGarantia.Controls.Add(this.txtTelefonoGar);
            this.pnGarantia.Controls.Add(this.gbFilterGar);
            this.pnGarantia.Location = new System.Drawing.Point(0, 0);
            this.pnGarantia.Name = "pnGarantia";
            this.pnGarantia.Size = new System.Drawing.Size(917, 128);
            this.pnGarantia.TabIndex = 103;
            // 
            // lblCorreoGar
            // 
            this.lblCorreoGar.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorreoGar.Location = new System.Drawing.Point(494, 106);
            this.lblCorreoGar.Name = "lblCorreoGar";
            this.lblCorreoGar.Size = new System.Drawing.Size(61, 14);
            this.lblCorreoGar.TabIndex = 108;
            this.lblCorreoGar.Text = "2_lblCorreo";
            // 
            // txtCorreoGar
            // 
            this.txtCorreoGar.Location = new System.Drawing.Point(560, 102);
            this.txtCorreoGar.Name = "txtCorreoGar";
            this.txtCorreoGar.ReadOnly = true;
            this.txtCorreoGar.Size = new System.Drawing.Size(149, 21);
            this.txtCorreoGar.TabIndex = 107;
            // 
            // lblDireccionGar
            // 
            this.lblDireccionGar.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDireccionGar.Location = new System.Drawing.Point(231, 106);
            this.lblDireccionGar.Name = "lblDireccionGar";
            this.lblDireccionGar.Size = new System.Drawing.Size(74, 14);
            this.lblDireccionGar.TabIndex = 106;
            this.lblDireccionGar.Text = "2_lblDireccion";
            // 
            // txtDireccionGar
            // 
            this.txtDireccionGar.Location = new System.Drawing.Point(307, 102);
            this.txtDireccionGar.Name = "txtDireccionGar";
            this.txtDireccionGar.ReadOnly = true;
            this.txtDireccionGar.Size = new System.Drawing.Size(132, 21);
            this.txtDireccionGar.TabIndex = 105;
            // 
            // lblTelefonoGar
            // 
            this.lblTelefonoGar.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefonoGar.Location = new System.Drawing.Point(15, 106);
            this.lblTelefonoGar.Name = "lblTelefonoGar";
            this.lblTelefonoGar.Size = new System.Drawing.Size(74, 14);
            this.lblTelefonoGar.TabIndex = 104;
            this.lblTelefonoGar.Text = "2_lblTelefono";
            // 
            // txtTelefonoGar
            // 
            this.txtTelefonoGar.Location = new System.Drawing.Point(82, 102);
            this.txtTelefonoGar.Name = "txtTelefonoGar";
            this.txtTelefonoGar.ReadOnly = true;
            this.txtTelefonoGar.Size = new System.Drawing.Size(106, 21);
            this.txtTelefonoGar.TabIndex = 103;
            // 
            // gbFilterGar
            // 
            this.gbFilterGar.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterGar.Appearance.Options.UseFont = true;
            this.gbFilterGar.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterGar.AppearanceCaption.Options.UseFont = true;
            this.gbFilterGar.Controls.Add(this.chkActivo);
            this.gbFilterGar.Controls.Add(this.cmbEstado);
            this.gbFilterGar.Controls.Add(this.lblDocNroGar);
            this.gbFilterGar.Controls.Add(this.lblEstado);
            this.gbFilterGar.Controls.Add(this.txtDocNroGar);
            this.gbFilterGar.Controls.Add(this.masterPrefijoGar);
            this.gbFilterGar.Controls.Add(this.masterDocumentoGar);
            this.gbFilterGar.Controls.Add(this.masterTerceroGar);
            this.gbFilterGar.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbFilterGar.Location = new System.Drawing.Point(2, 2);
            this.gbFilterGar.Name = "gbFilterGar";
            this.gbFilterGar.Size = new System.Drawing.Size(913, 97);
            this.gbFilterGar.TabIndex = 102;
            this.gbFilterGar.Text = "2_gbFilter";
            // 
            // chkActivo
            // 
            this.chkActivo.EditValue = true;
            this.chkActivo.Location = new System.Drawing.Point(371, 74);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkActivo.Properties.Appearance.Options.UseFont = true;
            this.chkActivo.Properties.Caption = "2_chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(75, 19);
            this.chkActivo.TabIndex = 107;
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(128, 73);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbEstado.Properties.DisplayMember = "Value";
            this.cmbEstado.Properties.ValueMember = "Key";
            this.cmbEstado.Size = new System.Drawing.Size(108, 20);
            this.cmbEstado.TabIndex = 106;
            // 
            // lblDocNroGar
            // 
            this.lblDocNroGar.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocNroGar.Location = new System.Drawing.Point(719, 54);
            this.lblDocNroGar.Name = "lblDocNroGar";
            this.lblDocNroGar.Size = new System.Drawing.Size(65, 14);
            this.lblDocNroGar.TabIndex = 101;
            this.lblDocNroGar.Text = "2_lblDocNro";
            // 
            // lblEstado
            // 
            this.lblEstado.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblEstado.Location = new System.Drawing.Point(10, 76);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(62, 14);
            this.lblEstado.TabIndex = 105;
            this.lblEstado.Text = "2_lblEstado";
            // 
            // txtDocNroGar
            // 
            this.txtDocNroGar.Location = new System.Drawing.Point(760, 49);
            this.txtDocNroGar.Name = "txtDocNroGar";
            this.txtDocNroGar.Size = new System.Drawing.Size(52, 22);
            this.txtDocNroGar.TabIndex = 100;
            // 
            // masterPrefijoGar
            // 
            this.masterPrefijoGar.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoGar.Filtros = null;
            this.masterPrefijoGar.Location = new System.Drawing.Point(372, 48);
            this.masterPrefijoGar.Name = "masterPrefijoGar";
            this.masterPrefijoGar.Size = new System.Drawing.Size(343, 25);
            this.masterPrefijoGar.TabIndex = 103;
            this.masterPrefijoGar.Value = "";
            // 
            // masterDocumentoGar
            // 
            this.masterDocumentoGar.BackColor = System.Drawing.Color.Transparent;
            this.masterDocumentoGar.Filtros = null;
            this.masterDocumentoGar.Location = new System.Drawing.Point(11, 47);
            this.masterDocumentoGar.Name = "masterDocumentoGar";
            this.masterDocumentoGar.Size = new System.Drawing.Size(351, 25);
            this.masterDocumentoGar.TabIndex = 102;
            this.masterDocumentoGar.Value = "";
            // 
            // masterTerceroGar
            // 
            this.masterTerceroGar.BackColor = System.Drawing.Color.Transparent;
            this.masterTerceroGar.Filtros = null;
            this.masterTerceroGar.Location = new System.Drawing.Point(12, 23);
            this.masterTerceroGar.Name = "masterTerceroGar";
            this.masterTerceroGar.Size = new System.Drawing.Size(350, 25);
            this.masterTerceroGar.TabIndex = 91;
            this.masterTerceroGar.Value = "";
            // 
            // GarantiaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 662);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "GarantiaControl";
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupDetalle)).EndInit();
            this.groupDetalle.ResumeLayout(false);
            this.pnDatosBasicos.ResumeLayout(false);
            this.pnDatosBasicos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkNuevo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVto.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrAsegurado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoGarantia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrFuente.Properties)).EndInit();
            this.gbGarantiaPrendaria.ResumeLayout(false);
            this.gbGarantiaPrendaria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFuentePre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoVehiculo.Properties)).EndInit();
            this.gbGarantiaHipotecaria.ResumeLayout(false);
            this.gbGarantiaHipotecaria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoInmueble.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFuenteHip.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcGarantia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGarantia)).EndInit();
            this.pnDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnGarantia)).EndInit();
            this.pnGarantia.ResumeLayout(false);
            this.pnGarantia.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGar)).EndInit();
            this.gbFilterGar.ResumeLayout(false);
            this.gbFilterGar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkActivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit editRichText;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit RichText;
        protected DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopup;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControl;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private System.Windows.Forms.TableLayoutPanel TbLyPanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.Panel pnDetails;
        private DevExpress.XtraEditors.PanelControl pnGarantia;
        private DevExpress.XtraEditors.LabelControl lblCorreoGar;
        private System.Windows.Forms.TextBox txtCorreoGar;
        private DevExpress.XtraEditors.LabelControl lblDireccionGar;
        private System.Windows.Forms.TextBox txtDireccionGar;
        private DevExpress.XtraEditors.LabelControl lblTelefonoGar;
        private System.Windows.Forms.TextBox txtTelefonoGar;
        private DevExpress.XtraEditors.GroupControl gbFilterGar;
        private DevExpress.XtraEditors.CheckEdit chkActivo;
        private DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private DevExpress.XtraEditors.LabelControl lblDocNroGar;
        private DevExpress.XtraEditors.LabelControl lblEstado;
        private System.Windows.Forms.TextBox txtDocNroGar;
        private ControlsUC.uc_MasterFind masterPrefijoGar;
        private ControlsUC.uc_MasterFind masterDocumentoGar;
        private ControlsUC.uc_MasterFind masterTerceroGar;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoGarantia;
        private DevExpress.XtraEditors.LabelControl lblTipoGarantia;
        private DevExpress.XtraEditors.GroupControl groupDetalle;
        private System.Windows.Forms.Label lblFechaVto;
        private DevExpress.XtraEditors.DateEdit dtFechaVto;
        private DevExpress.XtraEditors.DateEdit dtFechaInicio;
        private System.Windows.Forms.Label lblFechaInicio;
        private DevExpress.XtraEditors.LabelControl lblCodigoGaranPre;
        private System.Windows.Forms.TextBox txtCodigoGaranPre;
        private DevExpress.XtraEditors.LabelControl lblDatoAdd1;
        private System.Windows.Forms.TextBox txtMarcaFasecolda;
        private DevExpress.XtraEditors.LabelControl lblFuentePre;
        private DevExpress.XtraEditors.LabelControl lblModeloPre;
        private System.Windows.Forms.TextBox txtModeloPre;
        private DevExpress.XtraEditors.LabelControl lblDatoAdd2;
        private System.Windows.Forms.TextBox txtClaseFasecolda;
        private ControlsUC.uc_MasterFind masterFaseColda;
        private System.Windows.Forms.Label lblVlrAsegurado;
        private DevExpress.XtraEditors.TextEdit txtVlrAsegurado;
        private System.Windows.Forms.Label lblVlrFuente;
        private DevExpress.XtraEditors.TextEdit txtVlrFuente;
        private System.Windows.Forms.GroupBox gbGarantiaHipotecaria;
        private DevExpress.XtraEditors.LabelControl lblFuenteHip;
        private DevExpress.XtraEditors.LabelControl lblModeloHip;
        private System.Windows.Forms.TextBox txtAnoHip;
        private DevExpress.XtraEditors.LabelControl lblCodigoGaranHip;
        private System.Windows.Forms.TextBox txtCodigoGaranHip;
        private DevExpress.XtraEditors.LabelControl lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.GroupBox gbGarantiaPrendaria;
        private DevExpress.XtraGrid.GridControl gcGarantia;
        private DevExpress.XtraGrid.Views.Grid.GridView gvGarantia;
        private ControlsUC.uc_MasterFind masterGarantia;
        private DevExpress.XtraEditors.CheckEdit chkNuevo;
        private System.Windows.Forms.TextBox txtMotor;
        private DevExpress.XtraEditors.LabelControl lblMotor;
        private System.Windows.Forms.TextBox txtSerie;
        private DevExpress.XtraEditors.LabelControl lblSerie;
        private System.Windows.Forms.TextBox txtChasis;
        private DevExpress.XtraEditors.LabelControl lblChasis;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoVehiculo;
        private DevExpress.XtraEditors.LabelControl lblTipoVehiculo;
        private DevExpress.XtraEditors.LookUpEdit cmbFuentePre;
        private DevExpress.XtraEditors.LookUpEdit cmbFuenteHip;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoInmueble;
        private DevExpress.XtraEditors.LabelControl lblTipoInmueble;
        private System.Windows.Forms.TextBox txtPrenda;
        private DevExpress.XtraEditors.LabelControl lblPrenda;
        private System.Windows.Forms.TextBox txtEscritura;
        private DevExpress.XtraEditors.LabelControl lblEscritura;
        private System.Windows.Forms.Panel pnDatosBasicos;   
    }
}