namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MovimientosActivos
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.grpValores = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtValorRetiro = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbTipoDecpreciacionUSG = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.txtVidaUtilUSG = new System.Windows.Forms.TextBox();
            this.txtValorSalvamentUSG = new DevExpress.XtraEditors.TextEdit();
            this.txtValorSalvamentoLocal = new DevExpress.XtraEditors.TextEdit();
            this.cmbTipoDecpreciacionLocal = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.cmbTipoDecpreciacionIFRS = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.txtVidaUtilIFRS = new System.Windows.Forms.TextBox();
            this.txtVidaUtilLocal = new System.Windows.Forms.TextBox();
            this.txtValorSalvamentIFRS = new DevExpress.XtraEditors.TextEdit();
            this.lblValorResidual = new DevExpress.XtraEditors.LabelControl();
            this.lblLibroLocal = new DevExpress.XtraEditors.LabelControl();
            this.lblLibroUSGAP = new DevExpress.XtraEditors.LabelControl();
            this.lblTipoDepreciacion = new DevExpress.XtraEditors.LabelControl();
            this.lblVidaUtil = new DevExpress.XtraEditors.LabelControl();
            this.uc_Proyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_CentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_LocFisica = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_TipoMovimiento = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnActivos = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.uc_Tercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_Responsable = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
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
            this.gbGridProvider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProvider)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpValores)).BeginInit();
            this.grpValores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorRetiro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorSalvamentUSG.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorSalvamentoLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorSalvamentIFRS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnActivos.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.grpValores);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Size = new System.Drawing.Size(999, 167);
            // 
            // gcDocument
            // 
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
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.EmbeddedNavigator_ButtonClick);
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            gridLevelNode2.LevelTemplate = this.gvDetalle;
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2});
            this.gcDocument.Location = new System.Drawing.Point(4, 13);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.Size = new System.Drawing.Size(695, 228);
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
            this.gvDocument.OptionsView.ShowGroupPanel = false;
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
            this.btnMark.Margin = new System.Windows.Forms.Padding(2);
            this.btnMark.Size = new System.Drawing.Size(49, 20);
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
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
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
            this.gbGridDocument.Size = new System.Drawing.Size(703, 243);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(703, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(2);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 243);
            // 
            // gcProvider
            // 
            this.gcProvider.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcProvider.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcProvider.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcProvider.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcProvider.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcProvider.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcProvider.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcProvider.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcProvider.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcProvider.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcProvider.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcProvider.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcProvider.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcProvider.Location = new System.Drawing.Point(6, 15);
            this.gcProvider.LookAndFeel.SkinName = "Dark Side";
            this.gcProvider.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcProvider.Size = new System.Drawing.Size(284, 226);
            // 
            // gvProvider
            // 
            this.gvProvider.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvProvider.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvProvider.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvProvider.Appearance.Empty.Options.UseBackColor = true;
            this.gvProvider.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvProvider.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvProvider.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProvider.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvProvider.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvProvider.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvProvider.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProvider.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvProvider.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvProvider.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvProvider.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvProvider.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvProvider.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvProvider.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvProvider.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvProvider.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvProvider.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvProvider.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvProvider.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvProvider.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvProvider.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvProvider.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvProvider.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvProvider.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProvider.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvProvider.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvProvider.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvProvider.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvProvider.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvProvider.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvProvider.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvProvider.Appearance.Row.Options.UseBackColor = true;
            this.gvProvider.Appearance.Row.Options.UseForeColor = true;
            this.gvProvider.Appearance.Row.Options.UseTextOptions = true;
            this.gvProvider.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvProvider.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvProvider.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvProvider.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvProvider.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvProvider.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvProvider.Appearance.VertLine.Options.UseBackColor = true;
            this.gvProvider.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvProvider.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvProvider.OptionsCustomization.AllowFilter = false;
            this.gvProvider.OptionsCustomization.AllowSort = false;
            this.gvProvider.OptionsMenu.EnableColumnMenu = false;
            this.gvProvider.OptionsMenu.EnableFooterMenu = false;
            this.gvProvider.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvProvider.OptionsView.ColumnAutoWidth = false;
            this.gvProvider.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvProvider.OptionsView.ShowGroupPanel = false;
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.uc_Tercero);
            this.grpboxHeader.Controls.Add(this.labelControl3);
            this.grpboxHeader.Controls.Add(this.btnActivos);
            this.grpboxHeader.Controls.Add(this.uc_TipoMovimiento);
            this.grpboxHeader.Controls.Add(this.uc_LocFisica);
            this.grpboxHeader.Controls.Add(this.uc_CentroCosto);
            this.grpboxHeader.Controls.Add(this.uc_Proyecto);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Size = new System.Drawing.Size(995, 125);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "c2";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Empty.Options.UseFont = true;
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedCell.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedRow.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.HideSelectionRow.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDetalle.Appearance.Row.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.SelectedRow.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsBehavior.Editable = false;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            // 
            // grpValores
            // 
            this.grpValores.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.grpValores.Controls.Add(this.labelControl2);
            this.grpValores.Controls.Add(this.txtValorRetiro);
            this.grpValores.Controls.Add(this.labelControl1);
            this.grpValores.Controls.Add(this.cmbTipoDecpreciacionUSG);
            this.grpValores.Controls.Add(this.txtVidaUtilUSG);
            this.grpValores.Controls.Add(this.txtValorSalvamentUSG);
            this.grpValores.Controls.Add(this.txtValorSalvamentoLocal);
            this.grpValores.Controls.Add(this.cmbTipoDecpreciacionLocal);
            this.grpValores.Controls.Add(this.cmbTipoDecpreciacionIFRS);
            this.grpValores.Controls.Add(this.txtVidaUtilIFRS);
            this.grpValores.Controls.Add(this.txtVidaUtilLocal);
            this.grpValores.Controls.Add(this.txtValorSalvamentIFRS);
            this.grpValores.Controls.Add(this.lblValorResidual);
            this.grpValores.Controls.Add(this.lblLibroLocal);
            this.grpValores.Controls.Add(this.lblLibroUSGAP);
            this.grpValores.Controls.Add(this.lblTipoDepreciacion);
            this.grpValores.Controls.Add(this.lblVidaUtil);
            this.grpValores.Location = new System.Drawing.Point(7, 12);
            this.grpValores.Name = "grpValores";
            this.grpValores.Size = new System.Drawing.Size(531, 135);
            this.grpValores.TabIndex = 112;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(6, 107);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(104, 14);
            this.labelControl2.TabIndex = 152;
            this.labelControl2.Text = "61_ValorRetiroIFRS";
            // 
            // txtValorRetiro
            // 
            this.txtValorRetiro.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtValorRetiro.EditValue = "0";
            this.txtValorRetiro.Enabled = false;
            this.txtValorRetiro.Location = new System.Drawing.Point(382, 105);
            this.txtValorRetiro.Name = "txtValorRetiro";
            this.txtValorRetiro.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.txtValorRetiro.Properties.Appearance.Options.UseBackColor = true;
            this.txtValorRetiro.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorRetiro.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorRetiro.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorRetiro.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorRetiro.Properties.Mask.EditMask = "c";
            this.txtValorRetiro.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorRetiro.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorRetiro.Size = new System.Drawing.Size(109, 20);
            this.txtValorRetiro.TabIndex = 151;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(392, 2);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(82, 14);
            this.labelControl1.TabIndex = 150;
            this.labelControl1.Text = "61_lblLibroIFRS";
            // 
            // cmbTipoDecpreciacionUSG
            // 
            this.cmbTipoDecpreciacionUSG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoDecpreciacionUSG.Enabled = false;
            this.cmbTipoDecpreciacionUSG.FormattingEnabled = true;
            this.cmbTipoDecpreciacionUSG.Location = new System.Drawing.Point(261, 27);
            this.cmbTipoDecpreciacionUSG.Name = "cmbTipoDecpreciacionUSG";
            this.cmbTipoDecpreciacionUSG.Size = new System.Drawing.Size(109, 21);
            this.cmbTipoDecpreciacionUSG.TabIndex = 147;
            // 
            // txtVidaUtilUSG
            // 
            this.txtVidaUtilUSG.BackColor = System.Drawing.Color.White;
            this.txtVidaUtilUSG.Location = new System.Drawing.Point(261, 52);
            this.txtVidaUtilUSG.Name = "txtVidaUtilUSG";
            this.txtVidaUtilUSG.Size = new System.Drawing.Size(109, 20);
            this.txtVidaUtilUSG.TabIndex = 148;
            // 
            // txtValorSalvamentUSG
            // 
            this.txtValorSalvamentUSG.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtValorSalvamentUSG.EditValue = "0";
            this.txtValorSalvamentUSG.Enabled = false;
            this.txtValorSalvamentUSG.Location = new System.Drawing.Point(262, 77);
            this.txtValorSalvamentUSG.Name = "txtValorSalvamentUSG";
            this.txtValorSalvamentUSG.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.txtValorSalvamentUSG.Properties.Appearance.Options.UseBackColor = true;
            this.txtValorSalvamentUSG.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorSalvamentUSG.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorSalvamentUSG.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorSalvamentUSG.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorSalvamentUSG.Properties.Mask.EditMask = "c";
            this.txtValorSalvamentUSG.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorSalvamentUSG.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorSalvamentUSG.Size = new System.Drawing.Size(109, 20);
            this.txtValorSalvamentUSG.TabIndex = 149;
            // 
            // txtValorSalvamentoLocal
            // 
            this.txtValorSalvamentoLocal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtValorSalvamentoLocal.EditValue = "0";
            this.txtValorSalvamentoLocal.Enabled = false;
            this.txtValorSalvamentoLocal.Location = new System.Drawing.Point(142, 77);
            this.txtValorSalvamentoLocal.Name = "txtValorSalvamentoLocal";
            this.txtValorSalvamentoLocal.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.txtValorSalvamentoLocal.Properties.Appearance.Options.UseBackColor = true;
            this.txtValorSalvamentoLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorSalvamentoLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorSalvamentoLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorSalvamentoLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorSalvamentoLocal.Properties.Mask.EditMask = "c";
            this.txtValorSalvamentoLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorSalvamentoLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorSalvamentoLocal.Size = new System.Drawing.Size(109, 20);
            this.txtValorSalvamentoLocal.TabIndex = 2;
            // 
            // cmbTipoDecpreciacionLocal
            // 
            this.cmbTipoDecpreciacionLocal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoDecpreciacionLocal.Enabled = false;
            this.cmbTipoDecpreciacionLocal.FormattingEnabled = true;
            this.cmbTipoDecpreciacionLocal.Location = new System.Drawing.Point(140, 26);
            this.cmbTipoDecpreciacionLocal.Name = "cmbTipoDecpreciacionLocal";
            this.cmbTipoDecpreciacionLocal.Size = new System.Drawing.Size(109, 21);
            this.cmbTipoDecpreciacionLocal.TabIndex = 0;
            // 
            // cmbTipoDecpreciacionIFRS
            // 
            this.cmbTipoDecpreciacionIFRS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoDecpreciacionIFRS.Enabled = false;
            this.cmbTipoDecpreciacionIFRS.FormattingEnabled = true;
            this.cmbTipoDecpreciacionIFRS.Location = new System.Drawing.Point(380, 27);
            this.cmbTipoDecpreciacionIFRS.Name = "cmbTipoDecpreciacionIFRS";
            this.cmbTipoDecpreciacionIFRS.Size = new System.Drawing.Size(109, 21);
            this.cmbTipoDecpreciacionIFRS.TabIndex = 3;
            // 
            // txtVidaUtilIFRS
            // 
            this.txtVidaUtilIFRS.BackColor = System.Drawing.Color.White;
            this.txtVidaUtilIFRS.Location = new System.Drawing.Point(381, 52);
            this.txtVidaUtilIFRS.Name = "txtVidaUtilIFRS";
            this.txtVidaUtilIFRS.Size = new System.Drawing.Size(109, 20);
            this.txtVidaUtilIFRS.TabIndex = 4;
            // 
            // txtVidaUtilLocal
            // 
            this.txtVidaUtilLocal.BackColor = System.Drawing.Color.White;
            this.txtVidaUtilLocal.Location = new System.Drawing.Point(141, 51);
            this.txtVidaUtilLocal.Name = "txtVidaUtilLocal";
            this.txtVidaUtilLocal.Size = new System.Drawing.Size(109, 20);
            this.txtVidaUtilLocal.TabIndex = 1;
            // 
            // txtValorSalvamentIFRS
            // 
            this.txtValorSalvamentIFRS.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtValorSalvamentIFRS.EditValue = "0";
            this.txtValorSalvamentIFRS.Enabled = false;
            this.txtValorSalvamentIFRS.Location = new System.Drawing.Point(382, 78);
            this.txtValorSalvamentIFRS.Name = "txtValorSalvamentIFRS";
            this.txtValorSalvamentIFRS.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.txtValorSalvamentIFRS.Properties.Appearance.Options.UseBackColor = true;
            this.txtValorSalvamentIFRS.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorSalvamentIFRS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorSalvamentIFRS.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorSalvamentIFRS.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorSalvamentIFRS.Properties.Mask.EditMask = "c";
            this.txtValorSalvamentIFRS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorSalvamentIFRS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorSalvamentIFRS.Size = new System.Drawing.Size(109, 20);
            this.txtValorSalvamentIFRS.TabIndex = 5;
            // 
            // lblValorResidual
            // 
            this.lblValorResidual.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorResidual.Location = new System.Drawing.Point(6, 80);
            this.lblValorResidual.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorResidual.Name = "lblValorResidual";
            this.lblValorResidual.Size = new System.Drawing.Size(122, 14);
            this.lblValorResidual.TabIndex = 146;
            this.lblValorResidual.Text = "61_lblValorSalvamento";
            // 
            // lblLibroLocal
            // 
            this.lblLibroLocal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibroLocal.Location = new System.Drawing.Point(152, 2);
            this.lblLibroLocal.Margin = new System.Windows.Forms.Padding(4);
            this.lblLibroLocal.Name = "lblLibroLocal";
            this.lblLibroLocal.Size = new System.Drawing.Size(85, 14);
            this.lblLibroLocal.TabIndex = 140;
            this.lblLibroLocal.Text = "61_lblLibroLocal";
            // 
            // lblLibroUSGAP
            // 
            this.lblLibroUSGAP.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibroUSGAP.Location = new System.Drawing.Point(276, 2);
            this.lblLibroUSGAP.Margin = new System.Windows.Forms.Padding(4);
            this.lblLibroUSGAP.Name = "lblLibroUSGAP";
            this.lblLibroUSGAP.Size = new System.Drawing.Size(74, 14);
            this.lblLibroUSGAP.TabIndex = 141;
            this.lblLibroUSGAP.Text = "61_lblUS-GAP";
            // 
            // lblTipoDepreciacion
            // 
            this.lblTipoDepreciacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoDepreciacion.Location = new System.Drawing.Point(6, 30);
            this.lblTipoDepreciacion.Margin = new System.Windows.Forms.Padding(4);
            this.lblTipoDepreciacion.Name = "lblTipoDepreciacion";
            this.lblTipoDepreciacion.Size = new System.Drawing.Size(125, 14);
            this.lblTipoDepreciacion.TabIndex = 137;
            this.lblTipoDepreciacion.Text = "61_lblTipoDepreciacion";
            // 
            // lblVidaUtil
            // 
            this.lblVidaUtil.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVidaUtil.Location = new System.Drawing.Point(6, 56);
            this.lblVidaUtil.Margin = new System.Windows.Forms.Padding(4);
            this.lblVidaUtil.Name = "lblVidaUtil";
            this.lblVidaUtil.Size = new System.Drawing.Size(72, 14);
            this.lblVidaUtil.TabIndex = 138;
            this.lblVidaUtil.Text = "61_lblVidaUtil";
            // 
            // uc_Proyecto
            // 
            this.uc_Proyecto.BackColor = System.Drawing.Color.Transparent;
            this.uc_Proyecto.Filtros = null;
            this.uc_Proyecto.Location = new System.Drawing.Point(11, 35);
            this.uc_Proyecto.Name = "uc_Proyecto";
            this.uc_Proyecto.Size = new System.Drawing.Size(291, 22);
            this.uc_Proyecto.TabIndex = 1;
            this.uc_Proyecto.Value = "";
            // 
            // uc_CentroCosto
            // 
            this.uc_CentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.uc_CentroCosto.Filtros = null;
            this.uc_CentroCosto.Location = new System.Drawing.Point(308, 35);
            this.uc_CentroCosto.Name = "uc_CentroCosto";
            this.uc_CentroCosto.Size = new System.Drawing.Size(291, 22);
            this.uc_CentroCosto.TabIndex = 2;
            this.uc_CentroCosto.Value = "";
            // 
            // uc_LocFisica
            // 
            this.uc_LocFisica.BackColor = System.Drawing.Color.Transparent;
            this.uc_LocFisica.Filtros = null;
            this.uc_LocFisica.Location = new System.Drawing.Point(614, 35);
            this.uc_LocFisica.Name = "uc_LocFisica";
            this.uc_LocFisica.Size = new System.Drawing.Size(291, 22);
            this.uc_LocFisica.TabIndex = 3;
            this.uc_LocFisica.Value = "";
            // 
            // uc_TipoMovimiento
            // 
            this.uc_TipoMovimiento.BackColor = System.Drawing.Color.Transparent;
            this.uc_TipoMovimiento.Filtros = null;
            this.uc_TipoMovimiento.Location = new System.Drawing.Point(11, 73);
            this.uc_TipoMovimiento.Name = "uc_TipoMovimiento";
            this.uc_TipoMovimiento.Size = new System.Drawing.Size(291, 25);
            this.uc_TipoMovimiento.TabIndex = 4;
            this.uc_TipoMovimiento.Value = "";
            this.uc_TipoMovimiento.Leave += new System.EventHandler(this.uc_TipoMovimiento_Leave);
            // 
            // btnActivos
            // 
            this.btnActivos.Location = new System.Drawing.Point(111, 13);
            this.btnActivos.Name = "btnActivos";
            this.btnActivos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btnActivos.Size = new System.Drawing.Size(100, 20);
            this.btnActivos.TabIndex = 0;
            this.btnActivos.Click += new System.EventHandler(this.btnActivos_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl3.Location = new System.Drawing.Point(11, 16);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(55, 14);
            this.labelControl3.TabIndex = 120;
            this.labelControl3.Text = "61_Activo";
            // 
            // uc_Tercero
            // 
            this.uc_Tercero.BackColor = System.Drawing.Color.Transparent;
            this.uc_Tercero.Filtros = null;
            this.uc_Tercero.Location = new System.Drawing.Point(308, 73);
            this.uc_Tercero.Name = "uc_Tercero";
            this.uc_Tercero.Size = new System.Drawing.Size(291, 25);
            this.uc_Tercero.TabIndex = 5;
            this.uc_Tercero.Value = "";
            // 
            // uc_Responsable
            // 
            this.uc_Responsable.BackColor = System.Drawing.Color.Transparent;
            this.uc_Responsable.Filtros = null;
            this.uc_Responsable.Location = new System.Drawing.Point(11, 71);
            this.uc_Responsable.Name = "uc_Responsable";
            this.uc_Responsable.Size = new System.Drawing.Size(291, 25);
            this.uc_Responsable.TabIndex = 126;
            this.uc_Responsable.Value = "";
            // 
            // MovimientosActivos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1027, 581);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MovimientosActivos";
            this.Text = "61";
            this.grpboxDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
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
            this.gbGridProvider.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProvider)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpValores)).EndInit();
            this.grpValores.ResumeLayout(false);
            this.grpValores.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorRetiro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorSalvamentUSG.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorSalvamentoLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorSalvamentIFRS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnActivos.Properties)).EndInit();
            this.ResumeLayout(false);

        }

       

        #endregion

        private DevExpress.XtraEditors.GroupControl grpValores;
        private DevExpress.XtraEditors.LabelControl lblLibroLocal;
        private DevExpress.XtraEditors.LabelControl lblLibroUSGAP;
        private DevExpress.XtraEditors.LabelControl lblTipoDepreciacion;
        private DevExpress.XtraEditors.LabelControl lblVidaUtil;
        private DevExpress.XtraEditors.TextEdit txtValorSalvamentIFRS;
        private DevExpress.XtraEditors.LabelControl lblValorResidual;
        private System.Windows.Forms.TextBox txtVidaUtilIFRS;
        private System.Windows.Forms.TextBox txtVidaUtilLocal;
        private Clases.ComboBoxEx cmbTipoDecpreciacionLocal;
        private Clases.ComboBoxEx cmbTipoDecpreciacionIFRS;
        private DevExpress.XtraEditors.TextEdit txtValorSalvamentoLocal;
        private ControlsUC.uc_MasterFind uc_CentroCosto;
        private ControlsUC.uc_MasterFind uc_Proyecto;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private Clases.ComboBoxEx cmbTipoDecpreciacionUSG;
        private System.Windows.Forms.TextBox txtVidaUtilUSG;
        private DevExpress.XtraEditors.TextEdit txtValorSalvamentUSG;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtValorRetiro;
        private ControlsUC.uc_MasterFind uc_LocFisica;
        private ControlsUC.uc_MasterFind uc_TipoMovimiento;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ButtonEdit btnActivos;
        private ControlsUC.uc_MasterFind uc_Tercero;
        private ControlsUC.uc_MasterFind uc_Responsable;
    }
}