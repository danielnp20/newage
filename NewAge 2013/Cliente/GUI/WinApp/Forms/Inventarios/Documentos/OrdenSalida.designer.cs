namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class OrdenSalida
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrdenSalida));
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcTarea = new DevExpress.XtraGrid.GridControl();
            this.gvTarea = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblResumenMvtoImport = new System.Windows.Forms.Label();
            this.masterBodega = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterPrefijoProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblNro = new System.Windows.Forms.Label();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.txtNroProyecto = new System.Windows.Forms.TextBox();
            this.lblItems = new System.Windows.Forms.Label();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.txtDesc = new DevExpress.XtraEditors.MemoEdit();
            this.lblLicitacion = new DevExpress.XtraEditors.LabelControl();
            this.txtLicitacion = new DevExpress.XtraEditors.MemoEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblnroOrden = new DevExpress.XtraEditors.LabelControl();
            this.masterPrefijoOrden = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnDocQueryOrden = new DevExpress.XtraEditors.SimpleButton();
            this.txtNroOrden = new DevExpress.XtraEditors.TextEdit();
            this.grpboxDetail.SuspendLayout();
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
            this.gbGridDocument.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTarea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroOrden.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.lblItems);
            this.grpboxDetail.Controls.Add(this.gcTarea);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Padding = new System.Windows.Forms.Padding(12, 11, 6, 6);
            this.grpboxDetail.Size = new System.Drawing.Size(988, 104);
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
            this.txtDocumentoID.Size = new System.Drawing.Size(58, 22);
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Margin = new System.Windows.Forms.Padding(4);
            this.txtDocDesc.Size = new System.Drawing.Size(217, 22);
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
            this.gbGridDocument.Size = new System.Drawing.Size(692, 280);
            this.gbGridDocument.Controls.SetChildIndex(this.lblResumenMvtoImport, 0);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(692, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 280);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.lblnroOrden);
            this.grpboxHeader.Controls.Add(this.masterPrefijoOrden);
            this.grpboxHeader.Controls.Add(this.btnDocQueryOrden);
            this.grpboxHeader.Controls.Add(this.txtNroOrden);
            this.grpboxHeader.Controls.Add(this.groupControl1);
            this.grpboxHeader.Controls.Add(this.masterBodega);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(984, 166);
            this.grpboxHeader.TabIndex = 0;
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // gridView1
            // 
            this.gridView1.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gridView1.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gridView1.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.Empty.Options.UseBackColor = true;
            this.gridView1.Appearance.Empty.Options.UseFont = true;
            this.gridView1.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gridView1.Appearance.FixedLine.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedCell.Options.UseFont = true;
            this.gridView1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridView1.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gridView1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gridView1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridView1.Appearance.FocusedRow.Options.UseFont = true;
            this.gridView1.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridView1.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gridView1.Appearance.GroupRow.Options.UseForeColor = true;
            this.gridView1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gridView1.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView1.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridView1.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridView1.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gridView1.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gridView1.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gridView1.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gridView1.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridView1.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gridView1.Appearance.Row.Options.UseBackColor = true;
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.Appearance.Row.Options.UseForeColor = true;
            this.gridView1.Appearance.Row.Options.UseTextOptions = true;
            this.gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridView1.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridView1.Appearance.SelectedRow.Options.UseFont = true;
            this.gridView1.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gridView1.Appearance.TopNewRow.Options.UseFont = true;
            this.gridView1.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gridView1.GridControl = this.gcTarea;
            this.gridView1.HorzScrollStep = 50;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            // 
            // gcTarea
            // 
            this.gcTarea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTarea.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcTarea.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcTarea.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcTarea.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcTarea.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcTarea.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcTarea.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcTarea.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcTarea.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcTarea.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcTarea.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gridView1;
            gridLevelNode1.RelationName = "Detalle";
            this.gcTarea.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcTarea.Location = new System.Drawing.Point(12, 25);
            this.gcTarea.LookAndFeel.SkinName = "Dark Side";
            this.gcTarea.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcTarea.MainView = this.gvTarea;
            this.gcTarea.Margin = new System.Windows.Forms.Padding(5);
            this.gcTarea.Name = "gcTarea";
            this.gcTarea.Size = new System.Drawing.Size(970, 73);
            this.gcTarea.TabIndex = 51;
            this.gcTarea.UseEmbeddedNavigator = true;
            this.gcTarea.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTarea,
            this.gridView1});
            // 
            // gvTarea
            // 
            this.gvTarea.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTarea.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvTarea.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.Empty.Options.UseBackColor = true;
            this.gvTarea.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvTarea.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvTarea.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvTarea.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvTarea.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTarea.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvTarea.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvTarea.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvTarea.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvTarea.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTarea.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvTarea.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.Row.Options.UseBackColor = true;
            this.gvTarea.Appearance.Row.Options.UseForeColor = true;
            this.gvTarea.Appearance.Row.Options.UseTextOptions = true;
            this.gvTarea.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvTarea.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTarea.Appearance.VertLine.Options.UseBackColor = true;
            this.gvTarea.GridControl = this.gcTarea;
            this.gvTarea.HorzScrollStep = 50;
            this.gvTarea.Name = "gvTarea";
            this.gvTarea.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvTarea.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvTarea.OptionsCustomization.AllowColumnMoving = false;
            this.gvTarea.OptionsCustomization.AllowFilter = false;
            this.gvTarea.OptionsCustomization.AllowSort = false;
            this.gvTarea.OptionsMenu.EnableColumnMenu = false;
            this.gvTarea.OptionsMenu.EnableFooterMenu = false;
            this.gvTarea.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvTarea.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvTarea.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvTarea.OptionsView.ColumnAutoWidth = false;
            this.gvTarea.OptionsView.ShowGroupPanel = false;
            this.gvTarea.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvTarea.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvTarea_FocusedRowChanged);
            this.gvTarea.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvTarea_CellValueChanged);
            this.gvTarea.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvTarea_BeforeLeaveRow);
            this.gvTarea.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvTarea_CustomUnboundColumnData);
            // 
            // lblResumenMvtoImport
            // 
            this.lblResumenMvtoImport.AutoSize = true;
            this.lblResumenMvtoImport.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResumenMvtoImport.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumenMvtoImport.Location = new System.Drawing.Point(12, -3);
            this.lblResumenMvtoImport.Name = "lblResumenMvtoImport";
            this.lblResumenMvtoImport.Size = new System.Drawing.Size(111, 14);
            this.lblResumenMvtoImport.TabIndex = 51;
            this.lblResumenMvtoImport.Text = "59_lblExistencias";
            // 
            // masterBodega
            // 
            this.masterBodega.BackColor = System.Drawing.Color.Transparent;
            this.masterBodega.Filtros = null;
            this.masterBodega.Location = new System.Drawing.Point(14, 12);
            this.masterBodega.Margin = new System.Windows.Forms.Padding(4);
            this.masterBodega.Name = "masterBodega";
            this.masterBodega.Size = new System.Drawing.Size(305, 27);
            this.masterBodega.TabIndex = 28;
            this.masterBodega.Value = "";
            this.masterBodega.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(6, 23);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(305, 24);
            this.masterProyecto.TabIndex = 29;
            this.masterProyecto.Value = "";
            this.masterProyecto.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterPrefijoProyecto
            // 
            this.masterPrefijoProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoProyecto.Filtros = null;
            this.masterPrefijoProyecto.Location = new System.Drawing.Point(352, 25);
            this.masterPrefijoProyecto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.masterPrefijoProyecto.Name = "masterPrefijoProyecto";
            this.masterPrefijoProyecto.Size = new System.Drawing.Size(291, 22);
            this.masterPrefijoProyecto.TabIndex = 32;
            this.masterPrefijoProyecto.Value = "";
            // 
            // lblNro
            // 
            this.lblNro.AutoSize = true;
            this.lblNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNro.Location = new System.Drawing.Point(643, 30);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(58, 14);
            this.lblNro.TabIndex = 34;
            this.lblNro.Text = "59_lblNro";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Enabled = false;
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(714, 26);
            this.btnQueryDoc.LookAndFeel.SkinName = "Black";
            this.btnQueryDoc.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(26, 21);
            this.btnQueryDoc.TabIndex = 35;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnDocQuery_Click);
            // 
            // txtNroProyecto
            // 
            this.txtNroProyecto.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroProyecto.Location = new System.Drawing.Point(686, 26);
            this.txtNroProyecto.Name = "txtNroProyecto";
            this.txtNroProyecto.Size = new System.Drawing.Size(26, 21);
            this.txtNroProyecto.TabIndex = 33;
            this.txtNroProyecto.Text = "0";
            this.txtNroProyecto.Leave += new System.EventHandler(this.txtNroOrdenTrab_Leave);
            // 
            // lblItems
            // 
            this.lblItems.AutoSize = true;
            this.lblItems.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblItems.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItems.Location = new System.Drawing.Point(10, 8);
            this.lblItems.Name = "lblItems";
            this.lblItems.Size = new System.Drawing.Size(80, 14);
            this.lblItems.TabIndex = 52;
            this.lblItems.Text = "59_lblItems";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDescripcion.Location = new System.Drawing.Point(352, 56);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(82, 13);
            this.lblDescripcion.TabIndex = 85;
            this.lblDescripcion.Text = "59_lblDescripcion";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(453, 53);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Properties.ReadOnly = true;
            this.txtDesc.Size = new System.Drawing.Size(283, 42);
            this.txtDesc.TabIndex = 84;
            // 
            // lblLicitacion
            // 
            this.lblLicitacion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblLicitacion.Location = new System.Drawing.Point(5, 54);
            this.lblLicitacion.Name = "lblLicitacion";
            this.lblLicitacion.Size = new System.Drawing.Size(71, 13);
            this.lblLicitacion.TabIndex = 83;
            this.lblLicitacion.Text = "59_lblLicitacion";
            // 
            // txtLicitacion
            // 
            this.txtLicitacion.Location = new System.Drawing.Point(105, 50);
            this.txtLicitacion.Name = "txtLicitacion";
            this.txtLicitacion.Properties.ReadOnly = true;
            this.txtLicitacion.Size = new System.Drawing.Size(197, 30);
            this.txtLicitacion.TabIndex = 78;
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.masterCliente);
            this.groupControl1.Controls.Add(this.masterProyecto);
            this.groupControl1.Controls.Add(this.lblDescripcion);
            this.groupControl1.Controls.Add(this.masterPrefijoProyecto);
            this.groupControl1.Controls.Add(this.txtDesc);
            this.groupControl1.Controls.Add(this.btnQueryDoc);
            this.groupControl1.Controls.Add(this.lblLicitacion);
            this.groupControl1.Controls.Add(this.lblNro);
            this.groupControl1.Controls.Add(this.txtLicitacion);
            this.groupControl1.Controls.Add(this.txtNroProyecto);
            this.groupControl1.Location = new System.Drawing.Point(10, 41);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(805, 118);
            this.groupControl1.TabIndex = 86;
            this.groupControl1.Text = "Seleccionar Proyecto";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(5, 81);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(306, 27);
            this.masterCliente.TabIndex = 115;
            this.masterCliente.Value = "";
            // 
            // lblnroOrden
            // 
            this.lblnroOrden.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblnroOrden.Location = new System.Drawing.Point(549, 19);
            this.lblnroOrden.Name = "lblnroOrden";
            this.lblnroOrden.Size = new System.Drawing.Size(45, 13);
            this.lblnroOrden.TabIndex = 122;
            this.lblnroOrden.Text = "59_lblNro";
            // 
            // masterPrefijoOrden
            // 
            this.masterPrefijoOrden.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoOrden.Filtros = null;
            this.masterPrefijoOrden.Location = new System.Drawing.Point(347, 13);
            this.masterPrefijoOrden.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterPrefijoOrden.Name = "masterPrefijoOrden";
            this.masterPrefijoOrden.Size = new System.Drawing.Size(200, 21);
            this.masterPrefijoOrden.TabIndex = 119;
            this.masterPrefijoOrden.Value = "";
            // 
            // btnDocQueryOrden
            // 
            this.btnDocQueryOrden.Image = ((System.Drawing.Image)(resources.GetObject("btnDocQueryOrden.Image")));
            this.btnDocQueryOrden.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDocQueryOrden.Location = new System.Drawing.Point(635, 15);
            this.btnDocQueryOrden.LookAndFeel.SkinName = "Black";
            this.btnDocQueryOrden.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnDocQueryOrden.Name = "btnDocQueryOrden";
            this.btnDocQueryOrden.Size = new System.Drawing.Size(24, 20);
            this.btnDocQueryOrden.TabIndex = 121;
            this.btnDocQueryOrden.ToolTip = "1005_btnQueryDoc";
            this.btnDocQueryOrden.Click += new System.EventHandler(this.btnDocQueryOrden_Click);
            // 
            // txtNroOrden
            // 
            this.txtNroOrden.Location = new System.Drawing.Point(601, 15);
            this.txtNroOrden.Name = "txtNroOrden";
            this.txtNroOrden.Size = new System.Drawing.Size(31, 20);
            this.txtNroOrden.TabIndex = 120;
            this.txtNroOrden.Leave += new System.EventHandler(this.txtNroOrden_Leave);
            // 
            // OrdenSalida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1016, 596);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "OrdenSalida";
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
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
            this.gbGridDocument.ResumeLayout(false);
            this.gbGridDocument.PerformLayout();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTarea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroOrden.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label lblResumenMvtoImport;
        private ControlsUC.uc_MasterFind masterBodega;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterPrefijoProyecto;
        private System.Windows.Forms.Label lblNro;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private System.Windows.Forms.TextBox txtNroProyecto;
        private System.Windows.Forms.Label lblItems;
        protected DevExpress.XtraGrid.GridControl gcTarea;
        protected DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvTarea;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private DevExpress.XtraEditors.MemoEdit txtDesc;
        private DevExpress.XtraEditors.LabelControl lblLicitacion;
        private DevExpress.XtraEditors.MemoEdit txtLicitacion;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private ControlsUC.uc_MasterFind masterCliente;
        private DevExpress.XtraEditors.LabelControl lblnroOrden;
        private ControlsUC.uc_MasterFind masterPrefijoOrden;
        private DevExpress.XtraEditors.SimpleButton btnDocQueryOrden;
        private DevExpress.XtraEditors.TextEdit txtNroOrden;
    }
}