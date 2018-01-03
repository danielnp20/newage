namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalPolizasCartera
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editBtnMvto = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editBtnDetail = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gbHeader = new DevExpress.XtraEditors.GroupControl();
            this.gcPoliza = new DevExpress.XtraGrid.GridControl();
            this.gvPoliza = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.btnFilter = new DevExpress.XtraEditors.SimpleButton();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
            this.lblPoliza = new System.Windows.Forms.Label();
            this.txtPoliza = new System.Windows.Forms.TextBox();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblTercero = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbHeader)).BeginInit();
            this.gbHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPoliza)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPoliza)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editBtnMvto,
            this.editBtnDetail});
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editBtnMvto
            // 
            this.editBtnMvto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver Movimiento", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.editBtnMvto.Name = "editBtnMvto";
            this.editBtnMvto.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnMvto.ValidateOnEnterKey = true;
            // 
            // editBtnDetail
            // 
            this.editBtnDetail.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver detalle", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "Ver detalle de la referencia", null, null, true)});
            this.editBtnDetail.Name = "editBtnDetail";
            this.editBtnDetail.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnDetail.ValidateOnEnterKey = true;
            // 
            // gbHeader
            // 
            this.gbHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbHeader.AppearanceCaption.Options.UseFont = true;
            this.gbHeader.AppearanceCaption.Options.UseTextOptions = true;
            this.gbHeader.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbHeader.Controls.Add(this.gcPoliza);
            this.gbHeader.Location = new System.Drawing.Point(4, 1);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.ShowCaption = false;
            this.gbHeader.Size = new System.Drawing.Size(673, 263);
            this.gbHeader.TabIndex = 7;
            // 
            // gcPoliza
            // 
            this.gcPoliza.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcPoliza.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcPoliza.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcPoliza.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcPoliza.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcPoliza.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcPoliza.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcPoliza.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcPoliza.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcPoliza.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcPoliza.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcPoliza.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcPoliza.Location = new System.Drawing.Point(2, 2);
            this.gcPoliza.LookAndFeel.SkinName = "Dark Side";
            this.gcPoliza.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPoliza.MainView = this.gvPoliza;
            this.gcPoliza.Margin = new System.Windows.Forms.Padding(4);
            this.gcPoliza.Name = "gcPoliza";
            this.gcPoliza.Size = new System.Drawing.Size(669, 259);
            this.gcPoliza.TabIndex = 2;
            this.gcPoliza.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPoliza});
            // 
            // gvPoliza
            // 
            this.gvPoliza.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPoliza.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvPoliza.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvPoliza.Appearance.Empty.Options.UseBackColor = true;
            this.gvPoliza.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvPoliza.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvPoliza.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPoliza.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvPoliza.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvPoliza.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvPoliza.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPoliza.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvPoliza.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvPoliza.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvPoliza.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvPoliza.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvPoliza.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvPoliza.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvPoliza.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPoliza.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvPoliza.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvPoliza.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvPoliza.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPoliza.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvPoliza.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvPoliza.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPoliza.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvPoliza.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPoliza.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvPoliza.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvPoliza.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvPoliza.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvPoliza.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvPoliza.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvPoliza.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvPoliza.Appearance.Row.Options.UseBackColor = true;
            this.gvPoliza.Appearance.Row.Options.UseForeColor = true;
            this.gvPoliza.Appearance.Row.Options.UseTextOptions = true;
            this.gvPoliza.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvPoliza.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvPoliza.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvPoliza.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvPoliza.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvPoliza.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvPoliza.Appearance.VertLine.Options.UseBackColor = true;
            this.gvPoliza.GridControl = this.gcPoliza;
            this.gvPoliza.Name = "gvPoliza";
            this.gvPoliza.OptionsBehavior.Editable = false;
            this.gvPoliza.OptionsCustomization.AllowColumnMoving = false;
            this.gvPoliza.OptionsCustomization.AllowFilter = false;
            this.gvPoliza.OptionsDetail.EnableMasterViewMode = false;
            this.gvPoliza.OptionsMenu.EnableColumnMenu = false;
            this.gvPoliza.OptionsMenu.EnableFooterMenu = false;
            this.gvPoliza.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPoliza.OptionsView.ShowGroupPanel = false;
            this.gvPoliza.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvPoliza_RowClick);
            this.gvPoliza.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvPoliza_FocusedRowChanged);
            this.gvPoliza.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvPoliza_CustomUnboundColumnData);
            this.gvPoliza.DoubleClick += new System.EventHandler(this.gvPoliza_DoubleClick);
            // 
            // pgGrid
            // 
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(123, 263);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(461, 24);
            this.pgGrid.TabIndex = 9;
            // 
            // lblLibranza
            // 
            this.lblLibranza.BackColor = System.Drawing.Color.Transparent;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblLibranza.Location = new System.Drawing.Point(428, 21);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(78, 18);
            this.lblLibranza.TabIndex = 43;
            this.lblLibranza.Text = "1047_lblLibranza";
            this.lblLibranza.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtLibranza
            // 
            this.txtLibranza.Location = new System.Drawing.Point(498, 18);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(67, 21);
            this.txtLibranza.TabIndex = 45;
            this.txtLibranza.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.txtLibranza.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLibranza_KeyPress);
            // 
            // btnFilter
            // 
            this.btnFilter.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnFilter.Appearance.Options.UseFont = true;
            this.btnFilter.Location = new System.Drawing.Point(583, 17);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(48, 22);
            this.btnFilter.TabIndex = 8;
            this.btnFilter.Text = "Filtrar";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(123, 16);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(305, 23);
            this.masterTercero.TabIndex = 34;
            this.masterTercero.Value = "";
            this.masterTercero.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // btnAccept
            // 
            this.btnAccept.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnAccept.Appearance.Options.UseFont = true;
            this.btnAccept.Location = new System.Drawing.Point(492, 340);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(95, 23);
            this.btnAccept.TabIndex = 13;
            this.btnAccept.Text = "Aceptar";
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // lblPoliza
            // 
            this.lblPoliza.BackColor = System.Drawing.Color.Transparent;
            this.lblPoliza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoliza.Location = new System.Drawing.Point(3, 18);
            this.lblPoliza.Name = "lblPoliza";
            this.lblPoliza.Size = new System.Drawing.Size(83, 18);
            this.lblPoliza.TabIndex = 46;
            this.lblPoliza.Text = "1047_lblPoliza";
            this.lblPoliza.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPoliza
            // 
            this.txtPoliza.Location = new System.Drawing.Point(65, 17);
            this.txtPoliza.Name = "txtPoliza";
            this.txtPoliza.Size = new System.Drawing.Size(65, 21);
            this.txtPoliza.TabIndex = 47;
            this.txtPoliza.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(593, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Filtro:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblTercero);
            this.panelControl1.Controls.Add(this.txtLibranza);
            this.panelControl1.Controls.Add(this.txtPoliza);
            this.panelControl1.Controls.Add(this.masterTercero);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.btnFilter);
            this.panelControl1.Controls.Add(this.lblPoliza);
            this.panelControl1.Controls.Add(this.lblLibranza);
            this.panelControl1.Location = new System.Drawing.Point(4, 288);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(671, 45);
            this.panelControl1.TabIndex = 52;
            // 
            // lblTercero
            // 
            this.lblTercero.BackColor = System.Drawing.Color.Transparent;
            this.lblTercero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTercero.Location = new System.Drawing.Point(131, 20);
            this.lblTercero.Name = "lblTercero";
            this.lblTercero.Size = new System.Drawing.Size(88, 18);
            this.lblTercero.TabIndex = 53;
            this.lblTercero.Text = "1047_lblTercero";
            this.lblTercero.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModalPolizasCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(689, 367);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.gbHeader);
            this.Controls.Add(this.pgGrid);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(887, 424);
            this.MinimizeBox = false;
            this.Name = "ModalPolizasCartera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1047";
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbHeader)).EndInit();
            this.gbHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcPoliza)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPoliza)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.GroupControl gbHeader;
        private ControlsUC.uc_Pagging pgGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnMvto;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnDetail;
        private ControlsUC.uc_MasterFind masterTercero;
        private DevExpress.XtraEditors.SimpleButton btnFilter;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Label lblLibranza;
        private System.Windows.Forms.TextBox txtLibranza;
        private DevExpress.XtraGrid.GridControl gcPoliza;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPoliza;
        private DevExpress.XtraEditors.SimpleButton btnAccept;
        private System.Windows.Forms.Label lblPoliza;
        private System.Windows.Forms.TextBox txtPoliza;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblTercero;

    }
}