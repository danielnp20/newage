namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalMasterHierarchy
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
            this.groupHierarachy = new DevExpress.XtraEditors.GroupControl();
            this.groupFind = new System.Windows.Forms.GroupBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.grlcontrolHierarachy = new DevExpress.XtraGrid.GridControl();
            this.gvHierarachy = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.label1 = new System.Windows.Forms.Label();
            this.RepositoryControl = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.btnReturn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.toolTipGrid = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupHierarachy)).BeginInit();
            this.groupHierarachy.SuspendLayout();
            this.groupFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlcontrolHierarachy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHierarachy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).BeginInit();
            this.SuspendLayout();
            // 
            // groupHierarachy
            // 
            this.groupHierarachy.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupHierarachy.Appearance.Options.UseBackColor = true;
            this.groupHierarachy.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupHierarachy.AppearanceCaption.Options.UseFont = true;
            this.groupHierarachy.AppearanceCaption.Options.UseTextOptions = true;
            this.groupHierarachy.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupHierarachy.Controls.Add(this.groupFind);
            this.groupHierarachy.Controls.Add(this.lblTitle);
            this.groupHierarachy.Controls.Add(this.pgGrid);
            this.groupHierarachy.Controls.Add(this.grlcontrolHierarachy);
            this.groupHierarachy.Controls.Add(this.label1);
            this.groupHierarachy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupHierarachy.Location = new System.Drawing.Point(0, 0);
            this.groupHierarachy.LookAndFeel.SkinName = "iMaginary";
            this.groupHierarachy.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupHierarachy.Name = "groupHierarachy";
            this.groupHierarachy.Size = new System.Drawing.Size(518, 344);
            this.groupHierarachy.TabIndex = 49;
            // 
            // groupFind
            // 
            this.groupFind.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupFind.Controls.Add(this.lblCode);
            this.groupFind.Controls.Add(this.txtDescription);
            this.groupFind.Controls.Add(this.btnFind);
            this.groupFind.Controls.Add(this.lblDescription);
            this.groupFind.Controls.Add(this.txtCode);
            this.groupFind.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupFind.Location = new System.Drawing.Point(7, 283);
            this.groupFind.Name = "groupFind";
            this.groupFind.Size = new System.Drawing.Size(500, 50);
            this.groupFind.TabIndex = 52;
            this.groupFind.TabStop = false;
            this.groupFind.Text = "1002_groupFind";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lblCode.Location = new System.Drawing.Point(10, 22);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(81, 14);
            this.lblCode.TabIndex = 48;
            this.lblCode.Text = "1002_lblCode";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(254, 21);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(100, 22);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDescription_KeyDown);
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Location = new System.Drawing.Point(377, 20);
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
            this.lblDescription.Location = new System.Drawing.Point(184, 23);
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
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(11, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(495, 18);
            this.lblTitle.TabIndex = 78;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pgGrid
            // 
            this.pgGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(65, 260);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(442, 28);
            this.pgGrid.TabIndex = 2;
            // 
            // grlcontrolHierarachy
            // 
            this.grlcontrolHierarachy.AllowDrop = true;
            this.grlcontrolHierarachy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.grlcontrolHierarachy.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grlcontrolHierarachy.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grlcontrolHierarachy.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.grlcontrolHierarachy.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.grlcontrolHierarachy.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.grlcontrolHierarachy.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.grlcontrolHierarachy.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.grlcontrolHierarachy.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grlcontrolHierarachy.EmbeddedNavigator.TextStringFormat = "{0} of {1}";
            this.grlcontrolHierarachy.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.grlcontrolHierarachy.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grlcontrolHierarachy.Location = new System.Drawing.Point(8, 29);
            this.grlcontrolHierarachy.LookAndFeel.SkinName = "Dark Side";
            this.grlcontrolHierarachy.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grlcontrolHierarachy.MainView = this.gvHierarachy;
            this.grlcontrolHierarachy.Margin = new System.Windows.Forms.Padding(4);
            this.grlcontrolHierarachy.Name = "grlcontrolHierarachy";
            this.grlcontrolHierarachy.Size = new System.Drawing.Size(499, 230);
            this.grlcontrolHierarachy.TabIndex = 0;
            this.toolTipGrid.SetToolTip(this.grlcontrolHierarachy, "1002_toolTipGrid");
            this.grlcontrolHierarachy.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHierarachy,
            this.cardView1});
            this.grlcontrolHierarachy.DoubleClick += new System.EventHandler(this.grlcontrolHierarachy_DoubleClick);
            this.grlcontrolHierarachy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grlcontrolHierarachy_KeyDown);
            // 
            // gvHierarachy
            // 
            this.gvHierarachy.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvHierarachy.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvHierarachy.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvHierarachy.Appearance.Empty.Options.UseBackColor = true;
            this.gvHierarachy.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvHierarachy.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvHierarachy.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvHierarachy.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvHierarachy.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvHierarachy.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvHierarachy.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvHierarachy.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvHierarachy.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvHierarachy.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvHierarachy.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvHierarachy.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.DarkGray;
            this.gvHierarachy.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvHierarachy.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvHierarachy.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvHierarachy.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvHierarachy.Appearance.Row.Options.UseBackColor = true;
            this.gvHierarachy.Appearance.Row.Options.UseFont = true;
            this.gvHierarachy.Appearance.Row.Options.UseForeColor = true;
            this.gvHierarachy.Appearance.Row.Options.UseTextOptions = true;
            this.gvHierarachy.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvHierarachy.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvHierarachy.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvHierarachy.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvHierarachy.GridControl = this.grlcontrolHierarachy;
            this.gvHierarachy.HorzScrollStep = 50;
            this.gvHierarachy.Name = "gvHierarachy";
            this.gvHierarachy.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvHierarachy.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvHierarachy.OptionsBehavior.Editable = false;
            this.gvHierarachy.OptionsCustomization.AllowColumnMoving = false;
            this.gvHierarachy.OptionsCustomization.AllowFilter = false;
            this.gvHierarachy.OptionsCustomization.AllowSort = false;
            this.gvHierarachy.OptionsFind.AllowFindPanel = false;
            this.gvHierarachy.OptionsMenu.EnableColumnMenu = false;
            this.gvHierarachy.OptionsMenu.EnableFooterMenu = false;
            this.gvHierarachy.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvHierarachy.OptionsView.ColumnAutoWidth = false;
            this.gvHierarachy.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvHierarachy.OptionsView.ShowGroupPanel = false;
            this.gvHierarachy.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvHierarachy.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvHierarachy_FocusedRowChanged);
            this.gvHierarachy.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvHierarachy_CustomUnboundColumnData);
            this.gvHierarachy.Click += new System.EventHandler(this.gvMasterModal_Click);
            // 
            // cardView1
            // 
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.grlcontrolHierarachy;
            this.cardView1.Name = "cardView1";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(8, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 28);
            this.label1.TabIndex = 53;
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
            // ModalMasterHierarchy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 344);
            this.Controls.Add(this.groupHierarachy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ModalMasterHierarchy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupHierarachy)).EndInit();
            this.groupHierarachy.ResumeLayout(false);
            this.groupFind.ResumeLayout(false);
            this.groupFind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlcontrolHierarachy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHierarachy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescription;
        private DevExpress.XtraGrid.GridControl grlcontrolHierarachy;
        private DevExpress.XtraGrid.Views.Grid.GridView gvHierarachy;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.GroupBox groupFind;
        internal DevExpress.XtraEditors.GroupControl groupHierarachy;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryControl;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btnReturn;
        private ControlsUC.uc_Pagging pgGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolTip toolTipGrid;
    }
}