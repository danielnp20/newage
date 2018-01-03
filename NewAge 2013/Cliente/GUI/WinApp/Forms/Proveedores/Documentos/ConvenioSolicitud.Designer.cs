namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConvenioSolicitud
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.txtFechaRadicacion = new DevExpress.XtraEditors.TextEdit();
            this.lblNroOrdenCompra = new System.Windows.Forms.Label();
            this.txtNroOrdenCompra = new DevExpress.XtraEditors.TextEdit();
            this.lblFechaRadicado = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblFechaFactura = new System.Windows.Forms.Label();
            this.dtFechaFactura = new DevExpress.XtraEditors.DateEdit();
            this.masterProveedor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterMoneda = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtTotal = new DevExpress.XtraEditors.TextEdit();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.btnConvenios = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaRadicacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroOrdenCompra.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Size = new System.Drawing.Size(1109, 159);
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
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
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
            this.gbGridDocument.Size = new System.Drawing.Size(659, 243);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(659, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(457, 243);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.btnConvenios);
            this.grpboxHeader.Controls.Add(this.btnQueryDoc);
            this.grpboxHeader.Controls.Add(this.txtTotal);
            this.grpboxHeader.Controls.Add(this.lblTotal);
            this.grpboxHeader.Controls.Add(this.masterMoneda);
            this.grpboxHeader.Controls.Add(this.masterProveedor);
            this.grpboxHeader.Controls.Add(this.lblNroOrdenCompra);
            this.grpboxHeader.Controls.Add(this.txtNroOrdenCompra);
            this.grpboxHeader.Controls.Add(this.txtDescripcion);
            this.grpboxHeader.Controls.Add(this.lblDescripcion);
            this.grpboxHeader.Controls.Add(this.lblFechaFactura);
            this.grpboxHeader.Controls.Add(this.dtFechaFactura);
            this.grpboxHeader.Size = new System.Drawing.Size(1102, 353);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtFechaRadicacion
            // 
            this.txtFechaRadicacion.Location = new System.Drawing.Point(0, 0);
            this.txtFechaRadicacion.Name = "txtFechaRadicacion";
            this.txtFechaRadicacion.Size = new System.Drawing.Size(100, 20);
            this.txtFechaRadicacion.TabIndex = 0;
            // 
            // lblNroOrdenCompra
            // 
            this.lblNroOrdenCompra.AutoSize = true;
            this.lblNroOrdenCompra.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroOrdenCompra.Location = new System.Drawing.Point(356, 19);
            this.lblNroOrdenCompra.Name = "lblNroOrdenCompra";
            this.lblNroOrdenCompra.Size = new System.Drawing.Size(133, 14);
            this.lblNroOrdenCompra.TabIndex = 92;
            this.lblNroOrdenCompra.Text = "76_lblNroOrdenCompra";
            // 
            // txtNroOrdenCompra
            // 
            this.txtNroOrdenCompra.Location = new System.Drawing.Point(463, 16);
            this.txtNroOrdenCompra.Name = "txtNroOrdenCompra";
            this.txtNroOrdenCompra.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroOrdenCompra.Properties.Appearance.Options.UseFont = true;
            this.txtNroOrdenCompra.Size = new System.Drawing.Size(61, 20);
            this.txtNroOrdenCompra.TabIndex = 2;
            this.txtNroOrdenCompra.Leave += new System.EventHandler(this.txtNroOrdenCompra_Leave);
            // 
            // lblFechaRadicado
            // 
            this.lblFechaRadicado.Location = new System.Drawing.Point(0, 0);
            this.lblFechaRadicado.Name = "lblFechaRadicado";
            this.lblFechaRadicado.Size = new System.Drawing.Size(100, 23);
            this.lblFechaRadicado.TabIndex = 0;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescripcion.Location = new System.Drawing.Point(128, 73);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(893, 24);
            this.txtDescripcion.TabIndex = 8;
            this.txtDescripcion.Visible = false;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcion.Location = new System.Drawing.Point(8, 72);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(100, 14);
            this.lblDescripcion.TabIndex = 17;
            this.lblDescripcion.Text = "76_lblDescripcion";
            this.lblDescripcion.Visible = false;
            // 
            // lblFechaFactura
            // 
            this.lblFechaFactura.AutoSize = true;
            this.lblFechaFactura.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaFactura.Location = new System.Drawing.Point(573, 19);
            this.lblFechaFactura.Name = "lblFechaFactura";
            this.lblFechaFactura.Size = new System.Drawing.Size(111, 14);
            this.lblFechaFactura.TabIndex = 7;
            this.lblFechaFactura.Text = "76_lblFechaFactura";
            this.lblFechaFactura.Visible = false;
            // 
            // dtFechaFactura
            // 
            this.dtFechaFactura.EditValue = new System.DateTime(2013, 11, 15, 0, 0, 0, 0);
            this.dtFechaFactura.Location = new System.Drawing.Point(683, 16);
            this.dtFechaFactura.Name = "dtFechaFactura";
            this.dtFechaFactura.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaFactura.Properties.Appearance.Options.UseFont = true;
            this.dtFechaFactura.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaFactura.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaFactura.Size = new System.Drawing.Size(103, 20);
            this.dtFechaFactura.TabIndex = 3;
            this.dtFechaFactura.Visible = false;
            // 
            // masterProveedor
            // 
            this.masterProveedor.BackColor = System.Drawing.Color.Transparent;
            this.masterProveedor.Filtros = null;
            this.masterProveedor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterProveedor.Location = new System.Drawing.Point(11, 13);
            this.masterProveedor.Name = "masterProveedor";
            this.masterProveedor.Size = new System.Drawing.Size(345, 27);
            this.masterProveedor.TabIndex = 97;
            this.masterProveedor.Value = "";
            this.masterProveedor.Leave += new System.EventHandler(this.masterProveedor_Leave);
            // 
            // masterMoneda
            // 
            this.masterMoneda.BackColor = System.Drawing.Color.Transparent;
            this.masterMoneda.Filtros = null;
            this.masterMoneda.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterMoneda.Location = new System.Drawing.Point(10, 42);
            this.masterMoneda.Name = "masterMoneda";
            this.masterMoneda.Size = new System.Drawing.Size(345, 27);
            this.masterMoneda.TabIndex = 98;
            this.masterMoneda.Value = "";
            // 
            // txtTotal
            // 
            this.txtTotal.EditValue = "0";
            this.txtTotal.Location = new System.Drawing.Point(907, 47);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.Properties.Appearance.Options.UseFont = true;
            this.txtTotal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotal.Properties.Mask.EditMask = "c";
            this.txtTotal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotal.Properties.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(112, 20);
            this.txtTotal.TabIndex = 105;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(803, 49);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(76, 14);
            this.lblTotal.TabIndex = 106;
            this.lblTotal.Text = "76_lblTotal";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(525, 16);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(28, 20);
            this.btnQueryDoc.TabIndex = 21427;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // btnConvenios
            // 
            this.btnConvenios.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnConvenios.Appearance.Options.UseFont = true;
            this.btnConvenios.Enabled = false;
            this.btnConvenios.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnConvenios.Location = new System.Drawing.Point(907, 17);
            this.btnConvenios.Name = "btnConvenios";
            this.btnConvenios.Size = new System.Drawing.Size(112, 20);
            this.btnConvenios.TabIndex = 21428;
            this.btnConvenios.Text = "76_btnViewConvenio";
            this.btnConvenios.Click += new System.EventHandler(this.btnConvenios_Click);
            // 
            // ConvenioSolicitud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "ConvenioSolicitud";
            this.Text = "76";
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtFechaRadicacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroOrdenCompra.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Label lblFechaFactura;
        private DevExpress.XtraEditors.DateEdit dtFechaFactura;
        private System.Windows.Forms.Label lblFechaRadicado;
        private System.Windows.Forms.Label lblNroOrdenCompra;
        private DevExpress.XtraEditors.TextEdit txtNroOrdenCompra;
        private DevExpress.XtraEditors.TextEdit txtFechaRadicacion;
        private ControlsUC.uc_MasterFind masterProveedor;
        private DevExpress.XtraEditors.TextEdit txtTotal;
        private System.Windows.Forms.Label lblTotal;
        private ControlsUC.uc_MasterFind masterMoneda;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.SimpleButton btnConvenios;
    }
}