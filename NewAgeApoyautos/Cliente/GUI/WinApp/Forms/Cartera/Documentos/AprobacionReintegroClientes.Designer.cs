namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionReintegroClientes
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterComponentes = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtVlrTotalReintegro = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrGiro = new System.Windows.Forms.Label();
            this.lbldtAprobReintegro = new DevExpress.XtraEditors.LabelControl();
            this.dtAprobReintegro = new DevExpress.XtraEditors.DateEdit();
            this.masterSaldos = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lkpTipoAprobacion = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalReintegro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAprobReintegro.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAprobReintegro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoAprobacion.Properties)).BeginInit();
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
            this.richEditControl.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
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
            this.cmbUserTareas.Location = new System.Drawing.Point(1037, 11);
            this.cmbUserTareas.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserTareas.Size = new System.Drawing.Size(113, 22);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserTareas.Location = new System.Drawing.Point(1027, 13);
            this.lblUserTareas.Margin = new System.Windows.Forms.Padding(6);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lkpTipoAprobacion);
            this.grpboxHeader.Controls.Add(this.labelControl1);
            this.grpboxHeader.Controls.Add(this.masterSaldos);
            this.grpboxHeader.Controls.Add(this.lbldtAprobReintegro);
            this.grpboxHeader.Controls.Add(this.dtAprobReintegro);
            this.grpboxHeader.Controls.Add(this.txtVlrTotalReintegro);
            this.grpboxHeader.Controls.Add(this.lblVlrGiro);
            this.grpboxHeader.Controls.Add(this.masterComponentes);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(2);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(2);
            this.grpboxHeader.Size = new System.Drawing.Size(1146, 58);
            this.grpboxHeader.Visible = true;
            this.grpboxHeader.Controls.SetChildIndex(this.chkSeleccionar, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.cmbUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.masterComponentes, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblVlrGiro, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.txtVlrTotalReintegro, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.dtAprobReintegro, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lbldtAprobReintegro, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.masterSaldos, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.labelControl1, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lkpTipoAprobacion, 0);
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.Location = new System.Drawing.Point(8, 36);
            this.chkSeleccionar.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkSeleccionar.Size = new System.Drawing.Size(99, 18);
            this.chkSeleccionar.Text = "32571_chkAll";
            this.chkSeleccionar.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P";
            // 
            // gcDetails
            // 
            this.gcDetails.Size = new System.Drawing.Size(1150, 85);
            // 
            // masterComponentes
            // 
            this.masterComponentes.BackColor = System.Drawing.Color.Transparent;
            this.masterComponentes.Filtros = null;
            this.masterComponentes.Location = new System.Drawing.Point(267, 9);
            this.masterComponentes.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.masterComponentes.Name = "masterComponentes";
            this.masterComponentes.Size = new System.Drawing.Size(302, 23);
            this.masterComponentes.TabIndex = 2;
            this.masterComponentes.Value = "";
            this.masterComponentes.Leave += new System.EventHandler(this.masterComponentes_Leave);
            // 
            // txtVlrTotalReintegro
            // 
            this.txtVlrTotalReintegro.EditValue = "0";
            this.txtVlrTotalReintegro.Location = new System.Drawing.Point(977, 12);
            this.txtVlrTotalReintegro.Name = "txtVlrTotalReintegro";
            this.txtVlrTotalReintegro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrTotalReintegro.Properties.Appearance.Options.UseFont = true;
            this.txtVlrTotalReintegro.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrTotalReintegro.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrTotalReintegro.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrTotalReintegro.Properties.Mask.EditMask = "c";
            this.txtVlrTotalReintegro.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrTotalReintegro.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrTotalReintegro.Properties.ReadOnly = true;
            this.txtVlrTotalReintegro.Size = new System.Drawing.Size(106, 20);
            this.txtVlrTotalReintegro.TabIndex = 4;
            // 
            // lblVlrGiro
            // 
            this.lblVlrGiro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVlrGiro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrGiro.Location = new System.Drawing.Point(838, 15);
            this.lblVlrGiro.Name = "lblVlrGiro";
            this.lblVlrGiro.Size = new System.Drawing.Size(149, 37);
            this.lblVlrGiro.TabIndex = 3;
            this.lblVlrGiro.Text = "32571_VlrTotalReintegro";
            // 
            // lbldtAprobReintegro
            // 
            this.lbldtAprobReintegro.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldtAprobReintegro.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lbldtAprobReintegro.Location = new System.Drawing.Point(565, 16);
            this.lbldtAprobReintegro.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbldtAprobReintegro.Name = "lbldtAprobReintegro";
            this.lbldtAprobReintegro.Size = new System.Drawing.Size(140, 14);
            this.lbldtAprobReintegro.TabIndex = 25;
            this.lbldtAprobReintegro.Text = "32571_dtAprobReintegro";
            // 
            // dtAprobReintegro
            // 
            this.dtAprobReintegro.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtAprobReintegro.Location = new System.Drawing.Point(732, 13);
            this.dtAprobReintegro.Margin = new System.Windows.Forms.Padding(1);
            this.dtAprobReintegro.Name = "dtAprobReintegro";
            this.dtAprobReintegro.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtAprobReintegro.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtAprobReintegro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtAprobReintegro.Properties.Appearance.Options.UseBackColor = true;
            this.dtAprobReintegro.Properties.Appearance.Options.UseFont = true;
            this.dtAprobReintegro.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtAprobReintegro.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtAprobReintegro.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtAprobReintegro.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtAprobReintegro.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtAprobReintegro.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtAprobReintegro.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtAprobReintegro.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtAprobReintegro.Size = new System.Drawing.Size(96, 20);
            this.dtAprobReintegro.TabIndex = 26;
            // 
            // masterSaldos
            // 
            this.masterSaldos.BackColor = System.Drawing.Color.Transparent;
            this.masterSaldos.Filtros = null;
            this.masterSaldos.Location = new System.Drawing.Point(264, 11);
            this.masterSaldos.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.masterSaldos.Name = "masterSaldos";
            this.masterSaldos.Size = new System.Drawing.Size(302, 23);
            this.masterSaldos.TabIndex = 27;
            this.masterSaldos.Value = "";
            this.masterSaldos.Leave += new System.EventHandler(this.masterSaldos_Leave);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.labelControl1.Location = new System.Drawing.Point(8, 16);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(127, 14);
            this.labelControl1.TabIndex = 28;
            this.labelControl1.Text = "32571_TipoAprobacion";
            // 
            // lkpTipoAprobacion
            // 
            this.lkpTipoAprobacion.Location = new System.Drawing.Point(141, 14);
            this.lkpTipoAprobacion.Name = "lkpTipoAprobacion";
            this.lkpTipoAprobacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpTipoAprobacion.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkpTipoAprobacion.Properties.DisplayMember = "Value";
            this.lkpTipoAprobacion.Properties.NullText = " ";
            this.lkpTipoAprobacion.Properties.ValueMember = "Key";
            this.lkpTipoAprobacion.Size = new System.Drawing.Size(114, 20);
            this.lkpTipoAprobacion.TabIndex = 74;
            this.lkpTipoAprobacion.EditValueChanged += new System.EventHandler(this.lkpTipoAprobacion_EditValueChanged);
            // 
            // AprobacionReintegroClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 501);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AprobacionReintegroClientes";
            this.Text = "32561_AprobacionGiro";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrTotalReintegro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAprobReintegro.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAprobReintegro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoAprobacion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterComponentes;
        private DevExpress.XtraEditors.TextEdit txtVlrTotalReintegro;
        private System.Windows.Forms.Label lblVlrGiro;
        private DevExpress.XtraEditors.LabelControl lbldtAprobReintegro;
        protected DevExpress.XtraEditors.DateEdit dtAprobReintegro;
        private ControlsUC.uc_MasterFind masterSaldos;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit lkpTipoAprobacion;

    }
}