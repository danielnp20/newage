namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionSolicitud
    {

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lblUserTareas = new System.Windows.Forms.Label();
            this.lookUpDocumentos = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.pnlUserTarea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpDocumentos.Properties)).BeginInit();
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
            // pnlUserTarea
            // 
            this.pnlUserTarea.Controls.Add(this.lookUpDocumentos);
            this.pnlUserTarea.Controls.Add(this.lblUserTareas);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.AutoSize = true;
            this.lblUserTareas.Location = new System.Drawing.Point(23, 7);
            this.lblUserTareas.Name = "lblUserTareas";
            this.lblUserTareas.Size = new System.Drawing.Size(108, 13);
            this.lblUserTareas.TabIndex = 0;
            this.lblUserTareas.Text = "71552_lblUserTareas";
            // 
            // lookUpDocumentos
            // 
            this.lookUpDocumentos.Location = new System.Drawing.Point(132, 3);
            this.lookUpDocumentos.Name = "lookUpDocumentos";
            this.lookUpDocumentos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpDocumentos.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lookUpDocumentos.Properties.DisplayMember = "Value";
            this.lookUpDocumentos.Properties.ValueMember = "key";
            this.lookUpDocumentos.Size = new System.Drawing.Size(225, 20);
            this.lookUpDocumentos.TabIndex = 98;
            this.lookUpDocumentos.EditValueChanged += new System.EventHandler(this.lookUpDocumentos_EditValueChanged);
            // 
            // AprobacionSolicitud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 567);
            this.Name = "AprobacionSolicitud";
            ((System.ComponentModel.ISupportInitialize)(this.richText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.pnlUserTarea.ResumeLayout(false);
            this.pnlUserTarea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpDocumentos.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label lblUserTareas;
        public DevExpress.XtraEditors.LookUpEdit lookUpDocumentos;


    }
}