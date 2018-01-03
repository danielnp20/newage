namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Prestamos
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
            this.txtValorPrestamo = new DevExpress.XtraEditors.TextEdit();
            this.lblValorPrestamo = new System.Windows.Forms.Label();
            this.lblPeriodoPago = new System.Windows.Forms.Label();
            this.dtFechaPrestamo = new DevExpress.XtraEditors.DateEdit();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblValorCuota = new System.Windows.Forms.Label();
            this.txtValorCuota = new DevExpress.XtraEditors.TextEdit();
            this.lblDescPrima = new System.Windows.Forms.Label();
            this.txtDescPrima = new DevExpress.XtraEditors.TextEdit();
            this.lblAbono = new System.Windows.Forms.Label();
            this.txtAbono = new DevExpress.XtraEditors.TextEdit();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.txtSaldo = new DevExpress.XtraEditors.TextEdit();
            this.uc_MasterConcepto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.lkpCmbPeriodo = new DevExpress.XtraEditors.LookUpEdit();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editlookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorPrestamo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPrestamo.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPrestamo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCuota.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescPrima.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAbono.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpCmbPeriodo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.lkpCmbPeriodo);
            this.grpboxDetail.Controls.Add(this.btnAdicionar);
            this.grpboxDetail.Controls.Add(this.uc_MasterConcepto);
            this.grpboxDetail.Controls.Add(this.lblSaldo);
            this.grpboxDetail.Controls.Add(this.txtSaldo);
            this.grpboxDetail.Controls.Add(this.lblAbono);
            this.grpboxDetail.Controls.Add(this.txtAbono);
            this.grpboxDetail.Controls.Add(this.lblDescPrima);
            this.grpboxDetail.Controls.Add(this.txtDescPrima);
            this.grpboxDetail.Controls.Add(this.lblValorCuota);
            this.grpboxDetail.Controls.Add(this.txtValorCuota);
            this.grpboxDetail.Controls.Add(this.lblFecha);
            this.grpboxDetail.Controls.Add(this.dtFechaPrestamo);
            this.grpboxDetail.Controls.Add(this.lblPeriodoPago);
            this.grpboxDetail.Controls.Add(this.lblValorPrestamo);
            this.grpboxDetail.Controls.Add(this.txtValorPrestamo);
            this.grpboxDetail.Location = new System.Drawing.Point(7, -1);
            this.grpboxDetail.Size = new System.Drawing.Size(1107, 92);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editBtnGrid,
            this.editCmb,
            this.editText,
            this.editSpin,
            this.editSpin4,
            this.editDate,
            this.editValue,
            this.editValue4,
            this.editlookUpEdit});
            // 
            // editCmb
            // 
            this.editCmb.AppearanceDropDown.BackColor = System.Drawing.Color.LightGray;
            this.editCmb.AppearanceDropDown.Options.UseBackColor = true;
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
            // editDate
            // 
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editValue4
            // 
            this.editValue4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblNumeroDoc
            // 
            this.lblNumeroDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblPrefix
            // 
            this.lblPrefix.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // uc_Empleados
            // 
            this.uc_Empleados.Size = new System.Drawing.Size(810, 224);
            // 
            // txtValorPrestamo
            // 
            this.txtValorPrestamo.EditValue = "0";
            this.txtValorPrestamo.Location = new System.Drawing.Point(724, 11);
            this.txtValorPrestamo.Name = "txtValorPrestamo";
            this.txtValorPrestamo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorPrestamo.Properties.Appearance.Options.UseFont = true;
            this.txtValorPrestamo.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorPrestamo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorPrestamo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorPrestamo.Properties.Mask.EditMask = "c";
            this.txtValorPrestamo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorPrestamo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorPrestamo.Size = new System.Drawing.Size(124, 20);
            this.txtValorPrestamo.TabIndex = 2;
            // 
            // lblValorPrestamo
            // 
            this.lblValorPrestamo.AutoSize = true;
            this.lblValorPrestamo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPrestamo.Location = new System.Drawing.Point(571, 12);
            this.lblValorPrestamo.Name = "lblValorPrestamo";
            this.lblValorPrestamo.Size = new System.Drawing.Size(138, 14);
            this.lblValorPrestamo.TabIndex = 1;
            this.lblValorPrestamo.Text = "29103_lblValorPrestamo";
            // 
            // lblPeriodoPago
            // 
            this.lblPeriodoPago.AutoSize = true;
            this.lblPeriodoPago.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodoPago.Location = new System.Drawing.Point(17, 38);
            this.lblPeriodoPago.Name = "lblPeriodoPago";
            this.lblPeriodoPago.Size = new System.Drawing.Size(128, 14);
            this.lblPeriodoPago.TabIndex = 11;
            this.lblPeriodoPago.Text = "29103_lblPeriodoPago";
            // 
            // dtFechaPrestamo
            // 
            this.dtFechaPrestamo.EditValue = null;
            this.dtFechaPrestamo.Location = new System.Drawing.Point(435, 11);
            this.dtFechaPrestamo.Name = "dtFechaPrestamo";
            this.dtFechaPrestamo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaPrestamo.Properties.Appearance.Options.UseFont = true;
            this.dtFechaPrestamo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaPrestamo.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaPrestamo.Size = new System.Drawing.Size(121, 20);
            this.dtFechaPrestamo.TabIndex = 1;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.Location = new System.Drawing.Point(337, 12);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(92, 14);
            this.lblFecha.TabIndex = 16;
            this.lblFecha.Text = "29103_lblFecha";
            // 
            // lblValorCuota
            // 
            this.lblValorCuota.AutoSize = true;
            this.lblValorCuota.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorCuota.Location = new System.Drawing.Point(307, 40);
            this.lblValorCuota.Name = "lblValorCuota";
            this.lblValorCuota.Size = new System.Drawing.Size(119, 14);
            this.lblValorCuota.TabIndex = 8;
            this.lblValorCuota.Text = "29103_lblValorCuota";
            // 
            // txtValorCuota
            // 
            this.txtValorCuota.EditValue = "0";
            this.txtValorCuota.Location = new System.Drawing.Point(435, 37);
            this.txtValorCuota.Name = "txtValorCuota";
            this.txtValorCuota.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorCuota.Properties.Appearance.Options.UseFont = true;
            this.txtValorCuota.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorCuota.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorCuota.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorCuota.Properties.Mask.EditMask = "c";
            this.txtValorCuota.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorCuota.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorCuota.Size = new System.Drawing.Size(121, 20);
            this.txtValorCuota.TabIndex = 4;
            // 
            // lblDescPrima
            // 
            this.lblDescPrima.AutoSize = true;
            this.lblDescPrima.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescPrima.Location = new System.Drawing.Point(311, 66);
            this.lblDescPrima.Name = "lblDescPrima";
            this.lblDescPrima.Size = new System.Drawing.Size(115, 14);
            this.lblDescPrima.TabIndex = 3;
            this.lblDescPrima.Text = "29103_lblDescPrima";
            // 
            // txtDescPrima
            // 
            this.txtDescPrima.EditValue = "0";
            this.txtDescPrima.Location = new System.Drawing.Point(435, 63);
            this.txtDescPrima.Name = "txtDescPrima";
            this.txtDescPrima.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescPrima.Properties.Appearance.Options.UseFont = true;
            this.txtDescPrima.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDescPrima.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtDescPrima.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtDescPrima.Properties.Mask.EditMask = "c";
            this.txtDescPrima.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtDescPrima.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtDescPrima.Size = new System.Drawing.Size(121, 20);
            this.txtDescPrima.TabIndex = 6;
            // 
            // lblAbono
            // 
            this.lblAbono.AutoSize = true;
            this.lblAbono.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAbono.Location = new System.Drawing.Point(613, 39);
            this.lblAbono.Name = "lblAbono";
            this.lblAbono.Size = new System.Drawing.Size(96, 14);
            this.lblAbono.TabIndex = 12;
            this.lblAbono.Text = "29103_lblAbono";
            // 
            // txtAbono
            // 
            this.txtAbono.EditValue = "0";
            this.txtAbono.Enabled = false;
            this.txtAbono.Location = new System.Drawing.Point(724, 37);
            this.txtAbono.Name = "txtAbono";
            this.txtAbono.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAbono.Properties.Appearance.Options.UseFont = true;
            this.txtAbono.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAbono.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtAbono.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtAbono.Properties.Mask.EditMask = "c";
            this.txtAbono.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtAbono.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtAbono.Size = new System.Drawing.Size(124, 20);
            this.txtAbono.TabIndex = 5;
            // 
            // lblSaldo
            // 
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSaldo.Location = new System.Drawing.Point(620, 66);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(89, 14);
            this.lblSaldo.TabIndex = 15;
            this.lblSaldo.Text = "29103_lblSaldo";
            // 
            // txtSaldo
            // 
            this.txtSaldo.EditValue = "0";
            this.txtSaldo.Enabled = false;
            this.txtSaldo.Location = new System.Drawing.Point(724, 63);
            this.txtSaldo.Name = "txtSaldo";
            this.txtSaldo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaldo.Properties.Appearance.Options.UseFont = true;
            this.txtSaldo.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSaldo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSaldo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSaldo.Properties.Mask.EditMask = "c";
            this.txtSaldo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSaldo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtSaldo.Size = new System.Drawing.Size(124, 20);
            this.txtSaldo.TabIndex = 7;
            // 
            // uc_MasterConcepto
            // 
            this.uc_MasterConcepto.BackColor = System.Drawing.Color.Transparent;
            this.uc_MasterConcepto.Filtros = null;
            this.uc_MasterConcepto.Location = new System.Drawing.Point(20, 9);
            this.uc_MasterConcepto.Name = "uc_MasterConcepto";
            this.uc_MasterConcepto.Size = new System.Drawing.Size(291, 25);
            this.uc_MasterConcepto.TabIndex = 0;
            this.uc_MasterConcepto.Value = "";
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Location = new System.Drawing.Point(994, 11);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(107, 23);
            this.btnAdicionar.TabIndex = 8;
            this.btnAdicionar.Text = "29001_btnAdd";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // lkpCmbPeriodo
            // 
            this.lkpCmbPeriodo.Location = new System.Drawing.Point(151, 36);
            this.lkpCmbPeriodo.Name = "lkpCmbPeriodo";
            this.lkpCmbPeriodo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpCmbPeriodo.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "Periodo")});
            this.lkpCmbPeriodo.Properties.DisplayMember = "Value";
            this.lkpCmbPeriodo.Properties.ValueMember = "Key";
            this.lkpCmbPeriodo.Size = new System.Drawing.Size(134, 20);
            this.lkpCmbPeriodo.TabIndex = 3;
            // 
            // Prestamos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "Prestamos";
            this.Text = "NovedadesNomina";
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editlookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorPrestamo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPrestamo.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPrestamo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCuota.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescPrima.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAbono.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpCmbPeriodo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblValorPrestamo;
        private DevExpress.XtraEditors.TextEdit txtValorPrestamo;
        private System.Windows.Forms.Label lblPeriodoPago;
        private DevExpress.XtraEditors.DateEdit dtFechaPrestamo;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblSaldo;
        private DevExpress.XtraEditors.TextEdit txtSaldo;
        private System.Windows.Forms.Label lblAbono;
        private DevExpress.XtraEditors.TextEdit txtAbono;
        private System.Windows.Forms.Label lblDescPrima;
        private DevExpress.XtraEditors.TextEdit txtDescPrima;
        private System.Windows.Forms.Label lblValorCuota;
        private DevExpress.XtraEditors.TextEdit txtValorCuota;
        private ControlsUC.uc_MasterFind uc_MasterConcepto;
        private System.Windows.Forms.Button btnAdicionar;
        private DevExpress.XtraEditors.LookUpEdit lkpCmbPeriodo;
    }
}