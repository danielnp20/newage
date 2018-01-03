namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class UC_Empleados
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gcEmpleado = new DevExpress.XtraGrid.GridControl();
            this.gvEmpleado = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.empleadoId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.empleado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCmbEstado = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editCheckBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaIngreso = new DevExpress.XtraEditors.DateEdit();
            this.uc_AreaFuncional = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_Proyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_Operacion = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_CentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtEstado = new DevExpress.XtraEditors.TextEdit();
            this.lkpEstadoDesc = new DevExpress.XtraEditors.ComboBoxEdit();
            this.chkRetirosmes = new DevExpress.XtraEditors.CheckEdit();
            this.cmbTipoContrato = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoContrato = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gcEmpleado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmpleado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCmbEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheckBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIngreso.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIngreso.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpEstadoDesc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRetirosmes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoContrato.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gcEmpleado
            // 
            this.gcEmpleado.AllowDrop = true;
            this.gcEmpleado.Dock = System.Windows.Forms.DockStyle.Left;
            this.gcEmpleado.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcEmpleado.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcEmpleado.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcEmpleado.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcEmpleado.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcEmpleado.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcEmpleado.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcEmpleado.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcEmpleado.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcEmpleado.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcEmpleado.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcEmpleado.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcEmpleado.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcEmpleado.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcEmpleado.Location = new System.Drawing.Point(0, 0);
            this.gcEmpleado.LookAndFeel.SkinName = "Dark Side";
            this.gcEmpleado.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcEmpleado.MainView = this.gvEmpleado;
            this.gcEmpleado.Margin = new System.Windows.Forms.Padding(4);
            this.gcEmpleado.Name = "gcEmpleado";
            this.gcEmpleado.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCmbEstado});
            this.gcEmpleado.Size = new System.Drawing.Size(497, 247);
            this.gcEmpleado.TabIndex = 6;
            this.gcEmpleado.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvEmpleado});
            // 
            // gvEmpleado
            // 
            this.gvEmpleado.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvEmpleado.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvEmpleado.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvEmpleado.Appearance.Empty.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvEmpleado.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvEmpleado.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvEmpleado.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvEmpleado.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvEmpleado.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpleado.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvEmpleado.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpleado.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvEmpleado.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvEmpleado.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvEmpleado.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvEmpleado.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvEmpleado.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvEmpleado.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvEmpleado.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvEmpleado.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvEmpleado.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvEmpleado.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvEmpleado.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvEmpleado.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvEmpleado.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvEmpleado.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvEmpleado.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvEmpleado.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvEmpleado.Appearance.Row.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.Row.Options.UseForeColor = true;
            this.gvEmpleado.Appearance.Row.Options.UseTextOptions = true;
            this.gvEmpleado.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvEmpleado.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvEmpleado.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvEmpleado.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpleado.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvEmpleado.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvEmpleado.Appearance.VertLine.Options.UseBackColor = true;
            this.gvEmpleado.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.empleadoId,
            this.empleado});
            this.gvEmpleado.GridControl = this.gcEmpleado;
            this.gvEmpleado.Name = "gvEmpleado";
            this.gvEmpleado.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvEmpleado.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvEmpleado.OptionsCustomization.AllowColumnMoving = false;
            this.gvEmpleado.OptionsCustomization.AllowFilter = false;
            this.gvEmpleado.OptionsFind.AlwaysVisible = true;
            this.gvEmpleado.OptionsFind.FindNullPrompt = "";
            this.gvEmpleado.OptionsMenu.EnableColumnMenu = false;
            this.gvEmpleado.OptionsMenu.EnableFooterMenu = false;
            this.gvEmpleado.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvEmpleado.OptionsSelection.MultiSelect = true;
            this.gvEmpleado.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gvEmpleado.OptionsView.ColumnAutoWidth = false;
            this.gvEmpleado.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvEmpleado.OptionsView.ShowGroupPanel = false;
            this.gvEmpleado.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvEmpleado.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gvEmpleado_SelectionChanged);
            this.gvEmpleado.FocusedRowObjectChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventHandler(this.gvEmpleado_FocusedRowObjectChanged);
            // 
            // empleadoId
            // 
            this.empleadoId.Caption = "29001_EmpleadoID";
            this.empleadoId.FieldName = "ID";
            this.empleadoId.Name = "empleadoId";
            this.empleadoId.OptionsColumn.AllowEdit = false;
            this.empleadoId.OptionsColumn.AllowFocus = false;
            this.empleadoId.OptionsEditForm.Caption = "29001_EmpleadoID";
            this.empleadoId.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.empleadoId.Visible = true;
            this.empleadoId.VisibleIndex = 1;
            this.empleadoId.Width = 100;
            // 
            // empleado
            // 
            this.empleado.Caption = "29001_Empleado_Descriptivo";
            this.empleado.FieldName = "Descriptivo";
            this.empleado.Name = "empleado";
            this.empleado.OptionsColumn.AllowEdit = false;
            this.empleado.OptionsColumn.AllowFocus = false;
            this.empleado.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.empleado.Visible = true;
            this.empleado.VisibleIndex = 2;
            this.empleado.Width = 300;
            // 
            // repositoryItemCmbEstado
            // 
            this.repositoryItemCmbEstado.AutoHeight = false;
            this.repositoryItemCmbEstado.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCmbEstado.Name = "repositoryItemCmbEstado";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editCheckBox});
            // 
            // editCheckBox
            // 
            this.editCheckBox.Caption = "";
            this.editCheckBox.DisplayValueChecked = "True";
            this.editCheckBox.DisplayValueUnchecked = "False";
            this.editCheckBox.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editCheckBox.Name = "editCheckBox";
            this.editCheckBox.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(518, 155);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(102, 13);
            this.labelControl1.TabIndex = 12;
            this.labelControl1.Text = "29001_FechaIngreso";
            // 
            // dtFechaIngreso
            // 
            this.dtFechaIngreso.EditValue = null;
            this.dtFechaIngreso.Location = new System.Drawing.Point(654, 153);
            this.dtFechaIngreso.Name = "dtFechaIngreso";
            this.dtFechaIngreso.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIngreso.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIngreso.Properties.ReadOnly = true;
            this.dtFechaIngreso.Size = new System.Drawing.Size(114, 20);
            this.dtFechaIngreso.TabIndex = 11;
            // 
            // uc_AreaFuncional
            // 
            this.uc_AreaFuncional.BackColor = System.Drawing.Color.Transparent;
            this.uc_AreaFuncional.Filtros = null;
            this.uc_AreaFuncional.Location = new System.Drawing.Point(518, 3);
            this.uc_AreaFuncional.Name = "uc_AreaFuncional";
            this.uc_AreaFuncional.Size = new System.Drawing.Size(302, 25);
            this.uc_AreaFuncional.TabIndex = 10;
            this.uc_AreaFuncional.Value = "";
            // 
            // uc_Proyecto
            // 
            this.uc_Proyecto.BackColor = System.Drawing.Color.Transparent;
            this.uc_Proyecto.Filtros = null;
            this.uc_Proyecto.Location = new System.Drawing.Point(518, 80);
            this.uc_Proyecto.Name = "uc_Proyecto";
            this.uc_Proyecto.Size = new System.Drawing.Size(302, 25);
            this.uc_Proyecto.TabIndex = 9;
            this.uc_Proyecto.Value = "";
            // 
            // uc_Operacion
            // 
            this.uc_Operacion.BackColor = System.Drawing.Color.Transparent;
            this.uc_Operacion.Filtros = null;
            this.uc_Operacion.Location = new System.Drawing.Point(518, 29);
            this.uc_Operacion.Name = "uc_Operacion";
            this.uc_Operacion.Size = new System.Drawing.Size(302, 25);
            this.uc_Operacion.TabIndex = 8;
            this.uc_Operacion.Value = "";
            // 
            // uc_CentroCosto
            // 
            this.uc_CentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.uc_CentroCosto.Filtros = null;
            this.uc_CentroCosto.Location = new System.Drawing.Point(518, 54);
            this.uc_CentroCosto.Name = "uc_CentroCosto";
            this.uc_CentroCosto.Size = new System.Drawing.Size(302, 25);
            this.uc_CentroCosto.TabIndex = 7;
            this.uc_CentroCosto.Value = "";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(518, 191);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(69, 13);
            this.labelControl2.TabIndex = 13;
            this.labelControl2.Text = "29001_Estado";
            // 
            // txtEstado
            // 
            this.txtEstado.EditValue = "1";
            this.txtEstado.Location = new System.Drawing.Point(625, 188);
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.Properties.ReadOnly = true;
            this.txtEstado.Size = new System.Drawing.Size(23, 20);
            this.txtEstado.TabIndex = 14;
            // 
            // lkpEstadoDesc
            // 
            this.lkpEstadoDesc.Location = new System.Drawing.Point(654, 188);
            this.lkpEstadoDesc.Name = "lkpEstadoDesc";
            this.lkpEstadoDesc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpEstadoDesc.Properties.DropDownRows = 3;
            this.lkpEstadoDesc.Size = new System.Drawing.Size(115, 20);
            this.lkpEstadoDesc.TabIndex = 15;
            this.lkpEstadoDesc.SelectedIndexChanged += new System.EventHandler(this.lkpEstadoDesc_SelectedIndexChanged);
            // 
            // chkRetirosmes
            // 
            this.chkRetirosmes.EditValue = true;
            this.chkRetirosmes.Enabled = false;
            this.chkRetirosmes.Location = new System.Drawing.Point(775, 188);
            this.chkRetirosmes.Name = "chkRetirosmes";
            this.chkRetirosmes.Properties.Caption = "Mes";
            this.chkRetirosmes.Size = new System.Drawing.Size(55, 19);
            this.chkRetirosmes.TabIndex = 16;
            this.chkRetirosmes.Visible = false;
            // 
            // cmbTipoContrato
            // 
            this.cmbTipoContrato.Location = new System.Drawing.Point(654, 121);
            this.cmbTipoContrato.Name = "cmbTipoContrato";
            this.cmbTipoContrato.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoContrato.Properties.ReadOnly = true;
            this.cmbTipoContrato.Size = new System.Drawing.Size(115, 20);
            this.cmbTipoContrato.TabIndex = 76;
            // 
            // lblTipoContrato
            // 
            this.lblTipoContrato.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblTipoContrato.Location = new System.Drawing.Point(519, 124);
            this.lblTipoContrato.Name = "lblTipoContrato";
            this.lblTipoContrato.Size = new System.Drawing.Size(109, 13);
            this.lblTipoContrato.TabIndex = 77;
            this.lblTipoContrato.Text = "29001_lblTipoContrato";
            // 
            // UC_Empleados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbTipoContrato);
            this.Controls.Add(this.lblTipoContrato);
            this.Controls.Add(this.chkRetirosmes);
            this.Controls.Add(this.lkpEstadoDesc);
            this.Controls.Add(this.txtEstado);
            this.Controls.Add(this.gcEmpleado);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.uc_CentroCosto);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.dtFechaIngreso);
            this.Controls.Add(this.uc_Proyecto);
            this.Controls.Add(this.uc_Operacion);
            this.Controls.Add(this.uc_AreaFuncional);
            this.Name = "UC_Empleados";
            this.Size = new System.Drawing.Size(843, 247);
            ((System.ComponentModel.ISupportInitialize)(this.gcEmpleado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmpleado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCmbEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheckBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIngreso.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIngreso.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpEstadoDesc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkRetirosmes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoContrato.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private DevExpress.XtraGrid.GridControl gcEmpleado;
        private DevExpress.XtraGrid.Views.Grid.GridView gvEmpleado;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheckBox;
        private DevExpress.XtraGrid.Columns.GridColumn empleadoId;
        private DevExpress.XtraGrid.Columns.GridColumn empleado;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemCmbEstado;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dtFechaIngreso;
        private ControlsUC.uc_MasterFind uc_AreaFuncional;
        private ControlsUC.uc_MasterFind uc_Proyecto;
        private ControlsUC.uc_MasterFind uc_Operacion;
        private ControlsUC.uc_MasterFind uc_CentroCosto;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtEstado;
        private DevExpress.XtraEditors.ComboBoxEdit lkpEstadoDesc;
        private DevExpress.XtraEditors.CheckEdit chkRetirosmes;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoContrato;
        private DevExpress.XtraEditors.LabelControl lblTipoContrato;
    }
}
