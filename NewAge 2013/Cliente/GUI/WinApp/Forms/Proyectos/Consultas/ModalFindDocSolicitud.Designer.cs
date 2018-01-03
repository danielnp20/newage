namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalFindDocSolicitud
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
            this.cmbTipoSol = new DevExpress.XtraEditors.LookUpEdit();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblTipoSol = new System.Windows.Forms.Label();
            this.masterClaseServicio = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblEmpresa = new System.Windows.Forms.Label();
            this.txtEmpresaNombre = new DevExpress.XtraEditors.TextEdit();
            this.txtLicitacion = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).BeginInit();
            this.pnGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnDocument)).BeginInit();
            this.pnDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnGrid)).BeginInit();
            this.pnGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGeneral)).BeginInit();
            this.gbFilterGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterByDocumento)).BeginInit();
            this.gbFilterByDocumento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoSol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpresaNombre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dtFechaInicial
            // 
            this.dtFechaInicial.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicial.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaInicial.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaInicial.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicial.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaInicial.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtFechaFinal
            // 
            this.dtFechaFinal.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinal.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinal.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinal.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtDocumentoNro
            // 
            this.txtDocumentoNro.Margin = new System.Windows.Forms.Padding(7, 2, 3, 2);
            // 
            // txtDocTercero
            // 
            this.txtDocTercero.Margin = new System.Windows.Forms.Padding(7, 2, 3, 2);
            // 
            // pnGeneral
            // 
            this.pnGeneral.Size = new System.Drawing.Size(883, 179);
            // 
            // pnGrid
            // 
            this.pnGrid.Size = new System.Drawing.Size(883, 237);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkSelectAll.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.chkSelectAll.Properties.Appearance.Options.UseFont = true;
            this.chkSelectAll.Size = new System.Drawing.Size(859, 19);
            // 
            // gbFilterGeneral
            // 
            this.gbFilterGeneral.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterGeneral.Appearance.Options.UseFont = true;
            this.gbFilterGeneral.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterGeneral.AppearanceCaption.Options.UseFont = true;
            this.gbFilterGeneral.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbFilterGeneral.Size = new System.Drawing.Size(534, 179);
            // 
            // gbFilterByDocumento
            // 
            this.gbFilterByDocumento.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilterByDocumento.Appearance.Options.UseFont = true;
            this.gbFilterByDocumento.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilterByDocumento.AppearanceCaption.Options.UseFont = true;
            this.gbFilterByDocumento.Controls.Add(this.label1);
            this.gbFilterByDocumento.Controls.Add(this.txtLicitacion);
            this.gbFilterByDocumento.Controls.Add(this.txtEmpresaNombre);
            this.gbFilterByDocumento.Controls.Add(this.cmbTipoSol);
            this.gbFilterByDocumento.Controls.Add(this.lblEmpresa);
            this.gbFilterByDocumento.Controls.Add(this.masterClaseServicio);
            this.gbFilterByDocumento.Controls.Add(this.lblTipoSol);
            this.gbFilterByDocumento.Controls.Add(this.masterCliente);
            this.gbFilterByDocumento.Location = new System.Drawing.Point(534, 0);
            this.gbFilterByDocumento.Size = new System.Drawing.Size(349, 179);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editSpinPorcen});
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
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // cmbEstate
            // 
            // 
            // cmbTipoSol
            // 
            this.cmbTipoSol.Location = new System.Drawing.Point(113, 71);
            this.cmbTipoSol.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTipoSol.Name = "cmbTipoSol";
            this.cmbTipoSol.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoSol.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipoSol.Properties.DisplayMember = "Value";
            this.cmbTipoSol.Properties.ValueMember = "Key";
            this.cmbTipoSol.Size = new System.Drawing.Size(109, 20);
            this.cmbTipoSol.TabIndex = 1;
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 8F);
            this.masterCliente.Location = new System.Drawing.Point(11, 47);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(313, 23);
            this.masterCliente.TabIndex = 2;
            this.masterCliente.Value = "";
            // 
            // lblTipoSol
            // 
            this.lblTipoSol.AutoSize = true;
            this.lblTipoSol.Location = new System.Drawing.Point(9, 72);
            this.lblTipoSol.Name = "lblTipoSol";
            this.lblTipoSol.Size = new System.Drawing.Size(93, 14);
            this.lblTipoSol.TabIndex = 3;
            this.lblTipoSol.Text = "1034_lblTipoSol";
            // 
            // masterClaseServicio
            // 
            this.masterClaseServicio.BackColor = System.Drawing.Color.Transparent;
            this.masterClaseServicio.Filtros = null;
            this.masterClaseServicio.Font = new System.Drawing.Font("Tahoma", 8F);
            this.masterClaseServicio.Location = new System.Drawing.Point(12, 24);
            this.masterClaseServicio.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.masterClaseServicio.Name = "masterClaseServicio";
            this.masterClaseServicio.Size = new System.Drawing.Size(312, 23);
            this.masterClaseServicio.TabIndex = 4;
            this.masterClaseServicio.Value = "";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.AutoSize = true;
            this.lblEmpresa.Location = new System.Drawing.Point(9, 94);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(131, 14);
            this.lblEmpresa.TabIndex = 6;
            this.lblEmpresa.Text = "1034_EmpresaNombre";
            // 
            // txtEmpresaNombre
            // 
            this.txtEmpresaNombre.Location = new System.Drawing.Point(113, 93);
            this.txtEmpresaNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtEmpresaNombre.Name = "txtEmpresaNombre";
            this.txtEmpresaNombre.Size = new System.Drawing.Size(220, 20);
            this.txtEmpresaNombre.TabIndex = 5;
            // 
            // txtLicitacion
            // 
            this.txtLicitacion.Location = new System.Drawing.Point(113, 115);
            this.txtLicitacion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLicitacion.Name = "txtLicitacion";
            this.txtLicitacion.Size = new System.Drawing.Size(220, 20);
            this.txtLicitacion.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "1034_lblLicitacion";
            // 
            // ModalFindDocSolicitud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 416);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ModalFindDocSolicitud";
            this.Text = "ModalFindDocSolicitud";
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).EndInit();
            this.pnGeneral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnDocument)).EndInit();
            this.pnDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnGrid)).EndInit();
            this.pnGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkSelectAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterGeneral)).EndInit();
            this.gbFilterGeneral.ResumeLayout(false);
            this.gbFilterGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterByDocumento)).EndInit();
            this.gbFilterByDocumento.ResumeLayout(false);
            this.gbFilterByDocumento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoSol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpresaNombre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private DevExpress.XtraEditors.LookUpEdit cmbTipoSol;
        private ControlsUC.uc_MasterFind masterCliente;
        private ControlsUC.uc_MasterFind masterClaseServicio;
        private System.Windows.Forms.Label lblTipoSol;
        private System.Windows.Forms.Label lblEmpresa;
        private DevExpress.XtraEditors.TextEdit txtEmpresaNombre;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtLicitacion;

    }
}