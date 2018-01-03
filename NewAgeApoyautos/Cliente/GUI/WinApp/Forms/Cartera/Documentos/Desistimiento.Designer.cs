namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Desistimiento
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lkp_Libranzas = new DevExpress.XtraEditors.LookUpEdit();
            this.dtFechaAplica = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaAplica = new System.Windows.Forms.Label();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnlPlanPago = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Libranzas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.tableLayoutPanel1);
            this.grpboxDetail.Location = new System.Drawing.Point(0, 5);
            this.grpboxDetail.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Padding = new System.Windows.Forms.Padding(6);
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
            this.btnMark.Margin = new System.Windows.Forms.Padding(6);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(6);
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Margin = new System.Windows.Forms.Padding(6);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(6);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(6);
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
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(12, 0, 12, 6);
            this.gbGridDocument.Size = new System.Drawing.Size(843, 303);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(843, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 303);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lkp_Libranzas);
            this.grpboxHeader.Controls.Add(this.dtFechaAplica);
            this.grpboxHeader.Controls.Add(this.lblFechaAplica);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.masterCliente);
            this.grpboxHeader.Location = new System.Drawing.Point(8, 22);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(1111, 227);
            this.grpboxHeader.TabIndex = 0;
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "c3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lkp_Libranzas
            // 
            this.lkp_Libranzas.Location = new System.Drawing.Point(113, 76);
            this.lkp_Libranzas.Name = "lkp_Libranzas";
            this.lkp_Libranzas.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Libranzas.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NumeroDoc", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Libranza", 40, "Descriptivo")});
            this.lkp_Libranzas.Properties.DisplayMember = "Libranza";
            this.lkp_Libranzas.Properties.NullText = "";
            this.lkp_Libranzas.Properties.ValueMember = "NumeroDoc";
            this.lkp_Libranzas.Size = new System.Drawing.Size(117, 20);
            this.lkp_Libranzas.TabIndex = 8;
            this.lkp_Libranzas.Leave += new System.EventHandler(this.lkp_Libranzas_Leave);
            // 
            // dtFechaAplica
            // 
            this.dtFechaAplica.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaAplica.Location = new System.Drawing.Point(113, 21);
            this.dtFechaAplica.Name = "dtFechaAplica";
            this.dtFechaAplica.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaAplica.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaAplica.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaAplica.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaAplica.Properties.Appearance.Options.UseFont = true;
            this.dtFechaAplica.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaAplica.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaAplica.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaAplica.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaAplica.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaAplica.Size = new System.Drawing.Size(117, 20);
            this.dtFechaAplica.TabIndex = 3;
            // 
            // lblFechaAplica
            // 
            this.lblFechaAplica.AutoSize = true;
            this.lblFechaAplica.Location = new System.Drawing.Point(11, 24);
            this.lblFechaAplica.Name = "lblFechaAplica";
            this.lblFechaAplica.Size = new System.Drawing.Size(70, 13);
            this.lblFechaAplica.TabIndex = 2;
            this.lblFechaAplica.Text = "1005_lblDate";
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Location = new System.Drawing.Point(11, 79);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(71, 13);
            this.lblLibranza.TabIndex = 7;
            this.lblLibranza.Text = "168_Libranza";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(14, 45);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(12);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(297, 24);
            this.masterCliente.TabIndex = 6;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // pnlPlanPago
            // 
            this.pnlPlanPago.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPlanPago.Location = new System.Drawing.Point(808, 3);
            this.pnlPlanPago.Name = "pnlPlanPago";
            this.pnlPlanPago.Size = new System.Drawing.Size(294, 16);
            this.pnlPlanPago.TabIndex = 23;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.67212F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.32788F));
            this.tableLayoutPanel1.Controls.Add(this.pnlPlanPago, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.82734F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.17266F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1105, 172);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // RedencionCreditos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1167, 634);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "RedencionCreditos";
            this.grpboxDetail.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Libranzas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lkp_Libranzas;
        protected DevExpress.XtraEditors.DateEdit dtFechaAplica;
        private System.Windows.Forms.Label lblFechaAplica;
        private System.Windows.Forms.Label lblLibranza;
        private ControlsUC.uc_MasterFind masterCliente;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlPlanPago;
    }
}