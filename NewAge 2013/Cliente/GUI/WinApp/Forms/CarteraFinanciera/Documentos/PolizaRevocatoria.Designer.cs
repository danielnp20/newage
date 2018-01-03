namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class PolizaRevocatoria
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PolizaRevocatoria));
            this.lkp_Libranzas = new DevExpress.XtraEditors.LookUpEdit();
            this.dtFechaRevoca = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaRevoca = new System.Windows.Forms.Label();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.masterAseguradora = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtNro = new DevExpress.XtraEditors.TextEdit();
            this.lblNro = new DevExpress.XtraEditors.LabelControl();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.txtDocRef = new DevExpress.XtraEditors.TextEdit();
            this.lblDocRef = new DevExpress.XtraEditors.LabelControl();
            this.lblVlrRevocar = new System.Windows.Forms.Label();
            this.txtVlrRevocar = new DevExpress.XtraEditors.TextEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Libranzas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaRevoca.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaRevoca.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDocRef.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrRevocar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Location = new System.Drawing.Point(3, 4);
            this.grpboxDetail.Size = new System.Drawing.Size(1117, 198);
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
            this.gbGridDocument.Size = new System.Drawing.Size(820, 357);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(820, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 357);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.txtVlrRevocar);
            this.grpboxHeader.Controls.Add(this.lblVlrRevocar);
            this.grpboxHeader.Controls.Add(this.txtDocRef);
            this.grpboxHeader.Controls.Add(this.lblDocRef);
            this.grpboxHeader.Controls.Add(this.txtNro);
            this.grpboxHeader.Controls.Add(this.lblNro);
            this.grpboxHeader.Controls.Add(this.btnQueryDoc);
            this.grpboxHeader.Controls.Add(this.masterPrefijo);
            this.grpboxHeader.Controls.Add(this.lkp_Libranzas);
            this.grpboxHeader.Controls.Add(this.dtFechaRevoca);
            this.grpboxHeader.Controls.Add(this.lblFechaRevoca);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.masterAseguradora);
            this.grpboxHeader.Location = new System.Drawing.Point(6, 24);
            this.grpboxHeader.Size = new System.Drawing.Size(1102, 588);
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
            // lkp_Libranzas
            // 
            this.lkp_Libranzas.Location = new System.Drawing.Point(1007, 17);
            this.lkp_Libranzas.Name = "lkp_Libranzas";
            this.lkp_Libranzas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Libranzas.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NumeroDoc", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Libranza", 40, "Descriptivo")});
            this.lkp_Libranzas.Properties.DisplayMember = "Libranza";
            this.lkp_Libranzas.Properties.NullText = " ";
            this.lkp_Libranzas.Properties.ValueMember = "NumeroDoc";
            this.lkp_Libranzas.Size = new System.Drawing.Size(83, 20);
            this.lkp_Libranzas.TabIndex = 8;
            this.lkp_Libranzas.Visible = false;
            // 
            // dtFechaRevoca
            // 
            this.dtFechaRevoca.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaRevoca.Location = new System.Drawing.Point(114, 77);
            this.dtFechaRevoca.Name = "dtFechaRevoca";
            this.dtFechaRevoca.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaRevoca.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaRevoca.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaRevoca.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaRevoca.Properties.Appearance.Options.UseFont = true;
            this.dtFechaRevoca.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaRevoca.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaRevoca.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaRevoca.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaRevoca.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaRevoca.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaRevoca.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaRevoca.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaRevoca.Size = new System.Drawing.Size(100, 20);
            this.dtFechaRevoca.TabIndex = 5;
            // 
            // lblFechaRevoca
            // 
            this.lblFechaRevoca.AutoSize = true;
            this.lblFechaRevoca.Location = new System.Drawing.Point(11, 80);
            this.lblFechaRevoca.Name = "lblFechaRevoca";
            this.lblFechaRevoca.Size = new System.Drawing.Size(96, 13);
            this.lblFechaRevoca.TabIndex = 4;
            this.lblFechaRevoca.Text = "185_FechaRevoca";
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Location = new System.Drawing.Point(904, 20);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(71, 13);
            this.lblLibranza.TabIndex = 7;
            this.lblLibranza.Text = "185_Libranza";
            this.lblLibranza.Visible = false;
            // 
            // masterAseguradora
            // 
            this.masterAseguradora.BackColor = System.Drawing.Color.Transparent;
            this.masterAseguradora.Filtros = null;
            this.masterAseguradora.Location = new System.Drawing.Point(14, 44);
            this.masterAseguradora.Name = "masterAseguradora";
            this.masterAseguradora.Size = new System.Drawing.Size(305, 25);
            this.masterAseguradora.TabIndex = 6;
            this.masterAseguradora.Value = "";
            this.masterAseguradora.Leave += new System.EventHandler(this.masterAseguradora_Leave);
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(14, 14);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(304, 25);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // txtNro
            // 
            this.txtNro.Location = new System.Drawing.Point(370, 17);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(30, 20);
            this.txtNro.TabIndex = 47;
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // lblNro
            // 
            this.lblNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNro.Location = new System.Drawing.Point(323, 20);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(51, 13);
            this.lblNro.TabIndex = 49;
            this.lblNro.Text = "185_lblNro";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(402, 17);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(20, 20);
            this.btnQueryDoc.TabIndex = 48;
            this.btnQueryDoc.Text = "btnQueryDoc";
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // txtDocRef
            // 
            this.txtDocRef.Location = new System.Drawing.Point(370, 49);
            this.txtDocRef.Name = "txtDocRef";
            this.txtDocRef.Size = new System.Drawing.Size(75, 20);
            this.txtDocRef.TabIndex = 50;
            // 
            // lblDocRef
            // 
            this.lblDocRef.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDocRef.Location = new System.Drawing.Point(321, 53);
            this.lblDocRef.Name = "lblDocRef";
            this.lblDocRef.Size = new System.Drawing.Size(70, 13);
            this.lblDocRef.TabIndex = 51;
            this.lblDocRef.Text = "185_lblDocRef";
            // 
            // lblVlrRevocar
            // 
            this.lblVlrRevocar.AutoSize = true;
            this.lblVlrRevocar.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrRevocar.Location = new System.Drawing.Point(7, 113);
            this.lblVlrRevocar.Name = "lblVlrRevocar";
            this.lblVlrRevocar.Size = new System.Drawing.Size(116, 17);
            this.lblVlrRevocar.TabIndex = 73;
            this.lblVlrRevocar.Text = "185_lblValorRevocar";
            // 
            // txtVlrRevocar
            // 
            this.txtVlrRevocar.EditValue = "0,00 ";
            this.txtVlrRevocar.Location = new System.Drawing.Point(115, 112);
            this.txtVlrRevocar.Name = "txtVlrRevocar";
            this.txtVlrRevocar.Properties.Appearance.BackColor = System.Drawing.Color.LightBlue;
            this.txtVlrRevocar.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrRevocar.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrRevocar.Properties.Appearance.Options.UseBackColor = true;
            this.txtVlrRevocar.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrRevocar.Properties.Appearance.Options.UseFont = true;
            this.txtVlrRevocar.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrRevocar.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrRevocar.Properties.AutoHeight = false;
            this.txtVlrRevocar.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtVlrRevocar.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrRevocar.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrRevocar.Properties.Mask.EditMask = "c";
            this.txtVlrRevocar.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrRevocar.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrRevocar.Properties.ReadOnly = true;
            this.txtVlrRevocar.Size = new System.Drawing.Size(123, 18);
            this.txtVlrRevocar.TabIndex = 74;
            // 
            // PolizaRevocatoria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 724);
            this.Name = "PolizaRevocatoria";
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
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Libranzas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaRevoca.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaRevoca.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDocRef.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrRevocar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lkp_Libranzas;
        protected DevExpress.XtraEditors.DateEdit dtFechaRevoca;
        private System.Windows.Forms.Label lblFechaRevoca;
        private System.Windows.Forms.Label lblLibranza;
        private ControlsUC.uc_MasterFind masterAseguradora;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.TextEdit txtNro;
        private DevExpress.XtraEditors.LabelControl lblNro;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.TextEdit txtDocRef;
        private DevExpress.XtraEditors.LabelControl lblDocRef;
        private System.Windows.Forms.Label lblVlrRevocar;
        private DevExpress.XtraEditors.TextEdit txtVlrRevocar;


    }
}