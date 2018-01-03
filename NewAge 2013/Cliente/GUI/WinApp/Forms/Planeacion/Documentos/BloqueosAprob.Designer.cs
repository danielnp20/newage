namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class BloqueosAprob
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
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gcDocuments = new DevExpress.XtraGrid.GridControl();
            this.gvDocuments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTitleDet = new System.Windows.Forms.Label();
            this.txtObservDoc = new System.Windows.Forms.TextBox();
            this.lblObserv = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblGerencia = new System.Windows.Forms.Label();
            this.masterAreaFuncional = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnFind = new DevExpress.XtraEditors.SimpleButton();
            this.lblTipoDoc = new System.Windows.Forms.Label();
            this.txtNroDoc = new DevExpress.XtraEditors.TextEdit();
            this.cmbTipoDoc = new DevExpress.XtraEditors.LookUpEdit();
            this.lblNroDoc = new System.Windows.Forms.Label();
            this.masterPrefijoDoc = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnDetails = new System.Windows.Forms.Panel();
            this.gcDetails = new DevExpress.XtraGrid.GridControl();
            this.gvDetails = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnFindOrigen = new DevExpress.XtraEditors.SimpleButton();
            this.masterActividad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterRecurso = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtObservDetalle = new System.Windows.Forms.TextBox();
            this.lblOrigenTraslado = new System.Windows.Forms.Label();
            this.gcOrigen = new DevExpress.XtraGrid.GridControl();
            this.gvOrigenTraslado = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblObservDetalle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.TbLyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoDoc.Properties)).BeginInit();
            this.pnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcOrigen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrigenTraslado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
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
            this.PopupContainerControl.Location = new System.Drawing.Point(3, 44);
            this.PopupContainerControl.Name = "PopupContainerControl";
            this.PopupContainerControl.Size = new System.Drawing.Size(2, 40);
            this.PopupContainerControl.TabIndex = 5;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.7850834F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 99.21492F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
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
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 237F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TbLyPanel.Size = new System.Drawing.Size(1039, 629);
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
            this.gcDocuments.Location = new System.Drawing.Point(11, 44);
            this.gcDocuments.LookAndFeel.SkinName = "Dark Side";
            this.gcDocuments.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocuments.MainView = this.gvDocuments;
            this.gcDocuments.Name = "gcDocuments";
            this.gcDocuments.Size = new System.Drawing.Size(1012, 143);
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
            this.gvDocuments.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocuments_CustomRowCellEdit);
            this.gvDocuments.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocuments_FocusedRowChanged);
            this.gvDocuments.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocuments_CellValueChanged);
            this.gvDocuments.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDocuments_CellValueChanging);
            this.gvDocuments.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDocuments_BeforeLeaveRow);
            this.gvDocuments.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            this.gvDocuments.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocuments_CustomColumnDisplayText);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTitleDet);
            this.panel2.Controls.Add(this.txtObservDoc);
            this.panel2.Controls.Add(this.lblObserv);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(10, 192);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1014, 41);
            this.panel2.TabIndex = 7;
            // 
            // lblTitleDet
            // 
            this.lblTitleDet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitleDet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleDet.Location = new System.Drawing.Point(3, 25);
            this.lblTitleDet.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.lblTitleDet.Name = "lblTitleDet";
            this.lblTitleDet.Size = new System.Drawing.Size(120, 15);
            this.lblTitleDet.TabIndex = 8;
            this.lblTitleDet.Text = "25555_lblTitleDet";
            this.lblTitleDet.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtObservDoc
            // 
            this.txtObservDoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservDoc.Location = new System.Drawing.Point(717, 1);
            this.txtObservDoc.Multiline = true;
            this.txtObservDoc.Name = "txtObservDoc";
            this.txtObservDoc.Size = new System.Drawing.Size(293, 34);
            this.txtObservDoc.TabIndex = 31;
            // 
            // lblObserv
            // 
            this.lblObserv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblObserv.AutoSize = true;
            this.lblObserv.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObserv.Location = new System.Drawing.Point(609, 0);
            this.lblObserv.Name = "lblObserv";
            this.lblObserv.Size = new System.Drawing.Size(147, 14);
            this.lblObserv.TabIndex = 30;
            this.lblObserv.Text = "25555_lblObservacionDoc";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(10, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1014, 37);
            this.panel1.TabIndex = 10;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.lblGerencia);
            this.panel4.Controls.Add(this.masterAreaFuncional);
            this.panel4.Controls.Add(this.btnFind);
            this.panel4.Controls.Add(this.lblTipoDoc);
            this.panel4.Controls.Add(this.txtNroDoc);
            this.panel4.Controls.Add(this.cmbTipoDoc);
            this.panel4.Controls.Add(this.lblNroDoc);
            this.panel4.Controls.Add(this.masterPrefijoDoc);
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1011, 32);
            this.panel4.TabIndex = 107;
            // 
            // lblGerencia
            // 
            this.lblGerencia.AutoSize = true;
            this.lblGerencia.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGerencia.Location = new System.Drawing.Point(4, 8);
            this.lblGerencia.Name = "lblGerencia";
            this.lblGerencia.Size = new System.Drawing.Size(107, 14);
            this.lblGerencia.TabIndex = 21430;
            this.lblGerencia.Text = "25555_lblGerencia";
            // 
            // masterAreaFuncional
            // 
            this.masterAreaFuncional.BackColor = System.Drawing.Color.Transparent;
            this.masterAreaFuncional.Filtros = null;
            this.masterAreaFuncional.Location = new System.Drawing.Point(-7, 2);
            this.masterAreaFuncional.Name = "masterAreaFuncional";
            this.masterAreaFuncional.Size = new System.Drawing.Size(349, 25);
            this.masterAreaFuncional.TabIndex = 4;
            this.masterAreaFuncional.Value = "";
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnFind.Appearance.Options.UseFont = true;
            this.btnFind.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnFind.Location = new System.Drawing.Point(908, 3);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(97, 24);
            this.btnFind.TabIndex = 21429;
            this.btnFind.Text = "25555_btnFind";
            // 
            // lblTipoDoc
            // 
            this.lblTipoDoc.AutoSize = true;
            this.lblTipoDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoDoc.Location = new System.Drawing.Point(345, 7);
            this.lblTipoDoc.Name = "lblTipoDoc";
            this.lblTipoDoc.Size = new System.Drawing.Size(105, 14);
            this.lblTipoDoc.TabIndex = 114;
            this.lblTipoDoc.Text = "25555_lblTipoDoc";
            // 
            // txtNroDoc
            // 
            this.txtNroDoc.EditValue = "0";
            this.txtNroDoc.Location = new System.Drawing.Point(849, 4);
            this.txtNroDoc.Name = "txtNroDoc";
            this.txtNroDoc.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroDoc.Properties.Appearance.Options.UseFont = true;
            this.txtNroDoc.Properties.Appearance.Options.UseTextOptions = true;
            this.txtNroDoc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtNroDoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNroDoc.Properties.Mask.EditMask = "n0";
            this.txtNroDoc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtNroDoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtNroDoc.Size = new System.Drawing.Size(38, 22);
            this.txtNroDoc.TabIndex = 170;
            // 
            // cmbTipoDoc
            // 
            this.cmbTipoDoc.Location = new System.Drawing.Point(435, 4);
            this.cmbTipoDoc.Name = "cmbTipoDoc";
            this.cmbTipoDoc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoDoc.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoDoc.Properties.DisplayMember = "Value";
            this.cmbTipoDoc.Properties.NullText = " ";
            this.cmbTipoDoc.Properties.ValueMember = "Key";
            this.cmbTipoDoc.Size = new System.Drawing.Size(97, 20);
            this.cmbTipoDoc.TabIndex = 115;
            // 
            // lblNroDoc
            // 
            this.lblNroDoc.AutoSize = true;
            this.lblNroDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroDoc.Location = new System.Drawing.Point(793, 9);
            this.lblNroDoc.Name = "lblNroDoc";
            this.lblNroDoc.Size = new System.Drawing.Size(89, 14);
            this.lblNroDoc.TabIndex = 171;
            this.lblNroDoc.Text = "25555_NroDoc";
            // 
            // masterPrefijoDoc
            // 
            this.masterPrefijoDoc.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoDoc.Filtros = null;
            this.masterPrefijoDoc.Location = new System.Drawing.Point(549, 3);
            this.masterPrefijoDoc.Name = "masterPrefijoDoc";
            this.masterPrefijoDoc.Size = new System.Drawing.Size(350, 27);
            this.masterPrefijoDoc.TabIndex = 169;
            this.masterPrefijoDoc.Value = "";
            // 
            // pnDetails
            // 
            this.pnDetails.Controls.Add(this.gcDetails);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDetails.Location = new System.Drawing.Point(11, 238);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(1012, 153);
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
            this.gcDetails.Size = new System.Drawing.Size(1012, 153);
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
            this.gvDetails.OptionsBehavior.Editable = false;
            this.gvDetails.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetails.OptionsCustomization.AllowFilter = false;
            this.gvDetails.OptionsCustomization.AllowSort = false;
            this.gvDetails.OptionsView.ShowGroupPanel = false;
            this.gvDetails.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocuments_CustomRowCellEdit);
            this.gvDetails.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetails_FocusedRowChanged);
            this.gvDetails.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.txtObservDetalle);
            this.panel3.Controls.Add(this.lblOrigenTraslado);
            this.panel3.Controls.Add(this.gcOrigen);
            this.panel3.Controls.Add(this.lblObservDetalle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(11, 397);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1012, 231);
            this.panel3.TabIndex = 13;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.masterProyecto);
            this.panel5.Controls.Add(this.btnFindOrigen);
            this.panel5.Controls.Add(this.masterActividad);
            this.panel5.Controls.Add(this.masterRecurso);
            this.panel5.Location = new System.Drawing.Point(7, 35);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(706, 57);
            this.panel5.TabIndex = 21431;
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(4, 3);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(350, 27);
            this.masterProyecto.TabIndex = 90;
            this.masterProyecto.Value = "";
            // 
            // btnFindOrigen
            // 
            this.btnFindOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnFindOrigen.Appearance.Options.UseFont = true;
            this.btnFindOrigen.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnFindOrigen.Location = new System.Drawing.Point(612, 29);
            this.btnFindOrigen.Name = "btnFindOrigen";
            this.btnFindOrigen.Size = new System.Drawing.Size(83, 23);
            this.btnFindOrigen.TabIndex = 21430;
            this.btnFindOrigen.Text = "25555_btnFind";
            // 
            // masterActividad
            // 
            this.masterActividad.BackColor = System.Drawing.Color.Transparent;
            this.masterActividad.Filtros = null;
            this.masterActividad.Location = new System.Drawing.Point(3, 28);
            this.masterActividad.Name = "masterActividad";
            this.masterActividad.Size = new System.Drawing.Size(350, 27);
            this.masterActividad.TabIndex = 91;
            this.masterActividad.Value = "";
            // 
            // masterRecurso
            // 
            this.masterRecurso.BackColor = System.Drawing.Color.Transparent;
            this.masterRecurso.Filtros = null;
            this.masterRecurso.Location = new System.Drawing.Point(355, 3);
            this.masterRecurso.Name = "masterRecurso";
            this.masterRecurso.Size = new System.Drawing.Size(350, 27);
            this.masterRecurso.TabIndex = 98;
            this.masterRecurso.Value = "";
            // 
            // txtObservDetalle
            // 
            this.txtObservDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservDetalle.Location = new System.Drawing.Point(723, 0);
            this.txtObservDetalle.Multiline = true;
            this.txtObservDetalle.Name = "txtObservDetalle";
            this.txtObservDetalle.Size = new System.Drawing.Size(288, 37);
            this.txtObservDetalle.TabIndex = 27;
            // 
            // lblOrigenTraslado
            // 
            this.lblOrigenTraslado.AutoSize = true;
            this.lblOrigenTraslado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrigenTraslado.Location = new System.Drawing.Point(7, 19);
            this.lblOrigenTraslado.Name = "lblOrigenTraslado";
            this.lblOrigenTraslado.Size = new System.Drawing.Size(160, 14);
            this.lblOrigenTraslado.TabIndex = 30;
            this.lblOrigenTraslado.Text = "25555_lblOrigenTraslado";
            // 
            // gcOrigen
            // 
            this.gcOrigen.AllowDrop = true;
            this.gcOrigen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcOrigen.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcOrigen.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcOrigen.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcOrigen.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcOrigen.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcOrigen.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcOrigen.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcOrigen.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcOrigen.Location = new System.Drawing.Point(3, 94);
            this.gcOrigen.LookAndFeel.SkinName = "Dark Side";
            this.gcOrigen.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcOrigen.MainView = this.gvOrigenTraslado;
            this.gcOrigen.Name = "gcOrigen";
            this.gcOrigen.Size = new System.Drawing.Size(963, 114);
            this.gcOrigen.TabIndex = 10;
            this.gcOrigen.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOrigenTraslado,
            this.gridView1});
            // 
            // gvOrigenTraslado
            // 
            this.gvOrigenTraslado.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOrigenTraslado.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvOrigenTraslado.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvOrigenTraslado.Appearance.Empty.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvOrigenTraslado.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigenTraslado.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvOrigenTraslado.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvOrigenTraslado.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigenTraslado.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigenTraslado.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvOrigenTraslado.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigenTraslado.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvOrigenTraslado.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvOrigenTraslado.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvOrigenTraslado.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOrigenTraslado.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvOrigenTraslado.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvOrigenTraslado.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvOrigenTraslado.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvOrigenTraslado.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvOrigenTraslado.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvOrigenTraslado.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvOrigenTraslado.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigenTraslado.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvOrigenTraslado.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvOrigenTraslado.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvOrigenTraslado.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvOrigenTraslado.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvOrigenTraslado.Appearance.Row.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.Row.Options.UseForeColor = true;
            this.gvOrigenTraslado.Appearance.Row.Options.UseTextOptions = true;
            this.gvOrigenTraslado.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvOrigenTraslado.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigenTraslado.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvOrigenTraslado.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigenTraslado.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvOrigenTraslado.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvOrigenTraslado.Appearance.VertLine.Options.UseBackColor = true;
            this.gvOrigenTraslado.GridControl = this.gcOrigen;
            this.gvOrigenTraslado.Name = "gvOrigenTraslado";
            this.gvOrigenTraslado.OptionsCustomization.AllowColumnMoving = false;
            this.gvOrigenTraslado.OptionsCustomization.AllowFilter = false;
            this.gvOrigenTraslado.OptionsCustomization.AllowSort = false;
            this.gvOrigenTraslado.OptionsView.ShowGroupPanel = false;
            this.gvOrigenTraslado.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocuments_CustomRowCellEdit);
            this.gvOrigenTraslado.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetFooter_FocusedRowChanged);
            this.gvOrigenTraslado.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocuments_CustomUnboundColumnData);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcOrigen;
            this.gridView1.Name = "gridView1";
            // 
            // lblObservDetalle
            // 
            this.lblObservDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblObservDetalle.AutoSize = true;
            this.lblObservDetalle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservDetalle.Location = new System.Drawing.Point(612, 1);
            this.lblObservDetalle.Name = "lblObservDetalle";
            this.lblObservDetalle.Size = new System.Drawing.Size(135, 14);
            this.lblObservDetalle.TabIndex = 26;
            this.lblObservDetalle.Text = "25555_lblObservDetalle";
            // 
            // BloqueosAprob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 629);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "BloqueosAprob";
            this.Text = "25555";
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.TbLyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoDoc.Properties)).EndInit();
            this.pnDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetails)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcOrigen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrigenTraslado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit editRichText;
        protected DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit RichText;
        protected DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopup;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControl;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControl;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocuments;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetails;
        protected DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gcDocuments;
        private DevExpress.XtraGrid.GridControl gcDetails;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel TbLyPanel;
        private ControlsUC.uc_MasterFind masterAreaFuncional;
        protected System.Windows.Forms.Panel pnDetails;
        private System.Windows.Forms.Label lblTitleDet;
        private System.Windows.Forms.TextBox txtObservDetalle;
        private System.Windows.Forms.Label lblObservDetalle;
        private DevExpress.XtraGrid.GridControl gcOrigen;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvOrigenTraslado;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.TextBox txtObservDoc;
        private System.Windows.Forms.Label lblObserv;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblOrigenTraslado;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoDoc;
        private System.Windows.Forms.Label lblTipoDoc;
        private DevExpress.XtraEditors.TextEdit txtNroDoc;
        private System.Windows.Forms.Label lblNroDoc;
        private ControlsUC.uc_MasterFind masterPrefijoDoc;
        private DevExpress.XtraEditors.SimpleButton btnFind;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterActividad;
        private DevExpress.XtraEditors.SimpleButton btnFindOrigen;
        private ControlsUC.uc_MasterFind masterRecurso;
        private System.Windows.Forms.Label lblGerencia;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;   
    }
}