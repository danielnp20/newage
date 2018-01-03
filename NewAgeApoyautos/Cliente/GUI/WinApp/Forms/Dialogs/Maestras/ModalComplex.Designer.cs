namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalComplex
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
            this.lblCode = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.grlcontrolMasterComplex = new DevExpress.XtraGrid.GridControl();
            this.gvMasterComplex = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.lblpgGrid = new System.Windows.Forms.Label();
            this.RepositoryControl = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.btnReturn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.toolTipGrid = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupMaster)).BeginInit();
            this.groupMaster.SuspendLayout();
            this.groupFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlcontrolMasterComplex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMasterComplex)).BeginInit();
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
            this.groupMaster.Controls.Add(this.pgGrid);
            this.groupMaster.Controls.Add(this.grlcontrolMasterComplex);
            this.groupMaster.Controls.Add(this.lblpgGrid);
            this.groupMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMaster.Location = new System.Drawing.Point(0, 0);
            this.groupMaster.LookAndFeel.SkinName = "iMaginary";
            this.groupMaster.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupMaster.Name = "groupMaster";
            this.groupMaster.Size = new System.Drawing.Size(518, 345);
            this.groupMaster.TabIndex = 0;
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
            this.groupFind.TabIndex = 4;
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
            this.btnFind.Location = new System.Drawing.Point(376, 20);
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
            this.lblDescription.Location = new System.Drawing.Point(183, 23);
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
            this.lblTitle.Location = new System.Drawing.Point(8, 5);
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
            this.pgGrid.Location = new System.Drawing.Point(62, 261);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(443, 34);
            this.pgGrid.TabIndex = 2;
            // 
            // grlcontrolMasterComplex
            // 
            this.grlcontrolMasterComplex.AllowDrop = true;
            this.grlcontrolMasterComplex.Cursor = System.Windows.Forms.Cursors.Hand;
            this.grlcontrolMasterComplex.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grlcontrolMasterComplex.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grlcontrolMasterComplex.EmbeddedNavigator.Appearance.BackColor2 = System.Drawing.Color.White;
            this.grlcontrolMasterComplex.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.grlcontrolMasterComplex.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.grlcontrolMasterComplex.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.grlcontrolMasterComplex.EmbeddedNavigator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.grlcontrolMasterComplex.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.grlcontrolMasterComplex.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.grlcontrolMasterComplex.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.grlcontrolMasterComplex.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grlcontrolMasterComplex.Location = new System.Drawing.Point(8, 30);
            this.grlcontrolMasterComplex.LookAndFeel.SkinName = "Dark Side";
            this.grlcontrolMasterComplex.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grlcontrolMasterComplex.MainView = this.gvMasterComplex;
            this.grlcontrolMasterComplex.Margin = new System.Windows.Forms.Padding(4);
            this.grlcontrolMasterComplex.Name = "grlcontrolMasterComplex";
            this.grlcontrolMasterComplex.Size = new System.Drawing.Size(499, 230);
            this.grlcontrolMasterComplex.TabIndex = 0;
            this.grlcontrolMasterComplex.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMasterComplex,
            this.cardView1});
            this.grlcontrolMasterComplex.DoubleClick += new System.EventHandler(this.grlcontrolMasterModal_DoubleClick);
            this.grlcontrolMasterComplex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grlcontrolMasterModal_KeyDown);
            // 
            // gvMasterComplex
            // 
            this.gvMasterComplex.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMasterComplex.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvMasterComplex.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvMasterComplex.Appearance.Empty.Options.UseBackColor = true;
            this.gvMasterComplex.Appearance.FocusedCell.BackColor = System.Drawing.Color.Silver;
            this.gvMasterComplex.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvMasterComplex.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvMasterComplex.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvMasterComplex.Appearance.FocusedRow.BackColor = System.Drawing.Color.Silver;
            this.gvMasterComplex.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvMasterComplex.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvMasterComplex.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvMasterComplex.Appearance.HeaderPanel.BackColor = System.Drawing.Color.DimGray;
            this.gvMasterComplex.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.gvMasterComplex.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMasterComplex.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvMasterComplex.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvMasterComplex.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvMasterComplex.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvMasterComplex.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvMasterComplex.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvMasterComplex.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvMasterComplex.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvMasterComplex.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Silver;
            this.gvMasterComplex.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvMasterComplex.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvMasterComplex.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvMasterComplex.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvMasterComplex.Appearance.Row.Options.UseBackColor = true;
            this.gvMasterComplex.Appearance.Row.Options.UseFont = true;
            this.gvMasterComplex.Appearance.Row.Options.UseForeColor = true;
            this.gvMasterComplex.Appearance.Row.Options.UseTextOptions = true;
            this.gvMasterComplex.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvMasterComplex.Appearance.SelectedRow.BackColor = System.Drawing.Color.Silver;
            this.gvMasterComplex.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvMasterComplex.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gvMasterComplex.GridControl = this.grlcontrolMasterComplex;
            this.gvMasterComplex.HorzScrollStep = 50;
            this.gvMasterComplex.Name = "gvMasterComplex";
            this.gvMasterComplex.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvMasterComplex.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvMasterComplex.OptionsBehavior.Editable = false;
            this.gvMasterComplex.OptionsCustomization.AllowColumnMoving = false;
            this.gvMasterComplex.OptionsCustomization.AllowFilter = false;
            this.gvMasterComplex.OptionsCustomization.AllowSort = false;
            this.gvMasterComplex.OptionsFind.AllowFindPanel = false;
            this.gvMasterComplex.OptionsMenu.EnableColumnMenu = false;
            this.gvMasterComplex.OptionsMenu.EnableFooterMenu = false;
            this.gvMasterComplex.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvMasterComplex.OptionsView.ColumnAutoWidth = false;
            this.gvMasterComplex.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvMasterComplex.OptionsView.ShowGroupPanel = false;
            this.gvMasterComplex.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvMasterComplex.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvMasterModal_FocusedRowChanged);
            this.gvMasterComplex.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvMasterModal_CustomUnboundColumnData);
            this.gvMasterComplex.Click += new System.EventHandler(this.gvMasterModal_Click);
            // 
            // cardView1
            // 
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.grlcontrolMasterComplex;
            this.cardView1.Name = "cardView1";
            // 
            // lblpgGrid
            // 
            this.lblpgGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblpgGrid.Location = new System.Drawing.Point(8, 261);
            this.lblpgGrid.Name = "lblpgGrid";
            this.lblpgGrid.Size = new System.Drawing.Size(155, 34);
            this.lblpgGrid.TabIndex = 53;
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
            // ModalComplex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 345);
            this.Controls.Add(this.groupMaster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ModalComplex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupMaster)).EndInit();
            this.groupMaster.ResumeLayout(false);
            this.groupFind.ResumeLayout(false);
            this.groupFind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grlcontrolMasterComplex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMasterComplex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReturn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescription;
        private DevExpress.XtraGrid.GridControl grlcontrolMasterComplex;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMasterComplex;
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
        private System.Windows.Forms.Label lblpgGrid;

    }
}