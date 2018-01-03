namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class VentaCartera
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterCompradorCartera = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.editSpinPorc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.lblFechaPago = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaFlujo = new DevExpress.XtraEditors.DateEdit();
            this.lblOferta = new System.Windows.Forms.Label();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.txtTasaMensual = new DevExpress.XtraEditors.TextEdit();
            this.txtCartaAceptacion = new System.Windows.Forms.TextBox();
            this.lblCartaAceptacion = new System.Windows.Forms.Label();
            this.txtVlrTotalNeto = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrTotalNeto = new System.Windows.Forms.Label();
            this.txtVlrTotalVenta = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrTotalVenta = new System.Windows.Forms.Label();
            this.txtVlrTotalNominal = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrTotalNominal = new System.Windows.Forms.Label();
            this.lblTasaAnual = new System.Windows.Forms.Label();
            this.lblTasaMensual = new System.Windows.Forms.Label();
            this.txtTasaAnual = new DevExpress.XtraEditors.TextEdit();
            this.lblFechaAceptacion = new DevExpress.XtraEditors.LabelControl();
            this.dtFechaLiquida = new DevExpress.XtraEditors.DateEdit();
            this.lkp_Oferta = new DevExpress.XtraEditors.LookUpEdit();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFlujo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFlujo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaMensual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalNeto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalVenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalNominal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaAnual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaLiquida.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaLiquida.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Oferta.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.txtVlrTotalNominal);
            this.grpboxDetail.Controls.Add(this.lblVlrTotalNominal);
            this.grpboxDetail.Controls.Add(this.txtVlrTotalNeto);
            this.grpboxDetail.Controls.Add(this.lblVlrTotalNeto);
            this.grpboxDetail.Controls.Add(this.txtVlrTotalVenta);
            this.grpboxDetail.Controls.Add(this.lblVlrTotalVenta);
            this.grpboxDetail.Location = new System.Drawing.Point(12, 17);
            this.grpboxDetail.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Size = new System.Drawing.Size(790, 67);
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
            this.editSpin7,
            this.editDate,
            this.editValue,
            this.editValue4,
            this.editLink,
            this.editSpinPorcen,
            this.editSpin0,
            this.editSpinPorc});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c0";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin4
            // 
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin7
            // 
            this.editSpin7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.Mask.EditMask = "c7";
            this.editSpin7.Mask.UseMaskAsDisplayFormat = true;
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
            // btnMark
            // 
            this.btnMark.Margin = new System.Windows.Forms.Padding(12);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(6);
            this.txtDocumentoID.Size = new System.Drawing.Size(58, 18);
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtDocDesc.Size = new System.Drawing.Size(217, 16);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(6);
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 18);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(12);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(6);
            this.txtPrefix.Size = new System.Drawing.Size(50, 18);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtPeriod
            // 
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.dtPeriod.Size = new System.Drawing.Size(130, 16);
            // 
            // txtAF
            // 
            this.txtAF.Margin = new System.Windows.Forms.Padding(6);
            this.txtAF.Size = new System.Drawing.Size(91, 16);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(12, 0, 12, 6);
            this.gbGridDocument.Size = new System.Drawing.Size(579, 245);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(579, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 245);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.lkp_Oferta);
            this.grpboxHeader.Controls.Add(this.txtTasaAnual);
            this.grpboxHeader.Controls.Add(this.lblTasaAnual);
            this.grpboxHeader.Controls.Add(this.lblTasaMensual);
            this.grpboxHeader.Controls.Add(this.txtCartaAceptacion);
            this.grpboxHeader.Controls.Add(this.lblCartaAceptacion);
            this.grpboxHeader.Controls.Add(this.txtTasaMensual);
            this.grpboxHeader.Controls.Add(this.txtObservacion);
            this.grpboxHeader.Controls.Add(this.lblObservaciones);
            this.grpboxHeader.Controls.Add(this.lblOferta);
            this.grpboxHeader.Controls.Add(this.lblFechaPago);
            this.grpboxHeader.Controls.Add(this.dtFechaFlujo);
            this.grpboxHeader.Controls.Add(this.dtFechaLiquida);
            this.grpboxHeader.Controls.Add(this.masterCompradorCartera);
            this.grpboxHeader.Controls.Add(this.lblFechaAceptacion);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(871, 142);
            this.grpboxHeader.TabIndex = 0;
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "c0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // masterCompradorCartera
            // 
            this.masterCompradorCartera.BackColor = System.Drawing.Color.Transparent;
            this.masterCompradorCartera.Filtros = null;
            this.masterCompradorCartera.Location = new System.Drawing.Point(6, 10);
            this.masterCompradorCartera.Margin = new System.Windows.Forms.Padding(6);
            this.masterCompradorCartera.Name = "masterCompradorCartera";
            this.masterCompradorCartera.Size = new System.Drawing.Size(296, 25);
            this.masterCompradorCartera.TabIndex = 0;
            this.masterCompradorCartera.Value = "";
            this.masterCompradorCartera.Leave += new System.EventHandler(this.masterCompradorCartera_Leave);
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P2";
            this.editSpinPorc.Name = "editSpinPorc";
            // 
            // lblFechaPago
            // 
            this.lblFechaPago.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaPago.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaPago.Location = new System.Drawing.Point(313, 44);
            this.lblFechaPago.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblFechaPago.Name = "lblFechaPago";
            this.lblFechaPago.Size = new System.Drawing.Size(87, 14);
            this.lblFechaPago.TabIndex = 5;
            this.lblFechaPago.Text = "164_FechaPago";
            // 
            // dtFechaFlujo
            // 
            this.dtFechaFlujo.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaFlujo.Enabled = false;
            this.dtFechaFlujo.Location = new System.Drawing.Point(420, 42);
            this.dtFechaFlujo.Margin = new System.Windows.Forms.Padding(1);
            this.dtFechaFlujo.Name = "dtFechaFlujo";
            this.dtFechaFlujo.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFlujo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaFlujo.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFlujo.Properties.Appearance.Options.UseFont = true;
            this.dtFechaFlujo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFlujo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFlujo.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFlujo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFlujo.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFlujo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFlujo.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFlujo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFlujo.Size = new System.Drawing.Size(95, 20);
            this.dtFechaFlujo.TabIndex = 6;
            // 
            // lblOferta
            // 
            this.lblOferta.AutoSize = true;
            this.lblOferta.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOferta.Location = new System.Drawing.Point(310, 16);
            this.lblOferta.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblOferta.Name = "lblOferta";
            this.lblOferta.Size = new System.Drawing.Size(70, 14);
            this.lblOferta.TabIndex = 1;
            this.lblOferta.Text = "164_Oferta";
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservaciones.Location = new System.Drawing.Point(4, 100);
            this.lblObservaciones.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(113, 14);
            this.lblObservaciones.TabIndex = 11;
            this.lblObservaciones.Text = "164_Observaciones";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacion.Location = new System.Drawing.Point(114, 97);
            this.txtObservacion.Margin = new System.Windows.Forms.Padding(1);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(400, 40);
            this.txtObservacion.TabIndex = 12;
            // 
            // txtTasaMensual
            // 
            this.txtTasaMensual.EditValue = "0";
            this.txtTasaMensual.Enabled = false;
            this.txtTasaMensual.Location = new System.Drawing.Point(113, 69);
            this.txtTasaMensual.Margin = new System.Windows.Forms.Padding(1);
            this.txtTasaMensual.Name = "txtTasaMensual";
            this.txtTasaMensual.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTasaMensual.Properties.Appearance.Options.UseFont = true;
            this.txtTasaMensual.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaMensual.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaMensual.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaMensual.Properties.Mask.EditMask = "P7";
            this.txtTasaMensual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaMensual.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaMensual.Properties.ReadOnly = true;
            this.txtTasaMensual.Size = new System.Drawing.Size(95, 20);
            this.txtTasaMensual.TabIndex = 10;
            // 
            // txtCartaAceptacion
            // 
            this.txtCartaAceptacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCartaAceptacion.Location = new System.Drawing.Point(667, 10);
            this.txtCartaAceptacion.Margin = new System.Windows.Forms.Padding(1);
            this.txtCartaAceptacion.Name = "txtCartaAceptacion";
            this.txtCartaAceptacion.Size = new System.Drawing.Size(61, 22);
            this.txtCartaAceptacion.TabIndex = 4;
            // 
            // lblCartaAceptacion
            // 
            this.lblCartaAceptacion.AutoSize = true;
            this.lblCartaAceptacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCartaAceptacion.Location = new System.Drawing.Point(537, 15);
            this.lblCartaAceptacion.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblCartaAceptacion.Name = "lblCartaAceptacion";
            this.lblCartaAceptacion.Size = new System.Drawing.Size(142, 14);
            this.lblCartaAceptacion.TabIndex = 3;
            this.lblCartaAceptacion.Text = "164_RefCartaAceptacion";
            // 
            // txtVlrTotalNeto
            // 
            this.txtVlrTotalNeto.EditValue = "0";
            this.txtVlrTotalNeto.Enabled = false;
            this.txtVlrTotalNeto.Location = new System.Drawing.Point(583, 30);
            this.txtVlrTotalNeto.Margin = new System.Windows.Forms.Padding(1);
            this.txtVlrTotalNeto.Name = "txtVlrTotalNeto";
            this.txtVlrTotalNeto.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrTotalNeto.Properties.Appearance.Options.UseFont = true;
            this.txtVlrTotalNeto.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrTotalNeto.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrTotalNeto.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrTotalNeto.Properties.Mask.EditMask = "c0";
            this.txtVlrTotalNeto.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrTotalNeto.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrTotalNeto.Properties.ReadOnly = true;
            this.txtVlrTotalNeto.Size = new System.Drawing.Size(95, 20);
            this.txtVlrTotalNeto.TabIndex = 20;
            // 
            // lblVlrTotalNeto
            // 
            this.lblVlrTotalNeto.AutoSize = true;
            this.lblVlrTotalNeto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrTotalNeto.Location = new System.Drawing.Point(485, 32);
            this.lblVlrTotalNeto.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblVlrTotalNeto.Name = "lblVlrTotalNeto";
            this.lblVlrTotalNeto.Size = new System.Drawing.Size(104, 14);
            this.lblVlrTotalNeto.TabIndex = 19;
            this.lblVlrTotalNeto.Text = "164_VlrTotalNeto";
            // 
            // txtVlrTotalVenta
            // 
            this.txtVlrTotalVenta.EditValue = "0";
            this.txtVlrTotalVenta.Enabled = false;
            this.txtVlrTotalVenta.Location = new System.Drawing.Point(104, 30);
            this.txtVlrTotalVenta.Margin = new System.Windows.Forms.Padding(1);
            this.txtVlrTotalVenta.Name = "txtVlrTotalVenta";
            this.txtVlrTotalVenta.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrTotalVenta.Properties.Appearance.Options.UseFont = true;
            this.txtVlrTotalVenta.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrTotalVenta.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrTotalVenta.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrTotalVenta.Properties.Mask.EditMask = "c0";
            this.txtVlrTotalVenta.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrTotalVenta.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrTotalVenta.Properties.ReadOnly = true;
            this.txtVlrTotalVenta.Size = new System.Drawing.Size(95, 20);
            this.txtVlrTotalVenta.TabIndex = 16;
            // 
            // lblVlrTotalVenta
            // 
            this.lblVlrTotalVenta.AutoSize = true;
            this.lblVlrTotalVenta.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrTotalVenta.Location = new System.Drawing.Point(2, 33);
            this.lblVlrTotalVenta.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblVlrTotalVenta.Name = "lblVlrTotalVenta";
            this.lblVlrTotalVenta.Size = new System.Drawing.Size(110, 14);
            this.lblVlrTotalVenta.TabIndex = 15;
            this.lblVlrTotalVenta.Text = "164_VlrTotalVenta";
            // 
            // txtVlrTotalNominal
            // 
            this.txtVlrTotalNominal.EditValue = "0";
            this.txtVlrTotalNominal.Enabled = false;
            this.txtVlrTotalNominal.Location = new System.Drawing.Point(354, 29);
            this.txtVlrTotalNominal.Margin = new System.Windows.Forms.Padding(1);
            this.txtVlrTotalNominal.Name = "txtVlrTotalNominal";
            this.txtVlrTotalNominal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrTotalNominal.Properties.Appearance.Options.UseFont = true;
            this.txtVlrTotalNominal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrTotalNominal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrTotalNominal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrTotalNominal.Properties.Mask.EditMask = "c0";
            this.txtVlrTotalNominal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrTotalNominal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrTotalNominal.Properties.ReadOnly = true;
            this.txtVlrTotalNominal.Size = new System.Drawing.Size(95, 20);
            this.txtVlrTotalNominal.TabIndex = 27;
            // 
            // lblVlrTotalNominal
            // 
            this.lblVlrTotalNominal.AutoSize = true;
            this.lblVlrTotalNominal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrTotalNominal.Location = new System.Drawing.Point(235, 31);
            this.lblVlrTotalNominal.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lblVlrTotalNominal.Name = "lblVlrTotalNominal";
            this.lblVlrTotalNominal.Size = new System.Drawing.Size(119, 14);
            this.lblVlrTotalNominal.TabIndex = 26;
            this.lblVlrTotalNominal.Text = "164_VlrTotalNominal";
            // 
            // lblTasaAnual
            // 
            this.lblTasaAnual.AutoSize = true;
            this.lblTasaAnual.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaAnual.Location = new System.Drawing.Point(310, 73);
            this.lblTasaAnual.Name = "lblTasaAnual";
            this.lblTasaAnual.Size = new System.Drawing.Size(123, 16);
            this.lblTasaAnual.TabIndex = 17;
            this.lblTasaAnual.Text = "32566_lblTasaAnual";
            // 
            // lblTasaMensual
            // 
            this.lblTasaMensual.AutoSize = true;
            this.lblTasaMensual.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaMensual.Location = new System.Drawing.Point(2, 70);
            this.lblTasaMensual.Name = "lblTasaMensual";
            this.lblTasaMensual.Size = new System.Drawing.Size(138, 16);
            this.lblTasaMensual.TabIndex = 16;
            this.lblTasaMensual.Text = "32566_lblTasaMensual";
            // 
            // txtTasaAnual
            // 
            this.txtTasaAnual.EditValue = "0";
            this.txtTasaAnual.Enabled = false;
            this.txtTasaAnual.Location = new System.Drawing.Point(419, 70);
            this.txtTasaAnual.Margin = new System.Windows.Forms.Padding(1);
            this.txtTasaAnual.Name = "txtTasaAnual";
            this.txtTasaAnual.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTasaAnual.Properties.Appearance.Options.UseFont = true;
            this.txtTasaAnual.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaAnual.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaAnual.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaAnual.Properties.Mask.EditMask = "P7";
            this.txtTasaAnual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaAnual.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaAnual.Properties.ReadOnly = true;
            this.txtTasaAnual.Size = new System.Drawing.Size(95, 20);
            this.txtTasaAnual.TabIndex = 18;
            // 
            // lblFechaAceptacion
            // 
            this.lblFechaAceptacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaAceptacion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaAceptacion.Location = new System.Drawing.Point(5, 44);
            this.lblFechaAceptacion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblFechaAceptacion.Name = "lblFechaAceptacion";
            this.lblFechaAceptacion.Size = new System.Drawing.Size(111, 14);
            this.lblFechaAceptacion.TabIndex = 7;
            this.lblFechaAceptacion.Text = "32564_FechaLiquida";
            // 
            // dtFechaLiquida
            // 
            this.dtFechaLiquida.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaLiquida.Enabled = false;
            this.dtFechaLiquida.Location = new System.Drawing.Point(112, 41);
            this.dtFechaLiquida.Margin = new System.Windows.Forms.Padding(1);
            this.dtFechaLiquida.Name = "dtFechaLiquida";
            this.dtFechaLiquida.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaLiquida.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaLiquida.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaLiquida.Properties.Appearance.Options.UseFont = true;
            this.dtFechaLiquida.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaLiquida.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaLiquida.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaLiquida.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaLiquida.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaLiquida.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaLiquida.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaLiquida.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaLiquida.Size = new System.Drawing.Size(95, 20);
            this.dtFechaLiquida.TabIndex = 8;
            // 
            // lkp_Oferta
            // 
            this.lkp_Oferta.Location = new System.Drawing.Point(398, 14);
            this.lkp_Oferta.Name = "lkp_Oferta";
            this.lkp_Oferta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Oferta.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "Oferta")});
            this.lkp_Oferta.Properties.DisplayMember = "Key";
            this.lkp_Oferta.Properties.NullText = "";
            this.lkp_Oferta.Properties.ValueMember = "Key";
            this.lkp_Oferta.Size = new System.Drawing.Size(117, 20);
            this.lkp_Oferta.TabIndex = 21;
            this.lkp_Oferta.EditValueChanged += new System.EventHandler(this.lkp_Oferta_EditValueChanged);
            // 
            // VentaCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(903, 537);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "VentaCartera";
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFlujo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFlujo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaMensual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalNeto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalVenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalNominal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaAnual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaLiquida.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaLiquida.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Oferta.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterCompradorCartera;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorc;
        private DevExpress.XtraEditors.LabelControl lblFechaPago;
        protected DevExpress.XtraEditors.DateEdit dtFechaFlujo;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.Label lblOferta;
        private System.Windows.Forms.TextBox txtObservacion;
        private DevExpress.XtraEditors.TextEdit txtTasaMensual;
        private System.Windows.Forms.TextBox txtCartaAceptacion;
        private System.Windows.Forms.Label lblCartaAceptacion;
        private DevExpress.XtraEditors.TextEdit txtVlrTotalNeto;
        private System.Windows.Forms.Label lblVlrTotalNeto;
        private DevExpress.XtraEditors.TextEdit txtVlrTotalVenta;
        private System.Windows.Forms.Label lblVlrTotalVenta;
        private DevExpress.XtraEditors.TextEdit txtVlrTotalNominal;
        private System.Windows.Forms.Label lblVlrTotalNominal;
        private System.Windows.Forms.Label lblTasaAnual;
        private System.Windows.Forms.Label lblTasaMensual;
        private DevExpress.XtraEditors.TextEdit txtTasaAnual;
        protected DevExpress.XtraEditors.DateEdit dtFechaLiquida;
        private DevExpress.XtraEditors.LabelControl lblFechaAceptacion;
        private DevExpress.XtraEditors.LookUpEdit lkp_Oferta;


    }
}