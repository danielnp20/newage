namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalReIncorporaciones
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
            this.uc_MasterDocumento = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtPeriodo = new DevExpress.XtraEditors.DateEdit();
            this.lblPeriodo = new DevExpress.XtraEditors.LabelControl();
            this.uc_Empleados = new NewAge.Cliente.GUI.WinApp.Forms.UC_Empleados();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSueldoIngreso = new DevExpress.XtraEditors.TextEdit();
            this.btnReincoporar = new System.Windows.Forms.Button();
            this.lblSueldo = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaIngreso = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaIngreso = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodo.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodo.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSueldoIngreso.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIngreso.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIngreso.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // uc_MasterDocumento
            // 
            this.uc_MasterDocumento.BackColor = System.Drawing.Color.Transparent;
            this.uc_MasterDocumento.Enabled = false;
            this.uc_MasterDocumento.Filtros = null;
            this.uc_MasterDocumento.Location = new System.Drawing.Point(12, 12);
            this.uc_MasterDocumento.Name = "uc_MasterDocumento";
            this.uc_MasterDocumento.Size = new System.Drawing.Size(291, 25);
            this.uc_MasterDocumento.TabIndex = 1;
            this.uc_MasterDocumento.Value = "";
            // 
            // dtPeriodo
            // 
            this.dtPeriodo.EditValue = null;
            this.dtPeriodo.Enabled = false;
            this.dtPeriodo.Location = new System.Drawing.Point(420, 12);
            this.dtPeriodo.Name = "dtPeriodo";
            this.dtPeriodo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPeriodo.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtPeriodo.Size = new System.Drawing.Size(100, 20);
            this.dtPeriodo.TabIndex = 0;
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.Location = new System.Drawing.Point(322, 15);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(82, 13);
            this.lblPeriodo.TabIndex = 3;
            this.lblPeriodo.Text = "29109_lblPeriodo";
            // 
            // uc_Empleados
            // 
            this.uc_Empleados.EmpActivos = 1;
            this.uc_Empleados.IsMultipleSeleccion = true;
            this.uc_Empleados.Location = new System.Drawing.Point(3, 38);
            this.uc_Empleados.Name = "uc_Empleados";
            this.uc_Empleados.Size = new System.Drawing.Size(810, 198);
            this.uc_Empleados.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSueldoIngreso);
            this.groupBox1.Controls.Add(this.btnReincoporar);
            this.groupBox1.Controls.Add(this.lblSueldo);
            this.groupBox1.Controls.Add(this.lblFechaIngreso);
            this.groupBox1.Controls.Add(this.dtFechaIngreso);
            this.groupBox1.Location = new System.Drawing.Point(12, 229);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(790, 49);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // txtSueldoIngreso
            // 
            this.txtSueldoIngreso.EditValue = "0";
            this.txtSueldoIngreso.Location = new System.Drawing.Point(299, 16);
            this.txtSueldoIngreso.Name = "txtSueldoIngreso";
            this.txtSueldoIngreso.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSueldoIngreso.Properties.Appearance.Options.UseFont = true;
            this.txtSueldoIngreso.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSueldoIngreso.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSueldoIngreso.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSueldoIngreso.Properties.Mask.EditMask = "c";
            this.txtSueldoIngreso.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSueldoIngreso.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSueldoIngreso.Size = new System.Drawing.Size(121, 20);
            this.txtSueldoIngreso.TabIndex = 18;
            // 
            // btnReincoporar
            // 
            this.btnReincoporar.Location = new System.Drawing.Point(449, 15);
            this.btnReincoporar.Name = "btnReincoporar";
            this.btnReincoporar.Size = new System.Drawing.Size(130, 23);
            this.btnReincoporar.TabIndex = 7;
            this.btnReincoporar.Text = "29109_btnReincorporar";
            this.btnReincoporar.UseVisualStyleBackColor = true;
            this.btnReincoporar.Click += new System.EventHandler(this.btnReincoporar_Click);
            // 
            // lblSueldo
            // 
            this.lblSueldo.Location = new System.Drawing.Point(228, 21);
            this.lblSueldo.Name = "lblSueldo";
            this.lblSueldo.Size = new System.Drawing.Size(78, 13);
            this.lblSueldo.TabIndex = 6;
            this.lblSueldo.Text = "29109_lblSueldo";
            // 
            // lblFechaIngreso
            // 
            this.lblFechaIngreso.Location = new System.Drawing.Point(12, 21);
            this.lblFechaIngreso.Name = "lblFechaIngreso";
            this.lblFechaIngreso.Size = new System.Drawing.Size(112, 13);
            this.lblFechaIngreso.TabIndex = 4;
            this.lblFechaIngreso.Text = "29109_lblFechaIngreso";
            // 
            // dtFechaIngreso
            // 
            this.dtFechaIngreso.EditValue = null;
            this.dtFechaIngreso.Location = new System.Drawing.Point(113, 17);
            this.dtFechaIngreso.Name = "dtFechaIngreso";
            this.dtFechaIngreso.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIngreso.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIngreso.Size = new System.Drawing.Size(100, 20);
            this.dtFechaIngreso.TabIndex = 3;
            // 
            // ModalReIncorporaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 288);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.uc_Empleados);
            this.Controls.Add(this.lblPeriodo);
            this.Controls.Add(this.dtPeriodo);
            this.Controls.Add(this.uc_MasterDocumento);
            this.MinimizeBox = false;
            this.Name = "ModalReIncorporaciones";
            this.Text = "ReIncorporaciones";
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodo.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodo.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSueldoIngreso.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIngreso.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIngreso.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ControlsUC.uc_MasterFind uc_MasterDocumento;
        private DevExpress.XtraEditors.DateEdit dtPeriodo;
        private DevExpress.XtraEditors.LabelControl lblPeriodo;
        private UC_Empleados uc_Empleados;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnReincoporar;
        private DevExpress.XtraEditors.LabelControl lblSueldo;
        private DevExpress.XtraEditors.LabelControl lblFechaIngreso;
        private DevExpress.XtraEditors.DateEdit dtFechaIngreso;
        private DevExpress.XtraEditors.TextEdit txtSueldoIngreso;
    }
}