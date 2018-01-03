namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DocumentProvForm
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gcCargos = new DevExpress.XtraGrid.GridControl();
            this.gvCargos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.masterCodigoBS = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterReferencia = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.editBtnGridCargos = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editSpinPorc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.pnlCargos = new DevExpress.XtraEditors.PanelControl();
            this.btnCodigoBS = new DevExpress.XtraEditors.ButtonEdit();
            this.btnReferencia = new DevExpress.XtraEditors.ButtonEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCargos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCargos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGridCargos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCargos)).BeginInit();
            this.pnlCargos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCodigoBS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReferencia.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.btnReferencia);
            this.grpboxDetail.Controls.Add(this.btnCodigoBS);
            this.grpboxDetail.Controls.Add(this.pnlCargos);
            this.grpboxDetail.Controls.Add(this.txtDesc);
            this.grpboxDetail.Controls.Add(this.lblDesc);
            this.grpboxDetail.Controls.Add(this.masterReferencia);
            this.grpboxDetail.Controls.Add(this.masterCodigoBS);
            this.grpboxDetail.Location = new System.Drawing.Point(4, -5);
            this.grpboxDetail.Size = new System.Drawing.Size(1109, 165);
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
            this.editBtnGridCargos,
            this.editSpinPorc});
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
            this.editValue.Mask.EditMask = "c4";
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
            // lblMark
            // 
            this.lblMark.Location = new System.Drawing.Point(980, 11);
            // 
            // btnMark
            // 
            this.btnMark.Location = new System.Drawing.Point(1058, 8);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Size = new System.Drawing.Size(820, 272);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(820, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 272);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Size = new System.Drawing.Size(1117, 310);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "c0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // gcCargos
            // 
            this.gcCargos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCargos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCargos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcCargos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcCargos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcCargos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcCargos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcCargos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcCargos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcCargos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcCargos.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcCargos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcCargos.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcCargos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcCargos.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcCargos_EmbeddedNavigator_ButtonClick);
            this.gcCargos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcCargos.Location = new System.Drawing.Point(2, 2);
            this.gcCargos.LookAndFeel.SkinName = "Dark Side";
            this.gcCargos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcCargos.MainView = this.gvCargos;
            this.gcCargos.Margin = new System.Windows.Forms.Padding(4);
            this.gcCargos.Name = "gcCargos";
            this.gcCargos.Size = new System.Drawing.Size(694, 150);
            this.gcCargos.TabIndex = 30;
            this.gcCargos.UseEmbeddedNavigator = true;
            this.gcCargos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCargos});
            this.gcCargos.Leave += new System.EventHandler(this.gcCargos_Leave);
            // 
            // gvCargos
            // 
            this.gvCargos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCargos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvCargos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvCargos.Appearance.Empty.Options.UseBackColor = true;
            this.gvCargos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvCargos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvCargos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCargos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvCargos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvCargos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvCargos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCargos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvCargos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvCargos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvCargos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvCargos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvCargos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvCargos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvCargos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCargos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvCargos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCargos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvCargos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvCargos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvCargos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvCargos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCargos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvCargos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCargos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvCargos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvCargos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvCargos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCargos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvCargos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvCargos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvCargos.Appearance.Row.Options.UseBackColor = true;
            this.gvCargos.Appearance.Row.Options.UseForeColor = true;
            this.gvCargos.Appearance.Row.Options.UseTextOptions = true;
            this.gvCargos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvCargos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCargos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvCargos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvCargos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvCargos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCargos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvCargos.GridControl = this.gcCargos;
            this.gvCargos.HorzScrollStep = 50;
            this.gvCargos.Name = "gvCargos";
            this.gvCargos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvCargos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvCargos.OptionsCustomization.AllowFilter = false;
            this.gvCargos.OptionsCustomization.AllowSort = false;
            this.gvCargos.OptionsMenu.EnableColumnMenu = false;
            this.gvCargos.OptionsMenu.EnableFooterMenu = false;
            this.gvCargos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvCargos.OptionsView.ColumnAutoWidth = false;
            this.gvCargos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvCargos.OptionsView.ShowGroupPanel = false;
            this.gvCargos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gvCargos.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvCargos_CustomRowCellEdit);
            this.gvCargos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvCargos_FocusedRowChanged);
            this.gvCargos.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gvCargos_FocusedColumnChanged);
            this.gvCargos.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvCargos_CellValueChanged);
            this.gvCargos.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvCargos_BeforeLeaveRow);
            this.gvCargos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvCargos_CustomUnboundColumnData);
            this.gvCargos.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gvCargos_KeyUp);
            // 
            // masterCodigoBS
            // 
            this.masterCodigoBS.BackColor = System.Drawing.Color.Transparent;
            this.masterCodigoBS.Filtros = null;
            this.masterCodigoBS.Location = new System.Drawing.Point(22, 14);
            this.masterCodigoBS.Name = "masterCodigoBS";
            this.masterCodigoBS.Size = new System.Drawing.Size(298, 25);
            this.masterCodigoBS.TabIndex = 1;
            this.masterCodigoBS.Value = "";
            this.masterCodigoBS.Leave += new System.EventHandler(this.masterDetails_Leave);
            // 
            // masterReferencia
            // 
            this.masterReferencia.BackColor = System.Drawing.Color.Transparent;
            this.masterReferencia.Filtros = null;
            this.masterReferencia.Location = new System.Drawing.Point(22, 38);
            this.masterReferencia.Name = "masterReferencia";
            this.masterReferencia.Size = new System.Drawing.Size(298, 25);
            this.masterReferencia.TabIndex = 2;
            this.masterReferencia.Value = "";
            this.masterReferencia.Leave += new System.EventHandler(this.masterDetails_Leave);
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(19, 66);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(58, 13);
            this.lblDesc.TabIndex = 3;
            this.lblDesc.Text = "71_lblDesc";
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(22, 82);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(291, 77);
            this.txtDesc.TabIndex = 4;
            this.txtDesc.Leave += new System.EventHandler(this.textControl_Leave);
            // 
            // editBtnGridCargos
            // 
            this.editBtnGridCargos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.editBtnGridCargos.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGridCargos.Name = "editBtnGridCargos";
            this.editBtnGridCargos.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnGridCargos_ButtonClick);
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P";
            this.editSpinPorc.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorc.Name = "editSpinPorc";
            // 
            // pnlCargos
            // 
            this.pnlCargos.Controls.Add(this.gcCargos);
            this.pnlCargos.Location = new System.Drawing.Point(405, 8);
            this.pnlCargos.Name = "pnlCargos";
            this.pnlCargos.Size = new System.Drawing.Size(698, 154);
            this.pnlCargos.TabIndex = 31;
            // 
            // btnCodigoBS
            // 
            this.btnCodigoBS.Enabled = false;
            this.btnCodigoBS.Location = new System.Drawing.Point(199, 17);
            this.btnCodigoBS.Name = "btnCodigoBS";
            this.btnCodigoBS.Properties.Appearance.BackColor = System.Drawing.Color.Azure;
            this.btnCodigoBS.Properties.Appearance.Options.UseBackColor = true;
            this.btnCodigoBS.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::NewAge.Properties.Resources.FindFkHierarchy, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.btnCodigoBS.Size = new System.Drawing.Size(22, 20);
            this.btnCodigoBS.TabIndex = 32;
            this.btnCodigoBS.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnCodigoBS_ButtonClick);
            // 
            // btnReferencia
            // 
            this.btnReferencia.Enabled = false;
            this.btnReferencia.Location = new System.Drawing.Point(199, 41);
            this.btnReferencia.Name = "btnReferencia";
            this.btnReferencia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::NewAge.Properties.Resources.FindFkHierarchy, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.btnReferencia.Size = new System.Drawing.Size(22, 20);
            this.btnReferencia.TabIndex = 33;
            this.btnReferencia.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnReferencia_ButtonClick);
            // 
            // DocumentProvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "DocumentProvForm";
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
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCargos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCargos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGridCargos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCargos)).EndInit();
            this.pnlCargos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnCodigoBS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReferencia.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGridCargos;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorc;
        protected DevExpress.XtraGrid.GridControl gcCargos;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvCargos;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblDesc;
        private ControlsUC.uc_MasterFind masterReferencia;
        private ControlsUC.uc_MasterFind masterCodigoBS;
        private DevExpress.XtraEditors.PanelControl pnlCargos;
        private DevExpress.XtraEditors.ButtonEdit btnCodigoBS;
        private DevExpress.XtraEditors.ButtonEdit btnReferencia;
    }
}