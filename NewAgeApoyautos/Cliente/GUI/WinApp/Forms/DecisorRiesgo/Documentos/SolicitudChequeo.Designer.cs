namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class SolicitudChequeo
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
        public virtual void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.riPopupTareas = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControlTareas = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControlTareas = new DevExpress.XtraRichEdit.RichEditControl();
            this.riPopupAnexos = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControlAnexos = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControlAnexos = new DevExpress.XtraRichEdit.RichEditControl();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editCmbDict = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlEdicion = new DevExpress.XtraEditors.PanelControl();
            this.gcDocuments = new DevExpress.XtraGrid.GridControl();
            this.gvDocuments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnDetails = new System.Windows.Forms.Panel();
            this.gcTareas = new DevExpress.XtraGrid.GridControl();
            this.gvTareas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.grpboxHeader = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.masterActividad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkSeleccionar = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlTareas)).BeginInit();
            this.PopupContainerControlTareas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupAnexos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlAnexos)).BeginInit();
            this.PopupContainerControlAnexos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmbDict)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlEdicion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).BeginInit();
            this.pnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareas)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riPopup,
            this.riPopupTareas,
            this.riPopupAnexos,
            this.editChkBox,
            this.editSpin,
            this.editCmb,
            this.editBtnGrid,
            this.editCmbDict});
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
            this.riPopup.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.riPopup.QueryResultValue += new DevExpress.XtraEditors.Controls.QueryResultValueEventHandler(this.riPopup_QueryResultValue);
            this.riPopup.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.riPopup_QueryPopUp);
            // 
            // PopupContainerControl
            // 
            this.PopupContainerControl.Controls.Add(this.richEditControl);
            this.PopupContainerControl.Location = new System.Drawing.Point(3, 24);
            this.PopupContainerControl.Name = "PopupContainerControl";
            this.PopupContainerControl.Size = new System.Drawing.Size(23, 164);
            this.PopupContainerControl.TabIndex = 5;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(23, 164);
            this.richEditControl.TabIndex = 2;
            this.richEditControl.Text = "richEditControl";
            // 
            // riPopupTareas
            // 
            this.riPopupTareas.AutoHeight = false;
            this.riPopupTareas.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riPopupTareas.Name = "riPopupTareas";
            this.riPopupTareas.PopupControl = this.PopupContainerControlTareas;
            this.riPopupTareas.PopupFormMinSize = new System.Drawing.Size(500, 0);
            this.riPopupTareas.PopupFormSize = new System.Drawing.Size(500, 300);
            this.riPopupTareas.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.riPopupTareas.QueryResultValue += new DevExpress.XtraEditors.Controls.QueryResultValueEventHandler(this.riPopupTareas_QueryResultValue);
            this.riPopupTareas.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.riPopupTareas_QueryPopUp);
            // 
            // PopupContainerControlTareas
            // 
            this.PopupContainerControlTareas.Controls.Add(this.richEditControlTareas);
            this.PopupContainerControlTareas.Location = new System.Drawing.Point(3, 27);
            this.PopupContainerControlTareas.Name = "PopupContainerControlTareas";
            this.PopupContainerControlTareas.Size = new System.Drawing.Size(24, 183);
            this.PopupContainerControlTareas.TabIndex = 5;
            // 
            // richEditControlTareas
            // 
            this.richEditControlTareas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControlTareas.EnableToolTips = true;
            this.richEditControlTareas.Location = new System.Drawing.Point(0, 0);
            this.richEditControlTareas.Name = "richEditControlTareas";
            this.richEditControlTareas.Size = new System.Drawing.Size(24, 183);
            this.richEditControlTareas.TabIndex = 2;
            this.richEditControlTareas.Text = "richEditControlTareas";
            // 
            // riPopupAnexos
            // 
            this.riPopupAnexos.AutoHeight = false;
            this.riPopupAnexos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riPopupAnexos.Name = "riPopupAnexos";
            this.riPopupAnexos.PopupControl = this.PopupContainerControlAnexos;
            this.riPopupAnexos.PopupFormMinSize = new System.Drawing.Size(500, 0);
            this.riPopupAnexos.PopupFormSize = new System.Drawing.Size(500, 300);
            this.riPopupAnexos.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.riPopupAnexos.QueryResultValue += new DevExpress.XtraEditors.Controls.QueryResultValueEventHandler(this.riPopupAnexos_QueryResultValue);
            // 
            // PopupContainerControlAnexos
            // 
            this.PopupContainerControlAnexos.Controls.Add(this.richEditControlAnexos);
            this.PopupContainerControlAnexos.Location = new System.Drawing.Point(3, 27);
            this.PopupContainerControlAnexos.Name = "PopupContainerControlAnexos";
            this.PopupContainerControlAnexos.Size = new System.Drawing.Size(24, 183);
            this.PopupContainerControlAnexos.TabIndex = 5;
            // 
            // richEditControlAnexos
            // 
            this.richEditControlAnexos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControlAnexos.EnableToolTips = true;
            this.richEditControlAnexos.Location = new System.Drawing.Point(0, 0);
            this.richEditControlAnexos.Name = "richEditControlAnexos";
            this.richEditControlAnexos.Size = new System.Drawing.Size(24, 183);
            this.richEditControlAnexos.TabIndex = 2;
            this.richEditControlAnexos.Text = "richEditControlAnexos";
            // 
            // editChkBox
            // 
            this.editChkBox.Caption = "";
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
            // editCmb
            // 
            this.editCmb.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editCmb.Name = "editCmb";
            this.editCmb.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.editCmb.SelectedIndexChanged += new System.EventHandler(this.editCmb_SelectedIndexChanged);
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::NewAge.Properties.Resources.FindFkHierarchy, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnGrid_ButtonClick);
            // 
            // editCmbDict
            // 
            this.editCmbDict.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editCmbDict.DisplayMember = "Value";
            this.editCmbDict.Name = "editCmbDict";
            this.editCmbDict.ValueMember = "Key";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlEdicion);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(32, 194);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(852, 139);
            this.panel1.TabIndex = 7;
            // 
            // pnlEdicion
            // 
            this.pnlEdicion.Location = new System.Drawing.Point(0, 1);
            this.pnlEdicion.Name = "pnlEdicion";
            this.pnlEdicion.Size = new System.Drawing.Size(851, 138);
            this.pnlEdicion.TabIndex = 0;
            // 
            // gcDocuments
            // 
            this.gcDocuments.AllowDrop = true;
            this.gcDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocuments.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocuments.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocuments.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocuments.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocuments.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocuments.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocuments.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcDocuments.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocuments.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocuments.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDocuments_EmbeddedNavigator_ButtonClick);
            this.gcDocuments.Location = new System.Drawing.Point(0, 24);
            this.gcDocuments.LookAndFeel.SkinName = "Dark Side";
            this.gcDocuments.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocuments.MainView = this.gvDocuments;
            this.gcDocuments.Name = "gcDocuments";
            this.gcDocuments.Size = new System.Drawing.Size(852, 140);
            this.gcDocuments.TabIndex = 9;
            this.gcDocuments.UseEmbeddedNavigator = true;
            this.gcDocuments.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocuments});
            // 
            // gvDocuments
            // 
            this.gvDocuments.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocuments.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocuments.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocuments.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocuments.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocuments.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocuments.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocuments.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocuments.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocuments.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocuments.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocuments.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocuments.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocuments.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocuments.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocuments.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocuments.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocuments.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocuments.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocuments.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocuments.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocuments.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocuments.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocuments.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocuments.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocuments.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocuments.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocuments.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocuments.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocuments.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocuments.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocuments.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocuments.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocuments.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocuments.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocuments.Appearance.Row.Options.UseBackColor = true;
            this.gvDocuments.Appearance.Row.Options.UseForeColor = true;
            this.gvDocuments.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocuments.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocuments.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocuments.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocuments.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocuments.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocuments.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocuments.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocuments.GridControl = this.gcDocuments;
            this.gvDocuments.Name = "gvDocuments";
            this.gvDocuments.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocuments.OptionsCustomization.AllowFilter = false;
            this.gvDocuments.OptionsCustomization.AllowSort = false;
            this.gvDocuments.OptionsView.ShowGroupPanel = false;
            this.gvDocuments.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocuments_CustomRowCellEdit);
            this.gvDocuments.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocuments_CustomRowCellEditForEditing);
            this.gvDocuments.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocuments_FocusedRowChanged);
            this.gvDocuments.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gcDocuments_CellValueChanged);
            this.gvDocuments.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocuments_CellValueChanging);
            this.gvDocuments.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocuments_BeforeLeaveRow);
            this.gvDocuments.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            this.gvDocuments.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocuments_CustomColumnDisplayText);
            // 
            // pnDetails
            // 
            this.pnDetails.Controls.Add(this.gcTareas);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDetails.Location = new System.Drawing.Point(32, 339);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(852, 140);
            this.pnDetails.TabIndex = 6;
            // 
            // gcTareas
            // 
            this.gcTareas.AllowDrop = true;
            this.gcTareas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTareas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcTareas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcTareas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcTareas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcTareas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcTareas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcTareas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcTareas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcTareas.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcTareas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcTareas.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcTareas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcTareas.Location = new System.Drawing.Point(0, 0);
            this.gcTareas.LookAndFeel.SkinName = "Dark Side";
            this.gcTareas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcTareas.MainView = this.gvTareas;
            this.gcTareas.Name = "gcTareas";
            this.gcTareas.Size = new System.Drawing.Size(852, 140);
            this.gcTareas.TabIndex = 11;
            this.gcTareas.UseEmbeddedNavigator = true;
            this.gcTareas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTareas});
            // 
            // gvTareas
            // 
            this.gvTareas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTareas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvTareas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvTareas.Appearance.Empty.Options.UseBackColor = true;
            this.gvTareas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvTareas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvTareas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTareas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvTareas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvTareas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTareas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTareas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTareas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvTareas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvTareas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvTareas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTareas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvTareas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvTareas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvTareas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvTareas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvTareas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvTareas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvTareas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvTareas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTareas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvTareas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvTareas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvTareas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTareas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvTareas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvTareas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.Row.Options.UseBackColor = true;
            this.gvTareas.Appearance.Row.Options.UseForeColor = true;
            this.gvTareas.Appearance.Row.Options.UseTextOptions = true;
            this.gvTareas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvTareas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTareas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTareas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvTareas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvTareas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTareas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvTareas.GridControl = this.gcTareas;
            this.gvTareas.Name = "gvTareas";
            this.gvTareas.OptionsCustomization.AllowColumnMoving = false;
            this.gvTareas.OptionsCustomization.AllowFilter = false;
            this.gvTareas.OptionsCustomization.AllowSort = false;
            this.gvTareas.OptionsView.ShowGroupPanel = false;
            this.gvTareas.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvTareas_CustomRowCellEdit);
            this.gvTareas.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvTareas_CustomRowCellEditForEditing);
            this.gvTareas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grpctrlHeader);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(32, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(852, 15);
            this.panel2.TabIndex = 0;
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
            this.grpctrlHeader.Controls.Add(this.grpboxHeader);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(0, 0);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(852, 15);
            this.grpctrlHeader.TabIndex = 1;
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.BackColor = System.Drawing.Color.Transparent;
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 2);
            this.grpboxHeader.Name = "grpboxHeader";
            this.grpboxHeader.Size = new System.Drawing.Size(848, 11);
            this.grpboxHeader.TabIndex = 8;
            this.grpboxHeader.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gcDocuments);
            this.panel3.Controls.Add(this.masterActividad);
            this.panel3.Controls.Add(this.chkSeleccionar);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(32, 24);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(852, 164);
            this.panel3.TabIndex = 8;
            // 
            // masterActividad
            // 
            this.masterActividad.BackColor = System.Drawing.Color.Transparent;
            this.masterActividad.Filtros = null;
            this.masterActividad.Location = new System.Drawing.Point(576, -1);
            this.masterActividad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.masterActividad.Name = "masterActividad";
            this.masterActividad.Size = new System.Drawing.Size(298, 25);
            this.masterActividad.TabIndex = 13;
            this.masterActividad.Value = "";
            this.masterActividad.Leave += new System.EventHandler(this.masterActividad_Leave);
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkSeleccionar.Location = new System.Drawing.Point(0, 0);
            this.chkSeleccionar.Name = "chkSeleccionar";
            this.chkSeleccionar.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.chkSeleccionar.Size = new System.Drawing.Size(852, 24);
            this.chkSeleccionar.TabIndex = 12;
            this.chkSeleccionar.Text = "32901_CheckAll";
            this.chkSeleccionar.UseVisualStyleBackColor = true;
            this.chkSeleccionar.CheckedChanged += new System.EventHandler(this.chkSeleccionar_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.285871F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 96.71413F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnDetails, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.PopupContainerControl, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.26761F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.73239F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(908, 482);
            this.tableLayoutPanel1.TabIndex = 2;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // SolicitudChequeo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 482);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SolicitudChequeo";
            this.Text = "DocumentAprobTareasForm";
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.riPopupTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlTareas)).EndInit();
            this.PopupContainerControlTareas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.riPopupAnexos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlAnexos)).EndInit();
            this.PopupContainerControlAnexos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmbDict)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlEdicion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).EndInit();
            this.pnDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareas)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richText;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richText1;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richTextTareas;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richTextTareas1;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richTextAnexos;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richTextAnexos1;
        protected DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopup;
        protected DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopupTareas;
        protected DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopupAnexos;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControl;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControlAnexos;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControlTareas;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControl;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControlTareas;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControlAnexos;
        protected DevExpress.XtraGrid.GridControl gcDocuments;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocuments;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
        protected System.Windows.Forms.Panel pnDetails;
        protected System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.Panel panel3;
        protected DevExpress.XtraGrid.GridControl gcTareas;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvTareas;
        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected ControlsUC.uc_MasterFind masterActividad;
        protected System.Windows.Forms.CheckBox chkSeleccionar;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        public System.Windows.Forms.GroupBox grpboxHeader;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        protected DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit editCmbDict;
        private DevExpress.XtraEditors.PanelControl pnlEdicion;

    }
}