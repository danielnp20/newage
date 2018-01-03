namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class CambioDatosCliente
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.lblCliente = new System.Windows.Forms.Label();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtDireccion = new DevExpress.XtraEditors.ButtonEdit();
            this.txtTelefono1 = new System.Windows.Forms.TextBox();
            this.masterCiudad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTelefono2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCelular2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCelular1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.rbTipoCambio = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.txtDireccion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbTipoCambio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(12, 40);
            this.richEditControl.TabIndex = 2;
            this.richEditControl.Text = "myRichEditControl";
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCliente.Location = new System.Drawing.Point(33, 23);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(122, 14);
            this.lblCliente.TabIndex = 38;
            this.lblCliente.Text = "32555_CedulaCliente";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCliente.Location = new System.Drawing.Point(84, 17);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(7);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(357, 27);
            this.masterCliente.TabIndex = 0;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // txtDireccion
            // 
            this.txtDireccion.EditValue = "";
            this.txtDireccion.Location = new System.Drawing.Point(201, 92);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDireccion.Properties.Appearance.Options.UseFont = true;
            this.txtDireccion.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDireccion.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtDireccion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::NewAge.Properties.Resources.FindFkHierarchy, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.txtDireccion.Size = new System.Drawing.Size(224, 20);
            this.txtDireccion.TabIndex = 2;
            this.txtDireccion.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtDireccion_ButtonClick);
            // 
            // txtTelefono1
            // 
            this.txtTelefono1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefono1.Location = new System.Drawing.Point(202, 140);
            this.txtTelefono1.Name = "txtTelefono1";
            this.txtTelefono1.Size = new System.Drawing.Size(222, 22);
            this.txtTelefono1.TabIndex = 4;
            // 
            // masterCiudad
            // 
            this.masterCiudad.BackColor = System.Drawing.Color.Transparent;
            this.masterCiudad.Filtros = null;
            this.masterCiudad.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCiudad.Location = new System.Drawing.Point(84, 112);
            this.masterCiudad.Margin = new System.Windows.Forms.Padding(7);
            this.masterCiudad.Name = "masterCiudad";
            this.masterCiudad.Size = new System.Drawing.Size(343, 24);
            this.masterCiudad.TabIndex = 3;
            this.masterCiudad.Value = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 14);
            this.label1.TabIndex = 89;
            this.label1.Text = "32006_Direccion";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 14);
            this.label2.TabIndex = 90;
            this.label2.Text = "32006_Telefono1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(32, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 14);
            this.label3.TabIndex = 92;
            this.label3.Text = "32006_Telefono2";
            // 
            // txtTelefono2
            // 
            this.txtTelefono2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefono2.Location = new System.Drawing.Point(202, 167);
            this.txtTelefono2.Name = "txtTelefono2";
            this.txtTelefono2.Size = new System.Drawing.Size(222, 22);
            this.txtTelefono2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(32, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 96;
            this.label4.Text = "32006_Celular2";
            // 
            // txtCelular2
            // 
            this.txtCelular2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCelular2.Location = new System.Drawing.Point(202, 221);
            this.txtCelular2.Name = "txtCelular2";
            this.txtCelular2.Size = new System.Drawing.Size(222, 22);
            this.txtCelular2.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(33, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 14);
            this.label5.TabIndex = 94;
            this.label5.Text = "32006_Celular1";
            // 
            // txtCelular1
            // 
            this.txtCelular1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCelular1.Location = new System.Drawing.Point(202, 194);
            this.txtCelular1.Name = "txtCelular1";
            this.txtCelular1.Size = new System.Drawing.Size(222, 22);
            this.txtCelular1.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(33, 251);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 14);
            this.label6.TabIndex = 98;
            this.label6.Text = "32006_Correo";
            // 
            // txtCorreo
            // 
            this.txtCorreo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCorreo.Location = new System.Drawing.Point(203, 248);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(222, 22);
            this.txtCorreo.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(32, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 14);
            this.label7.TabIndex = 99;
            this.label7.Text = "32006_Ciudad";
            // 
            // rbTipoCambio
            // 
            this.rbTipoCambio.Location = new System.Drawing.Point(36, 56);
            this.rbTipoCambio.Name = "rbTipoCambio";
            this.rbTipoCambio.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rbTipoCambio.Properties.Appearance.Options.UseBackColor = true;
            this.rbTipoCambio.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rbTipoCambio.Properties.Columns = 3;
            this.rbTipoCambio.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Forma Verbal"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Forma Escrita")});
            this.rbTipoCambio.Size = new System.Drawing.Size(389, 30);
            this.rbTipoCambio.TabIndex = 1;
            // 
            // CambioDatosCliente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 304);
            this.Controls.Add(this.rbTipoCambio);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtCorreo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCelular2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCelular1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTelefono2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.masterCiudad);
            this.Controls.Add(this.txtDireccion);
            this.Controls.Add(this.txtTelefono1);
            this.Controls.Add(this.lblCliente);
            this.Controls.Add(this.masterCliente);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CambioDatosCliente";
            this.Text = "SolicitudCredito";
            ((System.ComponentModel.ISupportInitialize)(this.txtDireccion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbTipoCambio.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit editRichText;
        private System.Windows.Forms.Label lblCliente;
        private ControlsUC.uc_MasterFind masterCliente;
        private DevExpress.XtraEditors.ButtonEdit txtDireccion;
        private System.Windows.Forms.TextBox txtTelefono1;
        private ControlsUC.uc_MasterFind masterCiudad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTelefono2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCelular2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCelular1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.RadioGroup rbTipoCambio;
    }
}