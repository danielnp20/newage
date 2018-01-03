﻿namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RenovacionPoliza
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtFechaDoc = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaDoc = new DevExpress.XtraEditors.LabelControl();
            this.btnQueryPoliza = new DevExpress.XtraEditors.SimpleButton();
            this.lkpCreditos = new DevExpress.XtraEditors.LookUpEdit();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.txtPoliza = new System.Windows.Forms.TextBox();
            this.lblPoliza = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkFinanciacion = new System.Windows.Forms.CheckBox();
            this.lblFechaPoliza = new DevExpress.XtraEditors.LabelControl();
            this.masterSegurosAsesor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblEstado = new System.Windows.Forms.Label();
            this.dtFechaPago = new DevExpress.XtraEditors.DateEdit();
            this.dtFechaPoliza = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblInicioVigencia = new DevExpress.XtraEditors.LabelControl();
            this.txtEstadoPoliza = new System.Windows.Forms.TextBox();
            this.dtFechaInicio = new DevExpress.XtraEditors.DateEdit();
            this.lblVlrPoliza = new System.Windows.Forms.Label();
            this.masterAseguradora = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.lblFechaTermina = new DevExpress.XtraEditors.LabelControl();
            this.lblNCRevoca = new System.Windows.Forms.Label();
            this.dtFechaTermina = new DevExpress.XtraEditors.DateEdit();
            this.txtNCRevoca = new System.Windows.Forms.TextBox();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.cmbTipoPoliza = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoPoliza = new System.Windows.Forms.Label();
            this.masterComponente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpCreditos.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPoliza.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPoliza.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaTermina.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaTermina.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoPoliza.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.masterComponente);
            this.groupBox1.Controls.Add(this.dtFechaDoc);
            this.groupBox1.Controls.Add(this.lblFechaDoc);
            this.groupBox1.Controls.Add(this.btnQueryPoliza);
            this.groupBox1.Controls.Add(this.lkpCreditos);
            this.groupBox1.Controls.Add(this.masterCliente);
            this.groupBox1.Controls.Add(this.lblLibranza);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(41, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(882, 120);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "32054_Header";
            // 
            // dtFechaDoc
            // 
            this.dtFechaDoc.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaDoc.Location = new System.Drawing.Point(756, 26);
            this.dtFechaDoc.Name = "dtFechaDoc";
            this.dtFechaDoc.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaDoc.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaDoc.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaDoc.Properties.Appearance.Options.UseFont = true;
            this.dtFechaDoc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaDoc.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaDoc.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaDoc.Size = new System.Drawing.Size(98, 20);
            this.dtFechaDoc.TabIndex = 22;
            // 
            // lblFechaDoc
            // 
            this.lblFechaDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaDoc.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaDoc.Location = new System.Drawing.Point(652, 29);
            this.lblFechaDoc.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaDoc.Name = "lblFechaDoc";
            this.lblFechaDoc.Size = new System.Drawing.Size(98, 14);
            this.lblFechaDoc.TabIndex = 23;
            this.lblFechaDoc.Text = "Fecha Renovación";
            // 
            // btnQueryPoliza
            // 
            this.btnQueryPoliza.Appearance.Font = new System.Drawing.Font("Tahoma", 7.9F, System.Drawing.FontStyle.Bold);
            this.btnQueryPoliza.Appearance.Options.UseFont = true;
            this.btnQueryPoliza.Location = new System.Drawing.Point(376, 29);
            this.btnQueryPoliza.Name = "btnQueryPoliza";
            this.btnQueryPoliza.Size = new System.Drawing.Size(108, 21);
            this.btnQueryPoliza.TabIndex = 2;
            this.btnQueryPoliza.Text = "Consultar Pólizas";
            this.btnQueryPoliza.Click += new System.EventHandler(this.btnQueryPoliza_Click);
            // 
            // lkpCreditos
            // 
            this.lkpCreditos.Location = new System.Drawing.Point(101, 75);
            this.lkpCreditos.Name = "lkpCreditos";
            this.lkpCreditos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpCreditos.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Libranza", 40, "Libranza")});
            this.lkpCreditos.Properties.DisplayMember = "Libranza";
            this.lkpCreditos.Properties.NullText = "";
            this.lkpCreditos.Properties.ValueMember = "Libranza";
            this.lkpCreditos.Size = new System.Drawing.Size(117, 20);
            this.lkpCreditos.TabIndex = 8;
            this.lkpCreditos.EditValueChanged += new System.EventHandler(this.lkpCreditos_EditValueChanged);
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCliente.Location = new System.Drawing.Point(11, 29);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(359, 24);
            this.masterCliente.TabIndex = 0;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(8, 73);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(103, 14);
            this.lblLibranza.TabIndex = 15;
            this.lblLibranza.Text = "32504_lblLibranza";
            // 
            // txtPoliza
            // 
            this.txtPoliza.BackColor = System.Drawing.Color.LightBlue;
            this.txtPoliza.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoliza.Location = new System.Drawing.Point(123, 23);
            this.txtPoliza.Name = "txtPoliza";
            this.txtPoliza.Size = new System.Drawing.Size(132, 20);
            this.txtPoliza.TabIndex = 1;
            this.txtPoliza.Leave += new System.EventHandler(this.txtPoliza_Leave);
            // 
            // lblPoliza
            // 
            this.lblPoliza.AutoSize = true;
            this.lblPoliza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoliza.Location = new System.Drawing.Point(6, 26);
            this.lblPoliza.Name = "lblPoliza";
            this.lblPoliza.Size = new System.Drawing.Size(89, 14);
            this.lblPoliza.TabIndex = 13;
            this.lblPoliza.Text = "32504_lblPoliza";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkFinanciacion);
            this.groupBox2.Controls.Add(this.lblFechaPoliza);
            this.groupBox2.Controls.Add(this.masterSegurosAsesor);
            this.groupBox2.Controls.Add(this.lblEstado);
            this.groupBox2.Controls.Add(this.dtFechaPago);
            this.groupBox2.Controls.Add(this.dtFechaPoliza);
            this.groupBox2.Controls.Add(this.lblPoliza);
            this.groupBox2.Controls.Add(this.txtPoliza);
            this.groupBox2.Controls.Add(this.labelControl1);
            this.groupBox2.Controls.Add(this.lblInicioVigencia);
            this.groupBox2.Controls.Add(this.txtEstadoPoliza);
            this.groupBox2.Controls.Add(this.dtFechaInicio);
            this.groupBox2.Controls.Add(this.lblVlrPoliza);
            this.groupBox2.Controls.Add(this.masterAseguradora);
            this.groupBox2.Controls.Add(this.txtValor);
            this.groupBox2.Controls.Add(this.lblFechaTermina);
            this.groupBox2.Controls.Add(this.lblNCRevoca);
            this.groupBox2.Controls.Add(this.dtFechaTermina);
            this.groupBox2.Controls.Add(this.txtNCRevoca);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(41, 203);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(882, 222);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "32054_Footer";
            // 
            // chkFinanciacion
            // 
            this.chkFinanciacion.AutoSize = true;
            this.chkFinanciacion.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.chkFinanciacion.Location = new System.Drawing.Point(731, 65);
            this.chkFinanciacion.Name = "chkFinanciacion";
            this.chkFinanciacion.Size = new System.Drawing.Size(101, 17);
            this.chkFinanciacion.TabIndex = 8;
            this.chkFinanciacion.Text = "32054_Financia";
            this.chkFinanciacion.UseVisualStyleBackColor = true;
            // 
            // lblFechaPoliza
            // 
            this.lblFechaPoliza.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaPoliza.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaPoliza.Location = new System.Drawing.Point(8, 108);
            this.lblFechaPoliza.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaPoliza.Name = "lblFechaPoliza";
            this.lblFechaPoliza.Size = new System.Drawing.Size(114, 14);
            this.lblFechaPoliza.TabIndex = 18;
            this.lblFechaPoliza.Text = "32504_lblFechaPoliza";
            // 
            // masterSegurosAsesor
            // 
            this.masterSegurosAsesor.BackColor = System.Drawing.Color.Transparent;
            this.masterSegurosAsesor.Filtros = null;
            this.masterSegurosAsesor.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterSegurosAsesor.Location = new System.Drawing.Point(365, 60);
            this.masterSegurosAsesor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterSegurosAsesor.Name = "masterSegurosAsesor";
            this.masterSegurosAsesor.Size = new System.Drawing.Size(359, 24);
            this.masterSegurosAsesor.TabIndex = 1;
            this.masterSegurosAsesor.Value = "";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(502, 107);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(97, 14);
            this.lblEstado.TabIndex = 17;
            this.lblEstado.Text = "32504_lblEstado";
            // 
            // dtFechaPago
            // 
            this.dtFechaPago.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaPago.Location = new System.Drawing.Point(623, 147);
            this.dtFechaPago.Name = "dtFechaPago";
            this.dtFechaPago.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaPago.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaPago.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaPago.Properties.Appearance.Options.UseFont = true;
            this.dtFechaPago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaPago.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaPago.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaPago.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaPago.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaPago.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaPago.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaPago.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaPago.Size = new System.Drawing.Size(98, 20);
            this.dtFechaPago.TabIndex = 7;
            // 
            // dtFechaPoliza
            // 
            this.dtFechaPoliza.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaPoliza.Location = new System.Drawing.Point(126, 105);
            this.dtFechaPoliza.Name = "dtFechaPoliza";
            this.dtFechaPoliza.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaPoliza.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaPoliza.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaPoliza.Properties.Appearance.Options.UseFont = true;
            this.dtFechaPoliza.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaPoliza.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaPoliza.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaPoliza.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaPoliza.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaPoliza.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaPoliza.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaPoliza.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaPoliza.Size = new System.Drawing.Size(114, 20);
            this.dtFechaPoliza.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl1.Location = new System.Drawing.Point(505, 150);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(112, 14);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "32504_lblFechaPago";
            // 
            // lblInicioVigencia
            // 
            this.lblInicioVigencia.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInicioVigencia.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblInicioVigencia.Location = new System.Drawing.Point(8, 150);
            this.lblInicioVigencia.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblInicioVigencia.Name = "lblInicioVigencia";
            this.lblInicioVigencia.Size = new System.Drawing.Size(113, 14);
            this.lblInicioVigencia.TabIndex = 16;
            this.lblInicioVigencia.Text = "32504_lblFechaInicio";
            // 
            // txtEstadoPoliza
            // 
            this.txtEstadoPoliza.Enabled = false;
            this.txtEstadoPoliza.Location = new System.Drawing.Point(624, 104);
            this.txtEstadoPoliza.Name = "txtEstadoPoliza";
            this.txtEstadoPoliza.Size = new System.Drawing.Size(120, 21);
            this.txtEstadoPoliza.TabIndex = 4;
            // 
            // dtFechaInicio
            // 
            this.dtFechaInicio.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicio.Location = new System.Drawing.Point(126, 147);
            this.dtFechaInicio.Name = "dtFechaInicio";
            this.dtFechaInicio.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaInicio.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaInicio.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaInicio.Properties.Appearance.Options.UseFont = true;
            this.dtFechaInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaInicio.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaInicio.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicio.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaInicio.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaInicio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaInicio.Size = new System.Drawing.Size(114, 20);
            this.dtFechaInicio.TabIndex = 5;
            // 
            // lblVlrPoliza
            // 
            this.lblVlrPoliza.AutoSize = true;
            this.lblVlrPoliza.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrPoliza.Location = new System.Drawing.Point(260, 107);
            this.lblVlrPoliza.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblVlrPoliza.Name = "lblVlrPoliza";
            this.lblVlrPoliza.Size = new System.Drawing.Size(80, 16);
            this.lblVlrPoliza.TabIndex = 12;
            this.lblVlrPoliza.Text = "32504_Valor";
            // 
            // masterAseguradora
            // 
            this.masterAseguradora.BackColor = System.Drawing.Color.Transparent;
            this.masterAseguradora.Filtros = null;
            this.masterAseguradora.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterAseguradora.Location = new System.Drawing.Point(9, 58);
            this.masterAseguradora.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterAseguradora.Name = "masterAseguradora";
            this.masterAseguradora.Size = new System.Drawing.Size(359, 24);
            this.masterAseguradora.TabIndex = 0;
            this.masterAseguradora.Value = "";
            // 
            // txtValor
            // 
            this.txtValor.EditValue = "0";
            this.txtValor.Location = new System.Drawing.Point(381, 105);
            this.txtValor.Margin = new System.Windows.Forms.Padding(1);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Properties.Appearance.Options.UseFont = true;
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.Mask.EditMask = "c0";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Size = new System.Drawing.Size(98, 20);
            this.txtValor.TabIndex = 3;
            // 
            // lblFechaTermina
            // 
            this.lblFechaTermina.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaTermina.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaTermina.Location = new System.Drawing.Point(263, 150);
            this.lblFechaTermina.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaTermina.Name = "lblFechaTermina";
            this.lblFechaTermina.Size = new System.Drawing.Size(129, 14);
            this.lblFechaTermina.TabIndex = 14;
            this.lblFechaTermina.Text = "32504_lblFechaTermina";
            // 
            // lblNCRevoca
            // 
            this.lblNCRevoca.AutoSize = true;
            this.lblNCRevoca.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNCRevoca.Location = new System.Drawing.Point(6, 191);
            this.lblNCRevoca.Name = "lblNCRevoca";
            this.lblNCRevoca.Size = new System.Drawing.Size(114, 14);
            this.lblNCRevoca.TabIndex = 9;
            this.lblNCRevoca.Text = "32504_lblNCRevoca";
            this.lblNCRevoca.Visible = false;
            // 
            // dtFechaTermina
            // 
            this.dtFechaTermina.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaTermina.Location = new System.Drawing.Point(381, 147);
            this.dtFechaTermina.Name = "dtFechaTermina";
            this.dtFechaTermina.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaTermina.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaTermina.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaTermina.Properties.Appearance.Options.UseFont = true;
            this.dtFechaTermina.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaTermina.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaTermina.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaTermina.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaTermina.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaTermina.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaTermina.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaTermina.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaTermina.Size = new System.Drawing.Size(98, 20);
            this.dtFechaTermina.TabIndex = 6;
            // 
            // txtNCRevoca
            // 
            this.txtNCRevoca.Location = new System.Drawing.Point(125, 188);
            this.txtNCRevoca.Name = "txtNCRevoca";
            this.txtNCRevoca.Size = new System.Drawing.Size(68, 21);
            this.txtNCRevoca.TabIndex = 8;
            this.txtNCRevoca.Visible = false;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.cmbTipoPoliza);
            this.panelControl3.Controls.Add(this.groupBox2);
            this.panelControl3.Controls.Add(this.lblTipoPoliza);
            this.panelControl3.Controls.Add(this.groupBox1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(949, 488);
            this.panelControl3.TabIndex = 16;
            // 
            // cmbTipoPoliza
            // 
            this.cmbTipoPoliza.Location = new System.Drawing.Point(151, 20);
            this.cmbTipoPoliza.Name = "cmbTipoPoliza";
            this.cmbTipoPoliza.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoPoliza.Properties.NullText = "";
            this.cmbTipoPoliza.Size = new System.Drawing.Size(117, 20);
            this.cmbTipoPoliza.TabIndex = 24;
            this.cmbTipoPoliza.EditValueChanged += new System.EventHandler(this.cmbTipoPoliza_EditValueChanged);
            // 
            // lblTipoPoliza
            // 
            this.lblTipoPoliza.AutoSize = true;
            this.lblTipoPoliza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoPoliza.Location = new System.Drawing.Point(47, 23);
            this.lblTipoPoliza.Name = "lblTipoPoliza";
            this.lblTipoPoliza.Size = new System.Drawing.Size(72, 14);
            this.lblTipoPoliza.TabIndex = 25;
            this.lblTipoPoliza.Text = "Tipo Póliza";
            // 
            // masterComponente
            // 
            this.masterComponente.BackColor = System.Drawing.Color.Transparent;
            this.masterComponente.Filtros = null;
            this.masterComponente.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterComponente.Location = new System.Drawing.Point(240, 71);
            this.masterComponente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterComponente.Name = "masterComponente";
            this.masterComponente.Size = new System.Drawing.Size(359, 24);
            this.masterComponente.TabIndex = 24;
            this.masterComponente.Value = "";
            this.masterComponente.Visible = false;
            // 
            // RenovacionPoliza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 488);
            this.Controls.Add(this.panelControl3);
            this.Name = "RenovacionPoliza";
            this.Text = "RenovacionPoliza";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpCreditos.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPoliza.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPoliza.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaTermina.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaTermina.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoPoliza.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnQueryPoliza;
        private DevExpress.XtraEditors.LookUpEdit lkpCreditos;
        private ControlsUC.uc_MasterFind masterCliente;
        private System.Windows.Forms.Label lblLibranza;
        private System.Windows.Forms.TextBox txtPoliza;
        private System.Windows.Forms.Label lblPoliza;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkFinanciacion;
        private DevExpress.XtraEditors.LabelControl lblFechaPoliza;
        private ControlsUC.uc_MasterFind masterSegurosAsesor;
        private System.Windows.Forms.Label lblEstado;
        private DevExpress.XtraEditors.DateEdit dtFechaPago;
        private DevExpress.XtraEditors.DateEdit dtFechaPoliza;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblInicioVigencia;
        private System.Windows.Forms.TextBox txtEstadoPoliza;
        private DevExpress.XtraEditors.DateEdit dtFechaInicio;
        private System.Windows.Forms.Label lblVlrPoliza;
        private ControlsUC.uc_MasterFind masterAseguradora;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private DevExpress.XtraEditors.LabelControl lblFechaTermina;
        private System.Windows.Forms.Label lblNCRevoca;
        private DevExpress.XtraEditors.DateEdit dtFechaTermina;
        private System.Windows.Forms.TextBox txtNCRevoca;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.DateEdit dtFechaDoc;
        private DevExpress.XtraEditors.LabelControl lblFechaDoc;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoPoliza;
        private System.Windows.Forms.Label lblTipoPoliza;
        private ControlsUC.uc_MasterFind masterComponente;
    }
}