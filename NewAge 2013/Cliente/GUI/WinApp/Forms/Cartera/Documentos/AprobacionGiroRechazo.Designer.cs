namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionGiroRechazo
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.txtVlrGiro = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrGiro = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrGiro.Properties)).BeginInit();
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
            this.richEditControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
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
            this.cmbUserTareas.Location = new System.Drawing.Point(350, 11);
            this.cmbUserTareas.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserTareas.Size = new System.Drawing.Size(113, 22);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserTareas.Location = new System.Drawing.Point(224, 15);
            this.lblUserTareas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.txtVlrGiro);
            this.grpboxHeader.Controls.Add(this.lblVlrGiro);
            this.grpboxHeader.Location = new System.Drawing.Point(2, 2);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(2);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(2);
            this.grpboxHeader.Size = new System.Drawing.Size(1040, 35);
            this.grpboxHeader.Visible = true;
            this.grpboxHeader.Controls.SetChildIndex(this.chkSeleccionar, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.cmbUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblVlrGiro, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.txtVlrGiro, 0);
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P";
            // 
            // gcDetails
            // 
            this.gcDetails.Size = new System.Drawing.Size(1044, 85);
            // 
            // txtVlrGiro
            // 
            this.txtVlrGiro.EditValue = "0";
            this.txtVlrGiro.Location = new System.Drawing.Point(267, 12);
            this.txtVlrGiro.Name = "txtVlrGiro";
            this.txtVlrGiro.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrGiro.Properties.Appearance.Options.UseFont = true;
            this.txtVlrGiro.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrGiro.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrGiro.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrGiro.Properties.Mask.EditMask = "c";
            this.txtVlrGiro.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrGiro.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrGiro.Properties.ReadOnly = true;
            this.txtVlrGiro.Size = new System.Drawing.Size(137, 20);
            this.txtVlrGiro.TabIndex = 4;
            // 
            // lblVlrGiro
            // 
            this.lblVlrGiro.AutoSize = true;
            this.lblVlrGiro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrGiro.Location = new System.Drawing.Point(180, 15);
            this.lblVlrGiro.Name = "lblVlrGiro";
            this.lblVlrGiro.Size = new System.Drawing.Size(84, 14);
            this.lblVlrGiro.TabIndex = 3;
            this.lblVlrGiro.Text = "32561_VlrGiro";
            // 
            // AprobacionGiroRechazo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 388);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AprobacionGiroRechazo";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrGiro.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtVlrGiro;
        private System.Windows.Forms.Label lblVlrGiro;

    }
}