namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class LiquidacionVacacionesColectivas
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDiasVac = new DevExpress.XtraEditors.TextEdit();
            this.lblResolucion = new System.Windows.Forms.Label();
            this.txtResolucion = new DevExpress.XtraEditors.TextEdit();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.lblFechaIni = new System.Windows.Forms.Label();
            this.dtFechaFin = new DevExpress.XtraEditors.DateEdit();
            this.dtFechaIni = new DevExpress.XtraEditors.DateEdit();
            this.txtTotalDeducciones = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalPago = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalDevengos = new DevExpress.XtraEditors.TextEdit();
            this.lblTotalPago = new System.Windows.Forms.Label();
            this.lblDeducciones = new System.Windows.Forms.Label();
            this.lblDevengos = new System.Windows.Forms.Label();
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
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiasVac.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResolucion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFin.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIni.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIni.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalDeducciones.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalDevengos.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.txtTotalDeducciones);
            this.grpboxDetail.Controls.Add(this.txtTotalPago);
            this.grpboxDetail.Controls.Add(this.txtTotalDevengos);
            this.grpboxDetail.Controls.Add(this.lblTotalPago);
            this.grpboxDetail.Controls.Add(this.lblDeducciones);
            this.grpboxDetail.Controls.Add(this.lblDevengos);
            this.grpboxDetail.Location = new System.Drawing.Point(5, 0);
            this.grpboxDetail.Size = new System.Drawing.Size(1109, 91);
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
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.groupBox1);
            this.grpboxHeader.Controls.SetChildIndex(this.groupBox1, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.uc_Empleados, 0);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDiasVac);
            this.groupBox1.Controls.Add(this.lblResolucion);
            this.groupBox1.Controls.Add(this.txtResolucion);
            this.groupBox1.Controls.Add(this.lblFechaFin);
            this.groupBox1.Controls.Add(this.lblFechaIni);
            this.groupBox1.Controls.Add(this.dtFechaFin);
            this.groupBox1.Controls.Add(this.dtFechaIni);
            this.groupBox1.Location = new System.Drawing.Point(816, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 172);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "82_lblDiasTomados";
            // 
            // txtDiasVac
            // 
            this.txtDiasVac.Location = new System.Drawing.Point(163, 115);
            this.txtDiasVac.Name = "txtDiasVac";
            this.txtDiasVac.Properties.ReadOnly = true;
            this.txtDiasVac.Size = new System.Drawing.Size(35, 20);
            this.txtDiasVac.TabIndex = 6;
            // 
            // lblResolucion
            // 
            this.lblResolucion.AutoSize = true;
            this.lblResolucion.Location = new System.Drawing.Point(17, 92);
            this.lblResolucion.Name = "lblResolucion";
            this.lblResolucion.Size = new System.Drawing.Size(88, 13);
            this.lblResolucion.TabIndex = 5;
            this.lblResolucion.Text = "82_lblResolucion";
            // 
            // txtResolucion
            // 
            this.txtResolucion.Location = new System.Drawing.Point(111, 89);
            this.txtResolucion.Name = "txtResolucion";
            this.txtResolucion.Size = new System.Drawing.Size(87, 20);
            this.txtResolucion.TabIndex = 4;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Location = new System.Drawing.Point(17, 47);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(79, 13);
            this.lblFechaFin.TabIndex = 3;
            this.lblFechaFin.Text = "82_lblFechaFin";
            // 
            // lblFechaIni
            // 
            this.lblFechaIni.AutoSize = true;
            this.lblFechaIni.Location = new System.Drawing.Point(16, 20);
            this.lblFechaIni.Name = "lblFechaIni";
            this.lblFechaIni.Size = new System.Drawing.Size(76, 13);
            this.lblFechaIni.TabIndex = 2;
            this.lblFechaIni.Text = "82_lblFechaIni";
            // 
            // dtFechaFin
            // 
            this.dtFechaFin.EditValue = null;
            this.dtFechaFin.Location = new System.Drawing.Point(98, 43);
            this.dtFechaFin.Name = "dtFechaFin";
            this.dtFechaFin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFin.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFin.Size = new System.Drawing.Size(100, 20);
            this.dtFechaFin.TabIndex = 1;
            this.dtFechaFin.EditValueChanged += new System.EventHandler(this.dtFechaFin_EditValueChanged);
            // 
            // dtFechaIni
            // 
            this.dtFechaIni.EditValue = null;
            this.dtFechaIni.Location = new System.Drawing.Point(98, 17);
            this.dtFechaIni.Name = "dtFechaIni";
            this.dtFechaIni.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIni.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIni.Size = new System.Drawing.Size(100, 20);
            this.dtFechaIni.TabIndex = 0;
            this.dtFechaIni.EditValueChanged += new System.EventHandler(this.dtFechaIni_EditValueChanged);
            // 
            // txtTotalDeducciones
            // 
            this.txtTotalDeducciones.EditValue = 0D;
            this.txtTotalDeducciones.Location = new System.Drawing.Point(995, 39);
            this.txtTotalDeducciones.Name = "txtTotalDeducciones";
            this.txtTotalDeducciones.Properties.Mask.EditMask = "c0";
            this.txtTotalDeducciones.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalDeducciones.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalDeducciones.Properties.ReadOnly = true;
            this.txtTotalDeducciones.Size = new System.Drawing.Size(100, 20);
            this.txtTotalDeducciones.TabIndex = 11;
            // 
            // txtTotalPago
            // 
            this.txtTotalPago.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtTotalPago.Location = new System.Drawing.Point(995, 65);
            this.txtTotalPago.Name = "txtTotalPago";
            this.txtTotalPago.Properties.Mask.EditMask = "c0";
            this.txtTotalPago.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalPago.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalPago.Properties.ReadOnly = true;
            this.txtTotalPago.Size = new System.Drawing.Size(100, 20);
            this.txtTotalPago.TabIndex = 10;
            // 
            // txtTotalDevengos
            // 
            this.txtTotalDevengos.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtTotalDevengos.Location = new System.Drawing.Point(995, 13);
            this.txtTotalDevengos.Name = "txtTotalDevengos";
            this.txtTotalDevengos.Properties.Mask.EditMask = "c0";
            this.txtTotalDevengos.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalDevengos.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalDevengos.Properties.ReadOnly = true;
            this.txtTotalDevengos.Size = new System.Drawing.Size(100, 20);
            this.txtTotalDevengos.TabIndex = 9;
            // 
            // lblTotalPago
            // 
            this.lblTotalPago.AutoSize = true;
            this.lblTotalPago.Location = new System.Drawing.Point(873, 68);
            this.lblTotalPago.Name = "lblTotalPago";
            this.lblTotalPago.Size = new System.Drawing.Size(102, 13);
            this.lblTotalPago.TabIndex = 8;
            this.lblTotalPago.Text = "29107_lblTotalPago";
            // 
            // lblDeducciones
            // 
            this.lblDeducciones.AutoSize = true;
            this.lblDeducciones.Location = new System.Drawing.Point(873, 42);
            this.lblDeducciones.Name = "lblDeducciones";
            this.lblDeducciones.Size = new System.Drawing.Size(116, 13);
            this.lblDeducciones.TabIndex = 7;
            this.lblDeducciones.Text = "29107_lblDeducciones";
            // 
            // lblDevengos
            // 
            this.lblDevengos.AutoSize = true;
            this.lblDevengos.Location = new System.Drawing.Point(873, 16);
            this.lblDevengos.Name = "lblDevengos";
            this.lblDevengos.Size = new System.Drawing.Size(102, 13);
            this.lblDevengos.TabIndex = 6;
            this.lblDevengos.Text = "29107_lblDevengos";
            // 
            // LiquidacionVacacionesColectivas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "LiquidacionVacacionesColectivas";
            this.Text = "Liquidacion de Vacaciones";
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiasVac.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResolucion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFin.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIni.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIni.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalDeducciones.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalDevengos.Properties)).EndInit();
            this.ResumeLayout(false);

        }      

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblResolucion;
        private DevExpress.XtraEditors.TextEdit txtResolucion;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.Label lblFechaIni;
        private DevExpress.XtraEditors.DateEdit dtFechaFin;
        private DevExpress.XtraEditors.DateEdit dtFechaIni;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtDiasVac;
        private DevExpress.XtraEditors.TextEdit txtTotalDeducciones;
        private DevExpress.XtraEditors.TextEdit txtTotalPago;
        private DevExpress.XtraEditors.TextEdit txtTotalDevengos;
        private System.Windows.Forms.Label lblTotalPago;
        private System.Windows.Forms.Label lblDeducciones;
        private System.Windows.Forms.Label lblDevengos;
    }
}