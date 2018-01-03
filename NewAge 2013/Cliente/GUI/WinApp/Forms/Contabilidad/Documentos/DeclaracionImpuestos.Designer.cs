namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DeclaracionImpuestos
    {
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
        protected virtual void InitializeComponent()
        {
            this.TbLyPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gcDeclaraciones = new DevExpress.XtraGrid.GridControl();
            this.gvDeclaraciones = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnDetails = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriodo = new DevExpress.XtraEditors.LabelControl();
            this.TbLyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDeclaraciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDeclaraciones)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TbLyPanel
            // 
            this.TbLyPanel.ColumnCount = 3;
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.011173F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 97.98883F));
            this.TbLyPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.TbLyPanel.Controls.Add(this.gcDeclaraciones, 1, 1);
            this.TbLyPanel.Controls.Add(this.pnDetails, 1, 3);
            this.TbLyPanel.Controls.Add(this.panel1, 1, 2);
            this.TbLyPanel.Controls.Add(this.panel2, 1, 0);
            this.TbLyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLyPanel.Location = new System.Drawing.Point(0, 0);
            this.TbLyPanel.Name = "TbLyPanel";
            this.TbLyPanel.RowCount = 4;
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.10478F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.89522F));
            this.TbLyPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.TbLyPanel.Size = new System.Drawing.Size(917, 567);
            this.TbLyPanel.TabIndex = 0;
            // 
            // gcDeclaraciones
            // 
            this.gcDeclaraciones.AllowDrop = true;
            this.gcDeclaraciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDeclaraciones.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDeclaraciones.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDeclaraciones.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDeclaraciones.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDeclaraciones.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDeclaraciones.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDeclaraciones.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDeclaraciones.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDeclaraciones.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDeclaraciones.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDeclaraciones.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDeclaraciones.EmbeddedNavigator.TextStringFormat = " Registro {0} of {1}";
            this.gcDeclaraciones.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDeclaraciones.Location = new System.Drawing.Point(21, 48);
            this.gcDeclaraciones.LookAndFeel.SkinName = "Dark Side";
            this.gcDeclaraciones.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDeclaraciones.MainView = this.gvDeclaraciones;
            this.gcDeclaraciones.Name = "gcDeclaraciones";
            this.gcDeclaraciones.Size = new System.Drawing.Size(873, 365);
            this.gcDeclaraciones.TabIndex = 3;
            this.gcDeclaraciones.UseEmbeddedNavigator = true;
            this.gcDeclaraciones.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDeclaraciones});
            // 
            // gvDeclaraciones
            // 
            this.gvDeclaraciones.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDeclaraciones.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDeclaraciones.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDeclaraciones.Appearance.Empty.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDeclaraciones.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDeclaraciones.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDeclaraciones.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDeclaraciones.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDeclaraciones.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDeclaraciones.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDeclaraciones.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDeclaraciones.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDeclaraciones.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDeclaraciones.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDeclaraciones.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDeclaraciones.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDeclaraciones.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDeclaraciones.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDeclaraciones.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDeclaraciones.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDeclaraciones.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDeclaraciones.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDeclaraciones.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDeclaraciones.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDeclaraciones.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDeclaraciones.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDeclaraciones.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDeclaraciones.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDeclaraciones.Appearance.Row.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.Row.Options.UseForeColor = true;
            this.gvDeclaraciones.Appearance.Row.Options.UseTextOptions = true;
            this.gvDeclaraciones.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDeclaraciones.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDeclaraciones.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDeclaraciones.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDeclaraciones.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDeclaraciones.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDeclaraciones.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDeclaraciones.GridControl = this.gcDeclaraciones;
            this.gvDeclaraciones.Name = "gvDeclaraciones";
            this.gvDeclaraciones.OptionsCustomization.AllowFilter = false;
            this.gvDeclaraciones.OptionsCustomization.AllowSort = false;
            this.gvDeclaraciones.OptionsCustomization.AllowColumnMoving = false;
            this.gvDeclaraciones.OptionsView.ShowGroupPanel = false;
            this.gvDeclaraciones.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDeclaraciones_CellValueChanging);
            this.gvDeclaraciones.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDeclaraciones_CustomUnboundColumnData);
            // 
            // pnDetails
            // 
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDetails.Location = new System.Drawing.Point(21, 479);
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(873, 85);
            this.pnDetails.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(21, 419);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(873, 54);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dtPeriod);
            this.panel2.Controls.Add(this.lblPeriodo);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(21, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(873, 31);
            this.panel2.TabIndex = 0;
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(90, 6);
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 20);
            this.dtPeriod.TabIndex = 3;
            this.dtPeriod.ValueChanged += new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit.EventHandler(this.dtPeriod_EditValueChanged);
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblPeriodo.Location = new System.Drawing.Point(3, 9);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(94, 14);
            this.lblPeriodo.TabIndex = 1;
            this.lblPeriodo.Text = "20505_lblPeriodo";
            // 
            // DeclaracionImpuestos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 567);
            this.Controls.Add(this.TbLyPanel);
            this.Name = "DeclaracionImpuestos";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FormClosed);
            this.TbLyPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDeclaraciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDeclaraciones)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TableLayoutPanel TbLyPanel;
        protected DevExpress.XtraGrid.GridControl gcDeclaraciones;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDeclaraciones;
        private System.ComponentModel.IContainer components;
        protected System.Windows.Forms.Panel pnDetails;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel panel2;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        protected DevExpress.XtraEditors.LabelControl lblPeriodo;
    }
}