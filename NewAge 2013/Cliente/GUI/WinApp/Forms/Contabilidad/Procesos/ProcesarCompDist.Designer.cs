namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ProcesarCompDist
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
            this.btnPreliminar = new System.Windows.Forms.Button();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.periodBegin = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.periodEnd = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gcOrigen = new DevExpress.XtraGrid.GridControl();
            this.gvOrigen = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcOrigen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrigen)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAjuste
            // 
            this.btnPreliminar.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnPreliminar.Location = new System.Drawing.Point(69, 331);
            this.btnPreliminar.Name = "btnAjuste";
            this.btnPreliminar.Size = new System.Drawing.Size(152, 26);
            this.btnPreliminar.TabIndex = 4;
            this.btnPreliminar.Text = "1117_btnAjustePreliminar";
            this.btnPreliminar.UseVisualStyleBackColor = true;
            this.btnPreliminar.Click += new System.EventHandler(this.btnAjuste_Click);
            // 
            // btnProcesar
            // 
            this.btnProcesar.Enabled = false;
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnProcesar.Location = new System.Drawing.Point(261, 331);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(152, 25);
            this.btnProcesar.TabIndex = 5;
            this.btnProcesar.Text = "1117_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // periodBegin
            // 
            this.periodBegin.BackColor = System.Drawing.Color.Transparent;
            this.periodBegin.DateTime = new System.DateTime(((long)(0)));
            this.periodBegin.EnabledControl = true;
            this.periodBegin.ExtraPeriods = 0;
            this.periodBegin.Location = new System.Drawing.Point(130, 26);
            this.periodBegin.Name = "periodBegin";
            this.periodBegin.Size = new System.Drawing.Size(130, 20);
            this.periodBegin.TabIndex = 6;
            // 
            // periodEnd
            // 
            this.periodEnd.BackColor = System.Drawing.Color.Transparent;
            this.periodEnd.DateTime = new System.DateTime(((long)(0)));
            this.periodEnd.EnabledControl = true;
            this.periodEnd.ExtraPeriods = 0;
            this.periodEnd.Location = new System.Drawing.Point(130, 52);
            this.periodEnd.Name = "periodEnd";
            this.periodEnd.Size = new System.Drawing.Size(130, 20);
            this.periodEnd.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "1117_lblDesde";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "1117_lblHasta";
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
            this.gcOrigen.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcOrigen.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcOrigen.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcOrigen.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcOrigen.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcOrigen.Location = new System.Drawing.Point(47, 97);
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
            // ProcesarCompDest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 385);
            this.Controls.Add(this.gcOrigen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.periodEnd);
            this.Controls.Add(this.periodBegin);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.btnPreliminar);
            this.Name = "ProcesarCompDest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1117";
            ((System.ComponentModel.ISupportInitialize)(this.gcOrigen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrigen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPreliminar;
        private System.Windows.Forms.Button btnProcesar;
        private ControlsUC.uc_PeriodoEdit periodBegin;
        private ControlsUC.uc_PeriodoEdit periodEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        protected DevExpress.XtraGrid.GridControl gcOrigen;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvOrigen;
    }
}