namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalMaster
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
            this.btnSearchHierarchy = new System.Windows.Forms.Button();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.grlcontrolMasterModal = new DevExpress.XtraGrid.GridControl();
            this.gvMasterModal = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.RepositoryControl = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.btnReturn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.toolTipGrid = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.groupMaster)).BeginInit();
            this.groupMaster.SuspendLayout();
            this.groupFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlcontrolMasterModal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMasterModal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).BeginInit();
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
            this.groupMaster.Controls.Add(this.grlcontrolMasterModal);
            this.groupMaster.Controls.Add(this.pgGrid);
            this.groupMaster.Controls.Add(this.label1);
            this.groupMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMaster.Location = new System.Drawing.Point(0, 0);
            this.groupMaster.LookAndFeel.SkinName = "iMaginary";
            this.groupMaster.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupMaster.Name = "groupMaster";
            this.groupMaster.Size = new System.Drawing.Size(518, 363);
            this.groupMaster.TabIndex = 0;
            // 
            // groupFind
            // 
            this.groupFind.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupFind.Controls.Add(this.btnSearchHierarchy);
            this.groupFind.Controls.Add(this.lblCode);
            this.groupFind.Controls.Add(this.txtDescription);
            this.groupFind.Controls.Add(this.btnFind);
            this.groupFind.Controls.Add(this.lblDescription);
            this.groupFind.Controls.Add(this.txtCode);
            this.groupFind.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupFind.Location = new System.Drawing.Point(9, 283);
            this.groupFind.Name = "groupFind";
            this.groupFind.Size = new System.Drawing.Size(500, 75);
            this.groupFind.TabIndex = 4;
            this.groupFind.TabStop = false;
            this.groupFind.Text = "1002_groupFind";
            // 
            // btnSearchHierarchy
            // 
            this.btnSearchHierarchy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchHierarchy.Location = new System.Drawing.Point(333, 48);
            this.btnSearchHierarchy.Name = "btnSearchHierarchy";
            this.btnSearchHierarchy.Size = new System.Drawing.Size(146, 23);
            this.btnSearchHierarchy.TabIndex = 50;
            this.btnSearchHierarchy.Text = "1002_btnSearchHierarchy";
            this.btnSearchHierarchy.UseVisualStyleBackColor = true;
            this.btnSearchHierarchy.Visible = false;
            this.btnSearchHierarchy.Click += new System.EventHandler(this.btnSearchHierarchy_Click);
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lblCode.Location = new System.Drawing.Point(10, 24);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(81, 14);
            this.lblCode.TabIndex = 48;
            this.lblCode.Text = "1002_lblCode";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(239, 21);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(100, 22);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDescription_KeyDown);
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Location = new System.Drawing.Point(376, 21);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(102, 23);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "1002_btnFind";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lblDescription.Location = new System.Drawing.Point(166, 25);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(113, 14);
            this.lblDescription.TabIndex = 49;
            this.lblDescription.Text = "1002_lblDescription";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCode
            // 
            this.txtCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(56, 20);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 22);
            this.txtCode.TabIndex = 0;
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(9, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(499, 18);
            this.lblTitle.TabIndex = 77;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pgGrid
            // 
            this.pgGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(53, 260);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(456, 34);
            this.pgGrid.TabIndex = 2;
            // 
            // grlcontrolMasterModal
            // 
            this.grlcontrolMasterModal.AllowDrop = true;
            this.grlcontrolMasterModal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.grlcontrolMasterModal.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grlcontrolMasterModal.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grlcontrolMasterModal.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.grlcontrolMasterModal.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.grlcontrolMasterModal.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.grlcontrolMasterModal.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.grlcontrolMasterModal.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.grlcontrolMasterModal.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grlcontrolMasterModal.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.grlcontrolMasterModal.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.grlcontrolMasterModal.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grlcontrolMasterModal.Location = new System.Drawing.Point(10, 29);
            this.grlcontrolMasterModal.LookAndFeel.SkinName = "Dark Side";
            this.grlcontrolMasterModal.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grlcontrolMasterModal.MainView = this.gvMasterModal;
            this.grlcontrolMasterModal.Margin = new System.Windows.Forms.Padding(4);
            this.grlcontrolMasterModal.Name = "grlcontrolMasterModal";
            this.grlcontrolMasterModal.Size = new System.Drawing.Size(499, 230);
            this.grlcontrolMasterModal.TabIndex = 0;
            this.grlcontrolMasterModal.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMasterModal,
            this.cardView1});
            this.grlcontrolMasterModal.DoubleClick += new System.EventHandler(this.grlcontrolMasterModal_DoubleClick);
            this.grlcontrolMasterModal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grlcontrolMasterModal_KeyDown);
            // 
            // gvMasterModal
            // 
            this.gvMasterModal.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMasterModal.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvMasterModal.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvMasterModal.Appearance.Empty.Options.UseBackColor = true;
            this.gvMasterModal.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvMasterModal.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvMasterModal.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvMasterModal.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvMasterModal.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvMasterModal.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvMasterModal.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvMasterModal.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvMasterModal.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvMasterModal.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvMasterModal.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMasterModal.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvMasterModal.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvMasterModal.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvMasterModal.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvMasterModal.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvMasterModal.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvMasterModal.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvMasterModal.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvMasterModal.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvMasterModal.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvMasterModal.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvMasterModal.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMasterModal.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvMasterModal.Appearance.Row.Options.UseBackColor = true;
            this.gvMasterModal.Appearance.Row.Options.UseFont = true;
            this.gvMasterModal.Appearance.Row.Options.UseForeColor = true;
            this.gvMasterModal.Appearance.Row.Options.UseTextOptions = true;
            this.gvMasterModal.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvMasterModal.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvMasterModal.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvMasterModal.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvMasterModal.GridControl = this.grlcontrolMasterModal;
            this.gvMasterModal.HorzScrollStep = 50;
            this.gvMasterModal.Name = "gvMasterModal";
            this.gvMasterModal.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvMasterModal.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvMasterModal.OptionsBehavior.Editable = false;
            this.gvMasterModal.OptionsCustomization.AllowColumnMoving = false;
            this.gvMasterModal.OptionsCustomization.AllowFilter = false;
            this.gvMasterModal.OptionsCustomization.AllowSort = false;
            this.gvMasterModal.OptionsFind.AllowFindPanel = false;
            this.gvMasterModal.OptionsMenu.EnableColumnMenu = false;
            this.gvMasterModal.OptionsMenu.EnableFooterMenu = false;
            this.gvMasterModal.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvMasterModal.OptionsView.ColumnAutoWidth = false;
            this.gvMasterModal.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvMasterModal.OptionsView.ShowGroupPanel = false;
            this.gvMasterModal.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvMasterModal.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvMasterModal_FocusedRowChanged);
            this.gvMasterModal.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvMasterModal_CustomUnboundColumnData);
            this.gvMasterModal.Click += new System.EventHandler(this.gvMasterModal_Click);
            // 
            // cardView1
            // 
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.grlcontrolMasterModal;
            this.cardView1.Name = "cardView1";
            // 
            // RepositoryControl
            // 
            this.RepositoryControl.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.btnReturn});
            // 
            // btnReturn
            // 
            this.btnReturn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.OK)});
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.DoubleClick += new System.EventHandler(this.btnReturn_Click);
            this.btnReturn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnReturn_KeyDown);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(10, 263);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 28);
            this.label1.TabIndex = 78;
            // 
            // ModalMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 363);
            this.Controls.Add(this.groupMaster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ModalMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupMaster)).EndInit();
            this.groupMaster.ResumeLayout(false);
            this.groupFind.ResumeLayout(false);
            this.groupFind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlcontrolMasterModal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMasterModal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescription;
        private DevExpress.XtraGrid.GridControl grlcontrolMasterModal;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMasterModal;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.GroupBox groupFind;
        internal DevExpress.XtraEditors.GroupControl groupMaster;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryControl;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnReturn;
        private ControlsUC.uc_Pagging pgGrid;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolTip toolTipGrid;
        private System.Windows.Forms.Button btnSearchHierarchy;
        private System.Windows.Forms.Label label1;

    }
}