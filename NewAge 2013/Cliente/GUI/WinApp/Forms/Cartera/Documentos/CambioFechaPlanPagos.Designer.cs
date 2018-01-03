namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class CambioFechaPlanPagos
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.dtNuevaFechaCta = new DevExpress.XtraEditors.DateEdit();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblObservacion = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.dtNuevaFechaCta.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtNuevaFechaCta.Properties)).BeginInit();
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
            this.editValue4});
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
            this.btnMark.Size = new System.Drawing.Size(98, 32);
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
            this.dtFecha.Size = new System.Drawing.Size(200, 32);
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
            this.gbGridDocument.Size = new System.Drawing.Size(1558, 469);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(1558, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(592, 469);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.txtObservacion);
            this.grpboxHeader.Controls.Add(this.lblObservacion);
            this.grpboxHeader.Controls.Add(this.txtLibranza);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.dtNuevaFechaCta);
            this.grpboxHeader.Controls.Add(this.lblDate);
            this.grpboxHeader.Location = new System.Drawing.Point(12, 47);
            this.grpboxHeader.Size = new System.Drawing.Size(2104, 233);
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(365, 25);
            this.lblDate.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(266, 29);
            this.lblDate.TabIndex = 25;
            this.lblDate.Text = "32569_lblNuevaFechaCta";
            // 
            // dtNuevaFechaCta
            // 
            this.dtNuevaFechaCta.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtNuevaFechaCta.Location = new System.Drawing.Point(650, 21);
            this.dtNuevaFechaCta.Name = "dtNuevaFechaCta";
            this.dtNuevaFechaCta.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtNuevaFechaCta.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtNuevaFechaCta.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtNuevaFechaCta.Properties.Appearance.Options.UseBackColor = true;
            this.dtNuevaFechaCta.Properties.Appearance.Options.UseFont = true;
            this.dtNuevaFechaCta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtNuevaFechaCta.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtNuevaFechaCta.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtNuevaFechaCta.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtNuevaFechaCta.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtNuevaFechaCta.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtNuevaFechaCta.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtNuevaFechaCta.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtNuevaFechaCta.Size = new System.Drawing.Size(169, 36);
            this.dtNuevaFechaCta.TabIndex = 26;
            this.dtNuevaFechaCta.DateTimeChanged += new System.EventHandler(this.dtNuevaFechaCta_DateTimeChanged);
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(195, 21);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(121, 36);
            this.txtLibranza.TabIndex = 28;
            this.txtLibranza.Leave += new System.EventHandler(this.txtLibranza_Leave);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(9, 24);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(180, 29);
            this.lblLibranza.TabIndex = 27;
            this.lblLibranza.Text = "32569_Libranza";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacion.Location = new System.Drawing.Point(195, 103);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(427, 76);
            this.txtObservacion.TabIndex = 30;
            // 
            // lblObservacion
            // 
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.Location = new System.Drawing.Point(9, 127);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(221, 29);
            this.lblObservacion.TabIndex = 29;
            this.lblObservacion.Text = "32569_Observacion";
            // 
            // CambioFechaPlanPagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.ClientSize = new System.Drawing.Size(2202, 1117);
            this.Margin = new System.Windows.Forms.Padding(12);
            this.Name = "CambioFechaPlanPagos";
            this.Text = "32565";
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
            ((System.ComponentModel.ISupportInitialize)(this.dtNuevaFechaCta.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtNuevaFechaCta.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblDate;
        protected DevExpress.XtraEditors.DateEdit dtNuevaFechaCta;
        private System.Windows.Forms.TextBox txtLibranza;
        private System.Windows.Forms.Label lblLibranza;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Label lblObservacion;

    }
}