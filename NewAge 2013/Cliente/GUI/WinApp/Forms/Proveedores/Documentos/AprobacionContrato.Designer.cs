namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionContrato
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
            this.txtTotalMdaExt = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalMdaLocal = new DevExpress.XtraEditors.TextEdit();
            this.lblTotalMdaExt = new System.Windows.Forms.Label();
            this.lblTotalMonLoc = new System.Windows.Forms.Label();
            this.lblTitileDet = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.pnlUserTarea.SuspendLayout();
            this.pnlMedio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpDocumentos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalMdaExt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalMdaLocal.Properties)).BeginInit();
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
            // panel4
            // 
            this.pnlMedio.Controls.Add(this.lblTitileDet);
            this.pnlMedio.Controls.Add(this.txtTotalMdaExt);
            this.pnlMedio.Controls.Add(this.txtTotalMdaLocal);
            this.pnlMedio.Controls.Add(this.lblTotalMdaExt);
            this.pnlMedio.Controls.Add(this.lblTotalMonLoc);
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
            // 
            // txtTotalMdaExt
            // 
            this.txtTotalMdaExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalMdaExt.EditValue = "0,00 ";
            this.txtTotalMdaExt.Location = new System.Drawing.Point(466, 0);
            this.txtTotalMdaExt.Name = "txtTotalMdaExt";
            this.txtTotalMdaExt.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalMdaExt.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtTotalMdaExt.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalMdaExt.Properties.Appearance.Options.UseFont = true;
            this.txtTotalMdaExt.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalMdaExt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalMdaExt.Properties.AutoHeight = false;
            this.txtTotalMdaExt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalMdaExt.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalMdaExt.Properties.Mask.EditMask = "c";
            this.txtTotalMdaExt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalMdaExt.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalMdaExt.Properties.ReadOnly = true;
            this.txtTotalMdaExt.Size = new System.Drawing.Size(131, 19);
            this.txtTotalMdaExt.TabIndex = 16;
            // 
            // txtTotalMdaLocal
            // 
            this.txtTotalMdaLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalMdaLocal.EditValue = "0";
            this.txtTotalMdaLocal.Location = new System.Drawing.Point(740, 0);
            this.txtTotalMdaLocal.Name = "txtTotalMdaLocal";
            this.txtTotalMdaLocal.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalMdaLocal.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.txtTotalMdaLocal.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalMdaLocal.Properties.Appearance.Options.UseFont = true;
            this.txtTotalMdaLocal.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalMdaLocal.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotalMdaLocal.Properties.AutoHeight = false;
            this.txtTotalMdaLocal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalMdaLocal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalMdaLocal.Properties.Mask.EditMask = "c";
            this.txtTotalMdaLocal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalMdaLocal.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalMdaLocal.Properties.ReadOnly = true;
            this.txtTotalMdaLocal.Size = new System.Drawing.Size(131, 19);
            this.txtTotalMdaLocal.TabIndex = 15;
            // 
            // lblTotalMdaExt
            // 
            this.lblTotalMdaExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalMdaExt.AutoSize = true;
            this.lblTotalMdaExt.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalMdaExt.Location = new System.Drawing.Point(334, 3);
            this.lblTotalMdaExt.Name = "lblTotalMdaExt";
            this.lblTotalMdaExt.Size = new System.Drawing.Size(129, 14);
            this.lblTotalMdaExt.TabIndex = 14;
            this.lblTotalMdaExt.Text = "71557_lblTotalMonExt";
            // 
            // lblTotalMonLoc
            // 
            this.lblTotalMonLoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalMonLoc.AutoSize = true;
            this.lblTotalMonLoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalMonLoc.Location = new System.Drawing.Point(608, 4);
            this.lblTotalMonLoc.Name = "lblTotalMonLoc";
            this.lblTotalMonLoc.Size = new System.Drawing.Size(130, 14);
            this.lblTotalMonLoc.TabIndex = 13;
            this.lblTotalMonLoc.Text = "71557_lblTotalMonLoc";
            // 
            // lblTitileDet
            // 
            this.lblTitileDet.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTitileDet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitileDet.Location = new System.Drawing.Point(3, 7);
            this.lblTitileDet.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.lblTitileDet.Name = "lblTitileDet";
            this.lblTitileDet.Size = new System.Drawing.Size(120, 15);
            this.lblTitileDet.TabIndex = 17;
            this.lblTitileDet.Text = "71557_lblTitleDet";
            this.lblTitileDet.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // AprobacionContrato
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 567);
            this.Name = "AprobacionContrato";
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.pnlUserTarea.ResumeLayout(false);
            this.pnlUserTarea.PerformLayout();
            this.pnlMedio.ResumeLayout(false);
            this.pnlMedio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpDocumentos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalMdaExt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalMdaLocal.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label lblUserTareas;
        public DevExpress.XtraEditors.LookUpEdit lookUpDocumentos;
        private DevExpress.XtraEditors.TextEdit txtTotalMdaExt;
        private DevExpress.XtraEditors.TextEdit txtTotalMdaLocal;
        private System.Windows.Forms.Label lblTotalMdaExt;
        private System.Windows.Forms.Label lblTotalMonLoc;
        private System.Windows.Forms.Label lblTitileDet;


    }
}