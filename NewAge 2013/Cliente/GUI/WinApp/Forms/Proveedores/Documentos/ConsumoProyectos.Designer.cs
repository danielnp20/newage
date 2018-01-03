namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsumoProyectos
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
            this.lblNroConsumo = new System.Windows.Forms.Label();
            this.txtNroConsumo = new DevExpress.XtraEditors.TextEdit();
            this.lblFechaRadicado = new System.Windows.Forms.Label();
            this.lblFechaConsumo = new System.Windows.Forms.Label();
            this.dtFechaConsumo = new DevExpress.XtraEditors.DateEdit();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtTotal = new DevExpress.XtraEditors.TextEdit();
            this.lblTotal = new System.Windows.Forms.Label();
            this.masterCtoCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLocFisica = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.btnProyecto = new DevExpress.XtraEditors.SimpleButton();
            this.lblOrdenTrabajoNro = new System.Windows.Forms.Label();
            this.txtOrdenTrabajoNro = new DevExpress.XtraEditors.TextEdit();
            this.masterPrefOrdenTrabajo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnConvenios = new DevExpress.XtraEditors.SimpleButton();
            this.lblOrdenTrabajo = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.txtNroConsumo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsumo.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsumo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrdenTrabajoNro.Properties)).BeginInit();
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
            this.grpboxHeader.Controls.Add(this.lblOrdenTrabajo);
            this.grpboxHeader.Controls.Add(this.btnConvenios);
            this.grpboxHeader.Controls.Add(this.masterPrefOrdenTrabajo);
            this.grpboxHeader.Controls.Add(this.btnProyecto);
            this.grpboxHeader.Controls.Add(this.txtOrdenTrabajoNro);
            this.grpboxHeader.Controls.Add(this.btnQueryDoc);
            this.grpboxHeader.Controls.Add(this.masterLocFisica);
            this.grpboxHeader.Controls.Add(this.masterCtoCosto);
            this.grpboxHeader.Controls.Add(this.txtTotal);
            this.grpboxHeader.Controls.Add(this.lblTotal);
            this.grpboxHeader.Controls.Add(this.masterProyecto);
            this.grpboxHeader.Controls.Add(this.lblNroConsumo);
            this.grpboxHeader.Controls.Add(this.txtNroConsumo);
            this.grpboxHeader.Controls.Add(this.lblFechaConsumo);
            this.grpboxHeader.Controls.Add(this.dtFechaConsumo);
            this.grpboxHeader.Controls.Add(this.lblOrdenTrabajoNro);
            this.grpboxHeader.Size = new System.Drawing.Size(1102, 351);
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
            // lblNroConsumo
            // 
            this.lblNroConsumo.AutoSize = true;
            this.lblNroConsumo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroConsumo.Location = new System.Drawing.Point(241, 21);
            this.lblNroConsumo.Name = "lblNroConsumo";
            this.lblNroConsumo.Size = new System.Drawing.Size(108, 14);
            this.lblNroConsumo.TabIndex = 92;
            this.lblNroConsumo.Text = "75_lblNroConsumo";
            // 
            // txtNroConsumo
            // 
            this.txtNroConsumo.Location = new System.Drawing.Point(349, 19);
            this.txtNroConsumo.Name = "txtNroConsumo";
            this.txtNroConsumo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroConsumo.Properties.Appearance.Options.UseFont = true;
            this.txtNroConsumo.Size = new System.Drawing.Size(42, 20);
            this.txtNroConsumo.TabIndex = 2;
            this.txtNroConsumo.Leave += new System.EventHandler(this.txtNroConsumo_Leave);
            // 
            // lblFechaRadicado
            // 
            this.lblFechaRadicado.Location = new System.Drawing.Point(0, 0);
            this.lblFechaRadicado.Name = "lblFechaRadicado";
            this.lblFechaRadicado.Size = new System.Drawing.Size(100, 23);
            this.lblFechaRadicado.TabIndex = 0;
            // 
            // lblFechaConsumo
            // 
            this.lblFechaConsumo.AutoSize = true;
            this.lblFechaConsumo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaConsumo.Location = new System.Drawing.Point(9, 21);
            this.lblFechaConsumo.Name = "lblFechaConsumo";
            this.lblFechaConsumo.Size = new System.Drawing.Size(121, 14);
            this.lblFechaConsumo.TabIndex = 7;
            this.lblFechaConsumo.Text = "75_lblFechaConsumo";
            // 
            // dtFechaConsumo
            // 
            this.dtFechaConsumo.EditValue = new System.DateTime(2013, 11, 15, 0, 0, 0, 0);
            this.dtFechaConsumo.Location = new System.Drawing.Point(129, 18);
            this.dtFechaConsumo.Name = "dtFechaConsumo";
            this.dtFechaConsumo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaConsumo.Properties.Appearance.Options.UseFont = true;
            this.dtFechaConsumo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaConsumo.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaConsumo.Size = new System.Drawing.Size(99, 20);
            this.dtFechaConsumo.TabIndex = 3;
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterProyecto.Location = new System.Drawing.Point(11, 75);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(345, 27);
            this.masterProyecto.TabIndex = 98;
            this.masterProyecto.Value = "";
            // 
            // txtTotal
            // 
            this.txtTotal.EditValue = "0";
            this.txtTotal.Location = new System.Drawing.Point(900, 81);
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
            this.txtTotal.Size = new System.Drawing.Size(132, 20);
            this.txtTotal.TabIndex = 105;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(808, 83);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(76, 14);
            this.lblTotal.TabIndex = 106;
            this.lblTotal.Text = "75_lblTotal";
            // 
            // masterCtoCosto
            // 
            this.masterCtoCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCtoCosto.Filtros = null;
            this.masterCtoCosto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCtoCosto.Location = new System.Drawing.Point(441, 75);
            this.masterCtoCosto.Name = "masterCtoCosto";
            this.masterCtoCosto.Size = new System.Drawing.Size(345, 27);
            this.masterCtoCosto.TabIndex = 107;
            this.masterCtoCosto.Value = "";
            // 
            // masterLocFisica
            // 
            this.masterLocFisica.BackColor = System.Drawing.Color.Transparent;
            this.masterLocFisica.Filtros = null;
            this.masterLocFisica.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterLocFisica.Location = new System.Drawing.Point(11, 44);
            this.masterLocFisica.Name = "masterLocFisica";
            this.masterLocFisica.Size = new System.Drawing.Size(345, 27);
            this.masterLocFisica.TabIndex = 108;
            this.masterLocFisica.Value = "";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(393, 19);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(28, 20);
            this.btnQueryDoc.TabIndex = 21427;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // btnProyecto
            // 
            this.btnProyecto.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnProyecto.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnProyecto.Location = new System.Drawing.Point(948, 21);
            this.btnProyecto.Name = "btnProyecto";
            this.btnProyecto.Size = new System.Drawing.Size(28, 20);
            this.btnProyecto.TabIndex = 21430;
            this.btnProyecto.ToolTip = "1005_btnQueryDoc";
            this.btnProyecto.Visible = false;
            // 
            // lblOrdenTrabajoNro
            // 
            this.lblOrdenTrabajoNro.AutoSize = true;
            this.lblOrdenTrabajoNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrdenTrabajoNro.Location = new System.Drawing.Point(805, 23);
            this.lblOrdenTrabajoNro.Name = "lblOrdenTrabajoNro";
            this.lblOrdenTrabajoNro.Size = new System.Drawing.Size(133, 14);
            this.lblOrdenTrabajoNro.TabIndex = 21429;
            this.lblOrdenTrabajoNro.Text = "75_lblOrdenTrabajoNro";
            this.lblOrdenTrabajoNro.Visible = false;
            // 
            // txtOrdenTrabajoNro
            // 
            this.txtOrdenTrabajoNro.Location = new System.Drawing.Point(900, 21);
            this.txtOrdenTrabajoNro.Name = "txtOrdenTrabajoNro";
            this.txtOrdenTrabajoNro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOrdenTrabajoNro.Properties.Appearance.Options.UseFont = true;
            this.txtOrdenTrabajoNro.Size = new System.Drawing.Size(45, 20);
            this.txtOrdenTrabajoNro.TabIndex = 21428;
            this.txtOrdenTrabajoNro.Visible = false;
            // 
            // masterPrefOrdenTrabajo
            // 
            this.masterPrefOrdenTrabajo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefOrdenTrabajo.Filtros = null;
            this.masterPrefOrdenTrabajo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterPrefOrdenTrabajo.Location = new System.Drawing.Point(441, 18);
            this.masterPrefOrdenTrabajo.Name = "masterPrefOrdenTrabajo";
            this.masterPrefOrdenTrabajo.Size = new System.Drawing.Size(345, 27);
            this.masterPrefOrdenTrabajo.TabIndex = 21431;
            this.masterPrefOrdenTrabajo.Value = "";
            this.masterPrefOrdenTrabajo.Visible = false;
            // 
            // btnConvenios
            // 
            this.btnConvenios.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnConvenios.Appearance.Options.UseFont = true;
            this.btnConvenios.Enabled = false;
            this.btnConvenios.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnConvenios.Location = new System.Drawing.Point(900, 55);
            this.btnConvenios.Name = "btnConvenios";
            this.btnConvenios.Size = new System.Drawing.Size(132, 20);
            this.btnConvenios.TabIndex = 21432;
            this.btnConvenios.Text = "75_btnViewConvenio";
            this.btnConvenios.Click += new System.EventHandler(this.btnConvenios_Click);
            // 
            // lblOrdenTrabajo
            // 
            this.lblOrdenTrabajo.AutoSize = true;
            this.lblOrdenTrabajo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrdenTrabajo.Location = new System.Drawing.Point(438, 24);
            this.lblOrdenTrabajo.Name = "lblOrdenTrabajo";
            this.lblOrdenTrabajo.Size = new System.Drawing.Size(114, 14);
            this.lblOrdenTrabajo.TabIndex = 21433;
            this.lblOrdenTrabajo.Text = "75_lblOrdenTrabajo";
            // 
            // ConsumoProyectos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "ConsumoProyectos";
            this.Text = "75";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtNroConsumo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsumo.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaConsumo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrdenTrabajoNro.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblFechaConsumo;
        private DevExpress.XtraEditors.DateEdit dtFechaConsumo;
        private System.Windows.Forms.Label lblFechaRadicado;
        private System.Windows.Forms.Label lblNroConsumo;
        private DevExpress.XtraEditors.TextEdit txtNroConsumo;
        private DevExpress.XtraEditors.TextEdit txtFechaRadicacion;
        private DevExpress.XtraEditors.TextEdit txtTotal;
        private System.Windows.Forms.Label lblTotal;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterLocFisica;
        private ControlsUC.uc_MasterFind masterCtoCosto;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private ControlsUC.uc_MasterFind masterPrefOrdenTrabajo;
        private DevExpress.XtraEditors.SimpleButton btnProyecto;
        private System.Windows.Forms.Label lblOrdenTrabajoNro;
        private DevExpress.XtraEditors.TextEdit txtOrdenTrabajoNro;
        private DevExpress.XtraEditors.SimpleButton btnConvenios;
        private System.Windows.Forms.Label lblOrdenTrabajo;
    }
}