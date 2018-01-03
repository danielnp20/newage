namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PlaneacionRecursosSolicita
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaneacionRecursosSolicita));
            this.gvTarea = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcRecurso = new DevExpress.XtraGrid.GridControl();
            this.gvRecurso = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.richText = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editValue2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnDetails = new System.Windows.Forms.Panel();
            this.gcDetails = new DevExpress.XtraEditors.GroupControl();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.grpboxHeader = new System.Windows.Forms.GroupBox();
            this.btnFind = new DevExpress.XtraEditors.SimpleButton();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.chkSeleccionar = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            this.TbLyPanel.SuspendLayout();
            this.pnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvTarea
            // 
            this.gvTarea.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTarea.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvTarea.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTarea.Appearance.Empty.Options.UseBackColor = true;
            this.gvTarea.Appearance.Empty.Options.UseFont = true;
            this.gvTarea.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvTarea.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvTarea.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedCell.Options.UseFont = true;
            this.gvTarea.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvTarea.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvTarea.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvTarea.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedRow.Options.UseFont = true;
            this.gvTarea.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.GroupRow.BackColor = System.Drawing.Color.DarkSlateGray;
            this.gvTarea.Appearance.GroupRow.BackColor2 = System.Drawing.Color.DimGray;
            this.gvTarea.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.GroupRow.Options.UseBackColor = true;
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
            this.gvTarea.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvTarea.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvTarea.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvTarea.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvTarea.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.Row.Options.UseBackColor = true;
            this.gvTarea.Appearance.Row.Options.UseFont = true;
            this.gvTarea.Appearance.Row.Options.UseForeColor = true;
            this.gvTarea.Appearance.Row.Options.UseTextOptions = true;
            this.gvTarea.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvTarea.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvTarea.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.SelectedRow.Options.UseFont = true;
            this.gvTarea.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.TopNewRow.Options.UseFont = true;
            this.gvTarea.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvTarea.GridControl = this.gcRecurso;
            this.gvTarea.GroupFormat = "[#image]{1} {2}";
            this.gvTarea.HorzScrollStep = 50;
            this.gvTarea.Name = "gvTarea";
            this.gvTarea.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvTarea.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvTarea.OptionsBehavior.Editable = false;
            this.gvTarea.OptionsCustomization.AllowColumnMoving = false;
            this.gvTarea.OptionsCustomization.AllowFilter = false;
            this.gvTarea.OptionsCustomization.AllowSort = false;
            this.gvTarea.OptionsDetail.EnableMasterViewMode = false;
            this.gvTarea.OptionsMenu.EnableColumnMenu = false;
            this.gvTarea.OptionsMenu.EnableFooterMenu = false;
            this.gvTarea.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvTarea.OptionsView.ColumnAutoWidth = false;
            this.gvTarea.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvTarea.OptionsView.ShowGroupPanel = false;
            this.gvTarea.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvTarea.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvRecurso_CustomUnboundColumnData);
            // 
            // gcRecurso
            // 
            this.gcRecurso.AllowDrop = true;
            this.gcRecurso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcRecurso.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcRecurso.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcRecurso.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcRecurso.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcRecurso.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcRecurso.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcRecurso.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null)});
            this.gcRecurso.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcRecurso.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcRecurso.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcRecurso.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvTarea;
            gridLevelNode1.RelationName = "DetalleTareas";
            this.gcRecurso.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcRecurso.Location = new System.Drawing.Point(0, 0);
            this.gcRecurso.LookAndFeel.SkinName = "Dark Side";
            this.gcRecurso.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcRecurso.MainView = this.gvRecurso;
            this.gcRecurso.Margin = new System.Windows.Forms.Padding(5);
            this.gcRecurso.Name = "gcRecurso";
            this.gcRecurso.Size = new System.Drawing.Size(952, 446);
            this.gcRecurso.TabIndex = 52;
            this.gcRecurso.UseEmbeddedNavigator = true;
            this.gcRecurso.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRecurso,
            this.gvTarea});
            // 
            // gvRecurso
            // 
            this.gvRecurso.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecurso.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvRecurso.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.Empty.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvRecurso.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvRecurso.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.FooterPanel.Font = new System.Drawing.Font("Arial Narrow", 7.8F);
            this.gvRecurso.Appearance.FooterPanel.Options.UseFont = true;
            this.gvRecurso.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvRecurso.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.GroupFooter.Image = ((System.Drawing.Image)(resources.GetObject("gvRecurso.Appearance.GroupFooter.Image")));
            this.gvRecurso.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvRecurso.Appearance.GroupFooter.Options.UseImage = true;
            this.gvRecurso.Appearance.GroupFooter.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.GroupRow.BackColor = System.Drawing.Color.SteelBlue;
            this.gvRecurso.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvRecurso.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvRecurso.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvRecurso.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecurso.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvRecurso.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvRecurso.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvRecurso.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecurso.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvRecurso.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.Row.Options.UseBackColor = true;
            this.gvRecurso.Appearance.Row.Options.UseForeColor = true;
            this.gvRecurso.Appearance.Row.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvRecurso.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecurso.Appearance.VertLine.Options.UseBackColor = true;
            this.gvRecurso.GridControl = this.gcRecurso;
            this.gvRecurso.GroupFormat = "[#image]{1} ";
            this.gvRecurso.GroupRowHeight = 21;
            this.gvRecurso.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Unbound_CostoLocalTOT", null, "(SubTotal={0:c2})")});
            this.gvRecurso.HorzScrollStep = 50;
            this.gvRecurso.Name = "gvRecurso";
            this.gvRecurso.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.gvRecurso.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvRecurso.OptionsCustomization.AllowFilter = false;
            this.gvRecurso.OptionsMenu.EnableColumnMenu = false;
            this.gvRecurso.OptionsMenu.EnableFooterMenu = false;
            this.gvRecurso.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvRecurso.OptionsView.ColumnAutoWidth = false;
            this.gvRecurso.OptionsView.ShowAutoFilterRow = true;
            this.gvRecurso.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvRecurso.OptionsView.ShowGroupPanel = false;
            this.gvRecurso.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvRecurso.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvRecurso_CustomRowCellEdit);
            this.gvRecurso.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvRecurso_CustomRowCellEditForEditing);
            this.gvRecurso.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvRecurso_FocusedRowChanged);
            this.gvRecurso.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvRecurso_BeforeLeaveRow);
            this.gvRecurso.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvRecurso_CustomUnboundColumnData);
            this.gvRecurso.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvRecurso_CustomColumnDisplayText);
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.richText,
            this.riPopup,
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editLink,
            this.editValue2});
            // 
            // richText
            // 
            this.richText.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.WordML;
            this.richText.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.richText.Name = "richText";
            this.richText.ShowCaretInReadOnly = false;
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
            this.PopupContainerControl.Location = new System.Drawing.Point(3, 55);
            this.PopupContainerControl.Name = "PopupContainerControl";
            this.PopupContainerControl.Size = new System.Drawing.Size(7, 40);
            this.PopupContainerControl.TabIndex = 5;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(7, 40);
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
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // editValue2
            // 
            this.editValue2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editValue2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editValue2.Mask.EditMask = "n2";
            this.editValue2.Mask.UseMaskAsDisplayFormat = true;
            this.editValue2.Name = "editValue2";
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.587302F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.4127F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.TbLyPanel.Controls.Add(this.PopupContainerControl, 0, 1);
            this.TbLyPanel.Controls.Add(this.pnDetails, 1, 3);
            this.TbLyPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.TbLyPanel.Controls.Add(this.panel1, 1, 1);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 4;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.466F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.534005F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.TbLyPanel.Size = new System.Drawing.Size(980, 544);
            this.TbLyPanel.TabIndex = 0;
            // 
            // pnDetails
            // 
            this.pnDetails.Controls.Add(this.gcDetails);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDetails.Location = new System.Drawing.Point(18, 528);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(952, 13);
            this.pnDetails.TabIndex = 6;
            // 
            // gcDetails
            // 
            this.gcDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetails.Location = new System.Drawing.Point(0, 0);
            this.gcDetails.Name = "gcDetails";
            this.gcDetails.ShowCaption = false;
            this.gcDetails.Size = new System.Drawing.Size(952, 13);
            this.gcDetails.TabIndex = 0;
            this.gcDetails.Visible = false;
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
            this.grpctrlHeader.Location = new System.Drawing.Point(18, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(952, 46);
            this.grpctrlHeader.TabIndex = 8;
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.BackColor = System.Drawing.Color.Transparent;
            this.grpboxHeader.Controls.Add(this.masterProyecto);
            this.grpboxHeader.Controls.Add(this.btnFind);
            this.grpboxHeader.Controls.Add(this.lblDate);
            this.grpboxHeader.Controls.Add(this.dtFecha);
            this.grpboxHeader.Controls.Add(this.chkSeleccionar);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 2);
            this.grpboxHeader.Name = "grpboxHeader";
            this.grpboxHeader.Size = new System.Drawing.Size(948, 42);
            this.grpboxHeader.TabIndex = 8;
            this.grpboxHeader.TabStop = false;
            this.grpboxHeader.Visible = false;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(330, 16);
            this.btnFind.Margin = new System.Windows.Forms.Padding(2);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(92, 19);
            this.btnFind.TabIndex = 97;
            this.btnFind.Text = "33551_btnFind";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(139, 19);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(91, 14);
            this.lblDate.TabIndex = 96;
            this.lblDate.Text = "33551_lblDate";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(222, 17);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            this.dtFecha.TabIndex = 95;
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.AutoSize = true;
            this.chkSeleccionar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSeleccionar.Location = new System.Drawing.Point(9, 18);
            this.chkSeleccionar.Name = "chkSeleccionar";
            this.chkSeleccionar.Size = new System.Drawing.Size(113, 18);
            this.chkSeleccionar.TabIndex = 3;
            this.chkSeleccionar.Text = "32901_CheckAll";
            this.chkSeleccionar.UseVisualStyleBackColor = true;
            this.chkSeleccionar.CheckedChanged += new System.EventHandler(this.chkSeleccionar_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gcRecurso);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(18, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(952, 446);
            this.panel1.TabIndex = 7;
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(610, 15);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(4);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(300, 21);
            this.masterProyecto.TabIndex = 98;
            this.masterProyecto.Value = "";
            // 
            // PlaneacionRecursosSolicita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 544);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "PlaneacionRecursosSolicita";
            ((System.ComponentModel.ISupportInitialize)(this.gvTarea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
            this.pnDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richText;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit richText1;
        protected DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopup;
        protected System.Windows.Forms.TableLayoutPanel TbLyPanel;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControl;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected System.Windows.Forms.Panel pnDetails;
        public System.Windows.Forms.Panel panel1;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        public System.Windows.Forms.GroupBox grpboxHeader;
        protected System.Windows.Forms.CheckBox chkSeleccionar;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2;
        protected DevExpress.XtraEditors.GroupControl gcDetails;
        private DevExpress.XtraEditors.LabelControl lblDate;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        protected DevExpress.XtraGrid.GridControl gcRecurso;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvTarea;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvRecurso;
        private DevExpress.XtraEditors.SimpleButton btnFind;
        private ControlsUC.uc_MasterFind masterProyecto;
    }
}