namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReintegroClientes
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterReintegros = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txt_VlrTotaReintegro = new DevExpress.XtraEditors.TextEdit();
            this.lbl_VlrTotalReintegro = new System.Windows.Forms.Label();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.masterTerceros = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lkpTipos = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkPendientes = new System.Windows.Forms.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.txt_VlrTotaReintegro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipos.Properties)).BeginInit();
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
            this.btnMark.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(6);
            this.txtDocumentoID.Size = new System.Drawing.Size(58, 20);
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtDocDesc.Size = new System.Drawing.Size(217, 20);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(6);
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 20);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
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
            this.txtPrefix.Size = new System.Drawing.Size(50, 20);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtAF
            // 
            this.txtAF.Margin = new System.Windows.Forms.Padding(6);
            this.txtAF.Size = new System.Drawing.Size(91, 20);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(12, 0, 12, 6);
            this.gbGridDocument.Size = new System.Drawing.Size(865, 209);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(865, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(10, 209);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.chkPendientes);
            this.grpboxHeader.Controls.Add(this.label1);
            this.grpboxHeader.Controls.Add(this.lkpTipos);
            this.grpboxHeader.Controls.Add(this.label2);
            this.grpboxHeader.Controls.Add(this.masterTerceros);
            this.grpboxHeader.Controls.Add(this.chkAll);
            this.grpboxHeader.Controls.Add(this.txt_VlrTotaReintegro);
            this.grpboxHeader.Controls.Add(this.lbl_VlrTotalReintegro);
            this.grpboxHeader.Controls.Add(this.masterReintegros);
            this.grpboxHeader.Location = new System.Drawing.Point(6, 27);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(867, 75);
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
            // masterReintegros
            // 
            this.masterReintegros.BackColor = System.Drawing.Color.Transparent;
            this.masterReintegros.Filtros = null;
            this.masterReintegros.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterReintegros.Location = new System.Drawing.Point(7, 12);
            this.masterReintegros.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterReintegros.Name = "masterReintegros";
            this.masterReintegros.Size = new System.Drawing.Size(348, 31);
            this.masterReintegros.TabIndex = 0;
            this.masterReintegros.Value = "";
            this.masterReintegros.Leave += new System.EventHandler(this.masterReintegros_Leave);
            // 
            // txt_VlrTotaReintegro
            // 
            this.txt_VlrTotaReintegro.EditValue = "0";
            this.txt_VlrTotaReintegro.Enabled = false;
            this.txt_VlrTotaReintegro.Location = new System.Drawing.Point(129, 48);
            this.txt_VlrTotaReintegro.Margin = new System.Windows.Forms.Padding(1);
            this.txt_VlrTotaReintegro.Name = "txt_VlrTotaReintegro";
            this.txt_VlrTotaReintegro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_VlrTotaReintegro.Properties.Appearance.Options.UseFont = true;
            this.txt_VlrTotaReintegro.Properties.Appearance.Options.UseTextOptions = true;
            this.txt_VlrTotaReintegro.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txt_VlrTotaReintegro.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txt_VlrTotaReintegro.Properties.Mask.EditMask = "c";
            this.txt_VlrTotaReintegro.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txt_VlrTotaReintegro.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txt_VlrTotaReintegro.Size = new System.Drawing.Size(125, 20);
            this.txt_VlrTotaReintegro.TabIndex = 51;
            // 
            // lbl_VlrTotalReintegro
            // 
            this.lbl_VlrTotalReintegro.AutoSize = true;
            this.lbl_VlrTotalReintegro.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_VlrTotalReintegro.Location = new System.Drawing.Point(3, 50);
            this.lbl_VlrTotalReintegro.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.lbl_VlrTotalReintegro.Name = "lbl_VlrTotalReintegro";
            this.lbl_VlrTotalReintegro.Size = new System.Drawing.Size(136, 16);
            this.lbl_VlrTotalReintegro.TabIndex = 52;
            this.lbl_VlrTotalReintegro.Text = "172_VlrTotalReintegro";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAll.Location = new System.Drawing.Point(7, 78);
            this.chkAll.Margin = new System.Windows.Forms.Padding(2);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(85, 18);
            this.chkAll.TabIndex = 53;
            this.chkAll.Text = "172_chkAll";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // masterTerceros
            // 
            this.masterTerceros.BackColor = System.Drawing.Color.Transparent;
            this.masterTerceros.Filtros = null;
            this.masterTerceros.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterTerceros.Location = new System.Drawing.Point(376, 13);
            this.masterTerceros.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterTerceros.Name = "masterTerceros";
            this.masterTerceros.Size = new System.Drawing.Size(348, 31);
            this.masterTerceros.TabIndex = 54;
            this.masterTerceros.Value = "";
            this.masterTerceros.Leave += new System.EventHandler(this.masterTerceros_Leave);
            // 
            // lkp_Tipos
            // 
            this.lkpTipos.Location = new System.Drawing.Point(447, 52);
            this.lkpTipos.Name = "lkp_Tipos";
            this.lkpTipos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpTipos.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 60, "Descriptivo")});
            this.lkpTipos.Properties.DisplayMember = "Value";
            this.lkpTipos.Properties.NullText = " ";
            this.lkpTipos.Properties.ValueMember = "Key";
            this.lkpTipos.Size = new System.Drawing.Size(117, 20);
            this.lkpTipos.TabIndex = 55;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(328, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 14);
            this.label2.TabIndex = 56;
            this.label2.Text = "172_Tipo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 57;
            this.label1.Text = "172_GiroAjuste";
            // 
            // chkPendientes
            // 
            this.chkPendientes.AutoSize = true;
            this.chkPendientes.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPendientes.Location = new System.Drawing.Point(668, 53);
            this.chkPendientes.Margin = new System.Windows.Forms.Padding(2);
            this.chkPendientes.Name = "chkPendientes";
            this.chkPendientes.Size = new System.Drawing.Size(115, 18);
            this.chkPendientes.TabIndex = 58;
            this.chkPendientes.Text = "172_Pendientes";
            this.chkPendientes.UseVisualStyleBackColor = true;
            this.chkPendientes.CheckedChanged += new System.EventHandler(this.chkPendientes_CheckedChanged);
            // 
            // ReintegroClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(903, 476);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "ReintegroClientes";
            this.Text = "32564";
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
            ((System.ComponentModel.ISupportInitialize)(this.txt_VlrTotaReintegro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipos.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterReintegros;
        private DevExpress.XtraEditors.TextEdit txt_VlrTotaReintegro;
        private System.Windows.Forms.Label lbl_VlrTotalReintegro;
        private System.Windows.Forms.CheckBox chkAll;
        private ControlsUC.uc_MasterFind masterTerceros;
        private DevExpress.XtraEditors.LookUpEdit lkpTipos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPendientes;

    }
}