namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Incorporacion
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            this.comboTipoIncorp = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.lblTipoIncorp = new System.Windows.Forms.Label();
            this.dtMesIncorpora = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblMesIncorpora = new System.Windows.Forms.Label();
            this.dtFechaIncorpora = new DevExpress.XtraEditors.DateEdit();
            this.masterCentroPago = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblFechaIncorpora = new System.Windows.Forms.Label();
            this.chkGetPendIncorp = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupAnexos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlAnexos)).BeginInit();
            this.PopupContainerControlAnexos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlTareas)).BeginInit();
            this.PopupContainerControlTareas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RichTextTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RichTextAnexos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmbDict)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIncorpora.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIncorpora.Properties)).BeginInit();
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
            this.editSpin,
            this.editCmb,
            this.editBtnGrid,
            this.editCmbDict});
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            // 
            // masterActividad
            // 
            this.masterActividad.TabIndex = 1;
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.Size = new System.Drawing.Size(1027, 24);
            this.chkSeleccionar.TabIndex = 0;
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
            this.grpctrlHeader.Size = new System.Drawing.Size(1027, 38);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.chkGetPendIncorp);
            this.grpboxHeader.Controls.Add(this.comboTipoIncorp);
            this.grpboxHeader.Controls.Add(this.lblTipoIncorp);
            this.grpboxHeader.Controls.Add(this.dtMesIncorpora);
            this.grpboxHeader.Controls.Add(this.lblMesIncorpora);
            this.grpboxHeader.Controls.Add(this.dtFechaIncorpora);
            this.grpboxHeader.Controls.Add(this.masterCentroPago);
            this.grpboxHeader.Controls.Add(this.lblFechaIncorpora);
            this.grpboxHeader.Size = new System.Drawing.Size(1023, 34);
            // 
            // comboTipoIncorp
            // 
            this.comboTipoIncorp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTipoIncorp.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboTipoIncorp.FormattingEnabled = true;
            this.comboTipoIncorp.Location = new System.Drawing.Point(126, 10);
            this.comboTipoIncorp.Name = "comboTipoIncorp";
            this.comboTipoIncorp.Size = new System.Drawing.Size(118, 22);
            this.comboTipoIncorp.TabIndex = 0;
            // 
            // lblTipoIncorp
            // 
            this.lblTipoIncorp.AutoSize = true;
            this.lblTipoIncorp.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoIncorp.Location = new System.Drawing.Point(21, 14);
            this.lblTipoIncorp.Name = "lblTipoIncorp";
            this.lblTipoIncorp.Size = new System.Drawing.Size(94, 14);
            this.lblTipoIncorp.TabIndex = 27;
            this.lblTipoIncorp.Text = "163_TipoIncorp";
            // 
            // dtMesIncorpora
            // 
            this.dtMesIncorpora.BackColor = System.Drawing.Color.Transparent;
            this.dtMesIncorpora.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtMesIncorpora.EnabledControl = true;
            this.dtMesIncorpora.ExtraPeriods = 0;
            this.dtMesIncorpora.Location = new System.Drawing.Point(679, 11);
            this.dtMesIncorpora.Margin = new System.Windows.Forms.Padding(6);
            this.dtMesIncorpora.MaxValue = new System.DateTime(((long)(0)));
            this.dtMesIncorpora.MinValue = new System.DateTime(((long)(0)));
            this.dtMesIncorpora.Name = "dtMesIncorpora";
            this.dtMesIncorpora.Size = new System.Drawing.Size(105, 20);
            this.dtMesIncorpora.TabIndex = 2;
            // 
            // lblMesIncorpora
            // 
            this.lblMesIncorpora.AutoSize = true;
            this.lblMesIncorpora.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMesIncorpora.Location = new System.Drawing.Point(563, 14);
            this.lblMesIncorpora.Name = "lblMesIncorpora";
            this.lblMesIncorpora.Size = new System.Drawing.Size(120, 14);
            this.lblMesIncorpora.TabIndex = 34;
            this.lblMesIncorpora.Text = "163_dtMesIncorpora";
            // 
            // dtFechaIncorpora
            // 
            this.dtFechaIncorpora.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaIncorpora.Location = new System.Drawing.Point(437, 11);
            this.dtFechaIncorpora.Name = "dtFechaIncorpora";
            this.dtFechaIncorpora.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaIncorpora.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaIncorpora.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaIncorpora.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaIncorpora.Properties.Appearance.Options.UseFont = true;
            this.dtFechaIncorpora.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaIncorpora.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaIncorpora.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIncorpora.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIncorpora.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaIncorpora.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaIncorpora.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaIncorpora.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaIncorpora.Size = new System.Drawing.Size(90, 20);
            this.dtFechaIncorpora.TabIndex = 1;
            // 
            // masterCentroPago
            // 
            this.masterCentroPago.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroPago.Filtros = null;
            this.masterCentroPago.Location = new System.Drawing.Point(24, 34);
            this.masterCentroPago.Margin = new System.Windows.Forms.Padding(6);
            this.masterCentroPago.Name = "masterCentroPago";
            this.masterCentroPago.Size = new System.Drawing.Size(301, 26);
            this.masterCentroPago.TabIndex = 3;
            this.masterCentroPago.Value = "";
            this.masterCentroPago.Leave += new System.EventHandler(this.masterCentroPago_Leave);
            // 
            // lblFechaIncorpora
            // 
            this.lblFechaIncorpora.AutoSize = true;
            this.lblFechaIncorpora.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaIncorpora.Location = new System.Drawing.Point(320, 14);
            this.lblFechaIncorpora.Name = "lblFechaIncorpora";
            this.lblFechaIncorpora.Size = new System.Drawing.Size(121, 14);
            this.lblFechaIncorpora.TabIndex = 32;
            this.lblFechaIncorpora.Text = "163_dtIncorporacion";
            // 
            // chkGetPendIncorp
            // 
            this.chkGetPendIncorp.AutoSize = true;
            this.chkGetPendIncorp.Location = new System.Drawing.Point(822, 14);
            this.chkGetPendIncorp.Name = "chkGetPendIncorp";
            this.chkGetPendIncorp.Size = new System.Drawing.Size(138, 17);
            this.chkGetPendIncorp.TabIndex = 35;
            this.chkGetPendIncorp.Text = "163_chkGetPendIncorp";
            this.chkGetPendIncorp.UseVisualStyleBackColor = true;
            this.chkGetPendIncorp.CheckedChanged += new System.EventHandler(this.chkGetPendIncorp_CheckedChanged);
            // 
            // Incorporacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1089, 686);
            this.Name = "Incorporacion";
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopupAnexos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlAnexos)).EndInit();
            this.PopupContainerControlAnexos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControlTareas)).EndInit();
            this.PopupContainerControlTareas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RichText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RichTextTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RichTextAnexos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmbDict)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIncorpora.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaIncorpora.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Clases.ComboBoxEx comboTipoIncorp;
        private System.Windows.Forms.Label lblTipoIncorp;
        private ControlsUC.uc_PeriodoEdit dtMesIncorpora;
        private System.Windows.Forms.Label lblMesIncorpora;
        protected DevExpress.XtraEditors.DateEdit dtFechaIncorpora;
        private ControlsUC.uc_MasterFind masterCentroPago;
        private System.Windows.Forms.Label lblFechaIncorpora;
        private System.Windows.Forms.CheckBox chkGetPendIncorp;

    }
}