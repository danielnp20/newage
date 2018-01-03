namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaDocumentosCxP
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
            this.lbl_tipoConsulta = new System.Windows.Forms.Label();
            this.lkp_TipoConsulta = new DevExpress.XtraEditors.LookUpEdit();
            this.lbltipoFactura = new System.Windows.Forms.Label();
            this.lkpTipoFactura = new DevExpress.XtraEditors.LookUpEdit();
            this.uc_Periodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtNroFactura = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_RegExTextBox();
            this.lblFacturaNro = new System.Windows.Forms.Label();
            this.lbl_Periodo = new System.Windows.Forms.Label();
            this.masterConceptoCxP = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gbHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoConsulta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoFactura.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gbHeader
            // 
            this.gbHeader.Controls.Add(this.masterConceptoCxP);
            this.gbHeader.Controls.Add(this.lbl_tipoConsulta);
            this.gbHeader.Controls.Add(this.lkp_TipoConsulta);
            this.gbHeader.Controls.Add(this.lbltipoFactura);
            this.gbHeader.Controls.Add(this.lkpTipoFactura);
            this.gbHeader.Controls.Add(this.uc_Periodo);
            this.gbHeader.Controls.Add(this.masterTercero);
            this.gbHeader.Controls.Add(this.txtNroFactura);
            this.gbHeader.Controls.Add(this.lblFacturaNro);
            this.gbHeader.Controls.Add(this.lbl_Periodo);
            this.gbHeader.Size = new System.Drawing.Size(1019, 153);
            // 
            // gbGrid
            // 
            this.gbGrid.Size = new System.Drawing.Size(1019, 334);
            // 
            // repositoryEdit
            // 
            this.repositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.linkEditViewFile,
            this.TextEdit});
            // 
            // TextEdit
            // 
            this.TextEdit.Mask.EditMask = "c2";
            this.TextEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.TextEdit.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lbl_tipoConsulta
            // 
            this.lbl_tipoConsulta.AutoSize = true;
            this.lbl_tipoConsulta.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_tipoConsulta.Location = new System.Drawing.Point(16, 22);
            this.lbl_tipoConsulta.Name = "lbl_tipoConsulta";
            this.lbl_tipoConsulta.Size = new System.Drawing.Size(137, 14);
            this.lbl_tipoConsulta.TabIndex = 8;
            this.lbl_tipoConsulta.Text = "21310_lbl_TipoConsulta";
            // 
            // lkp_TipoConsulta
            // 
            this.lkp_TipoConsulta.Location = new System.Drawing.Point(120, 19);
            this.lkp_TipoConsulta.Name = "lkp_TipoConsulta";
            this.lkp_TipoConsulta.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lkp_TipoConsulta.Properties.Appearance.Options.UseFont = true;
            this.lkp_TipoConsulta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_TipoConsulta.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkp_TipoConsulta.Properties.DisplayMember = "Value";
            this.lkp_TipoConsulta.Properties.NullText = " ";
            this.lkp_TipoConsulta.Properties.ValueMember = "Key";
            this.lkp_TipoConsulta.Size = new System.Drawing.Size(101, 20);
            this.lkp_TipoConsulta.TabIndex = 7;
            this.lkp_TipoConsulta.EditValueChanged += new System.EventHandler(this.lkp_TipoConsulta_EditValueChanged);
            // 
            // lbltipoFactura
            // 
            this.lbltipoFactura.AutoSize = true;
            this.lbltipoFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltipoFactura.Location = new System.Drawing.Point(326, 52);
            this.lbltipoFactura.Name = "lbltipoFactura";
            this.lbltipoFactura.Size = new System.Drawing.Size(131, 14);
            this.lbltipoFactura.TabIndex = 6;
            this.lbltipoFactura.Text = "21310_lbl_TipoFactura";
            // 
            // lkpTipoFactura
            // 
            this.lkpTipoFactura.Location = new System.Drawing.Point(436, 49);
            this.lkpTipoFactura.Name = "lkpTipoFactura";
            this.lkpTipoFactura.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lkpTipoFactura.Properties.Appearance.Options.UseFont = true;
            this.lkpTipoFactura.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpTipoFactura.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkpTipoFactura.Properties.DisplayMember = "Value";
            this.lkpTipoFactura.Properties.NullText = " ";
            this.lkpTipoFactura.Properties.ValueMember = "Key";
            this.lkpTipoFactura.Size = new System.Drawing.Size(136, 20);
            this.lkpTipoFactura.TabIndex = 5;
            this.lkpTipoFactura.EditValueChanged += new System.EventHandler(this.lkp_TipoFactura_EditValueChanged);
            // 
            // uc_Periodo
            // 
            this.uc_Periodo.BackColor = System.Drawing.Color.Transparent;
            this.uc_Periodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.uc_Periodo.EnabledControl = true;
            this.uc_Periodo.ExtraPeriods = 0;
            this.uc_Periodo.Location = new System.Drawing.Point(356, 18);
            this.uc_Periodo.MaxValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.MinValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.Name = "uc_Periodo";
            this.uc_Periodo.Size = new System.Drawing.Size(130, 20);
            this.uc_Periodo.TabIndex = 1;
            // 
            // uc_MasterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Location = new System.Drawing.Point(19, 47);
            this.masterTercero.Name = "uc_MasterTercero";
            this.masterTercero.Size = new System.Drawing.Size(305, 25);
            this.masterTercero.TabIndex = 2;
            this.masterTercero.Value = "";
            // 
            // txtNroFactura
            // 
            this.txtNroFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroFactura.Location = new System.Drawing.Point(662, 48);
            this.txtNroFactura.Name = "txtNroFactura";
            this.txtNroFactura.Regular_Expression = null;
            this.txtNroFactura.Size = new System.Drawing.Size(63, 22);
            this.txtNroFactura.TabIndex = 3;
            // 
            // lblFacturaNro
            // 
            this.lblFacturaNro.AutoSize = true;
            this.lblFacturaNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFacturaNro.Location = new System.Drawing.Point(582, 52);
            this.lblFacturaNro.Name = "lblFacturaNro";
            this.lblFacturaNro.Size = new System.Drawing.Size(126, 14);
            this.lblFacturaNro.TabIndex = 4;
            this.lblFacturaNro.Text = "21310_lbl_FacturaNro";
            // 
            // lbl_Periodo
            // 
            this.lbl_Periodo.AutoSize = true;
            this.lbl_Periodo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Periodo.Location = new System.Drawing.Point(246, 22);
            this.lbl_Periodo.Name = "lbl_Periodo";
            this.lbl_Periodo.Size = new System.Drawing.Size(108, 14);
            this.lbl_Periodo.TabIndex = 1;
            this.lbl_Periodo.Text = "21310_lbl_Periodo";
            // 
            // masterConceptoCxP
            // 
            this.masterConceptoCxP.BackColor = System.Drawing.Color.Transparent;
            this.masterConceptoCxP.Filtros = null;
            this.masterConceptoCxP.Location = new System.Drawing.Point(18, 74);
            this.masterConceptoCxP.Name = "masterConceptoCxP";
            this.masterConceptoCxP.Size = new System.Drawing.Size(305, 25);
            this.masterConceptoCxP.TabIndex = 9;
            this.masterConceptoCxP.Value = "";
            // 
            // ConsultaDocumentosCxP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 417);
            this.Name = "ConsultaDocumentosCxP";
            this.Text = "21310";
            this.gbHeader.ResumeLayout(false);
            this.gbHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.linkEditViewFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoConsulta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoFactura.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cliente.GUI.WinApp.ControlsUC.uc_RegExTextBox txtNroFactura;
        private System.Windows.Forms.Label lblFacturaNro;
        private System.Windows.Forms.Label lbl_Periodo;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind masterTercero;
        private ControlsUC.uc_PeriodoEdit uc_Periodo;
        private System.Windows.Forms.Label lbltipoFactura;
        private DevExpress.XtraEditors.LookUpEdit lkpTipoFactura;
        private System.Windows.Forms.Label lbl_tipoConsulta;
        private DevExpress.XtraEditors.LookUpEdit lkp_TipoConsulta;
        private ControlsUC.uc_MasterFind masterConceptoCxP;
    }
}