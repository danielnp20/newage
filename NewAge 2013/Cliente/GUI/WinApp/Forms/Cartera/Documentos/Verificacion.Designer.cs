namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Verificacion
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lkp_TipoIncorporacion = new DevExpress.XtraEditors.LookUpEdit();
            this.dtFechaVerificacion = new DevExpress.XtraEditors.DateEdit();
            this.lblTipoIncorp = new System.Windows.Forms.Label();
            this.masterCentroPago = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaVerificacion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupAnexos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlAnexos)).BeginInit();
            this.PopupContainerControlAnexos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlTareas)).BeginInit();
            this.PopupContainerControlTareas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RichTextTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RichTextAnexos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoIncorporacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVerificacion.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVerificacion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RichText,
            this.RichTextTareas,
            this.RichTextAnexos,
            this.riPopup,
            this.riPopupTareas,
            this.riPopupAnexos,
            this.editChkBox,
            this.editSpin});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            // 
            // grpctrlHeader
            // 
            this.grpctrlHeader.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.grpctrlHeader.Appearance.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.grpctrlHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.grpctrlHeader.AppearanceCaption.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.Options.UseFont = true;
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lkp_TipoIncorporacion);
            this.grpboxHeader.Controls.Add(this.dtFechaVerificacion);
            this.grpboxHeader.Controls.Add(this.lblTipoIncorp);
            this.grpboxHeader.Controls.Add(this.masterCentroPago);
            this.grpboxHeader.Controls.Add(this.lblFechaVerificacion);
            // 
            // lkp_TipoIncorporacion
            // 
            this.lkp_TipoIncorporacion.Location = new System.Drawing.Point(207, 15);
            this.lkp_TipoIncorporacion.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lkp_TipoIncorporacion.Name = "lkp_TipoIncorporacion";
            this.lkp_TipoIncorporacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_TipoIncorporacion.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkp_TipoIncorporacion.Properties.DisplayMember = "Value";
            this.lkp_TipoIncorporacion.Properties.NullText = " ";
            this.lkp_TipoIncorporacion.Properties.ValueMember = "Key";
            this.lkp_TipoIncorporacion.Size = new System.Drawing.Size(118, 26);
            this.lkp_TipoIncorporacion.TabIndex = 6;
            this.lkp_TipoIncorporacion.EditValueChanged += new System.EventHandler(this.lkp_TipoIncorporacion_EditValueChanged);
            // 
            // dtFechaVerificacion
            // 
            this.dtFechaVerificacion.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaVerificacion.Location = new System.Drawing.Point(207, 75);
            this.dtFechaVerificacion.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtFechaVerificacion.Name = "dtFechaVerificacion";
            this.dtFechaVerificacion.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaVerificacion.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaVerificacion.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaVerificacion.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaVerificacion.Properties.Appearance.Options.UseFont = true;
            this.dtFechaVerificacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaVerificacion.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaVerificacion.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaVerificacion.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaVerificacion.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaVerificacion.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaVerificacion.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaVerificacion.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaVerificacion.Size = new System.Drawing.Size(119, 28);
            this.dtFechaVerificacion.TabIndex = 9;
            // 
            // lblTipoIncorp
            // 
            this.lblTipoIncorp.AutoSize = true;
            this.lblTipoIncorp.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoIncorp.Location = new System.Drawing.Point(23, 15);
            this.lblTipoIncorp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTipoIncorp.Name = "lblTipoIncorp";
            this.lblTipoIncorp.Size = new System.Drawing.Size(174, 22);
            this.lblTipoIncorp.TabIndex = 5;
            this.lblTipoIncorp.Text = "175_TipoVerificacion";
            // 
            // masterCentroPago
            // 
            this.masterCentroPago.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroPago.Filtros = null;
            this.masterCentroPago.Location = new System.Drawing.Point(346, 12);
            this.masterCentroPago.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.masterCentroPago.Name = "masterCentroPago";
            this.masterCentroPago.Size = new System.Drawing.Size(452, 33);
            this.masterCentroPago.TabIndex = 7;
            this.masterCentroPago.Value = "";
            this.masterCentroPago.Leave += new System.EventHandler(this.masterCentroPago_Leave);
            // 
            // lblFechaVerificacion
            // 
            this.lblFechaVerificacion.AutoSize = true;
            this.lblFechaVerificacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaVerificacion.Location = new System.Drawing.Point(23, 78);
            this.lblFechaVerificacion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFechaVerificacion.Name = "lblFechaVerificacion";
            this.lblFechaVerificacion.Size = new System.Drawing.Size(184, 22);
            this.lblFechaVerificacion.TabIndex = 8;
            this.lblFechaVerificacion.Text = "175_FechaVerificacion";
            // 
            // Verificacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.ClientSize = new System.Drawing.Size(1401, 775);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Verificacion";
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupAnexos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlAnexos)).EndInit();
            this.PopupContainerControlAnexos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlTareas)).EndInit();
            this.PopupContainerControlTareas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RichTextTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RichTextAnexos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoIncorporacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVerificacion.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaVerificacion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lkp_TipoIncorporacion;
        protected DevExpress.XtraEditors.DateEdit dtFechaVerificacion;
        private System.Windows.Forms.Label lblTipoIncorp;
        private ControlsUC.uc_MasterFind masterCentroPago;
        private System.Windows.Forms.Label lblFechaVerificacion;



    }
}