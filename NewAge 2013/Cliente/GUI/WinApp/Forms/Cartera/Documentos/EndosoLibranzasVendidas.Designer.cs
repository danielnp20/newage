namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class EndosoLibranzasVendidas
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.grpboxHeader.SuspendLayout();
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
            this.editLink});
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
            this.cmbUserTareas.Location = new System.Drawing.Point(1847, 23);
            this.cmbUserTareas.Size = new System.Drawing.Size(238, 33);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserTareas.Location = new System.Drawing.Point(1602, 25);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.chkSeleccionar);
            this.grpboxHeader.Location = new System.Drawing.Point(2, 2);
            this.grpboxHeader.Size = new System.Drawing.Size(2104, 71);
            this.grpboxHeader.Visible = true;
            this.grpboxHeader.Controls.SetChildIndex(this.lblUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.cmbUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.chkSeleccionar, 0);
            // 
            // EnvioPreseleccionLibranzas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.ClientSize = new System.Drawing.Size(2202, 1117);
            this.Margin = new System.Windows.Forms.Padding(12);
            this.Name = "EnvioPreseleccionLibranzas";
            this.Text = "32565";
            ((System.ComponentModel.ISupportInitialize)(this.richText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

    }
}