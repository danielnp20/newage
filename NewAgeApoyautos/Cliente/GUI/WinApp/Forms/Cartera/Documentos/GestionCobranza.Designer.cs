namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class GestionCobranza
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editPopUp = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gbGarantiaHipotecaria = new System.Windows.Forms.GroupBox();
            this.txtHistoria = new DevExpress.XtraEditors.MemoEdit();
            this.groupDeudor = new DevExpress.XtraEditors.GroupControl();
            this.gcCodeudor = new DevExpress.XtraGrid.GridControl();
            this.gvCodeudor = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnDetails = new System.Windows.Forms.Panel();
            this.gbFilterGar = new DevExpress.XtraEditors.GroupControl();
            this.cmbOrden = new DevExpress.XtraEditors.LookUpEdit();
            this.lblOrden = new DevExpress.XtraEditors.LabelControl();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPopUp)).BeginInit();
            this.TbLyPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.gbGarantiaHipotecaria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHistoria.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDeudor)).BeginInit();
            this.groupDeudor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCodeudor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCodeudor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGar)).BeginInit();
            this.gbFilterGar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrden.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvDetalle.GridControl = this.gcData;
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsFind.AllowFindPanel = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvData_CustomUnboundColumnData);
            this.gvDetalle.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvData_CustomColumnDisplayText);
            // 
            // gcData
            // 
            this.gcData.AllowDrop = true;
            this.gcData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcData.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcData.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcData.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcData.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcData.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcData.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcData.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcData.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcData.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcData.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcData.EmbeddedNavigator.Buttons.NextPage.Enabled = false;
            this.gcData.EmbeddedNavigator.Buttons.PrevPage.Enabled = false;
            this.gcData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcData.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcData.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcData.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcData.Location = new System.Drawing.Point(0, 0);
            this.gcData.LookAndFeel.SkinName = "Dark Side";
            this.gcData.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcData.MainView = this.gvData;
            this.gcData.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(972, 246);
            this.gcData.TabIndex = 1;
            this.gcData.UseEmbeddedNavigator = true;
            this.gcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData,
            this.gvDetalle});
            // 
            // gvData
            // 
            this.gvData.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvData.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvData.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvData.Appearance.Empty.Options.UseBackColor = true;
            this.gvData.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvData.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvData.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvData.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvData.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvData.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvData.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvData.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvData.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvData.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvData.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvData.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvData.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvData.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvData.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvData.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvData.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvData.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvData.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvData.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvData.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvData.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.Row.Options.UseBackColor = true;
            this.gvData.Appearance.Row.Options.UseFont = true;
            this.gvData.Appearance.Row.Options.UseForeColor = true;
            this.gvData.Appearance.Row.Options.UseTextOptions = true;
            this.gvData.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvData.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvData.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvData.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvData.GridControl = this.gcData;
            this.gvData.HorzScrollStep = 50;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvData.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvData.OptionsCustomization.AllowColumnMoving = false;
            this.gvData.OptionsCustomization.AllowFilter = false;
            this.gvData.OptionsCustomization.AllowSort = false;
            this.gvData.OptionsFind.AllowFindPanel = false;
            this.gvData.OptionsMenu.EnableColumnMenu = false;
            this.gvData.OptionsMenu.EnableFooterMenu = false;
            this.gvData.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvData.OptionsView.ShowGroupPanel = false;
            this.gvData.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            this.gvData.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvData_CellValueChanged);
            this.gvData.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvData_BeforeLeaveRow);
            this.gvData.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvData_CustomUnboundColumnData);
            this.gvData.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvData_CustomColumnDisplayText);
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riPopup,
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editLink,
            this.editPopUp});
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
            // 
            // editPopUp
            // 
            this.editPopUp.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editPopUp.Name = "editPopUp";
            this.editPopUp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.editPopUp.ShowIcon = false;
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
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TbLyPanel.Size = new System.Drawing.Size(1003, 629);
            this.TbLyPanel.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gbGarantiaHipotecaria);
            this.panel4.Controls.Add(this.groupDeudor);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(14, 341);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(972, 286);
            this.panel4.TabIndex = 11;
            // 
            // gbGarantiaHipotecaria
            // 
            this.gbGarantiaHipotecaria.Controls.Add(this.txtHistoria);
            this.gbGarantiaHipotecaria.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbGarantiaHipotecaria.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbGarantiaHipotecaria.Location = new System.Drawing.Point(714, 0);
            this.gbGarantiaHipotecaria.Name = "gbGarantiaHipotecaria";
            this.gbGarantiaHipotecaria.Size = new System.Drawing.Size(258, 281);
            this.gbGarantiaHipotecaria.TabIndex = 124;
            this.gbGarantiaHipotecaria.TabStop = false;
            this.gbGarantiaHipotecaria.Text = "32502_gbHistoria";
            // 
            // txtHistoria
            // 
            this.txtHistoria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHistoria.EditValue = "";
            this.txtHistoria.Location = new System.Drawing.Point(0, 16);
            this.txtHistoria.Name = "txtHistoria";
            this.txtHistoria.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 7.5F, System.Drawing.FontStyle.Bold);
            this.txtHistoria.Properties.Appearance.Options.UseFont = true;
            this.txtHistoria.Properties.LinesCount = 100;
            this.txtHistoria.Properties.ReadOnly = true;
            this.txtHistoria.Properties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHistoria.Properties.WordWrap = false;
            this.txtHistoria.Size = new System.Drawing.Size(252, 265);
            this.txtHistoria.TabIndex = 128;
            // 
            // groupDeudor
            // 
            this.groupDeudor.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupDeudor.Appearance.Options.UseFont = true;
            this.groupDeudor.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupDeudor.AppearanceCaption.Options.UseFont = true;
            this.groupDeudor.Controls.Add(this.gcCodeudor);
            this.groupDeudor.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupDeudor.Location = new System.Drawing.Point(0, 0);
            this.groupDeudor.Name = "groupDeudor";
            this.groupDeudor.Size = new System.Drawing.Size(714, 286);
            this.groupDeudor.TabIndex = 113;
            this.groupDeudor.Text = "32502_gbCodeudor";
            // 
            // gcCodeudor
            // 
            this.gcCodeudor.AllowDrop = true;
            this.gcCodeudor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcCodeudor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCodeudor.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCodeudor.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcCodeudor.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcCodeudor.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcCodeudor.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcCodeudor.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcCodeudor.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcCodeudor.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcCodeudor.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcCodeudor.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcCodeudor.EmbeddedNavigator.Buttons.NextPage.Enabled = false;
            this.gcCodeudor.EmbeddedNavigator.Buttons.PrevPage.Enabled = false;
            this.gcCodeudor.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcCodeudor.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcCodeudor.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcCodeudor.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcCodeudor.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcCodeudor.Location = new System.Drawing.Point(2, 22);
            this.gcCodeudor.LookAndFeel.SkinName = "Dark Side";
            this.gcCodeudor.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcCodeudor.MainView = this.gvCodeudor;
            this.gcCodeudor.Margin = new System.Windows.Forms.Padding(4);
            this.gcCodeudor.Name = "gcCodeudor";
            this.gcCodeudor.Size = new System.Drawing.Size(710, 262);
            this.gcCodeudor.TabIndex = 124;
            this.gcCodeudor.UseEmbeddedNavigator = true;
            this.gcCodeudor.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCodeudor,
            this.gridView1});
            // 
            // gvCodeudor
            // 
            this.gvCodeudor.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCodeudor.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvCodeudor.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvCodeudor.Appearance.Empty.Options.UseBackColor = true;
            this.gvCodeudor.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvCodeudor.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvCodeudor.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvCodeudor.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvCodeudor.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvCodeudor.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvCodeudor.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvCodeudor.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvCodeudor.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvCodeudor.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvCodeudor.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCodeudor.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvCodeudor.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCodeudor.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvCodeudor.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvCodeudor.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvCodeudor.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvCodeudor.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCodeudor.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvCodeudor.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvCodeudor.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvCodeudor.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvCodeudor.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCodeudor.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvCodeudor.Appearance.Row.Options.UseBackColor = true;
            this.gvCodeudor.Appearance.Row.Options.UseFont = true;
            this.gvCodeudor.Appearance.Row.Options.UseForeColor = true;
            this.gvCodeudor.Appearance.Row.Options.UseTextOptions = true;
            this.gvCodeudor.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvCodeudor.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvCodeudor.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvCodeudor.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvCodeudor.GridControl = this.gcCodeudor;
            this.gvCodeudor.HorzScrollStep = 50;
            this.gvCodeudor.Name = "gvCodeudor";
            this.gvCodeudor.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvCodeudor.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvCodeudor.OptionsCustomization.AllowColumnMoving = false;
            this.gvCodeudor.OptionsCustomization.AllowFilter = false;
            this.gvCodeudor.OptionsCustomization.AllowSort = false;
            this.gvCodeudor.OptionsFind.AllowFindPanel = false;
            this.gvCodeudor.OptionsMenu.EnableColumnMenu = false;
            this.gvCodeudor.OptionsMenu.EnableFooterMenu = false;
            this.gvCodeudor.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvCodeudor.OptionsView.ColumnAutoWidth = false;
            this.gvCodeudor.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvCodeudor.OptionsView.ShowGroupPanel = false;
            this.gvCodeudor.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvCodeudor.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvData_CustomUnboundColumnData);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcCodeudor;
            this.gridView1.Name = "gridView1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gcData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(14, 91);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(972, 246);
            this.panel2.TabIndex = 11;
            // 
            // pnDetails
            // 
            this.pnDetails.Controls.Add(this.gbFilterGar);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDetails.Location = new System.Drawing.Point(15, 3);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(970, 83);
            this.pnDetails.TabIndex = 6;
            // 
            // gbFilterGar
            // 
            this.gbFilterGar.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterGar.Appearance.Options.UseFont = true;
            this.gbFilterGar.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterGar.AppearanceCaption.Options.UseFont = true;
            this.gbFilterGar.Controls.Add(this.cmbOrden);
            this.gbFilterGar.Controls.Add(this.lblOrden);
            this.gbFilterGar.Controls.Add(this.masterCliente);
            this.gbFilterGar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilterGar.Location = new System.Drawing.Point(0, 0);
            this.gbFilterGar.Name = "gbFilterGar";
            this.gbFilterGar.Size = new System.Drawing.Size(970, 83);
            this.gbFilterGar.TabIndex = 103;
            this.gbFilterGar.Text = "32502_gbFilter";
            // 
            // cmbOrden
            // 
            this.cmbOrden.Location = new System.Drawing.Point(131, 57);
            this.cmbOrden.Name = "cmbOrden";
            this.cmbOrden.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbOrden.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbOrden.Properties.DisplayMember = "Value";
            this.cmbOrden.Properties.ValueMember = "Key";
            this.cmbOrden.Size = new System.Drawing.Size(113, 20);
            this.cmbOrden.TabIndex = 106;
            // 
            // lblOrden
            // 
            this.lblOrden.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblOrden.Location = new System.Drawing.Point(13, 60);
            this.lblOrden.Name = "lblOrden";
            this.lblOrden.Size = new System.Drawing.Size(87, 14);
            this.lblOrden.TabIndex = 105;
            this.lblOrden.Text = "32502_lblOrden";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(14, 28);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(408, 27);
            this.masterCliente.TabIndex = 91;
            this.masterCliente.Value = "";
            // 
            // GestionCobranza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 629);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "GestionCobranza";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editPopUp)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.gbGarantiaHipotecaria.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtHistoria.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDeudor)).EndInit();
            this.groupDeudor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCodeudor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCodeudor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.pnDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGar)).EndInit();
            this.gbFilterGar.ResumeLayout(false);
            this.gbFilterGar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrden.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit editRichText;
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
        private DevExpress.XtraEditors.GroupControl groupDeudor;
        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraEditors.GroupControl gbFilterGar;
        private DevExpress.XtraEditors.LookUpEdit cmbOrden;
        private DevExpress.XtraEditors.LabelControl lblOrden;
        private ControlsUC.uc_MasterFind masterCliente;
        private System.Windows.Forms.GroupBox gbGarantiaHipotecaria;
        private DevExpress.XtraGrid.GridControl gcCodeudor;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCodeudor;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit editPopUp;
        private DevExpress.XtraEditors.MemoEdit txtHistoria;   
    }
}