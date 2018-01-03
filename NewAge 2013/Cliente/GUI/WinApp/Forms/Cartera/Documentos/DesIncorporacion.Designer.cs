namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DesIncorporacion
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.masterCentroPago = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblDate = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
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
            this.grpboxHeader.Controls.Add(this.dtFecha);
            this.grpboxHeader.Controls.Add(this.lblDate);
            this.grpboxHeader.Controls.Add(this.txtLibranza);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.masterCentroPago);
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(828, 23);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(121, 36);
            this.txtLibranza.TabIndex = 47;
            this.txtLibranza.Leave += new System.EventHandler(this.txtLibranza_Leave);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(640, 26);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(154, 29);
            this.lblLibranza.TabIndex = 46;
            this.lblLibranza.Text = "171_Libranza";
            // 
            // masterCentroPago
            // 
            this.masterCentroPago.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroPago.Filtros = null;
            this.masterCentroPago.Location = new System.Drawing.Point(15, 19);
            this.masterCentroPago.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.masterCentroPago.Name = "masterCentroPago";
            this.masterCentroPago.Size = new System.Drawing.Size(590, 44);
            this.masterCentroPago.TabIndex = 43;
            this.masterCentroPago.Value = "";
            this.masterCentroPago.Leave += new System.EventHandler(this.masterCentroPago_Leave);
            // 
            // lblDate
            // 
            this.lblDate.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblDate.Location = new System.Drawing.Point(15, 77);
            this.lblDate.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(254, 29);
            this.lblDate.TabIndex = 48;
            this.lblDate.Text = "171_FechaDesincorpora";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(243, 74);
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
            this.dtFecha.Size = new System.Drawing.Size(175, 36);
            this.dtFecha.TabIndex = 49;
            // 
            // DesIncorporacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.ClientSize = new System.Drawing.Size(1868, 969);
            this.Name = "DesIncorporacion";
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
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtLibranza;
        private System.Windows.Forms.Label lblLibranza;
        private ControlsUC.uc_MasterFind masterCentroPago;
        private DevExpress.XtraEditors.DateEdit dtFecha;
        private DevExpress.XtraEditors.LabelControl lblDate;


    }
}