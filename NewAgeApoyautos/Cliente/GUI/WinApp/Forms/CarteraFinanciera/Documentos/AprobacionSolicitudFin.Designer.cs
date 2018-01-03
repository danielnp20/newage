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
            this.lblFecha = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblObservacion = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            this.gcDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.richText,
            this.riPopup,
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editLink,
            this.editSpinPorc});
            // 
            // richEditControl
            // 
            this.richEditControl.Margin = new System.Windows.Forms.Padding(12);
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
            this.cmbUserTareas.Location = new System.Drawing.Point(1117, 18);
            this.cmbUserTareas.Margin = new System.Windows.Forms.Padding(3);
            this.cmbUserTareas.Size = new System.Drawing.Size(168, 30);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserTareas.Location = new System.Drawing.Point(1117, 20);
            this.lblUserTareas.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lblFecha);
            this.grpboxHeader.Controls.Add(this.dtFecha);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(3);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(3);
            this.grpboxHeader.Size = new System.Drawing.Size(1287, 55);
            this.grpboxHeader.Visible = true;
            this.grpboxHeader.Controls.SetChildIndex(this.chkSeleccionar, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.cmbUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.dtFecha, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblFecha, 0);
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.Location = new System.Drawing.Point(12, 62);
            this.chkSeleccionar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.gcDetails.Size = new System.Drawing.Size(1291, 129);
            // 
            // lblFecha
            // 
            this.lblFecha.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFecha.Location = new System.Drawing.Point(12, 20);
            this.lblFecha.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(105, 22);
            this.lblFecha.TabIndex = 25;
            this.lblFecha.Text = "32572_Fecha";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(271, 18);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(2);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Appearance.Options.UseFont = true;
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Size = new System.Drawing.Size(144, 28);
            this.dtFecha.TabIndex = 26;
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(273, 25);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.ReadOnly = true;
            this.txtObservacion.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtObservacion.Size = new System.Drawing.Size(603, 61);
            this.txtObservacion.TabIndex = 0;
            // 
            // lblObservacion
            // 
            this.lblObservacion.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblObservacion.Location = new System.Drawing.Point(14, 26);
            this.lblObservacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(156, 22);
            this.lblObservacion.TabIndex = 26;
            this.lblObservacion.Text = "32572_Observacion";
            // 
            // AprobacionViabilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 600);
            this.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.Name = "AprobacionViabilidad";
            this.Text = "32579_AprobacionViabilidad";
            ((System.ComponentModel.ISupportInitialize)(this.richText)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblFecha;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private DevExpress.XtraEditors.LabelControl lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;

    }
}