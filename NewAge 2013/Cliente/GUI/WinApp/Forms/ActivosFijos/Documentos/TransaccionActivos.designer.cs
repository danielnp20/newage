namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class TransaccionActivos
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
            this.MasterMvtoTipo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtPlaqueta = new System.Windows.Forms.TextBox();
            this.lblPlaqueta = new System.Windows.Forms.Label();
            this.lblSerial = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.MasterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.MasterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gcValores = new DevExpress.XtraGrid.GridControl();
            this.gvValores = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcValores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvValores)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.gcValores);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Size = new System.Drawing.Size(999, 167);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editBtnGrid,
            this.editCmb,
            this.editText,
            this.editSpin,
            this.editSpin4,
            this.editDate,
            this.editValue,
            this.editValue4});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin4
            // 
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editDate
            // 
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editValue4
            // 
            this.editValue4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblMark
            // 
            this.lblMark.Location = new System.Drawing.Point(1103, 5);
            // 
            // btnMark
            // 
            this.btnMark.Location = new System.Drawing.Point(1293, 2);
            this.btnMark.Margin = new System.Windows.Forms.Padding(2);
            this.btnMark.Visible = false;
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(2);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(2);
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 20);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(2);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(2);
            this.txtPrefix.Size = new System.Drawing.Size(50, 20);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtPeriod
            // 
            this.dtPeriod.Size = new System.Drawing.Size(130, 20);
            // 
            // txtAF
            // 
            this.txtAF.Margin = new System.Windows.Forms.Padding(2);
            this.txtAF.Size = new System.Drawing.Size(91, 20);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(2);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(4, 0, 4, 2);
            this.gbGridDocument.Size = new System.Drawing.Size(703, 248);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(703, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(2);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 248);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.MasterProyecto);
            this.grpboxHeader.Controls.Add(this.MasterCentroCosto);
            this.grpboxHeader.Controls.Add(this.lblSerial);
            this.grpboxHeader.Controls.Add(this.txtSerial);
            this.grpboxHeader.Controls.Add(this.lblPlaqueta);
            this.grpboxHeader.Controls.Add(this.txtPlaqueta);
            this.grpboxHeader.Controls.Add(this.MasterMvtoTipo);
            this.grpboxHeader.Location = new System.Drawing.Point(6, 26);
            this.grpboxHeader.Size = new System.Drawing.Size(1117, 159);
            // 
            // MasterMvtoTipo
            // 
            this.MasterMvtoTipo.BackColor = System.Drawing.Color.Transparent;
            this.MasterMvtoTipo.Filtros = null;
            this.MasterMvtoTipo.Location = new System.Drawing.Point(16, 19);
            this.MasterMvtoTipo.Name = "MasterMvtoTipo";
            this.MasterMvtoTipo.Size = new System.Drawing.Size(291, 25);
            this.MasterMvtoTipo.TabIndex = 18;
            this.MasterMvtoTipo.Value = "";
            this.MasterMvtoTipo.Leave += new System.EventHandler(this.masterMovimientoTipo_Leave);
            // 
            // txtPlaqueta
            // 
            this.txtPlaqueta.Location = new System.Drawing.Point(464, 19);
            this.txtPlaqueta.Name = "txtPlaqueta";
            this.txtPlaqueta.Size = new System.Drawing.Size(100, 20);
            this.txtPlaqueta.TabIndex = 17;
            // 
            // lblPlaqueta
            // 
            this.lblPlaqueta.AutoSize = true;
            this.lblPlaqueta.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlaqueta.Location = new System.Drawing.Point(362, 19);
            this.lblPlaqueta.Name = "lblPlaqueta";
            this.lblPlaqueta.Size = new System.Drawing.Size(90, 14);
            this.lblPlaqueta.TabIndex = 19;
            this.lblPlaqueta.Text = "64_lblPLaqueta";
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSerial.Location = new System.Drawing.Point(362, 55);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(67, 14);
            this.lblSerial.TabIndex = 21;
            this.lblSerial.Text = "64_lblSerial";
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(464, 55);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(100, 20);
            this.txtSerial.TabIndex = 20;
            // 
            // MasterCentroCosto
            // 
            this.MasterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.MasterCentroCosto.Filtros = null;
            this.MasterCentroCosto.Location = new System.Drawing.Point(663, 19);
            this.MasterCentroCosto.Name = "MasterCentroCosto";
            this.MasterCentroCosto.Size = new System.Drawing.Size(291, 25);
            this.MasterCentroCosto.TabIndex = 22;
            this.MasterCentroCosto.Value = "";
            // 
            // MasterProyecto
            // 
            this.MasterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.MasterProyecto.Filtros = null;
            this.MasterProyecto.Location = new System.Drawing.Point(663, 55);
            this.MasterProyecto.Name = "MasterProyecto";
            this.MasterProyecto.Size = new System.Drawing.Size(291, 25);
            this.MasterProyecto.TabIndex = 23;
            this.MasterProyecto.Value = "";
            // 
            // gcValores
            // 
            this.gcValores.AllowDrop = true;
            this.gcValores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcValores.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gcValores.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcValores.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcValores.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcValores.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcValores.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcValores.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcValores.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcValores.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcValores.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcValores.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcValores.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcValores.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcValores.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcValores.Location = new System.Drawing.Point(3, 16);
            this.gcValores.LookAndFeel.SkinName = "Dark Side";
            this.gcValores.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcValores.MainView = this.gvValores;
            this.gcValores.Margin = new System.Windows.Forms.Padding(4);
            this.gcValores.Name = "gcValores";
            this.gcValores.Size = new System.Drawing.Size(993, 148);
            this.gcValores.TabIndex = 51;
            this.gcValores.UseEmbeddedNavigator = true;
            this.gcValores.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvValores});
            // 
            // gvValores
            // 
            this.gvValores.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvValores.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvValores.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvValores.Appearance.Empty.Options.UseBackColor = true;
            this.gvValores.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvValores.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvValores.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvValores.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvValores.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvValores.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvValores.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvValores.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvValores.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvValores.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvValores.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvValores.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvValores.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvValores.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvValores.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvValores.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvValores.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvValores.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvValores.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvValores.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvValores.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvValores.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvValores.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvValores.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvValores.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvValores.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvValores.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvValores.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvValores.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvValores.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvValores.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvValores.Appearance.Row.Options.UseBackColor = true;
            this.gvValores.Appearance.Row.Options.UseForeColor = true;
            this.gvValores.Appearance.Row.Options.UseTextOptions = true;
            this.gvValores.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvValores.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvValores.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvValores.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvValores.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvValores.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvValores.Appearance.VertLine.Options.UseBackColor = true;
            this.gvValores.GridControl = this.gcValores;
            this.gvValores.HorzScrollStep = 50;
            this.gvValores.Name = "gvValores";
            this.gvValores.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvValores.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvValores.OptionsCustomization.AllowColumnMoving = false;
            this.gvValores.OptionsCustomization.AllowFilter = false;
            this.gvValores.OptionsCustomization.AllowSort = false;
            this.gvValores.OptionsMenu.EnableColumnMenu = false;
            this.gvValores.OptionsMenu.EnableFooterMenu = false;
            this.gvValores.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvValores.OptionsView.ColumnAutoWidth = false;
            this.gvValores.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvValores.OptionsView.ShowGroupPanel = false;
            this.gvValores.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvValores.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvValores_CellValueChanged);
            this.gvValores.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvValores_CustomUnboundColumnData);
            // 
            // TransaccionActivos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1027, 589);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TransaccionActivos";
            this.Text = "61";
            this.grpboxDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcValores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvValores)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private ControlsUC.uc_MasterFind MasterMvtoTipo;
        private System.Windows.Forms.TextBox txtPlaqueta;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.Label lblPlaqueta;
        private ControlsUC.uc_MasterFind MasterProyecto;
        private ControlsUC.uc_MasterFind MasterCentroCosto;
        protected DevExpress.XtraGrid.GridControl gcValores;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvValores;

    }
}