namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AdicionInvActivos
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
            this.grpCtrlCurrency = new DevExpress.XtraEditors.GroupControl();
            this.lblCurrLocal = new DevExpress.XtraEditors.LabelControl();
            this.lblCurrForeign = new DevExpress.XtraEditors.LabelControl();
            this.lblDebit = new DevExpress.XtraEditors.LabelControl();
            this.lblCredit = new DevExpress.XtraEditors.LabelControl();
            this.lblTotal = new DevExpress.XtraEditors.LabelControl();
            this.txtDebLocal = new DevExpress.XtraEditors.TextEdit();
            this.txtCredLocal = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalLocal = new DevExpress.XtraEditors.TextEdit();
            this.txtDebForeign = new DevExpress.XtraEditors.TextEdit();
            this.txtCredForeign = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalForeign = new DevExpress.XtraEditors.TextEdit();
            this.grpValores = new DevExpress.XtraEditors.GroupControl();
            this.txtVlrRetiro = new System.Windows.Forms.TextBox();
            this.lbl_VlrRetiro = new DevExpress.XtraEditors.LabelControl();
            this.txtValorResidualML = new DevExpress.XtraEditors.TextEdit();
            this.cmbTipoDecpreciacionML = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.cmbTipoDecpreciacionME = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.txtVidaUtilIFRS = new System.Windows.Forms.TextBox();
            this.txtVidaUtilLocal = new System.Windows.Forms.TextBox();
            this.txtValorResidualME = new DevExpress.XtraEditors.TextEdit();
            this.lblVaolResExtran = new DevExpress.XtraEditors.LabelControl();
            this.lblTipoDepreciacionExtranjera = new DevExpress.XtraEditors.LabelControl();
            this.lblVidaUtilExtranjera = new DevExpress.XtraEditors.LabelControl();
            this.lblValorResidual = new DevExpress.XtraEditors.LabelControl();
            this.lblLibroLocal = new DevExpress.XtraEditors.LabelControl();
            this.lblLibroIFRS = new DevExpress.XtraEditors.LabelControl();
            this.lblTipoDepreciacion = new DevExpress.XtraEditors.LabelControl();
            this.lblVidaUtil = new DevExpress.XtraEditors.LabelControl();
            this.txtActivoID = new DevExpress.XtraEditors.TextEdit();
            this.btnBuscarActivo = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
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
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlCurrency)).BeginInit();
            this.grpCtrlCurrency.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCredLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebForeign.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCredForeign.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalForeign.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpValores)).BeginInit();
            this.grpValores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorResidualML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorResidualME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtActivoID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.grpValores);
            this.grpboxDetail.Controls.Add(this.grpCtrlCurrency);
            this.grpboxDetail.Size = new System.Drawing.Size(966, 166);
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
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
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
            this.editValue4});
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
            this.gbGridDocument.Size = new System.Drawing.Size(703, 243);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(703, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(2);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
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
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcProvider.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
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
            this.grpboxHeader.Controls.Add(this.labelControl1);
            this.grpboxHeader.Controls.Add(this.btnBuscarActivo);
            this.grpboxHeader.Controls.Add(this.txtActivoID);
            this.grpboxHeader.Location = new System.Drawing.Point(6, 26);
            this.grpboxHeader.Size = new System.Drawing.Size(988, 52);
            // 
            // grpCtrlCurrency
            // 
            this.grpCtrlCurrency.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.grpCtrlCurrency.Controls.Add(this.lblCurrLocal);
            this.grpCtrlCurrency.Controls.Add(this.lblCurrForeign);
            this.grpCtrlCurrency.Controls.Add(this.lblDebit);
            this.grpCtrlCurrency.Controls.Add(this.lblCredit);
            this.grpCtrlCurrency.Controls.Add(this.lblTotal);
            this.grpCtrlCurrency.Controls.Add(this.txtDebLocal);
            this.grpCtrlCurrency.Controls.Add(this.txtCredLocal);
            this.grpCtrlCurrency.Controls.Add(this.txtTotalLocal);
            this.grpCtrlCurrency.Controls.Add(this.txtDebForeign);
            this.grpCtrlCurrency.Controls.Add(this.txtCredForeign);
            this.grpCtrlCurrency.Controls.Add(this.txtTotalForeign);
            this.grpCtrlCurrency.Location = new System.Drawing.Point(568, 19);
            this.grpCtrlCurrency.Name = "grpCtrlCurrency";
            this.grpCtrlCurrency.Size = new System.Drawing.Size(370, 106);
            this.grpCtrlCurrency.TabIndex = 111;
            // 
            // lblCurrLocal
            // 
            this.lblCurrLocal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrLocal.Location = new System.Drawing.Point(69, 2);
            this.lblCurrLocal.Margin = new System.Windows.Forms.Padding(4);
            this.lblCurrLocal.Name = "lblCurrLocal";
            this.lblCurrLocal.Size = new System.Drawing.Size(95, 14);
            this.lblCurrLocal.TabIndex = 140;
            this.lblCurrLocal.Text = "1005_lblCurrLocal";
            // 
            // lblCurrForeign
            // 
            this.lblCurrForeign.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrForeign.Location = new System.Drawing.Point(221, 2);
            this.lblCurrForeign.Margin = new System.Windows.Forms.Padding(4);
            this.lblCurrForeign.Name = "lblCurrForeign";
            this.lblCurrForeign.Size = new System.Drawing.Size(94, 14);
            this.lblCurrForeign.TabIndex = 141;
            this.lblCurrForeign.Text = "61_lblCurrForeign";
            // 
            // lblDebit
            // 
            this.lblDebit.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebit.Location = new System.Drawing.Point(8, 30);
            this.lblDebit.Margin = new System.Windows.Forms.Padding(4);
            this.lblDebit.Name = "lblDebit";
            this.lblDebit.Size = new System.Drawing.Size(75, 14);
            this.lblDebit.TabIndex = 137;
            this.lblDebit.Text = "1005_lblDebit";
            // 
            // lblCredit
            // 
            this.lblCredit.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredit.Location = new System.Drawing.Point(7, 53);
            this.lblCredit.Margin = new System.Windows.Forms.Padding(4);
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Size = new System.Drawing.Size(78, 14);
            this.lblCredit.TabIndex = 138;
            this.lblCredit.Text = "1005_lblCredit";
            // 
            // lblTotal
            // 
            this.lblTotal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(8, 78);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(85, 14);
            this.lblTotal.TabIndex = 139;
            this.lblTotal.Text = "1005_lblTotal";
            // 
            // txtDebLocal
            // 
            this.txtDebLocal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtDebLocal.EditValue = "0";
            this.txtDebLocal.Enabled = false;
            this.txtDebLocal.Location = new System.Drawing.Point(91, 29);
            this.txtDebLocal.Name = "txtDebLocal";
            this.txtDebLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDebLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtDebLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtDebLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtDebLocal.Properties.Mask.EditMask = "c";
            this.txtDebLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtDebLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDebLocal.Properties.ReadOnly = true;
            this.txtDebLocal.Size = new System.Drawing.Size(118, 20);
            this.txtDebLocal.TabIndex = 106;
            // 
            // txtCredLocal
            // 
            this.txtCredLocal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCredLocal.EditValue = "0";
            this.txtCredLocal.Enabled = false;
            this.txtCredLocal.Location = new System.Drawing.Point(92, 52);
            this.txtCredLocal.Name = "txtCredLocal";
            this.txtCredLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCredLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCredLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCredLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCredLocal.Properties.Mask.EditMask = "c";
            this.txtCredLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCredLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCredLocal.Properties.ReadOnly = true;
            this.txtCredLocal.Size = new System.Drawing.Size(118, 20);
            this.txtCredLocal.TabIndex = 108;
            // 
            // txtTotalLocal
            // 
            this.txtTotalLocal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTotalLocal.EditValue = "0";
            this.txtTotalLocal.Enabled = false;
            this.txtTotalLocal.Location = new System.Drawing.Point(92, 77);
            this.txtTotalLocal.Name = "txtTotalLocal";
            this.txtTotalLocal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.txtTotalLocal.Properties.Appearance.Options.UseFont = true;
            this.txtTotalLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalLocal.Properties.Mask.EditMask = "c";
            this.txtTotalLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalLocal.Properties.ReadOnly = true;
            this.txtTotalLocal.Size = new System.Drawing.Size(118, 20);
            this.txtTotalLocal.TabIndex = 110;
            // 
            // txtDebForeign
            // 
            this.txtDebForeign.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtDebForeign.EditValue = "0";
            this.txtDebForeign.Enabled = false;
            this.txtDebForeign.Location = new System.Drawing.Point(234, 28);
            this.txtDebForeign.Name = "txtDebForeign";
            this.txtDebForeign.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDebForeign.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtDebForeign.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtDebForeign.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtDebForeign.Properties.Mask.EditMask = "c";
            this.txtDebForeign.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtDebForeign.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDebForeign.Properties.ReadOnly = true;
            this.txtDebForeign.Size = new System.Drawing.Size(118, 20);
            this.txtDebForeign.TabIndex = 0;
            // 
            // txtCredForeign
            // 
            this.txtCredForeign.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCredForeign.EditValue = "0";
            this.txtCredForeign.Enabled = false;
            this.txtCredForeign.Location = new System.Drawing.Point(234, 51);
            this.txtCredForeign.Name = "txtCredForeign";
            this.txtCredForeign.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCredForeign.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCredForeign.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCredForeign.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCredForeign.Properties.Mask.EditMask = "c";
            this.txtCredForeign.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCredForeign.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCredForeign.Properties.ReadOnly = true;
            this.txtCredForeign.Size = new System.Drawing.Size(118, 20);
            this.txtCredForeign.TabIndex = 1;
            // 
            // txtTotalForeign
            // 
            this.txtTotalForeign.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTotalForeign.EditValue = "0";
            this.txtTotalForeign.Enabled = false;
            this.txtTotalForeign.Location = new System.Drawing.Point(234, 77);
            this.txtTotalForeign.Name = "txtTotalForeign";
            this.txtTotalForeign.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.txtTotalForeign.Properties.Appearance.Options.UseFont = true;
            this.txtTotalForeign.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalForeign.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalForeign.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalForeign.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalForeign.Properties.Mask.EditMask = "c";
            this.txtTotalForeign.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalForeign.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalForeign.Properties.ReadOnly = true;
            this.txtTotalForeign.Size = new System.Drawing.Size(118, 20);
            this.txtTotalForeign.TabIndex = 2;
            // 
            // grpValores
            // 
            this.grpValores.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.grpValores.Controls.Add(this.txtVlrRetiro);
            this.grpValores.Controls.Add(this.lbl_VlrRetiro);
            this.grpValores.Controls.Add(this.txtValorResidualML);
            this.grpValores.Controls.Add(this.cmbTipoDecpreciacionML);
            this.grpValores.Controls.Add(this.cmbTipoDecpreciacionME);
            this.grpValores.Controls.Add(this.txtVidaUtilIFRS);
            this.grpValores.Controls.Add(this.txtVidaUtilLocal);
            this.grpValores.Controls.Add(this.txtValorResidualME);
            this.grpValores.Controls.Add(this.lblVaolResExtran);
            this.grpValores.Controls.Add(this.lblTipoDepreciacionExtranjera);
            this.grpValores.Controls.Add(this.lblVidaUtilExtranjera);
            this.grpValores.Controls.Add(this.lblValorResidual);
            this.grpValores.Controls.Add(this.lblLibroLocal);
            this.grpValores.Controls.Add(this.lblLibroIFRS);
            this.grpValores.Controls.Add(this.lblTipoDepreciacion);
            this.grpValores.Controls.Add(this.lblVidaUtil);
            this.grpValores.Location = new System.Drawing.Point(11, 19);
            this.grpValores.Name = "grpValores";
            this.grpValores.Size = new System.Drawing.Size(527, 129);
            this.grpValores.TabIndex = 112;
            // 
            // txtVlrRetiro
            // 
            this.txtVlrRetiro.BackColor = System.Drawing.Color.White;
            this.txtVlrRetiro.Location = new System.Drawing.Point(404, 99);
            this.txtVlrRetiro.Name = "txtVlrRetiro";
            this.txtVlrRetiro.Size = new System.Drawing.Size(109, 20);
            this.txtVlrRetiro.TabIndex = 155;
            // 
            // lbl_VlrRetiro
            // 
            this.lbl_VlrRetiro.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VlrRetiro.Location = new System.Drawing.Point(272, 101);
            this.lbl_VlrRetiro.Margin = new System.Windows.Forms.Padding(4);
            this.lbl_VlrRetiro.Name = "lbl_VlrRetiro";
            this.lbl_VlrRetiro.Size = new System.Drawing.Size(85, 14);
            this.lbl_VlrRetiro.TabIndex = 154;
            this.lbl_VlrRetiro.Text = "61_lbl_VlrRetiro";
            // 
            // txtValorResidualML
            // 
            this.txtValorResidualML.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtValorResidualML.EditValue = "0";
            this.txtValorResidualML.Enabled = false;
            this.txtValorResidualML.Location = new System.Drawing.Point(142, 73);
            this.txtValorResidualML.Name = "txtValorResidualML";
            this.txtValorResidualML.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.txtValorResidualML.Properties.Appearance.Options.UseBackColor = true;
            this.txtValorResidualML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorResidualML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorResidualML.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorResidualML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorResidualML.Properties.Mask.EditMask = "c";
            this.txtValorResidualML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorResidualML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorResidualML.Size = new System.Drawing.Size(109, 20);
            this.txtValorResidualML.TabIndex = 2;
            this.txtValorResidualML.EditValueChanged += new System.EventHandler(this.txtValorResidualML_EditValueChanged);
            // 
            // cmbTipoDecpreciacionML
            // 
            this.cmbTipoDecpreciacionML.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoDecpreciacionML.Enabled = false;
            this.cmbTipoDecpreciacionML.FormattingEnabled = true;
            this.cmbTipoDecpreciacionML.Location = new System.Drawing.Point(142, 22);
            this.cmbTipoDecpreciacionML.Name = "cmbTipoDecpreciacionML";
            this.cmbTipoDecpreciacionML.Size = new System.Drawing.Size(109, 21);
            this.cmbTipoDecpreciacionML.TabIndex = 0;
            this.cmbTipoDecpreciacionML.SelectedIndexChanged += new System.EventHandler(this.cmbTipoDecpreciacionML_SelectedIndexChanged);
            this.cmbTipoDecpreciacionML.Leave += new System.EventHandler(this.cmbControl_Leave);
            // 
            // cmbTipoDecpreciacionME
            // 
            this.cmbTipoDecpreciacionME.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoDecpreciacionME.Enabled = false;
            this.cmbTipoDecpreciacionME.FormattingEnabled = true;
            this.cmbTipoDecpreciacionME.Location = new System.Drawing.Point(404, 24);
            this.cmbTipoDecpreciacionME.Name = "cmbTipoDecpreciacionME";
            this.cmbTipoDecpreciacionME.Size = new System.Drawing.Size(109, 21);
            this.cmbTipoDecpreciacionME.TabIndex = 3;
            this.cmbTipoDecpreciacionME.SelectedIndexChanged += new System.EventHandler(this.cmbTipoDecpreciacionME_SelectedIndexChanged);
            this.cmbTipoDecpreciacionME.Leave += new System.EventHandler(this.cmbControl_Leave);
            // 
            // txtVidaUtilIFRS
            // 
            this.txtVidaUtilIFRS.BackColor = System.Drawing.Color.White;
            this.txtVidaUtilIFRS.Location = new System.Drawing.Point(404, 47);
            this.txtVidaUtilIFRS.Name = "txtVidaUtilIFRS";
            this.txtVidaUtilIFRS.Size = new System.Drawing.Size(109, 20);
            this.txtVidaUtilIFRS.TabIndex = 4;
            this.txtVidaUtilIFRS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumero_TextChanged);
            this.txtVidaUtilIFRS.Leave += new System.EventHandler(this.textControl_Leave);
            // 
            // txtVidaUtilLocal
            // 
            this.txtVidaUtilLocal.BackColor = System.Drawing.Color.White;
            this.txtVidaUtilLocal.Location = new System.Drawing.Point(142, 47);
            this.txtVidaUtilLocal.Name = "txtVidaUtilLocal";
            this.txtVidaUtilLocal.Size = new System.Drawing.Size(109, 20);
            this.txtVidaUtilLocal.TabIndex = 1;
            this.txtVidaUtilLocal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumero_TextChanged);
            this.txtVidaUtilLocal.Leave += new System.EventHandler(this.textControl_Leave);
            // 
            // txtValorResidualME
            // 
            this.txtValorResidualME.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtValorResidualME.EditValue = "0";
            this.txtValorResidualME.Enabled = false;
            this.txtValorResidualME.Location = new System.Drawing.Point(404, 73);
            this.txtValorResidualME.Name = "txtValorResidualME";
            this.txtValorResidualME.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.txtValorResidualME.Properties.Appearance.Options.UseBackColor = true;
            this.txtValorResidualME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorResidualME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorResidualME.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorResidualME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorResidualME.Properties.Mask.EditMask = "c";
            this.txtValorResidualME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorResidualME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorResidualME.Size = new System.Drawing.Size(109, 20);
            this.txtValorResidualME.TabIndex = 5;
            this.txtValorResidualME.EditValueChanged += new System.EventHandler(this.txtValorResidualME_EditValueChanged);
            // 
            // lblVaolResExtran
            // 
            this.lblVaolResExtran.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVaolResExtran.Location = new System.Drawing.Point(272, 75);
            this.lblVaolResExtran.Margin = new System.Windows.Forms.Padding(4);
            this.lblVaolResExtran.Name = "lblVaolResExtran";
            this.lblVaolResExtran.Size = new System.Drawing.Size(102, 14);
            this.lblVaolResExtran.TabIndex = 152;
            this.lblVaolResExtran.Text = "61_lblValorResidual";
            // 
            // lblTipoDepreciacionExtranjera
            // 
            this.lblTipoDepreciacionExtranjera.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoDepreciacionExtranjera.Location = new System.Drawing.Point(272, 27);
            this.lblTipoDepreciacionExtranjera.Margin = new System.Windows.Forms.Padding(4);
            this.lblTipoDepreciacionExtranjera.Name = "lblTipoDepreciacionExtranjera";
            this.lblTipoDepreciacionExtranjera.Size = new System.Drawing.Size(125, 14);
            this.lblTipoDepreciacionExtranjera.TabIndex = 148;
            this.lblTipoDepreciacionExtranjera.Text = "61_lblTipoDepreciacion";
            // 
            // lblVidaUtilExtranjera
            // 
            this.lblVidaUtilExtranjera.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVidaUtilExtranjera.Location = new System.Drawing.Point(272, 52);
            this.lblVidaUtilExtranjera.Margin = new System.Windows.Forms.Padding(4);
            this.lblVidaUtilExtranjera.Name = "lblVidaUtilExtranjera";
            this.lblVidaUtilExtranjera.Size = new System.Drawing.Size(72, 14);
            this.lblVidaUtilExtranjera.TabIndex = 149;
            this.lblVidaUtilExtranjera.Text = "61_lblVidaUtil";
            // 
            // lblValorResidual
            // 
            this.lblValorResidual.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorResidual.Location = new System.Drawing.Point(8, 76);
            this.lblValorResidual.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorResidual.Name = "lblValorResidual";
            this.lblValorResidual.Size = new System.Drawing.Size(102, 14);
            this.lblValorResidual.TabIndex = 146;
            this.lblValorResidual.Text = "61_lblValorResidual";
            // 
            // lblLibroLocal
            // 
            this.lblLibroLocal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibroLocal.Location = new System.Drawing.Point(78, 2);
            this.lblLibroLocal.Margin = new System.Windows.Forms.Padding(4);
            this.lblLibroLocal.Name = "lblLibroLocal";
            this.lblLibroLocal.Size = new System.Drawing.Size(85, 14);
            this.lblLibroLocal.TabIndex = 140;
            this.lblLibroLocal.Text = "61_lblLibroLocal";
            // 
            // lblLibroIFRS
            // 
            this.lblLibroIFRS.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibroIFRS.Location = new System.Drawing.Point(319, 2);
            this.lblLibroIFRS.Margin = new System.Windows.Forms.Padding(4);
            this.lblLibroIFRS.Name = "lblLibroIFRS";
            this.lblLibroIFRS.Size = new System.Drawing.Size(82, 14);
            this.lblLibroIFRS.TabIndex = 141;
            this.lblLibroIFRS.Text = "61_lblLibroIFRS";
            // 
            // lblTipoDepreciacion
            // 
            this.lblTipoDepreciacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoDepreciacion.Location = new System.Drawing.Point(8, 26);
            this.lblTipoDepreciacion.Margin = new System.Windows.Forms.Padding(4);
            this.lblTipoDepreciacion.Name = "lblTipoDepreciacion";
            this.lblTipoDepreciacion.Size = new System.Drawing.Size(125, 14);
            this.lblTipoDepreciacion.TabIndex = 137;
            this.lblTipoDepreciacion.Text = "61_lblTipoDepreciacion";
            // 
            // lblVidaUtil
            // 
            this.lblVidaUtil.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVidaUtil.Location = new System.Drawing.Point(8, 52);
            this.lblVidaUtil.Margin = new System.Windows.Forms.Padding(4);
            this.lblVidaUtil.Name = "lblVidaUtil";
            this.lblVidaUtil.Size = new System.Drawing.Size(72, 14);
            this.lblVidaUtil.TabIndex = 138;
            this.lblVidaUtil.Text = "61_lblVidaUtil";
            // 
            // txtActivoID
            // 
            this.txtActivoID.Location = new System.Drawing.Point(87, 16);
            this.txtActivoID.Name = "txtActivoID";
            this.txtActivoID.Properties.ReadOnly = true;
            this.txtActivoID.Size = new System.Drawing.Size(55, 20);
            this.txtActivoID.TabIndex = 1;
            // 
            // btnBuscarActivo
            // 
            this.btnBuscarActivo.Location = new System.Drawing.Point(153, 14);
            this.btnBuscarActivo.Name = "btnBuscarActivo";
            this.btnBuscarActivo.Size = new System.Drawing.Size(95, 23);
            this.btnBuscarActivo.TabIndex = 2;
            this.btnBuscarActivo.Text = "66_buscarActivo";
            this.btnBuscarActivo.Click += new System.EventHandler(this.btnBuscarActivo_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 19);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(58, 13);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "66_lblActivo";
            // 
            // AdicionActivos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1027, 581);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AdicionActivos";
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
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlCurrency)).EndInit();
            this.grpCtrlCurrency.ResumeLayout(false);
            this.grpCtrlCurrency.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCredLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebForeign.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCredForeign.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalForeign.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpValores)).EndInit();
            this.grpValores.ResumeLayout(false);
            this.grpValores.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorResidualML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorResidualME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtActivoID.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grpCtrlCurrency;
        private DevExpress.XtraEditors.LabelControl lblCurrLocal;
        private DevExpress.XtraEditors.LabelControl lblCurrForeign;
        private DevExpress.XtraEditors.LabelControl lblDebit;
        private DevExpress.XtraEditors.LabelControl lblCredit;
        private DevExpress.XtraEditors.LabelControl lblTotal;
        private DevExpress.XtraEditors.TextEdit txtDebLocal;
        private DevExpress.XtraEditors.TextEdit txtCredLocal;
        protected DevExpress.XtraEditors.TextEdit txtTotalLocal;
        private DevExpress.XtraEditors.TextEdit txtDebForeign;
        private DevExpress.XtraEditors.TextEdit txtCredForeign;
        protected DevExpress.XtraEditors.TextEdit txtTotalForeign;
        private DevExpress.XtraEditors.GroupControl grpValores;
        private DevExpress.XtraEditors.LabelControl lblLibroLocal;
        private DevExpress.XtraEditors.LabelControl lblLibroIFRS;
        private DevExpress.XtraEditors.LabelControl lblTipoDepreciacion;
        private DevExpress.XtraEditors.LabelControl lblVidaUtil;
        private DevExpress.XtraEditors.TextEdit txtValorResidualME;
        private DevExpress.XtraEditors.LabelControl lblVaolResExtran;
        private DevExpress.XtraEditors.LabelControl lblTipoDepreciacionExtranjera;
        private DevExpress.XtraEditors.LabelControl lblVidaUtilExtranjera;
        private DevExpress.XtraEditors.LabelControl lblValorResidual;
        private System.Windows.Forms.TextBox txtVidaUtilIFRS;
        private System.Windows.Forms.TextBox txtVidaUtilLocal;
        private Clases.ComboBoxEx cmbTipoDecpreciacionML;
        private Clases.ComboBoxEx cmbTipoDecpreciacionME;
        private DevExpress.XtraEditors.TextEdit txtValorResidualML;
        private DevExpress.XtraEditors.LabelControl lbl_VlrRetiro;
        private System.Windows.Forms.TextBox txtVlrRetiro;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnBuscarActivo;
        private DevExpress.XtraEditors.TextEdit txtActivoID;

    }
}