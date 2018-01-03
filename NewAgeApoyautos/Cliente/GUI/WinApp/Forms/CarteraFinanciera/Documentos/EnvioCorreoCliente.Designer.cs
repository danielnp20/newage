namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class EnvioCorreoCliente
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnvioCorreoCliente));
            this.chkConyuge = new System.Windows.Forms.CheckBox();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnDestinatarios = new DevExpress.XtraEditors.SimpleButton();
            this.chkCodeudor = new System.Windows.Forms.CheckBox();
            this.chkCliente = new System.Windows.Forms.CheckBox();
            this.gbCorreo = new DevExpress.XtraEditors.GroupControl();
            this.lbl_Revelaciones = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uc_Revelaciones = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Revelaciones();
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            this.lbl_TituloRevelaciones = new System.Windows.Forms.Label();
            this.txtAsunto = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbCorreo)).BeginInit();
            this.gbCorreo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAsunto.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chkConyuge
            // 
            this.chkConyuge.AutoSize = true;
            this.chkConyuge.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.chkConyuge.Location = new System.Drawing.Point(795, 25);
            this.chkConyuge.Name = "chkConyuge";
            this.chkConyuge.Size = new System.Drawing.Size(80, 18);
            this.chkConyuge.TabIndex = 8;
            this.chkConyuge.Text = "Cónyuge";
            this.chkConyuge.UseVisualStyleBackColor = true;
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCliente.Location = new System.Drawing.Point(28, 20);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(359, 24);
            this.masterCliente.TabIndex = 0;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.btnDestinatarios);
            this.panelControl3.Controls.Add(this.masterCliente);
            this.panelControl3.Controls.Add(this.chkCodeudor);
            this.panelControl3.Controls.Add(this.chkCliente);
            this.panelControl3.Controls.Add(this.gbCorreo);
            this.panelControl3.Controls.Add(this.chkConyuge);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1067, 664);
            this.panelControl3.TabIndex = 16;
            // 
            // btnDestinatarios
            // 
            this.btnDestinatarios.Appearance.Font = new System.Drawing.Font("Tahoma", 7.9F, System.Drawing.FontStyle.Bold);
            this.btnDestinatarios.Appearance.Options.UseFont = true;
            this.btnDestinatarios.Enabled = false;
            this.btnDestinatarios.Location = new System.Drawing.Point(412, 22);
            this.btnDestinatarios.Name = "btnDestinatarios";
            this.btnDestinatarios.Size = new System.Drawing.Size(119, 21);
            this.btnDestinatarios.TabIndex = 80;
            this.btnDestinatarios.Text = "Ver Destinatarios";
            this.btnDestinatarios.Click += new System.EventHandler(this.btnDestinatarios_Click);
            // 
            // chkCodeudor
            // 
            this.chkCodeudor.AutoSize = true;
            this.chkCodeudor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.chkCodeudor.Location = new System.Drawing.Point(898, 25);
            this.chkCodeudor.Name = "chkCodeudor";
            this.chkCodeudor.Size = new System.Drawing.Size(86, 18);
            this.chkCodeudor.TabIndex = 11;
            this.chkCodeudor.Text = "Codeudor";
            this.chkCodeudor.UseVisualStyleBackColor = true;
            // 
            // chkCliente
            // 
            this.chkCliente.AutoSize = true;
            this.chkCliente.Checked = true;
            this.chkCliente.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCliente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.chkCliente.Location = new System.Drawing.Point(695, 25);
            this.chkCliente.Name = "chkCliente";
            this.chkCliente.Size = new System.Drawing.Size(68, 18);
            this.chkCliente.TabIndex = 10;
            this.chkCliente.Text = "Cliente";
            this.chkCliente.UseVisualStyleBackColor = true;
            // 
            // gbCorreo
            // 
            this.gbCorreo.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbCorreo.AppearanceCaption.Options.UseFont = true;
            this.gbCorreo.Controls.Add(this.lbl_Revelaciones);
            this.gbCorreo.Controls.Add(this.groupBox2);
            this.gbCorreo.Controls.Add(this.xtraScrollableControl1);
            this.gbCorreo.Controls.Add(this.lbl_TituloRevelaciones);
            this.gbCorreo.Controls.Add(this.txtAsunto);
            this.gbCorreo.Location = new System.Drawing.Point(26, 51);
            this.gbCorreo.Name = "gbCorreo";
            this.gbCorreo.Size = new System.Drawing.Size(980, 607);
            this.gbCorreo.TabIndex = 9;
            this.gbCorreo.Text = "Redactar Correo";
            // 
            // lbl_Revelaciones
            // 
            this.lbl_Revelaciones.AutoSize = true;
            this.lbl_Revelaciones.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbl_Revelaciones.Location = new System.Drawing.Point(13, 74);
            this.lbl_Revelaciones.Name = "lbl_Revelaciones";
            this.lbl_Revelaciones.Size = new System.Drawing.Size(57, 14);
            this.lbl_Revelaciones.TabIndex = 9;
            this.lbl_Revelaciones.Text = "Mensaje";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uc_Revelaciones);
            this.groupBox2.Location = new System.Drawing.Point(16, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(978, 501);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            // 
            // uc_Revelaciones
            // 
            this.uc_Revelaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uc_Revelaciones.Location = new System.Drawing.Point(3, 17);
            this.uc_Revelaciones.Name = "uc_Revelaciones";
            this.uc_Revelaciones.Size = new System.Drawing.Size(972, 481);
            this.uc_Revelaciones.TabIndex = 3;
            this.uc_Revelaciones.ValueXML = resources.GetString("uc_Revelaciones.Value");
            // 
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Location = new System.Drawing.Point(13, 117);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Size = new System.Drawing.Size(971, 456);
            this.xtraScrollableControl1.TabIndex = 10;
            // 
            // lbl_TituloRevelaciones
            // 
            this.lbl_TituloRevelaciones.AutoSize = true;
            this.lbl_TituloRevelaciones.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lbl_TituloRevelaciones.Location = new System.Drawing.Point(13, 29);
            this.lbl_TituloRevelaciones.Name = "lbl_TituloRevelaciones";
            this.lbl_TituloRevelaciones.Size = new System.Drawing.Size(52, 14);
            this.lbl_TituloRevelaciones.TabIndex = 7;
            this.lbl_TituloRevelaciones.Text = "Asunto";
            // 
            // txtAsunto
            // 
            this.txtAsunto.Location = new System.Drawing.Point(13, 45);
            this.txtAsunto.Name = "txtAsunto";
            this.txtAsunto.Size = new System.Drawing.Size(935, 24);
            this.txtAsunto.TabIndex = 6;
            // 
            // EnvioCorreoCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 664);
            this.Controls.Add(this.panelControl3);
            this.Name = "EnvioCorreoCliente";
            this.Text = "1048";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbCorreo)).EndInit();
            this.gbCorreo.ResumeLayout(false);
            this.gbCorreo.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtAsunto.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind masterCliente;
        private System.Windows.Forms.CheckBox chkConyuge;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.GroupControl gbCorreo;
        private System.Windows.Forms.Label lbl_Revelaciones;
        private ControlsUC.uc_Revelaciones uc_Revelaciones;
        private System.Windows.Forms.Label lbl_TituloRevelaciones;
        private DevExpress.XtraEditors.MemoEdit txtAsunto;
        private System.Windows.Forms.CheckBox chkCodeudor;
        private System.Windows.Forms.CheckBox chkCliente;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.SimpleButton btnDestinatarios;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
    }
}