namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Simulador
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
            this.masterRegimenEmp = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.gcResults = new DevExpress.XtraGrid.GridControl();
            this.gvResults = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.masterRegimenTer = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProveedor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLugarGeo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterBienServicio = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterConcCargo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLineaPresup = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtValue = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterOperacion = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gbResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // masterRegimenEmp
            // 
            this.masterRegimenEmp.BackColor = System.Drawing.Color.Transparent;
            this.masterRegimenEmp.Filtros = null;
            this.masterRegimenEmp.Location = new System.Drawing.Point(63, 22);
            this.masterRegimenEmp.Name = "masterRegimenEmp";
            this.masterRegimenEmp.Size = new System.Drawing.Size(291, 25);
            this.masterRegimenEmp.TabIndex = 0;
            this.masterRegimenEmp.Value = "";
            // 
            // gbResults
            // 
            this.gbResults.Controls.Add(this.gcResults);
            this.gbResults.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbResults.Location = new System.Drawing.Point(50, 313);
            this.gbResults.Name = "gbResults";
            this.gbResults.Padding = new System.Windows.Forms.Padding(12, 7, 25, 7);
            this.gbResults.Size = new System.Drawing.Size(736, 256);
            this.gbResults.TabIndex = 5;
            this.gbResults.TabStop = false;
            this.gbResults.Text = "20310_gbResults";
            // 
            // gcResults
            // 
            this.gcResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcResults.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcResults.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcResults.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcResults.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcResults.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcResults.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcResults.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcResults.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcResults.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcResults.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcResults.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcResults.Location = new System.Drawing.Point(12, 22);
            this.gcResults.LookAndFeel.SkinName = "Dark Side";
            this.gcResults.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcResults.MainView = this.gvResults;
            this.gcResults.Margin = new System.Windows.Forms.Padding(4);
            this.gcResults.Name = "gcResults";
            this.gcResults.Size = new System.Drawing.Size(699, 227);
            this.gcResults.TabIndex = 0;
            this.gcResults.UseEmbeddedNavigator = true;
            this.gcResults.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvResults});
            // 
            // gvResults
            // 
            this.gvResults.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvResults.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvResults.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvResults.Appearance.Empty.Options.UseBackColor = true;
            this.gvResults.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvResults.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvResults.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvResults.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvResults.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvResults.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvResults.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvResults.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvResults.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvResults.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvResults.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvResults.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvResults.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvResults.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvResults.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvResults.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvResults.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvResults.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvResults.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvResults.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvResults.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvResults.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvResults.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvResults.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvResults.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvResults.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvResults.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvResults.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvResults.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvResults.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvResults.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvResults.Appearance.Row.Options.UseBackColor = true;
            this.gvResults.Appearance.Row.Options.UseForeColor = true;
            this.gvResults.Appearance.Row.Options.UseTextOptions = true;
            this.gvResults.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvResults.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvResults.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvResults.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvResults.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvResults.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvResults.Appearance.VertLine.Options.UseBackColor = true;
            this.gvResults.GridControl = this.gcResults;
            this.gvResults.Name = "gvResults";
            this.gvResults.OptionsCustomization.AllowColumnMoving = false;
            this.gvResults.OptionsCustomization.AllowFilter = false;
            this.gvResults.OptionsCustomization.AllowSort = false;
            this.gvResults.OptionsMenu.EnableColumnMenu = false;
            this.gvResults.OptionsMenu.EnableFooterMenu = false;
            this.gvResults.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvResults.OptionsView.ColumnAutoWidth = false;
            this.gvResults.OptionsView.ShowGroupPanel = false;
            this.gvResults.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvResults_CustomRowCellEdit);
            this.gvResults.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvResults_CustomUnboundColumnData);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue});
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(58, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "20310_RegimenEmpresa";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(387, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "20310_RegimenTercero";
            // 
            // masterRegimenTer
            // 
            this.masterRegimenTer.BackColor = System.Drawing.Color.Transparent;
            this.masterRegimenTer.Filtros = null;
            this.masterRegimenTer.Location = new System.Drawing.Point(390, 44);
            this.masterRegimenTer.Name = "masterRegimenTer";
            this.masterRegimenTer.Size = new System.Drawing.Size(291, 25);
            this.masterRegimenTer.TabIndex = 2;
            this.masterRegimenTer.Value = "";
            this.masterRegimenTer.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterProveedor
            // 
            this.masterProveedor.BackColor = System.Drawing.Color.Transparent;
            this.masterProveedor.Filtros = null;
            this.masterProveedor.Location = new System.Drawing.Point(13, 17);
            this.masterProveedor.Name = "masterProveedor";
            this.masterProveedor.Size = new System.Drawing.Size(291, 25);
            this.masterProveedor.TabIndex = 0;
            this.masterProveedor.Value = "";
            this.masterProveedor.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(13, 44);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(291, 25);
            this.masterTercero.TabIndex = 1;
            this.masterTercero.Value = "";
            this.masterTercero.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterLugarGeo
            // 
            this.masterLugarGeo.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeo.Filtros = null;
            this.masterLugarGeo.Location = new System.Drawing.Point(13, 13);
            this.masterLugarGeo.Name = "masterLugarGeo";
            this.masterLugarGeo.Size = new System.Drawing.Size(291, 25);
            this.masterLugarGeo.TabIndex = 0;
            this.masterLugarGeo.Value = "";
            this.masterLugarGeo.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterBienServicio
            // 
            this.masterBienServicio.BackColor = System.Drawing.Color.Transparent;
            this.masterBienServicio.Filtros = null;
            this.masterBienServicio.Location = new System.Drawing.Point(13, 12);
            this.masterBienServicio.Name = "masterBienServicio";
            this.masterBienServicio.Size = new System.Drawing.Size(291, 25);
            this.masterBienServicio.TabIndex = 0;
            this.masterBienServicio.Value = "";
            this.masterBienServicio.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterConcCargo
            // 
            this.masterConcCargo.BackColor = System.Drawing.Color.Transparent;
            this.masterConcCargo.Filtros = null;
            this.masterConcCargo.Location = new System.Drawing.Point(390, 12);
            this.masterConcCargo.Name = "masterConcCargo";
            this.masterConcCargo.Size = new System.Drawing.Size(291, 25);
            this.masterConcCargo.TabIndex = 1;
            this.masterConcCargo.Value = "";
            this.masterConcCargo.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterLineaPresup
            // 
            this.masterLineaPresup.BackColor = System.Drawing.Color.Transparent;
            this.masterLineaPresup.Filtros = null;
            this.masterLineaPresup.Location = new System.Drawing.Point(390, 13);
            this.masterLineaPresup.Name = "masterLineaPresup";
            this.masterLineaPresup.Size = new System.Drawing.Size(291, 25);
            this.masterLineaPresup.TabIndex = 1;
            this.masterLineaPresup.Value = "";
            this.masterLineaPresup.Leave += new System.EventHandler(this.master_Leave);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.masterProveedor);
            this.groupBox5.Controls.Add(this.masterTercero);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.masterRegimenTer);
            this.groupBox5.Location = new System.Drawing.Point(50, 53);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(736, 78);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.masterLugarGeo);
            this.groupBox2.Controls.Add(this.masterLineaPresup);
            this.groupBox2.Location = new System.Drawing.Point(50, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(736, 43);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.masterBienServicio);
            this.groupBox1.Controls.Add(this.masterConcCargo);
            this.groupBox1.Location = new System.Drawing.Point(50, 175);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(736, 43);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.masterCentroCosto);
            this.groupBox3.Controls.Add(this.txtValue);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.masterProyecto);
            this.groupBox3.Controls.Add(this.masterOperacion);
            this.groupBox3.Location = new System.Drawing.Point(50, 218);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(736, 77);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(491, 45);
            this.txtValue.Name = "txtValue";
            this.txtValue.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValue.Properties.Mask.EditMask = "c";
            this.txtValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValue.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValue.Size = new System.Drawing.Size(99, 20);
            this.txtValue.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(387, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 14);
            this.label3.TabIndex = 11;
            this.label3.Text = "20310_txtValor";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(13, 12);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(291, 25);
            this.masterProyecto.TabIndex = 0;
            this.masterProyecto.Value = "";
            this.masterProyecto.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterOperacion
            // 
            this.masterOperacion.BackColor = System.Drawing.Color.Transparent;
            this.masterOperacion.Filtros = null;
            this.masterOperacion.Location = new System.Drawing.Point(390, 12);
            this.masterOperacion.Name = "masterOperacion";
            this.masterOperacion.Size = new System.Drawing.Size(291, 25);
            this.masterOperacion.TabIndex = 2;
            this.masterOperacion.Value = "";
            this.masterOperacion.Leave += new System.EventHandler(this.master_Leave);
            // 
            // masterCentroCosto
            // 
            this.masterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCosto.Filtros = null;
            this.masterCentroCosto.Location = new System.Drawing.Point(13, 42);
            this.masterCentroCosto.Name = "masterCentroCosto";
            this.masterCentroCosto.Size = new System.Drawing.Size(291, 25);
            this.masterCentroCosto.TabIndex = 1;
            this.masterCentroCosto.Value = "";
            this.masterCentroCosto.Leave += new System.EventHandler(this.master_Leave);
            // 
            // Simulador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(827, 596);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbResults);
            this.Controls.Add(this.masterRegimenEmp);
            this.Name = "Simulador";
            this.Text = "20310";
            this.gbResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlsUC.uc_MasterFind masterRegimenEmp;
        private System.Windows.Forms.GroupBox gbResults;
        private DevExpress.XtraGrid.GridControl gcResults;
        private DevExpress.XtraGrid.Views.Grid.GridView gvResults;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ControlsUC.uc_MasterFind masterRegimenTer;
        private ControlsUC.uc_MasterFind masterProveedor;
        private ControlsUC.uc_MasterFind masterTercero;
        private ControlsUC.uc_MasterFind masterLugarGeo;
        private ControlsUC.uc_MasterFind masterBienServicio;
        private ControlsUC.uc_MasterFind masterConcCargo;
        private ControlsUC.uc_MasterFind masterLineaPresup;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterOperacion;
        private DevExpress.XtraEditors.TextEdit txtValue;
        private ControlsUC.uc_MasterFind masterCentroCosto;

    }
}