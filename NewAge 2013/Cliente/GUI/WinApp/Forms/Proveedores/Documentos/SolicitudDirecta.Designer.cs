namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class SolicitudDirecta
    {
    
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();        
            this.editBtnGridCargos = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editSpinPorc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.masterProveedor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaSol = new DevExpress.XtraEditors.LabelControl();
            this.txtSolicitudNro = new System.Windows.Forms.TextBox();
            this.lblSolicitudNro = new System.Windows.Forms.Label();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaSol = new DevExpress.XtraEditors.DateEdit();
            this.lblDescDoc = new System.Windows.Forms.Label();
            this.txtDescDoc = new System.Windows.Forms.TextBox();
            this.btnProyectoNro = new DevExpress.XtraEditors.SimpleButton();
            this.txtProyectoNro = new System.Windows.Forms.TextBox();
            this.lblNroProyecto = new System.Windows.Forms.Label();
            this.masterPrefijoProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblPrefijoProy = new System.Windows.Forms.Label();
            this.lblValorTotalML = new DevExpress.XtraEditors.LabelControl();
            this.txtValorTotalML = new DevExpress.XtraEditors.TextEdit();
            this.lblIValorIVATotalML = new DevExpress.XtraEditors.LabelControl();
            this.txtIvaTotalML = new DevExpress.XtraEditors.TextEdit();
            this.lblIvaTotalME = new DevExpress.XtraEditors.LabelControl();
            this.txtIvaTotalME = new DevExpress.XtraEditors.TextEdit();
            this.lblValorTotalME = new DevExpress.XtraEditors.LabelControl();
            this.txtValorTotalME = new DevExpress.XtraEditors.TextEdit();
            this.lblTasacambio = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGridCargos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSol.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSol.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIvaTotalML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIvaTotalME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            // 
            // editBtnGridCargos
            // 
            this.editBtnGridCargos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.editBtnGridCargos.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGridCargos.Name = "editBtnGridCargos";
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P";
            this.editSpinPorc.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorc.Name = "editSpinPorc";
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
            this.editSpinPorcen,
            this.editBtnGridCargos,
            this.editSpinPorc});
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
            this.editValue.Mask.EditMask = "c4";
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
            // gbGridDocument
            // 
            this.gbGridDocument.Size = new System.Drawing.Size(820, 243);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lblPrefijoProy);
            this.grpboxHeader.Controls.Add(this.lblTasacambio);
            this.grpboxHeader.Controls.Add(this.txtTasaCambio);
            this.grpboxHeader.Controls.Add(this.lblIvaTotalME);
            this.grpboxHeader.Controls.Add(this.txtIvaTotalME);
            this.grpboxHeader.Controls.Add(this.lblValorTotalME);
            this.grpboxHeader.Controls.Add(this.txtValorTotalME);
            this.grpboxHeader.Controls.Add(this.txtIvaTotalML);
            this.grpboxHeader.Controls.Add(this.txtValorTotalML);
            this.grpboxHeader.Controls.Add(this.btnProyectoNro);
            this.grpboxHeader.Controls.Add(this.txtProyectoNro);
            this.grpboxHeader.Controls.Add(this.lblNroProyecto);
            this.grpboxHeader.Controls.Add(this.masterPrefijoProyecto);
            this.grpboxHeader.Controls.Add(this.btnQueryDoc);
            this.grpboxHeader.Controls.Add(this.txtDescDoc);
            this.grpboxHeader.Controls.Add(this.lblDescDoc);
            this.grpboxHeader.Controls.Add(this.dtFechaSol);
            this.grpboxHeader.Controls.Add(this.masterProveedor);
            this.grpboxHeader.Controls.Add(this.lblFechaSol);
            this.grpboxHeader.Controls.Add(this.txtSolicitudNro);
            this.grpboxHeader.Controls.Add(this.lblSolicitudNro);
            this.grpboxHeader.Controls.Add(this.masterPrefijo);
            this.grpboxHeader.Controls.Add(this.lblIValorIVATotalML);
            this.grpboxHeader.Controls.Add(this.lblValorTotalML);
            this.grpboxHeader.Size = new System.Drawing.Size(1117, 126);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;  
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(520, 14);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(28, 20);
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);

            // 
            // lblFechaSol
            // 
            this.lblFechaSol.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaSol.Location = new System.Drawing.Point(334, 40);
            this.lblFechaSol.Margin = new System.Windows.Forms.Padding(4);
            this.lblFechaSol.Name = "lblFechaSol";
            this.lblFechaSol.Size = new System.Drawing.Size(80, 14);
            this.lblFechaSol.Text = "71_lblFechaSol";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(20, 12);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(298, 22);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            this.masterPrefijo.Enter += new System.EventHandler(this.masterPrefijo_Enter);
            this.masterPrefijo.Leave += new System.EventHandler(this.masterPrefijo_Leave);
            // 
            // txtSolicitudNro
            // 
            this.txtSolicitudNro.Location = new System.Drawing.Point(439, 14);
            this.txtSolicitudNro.Name = "txtSolicitudNro";
            this.txtSolicitudNro.Size = new System.Drawing.Size(77, 20);
            this.txtSolicitudNro.TabIndex = 2;
            this.txtSolicitudNro.Enter += new System.EventHandler(this.txtSolicitudNro_Enter);
            this.txtSolicitudNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSolicitudNro_KeyPress);
            this.txtSolicitudNro.Leave += new System.EventHandler(this.txtSolicitudNro_Leave);
            // 
            // masterProveedor
            // 
            this.masterProveedor.BackColor = System.Drawing.Color.Transparent;
            this.masterProveedor.Filtros = null;
            this.masterProveedor.Location = new System.Drawing.Point(20, 34);
            this.masterProveedor.Name = "masterProveedor";
            this.masterProveedor.Size = new System.Drawing.Size(298, 22);
            this.masterProveedor.TabIndex = 3;
            this.masterProveedor.Value = "";
            this.masterProveedor.Leave += new System.EventHandler(this.masterProveedor_Leave);
            // 
            // lblSolicitudNro
            // 
            this.lblSolicitudNro.AutoSize = true;
            this.lblSolicitudNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolicitudNro.Location = new System.Drawing.Point(331, 16);
            this.lblSolicitudNro.Name = "lblSolicitudNro";
            this.lblSolicitudNro.Size = new System.Drawing.Size(103, 14);
            this.lblSolicitudNro.Text = "71_lblSolicitudNro";

            // 
            // dtFechaSol
            // 
            this.dtFechaSol.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaSol.Location = new System.Drawing.Point(439, 37);
            this.dtFechaSol.Name = "dtFechaSol";
            this.dtFechaSol.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaSol.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaSol.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaSol.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaSol.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaSol.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaSol.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaSol.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaSol.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaSol.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaSol.Size = new System.Drawing.Size(108, 20);
            this.dtFechaSol.TabIndex = 4;
            this.dtFechaSol.DateTimeChanged += new System.EventHandler(this.dtFechas_DateTimeChanged);
            // 
            // lblDescDoc
            // 
            this.lblDescDoc.AutoSize = true;
            this.lblDescDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescDoc.Location = new System.Drawing.Point(17, 62);
            this.lblDescDoc.Name = "lblDescDoc";
            this.lblDescDoc.Size = new System.Drawing.Size(86, 14);
            this.lblDescDoc.Text = "71_lblDescDoc";
            // 
            // txtDescDoc
            // 
            this.txtDescDoc.Location = new System.Drawing.Point(18, 77);
            this.txtDescDoc.Multiline = true;
            this.txtDescDoc.Name = "txtDescDoc";
            this.txtDescDoc.Size = new System.Drawing.Size(293, 43);
            this.txtDescDoc.TabIndex = 5;
            // 
            // btnProyectoNro
            // 
            this.btnProyectoNro.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnProyectoNro.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnProyectoNro.Location = new System.Drawing.Point(1017, 13);
            this.btnProyectoNro.Name = "btnProyectoNro";
            this.btnProyectoNro.Size = new System.Drawing.Size(28, 20);
            this.btnProyectoNro.ToolTip = "1005_btnQueryDoc";
            this.btnProyectoNro.Visible = false;
            this.btnProyectoNro.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // txtProyectoNro
            // 
            this.txtProyectoNro.Location = new System.Drawing.Point(975, 13);
            this.txtProyectoNro.Name = "txtProyectoNro";
            this.txtProyectoNro.Size = new System.Drawing.Size(41, 20);
            this.txtProyectoNro.TabIndex = 7;
            this.txtProyectoNro.Visible = false;
            this.txtProyectoNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSolicitudNro_KeyPress);
            this.txtProyectoNro.Leave += new System.EventHandler(this.txtProyectoNro_Leave);
            // 
            // lblNroProyecto
            // 
            this.lblNroProyecto.AutoSize = true;
            this.lblNroProyecto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroProyecto.Location = new System.Drawing.Point(876, 17);
            this.lblNroProyecto.Name = "lblNroProyecto";
            this.lblNroProyecto.Size = new System.Drawing.Size(107, 14);
            this.lblNroProyecto.Text = "71_lblProyectoNro";
            this.lblNroProyecto.Visible = false;
            // 
            // masterPrefijoProyecto
            // 
            this.masterPrefijoProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoProyecto.Filtros = null;
            this.masterPrefijoProyecto.Location = new System.Drawing.Point(578, 12);
            this.masterPrefijoProyecto.Name = "masterPrefijoProyecto";
            this.masterPrefijoProyecto.Size = new System.Drawing.Size(300, 22);
            this.masterPrefijoProyecto.TabIndex = 6;
            this.masterPrefijoProyecto.Value = "";
            this.masterPrefijoProyecto.Visible = false;
            // 
            // lblPrefijoProy
            // 
            this.lblPrefijoProy.AutoSize = true;
            this.lblPrefijoProy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefijoProy.Location = new System.Drawing.Point(560, 17);
            this.lblPrefijoProy.Name = "lblPrefijoProy";
            this.lblPrefijoProy.Size = new System.Drawing.Size(97, 14);
            this.lblPrefijoProy.Text = "71_lblPrefijoProy";
            this.lblPrefijoProy.Visible = false;
            // 
            // lblValorTotalML
            // 
            this.lblValorTotalML.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblValorTotalML.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTotalML.Location = new System.Drawing.Point(871, 80);
            this.lblValorTotalML.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorTotalML.Name = "lblValorTotalML";
            this.lblValorTotalML.Size = new System.Drawing.Size(102, 14);
            this.lblValorTotalML.Text = "79_lblValorTotalML";
            // 
            // txtValorTotalML
            // 
            this.txtValorTotalML.EditValue = "0";
            this.txtValorTotalML.Location = new System.Drawing.Point(972, 76);
            this.txtValorTotalML.Name = "txtValorTotalML";
            this.txtValorTotalML.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorTotalML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtValorTotalML.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorTotalML.Properties.Appearance.Options.UseFont = true;
            this.txtValorTotalML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorTotalML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorTotalML.Properties.AutoHeight = false;
            this.txtValorTotalML.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalML.Properties.Mask.EditMask = "c";
            this.txtValorTotalML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorTotalML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorTotalML.Properties.ReadOnly = true;
            this.txtValorTotalML.Size = new System.Drawing.Size(124, 21);
            // 
            // lblIValorIVATotalML
            // 
            this.lblIValorIVATotalML.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblIValorIVATotalML.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIValorIVATotalML.Location = new System.Drawing.Point(871, 102);
            this.lblIValorIVATotalML.Margin = new System.Windows.Forms.Padding(4);
            this.lblIValorIVATotalML.Name = "lblIValorIVATotalML";
            this.lblIValorIVATotalML.Size = new System.Drawing.Size(91, 14);
            this.lblIValorIVATotalML.Text = "79_lblIvaTotalML";
            // 
            // txtIvaTotalML
            // 
            this.txtIvaTotalML.EditValue = "0";
            this.txtIvaTotalML.Location = new System.Drawing.Point(972, 98);
            this.txtIvaTotalML.Name = "txtIvaTotalML";
            this.txtIvaTotalML.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtIvaTotalML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtIvaTotalML.Properties.Appearance.Options.UseBorderColor = true;
            this.txtIvaTotalML.Properties.Appearance.Options.UseFont = true;
            this.txtIvaTotalML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtIvaTotalML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtIvaTotalML.Properties.AutoHeight = false;
            this.txtIvaTotalML.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIvaTotalML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIvaTotalML.Properties.Mask.EditMask = "c";
            this.txtIvaTotalML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtIvaTotalML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtIvaTotalML.Properties.ReadOnly = true;
            this.txtIvaTotalML.Size = new System.Drawing.Size(124, 21);
            // 
            // lblIvaTotalME
            // 
            this.lblIvaTotalME.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblIvaTotalME.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIvaTotalME.Location = new System.Drawing.Point(646, 102);
            this.lblIvaTotalME.Margin = new System.Windows.Forms.Padding(4);
            this.lblIvaTotalME.Name = "lblIvaTotalME";
            this.lblIvaTotalME.Size = new System.Drawing.Size(92, 14);
            this.lblIvaTotalME.Text = "79_lblIvaTotalME";
            this.lblIvaTotalME.Visible = false;
            // 
            // txtIvaTotalME
            // 
            this.txtIvaTotalME.EditValue = "0";
            this.txtIvaTotalME.Location = new System.Drawing.Point(743, 98);
            this.txtIvaTotalME.Name = "txtIvaTotalME";
            this.txtIvaTotalME.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtIvaTotalME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtIvaTotalME.Properties.Appearance.Options.UseBorderColor = true;
            this.txtIvaTotalME.Properties.Appearance.Options.UseFont = true;
            this.txtIvaTotalME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtIvaTotalME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtIvaTotalME.Properties.AutoHeight = false;
            this.txtIvaTotalME.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIvaTotalME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtIvaTotalME.Properties.Mask.EditMask = "c";
            this.txtIvaTotalME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtIvaTotalME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtIvaTotalME.Properties.ReadOnly = true;
            this.txtIvaTotalME.Size = new System.Drawing.Size(124, 21);
            this.txtIvaTotalME.Visible = false;
            // 
            // lblValorTotalME
            // 
            this.lblValorTotalME.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblValorTotalME.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTotalME.Location = new System.Drawing.Point(646, 80);
            this.lblValorTotalME.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorTotalME.Name = "lblValorTotalME";
            this.lblValorTotalME.Size = new System.Drawing.Size(103, 14);
            this.lblValorTotalME.Text = "79_lblValorTotalME";
            this.lblValorTotalME.Visible = false;
            // 
            // txtValorTotalME
            // 
            this.txtValorTotalME.EditValue = "0";
            this.txtValorTotalME.Location = new System.Drawing.Point(743, 76);
            this.txtValorTotalME.Name = "txtValorTotalME";
            this.txtValorTotalME.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorTotalME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtValorTotalME.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorTotalME.Properties.Appearance.Options.UseFont = true;
            this.txtValorTotalME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorTotalME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorTotalME.Properties.AutoHeight = false;
            this.txtValorTotalME.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalME.Properties.Mask.EditMask = "c";
            this.txtValorTotalME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorTotalME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorTotalME.Properties.ReadOnly = true;
            this.txtValorTotalME.Size = new System.Drawing.Size(124, 21);
            this.txtValorTotalME.Visible = false;
            // 
            // lblTasacambio
            // 
            this.lblTasacambio.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblTasacambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasacambio.Location = new System.Drawing.Point(334, 65);
            this.lblTasacambio.Margin = new System.Windows.Forms.Padding(4);
            this.lblTasacambio.Name = "lblTasacambio";
            this.lblTasacambio.Size = new System.Drawing.Size(96, 14);
            this.lblTasacambio.Text = "79_lblTasaCambio";
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.EditValue = "0";
            this.txtTasaCambio.Location = new System.Drawing.Point(439, 61);
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTasaCambio.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtTasaCambio.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTasaCambio.Properties.Appearance.Options.UseFont = true;
            this.txtTasaCambio.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTasaCambio.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTasaCambio.Properties.AutoHeight = false;
            this.txtTasaCambio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.Mask.EditMask = "c";
            this.txtTasaCambio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambio.Properties.ReadOnly = true;
            this.txtTasaCambio.Size = new System.Drawing.Size(108, 21);
            // 
            // SolicitudDirecta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "SolicitudDirecta";
            this.Text = "SolicitudDirecta";
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGridCargos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSol.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIvaTotalML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIvaTotalME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        
        private ControlsUC.uc_MasterFind masterProveedor;
        protected DevExpress.XtraEditors.LabelControl lblFechaSol;
        private System.Windows.Forms.TextBox txtSolicitudNro;
        private System.Windows.Forms.Label lblSolicitudNro;
        private ControlsUC.uc_MasterFind masterPrefijo;
        protected DevExpress.XtraEditors.DateEdit dtFechaSol;
        private System.Windows.Forms.Label lblDescDoc;
        private System.Windows.Forms.TextBox txtDescDoc;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.SimpleButton btnProyectoNro;
        private System.Windows.Forms.TextBox txtProyectoNro;
        private System.Windows.Forms.Label lblNroProyecto;
        private ControlsUC.uc_MasterFind masterPrefijoProyecto;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGridCargos;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorc;
        private System.Windows.Forms.Label lblPrefijoProy;
        private DevExpress.XtraEditors.LabelControl lblIValorIVATotalML;
        private DevExpress.XtraEditors.TextEdit txtIvaTotalML;
        private DevExpress.XtraEditors.LabelControl lblValorTotalML;
        private DevExpress.XtraEditors.TextEdit txtValorTotalML;
        private DevExpress.XtraEditors.LabelControl lblIvaTotalME;
        private DevExpress.XtraEditors.TextEdit txtIvaTotalME;
        private DevExpress.XtraEditors.LabelControl lblValorTotalME;
        private DevExpress.XtraEditors.TextEdit txtValorTotalME;
        private DevExpress.XtraEditors.LabelControl lblTasacambio;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;  
    }
}