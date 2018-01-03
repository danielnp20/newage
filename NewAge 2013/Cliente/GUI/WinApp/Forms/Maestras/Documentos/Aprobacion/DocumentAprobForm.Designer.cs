namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DocumentAprobForm
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
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.richText = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gcDocuments = new DevExpress.XtraGrid.GridControl();
            this.gvDocuments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnDetails = new System.Windows.Forms.Panel();
            this.gcDetails = new DevExpress.XtraGrid.GridControl();
            this.gvDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.grpboxHeader = new System.Windows.Forms.GroupBox();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSeleccionar = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            this.TbLyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).BeginInit();
            this.pnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.editCmb});
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
            this.PopupContainerControl.Location = new System.Drawing.Point(3, 48);
            this.PopupContainerControl.Name = "PopupContainerControl";
            this.PopupContainerControl.Size = new System.Drawing.Size(11, 40);
            this.PopupContainerControl.TabIndex = 5;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(11, 40);
            this.richEditControl.TabIndex = 2;
            this.richEditControl.Text = "myRichEditControl";
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
            // editCmb
            // 
            this.editCmb.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editCmb.Name = "editCmb";
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.011173F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 97.98883F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.TbLyPanel.Controls.Add(this.PopupContainerControl, 0, 1);
            this.TbLyPanel.Controls.Add(this.gcDocuments, 1, 1);
            this.TbLyPanel.Controls.Add(this.pnDetails, 1, 3);
            this.TbLyPanel.Controls.Add(this.panel1, 1, 2);
            this.TbLyPanel.Controls.Add(this.grpctrlHeader, 1, 4);
            this.TbLyPanel.Controls.Add(this.groupControl1, 1, 0);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 5;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.30434F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.695652F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.TbLyPanel.Size = new System.Drawing.Size(904, 482);
            this.TbLyPanel.TabIndex = 0;
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
            this.gcDocuments.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDocuments.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcDocuments.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocuments.Location = new System.Drawing.Point(20, 48);
            this.gcDocuments.LookAndFeel.SkinName = "Dark Side";
            this.gcDocuments.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocuments.MainView = this.gvDocuments;
            this.gcDocuments.Name = "gcDocuments";
            this.gcDocuments.Size = new System.Drawing.Size(861, 142);
            this.gcDocuments.TabIndex = 3;
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
            this.gvDocuments.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvDocuments.OptionsView.ShowGroupPanel = false;
            this.gvDocuments.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocuments_CustomRowCellEdit);
            this.gvDocuments.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocuments_CustomRowCellEditForEditing);
            this.gvDocuments.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocuments_FocusedRowChanged);
            this.gvDocuments.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocuments_CellValueChanging);
            this.gvDocuments.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocuments_BeforeLeaveRow);
            this.gvDocuments.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            this.gvDocuments.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocuments_CustomColumnDisplayText);
            // 
            // pnDetails
            // 
            this.pnDetails.Controls.Add(this.gcDetails);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDetails.Location = new System.Drawing.Point(20, 210);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(861, 212);
            this.pnDetails.TabIndex = 6;
            // 
            // gcDetails
            // 
            this.gcDetails.AllowDrop = true;
            this.gcDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetails.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetails.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetails.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetails.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetails.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDetails.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDetails.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcDetails.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetails.Location = new System.Drawing.Point(0, 0);
            this.gcDetails.LookAndFeel.SkinName = "Dark Side";
            this.gcDetails.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetails.MainView = this.gvDetails;
            this.gcDetails.Name = "gcDetails";
            this.gcDetails.Size = new System.Drawing.Size(861, 212);
            this.gcDetails.TabIndex = 4;
            this.gcDetails.UseEmbeddedNavigator = true;
            this.gcDetails.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetails,
            this.gridView1});
            // 
            // gvDetails
            // 
            this.gvDetails.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetails.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetails.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetails.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetails.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetails.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetails.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetails.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetails.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetails.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetails.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetails.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetails.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetails.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetails.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetails.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetails.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetails.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetails.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetails.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetails.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetails.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetails.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetails.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetails.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetails.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetails.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetails.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetails.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetails.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetails.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetails.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.Row.Options.UseBackColor = true;
            this.gvDetails.Appearance.Row.Options.UseForeColor = true;
            this.gvDetails.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetails.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetails.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetails.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetails.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetails.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetails.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetails.GridControl = this.gcDetails;
            this.gvDetails.Name = "gvDetails";
            this.gvDetails.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetails.OptionsCustomization.AllowFilter = false;
            this.gvDetails.OptionsCustomization.AllowSort = false;
            this.gvDetails.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvDetails.OptionsView.ShowGroupPanel = false;
            this.gvDetails.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDetails_CustomRowCellEdit);
            this.gvDetails.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDetails_CustomRowCellEditForEditing);
            this.gvDetails.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcDetails;
            this.gridView1.Name = "gridView1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 196);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(861, 8);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(861, 8);
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
            this.grpctrlHeader.Location = new System.Drawing.Point(20, 428);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(861, 51);
            this.grpctrlHeader.TabIndex = 8;
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.BackColor = System.Drawing.Color.Transparent;
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 2);
            this.grpboxHeader.Name = "grpboxHeader";
            this.grpboxHeader.Size = new System.Drawing.Size(857, 47);
            this.grpboxHeader.TabIndex = 8;
            this.grpboxHeader.TabStop = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupControl1.AppearanceCaption.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.groupControl1.Controls.Add(this.groupBox1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(20, 3);
            this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(861, 39);
            this.groupControl1.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.chkSeleccionar);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(857, 35);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.AutoSize = true;
            this.chkSeleccionar.Location = new System.Drawing.Point(14, 12);
            this.chkSeleccionar.Name = "chkSeleccionar";
            this.chkSeleccionar.Size = new System.Drawing.Size(102, 17);
            this.chkSeleccionar.TabIndex = 0;
            this.chkSeleccionar.Text = "32901_CheckAll";
            this.chkSeleccionar.UseVisualStyleBackColor = true;
            this.chkSeleccionar.CheckedChanged += new System.EventHandler(this.chkSeleccionar_CheckedChanged);
            // 
            // DocumentAprobForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 482);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "DocumentAprobForm";
            ((System.ComponentModel.ISupportInitialize)(this.richText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).EndInit();
            this.pnDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        protected DevExpress.XtraGrid.GridControl gcDocuments;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocuments;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected System.Windows.Forms.Panel pnDetails;
        protected DevExpress.XtraGrid.GridControl gcDetails;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetails;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel panel2;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        public System.Windows.Forms.GroupBox grpboxHeader;
        public DevExpress.XtraEditors.GroupControl groupControl1;
        public System.Windows.Forms.GroupBox groupBox1;
        protected System.Windows.Forms.CheckBox chkSeleccionar;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
    }
}