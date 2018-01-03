namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalNotasEnvio
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
            this.master = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.RepositoryControl = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.toolTipGrid = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupAnticipo)).BeginInit();
            this.groupAnticipo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
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
            this.groupAnticipo.Controls.Add(this.master);
            this.groupAnticipo.Controls.Add(this.btnUpdate);
            this.groupAnticipo.Controls.Add(this.lblTitle);
            this.groupAnticipo.Controls.Add(this.gcData);
            this.groupAnticipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupAnticipo.Location = new System.Drawing.Point(0, 0);
            this.groupAnticipo.LookAndFeel.SkinName = "iMaginary";
            this.groupAnticipo.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupAnticipo.Name = "groupAnticipo";
            this.groupAnticipo.Size = new System.Drawing.Size(706, 352);
            this.groupAnticipo.TabIndex = 0;
            // 
            // master
            // 
            this.master.BackColor = System.Drawing.Color.Transparent;
            this.master.Filtros = null;
            this.master.Location = new System.Drawing.Point(15, 29);
            this.master.Name = "master";
            this.master.Size = new System.Drawing.Size(307, 25);
            this.master.TabIndex = 80;
            this.master.Value = "";
            this.master.Visible = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(586, 317);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(107, 30);
            this.btnUpdate.TabIndex = 78;
            this.btnUpdate.Text = "1026_btnUpdate";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(210, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(162, 18);
            this.lblTitle.TabIndex = 77;
            this.lblTitle.Text = "1026_lblNotasEnvio";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gcData
            // 
            this.gcData.AllowDrop = true;
            this.gcData.Cursor = System.Windows.Forms.Cursors.Hand;
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
            this.gcData.Location = new System.Drawing.Point(13, 56);
            this.gcData.LookAndFeel.SkinName = "Dark Side";
            this.gcData.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcData.MainView = this.gvData;
            this.gcData.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(680, 254);
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
            this.gvData.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvData_CustomRowCellEdit);
            this.gvData.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvData_CellValueChanging);
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
            // ModalNotasEnvio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 352);
            this.Controls.Add(this.groupAnticipo);
            this.MaximizeBox = false;
            this.Name = "ModalNotasEnvio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupAnticipo)).EndInit();
            this.groupAnticipo.ResumeLayout(false);
            this.groupAnticipo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        internal DevExpress.XtraEditors.GroupControl groupAnticipo;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryControl;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolTip toolTipGrid;
        private System.Windows.Forms.Button btnUpdate;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        private ControlsUC.uc_MasterFind master;
    }
}