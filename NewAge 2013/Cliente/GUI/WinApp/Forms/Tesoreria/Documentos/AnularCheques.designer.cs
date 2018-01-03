namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AnularCheques
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
        protected void InitializeComponent()
        {
            this.lkpDocumentos = new DevExpress.XtraEditors.LookUpEdit();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.LinkEdit = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.gbFilter = new DevExpress.XtraEditors.GroupControl();
            this.txtChequeNro = new System.Windows.Forms.TextBox();
            this.masterBancoCta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblChequeNro = new System.Windows.Forms.Label();
            this.dtFechaFinal = new DevExpress.XtraEditors.DateEdit();
            this.btnFilter = new DevExpress.XtraEditors.CheckButton();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.txtAF = new System.Windows.Forms.TextBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblAF = new DevExpress.XtraEditors.LabelControl();
            this.lblBreak = new DevExpress.XtraEditors.LabelControl();
            this.txtDocDesc = new System.Windows.Forms.TextBox();
            this.txtDocumentoID = new System.Windows.Forms.TextBox();
            this.txtNumeroDoc = new System.Windows.Forms.TextBox();
            this.lblNumeroDoc = new DevExpress.XtraEditors.LabelControl();
            this.lblPrefix = new DevExpress.XtraEditors.LabelControl();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpDocumentos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilter)).BeginInit();
            this.gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lkpDocumentos
            // 
            this.lkpDocumentos.Location = new System.Drawing.Point(142, 18);
            this.lkpDocumentos.Name = "lkpDocumentos";
            this.lkpDocumentos.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.lkpDocumentos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpDocumentos.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Descriptivo", "Documento")});
            this.lkpDocumentos.Properties.DisplayMember = "Descriptivo";
            this.lkpDocumentos.Properties.NullText = "";
            this.lkpDocumentos.Properties.ValueMember = "ID";
            this.lkpDocumentos.Size = new System.Drawing.Size(184, 20);
            this.lkpDocumentos.TabIndex = 19;
            // 
            // lblDocumento
            // 
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumento.Location = new System.Drawing.Point(12, 20);
            this.lblDocumento.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new System.Drawing.Size(117, 14);
            this.lblDocumento.TabIndex = 20;
            this.lblDocumento.Text = "1035_lblDocumento";
            // 
            // LinkEdit
            // 
            this.LinkEdit.Name = "LinkEdit";
            // 
            // gbFilter
            // 
            this.gbFilter.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gbFilter.Appearance.Options.UseFont = true;
            this.gbFilter.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gbFilter.AppearanceCaption.Options.UseFont = true;
            this.gbFilter.Controls.Add(this.txtChequeNro);
            this.gbFilter.Controls.Add(this.masterBancoCta);
            this.gbFilter.Controls.Add(this.lblChequeNro);
            this.gbFilter.Location = new System.Drawing.Point(17, 45);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(800, 96);
            this.gbFilter.TabIndex = 102;
            this.gbFilter.Text = "Seleccionar Banco";
            // 
            // txtChequeNro
            // 
            this.txtChequeNro.Location = new System.Drawing.Point(455, 46);
            this.txtChequeNro.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.txtChequeNro.Name = "txtChequeNro";
            this.txtChequeNro.ReadOnly = true;
            this.txtChequeNro.Size = new System.Drawing.Size(113, 22);
            this.txtChequeNro.TabIndex = 107;
            // 
            // masterBancoCta
            // 
            this.masterBancoCta.BackColor = System.Drawing.Color.Transparent;
            this.masterBancoCta.Filtros = null;
            this.masterBancoCta.Location = new System.Drawing.Point(12, 41);
            this.masterBancoCta.Name = "masterBancoCta";
            this.masterBancoCta.Size = new System.Drawing.Size(346, 24);
            this.masterBancoCta.TabIndex = 122;
            this.masterBancoCta.Value = "";
            this.masterBancoCta.Leave += new System.EventHandler(this.masterBancoCta_Leave);
            // 
            // lblChequeNro
            // 
            this.lblChequeNro.AutoSize = true;
            this.lblChequeNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChequeNro.Location = new System.Drawing.Point(371, 50);
            this.lblChequeNro.Margin = new System.Windows.Forms.Padding(17, 0, 3, 0);
            this.lblChequeNro.Name = "lblChequeNro";
            this.lblChequeNro.Size = new System.Drawing.Size(121, 14);
            this.lblChequeNro.TabIndex = 102;
            this.lblChequeNro.Text = "22504_lblChequeNro";
            // 
            // dtFechaFinal
            // 
            this.dtFechaFinal.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaFinal.Location = new System.Drawing.Point(800, 18);
            this.dtFechaFinal.Name = "dtFechaFinal";
            this.dtFechaFinal.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaFinal.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFinal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFinal.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaFinal.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaFinal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaFinal.Size = new System.Drawing.Size(78, 20);
            this.dtFechaFinal.TabIndex = 111;
            // 
            // btnFilter
            // 
            this.btnFilter.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnFilter.Appearance.Options.UseFont = true;
            this.btnFilter.Location = new System.Drawing.Point(346, 16);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(91, 18);
            this.btnFilter.TabIndex = 112;
            this.btnFilter.Text = "1035_btnFilter";
            // 
            // grpctrlHeader
            // 
            this.grpctrlHeader.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.grpctrlHeader.Appearance.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.grpctrlHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.grpctrlHeader.AppearanceCaption.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.Options.UseFont = true;
            this.grpctrlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.grpctrlHeader.Controls.Add(this.txtAF);
            this.grpctrlHeader.Controls.Add(this.txtPrefix);
            this.grpctrlHeader.Controls.Add(this.dtPeriod);
            this.grpctrlHeader.Controls.Add(this.gbFilter);
            this.grpctrlHeader.Controls.Add(this.lblAF);
            this.grpctrlHeader.Controls.Add(this.lblBreak);
            this.grpctrlHeader.Controls.Add(this.txtDocDesc);
            this.grpctrlHeader.Controls.Add(this.txtDocumentoID);
            this.grpctrlHeader.Controls.Add(this.txtNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblNumeroDoc);
            this.grpctrlHeader.Controls.Add(this.lblPrefix);
            this.grpctrlHeader.Controls.Add(this.lblDate);
            this.grpctrlHeader.Controls.Add(this.lblPeriod);
            this.grpctrlHeader.Controls.Add(this.dtFecha);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(0, 0);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.Size = new System.Drawing.Size(1028, 478);
            this.grpctrlHeader.TabIndex = 112;
            // 
            // txtAF
            // 
            this.txtAF.Enabled = false;
            this.txtAF.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAF.Location = new System.Drawing.Point(774, 1);
            this.txtAF.Multiline = true;
            this.txtAF.Name = "txtAF";
            this.txtAF.Size = new System.Drawing.Size(91, 19);
            this.txtAF.TabIndex = 5;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Enabled = false;
            this.txtPrefix.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrefix.Location = new System.Drawing.Point(940, 1);
            this.txtPrefix.Multiline = true;
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(50, 19);
            this.txtPrefix.TabIndex = 6;
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(454, 1);
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(6);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 18);
            this.dtPeriod.TabIndex = 3;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAF.Location = new System.Drawing.Point(748, 4);
            this.lblAF.Margin = new System.Windows.Forms.Padding(4);
            this.lblAF.Name = "lblAF";
            this.lblAF.Size = new System.Drawing.Size(69, 14);
            this.lblAF.TabIndex = 96;
            this.lblAF.Text = "1005_lblAF";
            // 
            // lblBreak
            // 
            this.lblBreak.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBreak.Location = new System.Drawing.Point(67, 4);
            this.lblBreak.Margin = new System.Windows.Forms.Padding(4);
            this.lblBreak.Name = "lblBreak";
            this.lblBreak.Size = new System.Drawing.Size(5, 13);
            this.lblBreak.TabIndex = 7;
            this.lblBreak.Text = "-";
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.Enabled = false;
            this.txtDocDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocDesc.Location = new System.Drawing.Point(75, 1);
            this.txtDocDesc.Multiline = true;
            this.txtDocDesc.Name = "txtDocDesc";
            this.txtDocDesc.Size = new System.Drawing.Size(217, 19);
            this.txtDocDesc.TabIndex = 1;
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Enabled = false;
            this.txtDocumentoID.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoID.Location = new System.Drawing.Point(7, 1);
            this.txtDocumentoID.Multiline = true;
            this.txtDocumentoID.Name = "txtDocumentoID";
            this.txtDocumentoID.Size = new System.Drawing.Size(58, 19);
            this.txtDocumentoID.TabIndex = 0;
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Enabled = false;
            this.txtNumeroDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroDoc.Location = new System.Drawing.Point(321, 1);
            this.txtNumeroDoc.Multiline = true;
            this.txtNumeroDoc.Name = "txtNumeroDoc";
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 19);
            this.txtNumeroDoc.TabIndex = 2;
            // 
            // lblNumeroDoc
            // 
            this.lblNumeroDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumeroDoc.Location = new System.Drawing.Point(307, 4);
            this.lblNumeroDoc.Margin = new System.Windows.Forms.Padding(4);
            this.lblNumeroDoc.Name = "lblNumeroDoc";
            this.lblNumeroDoc.Size = new System.Drawing.Size(10, 14);
            this.lblNumeroDoc.TabIndex = 92;
            this.lblNumeroDoc.Text = "#";
            // 
            // lblPrefix
            // 
            this.lblPrefix.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefix.Location = new System.Drawing.Point(881, 4);
            this.lblPrefix.Margin = new System.Windows.Forms.Padding(4);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(89, 14);
            this.lblPrefix.TabIndex = 93;
            this.lblPrefix.Text = "1005_lblPrefix";
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(586, 4);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(83, 14);
            this.lblDate.TabIndex = 94;
            this.lblDate.Text = "1005_lblDate";
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(392, 4);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(93, 14);
            this.lblPeriod.TabIndex = 82;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(630, 1);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            this.dtFecha.TabIndex = 4;
            // 
            // AnularCheques
            // 
            this.ClientSize = new System.Drawing.Size(1028, 478);
            this.Controls.Add(this.grpctrlHeader);
            this.Controls.Add(this.dtFechaFinal);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AnularCheques";
            ((System.ComponentModel.ISupportInitialize)(this.lkpDocumentos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilter)).EndInit();
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpctrlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.ResumeLayout(false);

        }             

        #endregion

        private System.Windows.Forms.Label lblDocumento;
        public DevExpress.XtraEditors.LookUpEdit lkpDocumentos;
        public DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit LinkEdit;
        protected DevExpress.XtraEditors.GroupControl gbFilter;
        protected DevExpress.XtraEditors.DateEdit dtFechaFinal;
        private System.Windows.Forms.Label lblChequeNro;
        protected System.Windows.Forms.TextBox txtChequeNro;
        private DevExpress.XtraEditors.CheckButton btnFilter;
        public DevExpress.XtraEditors.GroupControl grpctrlHeader;
        protected System.Windows.Forms.TextBox txtAF;
        protected System.Windows.Forms.TextBox txtPrefix;
        protected ControlsUC.uc_PeriodoEdit dtPeriod;
        protected DevExpress.XtraEditors.LabelControl lblAF;
        private DevExpress.XtraEditors.LabelControl lblBreak;
        protected System.Windows.Forms.TextBox txtDocDesc;
        protected System.Windows.Forms.TextBox txtDocumentoID;
        protected System.Windows.Forms.TextBox txtNumeroDoc;
        private DevExpress.XtraEditors.LabelControl lblNumeroDoc;
        private DevExpress.XtraEditors.LabelControl lblPrefix;
        private DevExpress.XtraEditors.LabelControl lblDate;
        private DevExpress.XtraEditors.LabelControl lblPeriod;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        protected ControlsUC.uc_MasterFind masterBancoCta;


    }
}