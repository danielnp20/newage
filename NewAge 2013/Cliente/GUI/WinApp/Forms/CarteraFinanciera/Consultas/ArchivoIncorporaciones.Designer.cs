namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ArchivoIncorporaciones
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
            this.gb_Header = new System.Windows.Forms.GroupBox();
            this.comboTipoIncorp = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.dtFechaIncorpora = new DevExpress.XtraEditors.DateEdit();
            this.uc_MasterCentroPago = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lbl_TipoIncopora = new System.Windows.Forms.Label();
            this.lbl_Periodo = new System.Windows.Forms.Label();
            this.btn_Consultar = new System.Windows.Forms.Button();
            this.persistentRepository1 = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.linkEditViewFile = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.TextEdit = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.linkEditAnular = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocuments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gb_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIncorpora.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIncorpora.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditAnular)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_Header
            // 
            this.gb_Header.Controls.Add(this.comboTipoIncorp);
            this.gb_Header.Controls.Add(this.dtFechaIncorpora);
            this.gb_Header.Controls.Add(this.uc_MasterCentroPago);
            this.gb_Header.Controls.Add(this.lbl_TipoIncopora);
            this.gb_Header.Controls.Add(this.lbl_Periodo);
            this.gb_Header.Location = new System.Drawing.Point(8, 7);
            this.gb_Header.Name = "gb_Header";
            this.gb_Header.Size = new System.Drawing.Size(1002, 71);
            this.gb_Header.TabIndex = 0;
            this.gb_Header.TabStop = false;
            // 
            // comboTipoIncorp
            // 
            this.comboTipoIncorp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTipoIncorp.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboTipoIncorp.FormattingEnabled = true;
            this.comboTipoIncorp.Location = new System.Drawing.Point(174, 30);
            this.comboTipoIncorp.Name = "comboTipoIncorp";
            this.comboTipoIncorp.Size = new System.Drawing.Size(118, 22);
            this.comboTipoIncorp.TabIndex = 35;
            this.comboTipoIncorp.SelectedIndexChanged += new System.EventHandler(this.comboTipoIncorp_SelectedIndexChanged);
            // 
            // dtFechaIncorpora
            // 
            this.dtFechaIncorpora.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaIncorpora.Location = new System.Drawing.Point(486, 30);
            this.dtFechaIncorpora.Name = "dtFechaIncorpora";
            this.dtFechaIncorpora.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaIncorpora.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaIncorpora.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaIncorpora.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaIncorpora.Properties.Appearance.Options.UseFont = true;
            this.dtFechaIncorpora.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIncorpora.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIncorpora.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIncorpora.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIncorpora.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIncorpora.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaIncorpora.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaIncorpora.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIncorpora.Size = new System.Drawing.Size(90, 20);
            this.dtFechaIncorpora.TabIndex = 34;
            // 
            // uc_MasterCentroPago
            // 
            this.uc_MasterCentroPago.BackColor = System.Drawing.Color.Transparent;
            this.uc_MasterCentroPago.Filtros = null;
            this.uc_MasterCentroPago.Location = new System.Drawing.Point(642, 29);
            this.uc_MasterCentroPago.Name = "uc_MasterCentroPago";
            this.uc_MasterCentroPago.Size = new System.Drawing.Size(291, 25);
            this.uc_MasterCentroPago.TabIndex = 2;
            this.uc_MasterCentroPago.Value = "";
            this.uc_MasterCentroPago.Leave += new System.EventHandler(this.uc_MasterCentroPago_Leave);
            // 
            // lbl_TipoIncopora
            // 
            this.lbl_TipoIncopora.AutoSize = true;
            this.lbl_TipoIncopora.Location = new System.Drawing.Point(39, 34);
            this.lbl_TipoIncopora.Name = "lbl_TipoIncopora";
            this.lbl_TipoIncopora.Size = new System.Drawing.Size(106, 13);
            this.lbl_TipoIncopora.TabIndex = 4;
            this.lbl_TipoIncopora.Text = "32314_TipoIncopora";
            // 
            // lbl_Periodo
            // 
            this.lbl_Periodo.AutoSize = true;
            this.lbl_Periodo.Location = new System.Drawing.Point(371, 35);
            this.lbl_Periodo.Name = "lbl_Periodo";
            this.lbl_Periodo.Size = new System.Drawing.Size(95, 13);
            this.lbl_Periodo.TabIndex = 1;
            this.lbl_Periodo.Text = "32314_lbl_Periodo";
            // 
            // btn_Consultar
            // 
            this.btn_Consultar.Location = new System.Drawing.Point(933, 360);
            this.btn_Consultar.Name = "btn_Consultar";
            this.btn_Consultar.Size = new System.Drawing.Size(77, 30);
            this.btn_Consultar.TabIndex = 4;
            this.btn_Consultar.Text = "21310_btn_Consultar";
            this.btn_Consultar.UseVisualStyleBackColor = true;
            this.btn_Consultar.Click += new System.EventHandler(this.btn_Consultar_Click);
            // 
            // persistentRepository1
            // 
            this.persistentRepository1.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.linkEditViewFile,
            this.TextEdit,
            this.linkEditAnular,
            this.editSpin});
            // 
            // linkEditViewFile
            // 
            this.linkEditViewFile.Name = "linkEditViewFile";
            // 
            // TextEdit
            // 
            this.TextEdit.Name = "TextEdit";
            // 
            // linkEditAnular
            // 
            this.linkEditAnular.Name = "linkEditAnular";
            // 
            // editSpin
            // 
            this.editSpin.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c2";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gcDocument);
            this.groupBox1.Location = new System.Drawing.Point(8, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1002, 270);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // gcDocument
            // 
            this.gcDocument.AllowDrop = true;
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocument.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocument.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocument.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocument.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Location = new System.Drawing.Point(3, 16);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocuments;
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(996, 251);
            this.gcDocument.TabIndex = 10;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
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
            this.gvDocuments.GridControl = this.gcDocument;
            this.gvDocuments.Name = "gvDocuments";
            this.gvDocuments.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocuments.OptionsCustomization.AllowFilter = false;
            this.gvDocuments.OptionsCustomization.AllowSort = false;
            this.gvDocuments.OptionsView.ShowGroupPanel = false;
            this.gvDocuments.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // ArchivoIncorporaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 417);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_Consultar);
            this.Controls.Add(this.gb_Header);
            this.Name = "ArchivoIncorporaciones";
            this.Text = "ConsultaDocuementosCxP";
            this.gb_Header.ResumeLayout(false);
            this.gb_Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIncorpora.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIncorpora.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditAnular)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocuments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Header;
        private System.Windows.Forms.Label lbl_TipoIncopora;
        private System.Windows.Forms.Label lbl_Periodo;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_MasterCentroPago;
        private System.Windows.Forms.Button btn_Consultar;
        private DevExpress.XtraEditors.Repository.PersistentRepository persistentRepository1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkEditViewFile;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TextEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit linkEditAnular;
        protected DevExpress.XtraEditors.DateEdit dtFechaIncorpora;
        private Clases.ComboBoxEx comboTipoIncorp;
        private System.Windows.Forms.GroupBox groupBox1;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocuments;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
    }
}