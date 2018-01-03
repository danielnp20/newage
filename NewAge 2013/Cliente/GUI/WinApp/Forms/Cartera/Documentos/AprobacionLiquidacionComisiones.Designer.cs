namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionLiquidacionComisiones
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lblFechaAprobComisiones = new DevExpress.XtraEditors.LabelControl();
            this.dtAprobComisiones = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAprobComisiones.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAprobComisiones.Properties)).BeginInit();
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
            this.cmbUserTareas.Location = new System.Drawing.Point(1489, 23);
            this.cmbUserTareas.Margin = new System.Windows.Forms.Padding(4);
            this.cmbUserTareas.Size = new System.Drawing.Size(222, 33);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserTareas.Location = new System.Drawing.Point(1489, 25);
            this.lblUserTareas.Margin = new System.Windows.Forms.Padding(12);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lblFechaAprobComisiones);
            this.grpboxHeader.Controls.Add(this.dtAprobComisiones);
            this.grpboxHeader.Location = new System.Drawing.Point(2, 2);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(4);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(4);
            this.grpboxHeader.Size = new System.Drawing.Size(1716, 71);
            this.grpboxHeader.Visible = true;
            this.grpboxHeader.Controls.SetChildIndex(this.chkSeleccionar, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.cmbUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.dtAprobComisiones, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblFechaAprobComisiones, 0);
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.Location = new System.Drawing.Point(16, 77);
            this.chkSeleccionar.Text = "32571_CheckAll";
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P2";
            this.editSpinPorc.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblFechaAprobComisiones
            // 
            this.lblFechaAprobComisiones.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaAprobComisiones.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFechaAprobComisiones.Location = new System.Drawing.Point(16, 25);
            this.lblFechaAprobComisiones.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblFechaAprobComisiones.Name = "lblFechaAprobComisiones";
            this.lblFechaAprobComisiones.Size = new System.Drawing.Size(321, 29);
            this.lblFechaAprobComisiones.TabIndex = 25;
            this.lblFechaAprobComisiones.Text = "32570_FechaAprobComisiones";
            // 
            // dtAprobComisiones
            // 
            this.dtAprobComisiones.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtAprobComisiones.Location = new System.Drawing.Point(361, 22);
            this.dtAprobComisiones.Name = "dtAprobComisiones";
            this.dtAprobComisiones.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtAprobComisiones.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtAprobComisiones.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtAprobComisiones.Properties.Appearance.Options.UseBackColor = true;
            this.dtAprobComisiones.Properties.Appearance.Options.UseFont = true;
            this.dtAprobComisiones.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtAprobComisiones.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtAprobComisiones.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtAprobComisiones.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtAprobComisiones.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtAprobComisiones.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtAprobComisiones.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtAprobComisiones.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtAprobComisiones.Size = new System.Drawing.Size(192, 36);
            this.dtAprobComisiones.TabIndex = 26;
            //this.dtAprobComisiones.Enter += new System.EventHandler(this.dtFecha_Enter);
            this.dtAprobComisiones.Leave += new System.EventHandler(this.dtAprobComisiones_Leave);
            // 
            // AprobacionLiquidacionComisiones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1806, 750);
            this.Margin = new System.Windows.Forms.Padding(12);
            this.Name = "AprobacionLiquidacionComisiones";
            this.Text = "32570_AprobacionLiquidacionComisiones";
            ((System.ComponentModel.ISupportInitialize)(this.richText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAprobComisiones.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAprobComisiones.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblFechaAprobComisiones;
        protected DevExpress.XtraEditors.DateEdit dtAprobComisiones;
        //protected uc_PeriodoEdit dtAprobComisiones;

    }
}