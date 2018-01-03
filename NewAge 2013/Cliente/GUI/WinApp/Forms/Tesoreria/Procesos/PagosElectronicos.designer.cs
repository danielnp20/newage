using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PagosElectronicos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PagosElectronicos));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.masterCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gctrlGrid = new DevExpress.XtraEditors.GroupControl();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.gcPagos = new DevExpress.XtraGrid.GridControl();
            this.gvPagos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.btnTransmitir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlHeader)).BeginInit();
            this.gctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlGrid)).BeginInit();
            this.gctrlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(9);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(1363, 567);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Margin = new System.Windows.Forms.Padding(4);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.453958F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98.54604F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1359, 563);
            this.tlSeparatorPanel.TabIndex = 54;
            // 
            // pnlDetail
            // 
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(17, 558);
            this.pnlDetail.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1327, 1);
            this.pnlDetail.TabIndex = 112;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gctrlHeader);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(17, 12);
            this.pnlGrids.Margin = new System.Windows.Forms.Padding(4);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(1327, 538);
            this.pnlGrids.TabIndex = 113;
            // 
            // gctrlHeader
            // 
            this.gctrlHeader.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.gctrlHeader.Appearance.Options.UseBackColor = true;
            this.gctrlHeader.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.gctrlHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gctrlHeader.AppearanceCaption.Options.UseBackColor = true;
            this.gctrlHeader.AppearanceCaption.Options.UseFont = true;
            this.gctrlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gctrlHeader.Controls.Add(this.masterCuenta);
            this.gctrlHeader.Controls.Add(this.gctrlGrid);
            this.gctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gctrlHeader.Location = new System.Drawing.Point(0, 0);
            this.gctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlHeader.Margin = new System.Windows.Forms.Padding(4);
            this.gctrlHeader.Name = "gctrlHeader";
            this.gctrlHeader.Size = new System.Drawing.Size(1327, 538);
            this.gctrlHeader.TabIndex = 0;
            // 
            // masterCuenta
            // 
            this.masterCuenta.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta.Filtros = null;
            this.masterCuenta.Location = new System.Drawing.Point(37, 25);
            this.masterCuenta.Margin = new System.Windows.Forms.Padding(5);
            this.masterCuenta.Name = "masterCuenta";
            this.masterCuenta.Size = new System.Drawing.Size(388, 31);
            this.masterCuenta.TabIndex = 110;
            this.masterCuenta.Value = "";
            this.masterCuenta.Leave += new System.EventHandler(this.masterCuenta_Leave);
            // 
            // gctrlGrid
            // 
            this.gctrlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gctrlGrid.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.gctrlGrid.Appearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gctrlGrid.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gctrlGrid.Appearance.Options.UseBackColor = true;
            this.gctrlGrid.Appearance.Options.UseBorderColor = true;
            this.gctrlGrid.Appearance.Options.UseFont = true;
            this.gctrlGrid.AppearanceCaption.BorderColor = System.Drawing.SystemColors.Control;
            this.gctrlGrid.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gctrlGrid.AppearanceCaption.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.gctrlGrid.AppearanceCaption.Options.UseBorderColor = true;
            this.gctrlGrid.AppearanceCaption.Options.UseFont = true;
            this.gctrlGrid.AppearanceCaption.Options.UseForeColor = true;
            this.gctrlGrid.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gctrlGrid.Controls.Add(this.btnTransmitir);
            this.gctrlGrid.Controls.Add(this.chkSelectAll);
            this.gctrlGrid.Controls.Add(this.gcPagos);
            this.gctrlGrid.Location = new System.Drawing.Point(21, 65);
            this.gctrlGrid.LookAndFeel.SkinName = "Seven Classic";
            this.gctrlGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlGrid.Margin = new System.Windows.Forms.Padding(4);
            this.gctrlGrid.Name = "gctrlGrid";
            this.gctrlGrid.ShowCaption = false;
            this.gctrlGrid.Size = new System.Drawing.Size(1278, 469);
            this.gctrlGrid.TabIndex = 0;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.BackColor = System.Drawing.SystemColors.Control;
            this.chkSelectAll.Enabled = false;
            this.chkSelectAll.ForeColor = System.Drawing.Color.Black;
            this.chkSelectAll.Location = new System.Drawing.Point(5, 412);
            this.chkSelectAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(146, 21);
            this.chkSelectAll.TabIndex = 109;
            this.chkSelectAll.Text = "1109_chkSelectAll";
            this.chkSelectAll.UseVisualStyleBackColor = false;
            this.chkSelectAll.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkSelectAll_MouseClick);
            // 
            // gcPagos
            // 
            this.gcPagos.AllowDrop = true;
            this.gcPagos.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcPagos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gcPagos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcPagos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcPagos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcPagos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcPagos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcPagos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcPagos.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcPagos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcPagos.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcPagos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcPagos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcPagos.Location = new System.Drawing.Point(2, 2);
            this.gcPagos.LookAndFeel.SkinName = "Dark Side";
            this.gcPagos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPagos.MainView = this.gvPagos;
            this.gcPagos.Margin = new System.Windows.Forms.Padding(5);
            this.gcPagos.Name = "gcPagos";
            this.gcPagos.Size = new System.Drawing.Size(1274, 403);
            this.gcPagos.TabIndex = 51;
            this.gcPagos.UseEmbeddedNavigator = true;
            this.gcPagos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPagos});
            // 
            // gvPagos
            // 
            this.gvPagos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPagos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvPagos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvPagos.Appearance.Empty.Options.UseBackColor = true;
            this.gvPagos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvPagos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvPagos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPagos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvPagos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvPagos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPagos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvPagos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvPagos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvPagos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvPagos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvPagos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPagos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvPagos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvPagos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvPagos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPagos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvPagos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvPagos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPagos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvPagos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPagos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvPagos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvPagos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvPagos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvPagos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvPagos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvPagos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.Row.Options.UseBackColor = true;
            this.gvPagos.Appearance.Row.Options.UseForeColor = true;
            this.gvPagos.Appearance.Row.Options.UseTextOptions = true;
            this.gvPagos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvPagos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPagos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvPagos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvPagos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvPagos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvPagos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvPagos.GridControl = this.gcPagos;
            this.gvPagos.HorzScrollStep = 50;
            this.gvPagos.Name = "gvPagos";
            this.gvPagos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvPagos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvPagos.OptionsCustomization.AllowFilter = false;
            this.gvPagos.OptionsCustomization.AllowSort = false;
            this.gvPagos.OptionsCustomization.AllowColumnMoving = false;
            this.gvPagos.OptionsMenu.EnableColumnMenu = false;
            this.gvPagos.OptionsMenu.EnableFooterMenu = false;
            this.gvPagos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPagos.OptionsView.ColumnAutoWidth = false;
            this.gvPagos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvPagos.OptionsView.ShowGroupPanel = false;
            this.gvPagos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvPagos.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvPagos_CustomRowCellEdit);
            this.gvPagos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvPagos_FocusedRowChanged);
            this.gvPagos.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvPagos_CellValueChanging);
            this.gvPagos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvPagos_CustomUnboundColumnData);
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
            this.editCheck,
            this.editValue,
            this.editValue4});
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
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("editBtnGrid.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            // 
            // editCmb
            // 
            this.editCmb.Name = "editCmb";
            // 
            // editText
            // 
            this.editText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            this.editText.Name = "editText";
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
            // editDate
            // 
            this.editDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            this.editDate.Name = "editDate";
            this.editDate.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            // 
            // editCheck
            // 
            this.editCheck.Name = "editCheck";
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editValue4
            // 
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            this.editValue4.Name = "editValue4";
            // 
            // btnTransmitir
            // 
            this.btnTransmitir.Location = new System.Drawing.Point(5, 438);
            this.btnTransmitir.Name = "btnTransmitir";
            this.btnTransmitir.Size = new System.Drawing.Size(146, 23);
            this.btnTransmitir.TabIndex = 110;
            this.btnTransmitir.Text = "1109_btnTransmitir";
            this.btnTransmitir.UseVisualStyleBackColor = true;
            this.btnTransmitir.Click += new System.EventHandler(this.btnTransmitir_Click);
            // 
            // PagosElectronicos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1363, 567);
            this.Controls.Add(this.pnlMainContainer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PagosElectronicos";
            this.Text = "1109";
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gctrlHeader)).EndInit();
            this.gctrlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gctrlGrid)).EndInit();
            this.gctrlGrid.ResumeLayout(false);
            this.gctrlGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        protected DevExpress.XtraEditors.GroupControl gctrlHeader;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editDate;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        private System.Windows.Forms.Panel pnlDetail;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editText;
        protected System.Windows.Forms.Panel pnlGrids;
        private System.ComponentModel.IContainer components;
        private DevExpress.XtraEditors.GroupControl gctrlGrid;
        protected DevExpress.XtraGrid.GridControl gcPagos;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvPagos;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private uc_MasterFind masterCuenta;
        private System.Windows.Forms.Button btnTransmitir;       
    }
}