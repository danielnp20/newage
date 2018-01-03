namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalSaldosCartera
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
            this.groupAnticipo = new DevExpress.XtraEditors.GroupControl();
            this.label1 = new System.Windows.Forms.Label();
            this.gcPLanPagos = new DevExpress.XtraGrid.GridControl();
            this.gvPlanPagos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.gcComponentes = new DevExpress.XtraGrid.GridControl();
            this.gvComponentes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView2 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.RepositoryControl = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupAnticipo)).BeginInit();
            this.groupAnticipo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcPLanPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPlanPagos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcComponentes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvComponentes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
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
            this.groupAnticipo.Controls.Add(this.label1);
            this.groupAnticipo.Controls.Add(this.gcPLanPagos);
            this.groupAnticipo.Controls.Add(this.gcComponentes);
            this.groupAnticipo.Controls.Add(this.lblTitle);
            this.groupAnticipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupAnticipo.Location = new System.Drawing.Point(0, 0);
            this.groupAnticipo.LookAndFeel.SkinName = "iMaginary";
            this.groupAnticipo.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupAnticipo.Name = "groupAnticipo";
            this.groupAnticipo.Size = new System.Drawing.Size(713, 420);
            this.groupAnticipo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(705, 18);
            this.label1.TabIndex = 80;
            this.label1.Text = "1028_Componentes";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gcPLanPagos
            // 
            this.gcPLanPagos.AllowDrop = true;
            this.gcPLanPagos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcPLanPagos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gcPLanPagos.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcPLanPagos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.gcPLanPagos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcPLanPagos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcPLanPagos.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.gcPLanPagos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcPLanPagos.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcPLanPagos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcPLanPagos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcPLanPagos.Location = new System.Drawing.Point(48, 26);
            this.gcPLanPagos.LookAndFeel.SkinName = "Dark Side";
            this.gcPLanPagos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcPLanPagos.MainView = this.gvPlanPagos;
            this.gcPLanPagos.Margin = new System.Windows.Forms.Padding(4);
            this.gcPLanPagos.Name = "gcPLanPagos";
            this.gcPLanPagos.Size = new System.Drawing.Size(603, 202);
            this.gcPLanPagos.TabIndex = 79;
            this.gcPLanPagos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPlanPagos,
            this.cardView1});
            // 
            // gvPlanPagos
            // 
            this.gvPlanPagos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPlanPagos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvPlanPagos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvPlanPagos.Appearance.Empty.Options.UseBackColor = true;
            this.gvPlanPagos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvPlanPagos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvPlanPagos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvPlanPagos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvPlanPagos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvPlanPagos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvPlanPagos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvPlanPagos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvPlanPagos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvPlanPagos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvPlanPagos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPlanPagos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvPlanPagos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvPlanPagos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvPlanPagos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPlanPagos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvPlanPagos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvPlanPagos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPlanPagos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvPlanPagos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvPlanPagos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvPlanPagos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvPlanPagos.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvPlanPagos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvPlanPagos.Appearance.Row.Options.UseBackColor = true;
            this.gvPlanPagos.Appearance.Row.Options.UseFont = true;
            this.gvPlanPagos.Appearance.Row.Options.UseForeColor = true;
            this.gvPlanPagos.Appearance.Row.Options.UseTextOptions = true;
            this.gvPlanPagos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvPlanPagos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvPlanPagos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvPlanPagos.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvPlanPagos.GridControl = this.gcPLanPagos;
            this.gvPlanPagos.HorzScrollStep = 50;
            this.gvPlanPagos.Name = "gvPlanPagos";
            this.gvPlanPagos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvPlanPagos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvPlanPagos.OptionsBehavior.Editable = false;
            this.gvPlanPagos.OptionsCustomization.AllowColumnMoving = false;
            this.gvPlanPagos.OptionsCustomization.AllowFilter = false;
            this.gvPlanPagos.OptionsCustomization.AllowSort = false;
            this.gvPlanPagos.OptionsFind.AllowFindPanel = false;
            this.gvPlanPagos.OptionsMenu.EnableColumnMenu = false;
            this.gvPlanPagos.OptionsMenu.EnableFooterMenu = false;
            this.gvPlanPagos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPlanPagos.OptionsView.ColumnAutoWidth = false;
            this.gvPlanPagos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvPlanPagos.OptionsView.ShowGroupPanel = false;
            this.gvPlanPagos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvPlanPagos.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocument_CustomRowCellEdit);
            this.gvPlanPagos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvPlanPagos_FocusedRowChanged);
            this.gvPlanPagos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // cardView1
            // 
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.gcPLanPagos;
            this.cardView1.Name = "cardView1";
            // 
            // gcComponentes
            // 
            this.gcComponentes.AllowDrop = true;
            this.gcComponentes.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcComponentes.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gcComponentes.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcComponentes.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.gcComponentes.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcComponentes.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcComponentes.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.gcComponentes.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcComponentes.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcComponentes.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcComponentes.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcComponentes.Location = new System.Drawing.Point(48, 269);
            this.gcComponentes.LookAndFeel.SkinName = "Dark Side";
            this.gcComponentes.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcComponentes.MainView = this.gvComponentes;
            this.gcComponentes.Margin = new System.Windows.Forms.Padding(4);
            this.gcComponentes.Name = "gcComponentes";
            this.gcComponentes.Size = new System.Drawing.Size(603, 138);
            this.gcComponentes.TabIndex = 78;
            this.gcComponentes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvComponentes,
            this.cardView2});
            // 
            // gvComponentes
            // 
            this.gvComponentes.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvComponentes.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvComponentes.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvComponentes.Appearance.Empty.Options.UseBackColor = true;
            this.gvComponentes.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvComponentes.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvComponentes.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvComponentes.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvComponentes.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvComponentes.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvComponentes.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvComponentes.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvComponentes.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvComponentes.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvComponentes.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvComponentes.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvComponentes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvComponentes.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvComponentes.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvComponentes.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvComponentes.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvComponentes.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvComponentes.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvComponentes.Appearance.Row.Options.UseBackColor = true;
            this.gvComponentes.Appearance.Row.Options.UseFont = true;
            this.gvComponentes.Appearance.Row.Options.UseForeColor = true;
            this.gvComponentes.Appearance.Row.Options.UseTextOptions = true;
            this.gvComponentes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvComponentes.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvComponentes.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvComponentes.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvComponentes.GridControl = this.gcComponentes;
            this.gvComponentes.HorzScrollStep = 50;
            this.gvComponentes.Name = "gvComponentes";
            this.gvComponentes.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvComponentes.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvComponentes.OptionsBehavior.Editable = false;
            this.gvComponentes.OptionsCustomization.AllowColumnMoving = false;
            this.gvComponentes.OptionsCustomization.AllowFilter = false;
            this.gvComponentes.OptionsCustomization.AllowSort = false;
            this.gvComponentes.OptionsFind.AllowFindPanel = false;
            this.gvComponentes.OptionsMenu.EnableColumnMenu = false;
            this.gvComponentes.OptionsMenu.EnableFooterMenu = false;
            this.gvComponentes.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvComponentes.OptionsView.ColumnAutoWidth = false;
            this.gvComponentes.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvComponentes.OptionsView.ShowGroupPanel = false;
            this.gvComponentes.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvComponentes.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocument_CustomRowCellEdit);
            this.gvComponentes.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // cardView2
            // 
            this.cardView2.FocusedCardTopFieldIndex = 0;
            this.cardView2.GridControl = this.gcComponentes;
            this.cardView2.Name = "cardView2";
            // 
            // lblPlanPagos
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(8, 4);
            this.lblTitle.Name = "lblPlanPagos";
            this.lblTitle.Size = new System.Drawing.Size(700, 18);
            this.lblTitle.TabIndex = 77;
            this.lblTitle.Text = "1028_Title";
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
            // ModalSaldosCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 420);
            this.Controls.Add(this.groupAnticipo);
            this.MaximizeBox = false;
            this.Name = "ModalSaldosCartera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupAnticipo)).EndInit();
            this.groupAnticipo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcPLanPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPlanPagos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcComponentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvComponentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraEditors.GroupControl groupAnticipo;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryControl;
        public System.Windows.Forms.Label lblTitle;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        private DevExpress.XtraGrid.GridControl gcComponentes;
        private DevExpress.XtraGrid.Views.Grid.GridView gvComponentes;
        private DevExpress.XtraGrid.Views.Card.CardView cardView2;
        private DevExpress.XtraGrid.GridControl gcPLanPagos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPlanPagos;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private System.Windows.Forms.Label label1;
    }
}