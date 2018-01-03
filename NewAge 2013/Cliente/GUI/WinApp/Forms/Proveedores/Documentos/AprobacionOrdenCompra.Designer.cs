namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionOrdenCompra
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
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue2Cant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gcDocuments = new DevExpress.XtraGrid.GridControl();
            this.gvDocuments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtInstrucciones = new System.Windows.Forms.TextBox();
            this.lblTitileDet = new System.Windows.Forms.Label();
            this.masterLocalidad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblJustif = new System.Windows.Forms.Label();
            this.txtObservOC = new System.Windows.Forms.TextBox();
            this.lblObserv = new System.Windows.Forms.Label();
            this.txtTotalMdaExt = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalMdaLocal = new DevExpress.XtraEditors.TextEdit();
            this.lblTotalMdaExt = new System.Windows.Forms.Label();
            this.lblTotalMonLoc = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnDetails = new System.Windows.Forms.Panel();
            this.gcDetails = new DevExpress.XtraGrid.GridControl();
            this.gvDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.gcDetFooter = new DevExpress.XtraGrid.GridControl();
            this.gvDetFooter = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblDescripDetalle = new System.Windows.Forms.Label();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.TbLyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalMdaExt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalMdaLocal.Properties)).BeginInit();
            this.pnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetFooter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riPopup,
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editValue2Cant,
            this.editLink});
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
            this.PopupContainerControl.Location = new System.Drawing.Point(3, 16);
            this.PopupContainerControl.Name = "PopupContainerControl";
            this.PopupContainerControl.Size = new System.Drawing.Size(9, 40);
            this.PopupContainerControl.TabIndex = 5;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(9, 40);
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
            // editValue2Cant
            // 
            this.editValue2Cant.AllowMouseWheel = false;
            this.editValue2Cant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2Cant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue2Cant.Mask.EditMask = "n2";
            this.editValue2Cant.Mask.UseMaskAsDisplayFormat = true;
            this.editValue2Cant.Name = "editValue2Cant";
            // 
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.SingleClick = true;
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.570167F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.42983F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.TbLyPanel.Controls.Add(this.PopupContainerControl, 0, 1);
            this.TbLyPanel.Controls.Add(this.gcDocuments, 1, 1);
            this.TbLyPanel.Controls.Add(this.panel2, 1, 2);
            this.TbLyPanel.Controls.Add(this.panel1, 1, 0);
            this.TbLyPanel.Controls.Add(this.pnDetails, 1, 3);
            this.TbLyPanel.Controls.Add(this.panel3, 1, 4);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 6;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 183F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 206F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TbLyPanel.Size = new System.Drawing.Size(1022, 602);
            this.TbLyPanel.TabIndex = 0;
            // 
            // gcDocuments
            // 
            this.gcDocuments.AllowDrop = true;
            this.gcDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocuments.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocuments.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocuments.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocuments.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocuments.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocuments.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocuments.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDocuments.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcDocuments.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocuments.Location = new System.Drawing.Point(18, 16);
            this.gcDocuments.LookAndFeel.SkinName = "Dark Side";
            this.gcDocuments.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocuments.MainView = this.gvDocuments;
            this.gcDocuments.Name = "gcDocuments";
            this.gcDocuments.Size = new System.Drawing.Size(981, 177);
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
            this.gvDocuments.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocuments_FocusedRowChanged);
            this.gvDocuments.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocuments_CellValueChanged);
            this.gvDocuments.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocuments_CellValueChanging);
            this.gvDocuments.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocuments_BeforeLeaveRow);
            this.gvDocuments.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            this.gvDocuments.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocuments_CustomColumnDisplayText);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtInstrucciones);
            this.panel2.Controls.Add(this.lblTitileDet);
            this.panel2.Controls.Add(this.masterLocalidad);
            this.panel2.Controls.Add(this.lblJustif);
            this.panel2.Controls.Add(this.txtObservOC);
            this.panel2.Controls.Add(this.lblObserv);
            this.panel2.Controls.Add(this.txtTotalMdaExt);
            this.panel2.Controls.Add(this.txtTotalMdaLocal);
            this.panel2.Controls.Add(this.lblTotalMdaExt);
            this.panel2.Controls.Add(this.lblTotalMonLoc);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(17, 198);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(983, 103);
            this.panel2.TabIndex = 7;
            // 
            // txtInstrucciones
            // 
            this.txtInstrucciones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstrucciones.Location = new System.Drawing.Point(124, 28);
            this.txtInstrucciones.Multiline = true;
            this.txtInstrucciones.Name = "txtInstrucciones";
            this.txtInstrucciones.ReadOnly = true;
            this.txtInstrucciones.Size = new System.Drawing.Size(618, 19);
            this.txtInstrucciones.TabIndex = 33;
            // 
            // lblTitileDet
            // 
            this.lblTitileDet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitileDet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitileDet.Location = new System.Drawing.Point(3, 87);
            this.lblTitileDet.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.lblTitileDet.Name = "lblTitileDet";
            this.lblTitileDet.Size = new System.Drawing.Size(120, 15);
            this.lblTitileDet.TabIndex = 8;
            this.lblTitileDet.Text = "71555_lblTitleDet";
            this.lblTitileDet.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // masterLocalidad
            // 
            this.masterLocalidad.BackColor = System.Drawing.Color.Transparent;
            this.masterLocalidad.Filtros = null;
            this.masterLocalidad.Location = new System.Drawing.Point(7, 48);
            this.masterLocalidad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterLocalidad.Name = "masterLocalidad";
            this.masterLocalidad.Size = new System.Drawing.Size(360, 25);
            this.masterLocalidad.TabIndex = 4;
            this.masterLocalidad.Value = "";
            // 
            // lblJustif
            // 
            this.lblJustif.AutoSize = true;
            this.lblJustif.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJustif.Location = new System.Drawing.Point(4, 30);
            this.lblJustif.Name = "lblJustif";
            this.lblJustif.Size = new System.Drawing.Size(79, 14);
            this.lblJustif.TabIndex = 32;
            this.lblJustif.Text = "Instrucciones";
            // 
            // txtObservOC
            // 
            this.txtObservOC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservOC.Location = new System.Drawing.Point(124, 5);
            this.txtObservOC.Multiline = true;
            this.txtObservOC.Name = "txtObservOC";
            this.txtObservOC.ReadOnly = true;
            this.txtObservOC.Size = new System.Drawing.Size(618, 20);
            this.txtObservOC.TabIndex = 31;
            // 
            // lblObserv
            // 
            this.lblObserv.AutoSize = true;
            this.lblObserv.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObserv.Location = new System.Drawing.Point(4, 8);
            this.lblObserv.Name = "lblObserv";
            this.lblObserv.Size = new System.Drawing.Size(138, 14);
            this.lblObserv.TabIndex = 30;
            this.lblObserv.Text = "71555_lblObservaciones";
            // 
            // txtTotalMdaExt
            // 
            this.txtTotalMdaExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalMdaExt.EditValue = "0,00 ";
            this.txtTotalMdaExt.Location = new System.Drawing.Point(857, 23);
            this.txtTotalMdaExt.Name = "txtTotalMdaExt";
            this.txtTotalMdaExt.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalMdaExt.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtTotalMdaExt.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalMdaExt.Properties.Appearance.Options.UseFont = true;
            this.txtTotalMdaExt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalMdaExt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalMdaExt.Properties.AutoHeight = false;
            this.txtTotalMdaExt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalMdaExt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalMdaExt.Properties.Mask.EditMask = "c";
            this.txtTotalMdaExt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalMdaExt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalMdaExt.Properties.ReadOnly = true;
            this.txtTotalMdaExt.Size = new System.Drawing.Size(123, 20);
            this.txtTotalMdaExt.TabIndex = 12;
            // 
            // txtTotalMdaLocal
            // 
            this.txtTotalMdaLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalMdaLocal.EditValue = "0";
            this.txtTotalMdaLocal.Location = new System.Drawing.Point(857, 1);
            this.txtTotalMdaLocal.Name = "txtTotalMdaLocal";
            this.txtTotalMdaLocal.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalMdaLocal.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtTotalMdaLocal.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalMdaLocal.Properties.Appearance.Options.UseFont = true;
            this.txtTotalMdaLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalMdaLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalMdaLocal.Properties.AutoHeight = false;
            this.txtTotalMdaLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalMdaLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalMdaLocal.Properties.Mask.EditMask = "c";
            this.txtTotalMdaLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalMdaLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalMdaLocal.Properties.ReadOnly = true;
            this.txtTotalMdaLocal.Size = new System.Drawing.Size(123, 21);
            this.txtTotalMdaLocal.TabIndex = 11;
            // 
            // lblTotalMdaExt
            // 
            this.lblTotalMdaExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalMdaExt.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalMdaExt.Location = new System.Drawing.Point(746, 25);
            this.lblTotalMdaExt.Name = "lblTotalMdaExt";
            this.lblTotalMdaExt.Size = new System.Drawing.Size(110, 18);
            this.lblTotalMdaExt.TabIndex = 10;
            this.lblTotalMdaExt.Text = "71555_lblTotalMonExt";
            this.lblTotalMdaExt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalMonLoc
            // 
            this.lblTotalMonLoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalMonLoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalMonLoc.Location = new System.Drawing.Point(746, 4);
            this.lblTotalMonLoc.Name = "lblTotalMonLoc";
            this.lblTotalMonLoc.Size = new System.Drawing.Size(111, 17);
            this.lblTotalMonLoc.TabIndex = 8;
            this.lblTotalMonLoc.Text = "71555_lblTotalMonLoc";
            this.lblTotalMonLoc.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(17, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(983, 9);
            this.panel1.TabIndex = 10;
            // 
            // pnDetails
            // 
            this.pnDetails.Controls.Add(this.gcDetails);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDetails.Location = new System.Drawing.Point(18, 306);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(981, 200);
            this.pnDetails.TabIndex = 6;
            // 
            // gcDetails
            // 
            this.gcDetails.AllowDrop = true;
            this.gcDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetails.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetails.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetails.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetails.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetails.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetails.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetails.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDetails.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetails.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcDetails.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetails.Location = new System.Drawing.Point(0, 0);
            this.gcDetails.LookAndFeel.SkinName = "Dark Side";
            this.gcDetails.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetails.MainView = this.gvDetails;
            this.gcDetails.Name = "gcDetails";
            this.gcDetails.ShowOnlyPredefinedDetails = true;
            this.gcDetails.Size = new System.Drawing.Size(981, 200);
            this.gcDetails.TabIndex = 4;
            this.gcDetails.UseEmbeddedNavigator = true;
            this.gcDetails.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetails});
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
            this.gvDetails.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 7.6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvDetails.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.3F);
            this.gvDetails.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetails.Appearance.Row.Options.UseBackColor = true;
            this.gvDetails.Appearance.Row.Options.UseFont = true;
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
            this.gvDetails.OptionsBehavior.Editable = false;
            this.gvDetails.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetails.OptionsCustomization.AllowFilter = false;
            this.gvDetails.OptionsCustomization.AllowSort = false;
            this.gvDetails.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvDetails.OptionsView.ShowGroupPanel = false;
            this.gvDetails.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetails_FocusedRowChanged);
            this.gvDetails.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.gcDetFooter);
            this.panel3.Controls.Add(this.lblDescripDetalle);
            this.panel3.Controls.Add(this.txtObservaciones);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(18, 512);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(981, 84);
            this.panel3.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(570, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 14);
            this.label1.TabIndex = 30;
            this.label1.Text = "71555_lblDistribucion";
            // 
            // gcDetFooter
            // 
            this.gcDetFooter.AllowDrop = true;
            this.gcDetFooter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetFooter.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetFooter.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetFooter.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetFooter.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetFooter.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetFooter.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetFooter.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetFooter.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetFooter.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetFooter.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDetFooter.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetFooter.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcDetFooter.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetFooter.Location = new System.Drawing.Point(573, 17);
            this.gcDetFooter.LookAndFeel.SkinName = "Dark Side";
            this.gcDetFooter.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetFooter.MainView = this.gvDetFooter;
            this.gcDetFooter.Name = "gcDetFooter";
            this.gcDetFooter.Size = new System.Drawing.Size(389, 63);
            this.gcDetFooter.TabIndex = 10;
            this.gcDetFooter.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetFooter,
            this.gridView1});
            // 
            // gvDetFooter
            // 
            this.gvDetFooter.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetFooter.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetFooter.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetFooter.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetFooter.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetFooter.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetFooter.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetFooter.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetFooter.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetFooter.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetFooter.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetFooter.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetFooter.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetFooter.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetFooter.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetFooter.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetFooter.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetFooter.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetFooter.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetFooter.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetFooter.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetFooter.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetFooter.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetFooter.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetFooter.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetFooter.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetFooter.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetFooter.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetFooter.Appearance.Row.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.Row.Options.UseForeColor = true;
            this.gvDetFooter.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetFooter.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetFooter.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetFooter.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetFooter.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetFooter.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetFooter.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetFooter.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetFooter.GridControl = this.gcDetFooter;
            this.gvDetFooter.Name = "gvDetFooter";
            this.gvDetFooter.OptionsBehavior.Editable = false;
            this.gvDetFooter.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetFooter.OptionsCustomization.AllowFilter = false;
            this.gvDetFooter.OptionsCustomization.AllowSort = false;
            this.gvDetFooter.OptionsView.ShowGroupPanel = false;
            this.gvDetFooter.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetFooter_FocusedRowChanged);
            this.gvDetFooter.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcDetFooter;
            this.gridView1.Name = "gridView1";
            // 
            // lblDescripDetalle
            // 
            this.lblDescripDetalle.AutoSize = true;
            this.lblDescripDetalle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripDetalle.Location = new System.Drawing.Point(3, 3);
            this.lblDescripDetalle.Name = "lblDescripDetalle";
            this.lblDescripDetalle.Size = new System.Drawing.Size(136, 14);
            this.lblDescripDetalle.TabIndex = 26;
            this.lblDescripDetalle.Text = "71555_lblDescripDetalle";
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(3, 20);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.ReadOnly = true;
            this.txtObservaciones.Size = new System.Drawing.Size(432, 54);
            this.txtObservaciones.TabIndex = 27;
            // 
            // AprobacionOrdenCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 602);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "AprobacionOrdenCompra";
            this.Text = "71555";
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2Cant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalMdaExt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalMdaLocal.Properties)).EndInit();
            this.pnDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetFooter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetFooter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        protected DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopup;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControl;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControl;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocuments;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2Cant;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetails;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gcDocuments;
        private DevExpress.XtraGrid.GridControl gcDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel TbLyPanel;
        private ControlsUC.uc_MasterFind masterLocalidad;
        private System.Windows.Forms.Label lblTotalMdaExt;
        private System.Windows.Forms.Label lblTotalMonLoc;
        protected System.Windows.Forms.Panel pnDetails;
        private System.Windows.Forms.Label lblTitileDet;
        private DevExpress.XtraEditors.TextEdit txtTotalMdaExt;
        private DevExpress.XtraEditors.TextEdit txtTotalMdaLocal;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Label lblDescripDetalle;
        private DevExpress.XtraGrid.GridControl gcDetFooter;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetFooter;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.TextBox txtInstrucciones;
        private System.Windows.Forms.Label lblJustif;
        private System.Windows.Forms.TextBox txtObservOC;
        private System.Windows.Forms.Label lblObserv;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;   
    }
}