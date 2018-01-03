namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalPlanPagos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModalPlanPagos));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.groupCotiza = new DevExpress.XtraEditors.GroupControl();
            this.txtValorDoc = new DevExpress.XtraEditors.TextEdit();
            this.lblValorDoc = new System.Windows.Forms.Label();
            this.pnHeader = new DevExpress.XtraEditors.PanelControl();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.txtDocumentoNro = new System.Windows.Forms.TextBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.lblPrefijoOC = new System.Windows.Forms.Label();
            this.lblDocumentNro = new System.Windows.Forms.Label();
            this.masterPrefijoOC = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnAccept = new System.Windows.Forms.Button();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.toolTip = new DevExpress.Utils.ToolTipController(this.components);
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editMemo = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.toolTipGrid = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupCotiza)).BeginInit();
            this.groupCotiza.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnHeader)).BeginInit();
            this.pnHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editMemo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupCotiza
            // 
            this.groupCotiza.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupCotiza.Appearance.Options.UseBackColor = true;
            this.groupCotiza.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupCotiza.AppearanceCaption.Options.UseFont = true;
            this.groupCotiza.AppearanceCaption.Options.UseTextOptions = true;
            this.groupCotiza.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupCotiza.CaptionLocation = DevExpress.Utils.Locations.Bottom;
            this.groupCotiza.Controls.Add(this.txtValorDoc);
            this.groupCotiza.Controls.Add(this.lblValorDoc);
            this.groupCotiza.Controls.Add(this.pnHeader);
            this.groupCotiza.Controls.Add(this.btnAccept);
            this.groupCotiza.Controls.Add(this.gcData);
            this.groupCotiza.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupCotiza.Location = new System.Drawing.Point(0, 0);
            this.groupCotiza.LookAndFeel.SkinName = "iMaginary";
            this.groupCotiza.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupCotiza.Margin = new System.Windows.Forms.Padding(4);
            this.groupCotiza.Name = "groupCotiza";
            this.groupCotiza.Size = new System.Drawing.Size(747, 393);
            this.groupCotiza.TabIndex = 0;
            // 
            // txtValorDoc
            // 
            this.txtValorDoc.EditValue = "0";
            this.txtValorDoc.Location = new System.Drawing.Point(144, 359);
            this.txtValorDoc.Margin = new System.Windows.Forms.Padding(4);
            this.txtValorDoc.Name = "txtValorDoc";
            this.txtValorDoc.Properties.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtValorDoc.Properties.Appearance.Options.UseBackColor = true;
            this.txtValorDoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorDoc.Properties.Mask.EditMask = "c";
            this.txtValorDoc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorDoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorDoc.Properties.ReadOnly = true;
            this.txtValorDoc.Size = new System.Drawing.Size(173, 22);
            this.txtValorDoc.TabIndex = 81;
            this.txtValorDoc.Visible = false;
            // 
            // lblValorDoc
            // 
            this.lblValorDoc.AutoSize = true;
            this.lblValorDoc.BackColor = System.Drawing.Color.Transparent;
            this.lblValorDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorDoc.Location = new System.Drawing.Point(13, 361);
            this.lblValorDoc.Margin = new System.Windows.Forms.Padding(23, 0, 4, 0);
            this.lblValorDoc.Name = "lblValorDoc";
            this.lblValorDoc.Size = new System.Drawing.Size(120, 17);
            this.lblValorDoc.TabIndex = 80;
            this.lblValorDoc.Text = "1040_lblValorDoc";
            this.lblValorDoc.Visible = false;
            // 
            // pnHeader
            // 
            this.pnHeader.Controls.Add(this.btnQueryDoc);
            this.pnHeader.Controls.Add(this.txtDocumentoNro);
            this.pnHeader.Controls.Add(this.btnGet);
            this.pnHeader.Controls.Add(this.lblPrefijoOC);
            this.pnHeader.Controls.Add(this.lblDocumentNro);
            this.pnHeader.Controls.Add(this.masterPrefijoOC);
            this.pnHeader.Location = new System.Drawing.Point(11, 7);
            this.pnHeader.Margin = new System.Windows.Forms.Padding(4);
            this.pnHeader.Name = "pnHeader";
            this.pnHeader.Size = new System.Drawing.Size(728, 37);
            this.pnHeader.TabIndex = 79;
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(516, 6);
            this.btnQueryDoc.Margin = new System.Windows.Forms.Padding(4);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(37, 25);
            this.btnQueryDoc.TabIndex = 21428;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Visible = false;
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // txtDocumentoNro
            // 
            this.txtDocumentoNro.Location = new System.Drawing.Point(467, 6);
            this.txtDocumentoNro.Margin = new System.Windows.Forms.Padding(11, 4, 4, 4);
            this.txtDocumentoNro.Name = "txtDocumentoNro";
            this.txtDocumentoNro.Size = new System.Drawing.Size(47, 23);
            this.txtDocumentoNro.TabIndex = 3;
            this.txtDocumentoNro.Visible = false;
            this.txtDocumentoNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDocumentoNro_KeyPress);
            // 
            // btnGet
            // 
            this.btnGet.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet.Location = new System.Drawing.Point(591, 5);
            this.btnGet.Margin = new System.Windows.Forms.Padding(4);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(125, 28);
            this.btnGet.TabIndex = 79;
            this.btnGet.Text = "1040_btnGetOC";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Visible = false;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // lblPrefijoOC
            // 
            this.lblPrefijoOC.AutoSize = true;
            this.lblPrefijoOC.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefijoOC.Location = new System.Drawing.Point(3, 10);
            this.lblPrefijoOC.Margin = new System.Windows.Forms.Padding(23, 0, 4, 0);
            this.lblPrefijoOC.Name = "lblPrefijoOC";
            this.lblPrefijoOC.Size = new System.Drawing.Size(120, 18);
            this.lblPrefijoOC.TabIndex = 4;
            this.lblPrefijoOC.Text = "1040_lblPrefijoOC";
            this.lblPrefijoOC.Visible = false;
            // 
            // lblDocumentNro
            // 
            this.lblDocumentNro.AutoSize = true;
            this.lblDocumentNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumentNro.Location = new System.Drawing.Point(400, 10);
            this.lblDocumentNro.Margin = new System.Windows.Forms.Padding(23, 0, 4, 0);
            this.lblDocumentNro.Name = "lblDocumentNro";
            this.lblDocumentNro.Size = new System.Drawing.Size(123, 18);
            this.lblDocumentNro.TabIndex = 2;
            this.lblDocumentNro.Text = "1040_lblNroPrefijo";
            this.lblDocumentNro.Visible = false;
            // 
            // masterPrefijoOC
            // 
            this.masterPrefijoOC.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoOC.Filtros = null;
            this.masterPrefijoOC.Location = new System.Drawing.Point(4, 4);
            this.masterPrefijoOC.Margin = new System.Windows.Forms.Padding(5);
            this.masterPrefijoOC.Name = "masterPrefijoOC";
            this.masterPrefijoOC.Size = new System.Drawing.Size(399, 31);
            this.masterPrefijoOC.TabIndex = 0;
            this.masterPrefijoOC.Value = "";
            this.masterPrefijoOC.Visible = false;
            // 
            // btnAccept
            // 
            this.btnAccept.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccept.Location = new System.Drawing.Point(576, 356);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(4);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(157, 32);
            this.btnAccept.TabIndex = 78;
            this.btnAccept.Text = "1040_btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // gcData
            // 
            this.gcData.AllowDrop = true;
            this.gcData.Cursor = System.Windows.Forms.Cursors.IBeam;
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
            this.gcData.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.gcData.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcData.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcData.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcData.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcData_EmbeddedNavigator_ButtonClick);
            this.gcData.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcData.Location = new System.Drawing.Point(12, 49);
            this.gcData.LookAndFeel.SkinName = "Dark Side";
            this.gcData.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcData.MainView = this.gvData;
            this.gcData.Margin = new System.Windows.Forms.Padding(5);
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(727, 299);
            this.gcData.TabIndex = 0;
            this.gcData.ToolTipController = this.toolTip;
            this.gcData.UseEmbeddedNavigator = true;
            this.gcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
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
            this.gvData.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gvData_FocusedColumnChanged);
            this.gvData.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvData_CellValueChanged);
            this.gvData.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvData_BeforeLeaveRow);
            this.gvData.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvData_CustomUnboundColumnData);
            // 
            // ttControler
            // 
            this.toolTip.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.toolTip.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.toolTip.Appearance.Options.UseBackColor = true;
            this.toolTip.Appearance.Options.UseFont = true;
            this.toolTip.IconSize = DevExpress.Utils.ToolTipIconSize.Large;
            this.toolTip.Rounded = true;
            this.toolTip.ShowBeak = true;
            this.toolTip.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            this.toolTip.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTip_GetActiveObjectInfo);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editDate,
            this.editBtnGrid,
            this.editSpin,
            this.editMemo});
            // 
            // editDate
            // 
            this.editDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)});
            this.editDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "Seleccione Fecha", null, null, true)});
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            this.editDate.Name = "editDate";
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("editBtnGrid.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editMemo
            // 
            this.editMemo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editMemo.Name = "editMemo";
            this.editMemo.PopupResizeMode = DevExpress.XtraEditors.Controls.ResizeMode.LiveResize;
            // 
            // ModalPlanPagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 393);
            this.Controls.Add(this.groupCotiza);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "ModalPlanPagos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "140";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ModalPlanPagos_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.groupCotiza)).EndInit();
            this.groupCotiza.ResumeLayout(false);
            this.groupCotiza.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnHeader)).EndInit();
            this.pnHeader.ResumeLayout(false);
            this.pnHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editMemo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        internal DevExpress.XtraEditors.GroupControl groupCotiza;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private System.Windows.Forms.ToolTip toolTipGrid;
        private System.Windows.Forms.Button btnAccept;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        private DevExpress.XtraEditors.PanelControl pnHeader;
        private ControlsUC.uc_MasterFind masterPrefijoOC;
        private System.Windows.Forms.Label lblDocumentNro;
        protected System.Windows.Forms.TextBox txtDocumentoNro;
        private System.Windows.Forms.Label lblPrefijoOC;
        private System.Windows.Forms.Button btnGet;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private System.Windows.Forms.Label lblValorDoc;
        private DevExpress.XtraEditors.TextEdit txtValorDoc;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit editMemo;
        private DevExpress.Utils.ToolTipController toolTip;
    }
}