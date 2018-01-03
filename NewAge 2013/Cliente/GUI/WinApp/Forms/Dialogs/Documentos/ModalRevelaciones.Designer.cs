namespace NewAge.Forms.Dialogs.Documentos
{
    partial class ModalRevelaciones
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
            this.txtTituloRevelacion = new DevExpress.XtraEditors.MemoEdit();
            this.uc_NotaRevelacion = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lbl_TituloRevelaciones = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uc_Revelaciones = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Revelaciones();
            this.lbl_Revelaciones = new System.Windows.Forms.Label();
            this.btnIncluirRevelacion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtTituloRevelacion.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTituloRevelacion
            // 
            this.txtTituloRevelacion.Location = new System.Drawing.Point(9, 74);
            this.txtTituloRevelacion.Name = "txtTituloRevelacion";
            this.txtTituloRevelacion.Size = new System.Drawing.Size(866, 45);
            this.txtTituloRevelacion.TabIndex = 2;
            this.txtTituloRevelacion.Leave += new System.EventHandler(this.txtTituloRevelacion_Leave);
            // 
            // uc_NotaRevelacion
            // 
            this.uc_NotaRevelacion.BackColor = System.Drawing.Color.Transparent;
            this.uc_NotaRevelacion.Filtros = null;
            this.uc_NotaRevelacion.Location = new System.Drawing.Point(12, 12);
            this.uc_NotaRevelacion.Name = "uc_NotaRevelacion";
            this.uc_NotaRevelacion.Size = new System.Drawing.Size(291, 25);
            this.uc_NotaRevelacion.TabIndex = 1;
            this.uc_NotaRevelacion.Value = "";
            this.uc_NotaRevelacion.Leave += new System.EventHandler(this.uc_NotaRevelacion_Leave);
            // 
            // lbl_TituloRevelaciones
            // 
            this.lbl_TituloRevelaciones.AutoSize = true;
            this.lbl_TituloRevelaciones.Location = new System.Drawing.Point(9, 50);
            this.lbl_TituloRevelaciones.Name = "lbl_TituloRevelaciones";
            this.lbl_TituloRevelaciones.Size = new System.Drawing.Size(138, 13);
            this.lbl_TituloRevelaciones.TabIndex = 3;
            this.lbl_TituloRevelaciones.Text = "1037_lblTituloRevelaciones";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uc_Revelaciones);
            this.groupBox1.Location = new System.Drawing.Point(9, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(869, 498);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // uc_Revelaciones
            // 
            this.uc_Revelaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uc_Revelaciones.Location = new System.Drawing.Point(3, 16);
            this.uc_Revelaciones.Name = "uc_Revelaciones";
            this.uc_Revelaciones.Size = new System.Drawing.Size(863, 479);
            this.uc_Revelaciones.TabIndex = 3;
            this.uc_Revelaciones.Leave += new System.EventHandler(this.uc_Revelaciones_Leave);
            // 
            // lbl_Revelaciones
            // 
            this.lbl_Revelaciones.AutoSize = true;
            this.lbl_Revelaciones.Location = new System.Drawing.Point(9, 130);
            this.lbl_Revelaciones.Name = "lbl_Revelaciones";
            this.lbl_Revelaciones.Size = new System.Drawing.Size(112, 13);
            this.lbl_Revelaciones.TabIndex = 5;
            this.lbl_Revelaciones.Text = "1037_lblRevelaciones";
            // 
            // btnIncluirRevelacion
            // 
            this.btnIncluirRevelacion.Enabled = false;
            this.btnIncluirRevelacion.Location = new System.Drawing.Point(685, 27);
            this.btnIncluirRevelacion.Name = "btnIncluirRevelacion";
            this.btnIncluirRevelacion.Size = new System.Drawing.Size(193, 23);
            this.btnIncluirRevelacion.TabIndex = 4;
            this.btnIncluirRevelacion.Text = "1037_IncluirRevelacion";
            this.btnIncluirRevelacion.UseVisualStyleBackColor = true;
            this.btnIncluirRevelacion.Click += new System.EventHandler(this.btnIncluirRevelacion_Click);
            // 
            // ModalRevelaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(902, 544);
            this.Controls.Add(this.btnIncluirRevelacion);
            this.Controls.Add(this.lbl_Revelaciones);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_TituloRevelaciones);
            this.Controls.Add(this.uc_NotaRevelacion);
            this.Controls.Add(this.txtTituloRevelacion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalRevelaciones";
            this.Text = "ModalRevelaciones";
            ((System.ComponentModel.ISupportInitialize)(this.txtTituloRevelacion.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit txtTituloRevelacion;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_NotaRevelacion;
        private System.Windows.Forms.Label lbl_TituloRevelaciones;
        private System.Windows.Forms.GroupBox groupBox1;
        private Cliente.GUI.WinApp.ControlsUC.uc_Revelaciones uc_Revelaciones;
        private System.Windows.Forms.Label lbl_Revelaciones;
        private System.Windows.Forms.Button btnIncluirRevelacion;

    }
}