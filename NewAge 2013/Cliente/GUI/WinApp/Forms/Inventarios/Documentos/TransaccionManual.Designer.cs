namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class TransaccionManual
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterMvtoTipoInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblNumber = new DevExpress.XtraEditors.LabelControl();
            this.lblTasaCambio = new DevExpress.XtraEditors.LabelControl();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            this.masterBodegaDestino = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterBodegaOrigen = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTerceroHeader = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDocTercero = new DevExpress.XtraEditors.LabelControl();
            this.txtDocTercero = new System.Windows.Forms.TextBox();
            this.masterProyectoOrig = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCtoOrig = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblBodegaOrigen = new DevExpress.XtraEditors.LabelControl();
            this.lblBodegaDestino = new DevExpress.XtraEditors.LabelControl();
            this.masterMoneda = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtManifiestoCarga = new System.Windows.Forms.TextBox();
            this.lblManifiestoCarga = new System.Windows.Forms.Label();
            this.txtDocTransporte = new System.Windows.Forms.TextBox();
            this.lblDocTransporte = new System.Windows.Forms.Label();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.lblObservacion = new DevExpress.XtraEditors.LabelControl();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.masterProyectoDest = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCtoDest = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorFobML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorFobME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldoRef.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSaldos)).BeginInit();
            this.pnlSaldos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlValor)).BeginInit();
            this.pnlValor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTotal)).BeginInit();
            this.pnlTotal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalValorExt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalValorLoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalCantidad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUNI.Properties)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // masterEmpaque
            // 
            this.masterEmpaque.TabIndex = 0;
            // 
            // txtValorFobML
            // 
            this.txtValorFobML.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorFobML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtValorFobML.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorFobML.Properties.Appearance.Options.UseFont = true;
            this.txtValorFobML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorFobML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorFobML.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorFobML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorFobML.Properties.Mask.EditMask = "c2";
            this.txtValorFobML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorFobML.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtValorML
            // 
            this.txtValorML.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtValorML.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorML.Properties.Appearance.Options.UseFont = true;
            this.txtValorML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorML.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorML.Properties.Mask.EditMask = "c2";
            this.txtValorML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorML.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtValorFobME
            // 
            this.txtValorFobME.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorFobME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtValorFobME.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorFobME.Properties.Appearance.Options.UseFont = true;
            this.txtValorFobME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorFobME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorFobME.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorFobME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorFobME.Properties.Mask.EditMask = "c2";
            this.txtValorFobME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorFobME.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtValorME
            // 
            this.txtValorME.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9F);
            this.txtValorME.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorME.Properties.Appearance.Options.UseFont = true;
            this.txtValorME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorME.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorME.Properties.Mask.EditMask = "c2";
            this.txtValorME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorME.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // masterParametro2
            // 
            this.masterParametro2.TabIndex = 2;
            // 
            // masterParametro1
            // 
            this.masterParametro1.TabIndex = 1;
            // 
            // txtSaldoRef
            // 
            this.txtSaldoRef.Properties.Appearance.BackColor = System.Drawing.SystemColors.ControlDark;
            this.txtSaldoRef.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtSaldoRef.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaldoRef.Properties.Appearance.Options.UseBackColor = true;
            this.txtSaldoRef.Properties.Appearance.Options.UseBorderColor = true;
            this.txtSaldoRef.Properties.Appearance.Options.UseFont = true;
            this.txtSaldoRef.Properties.Appearance.Options.UseTextOptions = true;
            this.txtSaldoRef.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtSaldoRef.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSaldoRef.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtSaldoRef.Properties.Mask.EditMask = "n2";
            this.txtSaldoRef.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSaldoRef.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // pnlSaldos
            // 
            this.pnlSaldos.Appearance.BorderColor = System.Drawing.Color.Cyan;
            this.pnlSaldos.Appearance.Options.UseBorderColor = true;
            this.pnlSaldos.LookAndFeel.SkinName = "McSkin";
            this.pnlSaldos.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // pnlValor
            // 
            this.pnlValor.Appearance.BorderColor = System.Drawing.Color.Cyan;
            this.pnlValor.Appearance.Options.UseBorderColor = true;
            this.pnlValor.LookAndFeel.SkinName = "Foggy";
            this.pnlValor.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // pnlTotal
            // 
            this.pnlTotal.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlTotal.Appearance.BorderColor = System.Drawing.Color.Cyan;
            this.pnlTotal.Appearance.Options.UseBackColor = true;
            this.pnlTotal.Appearance.Options.UseBorderColor = true;
            this.pnlTotal.LookAndFeel.SkinName = "Foggy";
            this.pnlTotal.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // txtTotalValorExt
            // 
            this.txtTotalValorExt.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalValorExt.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtTotalValorExt.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalValorExt.Properties.Appearance.Options.UseFont = true;
            this.txtTotalValorExt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalValorExt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalValorExt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalValorExt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalValorExt.Properties.Mask.EditMask = "c2";
            this.txtTotalValorExt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalValorExt.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtTotalValorLoc
            // 
            this.txtTotalValorLoc.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalValorLoc.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtTotalValorLoc.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalValorLoc.Properties.Appearance.Options.UseFont = true;
            this.txtTotalValorLoc.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalValorLoc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalValorLoc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalValorLoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalValorLoc.Properties.Mask.EditMask = "c2";
            this.txtTotalValorLoc.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalValorLoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtTotalCantidad
            // 
            this.txtTotalCantidad.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalCantidad.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtTotalCantidad.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalCantidad.Properties.Appearance.Options.UseFont = true;
            this.txtTotalCantidad.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalCantidad.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalCantidad.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalCantidad.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalCantidad.Properties.Mask.EditMask = "n2";
            this.txtTotalCantidad.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalCantidad.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtValorUNI
            // 
            this.txtValorUNI.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorUNI.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F);
            this.txtValorUNI.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorUNI.Properties.Appearance.Options.UseFont = true;
            this.txtValorUNI.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorUNI.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorUNI.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUNI.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUNI.Properties.Mask.EditMask = "c2";
            this.txtValorUNI.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorUNI.Properties.Mask.UseMaskAsDisplayFormat = true;
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
            this.editSpin0});
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
            this.dtFecha.TabIndex = 3;
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtPeriod
            // 
            this.dtPeriod.TabIndex = 2;
            // 
            // txtAF
            // 
            this.txtAF.TabIndex = 4;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Size = new System.Drawing.Size(820, 272);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Size = new System.Drawing.Size(296, 272);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.labelControl4);
            this.grpboxHeader.Controls.Add(this.labelControl3);
            this.grpboxHeader.Controls.Add(this.labelControl2);
            this.grpboxHeader.Controls.Add(this.labelControl1);
            this.grpboxHeader.Controls.Add(this.masterProyectoDest);
            this.grpboxHeader.Controls.Add(this.masterCentroCtoDest);
            this.grpboxHeader.Controls.Add(this.masterProyectoOrig);
            this.grpboxHeader.Controls.Add(this.lblObservacion);
            this.grpboxHeader.Controls.Add(this.txtObservacion);
            this.grpboxHeader.Controls.Add(this.btnQueryDoc);
            this.grpboxHeader.Controls.Add(this.txtNumber);
            this.grpboxHeader.Controls.Add(this.lblNumber);
            this.grpboxHeader.Controls.Add(this.txtManifiestoCarga);
            this.grpboxHeader.Controls.Add(this.lblManifiestoCarga);
            this.grpboxHeader.Controls.Add(this.txtDocTransporte);
            this.grpboxHeader.Controls.Add(this.lblDocTransporte);
            this.grpboxHeader.Controls.Add(this.masterMoneda);
            this.grpboxHeader.Controls.Add(this.lblBodegaDestino);
            this.grpboxHeader.Controls.Add(this.lblBodegaOrigen);
            this.grpboxHeader.Controls.Add(this.masterCentroCtoOrig);
            this.grpboxHeader.Controls.Add(this.lblDocTercero);
            this.grpboxHeader.Controls.Add(this.txtDocTercero);
            this.grpboxHeader.Controls.Add(this.masterTerceroHeader);
            this.grpboxHeader.Controls.Add(this.masterBodegaOrigen);
            this.grpboxHeader.Controls.Add(this.masterBodegaDestino);
            this.grpboxHeader.Controls.Add(this.masterMvtoTipoInv);
            this.grpboxHeader.Controls.Add(this.lblTasaCambio);
            this.grpboxHeader.Controls.Add(this.txtTasaCambio);
            this.grpboxHeader.Size = new System.Drawing.Size(1117, 673);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "c3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "c0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // masterMvtoTipoInv
            // 
            this.masterMvtoTipoInv.BackColor = System.Drawing.Color.Transparent;
            this.masterMvtoTipoInv.Filtros = null;
            this.masterMvtoTipoInv.Location = new System.Drawing.Point(21, 16);
            this.masterMvtoTipoInv.Name = "masterMvtoTipoInv";
            this.masterMvtoTipoInv.Size = new System.Drawing.Size(301, 24);
            this.masterMvtoTipoInv.TabIndex = 0;
            this.masterMvtoTipoInv.Value = "";
            this.masterMvtoTipoInv.Leave += new System.EventHandler(this.masterHeader_Leave);
            // 
            // lblNumber
            // 
            this.lblNumber.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumber.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblNumber.Location = new System.Drawing.Point(319, 46);
            this.lblNumber.Margin = new System.Windows.Forms.Padding(4);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(75, 14);
            this.lblNumber.TabIndex = 2;
            this.lblNumber.Text = "51_lblNumber";
            // 
            // lblTasaCambio
            // 
            this.lblTasaCambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambio.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTasaCambio.Location = new System.Drawing.Point(837, 18);
            this.lblTasaCambio.Margin = new System.Windows.Forms.Padding(4);
            this.lblTasaCambio.Name = "lblTasaCambio";
            this.lblTasaCambio.Size = new System.Drawing.Size(110, 14);
            this.lblTasaCambio.TabIndex = 12;
            this.lblTasaCambio.Text = "51_lblExchangeRate";
            // 
            // txtNumber
            // 
            this.txtNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumber.Location = new System.Drawing.Point(347, 42);
            this.txtNumber.Multiline = true;
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(35, 21);
            this.txtNumber.TabIndex = 22;
            this.txtNumber.Enter += new System.EventHandler(this.txtNumber_Enter);
            this.txtNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
            this.txtNumber.Leave += new System.EventHandler(this.txtNumber_Leave);
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.EditValue = "0";
            this.txtTasaCambio.Enabled = false;
            this.txtTasaCambio.Location = new System.Drawing.Point(936, 15);
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.Mask.EditMask = "c";
            this.txtTasaCambio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambio.Size = new System.Drawing.Size(77, 20);
            this.txtTasaCambio.TabIndex = 7;
            // 
            // masterBodegaDestino
            // 
            this.masterBodegaDestino.BackColor = System.Drawing.Color.Transparent;
            this.masterBodegaDestino.Filtros = null;
            this.masterBodegaDestino.Location = new System.Drawing.Point(21, 65);
            this.masterBodegaDestino.Name = "masterBodegaDestino";
            this.masterBodegaDestino.Size = new System.Drawing.Size(299, 24);
            this.masterBodegaDestino.TabIndex = 2;
            this.masterBodegaDestino.Value = "";
            this.masterBodegaDestino.Leave += new System.EventHandler(this.masterHeader_Leave);
            // 
            // masterBodegaOrigen
            // 
            this.masterBodegaOrigen.BackColor = System.Drawing.Color.Transparent;
            this.masterBodegaOrigen.Filtros = null;
            this.masterBodegaOrigen.Location = new System.Drawing.Point(21, 41);
            this.masterBodegaOrigen.Name = "masterBodegaOrigen";
            this.masterBodegaOrigen.Size = new System.Drawing.Size(299, 24);
            this.masterBodegaOrigen.TabIndex = 1;
            this.masterBodegaOrigen.Value = "";
            this.masterBodegaOrigen.Leave += new System.EventHandler(this.masterHeader_Leave);
            // 
            // masterTerceroHeader
            // 
            this.masterTerceroHeader.BackColor = System.Drawing.Color.Transparent;
            this.masterTerceroHeader.Filtros = null;
            this.masterTerceroHeader.Location = new System.Drawing.Point(21, 90);
            this.masterTerceroHeader.Name = "masterTerceroHeader";
            this.masterTerceroHeader.Size = new System.Drawing.Size(308, 22);
            this.masterTerceroHeader.TabIndex = 4;
            this.masterTerceroHeader.Value = "";
            this.masterTerceroHeader.Leave += new System.EventHandler(this.masterHeader_Leave);
            // 
            // lblDocTercero
            // 
            this.lblDocTercero.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocTercero.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDocTercero.Location = new System.Drawing.Point(322, 93);
            this.lblDocTercero.Margin = new System.Windows.Forms.Padding(4);
            this.lblDocTercero.Name = "lblDocTercero";
            this.lblDocTercero.Size = new System.Drawing.Size(96, 14);
            this.lblDocTercero.TabIndex = 17;
            this.lblDocTercero.Text = "51_lblDocTercero";
            // 
            // txtDocTercero
            // 
            this.txtDocTercero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocTercero.Location = new System.Drawing.Point(404, 90);
            this.txtDocTercero.Name = "txtDocTercero";
            this.txtDocTercero.Size = new System.Drawing.Size(75, 22);
            this.txtDocTercero.TabIndex = 5;
            this.txtDocTercero.Leave += new System.EventHandler(this.txtDocTercero_Leave);
            // 
            // masterProyectoOrig
            // 
            this.masterProyectoOrig.BackColor = System.Drawing.Color.Transparent;
            this.masterProyectoOrig.Filtros = null;
            this.masterProyectoOrig.Location = new System.Drawing.Point(823, 40);
            this.masterProyectoOrig.Name = "masterProyectoOrig";
            this.masterProyectoOrig.Size = new System.Drawing.Size(308, 22);
            this.masterProyectoOrig.TabIndex = 9;
            this.masterProyectoOrig.Value = "";
            this.masterProyectoOrig.Leave += new System.EventHandler(this.masterHeader_Leave);
            // 
            // masterCentroCtoOrig
            // 
            this.masterCentroCtoOrig.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCtoOrig.Filtros = null;
            this.masterCentroCtoOrig.Location = new System.Drawing.Point(527, 41);
            this.masterCentroCtoOrig.Name = "masterCentroCtoOrig";
            this.masterCentroCtoOrig.Size = new System.Drawing.Size(308, 22);
            this.masterCentroCtoOrig.TabIndex = 8;
            this.masterCentroCtoOrig.Value = "";
            this.masterCentroCtoOrig.Leave += new System.EventHandler(this.masterHeader_Leave);
            // 
            // lblBodegaOrigen
            // 
            this.lblBodegaOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBodegaOrigen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblBodegaOrigen.Location = new System.Drawing.Point(21, 46);
            this.lblBodegaOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.lblBodegaOrigen.Name = "lblBodegaOrigen";
            this.lblBodegaOrigen.Size = new System.Drawing.Size(109, 14);
            this.lblBodegaOrigen.TabIndex = 20;
            this.lblBodegaOrigen.Text = "51_lblBodegaOrigen";
            // 
            // lblBodegaDestino
            // 
            this.lblBodegaDestino.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBodegaDestino.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblBodegaDestino.Location = new System.Drawing.Point(21, 70);
            this.lblBodegaDestino.Margin = new System.Windows.Forms.Padding(4);
            this.lblBodegaDestino.Name = "lblBodegaDestino";
            this.lblBodegaDestino.Size = new System.Drawing.Size(114, 14);
            this.lblBodegaDestino.TabIndex = 21;
            this.lblBodegaDestino.Text = "51_lblBodegaDestino";
            // 
            // masterMoneda
            // 
            this.masterMoneda.BackColor = System.Drawing.Color.Transparent;
            this.masterMoneda.Filtros = null;
            this.masterMoneda.Location = new System.Drawing.Point(527, 13);
            this.masterMoneda.Name = "masterMoneda";
            this.masterMoneda.Size = new System.Drawing.Size(308, 22);
            this.masterMoneda.TabIndex = 6;
            this.masterMoneda.Value = "";
            this.masterMoneda.Leave += new System.EventHandler(this.masterHeader_Leave);
            // 
            // txtManifiestoCarga
            // 
            this.txtManifiestoCarga.Location = new System.Drawing.Point(479, 68);
            this.txtManifiestoCarga.Name = "txtManifiestoCarga";
            this.txtManifiestoCarga.Size = new System.Drawing.Size(44, 21);
            this.txtManifiestoCarga.TabIndex = 42;
            this.txtManifiestoCarga.Visible = false;
            // 
            // lblManifiestoCarga
            // 
            this.lblManifiestoCarga.AutoSize = true;
            this.lblManifiestoCarga.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblManifiestoCarga.Location = new System.Drawing.Point(411, 70);
            this.lblManifiestoCarga.Name = "lblManifiestoCarga";
            this.lblManifiestoCarga.Size = new System.Drawing.Size(123, 14);
            this.lblManifiestoCarga.TabIndex = 41;
            this.lblManifiestoCarga.Text = "51_lblManifiestoCarga";
            this.lblManifiestoCarga.Visible = false;
            // 
            // txtDocTransporte
            // 
            this.txtDocTransporte.Location = new System.Drawing.Point(478, 42);
            this.txtDocTransporte.Name = "txtDocTransporte";
            this.txtDocTransporte.Size = new System.Drawing.Size(44, 21);
            this.txtDocTransporte.TabIndex = 40;
            this.txtDocTransporte.Visible = false;
            // 
            // lblDocTransporte
            // 
            this.lblDocTransporte.AutoSize = true;
            this.lblDocTransporte.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocTransporte.Location = new System.Drawing.Point(412, 45);
            this.lblDocTransporte.Name = "lblDocTransporte";
            this.lblDocTransporte.Size = new System.Drawing.Size(120, 14);
            this.lblDocTransporte.TabIndex = 39;
            this.lblDocTransporte.Text = "51_lblDocTransporte";
            this.lblDocTransporte.Visible = false;
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(383, 42);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(28, 21);
            this.btnQueryDoc.TabIndex = 43;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // lblObservacion
            // 
            this.lblObservacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblObservacion.Location = new System.Drawing.Point(21, 121);
            this.lblObservacion.Margin = new System.Windows.Forms.Padding(4);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(98, 14);
            this.lblObservacion.TabIndex = 45;
            this.lblObservacion.Text = "51_lblObservacion";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacion.Location = new System.Drawing.Point(121, 118);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(457, 34);
            this.txtObservacion.TabIndex = 44;
            // 
            // masterProyectoDest
            // 
            this.masterProyectoDest.BackColor = System.Drawing.Color.Transparent;
            this.masterProyectoDest.Filtros = null;
            this.masterProyectoDest.Location = new System.Drawing.Point(823, 64);
            this.masterProyectoDest.Name = "masterProyectoDest";
            this.masterProyectoDest.Size = new System.Drawing.Size(308, 22);
            this.masterProyectoDest.TabIndex = 47;
            this.masterProyectoDest.Value = "";
            // 
            // masterCentroCtoDest
            // 
            this.masterCentroCtoDest.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCtoDest.Filtros = null;
            this.masterCentroCtoDest.Location = new System.Drawing.Point(528, 65);
            this.masterCentroCtoDest.Name = "masterCentroCtoDest";
            this.masterCentroCtoDest.Size = new System.Drawing.Size(308, 22);
            this.masterCentroCtoDest.TabIndex = 46;
            this.masterCentroCtoDest.Value = "";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl1.Location = new System.Drawing.Point(831, 46);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 14);
            this.labelControl1.TabIndex = 48;
            this.labelControl1.Text = "Proyecto Inic";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl2.Location = new System.Drawing.Point(831, 70);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(78, 14);
            this.labelControl2.TabIndex = 49;
            this.labelControl2.Text = "Proyecto Final";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl3.Location = new System.Drawing.Point(532, 46);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(86, 14);
            this.labelControl3.TabIndex = 49;
            this.labelControl3.Text = "Centro Cto Inic";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl4.Location = new System.Drawing.Point(531, 70);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(89, 14);
            this.labelControl4.TabIndex = 49;
            this.labelControl4.Text = "Centro Cto Final";
            // 
            // TransaccionManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "TransaccionManual";
            ((System.ComponentModel.ISupportInitialize)(this.txtValorFobML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorFobME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldoRef.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSaldos)).EndInit();
            this.pnlSaldos.ResumeLayout(false);
            this.pnlSaldos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlValor)).EndInit();
            this.pnlValor.ResumeLayout(false);
            this.pnlValor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTotal)).EndInit();
            this.pnlTotal.ResumeLayout(false);
            this.pnlTotal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalValorExt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalValorLoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalCantidad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUNI.Properties)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        
        #endregion

        private DevExpress.XtraEditors.LabelControl lblNumber;
        private DevExpress.XtraEditors.LabelControl lblTasaCambio;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private System.Windows.Forms.TextBox txtNumber;
        private ControlsUC.uc_MasterFind masterMvtoTipoInv;
        private DevExpress.XtraEditors.LabelControl lblDocTercero;
        private System.Windows.Forms.TextBox txtDocTercero;
        private ControlsUC.uc_MasterFind masterTerceroHeader;
        private ControlsUC.uc_MasterFind masterBodegaOrigen;
        private ControlsUC.uc_MasterFind masterBodegaDestino;
        private ControlsUC.uc_MasterFind masterCentroCtoOrig;
        private ControlsUC.uc_MasterFind masterProyectoOrig;
        private DevExpress.XtraEditors.LabelControl lblBodegaDestino;
        private DevExpress.XtraEditors.LabelControl lblBodegaOrigen;
        private ControlsUC.uc_MasterFind masterMoneda;
        private System.Windows.Forms.TextBox txtManifiestoCarga;
        private System.Windows.Forms.Label lblManifiestoCarga;
        private System.Windows.Forms.TextBox txtDocTransporte;
        private System.Windows.Forms.Label lblDocTransporte;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.LabelControl lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private ControlsUC.uc_MasterFind masterProyectoDest;
        private ControlsUC.uc_MasterFind masterCentroCtoDest;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}