namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalConsultaInventario
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
            this.groupAnticipo = new DevExpress.XtraEditors.GroupControl();
            this.masterBodega = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.pnlParam = new DevExpress.XtraEditors.PanelControl();
            this.chkSerializado = new DevExpress.XtraEditors.CheckEdit();
            this.chkEstadoInv = new DevExpress.XtraEditors.CheckEdit();
            this.chkParam2 = new DevExpress.XtraEditors.CheckEdit();
            this.chkParam1 = new DevExpress.XtraEditors.CheckEdit();
            this.masterReferencia = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblTitle = new System.Windows.Forms.Label();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.RepositoryControl = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editCant2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editLink = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupAnticipo)).BeginInit();
            this.groupAnticipo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlParam)).BeginInit();
            this.pnlParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSerializado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEstadoInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkParam2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkParam1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.SuspendLayout();
            // 
            // groupAnticipo
            // 
            this.groupAnticipo.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupAnticipo.Appearance.Options.UseBackColor = true;
            this.groupAnticipo.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupAnticipo.AppearanceCaption.Options.UseFont = true;
            this.groupAnticipo.AppearanceCaption.Options.UseTextOptions = true;
            this.groupAnticipo.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupAnticipo.Controls.Add(this.masterBodega);
            this.groupAnticipo.Controls.Add(this.txtDescripcion);
            this.groupAnticipo.Controls.Add(this.pnlParam);
            this.groupAnticipo.Controls.Add(this.masterReferencia);
            this.groupAnticipo.Controls.Add(this.lblTitle);
            this.groupAnticipo.Controls.Add(this.gcData);
            this.groupAnticipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupAnticipo.Location = new System.Drawing.Point(0, 0);
            this.groupAnticipo.LookAndFeel.SkinName = "iMaginary";
            this.groupAnticipo.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupAnticipo.Name = "groupAnticipo";
            this.groupAnticipo.Size = new System.Drawing.Size(901, 332);
            this.groupAnticipo.TabIndex = 0;
            // 
            // masterBodega
            // 
            this.masterBodega.BackColor = System.Drawing.Color.Transparent;
            this.masterBodega.Filtros = null;
            this.masterBodega.Location = new System.Drawing.Point(26, 38);
            this.masterBodega.Name = "masterBodega";
            this.masterBodega.Size = new System.Drawing.Size(291, 25);
            this.masterBodega.TabIndex = 82;
            this.masterBodega.Value = "";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(226, 64);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ReadOnly = true;
            this.txtDescripcion.Size = new System.Drawing.Size(353, 45);
            this.txtDescripcion.TabIndex = 81;
            // 
            // pnlParam
            // 
            this.pnlParam.Controls.Add(this.chkSerializado);
            this.pnlParam.Controls.Add(this.chkEstadoInv);
            this.pnlParam.Controls.Add(this.chkParam2);
            this.pnlParam.Controls.Add(this.chkParam1);
            this.pnlParam.Location = new System.Drawing.Point(585, 63);
            this.pnlParam.Name = "pnlParam";
            this.pnlParam.Size = new System.Drawing.Size(303, 46);
            this.pnlParam.TabIndex = 80;
            this.pnlParam.Visible = false;
            // 
            // chkSerializado
            // 
            this.chkSerializado.Enabled = false;
            this.chkSerializado.Location = new System.Drawing.Point(152, 23);
            this.chkSerializado.Name = "chkSerializado";
            this.chkSerializado.Properties.AllowFocused = false;
            this.chkSerializado.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkSerializado.Properties.Appearance.Options.UseFont = true;
            this.chkSerializado.Properties.Caption = "26311_chkSerializada";
            this.chkSerializado.Properties.ReadOnly = true;
            this.chkSerializado.Size = new System.Drawing.Size(138, 19);
            this.chkSerializado.TabIndex = 3;
            // 
            // chkEstadoInv
            // 
            this.chkEstadoInv.Enabled = false;
            this.chkEstadoInv.Location = new System.Drawing.Point(152, 5);
            this.chkEstadoInv.Name = "chkEstadoInv";
            this.chkEstadoInv.Properties.AllowFocused = false;
            this.chkEstadoInv.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkEstadoInv.Properties.Appearance.Options.UseFont = true;
            this.chkEstadoInv.Properties.Caption = "26311_chkEstadoInv";
            this.chkEstadoInv.Properties.ReadOnly = true;
            this.chkEstadoInv.Size = new System.Drawing.Size(138, 19);
            this.chkEstadoInv.TabIndex = 2;
            // 
            // chkParam2
            // 
            this.chkParam2.Enabled = false;
            this.chkParam2.Location = new System.Drawing.Point(10, 23);
            this.chkParam2.Name = "chkParam2";
            this.chkParam2.Properties.AllowFocused = false;
            this.chkParam2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkParam2.Properties.Appearance.Options.UseFont = true;
            this.chkParam2.Properties.Caption = "26311_chkParametro2";
            this.chkParam2.Properties.ReadOnly = true;
            this.chkParam2.Size = new System.Drawing.Size(137, 19);
            this.chkParam2.TabIndex = 1;
            // 
            // chkParam1
            // 
            this.chkParam1.Enabled = false;
            this.chkParam1.Location = new System.Drawing.Point(10, 5);
            this.chkParam1.Name = "chkParam1";
            this.chkParam1.Properties.AllowFocused = false;
            this.chkParam1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkParam1.Properties.Appearance.Options.UseFont = true;
            this.chkParam1.Properties.Caption = "26311_chkParametro1";
            this.chkParam1.Properties.ReadOnly = true;
            this.chkParam1.Size = new System.Drawing.Size(137, 19);
            this.chkParam1.TabIndex = 0;
            // 
            // masterReferencia
            // 
            this.masterReferencia.BackColor = System.Drawing.Color.Transparent;
            this.masterReferencia.Filtros = null;
            this.masterReferencia.Location = new System.Drawing.Point(26, 62);
            this.masterReferencia.Name = "masterReferencia";
            this.masterReferencia.Size = new System.Drawing.Size(291, 25);
            this.masterReferencia.TabIndex = 79;
            this.masterReferencia.Value = "";
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(8, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(888, 18);
            this.lblTitle.TabIndex = 77;
            this.lblTitle.Text = "26311";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gcData
            // 
            this.gcData.AllowDrop = true;
            this.gcData.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcData.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gcData.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcData.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.gcData.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcData.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcData.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.gcData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcData.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcData.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcData.Location = new System.Drawing.Point(12, 113);
            this.gcData.LookAndFeel.SkinName = "Dark Side";
            this.gcData.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcData.MainView = this.gvData;
            this.gcData.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(876, 208);
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
            this.gvData.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gvData.OptionsDetail.EnableMasterViewMode = false;
            this.gvData.OptionsFind.AllowFindPanel = false;
            this.gvData.OptionsMenu.EnableColumnMenu = false;
            this.gvData.OptionsMenu.EnableFooterMenu = false;
            this.gvData.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvData.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvData.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvData.OptionsView.ShowGroupPanel = false;
            this.gvData.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvData.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvData_CustomUnboundColumnData);
            // 
            // cardView1
            // 
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.gcData;
            this.cardView1.Name = "cardView1";
            // 
            // RepositoryControl
            // 
            this.RepositoryControl.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editSpin,
            this.editCant2,
            this.editLink});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editCant2
            // 
            this.editCant2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editCant2.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editCant2.Mask.EditMask = "n2";
            this.editCant2.Mask.UseMaskAsDisplayFormat = true;
            this.editCant2.Name = "editCant2";
            // 
            // editLink
            // 
            this.editLink.Name = "editLink";
            this.editLink.SingleClick = true;
            this.editLink.Click += new System.EventHandler(this.editLink_Click);
            // 
            // ModalConsultaInventario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 332);
            this.Controls.Add(this.groupAnticipo);
            this.MaximizeBox = false;
            this.Name = "ModalConsultaInventario";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupAnticipo)).EndInit();
            this.groupAnticipo.ResumeLayout(false);
            this.groupAnticipo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlParam)).EndInit();
            this.pnlParam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkSerializado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEstadoInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkParam2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkParam1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        internal DevExpress.XtraEditors.GroupControl groupAnticipo;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryControl;
        private System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editCant2;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit editLink;
        private ControlsUC.uc_MasterFind masterReferencia;
        private DevExpress.XtraEditors.PanelControl pnlParam;
        private DevExpress.XtraEditors.CheckEdit chkSerializado;
        private DevExpress.XtraEditors.CheckEdit chkEstadoInv;
        private DevExpress.XtraEditors.CheckEdit chkParam2;
        private DevExpress.XtraEditors.CheckEdit chkParam1;
        private System.Windows.Forms.TextBox txtDescripcion;
        private ControlsUC.uc_MasterFind masterBodega;
    }
}