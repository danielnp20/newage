namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class SolicitudRevisionForm
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
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
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
            this.pnDetails = new System.Windows.Forms.Panel();
            this.gcTareas = new DevExpress.XtraGrid.GridControl();
            this.gvTareas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLibranza = new DevExpress.XtraEditors.TextEdit();
            this.masterActividad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
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
            this.pnDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareas)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLibranza.Properties)).BeginInit();
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
            this.riPopup.PopupFormMinSize = new System.Drawing.Size(500, 0);
            this.riPopup.PopupFormSize = new System.Drawing.Size(500, 300);
            this.riPopup.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
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
            this.editCmb.Name = "editCmb";
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Name = "editBtnGrid";
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(32, 185);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(852, 8);
            this.panel1.TabIndex = 7;
            // 
            // pnDetails
            // 
            this.pnDetails.Controls.Add(this.gcTareas);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDetails.Location = new System.Drawing.Point(32, 199);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(852, 280);
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
            this.gcTareas.Size = new System.Drawing.Size(852, 280);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.panelControl1);
            this.panel3.Controls.Add(this.masterActividad);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(32, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(852, 176);
            this.panel3.TabIndex = 8;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.txtLibranza);
            this.panelControl1.Location = new System.Drawing.Point(12, 15);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(222, 35);
            this.panelControl1.TabIndex = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 60;
            this.label1.Text = "32317_Libranza";
            // 
            // txtLibranza
            // 
            this.txtLibranza.EditValue = "";
            this.txtLibranza.Location = new System.Drawing.Point(116, 8);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtLibranza.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Properties.Appearance.Options.UseBorderColor = true;
            this.txtLibranza.Properties.Appearance.Options.UseFont = true;
            this.txtLibranza.Properties.Appearance.Options.UseTextOptions = true;
            this.txtLibranza.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtLibranza.Properties.AutoHeight = false;
            this.txtLibranza.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtLibranza.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtLibranza.Properties.Mask.EditMask = "n0";
            this.txtLibranza.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtLibranza.Size = new System.Drawing.Size(95, 20);
            this.txtLibranza.TabIndex = 78;
            // 
            // masterActividad
            // 
            this.masterActividad.BackColor = System.Drawing.Color.Transparent;
            this.masterActividad.Filtros = null;
            this.masterActividad.Location = new System.Drawing.Point(15, 61);
            this.masterActividad.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.masterActividad.Name = "masterActividad";
            this.masterActividad.Size = new System.Drawing.Size(298, 25);
            this.masterActividad.TabIndex = 13;
            this.masterActividad.Value = "";
            this.masterActividad.Leave += new System.EventHandler(this.masterActividad_Leave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.285871F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 96.71413F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnDetails, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 286F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(908, 482);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // SolicitudRevisionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 482);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SolicitudRevisionForm";
            this.Text = "DocumentAprobTareasForm";
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
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
            this.pnDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTareas)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLibranza.Properties)).EndInit();
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
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControlAnexos;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControlTareas;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControlTareas;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControlAnexos;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
        protected System.Windows.Forms.Panel pnDetails;
        protected System.Windows.Forms.Panel panel1;
        protected System.Windows.Forms.Panel panel3;
        protected DevExpress.XtraGrid.GridControl gcTareas;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvTareas;
        protected System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        protected ControlsUC.uc_MasterFind masterActividad;
        private System.ComponentModel.IContainer components;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        protected DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit editCmbDict;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtLibranza;
    }
}