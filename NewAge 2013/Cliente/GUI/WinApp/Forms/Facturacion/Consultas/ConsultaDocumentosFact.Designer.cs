namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaDocumentosFact
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.chkFacturaFija = new DevExpress.XtraEditors.CheckEdit();
            this.pnConsulta = new DevExpress.XtraEditors.PanelControl();
            this.lbl_tipoConsulta = new System.Windows.Forms.Label();
            this.lkp_TipoFactura = new DevExpress.XtraEditors.LookUpEdit();
            this.lbl_tipoFactura = new System.Windows.Forms.Label();
            this.lkp_TipoConsulta = new DevExpress.XtraEditors.LookUpEdit();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtFacturaVtaNro = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_RegExTextBox();
            this.lblFacturaVtaNro = new System.Windows.Forms.Label();
            this.masterZona = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterAsesor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_Periodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lbl_Periodo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            this.gbHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkFacturaFija.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnConsulta)).BeginInit();
            this.pnConsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoConsulta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).BeginInit();
            this.gbGrid.SuspendLayout();
            this.SuspendLayout();
             // 
            // chkFacturaFija
            // 
            this.chkFacturaFija.Location = new System.Drawing.Point(932, 106);
            this.chkFacturaFija.Name = "chkFacturaFija";
            this.chkFacturaFija.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkFacturaFija.Properties.Appearance.Options.UseFont = true;
            this.chkFacturaFija.Properties.Caption = "28310_chkFacturaFija";
            this.chkFacturaFija.Size = new System.Drawing.Size(142, 19);
            this.chkFacturaFija.TabIndex = 19;
            // 
            // pnConsulta
            // 
            this.pnConsulta.Controls.Add(this.lbl_tipoConsulta);
            this.pnConsulta.Controls.Add(this.lkp_TipoFactura);
            this.pnConsulta.Controls.Add(this.lbl_tipoFactura);
            this.pnConsulta.Controls.Add(this.lkp_TipoConsulta);
            this.pnConsulta.Location = new System.Drawing.Point(20, 13);
            this.pnConsulta.Name = "pnConsulta";
            this.pnConsulta.Size = new System.Drawing.Size(596, 35);
            this.pnConsulta.TabIndex = 18;
            // 
            // lbl_tipoConsulta
            // 
            this.lbl_tipoConsulta.AutoSize = true;
            this.lbl_tipoConsulta.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_tipoConsulta.Location = new System.Drawing.Point(5, 11);
            this.lbl_tipoConsulta.Name = "lbl_tipoConsulta";
            this.lbl_tipoConsulta.Size = new System.Drawing.Size(137, 14);
            this.lbl_tipoConsulta.TabIndex = 8;
            this.lbl_tipoConsulta.Text = "28310_lbl_TipoConsulta";
            // 
            // lkp_TipoFactura
            // 
            this.lkp_TipoFactura.Location = new System.Drawing.Point(403, 8);
            this.lkp_TipoFactura.Name = "lkp_TipoFactura";
            this.lkp_TipoFactura.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lkp_TipoFactura.Properties.Appearance.Options.UseFont = true;
            this.lkp_TipoFactura.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_TipoFactura.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkp_TipoFactura.Properties.DisplayMember = "Value";
            this.lkp_TipoFactura.Properties.ValueMember = "Key";
            this.lkp_TipoFactura.Size = new System.Drawing.Size(100, 20);
            this.lkp_TipoFactura.TabIndex = 5;
            this.lkp_TipoFactura.EditValueChanged += new System.EventHandler(this.lkp_TipoFactura_EditValueChanged);
            // 
            // lbl_tipoFactura
            // 
            this.lbl_tipoFactura.AutoSize = true;
            this.lbl_tipoFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_tipoFactura.Location = new System.Drawing.Point(305, 11);
            this.lbl_tipoFactura.Name = "lbl_tipoFactura";
            this.lbl_tipoFactura.Size = new System.Drawing.Size(131, 14);
            this.lbl_tipoFactura.TabIndex = 6;
            this.lbl_tipoFactura.Text = "28310_lbl_TipoFactura";
            // 
            // lkp_TipoConsulta
            // 
            this.lkp_TipoConsulta.Location = new System.Drawing.Point(110, 8);
            this.lkp_TipoConsulta.Name = "lkp_TipoConsulta";
            this.lkp_TipoConsulta.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lkp_TipoConsulta.Properties.Appearance.Options.UseFont = true;
            this.lkp_TipoConsulta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_TipoConsulta.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkp_TipoConsulta.Properties.DisplayMember = "Value";
            this.lkp_TipoConsulta.Properties.ValueMember = "Key";
            this.lkp_TipoConsulta.Size = new System.Drawing.Size(142, 20);
            this.lkp_TipoConsulta.TabIndex = 7;
            this.lkp_TipoConsulta.EditValueChanged += new System.EventHandler(this.lkp_TipoConsulta_EditValueChanged);
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(20, 77);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(301, 27);
            this.masterPrefijo.TabIndex = 17;
            this.masterPrefijo.Value = "";
            // 
            // txtFacturaVtaNro
            // 
            this.txtFacturaVtaNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFacturaVtaNro.Location = new System.Drawing.Point(422, 79);
            this.txtFacturaVtaNro.Name = "txtFacturaVtaNro";
            this.txtFacturaVtaNro.Regular_Expression = null;
            this.txtFacturaVtaNro.Size = new System.Drawing.Size(45, 22);
            this.txtFacturaVtaNro.TabIndex = 13;
            // 
            // lblFacturaVtaNro
            // 
            this.lblFacturaVtaNro.AutoSize = true;
            this.lblFacturaVtaNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFacturaVtaNro.Location = new System.Drawing.Point(321, 83);
            this.lblFacturaVtaNro.Name = "lblFacturaVtaNro";
            this.lblFacturaVtaNro.Size = new System.Drawing.Size(126, 14);
            this.lblFacturaVtaNro.TabIndex = 14;
            this.lblFacturaVtaNro.Text = "28310_lbl_FacturaNro";
            // 
            // masterZona
            // 
            this.masterZona.BackColor = System.Drawing.Color.Transparent;
            this.masterZona.Filtros = null;
            this.masterZona.Location = new System.Drawing.Point(322, 103);
            this.masterZona.Name = "masterZona";
            this.masterZona.Size = new System.Drawing.Size(300, 25);
            this.masterZona.TabIndex = 12;
            this.masterZona.Value = "";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(631, 103);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(302, 25);
            this.masterProyecto.TabIndex = 10;
            this.masterProyecto.Value = "";
            // 
            // masterAsesor
            // 
            this.masterAsesor.BackColor = System.Drawing.Color.Transparent;
            this.masterAsesor.Filtros = null;
            this.masterAsesor.Location = new System.Drawing.Point(19, 102);
            this.masterAsesor.Name = "masterAsesor";
            this.masterAsesor.Size = new System.Drawing.Size(301, 25);
            this.masterAsesor.TabIndex = 9;
            this.masterAsesor.Value = "";
            // 
            // uc_Periodo
            // 
            this.uc_Periodo.BackColor = System.Drawing.Color.Transparent;
            this.uc_Periodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.uc_Periodo.EnabledControl = true;
            this.uc_Periodo.ExtraPeriods = 0;
            this.uc_Periodo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uc_Periodo.Location = new System.Drawing.Point(119, 55);
            this.uc_Periodo.MaxValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.MinValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.Name = "uc_Periodo";
            this.uc_Periodo.Size = new System.Drawing.Size(143, 20);
            this.uc_Periodo.TabIndex = 1;
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(323, 53);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(319, 25);
            this.masterCliente.TabIndex = 2;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // lbl_Periodo
            // 
            this.lbl_Periodo.AutoSize = true;
            this.lbl_Periodo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Periodo.Location = new System.Drawing.Point(17, 59);
            this.lbl_Periodo.Name = "lbl_Periodo";
            this.lbl_Periodo.Size = new System.Drawing.Size(108, 14);
            this.lbl_Periodo.TabIndex = 1;
            this.lbl_Periodo.Text = "28310_lbl_Periodo"; 
           ///
            this.gbHeader.Controls.Add(this.chkFacturaFija);
            this.gbHeader.Controls.Add(this.pnConsulta);
            this.gbHeader.Controls.Add(this.masterPrefijo);
            this.gbHeader.Controls.Add(this.txtFacturaVtaNro);
            this.gbHeader.Controls.Add(this.lblFacturaVtaNro);
            this.gbHeader.Controls.Add(this.masterZona);
            this.gbHeader.Controls.Add(this.masterProyecto);
            this.gbHeader.Controls.Add(this.masterAsesor);
            this.gbHeader.Controls.Add(this.uc_Periodo);
            this.gbHeader.Controls.Add(this.masterCliente);
            this.gbHeader.Controls.Add(this.lbl_Periodo);
            // 
            // ConsultaDocumentosFact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 546);
            this.Controls.Add(this.gbGrid);
            this.Controls.Add(this.gbHeader);
            this.Name = "ConsultaDocumentosFact";
            this.Text = "28310";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            this.gbHeader.ResumeLayout(false);
            this.gbHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkFacturaFija.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnConsulta)).EndInit();
            this.pnConsulta.ResumeLayout(false);
            this.pnConsulta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoConsulta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).EndInit();
            this.gbGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Periodo;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind masterCliente;
        private ControlsUC.uc_PeriodoEdit uc_Periodo;
        private System.Windows.Forms.Label lbl_tipoFactura;
        private DevExpress.XtraEditors.LookUpEdit lkp_TipoFactura;
        private System.Windows.Forms.Label lbl_tipoConsulta;
        private DevExpress.XtraEditors.LookUpEdit lkp_TipoConsulta;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterAsesor;
        private ControlsUC.uc_MasterFind masterZona;
        private ControlsUC.uc_RegExTextBox txtFacturaVtaNro;
        private System.Windows.Forms.Label lblFacturaVtaNro;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.PanelControl pnConsulta;
        private DevExpress.XtraEditors.CheckEdit chkFacturaFija;
    }
}