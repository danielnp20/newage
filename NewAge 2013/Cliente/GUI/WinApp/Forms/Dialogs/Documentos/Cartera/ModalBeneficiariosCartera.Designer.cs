namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalBeneficiariosCartera
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
            this.txtVlrGiro = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrGrio = new System.Windows.Forms.Label();
            this.txtVlrBenefi = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrBenefi = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gcBeneficiario = new DevExpress.XtraGrid.GridControl();
            this.gvBeneficiario = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblBeneficiarios = new System.Windows.Forms.Label();
            this.RepositoryControl = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupAnticipo)).BeginInit();
            this.groupAnticipo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrGiro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrBenefi.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBeneficiario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBeneficiario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
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
            this.groupAnticipo.Controls.Add(this.txtVlrGiro);
            this.groupAnticipo.Controls.Add(this.lblVlrGrio);
            this.groupAnticipo.Controls.Add(this.txtVlrBenefi);
            this.groupAnticipo.Controls.Add(this.lblVlrBenefi);
            this.groupAnticipo.Controls.Add(this.groupBox3);
            this.groupAnticipo.Controls.Add(this.lblBeneficiarios);
            this.groupAnticipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupAnticipo.Location = new System.Drawing.Point(0, 0);
            this.groupAnticipo.LookAndFeel.SkinName = "iMaginary";
            this.groupAnticipo.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupAnticipo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupAnticipo.Name = "groupAnticipo";
            this.groupAnticipo.Size = new System.Drawing.Size(1070, 430);
            this.groupAnticipo.TabIndex = 0;
            // 
            // txtVlrGiro
            // 
            this.txtVlrGiro.EditValue = "0";
            this.txtVlrGiro.Location = new System.Drawing.Point(458, 369);
            this.txtVlrGiro.Name = "txtVlrGiro";
            this.txtVlrGiro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrGiro.Properties.Appearance.Options.UseFont = true;
            this.txtVlrGiro.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrGiro.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrGiro.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrGiro.Properties.Mask.EditMask = "c";
            this.txtVlrGiro.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrGiro.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrGiro.Properties.ReadOnly = true;
            this.txtVlrGiro.Size = new System.Drawing.Size(195, 28);
            this.txtVlrGiro.TabIndex = 85;
            // 
            // lblVlrGrio
            // 
            this.lblVlrGrio.AutoSize = true;
            this.lblVlrGrio.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrGrio.Location = new System.Drawing.Point(320, 372);
            this.lblVlrGrio.Name = "lblVlrGrio";
            this.lblVlrGrio.Size = new System.Drawing.Size(132, 22);
            this.lblVlrGrio.TabIndex = 84;
            this.lblVlrGrio.Text = "1036_ValorGiro";
            // 
            // txtVlrBenefi
            // 
            this.txtVlrBenefi.EditValue = "0";
            this.txtVlrBenefi.Location = new System.Drawing.Point(861, 369);
            this.txtVlrBenefi.Name = "txtVlrBenefi";
            this.txtVlrBenefi.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrBenefi.Properties.Appearance.Options.UseFont = true;
            this.txtVlrBenefi.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrBenefi.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrBenefi.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrBenefi.Properties.Mask.EditMask = "c";
            this.txtVlrBenefi.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrBenefi.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrBenefi.Properties.ReadOnly = true;
            this.txtVlrBenefi.Size = new System.Drawing.Size(195, 28);
            this.txtVlrBenefi.TabIndex = 83;
            // 
            // lblVlrBenefi
            // 
            this.lblVlrBenefi.AutoSize = true;
            this.lblVlrBenefi.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrBenefi.Location = new System.Drawing.Point(672, 372);
            this.lblVlrBenefi.Name = "lblVlrBenefi";
            this.lblVlrBenefi.Size = new System.Drawing.Size(198, 22);
            this.lblVlrBenefi.TabIndex = 82;
            this.lblVlrBenefi.Text = "1036_ValorBeneficiarios";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gcBeneficiario);
            this.groupBox3.Location = new System.Drawing.Point(12, 39);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1047, 324);
            this.groupBox3.TabIndex = 81;
            this.groupBox3.TabStop = false;
            // 
            // gcBeneficiario
            // 
            this.gcBeneficiario.AllowDrop = true;
            this.gcBeneficiario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBeneficiario.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gcBeneficiario.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcBeneficiario.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcBeneficiario.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcBeneficiario.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcBeneficiario.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcBeneficiario.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcBeneficiario.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcBeneficiario.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcBeneficiario.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcBeneficiario.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcBeneficiario.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcBeneficiario.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcBeneficiario_EmbeddedNavigator_ButtonClick);
            this.gcBeneficiario.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcBeneficiario.Location = new System.Drawing.Point(3, 22);
            this.gcBeneficiario.LookAndFeel.SkinName = "Dark Side";
            this.gcBeneficiario.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcBeneficiario.MainView = this.gvBeneficiario;
            this.gcBeneficiario.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcBeneficiario.Name = "gcBeneficiario";
            this.gcBeneficiario.Size = new System.Drawing.Size(1041, 299);
            this.gcBeneficiario.TabIndex = 0;
            this.gcBeneficiario.UseEmbeddedNavigator = true;
            this.gcBeneficiario.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBeneficiario});
            // 
            // gvBeneficiario
            // 
            this.gvBeneficiario.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvBeneficiario.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvBeneficiario.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvBeneficiario.Appearance.Empty.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvBeneficiario.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBeneficiario.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvBeneficiario.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvBeneficiario.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBeneficiario.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvBeneficiario.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvBeneficiario.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvBeneficiario.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvBeneficiario.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvBeneficiario.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvBeneficiario.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvBeneficiario.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvBeneficiario.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvBeneficiario.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvBeneficiario.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvBeneficiario.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvBeneficiario.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvBeneficiario.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvBeneficiario.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBeneficiario.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvBeneficiario.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvBeneficiario.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvBeneficiario.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvBeneficiario.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvBeneficiario.Appearance.Row.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.Row.Options.UseForeColor = true;
            this.gvBeneficiario.Appearance.Row.Options.UseTextOptions = true;
            this.gvBeneficiario.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvBeneficiario.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBeneficiario.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvBeneficiario.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvBeneficiario.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvBeneficiario.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvBeneficiario.Appearance.VertLine.Options.UseBackColor = true;
            this.gvBeneficiario.GridControl = this.gcBeneficiario;
            this.gvBeneficiario.HorzScrollStep = 50;
            this.gvBeneficiario.Name = "gvBeneficiario";
            this.gvBeneficiario.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvBeneficiario.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvBeneficiario.OptionsCustomization.AllowColumnMoving = false;
            this.gvBeneficiario.OptionsCustomization.AllowFilter = false;
            this.gvBeneficiario.OptionsCustomization.AllowSort = false;
            this.gvBeneficiario.OptionsMenu.EnableColumnMenu = false;
            this.gvBeneficiario.OptionsMenu.EnableFooterMenu = false;
            this.gvBeneficiario.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvBeneficiario.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvBeneficiario.OptionsView.ShowGroupPanel = false;
            this.gvBeneficiario.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvBeneficiario.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvBeneficiario_FocusedRowChanged);
            this.gvBeneficiario.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvBeneficiario_CellValueChanged);
            this.gvBeneficiario.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvBeneficiario_BeforeLeaveRow);
            this.gvBeneficiario.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvBeneficiario_CustomUnboundColumnData);
            // 
            // lblBeneficiarios
            // 
            this.lblBeneficiarios.BackColor = System.Drawing.Color.Transparent;
            this.lblBeneficiarios.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeneficiarios.Location = new System.Drawing.Point(12, 6);
            this.lblBeneficiarios.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBeneficiarios.Name = "lblBeneficiarios";
            this.lblBeneficiarios.Size = new System.Drawing.Size(1050, 28);
            this.lblBeneficiarios.TabIndex = 77;
            this.lblBeneficiarios.Text = "1036_Beneficiarios";
            this.lblBeneficiarios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RepositoryControl
            // 
            this.RepositoryControl.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editSpin,
            this.editBtnGrid});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editBtnGrid.Name = "editBtnGrid";
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnGrid_ButtonClick);
            // 
            // ModalBeneficiariosCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 430);
            this.Controls.Add(this.groupAnticipo);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "ModalBeneficiariosCartera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupAnticipo)).EndInit();
            this.groupAnticipo.ResumeLayout(false);
            this.groupAnticipo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrGiro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrBenefi.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBeneficiario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBeneficiario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraEditors.GroupControl groupAnticipo;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryControl;
        private System.Windows.Forms.Label lblBeneficiarios;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        private System.Windows.Forms.GroupBox groupBox3;
        protected DevExpress.XtraGrid.GridControl gcBeneficiario;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvBeneficiario;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        private DevExpress.XtraEditors.TextEdit txtVlrGiro;
        private System.Windows.Forms.Label lblVlrGrio;
        private DevExpress.XtraEditors.TextEdit txtVlrBenefi;
        private System.Windows.Forms.Label lblVlrBenefi;
    }
}