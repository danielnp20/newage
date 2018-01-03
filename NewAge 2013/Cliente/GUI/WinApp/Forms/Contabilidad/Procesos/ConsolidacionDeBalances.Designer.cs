namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsolidacionDeBalances
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
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.periodoEdit = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.gcCompanies = new DevExpress.XtraGrid.GridControl();
            this.gvCompanies = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcCompanies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCompanies)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProcesar
            // 
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.Location = new System.Drawing.Point(162, 316);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 33);
            this.btnProcesar.TabIndex = 4;
            this.btnProcesar.Text = "1120_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(29, 35);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(87, 14);
            this.lblPeriod.TabIndex = 6;
            this.lblPeriod.Text = "1120_lblPeriod";
            // 
            // periodoEdit
            // 
            this.periodoEdit.BackColor = System.Drawing.Color.Transparent;
            this.periodoEdit.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.periodoEdit.Enabled = false;
            this.periodoEdit.EnabledControl = true;
            this.periodoEdit.ExtraPeriods = 0;
            this.periodoEdit.Location = new System.Drawing.Point(136, 31);
            this.periodoEdit.MaxValue = new System.DateTime(((long)(0)));
            this.periodoEdit.MinValue = new System.DateTime(((long)(0)));
            this.periodoEdit.Name = "periodoEdit";
            this.periodoEdit.Size = new System.Drawing.Size(130, 21);
            this.periodoEdit.TabIndex = 7;
            // 
            // gcCompanies
            // 
            this.gcCompanies.AllowDrop = true;
            this.gcCompanies.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCompanies.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcCompanies.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcCompanies.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcCompanies.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcCompanies.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcCompanies.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcCompanies.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcCompanies.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcCompanies.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcCompanies.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcCompanies.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcCompanies.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcCompanies.Location = new System.Drawing.Point(32, 68);
            this.gcCompanies.LookAndFeel.SkinName = "Dark Side";
            this.gcCompanies.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcCompanies.MainView = this.gvCompanies;
            this.gcCompanies.Name = "gcCompanies";
            this.gcCompanies.Size = new System.Drawing.Size(461, 222);
            this.gcCompanies.TabIndex = 8;
            this.gcCompanies.UseEmbeddedNavigator = true;
            this.gcCompanies.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCompanies});
            // 
            // gvCompanies
            // 
            this.gvCompanies.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCompanies.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvCompanies.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvCompanies.Appearance.Empty.Options.UseBackColor = true;
            this.gvCompanies.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvCompanies.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvCompanies.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCompanies.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvCompanies.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvCompanies.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvCompanies.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCompanies.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvCompanies.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvCompanies.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvCompanies.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvCompanies.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvCompanies.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvCompanies.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvCompanies.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCompanies.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvCompanies.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCompanies.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvCompanies.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvCompanies.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvCompanies.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvCompanies.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCompanies.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvCompanies.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCompanies.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvCompanies.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvCompanies.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvCompanies.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCompanies.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvCompanies.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvCompanies.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvCompanies.Appearance.Row.Options.UseBackColor = true;
            this.gvCompanies.Appearance.Row.Options.UseForeColor = true;
            this.gvCompanies.Appearance.Row.Options.UseTextOptions = true;
            this.gvCompanies.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvCompanies.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCompanies.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvCompanies.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvCompanies.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvCompanies.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCompanies.Appearance.VertLine.Options.UseBackColor = true;
            this.gvCompanies.GridControl = this.gcCompanies;
            this.gvCompanies.Name = "gvCompanies";
            this.gvCompanies.OptionsCustomization.AllowColumnMoving = false;
            this.gvCompanies.OptionsCustomization.AllowFilter = false;
            this.gvCompanies.OptionsCustomization.AllowSort = false;
            this.gvCompanies.OptionsView.ShowGroupPanel = false;
            this.gvCompanies.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvCompanies_CustomUnboundColumnData);
            // 
            // ConsolidacionDeBalances
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 392);
            this.Controls.Add(this.gcCompanies);
            this.Controls.Add(this.periodoEdit);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.btnProcesar);
            this.Name = "ConsolidacionDeBalances";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1020";
            ((System.ComponentModel.ISupportInitialize)(this.gcCompanies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCompanies)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Label lblPeriod;
        private ControlsUC.uc_PeriodoEdit periodoEdit;
        protected DevExpress.XtraGrid.GridControl gcCompanies;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvCompanies;
    }
}