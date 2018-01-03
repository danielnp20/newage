namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AnexosDocumentos
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
            this.groupMaster = new DevExpress.XtraEditors.GroupControl();
            this.groupFind = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.gcAnexos = new DevExpress.XtraGrid.GridControl();
            this.gvAnexos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.lblpgGrid = new System.Windows.Forms.Label();
            this.RepositoryDocuments = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.linkVer = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.linkActualizar = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupMaster)).BeginInit();
            this.groupMaster.SuspendLayout();
            this.groupFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcAnexos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAnexos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkVer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkActualizar)).BeginInit();
            this.SuspendLayout();
            // 
            // groupMaster
            // 
            this.groupMaster.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupMaster.Appearance.Options.UseBackColor = true;
            this.groupMaster.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupMaster.AppearanceCaption.Options.UseFont = true;
            this.groupMaster.AppearanceCaption.Options.UseTextOptions = true;
            this.groupMaster.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupMaster.Controls.Add(this.groupFind);
            this.groupMaster.Controls.Add(this.lblTitle);
            this.groupMaster.Controls.Add(this.gcAnexos);
            this.groupMaster.Controls.Add(this.lblpgGrid);
            this.groupMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMaster.Location = new System.Drawing.Point(0, 0);
            this.groupMaster.LookAndFeel.SkinName = "iMaginary";
            this.groupMaster.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupMaster.Name = "groupMaster";
            this.groupMaster.Size = new System.Drawing.Size(615, 364);
            this.groupMaster.TabIndex = 0;
            // 
            // groupFind
            // 
            this.groupFind.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupFind.Controls.Add(this.btnSave);
            this.groupFind.Location = new System.Drawing.Point(25, 303);
            this.groupFind.Name = "groupFind";
            this.groupFind.Size = new System.Drawing.Size(578, 46);
            this.groupFind.TabIndex = 4;
            this.groupFind.TabStop = false;
            this.groupFind.Text = "1033_Opciones";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(459, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "1033_btnSalvar";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(26, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(499, 18);
            this.lblTitle.TabIndex = 77;
            this.lblTitle.Text = "1033_lblTitulo";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gcAnexos
            // 
            this.gcAnexos.AllowDrop = true;
            this.gcAnexos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcAnexos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcAnexos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gcAnexos.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.gcAnexos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.gcAnexos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcAnexos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcAnexos.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.gcAnexos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcAnexos.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcAnexos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcAnexos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcAnexos.Location = new System.Drawing.Point(13, 30);
            this.gcAnexos.LookAndFeel.SkinName = "Dark Side";
            this.gcAnexos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcAnexos.MainView = this.gvAnexos;
            this.gcAnexos.Margin = new System.Windows.Forms.Padding(4);
            this.gcAnexos.Name = "gcAnexos";
            this.gcAnexos.Size = new System.Drawing.Size(596, 266);
            this.gcAnexos.TabIndex = 0;
            this.gcAnexos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvAnexos,
            this.cardView1});
            // 
            // gvAnexos
            // 
            this.gvAnexos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvAnexos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvAnexos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvAnexos.Appearance.Empty.Options.UseBackColor = true;
            this.gvAnexos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvAnexos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvAnexos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvAnexos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvAnexos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvAnexos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvAnexos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvAnexos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvAnexos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvAnexos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvAnexos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvAnexos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvAnexos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvAnexos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvAnexos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvAnexos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvAnexos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvAnexos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvAnexos.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvAnexos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvAnexos.Appearance.Row.Options.UseBackColor = true;
            this.gvAnexos.Appearance.Row.Options.UseFont = true;
            this.gvAnexos.Appearance.Row.Options.UseForeColor = true;
            this.gvAnexos.Appearance.Row.Options.UseTextOptions = true;
            this.gvAnexos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvAnexos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvAnexos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvAnexos.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvAnexos.GridControl = this.gcAnexos;
            this.gvAnexos.HorzScrollStep = 50;
            this.gvAnexos.Name = "gvAnexos";
            this.gvAnexos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvAnexos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvAnexos.OptionsCustomization.AllowColumnMoving = false;
            this.gvAnexos.OptionsCustomization.AllowFilter = false;
            this.gvAnexos.OptionsCustomization.AllowSort = false;
            this.gvAnexos.OptionsFind.AllowFindPanel = false;
            this.gvAnexos.OptionsMenu.EnableColumnMenu = false;
            this.gvAnexos.OptionsMenu.EnableFooterMenu = false;
            this.gvAnexos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvAnexos.OptionsView.ColumnAutoWidth = false;
            this.gvAnexos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvAnexos.OptionsView.ShowGroupPanel = false;
            this.gvAnexos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvAnexos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvAnexos_FocusedRowChanged);
            this.gvAnexos.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvAnexos_CellValueChanging);
            this.gvAnexos.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvAnexos_BeforeLeaveRow);
            this.gvAnexos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvAnexos_CustomUnboundColumnData);
            this.gvAnexos.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvAnexos_CustomColumnDisplayText);
            // 
            // cardView1
            // 
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.gcAnexos;
            this.cardView1.Name = "cardView1";
            // 
            // lblpgGrid
            // 
            this.lblpgGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblpgGrid.Location = new System.Drawing.Point(26, 268);
            this.lblpgGrid.Name = "lblpgGrid";
            this.lblpgGrid.Size = new System.Drawing.Size(155, 34);
            this.lblpgGrid.TabIndex = 53;
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.linkVer,
            this.linkActualizar});
            // 
            // linkVer
            // 
            this.linkVer.Name = "linkVer";
            this.linkVer.SingleClick = true;
            this.linkVer.Click += new System.EventHandler(this.linkVer_Click);
            // 
            // linkActualizar
            // 
            this.linkActualizar.Name = "linkActualizar";
            this.linkActualizar.SingleClick = true;
            this.linkActualizar.Click += new System.EventHandler(this.linkActualizar_Click);
            // 
            // AnexosDocumentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 364);
            this.Controls.Add(this.groupMaster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AnexosDocumentos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupMaster)).EndInit();
            this.groupMaster.ResumeLayout(false);
            this.groupFind.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcAnexos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvAnexos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkVer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkActualizar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcAnexos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvAnexos;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupFind;
        internal DevExpress.XtraEditors.GroupControl groupMaster;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblpgGrid;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryDocuments;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkVer;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit linkActualizar;

    }
}