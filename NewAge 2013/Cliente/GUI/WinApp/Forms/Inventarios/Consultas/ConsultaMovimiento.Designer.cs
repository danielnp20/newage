namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaMovimiento
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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.gcMovimiento = new DevExpress.XtraGrid.GridControl();
            this.gvMovimiento = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnFiltros = new DevExpress.XtraEditors.GroupControl();
            this.lblEstado = new DevExpress.XtraEditors.LabelControl();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.rdEntradaSalida = new DevExpress.XtraEditors.RadioGroup();
            this.dtPeriodoFinal = new DevExpress.XtraEditors.DateEdit();
            this.dtPeriodoInicio = new DevExpress.XtraEditors.DateEdit();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.lblDoc = new System.Windows.Forms.Label();
            this.masterDocumento = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtDocNro = new DevExpress.XtraEditors.TextEdit();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDocNro = new System.Windows.Forms.Label();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.masterMarca = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtRefProveedor = new DevExpress.XtraEditors.TextEdit();
            this.lblRefProveedor = new System.Windows.Forms.Label();
            this.masterReferencia = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterBodega = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTipoMvtoInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaInicio = new DevExpress.XtraEditors.LabelControl();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editCant = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gcMovimiento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMovimiento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnFiltros)).BeginInit();
            this.pnFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdEntradaSalida.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoFinal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDocNro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefProveedor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant)).BeginInit();
            this.SuspendLayout();
            // 
            // pgGrid
            // 
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(373, 565);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(431, 26);
            this.pgGrid.TabIndex = 1;
            this.pgGrid.Visible = false;
            // 
            // gcMovimiento
            // 
            this.gcMovimiento.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcMovimiento.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcMovimiento.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcMovimiento.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcMovimiento.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcMovimiento.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcMovimiento.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcMovimiento.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcMovimiento.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcMovimiento.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcMovimiento.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcMovimiento.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            gridLevelNode1.RelationName = "Level1";
            this.gcMovimiento.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcMovimiento.Location = new System.Drawing.Point(9, 169);
            this.gcMovimiento.LookAndFeel.SkinName = "Dark Side";
            this.gcMovimiento.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcMovimiento.MainView = this.gvMovimiento;
            this.gcMovimiento.Margin = new System.Windows.Forms.Padding(4);
            this.gcMovimiento.Name = "gcMovimiento";
            this.gcMovimiento.Size = new System.Drawing.Size(1143, 389);
            this.gcMovimiento.TabIndex = 2;
            this.gcMovimiento.UseEmbeddedNavigator = true;
            this.gcMovimiento.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMovimiento});
            // 
            // gvMovimiento
            // 
            this.gvMovimiento.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMovimiento.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvMovimiento.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvMovimiento.Appearance.Empty.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvMovimiento.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvMovimiento.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvMovimiento.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvMovimiento.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvMovimiento.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvMovimiento.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvMovimiento.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvMovimiento.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvMovimiento.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvMovimiento.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvMovimiento.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMovimiento.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvMovimiento.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvMovimiento.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvMovimiento.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvMovimiento.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvMovimiento.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvMovimiento.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvMovimiento.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvMovimiento.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvMovimiento.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvMovimiento.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvMovimiento.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvMovimiento.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.5F);
            this.gvMovimiento.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvMovimiento.Appearance.Row.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.Row.Options.UseFont = true;
            this.gvMovimiento.Appearance.Row.Options.UseForeColor = true;
            this.gvMovimiento.Appearance.Row.Options.UseTextOptions = true;
            this.gvMovimiento.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvMovimiento.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvMovimiento.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvMovimiento.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvMovimiento.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvMovimiento.Appearance.VertLine.Options.UseBackColor = true;
            this.gvMovimiento.Appearance.ViewCaption.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.gvMovimiento.Appearance.ViewCaption.Options.UseFont = true;
            this.gvMovimiento.GridControl = this.gcMovimiento;
            this.gvMovimiento.HorzScrollStep = 50;
            this.gvMovimiento.Name = "gvMovimiento";
            this.gvMovimiento.OptionsBehavior.ReadOnly = true;
            this.gvMovimiento.OptionsCustomization.AllowFilter = false;
            this.gvMovimiento.OptionsMenu.EnableColumnMenu = false;
            this.gvMovimiento.OptionsMenu.EnableFooterMenu = false;
            this.gvMovimiento.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvMovimiento.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvMovimiento.OptionsView.ColumnAutoWidth = false;
            this.gvMovimiento.OptionsView.ShowAutoFilterRow = true;
            this.gvMovimiento.OptionsView.ShowFooter = true;
            this.gvMovimiento.OptionsView.ShowGroupPanel = false;
            this.gvMovimiento.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvMvto_CustomUnboundColumnData);
            this.gvMovimiento.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvMovimiento_CustomColumnDisplayText);
            // 
            // pnFiltros
            // 
            this.pnFiltros.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pnFiltros.Appearance.Options.UseBackColor = true;
            this.pnFiltros.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.pnFiltros.AppearanceCaption.Options.UseFont = true;
            this.pnFiltros.Controls.Add(this.lblEstado);
            this.pnFiltros.Controls.Add(this.cmbEstado);
            this.pnFiltros.Controls.Add(this.rdEntradaSalida);
            this.pnFiltros.Controls.Add(this.dtPeriodoFinal);
            this.pnFiltros.Controls.Add(this.dtPeriodoInicio);
            this.pnFiltros.Controls.Add(this.panelControl3);
            this.pnFiltros.Controls.Add(this.panelControl2);
            this.pnFiltros.Controls.Add(this.panelControl1);
            this.pnFiltros.Controls.Add(this.labelControl1);
            this.pnFiltros.Controls.Add(this.lblFechaInicio);
            this.pnFiltros.Location = new System.Drawing.Point(9, 12);
            this.pnFiltros.Name = "pnFiltros";
            this.pnFiltros.Size = new System.Drawing.Size(1134, 155);
            this.pnFiltros.TabIndex = 92;
            this.pnFiltros.Text = "20319_gbFilter";
            // 
            // lblEstado
            // 
            this.lblEstado.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblEstado.Location = new System.Drawing.Point(629, 29);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(90, 14);
            this.lblEstado.TabIndex = 74;
            this.lblEstado.Text = "26312_lblEstado";
            this.lblEstado.Visible = false;
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(725, 26);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Size = new System.Drawing.Size(112, 20);
            this.cmbEstado.TabIndex = 73;
            this.cmbEstado.Visible = false;
            // 
            // rdEntradaSalida
            // 
            this.rdEntradaSalida.EditValue = true;
            this.rdEntradaSalida.Location = new System.Drawing.Point(420, 24);
            this.rdEntradaSalida.Name = "rdEntradaSalida";
            this.rdEntradaSalida.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdEntradaSalida.Properties.Appearance.Options.UseBackColor = true;
            this.rdEntradaSalida.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdEntradaSalida.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "Todos"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "Entradas"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(false, "Salidas")});
            this.rdEntradaSalida.Size = new System.Drawing.Size(210, 22);
            this.rdEntradaSalida.TabIndex = 75;
            // 
            // dtPeriodoFinal
            // 
            this.dtPeriodoFinal.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtPeriodoFinal.Location = new System.Drawing.Point(309, 26);
            this.dtPeriodoFinal.Name = "dtPeriodoFinal";
            this.dtPeriodoFinal.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtPeriodoFinal.Properties.Appearance.Options.UseBackColor = true;
            this.dtPeriodoFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPeriodoFinal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtPeriodoFinal.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtPeriodoFinal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtPeriodoFinal.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtPeriodoFinal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtPeriodoFinal.Properties.Mask.EditMask = "yyyy/MM";
            this.dtPeriodoFinal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtPeriodoFinal.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.MonthView;
            this.dtPeriodoFinal.Size = new System.Drawing.Size(80, 20);
            this.dtPeriodoFinal.TabIndex = 107;
            // 
            // dtPeriodoInicio
            // 
            this.dtPeriodoInicio.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtPeriodoInicio.Location = new System.Drawing.Point(118, 26);
            this.dtPeriodoInicio.Name = "dtPeriodoInicio";
            this.dtPeriodoInicio.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtPeriodoInicio.Properties.Appearance.Options.UseBackColor = true;
            this.dtPeriodoInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPeriodoInicio.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtPeriodoInicio.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtPeriodoInicio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtPeriodoInicio.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtPeriodoInicio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtPeriodoInicio.Properties.Mask.EditMask = "yyyy/MM";
            this.dtPeriodoInicio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtPeriodoInicio.Size = new System.Drawing.Size(84, 20);
            this.dtPeriodoInicio.TabIndex = 106;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.lblDoc);
            this.panelControl3.Controls.Add(this.masterDocumento);
            this.panelControl3.Controls.Add(this.txtDocNro);
            this.panelControl3.Controls.Add(this.masterPrefijo);
            this.panelControl3.Controls.Add(this.lblDocNro);
            this.panelControl3.Location = new System.Drawing.Point(6, 119);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1114, 31);
            this.panelControl3.TabIndex = 2;
            // 
            // lblDoc
            // 
            this.lblDoc.AutoSize = true;
            this.lblDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDoc.Location = new System.Drawing.Point(6, 9);
            this.lblDoc.Name = "lblDoc";
            this.lblDoc.Size = new System.Drawing.Size(99, 14);
            this.lblDoc.TabIndex = 14;
            this.lblDoc.Text = "Tipo Documento";
            // 
            // masterDocumento
            // 
            this.masterDocumento.BackColor = System.Drawing.Color.Transparent;
            this.masterDocumento.Filtros = null;
            this.masterDocumento.Location = new System.Drawing.Point(9, 4);
            this.masterDocumento.Name = "masterDocumento";
            this.masterDocumento.Size = new System.Drawing.Size(298, 25);
            this.masterDocumento.TabIndex = 0;
            this.masterDocumento.Value = "";
            // 
            // txtDocNro
            // 
            this.txtDocNro.Location = new System.Drawing.Point(674, 6);
            this.txtDocNro.Name = "txtDocNro";
            this.txtDocNro.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDocNro.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtDocNro.Size = new System.Drawing.Size(46, 20);
            this.txtDocNro.TabIndex = 2;
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(314, 4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(301, 25);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // lblDocNro
            // 
            this.lblDocNro.AutoSize = true;
            this.lblDocNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocNro.Location = new System.Drawing.Point(620, 10);
            this.lblDocNro.Name = "lblDocNro";
            this.lblDocNro.Size = new System.Drawing.Size(100, 14);
            this.lblDocNro.TabIndex = 11;
            this.lblDocNro.Text = "26312_lblDocNro";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.masterMarca);
            this.panelControl2.Controls.Add(this.masterCentroCto);
            this.panelControl2.Controls.Add(this.masterProyecto);
            this.panelControl2.Location = new System.Drawing.Point(5, 84);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1115, 31);
            this.panelControl2.TabIndex = 92;
            // 
            // masterMarca
            // 
            this.masterMarca.BackColor = System.Drawing.Color.Transparent;
            this.masterMarca.Filtros = null;
            this.masterMarca.Location = new System.Drawing.Point(622, 3);
            this.masterMarca.Name = "masterMarca";
            this.masterMarca.Size = new System.Drawing.Size(303, 25);
            this.masterMarca.TabIndex = 3;
            this.masterMarca.Value = "";
            // 
            // masterCentroCto
            // 
            this.masterCentroCto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCto.Filtros = null;
            this.masterCentroCto.Location = new System.Drawing.Point(314, 5);
            this.masterCentroCto.Name = "masterCentroCto";
            this.masterCentroCto.Size = new System.Drawing.Size(300, 25);
            this.masterCentroCto.TabIndex = 2;
            this.masterCentroCto.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(9, 5);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(298, 25);
            this.masterProyecto.TabIndex = 0;
            this.masterProyecto.Value = "";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.txtRefProveedor);
            this.panelControl1.Controls.Add(this.lblRefProveedor);
            this.panelControl1.Controls.Add(this.masterReferencia);
            this.panelControl1.Controls.Add(this.masterBodega);
            this.panelControl1.Controls.Add(this.masterTipoMvtoInv);
            this.panelControl1.Location = new System.Drawing.Point(6, 50);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1114, 31);
            this.panelControl1.TabIndex = 91;
            // 
            // txtRefProveedor
            // 
            this.txtRefProveedor.Location = new System.Drawing.Point(1009, 6);
            this.txtRefProveedor.Name = "txtRefProveedor";
            this.txtRefProveedor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtRefProveedor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtRefProveedor.Size = new System.Drawing.Size(86, 20);
            this.txtRefProveedor.TabIndex = 12;
            // 
            // lblRefProveedor
            // 
            this.lblRefProveedor.AutoSize = true;
            this.lblRefProveedor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefProveedor.Location = new System.Drawing.Point(929, 9);
            this.lblRefProveedor.Name = "lblRefProveedor";
            this.lblRefProveedor.Size = new System.Drawing.Size(134, 14);
            this.lblRefProveedor.TabIndex = 13;
            this.lblRefProveedor.Text = "26312_lblRefProveedor";
            // 
            // masterReferencia
            // 
            this.masterReferencia.BackColor = System.Drawing.Color.Transparent;
            this.masterReferencia.Filtros = null;
            this.masterReferencia.Location = new System.Drawing.Point(621, 4);
            this.masterReferencia.Name = "masterReferencia";
            this.masterReferencia.Size = new System.Drawing.Size(301, 25);
            this.masterReferencia.TabIndex = 2;
            this.masterReferencia.Value = "";
            // 
            // masterBodega
            // 
            this.masterBodega.BackColor = System.Drawing.Color.Transparent;
            this.masterBodega.Filtros = null;
            this.masterBodega.Location = new System.Drawing.Point(314, 4);
            this.masterBodega.Name = "masterBodega";
            this.masterBodega.Size = new System.Drawing.Size(300, 25);
            this.masterBodega.TabIndex = 1;
            this.masterBodega.Value = "";
            // 
            // masterTipoMvtoInv
            // 
            this.masterTipoMvtoInv.BackColor = System.Drawing.Color.Transparent;
            this.masterTipoMvtoInv.Filtros = null;
            this.masterTipoMvtoInv.Location = new System.Drawing.Point(7, 4);
            this.masterTipoMvtoInv.Name = "masterTipoMvtoInv";
            this.masterTipoMvtoInv.Size = new System.Drawing.Size(300, 25);
            this.masterTipoMvtoInv.TabIndex = 0;
            this.masterTipoMvtoInv.Value = "";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.labelControl1.Location = new System.Drawing.Point(208, 28);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(100, 14);
            this.labelControl1.TabIndex = 90;
            this.labelControl1.Text = "26312_lblFechaFin";
            // 
            // lblFechaInicio
            // 
            this.lblFechaInicio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblFechaInicio.Location = new System.Drawing.Point(13, 29);
            this.lblFechaInicio.Name = "lblFechaInicio";
            this.lblFechaInicio.Size = new System.Drawing.Size(113, 14);
            this.lblFechaInicio.TabIndex = 88;
            this.lblFechaInicio.Text = "26312_lblFechaInicio";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editLink,
            this.editCant});
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c2";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.SingleClick = true;
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // editCant
            // 
            this.editCant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editCant.Mask.EditMask = "n0";
            this.editCant.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editCant.Mask.UseMaskAsDisplayFormat = true;
            this.editCant.Name = "editCant";
            // 
            // ConsultaMovimiento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 597);
            this.Controls.Add(this.pnFiltros);
            this.Controls.Add(this.gcMovimiento);
            this.Controls.Add(this.pgGrid);
            this.Name = "ConsultaMovimiento";
            this.Text = "26312";
            ((System.ComponentModel.ISupportInitialize)(this.gcMovimiento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMovimiento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnFiltros)).EndInit();
            this.pnFiltros.ResumeLayout(false);
            this.pnFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdEntradaSalida.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoFinal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoInicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDocNro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRefProveedor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_Pagging pgGrid;
        private DevExpress.XtraGrid.GridControl gcMovimiento;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMovimiento;
        private DevExpress.XtraEditors.GroupControl pnFiltros;
        protected DevExpress.XtraEditors.DateEdit dtPeriodoFinal;
        protected DevExpress.XtraEditors.DateEdit dtPeriodoInicio;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private System.Windows.Forms.Label lblDoc;
        private DevExpress.XtraEditors.TextEdit txtDocNro;
        private ControlsUC.uc_MasterFind masterDocumento;
        private System.Windows.Forms.Label lblDocNro;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private ControlsUC.uc_MasterFind masterProyecto;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private ControlsUC.uc_MasterFind masterReferencia;
        private ControlsUC.uc_MasterFind masterBodega;
        private ControlsUC.uc_MasterFind masterTipoMvtoInv;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblFechaInicio;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private DevExpress.XtraEditors.LabelControl lblEstado;
        private DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private DevExpress.XtraEditors.RadioGroup rdEntradaSalida;
        private ControlsUC.uc_MasterFind masterCentroCto;
        private ControlsUC.uc_MasterFind masterMarca;
        private DevExpress.XtraEditors.TextEdit txtRefProveedor;
        private System.Windows.Forms.Label lblRefProveedor;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editCant;
    }
}