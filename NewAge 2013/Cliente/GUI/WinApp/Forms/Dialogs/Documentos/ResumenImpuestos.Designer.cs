namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ResumenImpuestos
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
            this.groupImpustos = new DevExpress.XtraEditors.GroupControl();
            this.txtRegimen = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.masterDepto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.label = new System.Windows.Forms.Label();
            this.masterPais = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCiudad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterDocTipo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnlImpuestos = new DevExpress.XtraEditors.PanelControl();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAprobar = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.RepositoryControl = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpinPorc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.toolTipGrid = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupImpustos)).BeginInit();
            this.groupImpustos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlImpuestos)).BeginInit();
            this.pnlImpuestos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            this.SuspendLayout();
            // 
            // groupImpustos
            // 
            this.groupImpustos.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupImpustos.Appearance.Options.UseBackColor = true;
            this.groupImpustos.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupImpustos.AppearanceCaption.Options.UseFont = true;
            this.groupImpustos.AppearanceCaption.Options.UseTextOptions = true;
            this.groupImpustos.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupImpustos.Controls.Add(this.txtRegimen);
            this.groupImpustos.Controls.Add(this.label2);
            this.groupImpustos.Controls.Add(this.label1);
            this.groupImpustos.Controls.Add(this.masterDepto);
            this.groupImpustos.Controls.Add(this.label);
            this.groupImpustos.Controls.Add(this.masterPais);
            this.groupImpustos.Controls.Add(this.masterCiudad);
            this.groupImpustos.Controls.Add(this.masterDocTipo);
            this.groupImpustos.Controls.Add(this.masterTercero);
            this.groupImpustos.Controls.Add(this.pnlImpuestos);
            this.groupImpustos.Controls.Add(this.btnCancelar);
            this.groupImpustos.Controls.Add(this.btnAprobar);
            this.groupImpustos.Controls.Add(this.lblTitle);
            this.groupImpustos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupImpustos.Location = new System.Drawing.Point(0, 0);
            this.groupImpustos.LookAndFeel.SkinName = "iMaginary";
            this.groupImpustos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupImpustos.Name = "groupImpustos";
            this.groupImpustos.Size = new System.Drawing.Size(693, 485);
            this.groupImpustos.TabIndex = 0;
            // 
            // txtRegimen
            // 
            this.txtRegimen.Location = new System.Drawing.Point(458, 87);
            this.txtRegimen.Name = "txtRegimen";
            this.txtRegimen.ReadOnly = true;
            this.txtRegimen.Size = new System.Drawing.Size(100, 20);
            this.txtRegimen.TabIndex = 89;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(349, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 14);
            this.label2.TabIndex = 88;
            this.label2.Text = "1019_lblRegimen";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(349, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 14);
            this.label1.TabIndex = 87;
            this.label1.Text = "1019_lblDpto";
            // 
            // masterDepto
            // 
            this.masterDepto.BackColor = System.Drawing.Color.Transparent;
            this.masterDepto.Filtros = null;
            this.masterDepto.Location = new System.Drawing.Point(357, 120);
            this.masterDepto.Name = "masterDepto";
            this.masterDepto.Size = new System.Drawing.Size(291, 25);
            this.masterDepto.TabIndex = 86;
            this.masterDepto.Value = "";
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.Location = new System.Drawing.Point(26, 125);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(89, 14);
            this.label.TabIndex = 85;
            this.label.Text = "1019_lblCiudad";
            // 
            // masterPais
            // 
            this.masterPais.BackColor = System.Drawing.Color.Transparent;
            this.masterPais.Filtros = null;
            this.masterPais.Location = new System.Drawing.Point(29, 156);
            this.masterPais.Name = "masterPais";
            this.masterPais.Size = new System.Drawing.Size(291, 25);
            this.masterPais.TabIndex = 84;
            this.masterPais.Value = "";
            // 
            // masterCiudad
            // 
            this.masterCiudad.BackColor = System.Drawing.Color.Transparent;
            this.masterCiudad.Filtros = null;
            this.masterCiudad.Location = new System.Drawing.Point(29, 120);
            this.masterCiudad.Name = "masterCiudad";
            this.masterCiudad.Size = new System.Drawing.Size(291, 25);
            this.masterCiudad.TabIndex = 83;
            this.masterCiudad.Value = "";
            // 
            // masterDocTipo
            // 
            this.masterDocTipo.BackColor = System.Drawing.Color.Transparent;
            this.masterDocTipo.Filtros = null;
            this.masterDocTipo.Location = new System.Drawing.Point(29, 82);
            this.masterDocTipo.Name = "masterDocTipo";
            this.masterDocTipo.Size = new System.Drawing.Size(291, 25);
            this.masterDocTipo.TabIndex = 82;
            this.masterDocTipo.Value = "";
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(29, 45);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(291, 25);
            this.masterTercero.TabIndex = 81;
            this.masterTercero.Value = "";
            // 
            // pnlImpuestos
            // 
            this.pnlImpuestos.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlImpuestos.Appearance.Options.UseBackColor = true;
            this.pnlImpuestos.Controls.Add(this.gcData);
            this.pnlImpuestos.Location = new System.Drawing.Point(27, 198);
            this.pnlImpuestos.Name = "pnlImpuestos";
            this.pnlImpuestos.Size = new System.Drawing.Size(647, 237);
            this.pnlImpuestos.TabIndex = 80;
            // 
            // gcData
            // 
            this.gcData.AllowDrop = true;
            this.gcData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcData.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcData.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gcData.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcData.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.gcData.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcData.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcData.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.gcData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcData.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcData.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcData.Location = new System.Drawing.Point(2, 2);
            this.gcData.LookAndFeel.SkinName = "Dark Side";
            this.gcData.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcData.MainView = this.gvData;
            this.gcData.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(643, 233);
            this.gcData.TabIndex = 0;
            this.gcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData,
            this.cardView1});
            // 
            // gvData
            // 
            this.gvData.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvData.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvData.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvData.Appearance.Empty.Options.UseBackColor = true;
            this.gvData.Appearance.FocusedCell.BackColor = System.Drawing.Color.Lavender;
            this.gvData.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvData.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvData.Appearance.FocusedRow.BackColor = System.Drawing.Color.Lavender;
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
            this.gvData.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Lavender;
            this.gvData.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvData.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvData.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvData.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.Row.Options.UseBackColor = true;
            this.gvData.Appearance.Row.Options.UseFont = true;
            this.gvData.Appearance.Row.Options.UseForeColor = true;
            this.gvData.Appearance.Row.Options.UseTextOptions = true;
            this.gvData.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvData.Appearance.SelectedRow.BackColor = System.Drawing.Color.Lavender;
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
            this.gvData.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvData_RowStyle);
            this.gvData.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvData_CustomRowCellEdit);
            this.gvData.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvData_CustomUnboundColumnData);
            // 
            // cardView1
            // 
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.gcData;
            this.cardView1.Name = "cardView1";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(444, 441);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(187, 32);
            this.btnCancelar.TabIndex = 79;
            this.btnCancelar.Text = "1019_btnCancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAprobar
            // 
            this.btnAprobar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAprobar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAprobar.Location = new System.Drawing.Point(67, 441);
            this.btnAprobar.Name = "btnAprobar";
            this.btnAprobar.Size = new System.Drawing.Size(182, 32);
            this.btnAprobar.TabIndex = 78;
            this.btnAprobar.Text = "1019_btnAprobar";
            this.btnAprobar.UseVisualStyleBackColor = true;
            this.btnAprobar.Click += new System.EventHandler(this.btnAprobar_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(210, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(204, 18);
            this.lblTitle.TabIndex = 77;
            this.lblTitle.Text = "1019_ResumenImpuestos";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RepositoryControl
            // 
            this.RepositoryControl.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editSpin});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P";
            this.editSpinPorc.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorc.Name = "editSpinPorc";
            // 
            // ResumenImpuestos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 485);
            this.Controls.Add(this.groupImpustos);
            this.MaximizeBox = false;
            this.Name = "ResumenImpuestos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupImpustos)).EndInit();
            this.groupImpustos.ResumeLayout(false);
            this.groupImpustos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlImpuestos)).EndInit();
            this.pnlImpuestos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        internal DevExpress.XtraEditors.GroupControl groupImpustos;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryControl;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolTip toolTipGrid;
        private System.Windows.Forms.Button btnAprobar;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorc;
        private System.Windows.Forms.Button btnCancelar;
        private DevExpress.XtraEditors.PanelControl pnlImpuestos;
        private System.Windows.Forms.Label label;
        private ControlsUC.uc_MasterFind masterPais;
        private ControlsUC.uc_MasterFind masterCiudad;
        private ControlsUC.uc_MasterFind masterDocTipo;
        private ControlsUC.uc_MasterFind masterTercero;
        private System.Windows.Forms.TextBox txtRegimen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ControlsUC.uc_MasterFind masterDepto;
    }
}