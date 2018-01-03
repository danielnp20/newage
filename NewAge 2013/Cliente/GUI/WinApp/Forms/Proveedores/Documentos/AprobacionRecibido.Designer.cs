namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionRecibido
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
            this.RichText = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editValue2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gcDocuments = new DevExpress.XtraGrid.GridControl();
            this.gvDocuments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.dtDate = new DevExpress.XtraEditors.DateEdit();
            this.masterProveedor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gcDetails = new DevExpress.XtraGrid.GridControl();
            this.gvDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtIVALocal = new DevExpress.XtraEditors.TextEdit();
            this.txtIVAExtr = new DevExpress.XtraEditors.TextEdit();
            this.txtCostoLocal = new DevExpress.XtraEditors.TextEdit();
            this.txtCostoExt = new DevExpress.XtraEditors.TextEdit();
            this.lblIva1 = new System.Windows.Forms.Label();
            this.lblIva2 = new System.Windows.Forms.Label();
            this.lblCosto1 = new System.Windows.Forms.Label();
            this.lblCosto2 = new System.Windows.Forms.Label();
            this.masterProveedor_det = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterReferencia_det = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCodigoBS_det = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gcGoodRec = new DevExpress.XtraEditors.GroupControl();
            this.lblRecConf = new System.Windows.Forms.Label();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbSi = new System.Windows.Forms.RadioButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.rbSupEx = new System.Windows.Forms.RadioButton();
            this.rbTotal = new System.Windows.Forms.RadioButton();
            this.rbParc = new System.Windows.Forms.RadioButton();
            this.rbNoCump = new System.Windows.Forms.RadioButton();
            this.txtObservGR = new System.Windows.Forms.TextBox();
            this.lblObservGR = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.txtObservRech = new System.Windows.Forms.TextBox();
            this.lblObservRech = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.TbLyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVALocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVAExtr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoExt.Properties)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcGoodRec)).BeginInit();
            this.gcGoodRec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RichText,
            this.riPopup,
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editValue2,
            this.editLink});
            // 
            // RichText
            // 
            this.RichText.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.WordML;
            this.RichText.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.RichText.Name = "RichText";
            this.RichText.ShowCaretInReadOnly = false;
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
            this.PopupContainerControl.Location = new System.Drawing.Point(3, 33);
            this.PopupContainerControl.Name = "PopupContainerControl";
            this.PopupContainerControl.Size = new System.Drawing.Size(12, 40);
            this.PopupContainerControl.TabIndex = 5;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(12, 40);
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
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.SingleClick = true;
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.011173F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 97.98883F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.TbLyPanel.Controls.Add(this.PopupContainerControl, 0, 1);
            this.TbLyPanel.Controls.Add(this.gcDocuments, 1, 1);
            this.TbLyPanel.Controls.Add(this.panel1, 1, 0);
            this.TbLyPanel.Controls.Add(this.panel2, 1, 3);
            this.TbLyPanel.Controls.Add(this.panel3, 1, 4);
            this.TbLyPanel.Controls.Add(this.panel4, 1, 2);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 5;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 208F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TbLyPanel.Size = new System.Drawing.Size(1017, 602);
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
            this.gcDocuments.Location = new System.Drawing.Point(23, 33);
            this.gcDocuments.LookAndFeel.SkinName = "Dark Side";
            this.gcDocuments.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocuments.MainView = this.gvDocuments;
            this.gcDocuments.Name = "gcDocuments";
            this.gcDocuments.Size = new System.Drawing.Size(971, 184);
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
            this.gvDocuments.OptionsView.ShowGroupPanel = false;
            this.gvDocuments.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocuments_FocusedRowChanged);
            this.gvDocuments.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocuments_CellValueChanged);
            this.gvDocuments.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocuments_CellValueChanging);
            this.gvDocuments.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocuments_BeforeLeaveRow);
            this.gvDocuments.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            this.gvDocuments.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocuments_CustomColumnDisplayText);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblDate);
            this.panel1.Controls.Add(this.dtDate);
            this.panel1.Controls.Add(this.masterProveedor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(22, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(973, 26);
            this.panel1.TabIndex = 10;
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(6, 7);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(91, 14);
            this.lblDate.TabIndex = 96;
            this.lblDate.Text = "71556_lblDate";
            // 
            // dtDate
            // 
            this.dtDate.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtDate.Location = new System.Drawing.Point(90, 5);
            this.dtDate.Name = "dtDate";
            this.dtDate.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtDate.Properties.Appearance.Options.UseBackColor = true;
            this.dtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtDate.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtDate.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtDate.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtDate.Size = new System.Drawing.Size(100, 20);
            this.dtDate.TabIndex = 95;
            // 
            // masterProveedor
            // 
            this.masterProveedor.BackColor = System.Drawing.Color.Transparent;
            this.masterProveedor.Filtros = null;
            this.masterProveedor.Location = new System.Drawing.Point(203, 2);
            this.masterProveedor.Margin = new System.Windows.Forms.Padding(4);
            this.masterProveedor.Name = "masterProveedor";
            this.masterProveedor.Size = new System.Drawing.Size(317, 25);
            this.masterProveedor.TabIndex = 23;
            this.masterProveedor.Value = "";
            this.masterProveedor.Leave += new System.EventHandler(this.masterProveedor_Leave);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gcDetails);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(23, 328);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(971, 202);
            this.panel2.TabIndex = 13;
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
            this.gcDetails.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcDetails.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetails.Location = new System.Drawing.Point(0, 0);
            this.gcDetails.LookAndFeel.SkinName = "Dark Side";
            this.gcDetails.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetails.MainView = this.gvDetails;
            this.gcDetails.Name = "gcDetails";
            this.gcDetails.ShowOnlyPredefinedDetails = true;
            this.gcDetails.Size = new System.Drawing.Size(971, 202);
            this.gcDetails.TabIndex = 5;
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
            this.gvDetails.OptionsView.ShowGroupPanel = false;
            this.gvDetails.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetails_FocusedRowChanged);
            this.gvDetails.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            this.gvDetails.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocuments_CustomColumnDisplayText);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtIVALocal);
            this.panel3.Controls.Add(this.txtIVAExtr);
            this.panel3.Controls.Add(this.txtCostoLocal);
            this.panel3.Controls.Add(this.txtCostoExt);
            this.panel3.Controls.Add(this.lblIva1);
            this.panel3.Controls.Add(this.lblIva2);
            this.panel3.Controls.Add(this.lblCosto1);
            this.panel3.Controls.Add(this.lblCosto2);
            this.panel3.Controls.Add(this.masterProveedor_det);
            this.panel3.Controls.Add(this.masterReferencia_det);
            this.panel3.Controls.Add(this.masterCodigoBS_det);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(23, 536);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(971, 63);
            this.panel3.TabIndex = 14;
            // 
            // txtIVALocal
            // 
            this.txtIVALocal.EditValue = "0,00 ";
            this.txtIVALocal.Location = new System.Drawing.Point(713, 4);
            this.txtIVALocal.Name = "txtIVALocal";
            this.txtIVALocal.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtIVALocal.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtIVALocal.Properties.Appearance.Options.UseBorderColor = true;
            this.txtIVALocal.Properties.Appearance.Options.UseFont = true;
            this.txtIVALocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtIVALocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtIVALocal.Properties.AutoHeight = false;
            this.txtIVALocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIVALocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIVALocal.Properties.Mask.EditMask = "c";
            this.txtIVALocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtIVALocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtIVALocal.Properties.ReadOnly = true;
            this.txtIVALocal.Size = new System.Drawing.Size(131, 23);
            this.txtIVALocal.TabIndex = 36;
            // 
            // txtIVAExtr
            // 
            this.txtIVAExtr.EditValue = "0,00 ";
            this.txtIVAExtr.Location = new System.Drawing.Point(713, 28);
            this.txtIVAExtr.Name = "txtIVAExtr";
            this.txtIVAExtr.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtIVAExtr.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtIVAExtr.Properties.Appearance.Options.UseBorderColor = true;
            this.txtIVAExtr.Properties.Appearance.Options.UseFont = true;
            this.txtIVAExtr.Properties.Appearance.Options.UseTextOptions = true;
            this.txtIVAExtr.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtIVAExtr.Properties.AutoHeight = false;
            this.txtIVAExtr.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIVAExtr.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIVAExtr.Properties.Mask.EditMask = "c";
            this.txtIVAExtr.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtIVAExtr.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtIVAExtr.Properties.ReadOnly = true;
            this.txtIVAExtr.Size = new System.Drawing.Size(131, 23);
            this.txtIVAExtr.TabIndex = 35;
            // 
            // txtCostoLocal
            // 
            this.txtCostoLocal.EditValue = "0,00 ";
            this.txtCostoLocal.Location = new System.Drawing.Point(494, 4);
            this.txtCostoLocal.Name = "txtCostoLocal";
            this.txtCostoLocal.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCostoLocal.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtCostoLocal.Properties.Appearance.Options.UseBorderColor = true;
            this.txtCostoLocal.Properties.Appearance.Options.UseFont = true;
            this.txtCostoLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCostoLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCostoLocal.Properties.AutoHeight = false;
            this.txtCostoLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoLocal.Properties.Mask.EditMask = "c";
            this.txtCostoLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCostoLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCostoLocal.Properties.ReadOnly = true;
            this.txtCostoLocal.Size = new System.Drawing.Size(131, 23);
            this.txtCostoLocal.TabIndex = 34;
            // 
            // txtCostoExt
            // 
            this.txtCostoExt.EditValue = "0,00 ";
            this.txtCostoExt.Location = new System.Drawing.Point(494, 28);
            this.txtCostoExt.Name = "txtCostoExt";
            this.txtCostoExt.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCostoExt.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtCostoExt.Properties.Appearance.Options.UseBorderColor = true;
            this.txtCostoExt.Properties.Appearance.Options.UseFont = true;
            this.txtCostoExt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCostoExt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtCostoExt.Properties.AutoHeight = false;
            this.txtCostoExt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoExt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCostoExt.Properties.Mask.EditMask = "c";
            this.txtCostoExt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtCostoExt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtCostoExt.Properties.ReadOnly = true;
            this.txtCostoExt.Size = new System.Drawing.Size(131, 23);
            this.txtCostoExt.TabIndex = 33;
            // 
            // lblIva1
            // 
            this.lblIva1.AutoSize = true;
            this.lblIva1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIva1.Location = new System.Drawing.Point(631, 7);
            this.lblIva1.Name = "lblIva1";
            this.lblIva1.Size = new System.Drawing.Size(76, 14);
            this.lblIva1.TabIndex = 31;
            this.lblIva1.Text = "71556_lblIva";
            // 
            // lblIva2
            // 
            this.lblIva2.AutoSize = true;
            this.lblIva2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIva2.Location = new System.Drawing.Point(631, 30);
            this.lblIva2.Name = "lblIva2";
            this.lblIva2.Size = new System.Drawing.Size(98, 14);
            this.lblIva2.TabIndex = 27;
            this.lblIva2.Text = "71556_lblIvaExtr";
            // 
            // lblCosto1
            // 
            this.lblCosto1.AutoSize = true;
            this.lblCosto1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCosto1.Location = new System.Drawing.Point(406, 8);
            this.lblCosto1.Name = "lblCosto1";
            this.lblCosto1.Size = new System.Drawing.Size(91, 14);
            this.lblCosto1.TabIndex = 29;
            this.lblCosto1.Text = "71556_lblCosto";
            // 
            // lblCosto2
            // 
            this.lblCosto2.AutoSize = true;
            this.lblCosto2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCosto2.Location = new System.Drawing.Point(406, 31);
            this.lblCosto2.Name = "lblCosto2";
            this.lblCosto2.Size = new System.Drawing.Size(113, 14);
            this.lblCosto2.TabIndex = 25;
            this.lblCosto2.Text = "71556_lblCostoExtr";
            // 
            // masterProveedor_det
            // 
            this.masterProveedor_det.BackColor = System.Drawing.Color.Transparent;
            this.masterProveedor_det.Filtros = null;
            this.masterProveedor_det.Location = new System.Drawing.Point(16, 48);
            this.masterProveedor_det.Margin = new System.Windows.Forms.Padding(4);
            this.masterProveedor_det.Name = "masterProveedor_det";
            this.masterProveedor_det.Size = new System.Drawing.Size(310, 25);
            this.masterProveedor_det.TabIndex = 24;
            this.masterProveedor_det.Value = "";
            // 
            // masterReferencia_det
            // 
            this.masterReferencia_det.BackColor = System.Drawing.Color.Transparent;
            this.masterReferencia_det.Filtros = null;
            this.masterReferencia_det.Location = new System.Drawing.Point(16, 25);
            this.masterReferencia_det.Margin = new System.Windows.Forms.Padding(4);
            this.masterReferencia_det.Name = "masterReferencia_det";
            this.masterReferencia_det.Size = new System.Drawing.Size(310, 25);
            this.masterReferencia_det.TabIndex = 5;
            this.masterReferencia_det.Value = "";
            // 
            // masterCodigoBS_det
            // 
            this.masterCodigoBS_det.BackColor = System.Drawing.Color.Transparent;
            this.masterCodigoBS_det.Filtros = null;
            this.masterCodigoBS_det.Location = new System.Drawing.Point(16, 3);
            this.masterCodigoBS_det.Margin = new System.Windows.Forms.Padding(4);
            this.masterCodigoBS_det.Name = "masterCodigoBS_det";
            this.masterCodigoBS_det.Size = new System.Drawing.Size(310, 25);
            this.masterCodigoBS_det.TabIndex = 4;
            this.masterCodigoBS_det.Value = "";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gcGoodRec);
            this.panel4.Controls.Add(this.txtObservGR);
            this.panel4.Controls.Add(this.lblObservGR);
            this.panel4.Controls.Add(this.lblDetails);
            this.panel4.Controls.Add(this.txtObservRech);
            this.panel4.Controls.Add(this.lblObservRech);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(23, 223);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(971, 99);
            this.panel4.TabIndex = 15;
            // 
            // gcGoodRec
            // 
            this.gcGoodRec.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gcGoodRec.Appearance.Options.UseFont = true;
            this.gcGoodRec.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.gcGoodRec.AppearanceCaption.Options.UseFont = true;
            this.gcGoodRec.Controls.Add(this.lblRecConf);
            this.gcGoodRec.Controls.Add(this.rbNo);
            this.gcGoodRec.Controls.Add(this.rbSi);
            this.gcGoodRec.Controls.Add(this.panelControl1);
            this.gcGoodRec.Location = new System.Drawing.Point(630, 5);
            this.gcGoodRec.Name = "gcGoodRec";
            this.gcGoodRec.Size = new System.Drawing.Size(294, 82);
            this.gcGoodRec.TabIndex = 29;
            this.gcGoodRec.Text = "71556_gcGoodRec";
            // 
            // lblRecConf
            // 
            this.lblRecConf.AutoSize = true;
            this.lblRecConf.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecConf.Location = new System.Drawing.Point(8, 22);
            this.lblRecConf.Name = "lblRecConf";
            this.lblRecConf.Size = new System.Drawing.Size(94, 13);
            this.lblRecConf.TabIndex = 3;
            this.lblRecConf.Text = "71556_lblRecConf";
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNo.Location = new System.Drawing.Point(185, 20);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(84, 17);
            this.rbNo.TabIndex = 2;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "71556_rbNo";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbSi
            // 
            this.rbSi.AutoSize = true;
            this.rbSi.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSi.Location = new System.Drawing.Point(143, 20);
            this.rbSi.Name = "rbSi";
            this.rbSi.Size = new System.Drawing.Size(79, 17);
            this.rbSi.TabIndex = 1;
            this.rbSi.TabStop = true;
            this.rbSi.Text = "71556_rbSi";
            this.rbSi.UseVisualStyleBackColor = true;
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.rbSupEx);
            this.panelControl1.Controls.Add(this.rbTotal);
            this.panelControl1.Controls.Add(this.rbParc);
            this.panelControl1.Controls.Add(this.rbNoCump);
            this.panelControl1.Location = new System.Drawing.Point(0, 38);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(294, 44);
            this.panelControl1.TabIndex = 0;
            // 
            // rbSupEx
            // 
            this.rbSupEx.AutoSize = true;
            this.rbSupEx.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSupEx.Location = new System.Drawing.Point(143, 18);
            this.rbSupEx.Name = "rbSupEx";
            this.rbSupEx.Size = new System.Drawing.Size(101, 17);
            this.rbSupEx.TabIndex = 3;
            this.rbSupEx.TabStop = true;
            this.rbSupEx.Text = "71556_rbSupEx";
            this.rbSupEx.UseVisualStyleBackColor = true;
            // 
            // rbTotal
            // 
            this.rbTotal.AutoSize = true;
            this.rbTotal.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbTotal.Location = new System.Drawing.Point(143, 3);
            this.rbTotal.Name = "rbTotal";
            this.rbTotal.Size = new System.Drawing.Size(95, 17);
            this.rbTotal.TabIndex = 2;
            this.rbTotal.TabStop = true;
            this.rbTotal.Text = "71556_rbTotal";
            this.rbTotal.UseVisualStyleBackColor = true;
            // 
            // rbParc
            // 
            this.rbParc.AutoSize = true;
            this.rbParc.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbParc.Location = new System.Drawing.Point(10, 18);
            this.rbParc.Name = "rbParc";
            this.rbParc.Size = new System.Drawing.Size(92, 17);
            this.rbParc.TabIndex = 1;
            this.rbParc.TabStop = true;
            this.rbParc.Text = "71556_rbParc";
            this.rbParc.UseVisualStyleBackColor = true;
            // 
            // rbNoCump
            // 
            this.rbNoCump.AutoSize = true;
            this.rbNoCump.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNoCump.Location = new System.Drawing.Point(10, 3);
            this.rbNoCump.Name = "rbNoCump";
            this.rbNoCump.Size = new System.Drawing.Size(111, 17);
            this.rbNoCump.TabIndex = 0;
            this.rbNoCump.TabStop = true;
            this.rbNoCump.Text = "71556_rbNoCump";
            this.rbNoCump.UseVisualStyleBackColor = true;
            // 
            // txtObservGR
            // 
            this.txtObservGR.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservGR.Location = new System.Drawing.Point(325, 23);
            this.txtObservGR.Multiline = true;
            this.txtObservGR.Name = "txtObservGR";
            this.txtObservGR.Size = new System.Drawing.Size(291, 55);
            this.txtObservGR.TabIndex = 28;
            this.txtObservGR.Leave += new System.EventHandler(this.txtObservGR_Leave);
            // 
            // lblObservGR
            // 
            this.lblObservGR.AutoSize = true;
            this.lblObservGR.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservGR.Location = new System.Drawing.Point(324, 8);
            this.lblObservGR.Name = "lblObservGR";
            this.lblObservGR.Size = new System.Drawing.Size(113, 14);
            this.lblObservGR.TabIndex = 27;
            this.lblObservGR.Text = "71556_lblObservGR";
            // 
            // lblDetails
            // 
            this.lblDetails.AutoSize = true;
            this.lblDetails.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetails.Location = new System.Drawing.Point(4, 84);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(110, 14);
            this.lblDetails.TabIndex = 26;
            this.lblDetails.Text = "71556_lblDetails";
            // 
            // txtObservRech
            // 
            this.txtObservRech.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservRech.Location = new System.Drawing.Point(15, 23);
            this.txtObservRech.Multiline = true;
            this.txtObservRech.Name = "txtObservRech";
            this.txtObservRech.Size = new System.Drawing.Size(291, 55);
            this.txtObservRech.TabIndex = 25;
            this.txtObservRech.Leave += new System.EventHandler(this.txtObservRech_Leave);
            // 
            // lblObservRech
            // 
            this.lblObservRech.AutoSize = true;
            this.lblObservRech.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservRech.Location = new System.Drawing.Point(13, 8);
            this.lblObservRech.Name = "lblObservRech";
            this.lblObservRech.Size = new System.Drawing.Size(125, 14);
            this.lblObservRech.TabIndex = 24;
            this.lblObservRech.Text = "71556_lblObservRech";
            // 
            // AprobacionRecibido
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 602);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "AprobacionRecibido";
            this.Text = "71556";
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtDate.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVALocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIVAExtr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCostoExt.Properties)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcGoodRec)).EndInit();
            this.gcGoodRec.ResumeLayout(false);
            this.gcGoodRec.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit editRichText;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit RichText;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopup;
        private DevExpress.XtraEditors.PopupContainerControl PopupContainerControl;
        private DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocuments;
        private System.ComponentModel.IContainer components;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue2;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gcDocuments;
        private System.Windows.Forms.TableLayoutPanel TbLyPanel;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gcDetails;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetails;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.LabelControl lblDate;
        private DevExpress.XtraEditors.DateEdit dtDate;
        private System.Windows.Forms.Panel panel4;
        private ControlsUC.uc_MasterFind masterProveedor;
        private DevExpress.XtraEditors.GroupControl gcGoodRec;
        private System.Windows.Forms.Label lblRecConf;
        private System.Windows.Forms.RadioButton rbNo;
        private System.Windows.Forms.RadioButton rbSi;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.RadioButton rbSupEx;
        private System.Windows.Forms.RadioButton rbTotal;
        private System.Windows.Forms.RadioButton rbParc;
        private System.Windows.Forms.RadioButton rbNoCump;
        private System.Windows.Forms.TextBox txtObservGR;
        private System.Windows.Forms.Label lblObservGR;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.TextBox txtObservRech;
        private System.Windows.Forms.Label lblObservRech;
        private ControlsUC.uc_MasterFind masterProveedor_det;
        private ControlsUC.uc_MasterFind masterReferencia_det;
        private ControlsUC.uc_MasterFind masterCodigoBS_det;
        private System.Windows.Forms.Label lblIva1;
        private System.Windows.Forms.Label lblCosto1;
        private System.Windows.Forms.Label lblIva2;
        private System.Windows.Forms.Label lblCosto2;
        private DevExpress.XtraEditors.TextEdit txtIVALocal;
        private DevExpress.XtraEditors.TextEdit txtIVAExtr;
        private DevExpress.XtraEditors.TextEdit txtCostoLocal;
        private DevExpress.XtraEditors.TextEdit txtCostoExt;   
    }
}