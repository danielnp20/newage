namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionSolicitudFin
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.lblFecha = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblObservacion = new DevExpress.XtraEditors.LabelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLibranza = new DevExpress.XtraEditors.TextEdit();
            this.mfCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(repositoryItemRichTextEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            this.gcDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editViewReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLibranza.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            repositoryItemRichTextEdit4.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.WordML;
            repositoryItemRichTextEdit4.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            repositoryItemRichTextEdit4.Name = "richText";
            repositoryItemRichTextEdit4.ShowCaretInReadOnly = false;
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
            // cmbUserTareas
            // 
            this.cmbUserTareas.Location = new System.Drawing.Point(745, 12);
            this.cmbUserTareas.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserTareas.Size = new System.Drawing.Size(113, 22);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserTareas.Location = new System.Drawing.Point(745, 13);
            this.lblUserTareas.Margin = new System.Windows.Forms.Padding(6);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.label1);
            this.grpboxHeader.Controls.Add(this.txtLibranza);
            this.grpboxHeader.Controls.Add(this.mfCliente);
            this.grpboxHeader.Controls.Add(this.lblFecha);
            this.grpboxHeader.Controls.Add(this.dtFecha);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(2);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(2);
            this.grpboxHeader.Size = new System.Drawing.Size(852, 73);
            this.grpboxHeader.Visible = true;
            this.grpboxHeader.Controls.SetChildIndex(this.chkSeleccionar, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.cmbUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.dtFecha, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblFecha, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.mfCliente, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.txtLibranza, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.label1, 0);
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.Location = new System.Drawing.Point(8, 53);
            this.chkSeleccionar.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkSeleccionar.Text = "32572_CheckAll";
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P2";
            this.editSpinPorc.Mask.UseMaskAsDisplayFormat = true;
            // 
            // gcDetails
            // 
            this.gcDetails.Controls.Add(this.lblObservacion);
            this.gcDetails.Controls.Add(this.txtObservacion);
            this.gcDetails.Margin = new System.Windows.Forms.Padding(2);
            this.gcDetails.Size = new System.Drawing.Size(856, 85);
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "n0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblFecha
            // 
            this.lblFecha.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFecha.Location = new System.Drawing.Point(8, 13);
            this.lblFecha.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(74, 14);
            this.lblFecha.TabIndex = 25;
            this.lblFecha.Text = "32572_Fecha";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(63, 10);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(1);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Appearance.Options.UseFont = true;
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
            this.dtFecha.Size = new System.Drawing.Size(91, 20);
            this.dtFecha.TabIndex = 26;
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(182, 16);
            this.txtObservacion.Margin = new System.Windows.Forms.Padding(2);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.ReadOnly = true;
            this.txtObservacion.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtObservacion.Size = new System.Drawing.Size(403, 41);
            this.txtObservacion.TabIndex = 0;
            // 
            // lblObservacion
            // 
            this.lblObservacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblObservacion.Location = new System.Drawing.Point(9, 17);
            this.lblObservacion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(108, 14);
            this.lblObservacion.TabIndex = 26;
            this.lblObservacion.Text = "32572_Observacion";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label1.Location = new System.Drawing.Point(225, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 80;
            this.label1.Text = "32317_Libranza";
            // 
            // txtLibranza
            // 
            this.txtLibranza.EditValue = "";
            this.txtLibranza.Location = new System.Drawing.Point(345, 36);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtLibranza.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Properties.Appearance.Options.UseBorderColor = true;
            this.txtLibranza.Properties.Appearance.Options.UseFont = true;
            this.txtLibranza.Properties.Appearance.Options.UseTextOptions = true;
            this.txtLibranza.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtLibranza.Properties.AutoHeight = false;
            this.txtLibranza.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtLibranza.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtLibranza.Properties.Mask.EditMask = "n0";
            this.txtLibranza.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtLibranza.Size = new System.Drawing.Size(67, 20);
            this.txtLibranza.TabIndex = 81;
            this.txtLibranza.Leave += new System.EventHandler(this.txtLibranza_Leave);
            // 
            // mfCliente
            // 
            this.mfCliente.BackColor = System.Drawing.Color.Transparent;
            this.mfCliente.Filtros = null;
            this.mfCliente.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mfCliente.Location = new System.Drawing.Point(228, 8);
            this.mfCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mfCliente.Name = "mfCliente";
            this.mfCliente.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mfCliente.Size = new System.Drawing.Size(351, 25);
            this.mfCliente.TabIndex = 79;
            this.mfCliente.Value = "";
            this.mfCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // AprobacionSolicitudFin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 390);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AprobacionSolicitudFin";
            this.Text = "32579_AprobacionViabilidad";
            ((System.ComponentModel.ISupportInitialize)(repositoryItemRichTextEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            this.gcDetails.ResumeLayout(false);
            this.gcDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editViewReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLibranza.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblFecha;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private DevExpress.XtraEditors.LabelControl lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtLibranza;
        private ControlsUC.uc_MasterFind mfCliente;
    }
}