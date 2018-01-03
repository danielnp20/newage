namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RecibidoNotaEnvio
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lblNumber = new DevExpress.XtraEditors.LabelControl();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.masterBodegaDestino = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterBodegaOrigen = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblBodegaOrigen = new DevExpress.XtraEditors.LabelControl();
            this.lblBodegaDestino = new DevExpress.XtraEditors.LabelControl();
            this.lblTipoVeh = new DevExpress.XtraEditors.LabelControl();
            this.txtTipoVeh = new System.Windows.Forms.TextBox();
            this.lblPlacaVeh = new DevExpress.XtraEditors.LabelControl();
            this.txtPlacaVeh = new System.Windows.Forms.TextBox();
            this.lblTelConductor = new DevExpress.XtraEditors.LabelControl();
            this.txtTelConductor = new System.Windows.Forms.TextBox();
            this.lblObservacion = new DevExpress.XtraEditors.LabelControl();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblConductor = new DevExpress.XtraEditors.LabelControl();
            this.txtConductor = new System.Windows.Forms.TextBox();
            this.txtCedula = new System.Windows.Forms.TextBox();
            this.lblCedula = new DevExpress.XtraEditors.LabelControl();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnInfoInicial = new DevExpress.XtraEditors.PanelControl();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.pnInfoInicial)).BeginInit();
            this.pnInfoInicial.SuspendLayout();
            this.SuspendLayout();
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
            this.editSpinPorcen});
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
            // btnMark
            // 
            this.btnMark.Size = new System.Drawing.Size(49, 20);
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
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            this.dtFecha.TabIndex = 2;
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // dtPeriod
            // 
            this.dtPeriod.TabIndex = 1;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Size = new System.Drawing.Size(820, 243);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(820, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 243);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.pnInfoInicial);
            this.grpboxHeader.Controls.Add(this.masterCentroCto);
            this.grpboxHeader.Controls.Add(this.txtCedula);
            this.grpboxHeader.Controls.Add(this.lblCedula);
            this.grpboxHeader.Controls.Add(this.txtConductor);
            this.grpboxHeader.Controls.Add(this.lblConductor);
            this.grpboxHeader.Controls.Add(this.lblObservacion);
            this.grpboxHeader.Controls.Add(this.txtObservacion);
            this.grpboxHeader.Controls.Add(this.lblTelConductor);
            this.grpboxHeader.Controls.Add(this.txtTelConductor);
            this.grpboxHeader.Controls.Add(this.masterProyecto);
            this.grpboxHeader.Controls.Add(this.lblPlacaVeh);
            this.grpboxHeader.Controls.Add(this.txtPlacaVeh);
            this.grpboxHeader.Controls.Add(this.lblTipoVeh);
            this.grpboxHeader.Controls.Add(this.txtTipoVeh);
            this.grpboxHeader.Controls.Add(this.lblBodegaDestino);
            this.grpboxHeader.Controls.Add(this.masterBodegaDestino);
            this.grpboxHeader.Location = new System.Drawing.Point(5, 18);
            this.grpboxHeader.Size = new System.Drawing.Size(1117, 125);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblNumber
            // 
            this.lblNumber.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumber.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblNumber.Location = new System.Drawing.Point(620, 9);
            this.lblNumber.Margin = new System.Windows.Forms.Padding(4);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(75, 14);
            this.lblNumber.TabIndex = 22;
            this.lblNumber.Text = "51_lblNumber";
            // 
            // txtNumber
            // 
            this.txtNumber.BackColor = System.Drawing.Color.LightBlue;
            this.txtNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumber.Location = new System.Drawing.Point(660, 5);
            this.txtNumber.Multiline = true;
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(48, 21);
            this.txtNumber.TabIndex = 5;
            this.txtNumber.Enter += new System.EventHandler(this.txtNumber_Enter);
            this.txtNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
            this.txtNumber.Leave += new System.EventHandler(this.txtNumber_Leave);
            // 
            // masterBodegaDestino
            // 
            this.masterBodegaDestino.BackColor = System.Drawing.Color.Transparent;
            this.masterBodegaDestino.Enabled = false;
            this.masterBodegaDestino.Filtros = null;
            this.masterBodegaDestino.Location = new System.Drawing.Point(16, 54);
            this.masterBodegaDestino.Name = "masterBodegaDestino";
            this.masterBodegaDestino.Size = new System.Drawing.Size(299, 24);
            this.masterBodegaDestino.TabIndex = 21;
            this.masterBodegaDestino.Value = "";
            // 
            // masterBodegaOrigen
            // 
            this.masterBodegaOrigen.BackColor = System.Drawing.Color.Transparent;
            this.masterBodegaOrigen.Filtros = null;
            this.masterBodegaOrigen.Location = new System.Drawing.Point(6, 4);
            this.masterBodegaOrigen.Name = "masterBodegaOrigen";
            this.masterBodegaOrigen.Size = new System.Drawing.Size(299, 24);
            this.masterBodegaOrigen.TabIndex = 3;
            this.masterBodegaOrigen.Value = "";
            // 
            // masterCentroCto
            // 
            this.masterCentroCto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCto.Filtros = null;
            this.masterCentroCto.Location = new System.Drawing.Point(16, 101);
            this.masterCentroCto.Name = "masterCentroCto";
            this.masterCentroCto.Size = new System.Drawing.Size(308, 22);
            this.masterCentroCto.TabIndex = 5;
            this.masterCentroCto.Value = "";
            // 
            // lblBodegaOrigen
            // 
            this.lblBodegaOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBodegaOrigen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblBodegaOrigen.Location = new System.Drawing.Point(6, 9);
            this.lblBodegaOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.lblBodegaOrigen.Name = "lblBodegaOrigen";
            this.lblBodegaOrigen.Size = new System.Drawing.Size(109, 14);
            this.lblBodegaOrigen.TabIndex = 20;
            this.lblBodegaOrigen.Text = "52_lblBodegaOrigen";
            // 
            // lblBodegaDestino
            // 
            this.lblBodegaDestino.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBodegaDestino.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblBodegaDestino.Location = new System.Drawing.Point(16, 58);
            this.lblBodegaDestino.Margin = new System.Windows.Forms.Padding(4);
            this.lblBodegaDestino.Name = "lblBodegaDestino";
            this.lblBodegaDestino.Size = new System.Drawing.Size(114, 14);
            this.lblBodegaDestino.TabIndex = 19;
            this.lblBodegaDestino.Text = "52_lblBodegaDestino";
            // 
            // lblTipoVeh
            // 
            this.lblTipoVeh.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoVeh.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTipoVeh.Location = new System.Drawing.Point(423, 53);
            this.lblTipoVeh.Margin = new System.Windows.Forms.Padding(4);
            this.lblTipoVeh.Name = "lblTipoVeh";
            this.lblTipoVeh.Size = new System.Drawing.Size(102, 14);
            this.lblTipoVeh.TabIndex = 17;
            this.lblTipoVeh.Text = "52_lblTipoVehiculo";
            // 
            // txtTipoVeh
            // 
            this.txtTipoVeh.Enabled = false;
            this.txtTipoVeh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTipoVeh.Location = new System.Drawing.Point(509, 49);
            this.txtTipoVeh.Name = "txtTipoVeh";
            this.txtTipoVeh.Size = new System.Drawing.Size(125, 22);
            this.txtTipoVeh.TabIndex = 18;
            // 
            // lblPlacaVeh
            // 
            this.lblPlacaVeh.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlacaVeh.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblPlacaVeh.Location = new System.Drawing.Point(671, 50);
            this.lblPlacaVeh.Margin = new System.Windows.Forms.Padding(4);
            this.lblPlacaVeh.Name = "lblPlacaVeh";
            this.lblPlacaVeh.Size = new System.Drawing.Size(105, 14);
            this.lblPlacaVeh.TabIndex = 15;
            this.lblPlacaVeh.Text = "52_lblPlacaVehiculo";
            // 
            // txtPlacaVeh
            // 
            this.txtPlacaVeh.Enabled = false;
            this.txtPlacaVeh.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlacaVeh.Location = new System.Drawing.Point(752, 46);
            this.txtPlacaVeh.Name = "txtPlacaVeh";
            this.txtPlacaVeh.Size = new System.Drawing.Size(59, 22);
            this.txtPlacaVeh.TabIndex = 16;
            // 
            // lblTelConductor
            // 
            this.lblTelConductor.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelConductor.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTelConductor.Location = new System.Drawing.Point(423, 103);
            this.lblTelConductor.Margin = new System.Windows.Forms.Padding(4);
            this.lblTelConductor.Name = "lblTelConductor";
            this.lblTelConductor.Size = new System.Drawing.Size(81, 14);
            this.lblTelConductor.TabIndex = 12;
            this.lblTelConductor.Text = "52_lblTelefono";
            // 
            // txtTelConductor
            // 
            this.txtTelConductor.Enabled = false;
            this.txtTelConductor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelConductor.Location = new System.Drawing.Point(509, 99);
            this.txtTelConductor.Name = "txtTelConductor";
            this.txtTelConductor.Size = new System.Drawing.Size(125, 22);
            this.txtTelConductor.TabIndex = 13;
            this.txtTelConductor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
            // 
            // lblObservacion
            // 
            this.lblObservacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblObservacion.Location = new System.Drawing.Point(672, 98);
            this.lblObservacion.Margin = new System.Windows.Forms.Padding(4);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(98, 14);
            this.lblObservacion.TabIndex = 10;
            this.lblObservacion.Text = "52_lblObservacion";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Enabled = false;
            this.txtObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacion.Location = new System.Drawing.Point(754, 96);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(354, 41);
            this.txtObservacion.TabIndex = 11;
            // 
            // lblConductor
            // 
            this.lblConductor.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConductor.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblConductor.Location = new System.Drawing.Point(672, 75);
            this.lblConductor.Margin = new System.Windows.Forms.Padding(4);
            this.lblConductor.Name = "lblConductor";
            this.lblConductor.Size = new System.Drawing.Size(89, 14);
            this.lblConductor.TabIndex = 9;
            this.lblConductor.Text = "52_lblConductor";
            // 
            // txtConductor
            // 
            this.txtConductor.Enabled = false;
            this.txtConductor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConductor.Location = new System.Drawing.Point(753, 71);
            this.txtConductor.Name = "txtConductor";
            this.txtConductor.Size = new System.Drawing.Size(355, 22);
            this.txtConductor.TabIndex = 8;
            // 
            // txtCedula
            // 
            this.txtCedula.Enabled = false;
            this.txtCedula.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCedula.Location = new System.Drawing.Point(509, 74);
            this.txtCedula.Name = "txtCedula";
            this.txtCedula.Size = new System.Drawing.Size(125, 22);
            this.txtCedula.TabIndex = 6;
            this.txtCedula.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
            // 
            // lblCedula
            // 
            this.lblCedula.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCedula.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblCedula.Location = new System.Drawing.Point(423, 78);
            this.lblCedula.Margin = new System.Windows.Forms.Padding(4);
            this.lblCedula.Name = "lblCedula";
            this.lblCedula.Size = new System.Drawing.Size(68, 14);
            this.lblCedula.TabIndex = 7;
            this.lblCedula.Text = "52_lblCedula";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(16, 77);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(308, 22);
            this.masterProyecto.TabIndex = 14;
            this.masterProyecto.Value = "";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(311, 4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(302, 24);
            this.masterPrefijo.TabIndex = 4;
            this.masterPrefijo.Value = "";
            // 
            // pnInfoInicial
            // 
            this.pnInfoInicial.Controls.Add(this.btnQueryDoc);
            this.pnInfoInicial.Controls.Add(this.lblBodegaOrigen);
            this.pnInfoInicial.Controls.Add(this.masterPrefijo);
            this.pnInfoInicial.Controls.Add(this.txtNumber);
            this.pnInfoInicial.Controls.Add(this.lblNumber);
            this.pnInfoInicial.Controls.Add(this.masterBodegaOrigen);
            this.pnInfoInicial.Location = new System.Drawing.Point(10, 11);
            this.pnInfoInicial.Name = "pnInfoInicial";
            this.pnInfoInicial.Size = new System.Drawing.Size(799, 33);
            this.pnInfoInicial.TabIndex = 24;
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(710, 5);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(32, 21);
            this.btnQueryDoc.TabIndex = 24;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // RecibidoNotaEnvio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "RecibidoNotaEnvio";
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.pnInfoInicial)).EndInit();
            this.pnInfoInicial.ResumeLayout(false);
            this.pnInfoInicial.PerformLayout();
            this.ResumeLayout(false);

        }
        
        #endregion

        private DevExpress.XtraEditors.LabelControl lblNumber;
        private System.Windows.Forms.TextBox txtNumber;
        private ControlsUC.uc_MasterFind masterBodegaOrigen;
        private ControlsUC.uc_MasterFind masterBodegaDestino;
        private ControlsUC.uc_MasterFind masterCentroCto;
        private DevExpress.XtraEditors.LabelControl lblBodegaDestino;
        private DevExpress.XtraEditors.LabelControl lblBodegaOrigen;
        private DevExpress.XtraEditors.LabelControl lblPlacaVeh;
        private System.Windows.Forms.TextBox txtPlacaVeh;
        private DevExpress.XtraEditors.LabelControl lblTipoVeh;
        private System.Windows.Forms.TextBox txtTipoVeh;
        private DevExpress.XtraEditors.LabelControl lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private DevExpress.XtraEditors.LabelControl lblTelConductor;
        private System.Windows.Forms.TextBox txtTelConductor;
        private DevExpress.XtraEditors.LabelControl lblConductor;
        private System.Windows.Forms.TextBox txtCedula;
        private DevExpress.XtraEditors.LabelControl lblCedula;
        private System.Windows.Forms.TextBox txtConductor;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.PanelControl pnInfoInicial;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
    }
}