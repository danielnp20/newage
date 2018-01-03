namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class SentenciaJudicial
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editbtn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.lblFechaDoc = new DevExpress.XtraEditors.LabelControl();
            this.grpboxHeader = new System.Windows.Forms.GroupBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaSentencia = new DevExpress.XtraEditors.DateEdit();
            this.lblObligacion = new System.Windows.Forms.Label();
            this.lkpObligaciones = new DevExpress.XtraEditors.LookUpEdit();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbEstadoActual = new DevExpress.XtraEditors.LookUpEdit();
            this.lblEstadoActual = new DevExpress.XtraEditors.LabelControl();
            this.btnInteres = new System.Windows.Forms.Button();
            this.dtFechaDoc = new DevExpress.XtraEditors.DateEdit();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblBreak = new DevExpress.XtraEditors.LabelControl();
            this.txtDocDesc = new System.Windows.Forms.TextBox();
            this.txtDocumentoID = new System.Windows.Forms.TextBox();
            this.txtNumeroDoc = new System.Windows.Forms.TextBox();
            this.lblNumeroDoc = new DevExpress.XtraEditors.LabelControl();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbPlazoSent = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.dtFecha1CuotSent = new DevExpress.XtraEditors.DateEdit();
            this.txtValorCuota = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lblValor = new System.Windows.Forms.Label();
            this.lblPlazo = new System.Windows.Forms.Label();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gbGridDocument = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editbtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSentencia.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSentencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpObligaciones.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstadoActual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).BeginInit();
            this.pnlDetail.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha1CuotSent.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha1CuotSent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCuota.Properties)).BeginInit();
            this.pnlGrids.SuspendLayout();
            this.gbGridDocument.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Empty.Options.UseFont = true;
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.GridControl = this.gcDocument;
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
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gcDocument
            // 
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocument.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocument.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocument.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocument.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDocuments_EmbeddedNavigator_ButtonClick);
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode2.LevelTemplate = this.gvDetalle;
            gridLevelNode2.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gcDocument.Location = new System.Drawing.Point(6, 13);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.ShowOnlyPredefinedDetails = true;
            this.gcDocument.Size = new System.Drawing.Size(1045, 269);
            this.gcDocument.TabIndex = 0;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalle});
            this.gcDocument.Enter += new System.EventHandler(this.gcDocument_Enter);
            // 
            // gvDocument
            // 
            this.gvDocument.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocument.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocument.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocument.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocument.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocument.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocument.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocument.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.Row.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.Options.UseForeColor = true;
            this.gvDocument.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocument.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocument.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocument.GridControl = this.gcDocument;
            this.gvDocument.HorzScrollStep = 50;
            this.gvDocument.Name = "gvDocument";
            this.gvDocument.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDocument.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDocument.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocument.OptionsCustomization.AllowFilter = false;
            this.gvDocument.OptionsCustomization.AllowSort = false;
            this.gvDocument.OptionsMenu.EnableColumnMenu = false;
            this.gvDocument.OptionsMenu.EnableFooterMenu = false;
            this.gvDocument.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocument.OptionsView.ColumnAutoWidth = false;
            this.gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocument.OptionsView.ShowFooter = true;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocument.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvDocument.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocument_CellValueChanged);
            this.gvDocument.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocument_BeforeLeaveRow);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editSpin,
            this.editValue,
            this.editbtn,
            this.editBtnGrid});
            // 
            // editbtn
            // 
            this.editbtn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "--->", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.editbtn.Name = "editbtn";
            this.editbtn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editbtn.ValidateOnEnterKey = true;
            this.editbtn.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editbtn_ButtonClick);
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnGrid_ButtonClick);
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(0, 0);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.85085F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.14915F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1080, 620);
            this.tlSeparatorPanel.TabIndex = 63;
            // 
            // grpctrlHeader
            // 
            this.grpctrlHeader.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.grpctrlHeader.Appearance.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.grpctrlHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.grpctrlHeader.AppearanceCaption.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.Options.UseFont = true;
            this.grpctrlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.grpctrlHeader.Controls.Add(this.lblFechaDoc);
            this.grpctrlHeader.Controls.Add(this.grpboxHeader);
            this.grpctrlHeader.Controls.Add(this.dtFechaDoc);
            this.grpctrlHeader.Controls.Add(this.dtPeriod);
            this.grpctrlHeader.Controls.Add(this.lblBreak);
            this.grpctrlHeader.Controls.Add(this.txtDocDesc);
            this.grpctrlHeader.Controls.Add(this.txtDocumentoID);
            this.grpctrlHeader.Controls.Add(this.txtNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblPeriod);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(12, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.Size = new System.Drawing.Size(1057, 112);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // lblFechaDoc
            // 
            this.lblFechaDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaDoc.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaDoc.Location = new System.Drawing.Point(585, 3);
            this.lblFechaDoc.Name = "lblFechaDoc";
            this.lblFechaDoc.Size = new System.Drawing.Size(89, 14);
            this.lblFechaDoc.TabIndex = 77;
            this.lblFechaDoc.Text = "186_FechaDoc";
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.BackColor = System.Drawing.Color.Transparent;
            this.grpboxHeader.Controls.Add(this.labelControl1);
            this.grpboxHeader.Controls.Add(this.dtFechaSentencia);
            this.grpboxHeader.Controls.Add(this.lblObligacion);
            this.grpboxHeader.Controls.Add(this.lkpObligaciones);
            this.grpboxHeader.Controls.Add(this.masterCliente);
            this.grpboxHeader.Controls.Add(this.cmbEstadoActual);
            this.grpboxHeader.Controls.Add(this.lblEstadoActual);
            this.grpboxHeader.Controls.Add(this.btnInteres);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Name = "grpboxHeader";
            this.grpboxHeader.Size = new System.Drawing.Size(1053, 88);
            this.grpboxHeader.TabIndex = 8;
            this.grpboxHeader.TabStop = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl1.Location = new System.Drawing.Point(389, 21);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 14);
            this.labelControl1.TabIndex = 79;
            this.labelControl1.Text = "Fecha Sentencia";
            // 
            // dtFechaSentencia
            // 
            this.dtFechaSentencia.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaSentencia.Location = new System.Drawing.Point(483, 17);
            this.dtFechaSentencia.Margin = new System.Windows.Forms.Padding(2);
            this.dtFechaSentencia.Name = "dtFechaSentencia";
            this.dtFechaSentencia.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaSentencia.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaSentencia.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaSentencia.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaSentencia.Properties.Appearance.Options.UseFont = true;
            this.dtFechaSentencia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaSentencia.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaSentencia.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaSentencia.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaSentencia.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaSentencia.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaSentencia.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaSentencia.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaSentencia.Size = new System.Drawing.Size(113, 20);
            this.dtFechaSentencia.TabIndex = 1;
            // 
            // lblObligacion
            // 
            this.lblObligacion.AutoSize = true;
            this.lblObligacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObligacion.Location = new System.Drawing.Point(13, 59);
            this.lblObligacion.Name = "lblObligacion";
            this.lblObligacion.Size = new System.Drawing.Size(90, 14);
            this.lblObligacion.TabIndex = 76;
            this.lblObligacion.Text = "186_Obligacion";
            // 
            // lkpObligaciones
            // 
            this.lkpObligaciones.Location = new System.Drawing.Point(130, 56);
            this.lkpObligaciones.Name = "lkpObligaciones";
            this.lkpObligaciones.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpObligaciones.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NumeroDoc", "NumDoc"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Libranza", 40, "Libranza")});
            this.lkpObligaciones.Properties.DisplayMember = "Libranza";
            this.lkpObligaciones.Properties.NullText = " ";
            this.lkpObligaciones.Properties.ValueMember = "NumeroDoc";
            this.lkpObligaciones.Size = new System.Drawing.Size(117, 20);
            this.lkpObligaciones.TabIndex = 2;
            this.lkpObligaciones.EditValueChanged += new System.EventHandler(this.lkpObligaciones_EditValueChanged);
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCliente.Location = new System.Drawing.Point(16, 13);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(363, 29);
            this.masterCliente.TabIndex = 0;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // cmbEstadoActual
            // 
            this.cmbEstadoActual.Location = new System.Drawing.Point(393, 56);
            this.cmbEstadoActual.Name = "cmbEstadoActual";
            this.cmbEstadoActual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstadoActual.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbEstadoActual.Properties.DisplayMember = "Value";
            this.cmbEstadoActual.Properties.NullText = " ";
            this.cmbEstadoActual.Properties.ValueMember = "Key";
            this.cmbEstadoActual.Size = new System.Drawing.Size(117, 20);
            this.cmbEstadoActual.TabIndex = 3;
            // 
            // lblEstadoActual
            // 
            this.lblEstadoActual.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoActual.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblEstadoActual.Location = new System.Drawing.Point(281, 59);
            this.lblEstadoActual.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblEstadoActual.Name = "lblEstadoActual";
            this.lblEstadoActual.Size = new System.Drawing.Size(110, 14);
            this.lblEstadoActual.TabIndex = 68;
            this.lblEstadoActual.Text = "186_lblEstadoActual";
            // 
            // btnInteres
            // 
            this.btnInteres.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInteres.Location = new System.Drawing.Point(937, 50);
            this.btnInteres.Name = "btnInteres";
            this.btnInteres.Size = new System.Drawing.Size(113, 23);
            this.btnInteres.TabIndex = 74;
            this.btnInteres.Text = "Reliquidar";
            this.btnInteres.UseVisualStyleBackColor = true;
            this.btnInteres.Visible = false;
            this.btnInteres.Click += new System.EventHandler(this.btnInteres_Click);
            // 
            // dtFechaDoc
            // 
            this.dtFechaDoc.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaDoc.Location = new System.Drawing.Point(700, 0);
            this.dtFechaDoc.Margin = new System.Windows.Forms.Padding(2);
            this.dtFechaDoc.Name = "dtFechaDoc";
            this.dtFechaDoc.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaDoc.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaDoc.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaDoc.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaDoc.Properties.Appearance.Options.UseFont = true;
            this.dtFechaDoc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaDoc.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaDoc.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaDoc.Size = new System.Drawing.Size(113, 20);
            this.dtFechaDoc.TabIndex = 78;
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(454, 1);
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(6);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 18);
            this.dtPeriod.TabIndex = 3;
            // 
            // lblBreak
            // 
            this.lblBreak.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBreak.Location = new System.Drawing.Point(67, 4);
            this.lblBreak.Margin = new System.Windows.Forms.Padding(4);
            this.lblBreak.Name = "lblBreak";
            this.lblBreak.Size = new System.Drawing.Size(5, 13);
            this.lblBreak.TabIndex = 7;
            this.lblBreak.Text = "-";
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Enabled = false;
            this.txtDocDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocDesc.Location = new System.Drawing.Point(75, 1);
            this.txtDocDesc.Multiline = true;
            this.txtDocDesc.Name = "txtDocDesc";
            this.txtDocDesc.Size = new System.Drawing.Size(217, 19);
            this.txtDocDesc.TabIndex = 1;
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Enabled = false;
            this.txtDocumentoID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoID.Location = new System.Drawing.Point(7, 1);
            this.txtDocumentoID.Multiline = true;
            this.txtDocumentoID.Name = "txtDocumentoID";
            this.txtDocumentoID.Size = new System.Drawing.Size(58, 19);
            this.txtDocumentoID.TabIndex = 0;
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Enabled = false;
            this.txtNumeroDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroDoc.Location = new System.Drawing.Point(321, 1);
            this.txtNumeroDoc.Multiline = true;
            this.txtNumeroDoc.Name = "txtNumeroDoc";
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 19);
            this.txtNumeroDoc.TabIndex = 2;
            // 
            // lblNumeroDoc
            // 
            this.lblNumeroDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroDoc.Location = new System.Drawing.Point(307, 4);
            this.lblNumeroDoc.Margin = new System.Windows.Forms.Padding(4);
            this.lblNumeroDoc.Name = "lblNumeroDoc";
            this.lblNumeroDoc.Size = new System.Drawing.Size(10, 14);
            this.lblNumeroDoc.TabIndex = 92;
            this.lblNumeroDoc.Text = "#";
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(392, 4);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(93, 14);
            this.lblPeriod.TabIndex = 82;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.groupBox1);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(12, 412);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1057, 205);
            this.pnlDetail.TabIndex = 112;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbPlazoSent);
            this.groupBox1.Controls.Add(this.dtFecha1CuotSent);
            this.groupBox1.Controls.Add(this.txtValorCuota);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.lblValor);
            this.groupBox1.Controls.Add(this.lblPlazo);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(7, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(564, 59);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Plan de Pagos Sentencia";
            // 
            // cmbPlazoSent
            // 
            this.cmbPlazoSent.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPlazoSent.FormattingEnabled = true;
            this.cmbPlazoSent.Items.AddRange(new object[] {
            "3",
            "6",
            "10",
            "12",
            "15",
            "18",
            "20",
            "24",
            "25",
            "28",
            "30",
            "35",
            "36",
            "40",
            "42",
            "45",
            "48",
            "50",
            "54",
            "55",
            "60",
            "65",
            "66",
            "70",
            "72",
            "75",
            "78",
            "80",
            "84"});
            this.cmbPlazoSent.Location = new System.Drawing.Point(60, 25);
            this.cmbPlazoSent.Name = "cmbPlazoSent";
            this.cmbPlazoSent.Size = new System.Drawing.Size(58, 22);
            this.cmbPlazoSent.TabIndex = 0;
            this.cmbPlazoSent.SelectedIndexChanged += new System.EventHandler(this.cmbPlazoSent_SelectedIndexChanged);
            this.cmbPlazoSent.TextChanged += new System.EventHandler(this.cmbPlazoSent_TextChanged);
            // 
            // dtFecha1CuotSent
            // 
            this.dtFecha1CuotSent.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFecha1CuotSent.Location = new System.Drawing.Point(448, 26);
            this.dtFecha1CuotSent.Margin = new System.Windows.Forms.Padding(2);
            this.dtFecha1CuotSent.Name = "dtFecha1CuotSent";
            this.dtFecha1CuotSent.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFecha1CuotSent.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha1CuotSent.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFecha1CuotSent.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha1CuotSent.Properties.Appearance.Options.UseFont = true;
            this.dtFecha1CuotSent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha1CuotSent.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha1CuotSent.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha1CuotSent.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha1CuotSent.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha1CuotSent.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha1CuotSent.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha1CuotSent.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha1CuotSent.Size = new System.Drawing.Size(91, 20);
            this.dtFecha1CuotSent.TabIndex = 2;
            // 
            // txtValorCuota
            // 
            this.txtValorCuota.EditValue = "$ 0";
            this.txtValorCuota.Location = new System.Drawing.Point(221, 26);
            this.txtValorCuota.Name = "txtValorCuota";
            this.txtValorCuota.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorCuota.Properties.Appearance.Options.UseFont = true;
            this.txtValorCuota.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorCuota.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorCuota.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorCuota.Properties.Mask.EditMask = "c0";
            this.txtValorCuota.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorCuota.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorCuota.Size = new System.Drawing.Size(93, 20);
            this.txtValorCuota.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl2.Location = new System.Drawing.Point(354, 29);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(85, 14);
            this.labelControl2.TabIndex = 81;
            this.labelControl2.Text = "Fecha 1° Cuota";
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.Location = new System.Drawing.Point(148, 29);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(70, 14);
            this.lblValor.TabIndex = 82;
            this.lblValor.Text = "Valor Cuota";
            // 
            // lblPlazo
            // 
            this.lblPlazo.AutoSize = true;
            this.lblPlazo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlazo.Location = new System.Drawing.Point(22, 29);
            this.lblPlazo.Name = "lblPlazo";
            this.lblPlazo.Size = new System.Drawing.Size(34, 14);
            this.lblPlazo.TabIndex = 85;
            this.lblPlazo.Text = "Plazo";
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gbGridDocument);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(12, 121);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1057, 285);
            this.pnlGrids.TabIndex = 113;
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.gcDocument);
            this.gbGridDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGridDocument.Location = new System.Drawing.Point(0, 0);
            this.gbGridDocument.Name = "gbGridDocument";
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(6, 0, 6, 3);
            this.gbGridDocument.Size = new System.Drawing.Size(1057, 285);
            this.gbGridDocument.TabIndex = 54;
            this.gbGridDocument.TabStop = false;
            // 
            // SentenciaJudicial
            // 
            this.ClientSize = new System.Drawing.Size(1080, 620);
            this.Controls.Add(this.tlSeparatorPanel);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "SentenciaJudicial";
            this.Text = "32564";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editbtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            this.tlSeparatorPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpctrlHeader.PerformLayout();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSentencia.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSentencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpObligaciones.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstadoActual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha1CuotSent.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha1CuotSent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCuota.Properties)).EndInit();
            this.pnlGrids.ResumeLayout(false);
            this.gbGridDocument.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private System.ComponentModel.IContainer components;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private System.Windows.Forms.Panel pnlDetail;
        protected System.Windows.Forms.Panel pnlGrids;
        protected System.Windows.Forms.GroupBox gbGridDocument;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        public System.Windows.Forms.GroupBox grpboxHeader;
        private ControlsUC.uc_MasterFind masterCliente;
        private DevExpress.XtraEditors.LookUpEdit cmbEstadoActual;
        private DevExpress.XtraEditors.LabelControl lblEstadoActual;
        private System.Windows.Forms.Button btnInteres;
        protected ControlsUC.uc_PeriodoEdit dtPeriod;
        private DevExpress.XtraEditors.LabelControl lblBreak;
        protected System.Windows.Forms.TextBox txtDocDesc;
        protected System.Windows.Forms.TextBox txtDocumentoID;
        protected System.Windows.Forms.TextBox txtNumeroDoc;
        private DevExpress.XtraEditors.LabelControl lblNumeroDoc;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private System.Windows.Forms.Label lblObligacion;
        private DevExpress.XtraEditors.LookUpEdit lkpObligaciones;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editbtn;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        private DevExpress.XtraEditors.LabelControl lblFechaDoc;
        protected DevExpress.XtraEditors.DateEdit dtFechaDoc;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        protected DevExpress.XtraEditors.DateEdit dtFechaSentencia;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        protected DevExpress.XtraEditors.DateEdit dtFecha1CuotSent;
        private Clases.ComboBoxEx cmbPlazoSent;
        private DevExpress.XtraEditors.TextEdit txtValorCuota;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.Label lblPlazo;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}