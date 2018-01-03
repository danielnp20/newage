namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Solicitudes
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
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcGenerales = new DevExpress.XtraGrid.GridControl();
            this.gvGenerales = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.tc_QueryCreditos = new DevExpress.XtraTab.XtraTabControl();
            this.tp_DatosGenerales = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gbGridDocument = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbTipoReporte = new DevExpress.XtraEditors.LookUpEdit();
            this.lblReporte = new System.Windows.Forms.Label();
            this.btnDocumento = new System.Windows.Forms.Button();
            this.btnDevolucion = new System.Windows.Forms.Button();
            this.lbTarea = new System.Windows.Forms.TextBox();
            this.lblRevisa = new System.Windows.Forms.Label();
            this.Tarea = new System.Windows.Forms.Label();
            this.mfTarea = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLibranza = new DevExpress.XtraEditors.TextEdit();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.txtPlazo = new System.Windows.Forms.TextBox();
            this.btnReferencia = new System.Windows.Forms.Button();
            this.btnLiquidacion = new System.Windows.Forms.Button();
            this.btnAnalisis = new System.Windows.Forms.Button();
            this.btnAnexos = new System.Windows.Forms.Button();
            this.lblPlazo = new System.Windows.Forms.Label();
            this.lblValor = new System.Windows.Forms.Label();
            this.mfTipoCredito = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.mfLinea = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.mfCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcGenerales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGenerales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc_QueryCreditos)).BeginInit();
            this.tc_QueryCreditos.SuspendLayout();
            this.tp_DatosGenerales.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbGridDocument.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoReporte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLibranza.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetalle.GridControl = this.gcGenerales;
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvGenerales_CustomUnboundColumnData);
            // 
            // gcGenerales
            // 
            this.gcGenerales.AllowDrop = true;
            this.gcGenerales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcGenerales.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcGenerales.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcGenerales.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcGenerales.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcGenerales.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcGenerales.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcGenerales.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcGenerales.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcGenerales.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcGenerales.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcGenerales.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcGenerales.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcGenerales.Location = new System.Drawing.Point(3, 15);
            this.gcGenerales.LookAndFeel.SkinName = "Dark Side";
            this.gcGenerales.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcGenerales.MainView = this.gvGenerales;
            this.gcGenerales.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcGenerales.Name = "gcGenerales";
            this.gcGenerales.Size = new System.Drawing.Size(1237, 189);
            this.gcGenerales.TabIndex = 0;
            this.gcGenerales.UseEmbeddedNavigator = true;
            this.gcGenerales.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvGenerales,
            this.gvDetalle});
            // 
            // gvGenerales
            // 
            this.gvGenerales.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGenerales.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvGenerales.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvGenerales.Appearance.Empty.Options.UseBackColor = true;
            this.gvGenerales.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvGenerales.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvGenerales.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvGenerales.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvGenerales.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvGenerales.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvGenerales.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvGenerales.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvGenerales.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvGenerales.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvGenerales.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvGenerales.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvGenerales.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvGenerales.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvGenerales.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvGenerales.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvGenerales.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvGenerales.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvGenerales.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvGenerales.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvGenerales.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvGenerales.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvGenerales.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvGenerales.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.Row.Options.UseBackColor = true;
            this.gvGenerales.Appearance.Row.Options.UseForeColor = true;
            this.gvGenerales.Appearance.Row.Options.UseTextOptions = true;
            this.gvGenerales.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvGenerales.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvGenerales.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvGenerales.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvGenerales.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvGenerales.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvGenerales.Appearance.VertLine.Options.UseBackColor = true;
            this.gvGenerales.GridControl = this.gcGenerales;
            this.gvGenerales.HorzScrollStep = 50;
            this.gvGenerales.Name = "gvGenerales";
            this.gvGenerales.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvGenerales.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvGenerales.OptionsCustomization.AllowColumnMoving = false;
            this.gvGenerales.OptionsCustomization.AllowFilter = false;
            this.gvGenerales.OptionsCustomization.AllowSort = false;
            this.gvGenerales.OptionsMenu.EnableColumnMenu = false;
            this.gvGenerales.OptionsMenu.EnableFooterMenu = false;
            this.gvGenerales.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvGenerales.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvGenerales.OptionsView.ShowGroupPanel = false;
            this.gvGenerales.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvGenerales.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvGenerales_CustomUnboundColumnData);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editCheck});
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "C2";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editCheck
            // 
            this.editCheck.DisplayValueChecked = "True";
            this.editCheck.DisplayValueUnchecked = "False";
            this.editCheck.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editCheck.Name = "editCheck";
            this.editCheck.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // tc_QueryCreditos
            // 
            this.tc_QueryCreditos.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 9F);
            this.tc_QueryCreditos.AppearancePage.Header.Options.UseFont = true;
            this.tc_QueryCreditos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_QueryCreditos.Location = new System.Drawing.Point(0, 0);
            this.tc_QueryCreditos.Margin = new System.Windows.Forms.Padding(1);
            this.tc_QueryCreditos.Name = "tc_QueryCreditos";
            this.tc_QueryCreditos.SelectedTabPage = this.tp_DatosGenerales;
            this.tc_QueryCreditos.Size = new System.Drawing.Size(1251, 482);
            this.tc_QueryCreditos.TabIndex = 2;
            this.tc_QueryCreditos.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tp_DatosGenerales});
            // 
            // tp_DatosGenerales
            // 
            this.tp_DatosGenerales.Controls.Add(this.groupBox2);
            this.tp_DatosGenerales.Controls.Add(this.groupBox1);
            this.tp_DatosGenerales.Margin = new System.Windows.Forms.Padding(1);
            this.tp_DatosGenerales.Name = "tp_DatosGenerales";
            this.tp_DatosGenerales.Size = new System.Drawing.Size(1245, 453);
            this.tp_DatosGenerales.Text = "32311_tp_DatosGenerales";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.gbGridDocument);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 231);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(1245, 222);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "32317_InfoDetalle";
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.gcGenerales);
            this.gbGridDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGridDocument.Location = new System.Drawing.Point(1, 16);
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(1);
            this.gbGridDocument.Name = "gbGridDocument";
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.gbGridDocument.Size = new System.Drawing.Size(1243, 205);
            this.gbGridDocument.TabIndex = 0;
            this.gbGridDocument.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.cmbTipoReporte);
            this.groupBox1.Controls.Add(this.lblReporte);
            this.groupBox1.Controls.Add(this.btnDocumento);
            this.groupBox1.Controls.Add(this.btnDevolucion);
            this.groupBox1.Controls.Add(this.lbTarea);
            this.groupBox1.Controls.Add(this.lblRevisa);
            this.groupBox1.Controls.Add(this.Tarea);
            this.groupBox1.Controls.Add(this.mfTarea);
            this.groupBox1.Controls.Add(this.panelControl1);
            this.groupBox1.Controls.Add(this.txtValor);
            this.groupBox1.Controls.Add(this.txtPlazo);
            this.groupBox1.Controls.Add(this.btnReferencia);
            this.groupBox1.Controls.Add(this.btnLiquidacion);
            this.groupBox1.Controls.Add(this.btnAnalisis);
            this.groupBox1.Controls.Add(this.btnAnexos);
            this.groupBox1.Controls.Add(this.lblPlazo);
            this.groupBox1.Controls.Add(this.lblValor);
            this.groupBox1.Controls.Add(this.mfTipoCredito);
            this.groupBox1.Controls.Add(this.mfLinea);
            this.groupBox1.Controls.Add(this.mfCliente);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox1.Size = new System.Drawing.Size(1245, 231);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "32317_Solicitudes";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cmbTipoReporte
            // 
            this.cmbTipoReporte.Location = new System.Drawing.Point(943, 192);
            this.cmbTipoReporte.Name = "cmbTipoReporte";
            this.cmbTipoReporte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoReporte.Size = new System.Drawing.Size(132, 20);
            this.cmbTipoReporte.TabIndex = 89;
            // 
            // lblReporte
            // 
            this.lblReporte.AutoSize = true;
            this.lblReporte.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblReporte.Location = new System.Drawing.Point(842, 193);
            this.lblReporte.Name = "lblReporte";
            this.lblReporte.Size = new System.Drawing.Size(95, 16);
            this.lblReporte.TabIndex = 88;
            this.lblReporte.Text = "32317_Reporte";
            // 
            // btnDocumento
            // 
            this.btnDocumento.Enabled = false;
            this.btnDocumento.Location = new System.Drawing.Point(942, 22);
            this.btnDocumento.Name = "btnDocumento";
            this.btnDocumento.Size = new System.Drawing.Size(95, 23);
            this.btnDocumento.TabIndex = 85;
            this.btnDocumento.Text = "32317_Documento";
            this.btnDocumento.UseVisualStyleBackColor = true;
            this.btnDocumento.Click += new System.EventHandler(this.btnDocumento_Click);
            // 
            // btnDevolucion
            // 
            this.btnDevolucion.Enabled = false;
            this.btnDevolucion.Location = new System.Drawing.Point(942, 47);
            this.btnDevolucion.Name = "btnDevolucion";
            this.btnDevolucion.Size = new System.Drawing.Size(95, 23);
            this.btnDevolucion.TabIndex = 84;
            this.btnDevolucion.Text = "32317_Devolucion";
            this.btnDevolucion.UseVisualStyleBackColor = true;
            this.btnDevolucion.Click += new System.EventHandler(this.btnDevolucion_Click);
            // 
            // lbTarea
            // 
            this.lbTarea.Enabled = false;
            this.lbTarea.Location = new System.Drawing.Point(644, 102);
            this.lbTarea.Name = "lbTarea";
            this.lbTarea.Size = new System.Drawing.Size(118, 22);
            this.lbTarea.TabIndex = 83;
            // 
            // lblRevisa
            // 
            this.lblRevisa.AutoSize = true;
            this.lblRevisa.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblRevisa.Location = new System.Drawing.Point(531, 170);
            this.lblRevisa.Name = "lblRevisa";
            this.lblRevisa.Size = new System.Drawing.Size(87, 16);
            this.lblRevisa.TabIndex = 82;
            this.lblRevisa.Text = "32317_Revisa";
            this.lblRevisa.Visible = false;
            // 
            // Tarea
            // 
            this.Tarea.AutoSize = true;
            this.Tarea.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.Tarea.Location = new System.Drawing.Point(531, 104);
            this.Tarea.Name = "Tarea";
            this.Tarea.Size = new System.Drawing.Size(84, 16);
            this.Tarea.TabIndex = 81;
            this.Tarea.Text = "32317_Tarea";
            // 
            // mfTarea
            // 
            this.mfTarea.BackColor = System.Drawing.Color.Transparent;
            this.mfTarea.Filtros = null;
            this.mfTarea.Location = new System.Drawing.Point(527, 165);
            this.mfTarea.Name = "mfTarea";
            this.mfTarea.Size = new System.Drawing.Size(346, 25);
            this.mfTarea.TabIndex = 80;
            this.mfTarea.Value = "";
            this.mfTarea.Visible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.txtLibranza);
            this.panelControl1.Location = new System.Drawing.Point(78, 27);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(222, 35);
            this.panelControl1.TabIndex = 79;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 60;
            this.label1.Text = "32317_Libranza";
            // 
            // txtLibranza
            // 
            this.txtLibranza.EditValue = "";
            this.txtLibranza.Location = new System.Drawing.Point(116, 8);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtLibranza.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Properties.Appearance.Options.UseBorderColor = true;
            this.txtLibranza.Properties.Appearance.Options.UseFont = true;
            this.txtLibranza.Properties.Appearance.Options.UseTextOptions = true;
            this.txtLibranza.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtLibranza.Properties.AutoHeight = false;
            this.txtLibranza.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtLibranza.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtLibranza.Properties.Mask.EditMask = "n0";
            this.txtLibranza.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtLibranza.Size = new System.Drawing.Size(95, 20);
            this.txtLibranza.TabIndex = 78;
            this.txtLibranza.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLibranza_KeyPress);
            this.txtLibranza.Leave += new System.EventHandler(this.txtLibranza_Leave);
            // 
            // txtValor
            // 
            this.txtValor.EditValue = "0,00 ";
            this.txtValor.Enabled = false;
            this.txtValor.Location = new System.Drawing.Point(644, 70);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValor.Properties.Appearance.Options.UseFont = true;
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.AutoHeight = false;
            this.txtValor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.Mask.EditMask = "c";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Size = new System.Drawing.Size(118, 20);
            this.txtValor.TabIndex = 77;
            // 
            // txtPlazo
            // 
            this.txtPlazo.Enabled = false;
            this.txtPlazo.Location = new System.Drawing.Point(644, 135);
            this.txtPlazo.Name = "txtPlazo";
            this.txtPlazo.Size = new System.Drawing.Size(118, 22);
            this.txtPlazo.TabIndex = 59;
            // 
            // btnReferencia
            // 
            this.btnReferencia.Enabled = false;
            this.btnReferencia.Location = new System.Drawing.Point(942, 150);
            this.btnReferencia.Name = "btnReferencia";
            this.btnReferencia.Size = new System.Drawing.Size(95, 23);
            this.btnReferencia.TabIndex = 56;
            this.btnReferencia.Text = "32317_Referencia";
            this.btnReferencia.UseVisualStyleBackColor = true;
            this.btnReferencia.Click += new System.EventHandler(this.btnReferencia_Click);
            // 
            // btnLiquidacion
            // 
            this.btnLiquidacion.Enabled = false;
            this.btnLiquidacion.Location = new System.Drawing.Point(942, 124);
            this.btnLiquidacion.Name = "btnLiquidacion";
            this.btnLiquidacion.Size = new System.Drawing.Size(95, 23);
            this.btnLiquidacion.TabIndex = 55;
            this.btnLiquidacion.Text = "32317_Liquidación";
            this.btnLiquidacion.UseVisualStyleBackColor = true;
            this.btnLiquidacion.Click += new System.EventHandler(this.btnLiquidacion_Click);
            // 
            // btnAnalisis
            // 
            this.btnAnalisis.Enabled = false;
            this.btnAnalisis.Location = new System.Drawing.Point(942, 98);
            this.btnAnalisis.Name = "btnAnalisis";
            this.btnAnalisis.Size = new System.Drawing.Size(95, 23);
            this.btnAnalisis.TabIndex = 54;
            this.btnAnalisis.Text = "32317_Analisis";
            this.btnAnalisis.UseVisualStyleBackColor = true;
            this.btnAnalisis.Click += new System.EventHandler(this.btnAnalisis_Click);
            // 
            // btnAnexos
            // 
            this.btnAnexos.Enabled = false;
            this.btnAnexos.Location = new System.Drawing.Point(942, 72);
            this.btnAnexos.Name = "btnAnexos";
            this.btnAnexos.Size = new System.Drawing.Size(95, 23);
            this.btnAnexos.TabIndex = 53;
            this.btnAnexos.Text = "32317_Anexos";
            this.btnAnexos.UseVisualStyleBackColor = true;
            this.btnAnexos.Click += new System.EventHandler(this.btnAnexos_Click);
            // 
            // lblPlazo
            // 
            this.lblPlazo.AutoSize = true;
            this.lblPlazo.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblPlazo.Location = new System.Drawing.Point(532, 137);
            this.lblPlazo.Name = "lblPlazo";
            this.lblPlazo.Size = new System.Drawing.Size(80, 16);
            this.lblPlazo.TabIndex = 51;
            this.lblPlazo.Text = "32317_Plazo";
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.lblValor.Location = new System.Drawing.Point(531, 73);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(80, 16);
            this.lblValor.TabIndex = 49;
            this.lblValor.Text = "32317_Valor";
            // 
            // mfTipoCredito
            // 
            this.mfTipoCredito.BackColor = System.Drawing.Color.Transparent;
            this.mfTipoCredito.Filtros = null;
            this.mfTipoCredito.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mfTipoCredito.Location = new System.Drawing.Point(78, 132);
            this.mfTipoCredito.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mfTipoCredito.Name = "mfTipoCredito";
            this.mfTipoCredito.Size = new System.Drawing.Size(351, 27);
            this.mfTipoCredito.TabIndex = 39;
            this.mfTipoCredito.Value = "";
            // 
            // mfLinea
            // 
            this.mfLinea.BackColor = System.Drawing.Color.Transparent;
            this.mfLinea.Filtros = null;
            this.mfLinea.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mfLinea.Location = new System.Drawing.Point(78, 99);
            this.mfLinea.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mfLinea.Name = "mfLinea";
            this.mfLinea.Size = new System.Drawing.Size(351, 27);
            this.mfLinea.TabIndex = 38;
            this.mfLinea.Value = "";
            // 
            // mfCliente
            // 
            this.mfCliente.BackColor = System.Drawing.Color.Transparent;
            this.mfCliente.Filtros = null;
            this.mfCliente.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mfCliente.Location = new System.Drawing.Point(78, 68);
            this.mfCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mfCliente.Name = "mfCliente";
            this.mfCliente.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mfCliente.Size = new System.Drawing.Size(351, 25);
            this.mfCliente.TabIndex = 37;
            this.mfCliente.Value = "";
            // 
            // Solicitudes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 482);
            this.Controls.Add(this.tc_QueryCreditos);
            this.Name = "Solicitudes";
            this.Text = "ShowDocumentForm";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcGenerales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvGenerales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tc_QueryCreditos)).EndInit();
            this.tc_QueryCreditos.ResumeLayout(false);
            this.tp_DatosGenerales.ResumeLayout(false);
            this.tp_DatosGenerales.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.gbGridDocument.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoReporte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLibranza.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        private DevExpress.XtraTab.XtraTabControl tc_QueryCreditos;
        private DevExpress.XtraTab.XtraTabPage tp_DatosGenerales;
        private System.Windows.Forms.GroupBox groupBox2;
        protected System.Windows.Forms.GroupBox gbGridDocument;
        protected DevExpress.XtraGrid.GridControl gcGenerales;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvGenerales;
        private System.Windows.Forms.GroupBox groupBox1;
        private ControlsUC.uc_MasterFind mfTipoCredito;
        private ControlsUC.uc_MasterFind mfLinea;
        private ControlsUC.uc_MasterFind mfCliente;
        private System.Windows.Forms.Label lblPlazo;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.Button btnReferencia;
        private System.Windows.Forms.Button btnLiquidacion;
        private System.Windows.Forms.Button btnAnalisis;
        private System.Windows.Forms.Button btnAnexos;
        private System.Windows.Forms.TextBox txtPlazo;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private DevExpress.XtraEditors.TextEdit txtLibranza;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label Tarea;
        private ControlsUC.uc_MasterFind mfTarea;
        private System.Windows.Forms.Label lblRevisa;
        private System.Windows.Forms.TextBox lbTarea;
        private System.Windows.Forms.Button btnDocumento;
        private System.Windows.Forms.Button btnDevolucion;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private System.Windows.Forms.Label lblReporte;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoReporte;
    }
}