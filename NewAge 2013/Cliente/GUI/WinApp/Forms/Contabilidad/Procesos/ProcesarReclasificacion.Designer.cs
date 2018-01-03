namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ProcesarReclasificacion
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
            base.InitializeComponent();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.period = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.gcOrigen = new DevExpress.XtraGrid.GridControl();
            this.gvOrigen = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.masterBalanceTipo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.gcOrigen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrigen)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProcesar
            // 
            this.btnProcesar.Enabled = false;
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnProcesar.Location = new System.Drawing.Point(174, 325);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 25);
            this.btnProcesar.TabIndex = 5;
            this.btnProcesar.Text = "1119_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // period
            // 
            this.period.BackColor = System.Drawing.Color.Transparent;
            this.period.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.period.Enabled = false;
            this.period.EnabledControl = true;
            this.period.ExtraPeriods = 0;
            this.period.Location = new System.Drawing.Point(130, 19);
            this.period.MaxValue = new System.DateTime(((long)(0)));
            this.period.MinValue = new System.DateTime(((long)(0)));
            this.period.Name = "period";
            this.period.Size = new System.Drawing.Size(130, 20);
            this.period.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "1119_lblPeriodo";
            // 
            // gcOrigen
            // 
            this.gcOrigen.AllowDrop = true;
            this.gcOrigen.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcOrigen.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcOrigen.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcOrigen.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcOrigen.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcOrigen.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcOrigen.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcOrigen.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcOrigen.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcOrigen.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcOrigen.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcOrigen.Location = new System.Drawing.Point(47, 88);
            this.gcOrigen.LookAndFeel.SkinName = "Dark Side";
            this.gcOrigen.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcOrigen.MainView = this.gvOrigen;
            this.gcOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.gcOrigen.Name = "gcOrigen";
            this.gcOrigen.Size = new System.Drawing.Size(416, 217);
            this.gcOrigen.TabIndex = 53;
            this.gcOrigen.UseEmbeddedNavigator = true;
            this.gcOrigen.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOrigen});
            // 
            // gvOrigen
            // 
            this.gvOrigen.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOrigen.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvOrigen.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvOrigen.Appearance.Empty.Options.UseBackColor = true;
            this.gvOrigen.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvOrigen.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvOrigen.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigen.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvOrigen.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvOrigen.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigen.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvOrigen.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvOrigen.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvOrigen.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvOrigen.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvOrigen.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOrigen.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvOrigen.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvOrigen.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvOrigen.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigen.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvOrigen.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvOrigen.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvOrigen.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvOrigen.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvOrigen.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvOrigen.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.Row.Options.UseBackColor = true;
            this.gvOrigen.Appearance.Row.Options.UseForeColor = true;
            this.gvOrigen.Appearance.Row.Options.UseTextOptions = true;
            this.gvOrigen.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvOrigen.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigen.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvOrigen.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvOrigen.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvOrigen.Appearance.VertLine.Options.UseBackColor = true;
            this.gvOrigen.GridControl = this.gcOrigen;
            this.gvOrigen.HorzScrollStep = 50;
            this.gvOrigen.Name = "gvOrigen";
            this.gvOrigen.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvOrigen.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvOrigen.OptionsCustomization.AllowColumnMoving = false;
            this.gvOrigen.OptionsCustomization.AllowFilter = false;
            this.gvOrigen.OptionsCustomization.AllowSort = false;
            this.gvOrigen.OptionsMenu.EnableColumnMenu = false;
            this.gvOrigen.OptionsMenu.EnableFooterMenu = false;
            this.gvOrigen.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvOrigen.OptionsView.ColumnAutoWidth = false;
            this.gvOrigen.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvOrigen.OptionsView.ShowGroupPanel = false;
            this.gvOrigen.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvOrigen.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // masterBalanceTipo
            // 
            this.masterBalanceTipo.BackColor = System.Drawing.Color.Transparent;
            this.masterBalanceTipo.Filtros = null;
            this.masterBalanceTipo.Location = new System.Drawing.Point(50, 48);
            this.masterBalanceTipo.Name = "masterBalanceTipo";
            this.masterBalanceTipo.Size = new System.Drawing.Size(291, 25);
            this.masterBalanceTipo.TabIndex = 54;
            this.masterBalanceTipo.Value = "";
            this.masterBalanceTipo.Leave += new System.EventHandler(this.masterBalanceTipo_Leave);
            // 
            // ProcesarReclasificacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 385);
            this.Controls.Add(this.masterBalanceTipo);
            this.Controls.Add(this.gcOrigen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.period);
            this.Controls.Add(this.btnProcesar);
            this.Name = "ProcesarReclasificacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1119";
            ((System.ComponentModel.ISupportInitialize)(this.gcOrigen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrigen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcesar;
        private ControlsUC.uc_PeriodoEdit period;
        private System.Windows.Forms.Label label1;
        protected DevExpress.XtraGrid.GridControl gcOrigen;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvOrigen;
        private ControlsUC.uc_MasterFind masterBalanceTipo;
    }
}