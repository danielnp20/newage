namespace NewAge.Forms
{
    partial class FindActivosModal
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcContent = new DevExpress.XtraGrid.GridControl();
            this.gvContent = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.chkSelectAll = new DevExpress.XtraEditors.CheckEdit();
            this.uc_Paginador = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSerial = new DevExpress.XtraEditors.LabelControl();
            this.txtSerial = new DevExpress.XtraEditors.TextEdit();
            this.uc_Tipo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_Clase = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_Grupo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_CentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_Proyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkContenedor = new DevExpress.XtraEditors.CheckEdit();
            this.uc_Referencia = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_LocFisica = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblResponsable = new DevExpress.XtraEditors.LabelControl();
            this.lblPlaqueta = new DevExpress.XtraEditors.LabelControl();
            this.txtPlaqueta = new DevExpress.XtraEditors.TextEdit();
            this.uc_Responsable = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.persistentRepository = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editChek = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btnCargar = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSerial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkContenedor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlaqueta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChek)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gcContent);
            this.groupBox1.Controls.Add(this.chkSelectAll);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(944, 278);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // gcContent
            // 
            this.gcContent.AllowDrop = true;
            this.gcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcContent.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcContent.Location = new System.Drawing.Point(3, 35);
            this.gcContent.LookAndFeel.SkinName = "Dark Side";
            this.gcContent.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcContent.MainView = this.gvContent;
            this.gcContent.Margin = new System.Windows.Forms.Padding(4);
            this.gcContent.Name = "gcContent";
            this.gcContent.Size = new System.Drawing.Size(938, 240);
            this.gcContent.TabIndex = 0;
            this.gcContent.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvContent});
            // 
            // gvContent
            // 
            this.gvContent.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvContent.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvContent.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvContent.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvContent.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvContent.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvContent.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvContent.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvContent.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvContent.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvContent.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvContent.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvContent.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvContent.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvContent.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvContent.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvContent.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvContent.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvContent.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvContent.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvContent.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvContent.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvContent.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvContent.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvContent.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvContent.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvContent.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvContent.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvContent.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvContent.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvContent.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvContent.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvContent.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvContent.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvContent.Appearance.Row.Options.UseBackColor = true;
            this.gvContent.Appearance.Row.Options.UseForeColor = true;
            this.gvContent.Appearance.Row.Options.UseTextOptions = true;
            this.gvContent.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvContent.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvContent.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvContent.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvContent.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvContent.Appearance.VertLine.Options.UseBackColor = true;
            this.gvContent.GridControl = this.gcContent;
            this.gvContent.HorzScrollStep = 50;
            this.gvContent.Name = "gvContent";
            this.gvContent.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvContent.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvContent.OptionsCustomization.AllowColumnMoving = false;
            this.gvContent.OptionsCustomization.AllowFilter = false;
            this.gvContent.OptionsCustomization.AllowSort = false;
            this.gvContent.OptionsMenu.EnableColumnMenu = false;
            this.gvContent.OptionsMenu.EnableFooterMenu = false;
            this.gvContent.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvContent.OptionsView.ColumnAutoWidth = false;
            this.gvContent.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvContent.OptionsView.ShowGroupPanel = false;
            this.gvContent.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvContent.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvContent_CustomRowCellEdit);
            this.gvContent.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvContent_FocusedRowChanged);
            this.gvContent.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvContent_CustomUnboundColumnData);
            this.gvContent.DoubleClick += new System.EventHandler(this.gvContent_DoubleClick);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkSelectAll.Location = new System.Drawing.Point(3, 16);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Properties.Caption = "23310_chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(938, 19);
            this.chkSelectAll.TabIndex = 8;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // uc_Paginador
            // 
            this.uc_Paginador.Count = ((long)(0));
            this.uc_Paginador.Location = new System.Drawing.Point(278, 281);
            this.uc_Paginador.Name = "uc_Paginador";
            this.uc_Paginador.PageCount = 0;
            this.uc_Paginador.PageNumber = 0;
            this.uc_Paginador.PageSize = 0;
            this.uc_Paginador.Size = new System.Drawing.Size(406, 26);
            this.uc_Paginador.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblSerial);
            this.groupBox2.Controls.Add(this.txtSerial);
            this.groupBox2.Controls.Add(this.uc_Tipo);
            this.groupBox2.Controls.Add(this.uc_Clase);
            this.groupBox2.Controls.Add(this.uc_Grupo);
            this.groupBox2.Controls.Add(this.uc_CentroCosto);
            this.groupBox2.Controls.Add(this.uc_Proyecto);
            this.groupBox2.Controls.Add(this.chkContenedor);
            this.groupBox2.Controls.Add(this.uc_Referencia);
            this.groupBox2.Controls.Add(this.uc_LocFisica);
            this.groupBox2.Controls.Add(this.lblResponsable);
            this.groupBox2.Controls.Add(this.lblPlaqueta);
            this.groupBox2.Controls.Add(this.txtPlaqueta);
            this.groupBox2.Controls.Add(this.uc_Responsable);
            this.groupBox2.Location = new System.Drawing.Point(13, 304);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(943, 119);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // lblSerial
            // 
            this.lblSerial.Location = new System.Drawing.Point(265, 19);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(72, 13);
            this.lblSerial.TabIndex = 14;
            this.lblSerial.Text = "23310_lblSerial";
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(369, 16);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(118, 20);
            this.txtSerial.TabIndex = 13;
            // 
            // uc_Tipo
            // 
            this.uc_Tipo.BackColor = System.Drawing.Color.Transparent;
            this.uc_Tipo.Filtros = null;
            this.uc_Tipo.Location = new System.Drawing.Point(638, 43);
            this.uc_Tipo.Name = "uc_Tipo";
            this.uc_Tipo.Size = new System.Drawing.Size(291, 25);
            this.uc_Tipo.TabIndex = 12;
            this.uc_Tipo.Value = "";
            // 
            // uc_Clase
            // 
            this.uc_Clase.BackColor = System.Drawing.Color.Transparent;
            this.uc_Clase.Filtros = null;
            this.uc_Clase.Location = new System.Drawing.Point(322, 43);
            this.uc_Clase.Name = "uc_Clase";
            this.uc_Clase.Size = new System.Drawing.Size(291, 25);
            this.uc_Clase.TabIndex = 11;
            this.uc_Clase.Value = "";
            // 
            // uc_Grupo
            // 
            this.uc_Grupo.BackColor = System.Drawing.Color.Transparent;
            this.uc_Grupo.Filtros = null;
            this.uc_Grupo.Location = new System.Drawing.Point(322, 67);
            this.uc_Grupo.Name = "uc_Grupo";
            this.uc_Grupo.Size = new System.Drawing.Size(291, 25);
            this.uc_Grupo.TabIndex = 10;
            this.uc_Grupo.Value = "";
            // 
            // uc_CentroCosto
            // 
            this.uc_CentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.uc_CentroCosto.Filtros = null;
            this.uc_CentroCosto.Location = new System.Drawing.Point(12, 66);
            this.uc_CentroCosto.Name = "uc_CentroCosto";
            this.uc_CentroCosto.Size = new System.Drawing.Size(291, 25);
            this.uc_CentroCosto.TabIndex = 9;
            this.uc_CentroCosto.Value = "";
            // 
            // uc_Proyecto
            // 
            this.uc_Proyecto.BackColor = System.Drawing.Color.Transparent;
            this.uc_Proyecto.Filtros = null;
            this.uc_Proyecto.Location = new System.Drawing.Point(12, 43);
            this.uc_Proyecto.Name = "uc_Proyecto";
            this.uc_Proyecto.Size = new System.Drawing.Size(291, 25);
            this.uc_Proyecto.TabIndex = 8;
            this.uc_Proyecto.Value = "";
            // 
            // chkContenedor
            // 
            this.chkContenedor.Location = new System.Drawing.Point(534, 17);
            this.chkContenedor.Name = "chkContenedor";
            this.chkContenedor.Properties.Caption = "23310_EsContendor";
            this.chkContenedor.Size = new System.Drawing.Size(120, 19);
            this.chkContenedor.TabIndex = 7;
            // 
            // uc_Referencia
            // 
            this.uc_Referencia.BackColor = System.Drawing.Color.Transparent;
            this.uc_Referencia.Filtros = null;
            this.uc_Referencia.Location = new System.Drawing.Point(322, 91);
            this.uc_Referencia.Name = "uc_Referencia";
            this.uc_Referencia.Size = new System.Drawing.Size(291, 25);
            this.uc_Referencia.TabIndex = 5;
            this.uc_Referencia.Value = "";
            // 
            // uc_LocFisica
            // 
            this.uc_LocFisica.BackColor = System.Drawing.Color.Transparent;
            this.uc_LocFisica.Filtros = null;
            this.uc_LocFisica.Location = new System.Drawing.Point(12, 91);
            this.uc_LocFisica.Name = "uc_LocFisica";
            this.uc_LocFisica.Size = new System.Drawing.Size(291, 25);
            this.uc_LocFisica.TabIndex = 4;
            this.uc_LocFisica.Value = "";
            // 
            // lblResponsable
            // 
            this.lblResponsable.Location = new System.Drawing.Point(638, 74);
            this.lblResponsable.Name = "lblResponsable";
            this.lblResponsable.Size = new System.Drawing.Size(107, 13);
            this.lblResponsable.TabIndex = 3;
            this.lblResponsable.Text = "23310_lblResponsable";
            // 
            // lblPlaqueta
            // 
            this.lblPlaqueta.Location = new System.Drawing.Point(12, 19);
            this.lblPlaqueta.Name = "lblPlaqueta";
            this.lblPlaqueta.Size = new System.Drawing.Size(88, 13);
            this.lblPlaqueta.TabIndex = 2;
            this.lblPlaqueta.Text = "23310_lblPlaqueta";
            // 
            // txtPlaqueta
            // 
            this.txtPlaqueta.Location = new System.Drawing.Point(116, 16);
            this.txtPlaqueta.Name = "txtPlaqueta";
            this.txtPlaqueta.Size = new System.Drawing.Size(118, 20);
            this.txtPlaqueta.TabIndex = 0;
            // 
            // uc_Responsable
            // 
            this.uc_Responsable.BackColor = System.Drawing.Color.Transparent;
            this.uc_Responsable.Filtros = null;
            this.uc_Responsable.Location = new System.Drawing.Point(638, 67);
            this.uc_Responsable.Name = "uc_Responsable";
            this.uc_Responsable.Size = new System.Drawing.Size(291, 25);
            this.uc_Responsable.TabIndex = 15;
            this.uc_Responsable.Value = "";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(838, 426);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(118, 23);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "23310_btnBuscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // persistentRepository
            // 
            this.persistentRepository.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChek});
            // 
            // editChek
            // 
            this.editChek.Caption = "";
            this.editChek.DisplayValueChecked = "True";
            this.editChek.DisplayValueUnchecked = "False";
            this.editChek.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editChek.Name = "editChek";
            this.editChek.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // btnCargar
            // 
            this.btnCargar.Location = new System.Drawing.Point(714, 426);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(118, 23);
            this.btnCargar.TabIndex = 7;
            this.btnCargar.Text = "23310_btnCargar";
            this.btnCargar.UseVisualStyleBackColor = true;
            this.btnCargar.Visible = false;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            // 
            // FindActivosModal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 456);
            this.Controls.Add(this.btnCargar);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.uc_Paginador);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindActivosModal";
            this.Text = "FindActivosModal";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSerial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkContenedor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlaqueta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChek)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcContent;
        private DevExpress.XtraGrid.Views.Grid.GridView gvContent;
        private Cliente.GUI.WinApp.ControlsUC.uc_Pagging uc_Paginador;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.LabelControl lblResponsable;
        private DevExpress.XtraEditors.LabelControl lblPlaqueta;
        private DevExpress.XtraEditors.TextEdit txtPlaqueta;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_Referencia;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_LocFisica;
        private System.Windows.Forms.Button btnBuscar;
        private DevExpress.XtraEditors.CheckEdit chkContenedor;
        private DevExpress.XtraEditors.Repository.PersistentRepository persistentRepository;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChek;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_Tipo;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_Clase;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_Grupo;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_CentroCosto;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_Proyecto;
        private DevExpress.XtraEditors.CheckEdit chkSelectAll;
        private DevExpress.XtraEditors.LabelControl lblSerial;
        private DevExpress.XtraEditors.TextEdit txtSerial;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_Responsable;
        private System.Windows.Forms.Button btnCargar;
    }
}