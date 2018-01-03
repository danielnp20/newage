namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class AprobacionGiro
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.masterBancos = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtVlrGiro = new DevExpress.XtraEditors.TextEdit();
            this.lblVlrGiro = new System.Windows.Forms.Label();
            this.lblPlazo = new System.Windows.Forms.Label();
            this.lkp_TipoPago = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrGiro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoPago.Properties)).BeginInit();
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
            this.cmbUserTareas.Location = new System.Drawing.Point(525, 17);
            this.cmbUserTareas.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.cmbUserTareas.Size = new System.Drawing.Size(168, 28);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserTareas.Location = new System.Drawing.Point(336, 23);
            this.lblUserTareas.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lkp_TipoPago);
            this.grpboxHeader.Controls.Add(this.lblPlazo);
            this.grpboxHeader.Controls.Add(this.txtVlrGiro);
            this.grpboxHeader.Controls.Add(this.lblVlrGiro);
            this.grpboxHeader.Controls.Add(this.masterBancos);
            this.grpboxHeader.Location = new System.Drawing.Point(2, 2);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.grpboxHeader.Size = new System.Drawing.Size(1287, 55);
            this.grpboxHeader.Visible = true;
            this.grpboxHeader.Controls.SetChildIndex(this.chkSeleccionar, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.cmbUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.masterBancos, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblVlrGiro, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.txtVlrGiro, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblPlazo, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lkp_TipoPago, 0);
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P";
            // 
            // gcDocuments
            //
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "DetallePagosBeneficiarios";
            this.gcDocuments.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            // 
            // masterBancos
            // 
            this.masterBancos.BackColor = System.Drawing.Color.Transparent;
            this.masterBancos.Filtros = null;
            this.masterBancos.Location = new System.Drawing.Point(516, 15);
            this.masterBancos.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.masterBancos.Name = "masterBancos";
            this.masterBancos.Size = new System.Drawing.Size(436, 40);
            this.masterBancos.TabIndex = 2;
            this.masterBancos.Value = "";
            this.masterBancos.Leave += new System.EventHandler(this.masterBancos_Leave);
            // 
            // txtVlrGiro
            // 
            this.txtVlrGiro.EditValue = "0";
            this.txtVlrGiro.Location = new System.Drawing.Point(1092, 18);
            this.txtVlrGiro.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
            this.txtVlrGiro.Size = new System.Drawing.Size(205, 28);
            this.txtVlrGiro.TabIndex = 4;
            // 
            // lblVlrGiro
            // 
            this.lblVlrGiro.AutoSize = true;
            this.lblVlrGiro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVlrGiro.Location = new System.Drawing.Point(962, 23);
            this.lblVlrGiro.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVlrGiro.Name = "lblVlrGiro";
            this.lblVlrGiro.Size = new System.Drawing.Size(123, 22);
            this.lblVlrGiro.TabIndex = 3;
            this.lblVlrGiro.Text = "32561_VlrGiro";
            // 
            // lblPlazo
            // 
            this.lblPlazo.AutoSize = true;
            this.lblPlazo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlazo.Location = new System.Drawing.Point(198, 22);
            this.lblPlazo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlazo.Name = "lblPlazo";
            this.lblPlazo.Size = new System.Drawing.Size(144, 22);
            this.lblPlazo.TabIndex = 0;
            this.lblPlazo.Text = "32561_TipoPago";
            // 
            // lkp_TipoPago
            // 
            this.lkp_TipoPago.Location = new System.Drawing.Point(352, 18);
            this.lkp_TipoPago.Name = "lkp_TipoPago";
            this.lkp_TipoPago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_TipoPago.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkp_TipoPago.Properties.DisplayMember = "Value";
            this.lkp_TipoPago.Properties.NullText = " ";
            this.lkp_TipoPago.Properties.ValueMember = "Key";
            this.lkp_TipoPago.Size = new System.Drawing.Size(152, 26);
            this.lkp_TipoPago.TabIndex = 1;
            this.lkp_TipoPago.EditValueChanged += new System.EventHandler(this.lkp_TipoPago_EditValueChanged);
            // 
            // AprobacionGiro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 600);
            this.Margin = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.Name = "AprobacionGiro";
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
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrGiro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoPago.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterBancos;
        private DevExpress.XtraEditors.TextEdit txtVlrGiro;
        private System.Windows.Forms.Label lblVlrGiro;
        private System.Windows.Forms.Label lblPlazo;
        private DevExpress.XtraEditors.LookUpEdit lkp_TipoPago;

    }
}