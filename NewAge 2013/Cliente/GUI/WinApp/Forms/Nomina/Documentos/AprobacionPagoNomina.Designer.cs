namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionPagoNomina
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
        private void InitializeComponent()
        {
            base.InitializeComponent();
            this.lblTotalNomina = new System.Windows.Forms.Label();
            this.txtTotalNomina = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalPagados = new DevExpress.XtraEditors.TextEdit();
            this.lblTotalPagados = new System.Windows.Forms.Label();
            this.txtTotalAPagar = new DevExpress.XtraEditors.TextEdit();
            this.lblTotalAPagar = new System.Windows.Forms.Label();
            this.txtTotalPendientes = new DevExpress.XtraEditors.TextEdit();
            this.lblTotalPendientes = new System.Windows.Forms.Label();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpDocumentos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalNomina.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPagados.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAPagar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPendientes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.txtTotalPendientes);
            this.grpboxDetail.Controls.Add(this.lblTotalPendientes);
            this.grpboxDetail.Controls.Add(this.txtTotalAPagar);
            this.grpboxDetail.Controls.Add(this.lblTotalAPagar);
            this.grpboxDetail.Controls.Add(this.txtTotalPagados);
            this.grpboxDetail.Controls.Add(this.lblTotalPagados);
            this.grpboxDetail.Controls.Add(this.txtTotalNomina);
            this.grpboxDetail.Controls.Add(this.lblTotalNomina);
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
            this.editLook});
            // 
            // editCmb
            // 
            this.editCmb.AppearanceDropDown.BackColor = System.Drawing.Color.LightGray;
            this.editCmb.AppearanceDropDown.Options.UseBackColor = true;
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
            this.editValue.Mask.EditMask = "c0";
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
            // lblNumeroDoc
            // 
            this.lblNumeroDoc.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblPrefix
            // 
            this.lblPrefix.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lookUpDocumentos
            // 
            // 
            // lblTotalNomina
            // 
            this.lblTotalNomina.AutoSize = true;
            this.lblTotalNomina.Location = new System.Drawing.Point(852, 17);
            this.lblTotalNomina.Name = "lblTotalNomina";
            this.lblTotalNomina.Size = new System.Drawing.Size(113, 13);
            this.lblTotalNomina.TabIndex = 1;
            this.lblTotalNomina.Text = "80551_lblTotalNomina";
            // 
            // txtTotalNomina
            // 
            this.txtTotalNomina.Enabled = false;
            this.txtTotalNomina.Location = new System.Drawing.Point(995, 14);
            this.txtTotalNomina.Name = "txtTotalNomina";
            this.txtTotalNomina.Properties.Mask.EditMask = "c0";
            this.txtTotalNomina.Size = new System.Drawing.Size(100, 20);
            this.txtTotalNomina.TabIndex = 2;
            // 
            // txtTotalPagados
            // 
            this.txtTotalPagados.Enabled = false;
            this.txtTotalPagados.Location = new System.Drawing.Point(995, 40);
            this.txtTotalPagados.Name = "txtTotalPagados";
            this.txtTotalPagados.Properties.Mask.EditMask = "c0";
            this.txtTotalPagados.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalPagados.Size = new System.Drawing.Size(100, 20);
            this.txtTotalPagados.TabIndex = 4;
            // 
            // lblTotalPagados
            // 
            this.lblTotalPagados.AutoSize = true;
            this.lblTotalPagados.Location = new System.Drawing.Point(852, 43);
            this.lblTotalPagados.Name = "lblTotalPagados";
            this.lblTotalPagados.Size = new System.Drawing.Size(119, 13);
            this.lblTotalPagados.TabIndex = 3;
            this.lblTotalPagados.Text = "80551_lblTotalPagados";
            // 
            // txtTotalAPagar
            // 
            this.txtTotalAPagar.Enabled = false;
            this.txtTotalAPagar.Location = new System.Drawing.Point(995, 66);
            this.txtTotalAPagar.Name = "txtTotalAPagar";
            this.txtTotalAPagar.Properties.Mask.EditMask = "c0";
            this.txtTotalAPagar.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalAPagar.Size = new System.Drawing.Size(100, 20);
            this.txtTotalAPagar.TabIndex = 6;
            // 
            // lblTotalAPagar
            // 
            this.lblTotalAPagar.AutoSize = true;
            this.lblTotalAPagar.Location = new System.Drawing.Point(852, 69);
            this.lblTotalAPagar.Name = "lblTotalAPagar";
            this.lblTotalAPagar.Size = new System.Drawing.Size(112, 13);
            this.lblTotalAPagar.TabIndex = 5;
            this.lblTotalAPagar.Text = "80551_lblTotalAPagar";
            // 
            // txtTotalPendientes
            // 
            this.txtTotalPendientes.Enabled = false;
            this.txtTotalPendientes.Location = new System.Drawing.Point(995, 92);
            this.txtTotalPendientes.Name = "txtTotalPendientes";
            this.txtTotalPendientes.Properties.Mask.EditMask = "c0";
            this.txtTotalPendientes.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalPendientes.Size = new System.Drawing.Size(100, 20);
            this.txtTotalPendientes.TabIndex = 8;
            // 
            // lblTotalPendientes
            // 
            this.lblTotalPendientes.AutoSize = true;
            this.lblTotalPendientes.Location = new System.Drawing.Point(852, 95);
            this.lblTotalPendientes.Name = "lblTotalPendientes";
            this.lblTotalPendientes.Size = new System.Drawing.Size(130, 13);
            this.lblTotalPendientes.TabIndex = 7;
            this.lblTotalPendientes.Text = "80551_lblTotalPendientes";
            // 
            // AprobacionPagoNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "AprobacionPagoNomina";
            this.Text = "PagoNomina";
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpDocumentos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalNomina.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPagados.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalAPagar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPendientes.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtTotalPendientes;
        private System.Windows.Forms.Label lblTotalPendientes;
        private DevExpress.XtraEditors.TextEdit txtTotalAPagar;
        private System.Windows.Forms.Label lblTotalAPagar;
        private DevExpress.XtraEditors.TextEdit txtTotalPagados;
        private System.Windows.Forms.Label lblTotalPagados;
        private DevExpress.XtraEditors.TextEdit txtTotalNomina;
        private System.Windows.Forms.Label lblTotalNomina;
    }
}