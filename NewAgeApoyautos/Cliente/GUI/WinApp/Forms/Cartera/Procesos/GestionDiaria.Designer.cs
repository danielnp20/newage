namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class GestionDiaria
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
            this.masterGestionCob = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaCorte = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaCorte = new DevExpress.XtraEditors.DateEdit();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.lblEstado = new DevExpress.XtraEditors.LabelControl();
            this.cmbTipoComunic = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoComun = new DevExpress.XtraEditors.LabelControl();
            this.btnProcesar = new System.Windows.Forms.Button();
            this.btnMensajesTelefono = new System.Windows.Forms.Button();
            this.btnIncosistencias = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.btnGenerarCartas = new System.Windows.Forms.Button();
            this.btnGenerarArchivo = new System.Windows.Forms.Button();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.btnCorreos = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoComunic.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 305);
            this.pbProcess.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(458, 18);
            // 
            // masterGestionCob
            // 
            this.masterGestionCob.BackColor = System.Drawing.Color.Transparent;
            this.masterGestionCob.Filtros = null;
            this.masterGestionCob.Location = new System.Drawing.Point(28, 40);
            this.masterGestionCob.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.masterGestionCob.Name = "masterGestionCob";
            this.masterGestionCob.Size = new System.Drawing.Size(310, 25);
            this.masterGestionCob.TabIndex = 12;
            this.masterGestionCob.Value = "";
            this.masterGestionCob.Leave += new System.EventHandler(this.masterGestionCobranza_Leave);
            // 
            // lblFechaCorte
            // 
            this.lblFechaCorte.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaCorte.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaCorte.Location = new System.Drawing.Point(31, 16);
            this.lblFechaCorte.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaCorte.Name = "lblFechaCorte";
            this.lblFechaCorte.Size = new System.Drawing.Size(78, 14);
            this.lblFechaCorte.TabIndex = 29;
            this.lblFechaCorte.Text = "1154_lblFecha";
            // 
            // dtFechaCorte
            // 
            this.dtFechaCorte.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaCorte.Location = new System.Drawing.Point(128, 13);
            this.dtFechaCorte.Name = "dtFechaCorte";
            this.dtFechaCorte.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaCorte.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaCorte.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaCorte.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaCorte.Properties.Appearance.Options.UseFont = true;
            this.dtFechaCorte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaCorte.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaCorte.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCorte.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaCorte.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaCorte.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaCorte.Size = new System.Drawing.Size(97, 20);
            this.dtFechaCorte.TabIndex = 30;
            // 
            // cmbEstado
            // 
            this.cmbEstado.Location = new System.Drawing.Point(128, 108);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Size = new System.Drawing.Size(118, 20);
            this.cmbEstado.TabIndex = 76;
            // 
            // lblEstado
            // 
            this.lblEstado.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblEstado.Location = new System.Drawing.Point(28, 112);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(83, 14);
            this.lblEstado.TabIndex = 77;
            this.lblEstado.Text = "1154_lblEstado";
            // 
            // cmbTipoComunic
            // 
            this.cmbTipoComunic.Location = new System.Drawing.Point(128, 142);
            this.cmbTipoComunic.Name = "cmbTipoComunic";
            this.cmbTipoComunic.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoComunic.Size = new System.Drawing.Size(118, 20);
            this.cmbTipoComunic.TabIndex = 78;
            this.cmbTipoComunic.EditValueChanged += new System.EventHandler(this.cmbTipoComunic_EditValueChanged);
            // 
            // lblTipoComun
            // 
            this.lblTipoComun.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTipoComun.Location = new System.Drawing.Point(28, 146);
            this.lblTipoComun.Name = "lblTipoComun";
            this.lblTipoComun.Size = new System.Drawing.Size(116, 14);
            this.lblTipoComun.TabIndex = 79;
            this.lblTipoComun.Text = "1154_lblTipoComunic";
            // 
            // btnProcesar
            // 
            this.btnProcesar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcesar.Location = new System.Drawing.Point(31, 254);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(167, 42);
            this.btnProcesar.TabIndex = 82;
            this.btnProcesar.Text = "1154_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // btnMensajesTelefono
            // 
            this.btnMensajesTelefono.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnMensajesTelefono.Image = global::NewAge.Properties.Resources.sms_logo1;
            this.btnMensajesTelefono.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMensajesTelefono.Location = new System.Drawing.Point(267, 141);
            this.btnMensajesTelefono.Margin = new System.Windows.Forms.Padding(0);
            this.btnMensajesTelefono.Name = "btnMensajesTelefono";
            this.btnMensajesTelefono.Size = new System.Drawing.Size(90, 42);
            this.btnMensajesTelefono.TabIndex = 84;
            this.btnMensajesTelefono.Text = "Enviar Mensajes";
            this.btnMensajesTelefono.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMensajesTelefono.UseVisualStyleBackColor = true;
            this.btnMensajesTelefono.Visible = false;
            this.btnMensajesTelefono.Click += new System.EventHandler(this.btnGenerarMensajesTxt_Click);
            // 
            // btnIncosistencias
            // 
            this.btnIncosistencias.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncosistencias.Image = global::NewAge.Properties.Resources.errorIcon;
            this.btnIncosistencias.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIncosistencias.Location = new System.Drawing.Point(222, 254);
            this.btnIncosistencias.Name = "btnIncosistencias";
            this.btnIncosistencias.Size = new System.Drawing.Size(150, 42);
            this.btnIncosistencias.TabIndex = 83;
            this.btnIncosistencias.Text = "1154_btnInconsistencias";
            this.btnIncosistencias.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIncosistencias.UseVisualStyleBackColor = true;
            this.btnIncosistencias.Click += new System.EventHandler(this.btnIncosistencias_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.Image = global::NewAge.Properties.Resources.TBIconImport;
            this.btnImportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportar.Location = new System.Drawing.Point(222, 202);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(150, 42);
            this.btnImportar.TabIndex = 81;
            this.btnImportar.Text = "1154_btnImportar";
            this.btnImportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // btnGenerarCartas
            // 
            this.btnGenerarCartas.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnGenerarCartas.Image = global::NewAge.Properties.Resources.pdf;
            this.btnGenerarCartas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerarCartas.Location = new System.Drawing.Point(259, 141);
            this.btnGenerarCartas.Margin = new System.Windows.Forms.Padding(0);
            this.btnGenerarCartas.Name = "btnGenerarCartas";
            this.btnGenerarCartas.Size = new System.Drawing.Size(97, 42);
            this.btnGenerarCartas.TabIndex = 80;
            this.btnGenerarCartas.Text = "1154_btnGenerarCarta";
            this.btnGenerarCartas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGenerarCartas.UseVisualStyleBackColor = true;
            this.btnGenerarCartas.Visible = false;
            this.btnGenerarCartas.Click += new System.EventHandler(this.btnGenerarCartas_Click);
            // 
            // btnGenerarArchivo
            // 
            this.btnGenerarArchivo.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarArchivo.Image = global::NewAge.Properties.Resources.Excel;
            this.btnGenerarArchivo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenerarArchivo.Location = new System.Drawing.Point(31, 202);
            this.btnGenerarArchivo.Name = "btnGenerarArchivo";
            this.btnGenerarArchivo.Size = new System.Drawing.Size(167, 42);
            this.btnGenerarArchivo.TabIndex = 4;
            this.btnGenerarArchivo.Text = "1154_btnGenerarArchivo";
            this.btnGenerarArchivo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGenerarArchivo.UseVisualStyleBackColor = true;
            this.btnGenerarArchivo.Click += new System.EventHandler(this.btnGenerarDocumentos_Click);
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(28, 72);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(310, 25);
            this.masterCliente.TabIndex = 85;
            this.masterCliente.Value = "";
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(383, 72);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(59, 22);
            this.txtLibranza.TabIndex = 87;
            this.txtLibranza.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLibranza_KeyPress);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(323, 77);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(62, 14);
            this.lblLibranza.TabIndex = 86;
            this.lblLibranza.Text = "Obligación";
            // 
            // btnCorreos
            // 
            this.btnCorreos.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnCorreos.Image = global::NewAge.Properties.Resources.sms_logo1;
            this.btnCorreos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCorreos.Location = new System.Drawing.Point(276, 141);
            this.btnCorreos.Margin = new System.Windows.Forms.Padding(0);
            this.btnCorreos.Name = "btnCorreos";
            this.btnCorreos.Size = new System.Drawing.Size(90, 42);
            this.btnCorreos.TabIndex = 88;
            this.btnCorreos.Text = "Enviar Correos";
            this.btnCorreos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCorreos.UseVisualStyleBackColor = true;
            this.btnCorreos.Visible = false;
            this.btnCorreos.Click += new System.EventHandler(this.btnCorreos_Click);
            // 
            // GestionDiaria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 323);
            this.Controls.Add(this.btnCorreos);
            this.Controls.Add(this.txtLibranza);
            this.Controls.Add(this.lblLibranza);
            this.Controls.Add(this.masterCliente);
            this.Controls.Add(this.btnMensajesTelefono);
            this.Controls.Add(this.btnIncosistencias);
            this.Controls.Add(this.btnProcesar);
            this.Controls.Add(this.btnImportar);
            this.Controls.Add(this.btnGenerarCartas);
            this.Controls.Add(this.cmbTipoComunic);
            this.Controls.Add(this.lblTipoComun);
            this.Controls.Add(this.cmbEstado);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblFechaCorte);
            this.Controls.Add(this.dtFechaCorte);
            this.Controls.Add(this.masterGestionCob);
            this.Controls.Add(this.btnGenerarArchivo);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GestionDiaria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1154";
            this.Controls.SetChildIndex(this.btnGenerarArchivo, 0);
            this.Controls.SetChildIndex(this.masterGestionCob, 0);
            this.Controls.SetChildIndex(this.dtFechaCorte, 0);
            this.Controls.SetChildIndex(this.lblFechaCorte, 0);
            this.Controls.SetChildIndex(this.lblEstado, 0);
            this.Controls.SetChildIndex(this.cmbEstado, 0);
            this.Controls.SetChildIndex(this.lblTipoComun, 0);
            this.Controls.SetChildIndex(this.cmbTipoComunic, 0);
            this.Controls.SetChildIndex(this.btnGenerarCartas, 0);
            this.Controls.SetChildIndex(this.btnImportar, 0);
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.btnIncosistencias, 0);
            this.Controls.SetChildIndex(this.btnMensajesTelefono, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.masterCliente, 0);
            this.Controls.SetChildIndex(this.lblLibranza, 0);
            this.Controls.SetChildIndex(this.txtLibranza, 0);
            this.Controls.SetChildIndex(this.btnCorreos, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaCorte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoComunic.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerarArchivo;
        private ControlsUC.uc_MasterFind masterGestionCob;
        private DevExpress.XtraEditors.LabelControl lblFechaCorte;
        protected DevExpress.XtraEditors.DateEdit dtFechaCorte;
        private DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private DevExpress.XtraEditors.LabelControl lblEstado;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoComunic;
        private DevExpress.XtraEditors.LabelControl lblTipoComun;
        private System.Windows.Forms.Button btnGenerarCartas;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnProcesar;
        private System.Windows.Forms.Button btnIncosistencias;
        private System.Windows.Forms.Button btnMensajesTelefono;
        private ControlsUC.uc_MasterFind masterCliente;
        private System.Windows.Forms.TextBox txtLibranza;
        private System.Windows.Forms.Label lblLibranza;
        private System.Windows.Forms.Button btnCorreos;
    }
}