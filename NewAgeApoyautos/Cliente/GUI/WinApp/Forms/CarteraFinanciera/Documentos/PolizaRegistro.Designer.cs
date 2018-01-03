namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PolizaRegistro
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
            this.lkpTipoMvto = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoMvto = new System.Windows.Forms.Label();
            this.chkFinanciacion = new System.Windows.Forms.CheckBox();
            this.masterSegurosAsesor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaPago = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtEstadoPoliza = new System.Windows.Forms.TextBox();
            this.lblVlrPoliza = new System.Windows.Forms.Label();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.lblNCRevoca = new System.Windows.Forms.Label();
            this.txtNCRevoca = new System.Windows.Forms.TextBox();
            this.dtFechaTermina = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaTermina = new DevExpress.XtraEditors.LabelControl();
            this.masterAseguradora = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaInicio = new DevExpress.XtraEditors.DateEdit();
            this.lblInicioVigencia = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaPoliza = new DevExpress.XtraEditors.DateEdit();
            this.lblEstado = new System.Windows.Forms.Label();
            this.lblFechaPoliza = new DevExpress.XtraEditors.LabelControl();
            this.lkpCreditos = new DevExpress.XtraEditors.LookUpEdit();
            this.lkpSolicitudes = new DevExpress.XtraEditors.LookUpEdit();
            this.lblSolicitud = new System.Windows.Forms.Label();
            this.dtFechaMvto = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaMvto = new DevExpress.XtraEditors.LabelControl();
            this.masterTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblPoliza = new System.Windows.Forms.Label();
            this.txtPoliza = new System.Windows.Forms.TextBox();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnQueryPoliza = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkColectivaInd = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoMvto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaTermina.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaTermina.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPoliza.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPoliza.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpCreditos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpSolicitudes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaMvto.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaMvto.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lkpTipoMvto
            // 
            this.lkpTipoMvto.Location = new System.Drawing.Point(167, 70);
            this.lkpTipoMvto.Name = "lkpTipoMvto";
            this.lkpTipoMvto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpTipoMvto.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkpTipoMvto.Properties.DisplayMember = "Value";
            this.lkpTipoMvto.Properties.NullText = " ";
            this.lkpTipoMvto.Properties.ValueMember = "Key";
            this.lkpTipoMvto.Size = new System.Drawing.Size(182, 20);
            this.lkpTipoMvto.TabIndex = 1;
            this.lkpTipoMvto.EditValueChanged += new System.EventHandler(this.lkpTipoMvto_EditValueChanged);
            // 
            // lblTipoMvto
            // 
            this.lblTipoMvto.AutoSize = true;
            this.lblTipoMvto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoMvto.Location = new System.Drawing.Point(48, 73);
            this.lblTipoMvto.Name = "lblTipoMvto";
            this.lblTipoMvto.Size = new System.Drawing.Size(127, 14);
            this.lblTipoMvto.TabIndex = 1;
            this.lblTipoMvto.Text = "32504_lblTipoMvto";
            // 
            // chkFinanciacion
            // 
            this.chkFinanciacion.AutoSize = true;
            this.chkFinanciacion.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.chkFinanciacion.Location = new System.Drawing.Point(731, 37);
            this.chkFinanciacion.Name = "chkFinanciacion";
            this.chkFinanciacion.Size = new System.Drawing.Size(101, 17);
            this.chkFinanciacion.TabIndex = 8;
            this.chkFinanciacion.Text = "32054_Financia";
            this.chkFinanciacion.UseVisualStyleBackColor = true;
            // 
            // masterSegurosAsesor
            // 
            this.masterSegurosAsesor.BackColor = System.Drawing.Color.Transparent;
            this.masterSegurosAsesor.Filtros = null;
            this.masterSegurosAsesor.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterSegurosAsesor.Location = new System.Drawing.Point(365, 32);
            this.masterSegurosAsesor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterSegurosAsesor.Name = "masterSegurosAsesor";
            this.masterSegurosAsesor.Size = new System.Drawing.Size(359, 24);
            this.masterSegurosAsesor.TabIndex = 1;
            this.masterSegurosAsesor.Value = "";
            this.masterSegurosAsesor.Leave += new System.EventHandler(this.masterSegurosAsesor_Leave);
            // 
            // dtFechaPago
            // 
            this.dtFechaPago.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaPago.Location = new System.Drawing.Point(623, 119);
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
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl1.Location = new System.Drawing.Point(505, 122);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(112, 14);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "32504_lblFechaPago";
            // 
            // txtEstadoPoliza
            // 
            this.txtEstadoPoliza.Enabled = false;
            this.txtEstadoPoliza.Location = new System.Drawing.Point(624, 76);
            this.txtEstadoPoliza.Name = "txtEstadoPoliza";
            this.txtEstadoPoliza.Size = new System.Drawing.Size(120, 21);
            this.txtEstadoPoliza.TabIndex = 4;
            // 
            // lblVlrPoliza
            // 
            this.lblVlrPoliza.AutoSize = true;
            this.lblVlrPoliza.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrPoliza.Location = new System.Drawing.Point(260, 79);
            this.lblVlrPoliza.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblVlrPoliza.Name = "lblVlrPoliza";
            this.lblVlrPoliza.Size = new System.Drawing.Size(80, 16);
            this.lblVlrPoliza.TabIndex = 12;
            this.lblVlrPoliza.Text = "32504_Valor";
            // 
            // txtValor
            // 
            this.txtValor.EditValue = "0";
            this.txtValor.Location = new System.Drawing.Point(381, 77);
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
            // lblNCRevoca
            // 
            this.lblNCRevoca.AutoSize = true;
            this.lblNCRevoca.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNCRevoca.Location = new System.Drawing.Point(6, 163);
            this.lblNCRevoca.Name = "lblNCRevoca";
            this.lblNCRevoca.Size = new System.Drawing.Size(114, 14);
            this.lblNCRevoca.TabIndex = 9;
            this.lblNCRevoca.Text = "32504_lblNCRevoca";
            this.lblNCRevoca.Visible = false;
            // 
            // txtNCRevoca
            // 
            this.txtNCRevoca.Location = new System.Drawing.Point(125, 160);
            this.txtNCRevoca.Name = "txtNCRevoca";
            this.txtNCRevoca.Size = new System.Drawing.Size(68, 21);
            this.txtNCRevoca.TabIndex = 8;
            this.txtNCRevoca.Visible = false;
            // 
            // dtFechaTermina
            // 
            this.dtFechaTermina.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaTermina.Location = new System.Drawing.Point(381, 119);
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
            // lblFechaTermina
            // 
            this.lblFechaTermina.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaTermina.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaTermina.Location = new System.Drawing.Point(263, 122);
            this.lblFechaTermina.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaTermina.Name = "lblFechaTermina";
            this.lblFechaTermina.Size = new System.Drawing.Size(129, 14);
            this.lblFechaTermina.TabIndex = 14;
            this.lblFechaTermina.Text = "32504_lblFechaTermina";
            // 
            // masterAseguradora
            // 
            this.masterAseguradora.BackColor = System.Drawing.Color.Transparent;
            this.masterAseguradora.Filtros = null;
            this.masterAseguradora.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterAseguradora.Location = new System.Drawing.Point(9, 30);
            this.masterAseguradora.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterAseguradora.Name = "masterAseguradora";
            this.masterAseguradora.Size = new System.Drawing.Size(359, 24);
            this.masterAseguradora.TabIndex = 0;
            this.masterAseguradora.Value = "";
            this.masterAseguradora.Leave += new System.EventHandler(this.masterAseguradora_Leave);
            // 
            // dtFechaInicio
            // 
            this.dtFechaInicio.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaInicio.Location = new System.Drawing.Point(126, 119);
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
            this.dtFechaInicio.EditValueChanged += new System.EventHandler(this.dtFechaInicio_EditValueChanged);
            // 
            // lblInicioVigencia
            // 
            this.lblInicioVigencia.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInicioVigencia.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblInicioVigencia.Location = new System.Drawing.Point(8, 122);
            this.lblInicioVigencia.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblInicioVigencia.Name = "lblInicioVigencia";
            this.lblInicioVigencia.Size = new System.Drawing.Size(113, 14);
            this.lblInicioVigencia.TabIndex = 16;
            this.lblInicioVigencia.Text = "32504_lblFechaInicio";
            // 
            // dtFechaPoliza
            // 
            this.dtFechaPoliza.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaPoliza.Location = new System.Drawing.Point(126, 77);
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
            this.dtFechaPoliza.EditValueChanged += new System.EventHandler(this.dtFechaPoliza_EditValueChanged);
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(502, 79);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(97, 14);
            this.lblEstado.TabIndex = 17;
            this.lblEstado.Text = "32504_lblEstado";
            // 
            // lblFechaPoliza
            // 
            this.lblFechaPoliza.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaPoliza.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaPoliza.Location = new System.Drawing.Point(8, 80);
            this.lblFechaPoliza.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaPoliza.Name = "lblFechaPoliza";
            this.lblFechaPoliza.Size = new System.Drawing.Size(114, 14);
            this.lblFechaPoliza.TabIndex = 18;
            this.lblFechaPoliza.Text = "32504_lblFechaPoliza";
            // 
            // lkpCreditos
            // 
            this.lkpCreditos.Location = new System.Drawing.Point(396, 79);
            this.lkpCreditos.Name = "lkpCreditos";
            this.lkpCreditos.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lkpCreditos.Properties.Appearance.Options.UseFont = true;
            this.lkpCreditos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpCreditos.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "Value")});
            this.lkpCreditos.Properties.DisplayMember = "Value";
            this.lkpCreditos.Properties.NullText = " ";
            this.lkpCreditos.Properties.ValueMember = "Value";
            this.lkpCreditos.Size = new System.Drawing.Size(98, 20);
            this.lkpCreditos.TabIndex = 4;
            this.lkpCreditos.EditValueChanged += new System.EventHandler(this.lkpCreditos_EditValueChanged);
            // 
            // lkpSolicitudes
            // 
            this.lkpSolicitudes.Location = new System.Drawing.Point(141, 79);
            this.lkpSolicitudes.Name = "lkpSolicitudes";
            this.lkpSolicitudes.Properties.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lkpSolicitudes.Properties.Appearance.Options.UseFont = true;
            this.lkpSolicitudes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpSolicitudes.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "Value")});
            this.lkpSolicitudes.Properties.DisplayMember = "Value";
            this.lkpSolicitudes.Properties.NullText = " ";
            this.lkpSolicitudes.Properties.ValueMember = "Value";
            this.lkpSolicitudes.Size = new System.Drawing.Size(98, 20);
            this.lkpSolicitudes.TabIndex = 3;
            this.lkpSolicitudes.EditValueChanged += new System.EventHandler(this.lkpSolicitudes_EditValueChanged);
            // 
            // lblSolicitud
            // 
            this.lblSolicitud.AutoSize = true;
            this.lblSolicitud.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolicitud.Location = new System.Drawing.Point(21, 81);
            this.lblSolicitud.Name = "lblSolicitud";
            this.lblSolicitud.Size = new System.Drawing.Size(105, 14);
            this.lblSolicitud.TabIndex = 10;
            this.lblSolicitud.Text = "32504_lblSolicitud";
            // 
            // dtFechaMvto
            // 
            this.dtFechaMvto.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaMvto.Location = new System.Drawing.Point(514, 69);
            this.dtFechaMvto.Name = "dtFechaMvto";
            this.dtFechaMvto.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaMvto.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaMvto.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaMvto.Properties.Appearance.Options.UseFont = true;
            this.dtFechaMvto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaMvto.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaMvto.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaMvto.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaMvto.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaMvto.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaMvto.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaMvto.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaMvto.Size = new System.Drawing.Size(114, 20);
            this.dtFechaMvto.TabIndex = 2;
            // 
            // lblFechaMvto
            // 
            this.lblFechaMvto.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaMvto.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaMvto.Location = new System.Drawing.Point(400, 72);
            this.lblFechaMvto.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaMvto.Name = "lblFechaMvto";
            this.lblFechaMvto.Size = new System.Drawing.Size(128, 14);
            this.lblFechaMvto.TabIndex = 2;
            this.lblFechaMvto.Text = "32504_lblFechaMvto";
            // 
            // masterTercero
            // 
            this.masterTercero.BackColor = System.Drawing.Color.Transparent;
            this.masterTercero.Filtros = null;
            this.masterTercero.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterTercero.Location = new System.Drawing.Point(23, 30);
            this.masterTercero.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterTercero.Name = "masterTercero";
            this.masterTercero.Size = new System.Drawing.Size(359, 24);
            this.masterTercero.TabIndex = 0;
            this.masterTercero.Value = "";
            this.masterTercero.Leave += new System.EventHandler(this.masterTercero_Leave);
            // 
            // lblPoliza
            // 
            this.lblPoliza.AutoSize = true;
            this.lblPoliza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoliza.Location = new System.Drawing.Point(393, 36);
            this.lblPoliza.Name = "lblPoliza";
            this.lblPoliza.Size = new System.Drawing.Size(89, 14);
            this.lblPoliza.TabIndex = 13;
            this.lblPoliza.Text = "32504_lblPoliza";
            // 
            // txtPoliza
            // 
            this.txtPoliza.BackColor = System.Drawing.Color.LightBlue;
            this.txtPoliza.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoliza.Location = new System.Drawing.Point(510, 33);
            this.txtPoliza.Name = "txtPoliza";
            this.txtPoliza.Size = new System.Drawing.Size(132, 20);
            this.txtPoliza.TabIndex = 1;
            this.txtPoliza.Leave += new System.EventHandler(this.txtPoliza_Leave);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(275, 81);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(103, 14);
            this.lblLibranza.TabIndex = 15;
            this.lblLibranza.Text = "32504_lblLibranza";
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(113, 33);
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(6);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 18);
            this.dtPeriod.TabIndex = 0;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(51, 36);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(93, 14);
            this.lblPeriod.TabIndex = 0;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnQueryPoliza);
            this.groupBox1.Controls.Add(this.lkpCreditos);
            this.groupBox1.Controls.Add(this.masterTercero);
            this.groupBox1.Controls.Add(this.lblLibranza);
            this.groupBox1.Controls.Add(this.lkpSolicitudes);
            this.groupBox1.Controls.Add(this.lblSolicitud);
            this.groupBox1.Controls.Add(this.lblPoliza);
            this.groupBox1.Controls.Add(this.txtPoliza);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(41, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(882, 141);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "32054_Header";
            // 
            // btnQueryPoliza
            // 
            this.btnQueryPoliza.Appearance.Font = new System.Drawing.Font("Tahoma", 7.9F, System.Drawing.FontStyle.Bold);
            this.btnQueryPoliza.Appearance.Options.UseFont = true;
            this.btnQueryPoliza.Location = new System.Drawing.Point(649, 33);
            this.btnQueryPoliza.Name = "btnQueryPoliza";
            this.btnQueryPoliza.Size = new System.Drawing.Size(108, 21);
            this.btnQueryPoliza.TabIndex = 2;
            this.btnQueryPoliza.Text = "Consultar Pólizas";
            this.btnQueryPoliza.Click += new System.EventHandler(this.btnQueryPoliza_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.groupBox2);
            this.panelControl3.Controls.Add(this.groupBox1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1267, 488);
            this.panelControl3.TabIndex = 16;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkColectivaInd);
            this.groupBox2.Controls.Add(this.chkFinanciacion);
            this.groupBox2.Controls.Add(this.lblFechaPoliza);
            this.groupBox2.Controls.Add(this.masterSegurosAsesor);
            this.groupBox2.Controls.Add(this.lblEstado);
            this.groupBox2.Controls.Add(this.dtFechaPago);
            this.groupBox2.Controls.Add(this.dtFechaPoliza);
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
            this.groupBox2.Location = new System.Drawing.Point(41, 270);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(882, 206);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "32054_Footer";
            // 
            // chkColectivaInd
            // 
            this.chkColectivaInd.AutoSize = true;
            this.chkColectivaInd.Enabled = false;
            this.chkColectivaInd.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.chkColectivaInd.Location = new System.Drawing.Point(730, 53);
            this.chkColectivaInd.Name = "chkColectivaInd";
            this.chkColectivaInd.Size = new System.Drawing.Size(106, 17);
            this.chkColectivaInd.TabIndex = 22;
            this.chkColectivaInd.Text = "32054_Colectiva";
            this.chkColectivaInd.UseVisualStyleBackColor = true;
            // 
            // PolizaRegistro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1267, 488);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lkpTipoMvto);
            this.Controls.Add(this.lblTipoMvto);
            this.Controls.Add(this.dtFechaMvto);
            this.Controls.Add(this.lblFechaMvto);
            this.Controls.Add(this.panelControl3);
            this.Name = "PolizaRegistro";
            this.Text = "PolizaRegistro";
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoMvto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaTermina.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaTermina.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaInicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPoliza.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaPoliza.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpCreditos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpSolicitudes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaMvto.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaMvto.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lkpTipoMvto;
        private System.Windows.Forms.Label lblTipoMvto;
        private DevExpress.XtraEditors.LookUpEdit lkpCreditos;
        private DevExpress.XtraEditors.LookUpEdit lkpSolicitudes;
        private System.Windows.Forms.Label lblNCRevoca;
        private System.Windows.Forms.TextBox txtNCRevoca;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind masterAseguradora;
        private System.Windows.Forms.Label lblSolicitud;
        private DevExpress.XtraEditors.DateEdit dtFechaMvto;
        private DevExpress.XtraEditors.LabelControl lblFechaMvto;
        private System.Windows.Forms.Label lblVlrPoliza;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private DevExpress.XtraEditors.DateEdit dtFechaTermina;
        private DevExpress.XtraEditors.LabelControl lblFechaTermina;
        private DevExpress.XtraEditors.DateEdit dtFechaInicio;
        private System.Windows.Forms.Label lblLibranza;
        private DevExpress.XtraEditors.LabelControl lblInicioVigencia;
        private DevExpress.XtraEditors.DateEdit dtFechaPoliza;
        private System.Windows.Forms.Label lblEstado;
        private DevExpress.XtraEditors.LabelControl lblFechaPoliza;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind masterTercero;
        private System.Windows.Forms.Label lblPoliza;
        private System.Windows.Forms.TextBox txtPoliza;
        protected ControlsUC.uc_PeriodoEdit dtPeriod;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        private System.Windows.Forms.TextBox txtEstadoPoliza;
        private DevExpress.XtraEditors.DateEdit dtFechaPago;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private ControlsUC.uc_MasterFind masterSegurosAsesor;
        private System.Windows.Forms.CheckBox chkFinanciacion;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.SimpleButton btnQueryPoliza;
        private System.Windows.Forms.CheckBox chkColectivaInd;
    }
}