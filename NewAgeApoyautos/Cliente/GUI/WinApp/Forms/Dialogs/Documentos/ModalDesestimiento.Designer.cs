namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalDesestimiento
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.pnlGeneral = new DevExpress.XtraEditors.GroupControl();
            this.pnlMaster = new System.Windows.Forms.Panel();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMaster = new System.Windows.Forms.Label();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.master1 = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editBtnMvto = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editBtnDetail = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlGeneral)).BeginInit();
            this.pnlGeneral.SuspendLayout();
            this.pnlMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editBtnMvto,
            this.editBtnDetail});
            // 
            // pnlGeneral
            // 
            this.pnlGeneral.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.pnlGeneral.AppearanceCaption.Options.UseFont = true;
            this.pnlGeneral.AppearanceCaption.Options.UseTextOptions = true;
            this.pnlGeneral.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pnlGeneral.Controls.Add(this.pnlMaster);
            this.pnlGeneral.Location = new System.Drawing.Point(12, 10);
            this.pnlGeneral.Name = "pnlGeneral";
            this.pnlGeneral.ShowCaption = false;
            this.pnlGeneral.Size = new System.Drawing.Size(367, 87);
            this.pnlGeneral.TabIndex = 7;
            this.pnlGeneral.Text = "51_gbReferencia";
            // 
            // pnlMaster
            // 
            this.pnlMaster.Controls.Add(this.txt2);
            this.pnlMaster.Controls.Add(this.label3);
            this.pnlMaster.Controls.Add(this.lblMaster);
            this.pnlMaster.Controls.Add(this.master1);
            this.pnlMaster.Location = new System.Drawing.Point(5, 4);
            this.pnlMaster.Name = "pnlMaster";
            this.pnlMaster.Size = new System.Drawing.Size(357, 78);
            this.pnlMaster.TabIndex = 0;
            this.pnlMaster.Visible = false;
            // 
            // txt2
            // 
            this.txt2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.txt2.Location = new System.Drawing.Point(140, 42);
            this.txt2.Multiline = true;
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(190, 21);
            this.txt2.TabIndex = 49;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label3.Location = new System.Drawing.Point(2, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 18);
            this.label3.TabIndex = 48;
            this.label3.Text = "Observacion";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMaster
            // 
            this.lblMaster.BackColor = System.Drawing.Color.Transparent;
            this.lblMaster.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblMaster.Location = new System.Drawing.Point(4, 16);
            this.lblMaster.Name = "lblMaster";
            this.lblMaster.Size = new System.Drawing.Size(137, 27);
            this.lblMaster.TabIndex = 47;
            this.lblMaster.Text = "campo";
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // btnAccept
            // 
            this.btnAccept.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnAccept.Appearance.Options.UseFont = true;
            this.btnAccept.Location = new System.Drawing.Point(200, 106);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(81, 23);
            this.btnAccept.TabIndex = 13;
            this.btnAccept.Text = "Aceptar";
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(285, 106);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // master1
            // 
            this.master1.BackColor = System.Drawing.Color.Transparent;
            this.master1.Filtros = null;
            this.master1.Location = new System.Drawing.Point(44, 13);
            this.master1.Name = "master1";
            this.master1.Size = new System.Drawing.Size(301, 23);
            this.master1.TabIndex = 1;
            this.master1.Value = "";
            this.master1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editBtnMvto
            // 
            this.editBtnMvto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver Movimiento", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnMvto.Name = "editBtnMvto";
            this.editBtnMvto.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnMvto.ValidateOnEnterKey = true;
            // 
            // editBtnDetail
            // 
            this.editBtnDetail.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver detalle", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "Ver detalle de la referencia", null, null, true)});
            this.editBtnDetail.Name = "editBtnDetail";
            this.editBtnDetail.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnDetail.ValidateOnEnterKey = true;
            // 
            // ModalDesestimiento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(403, 135);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.pnlGeneral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(887, 424);
            this.MinimizeBox = false;
            this.Name = "ModalDesestimiento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editar campo";
            ((System.ComponentModel.ISupportInitialize)(this.pnlGeneral)).EndInit();
            this.pnlGeneral.ResumeLayout(false);
            this.pnlMaster.ResumeLayout(false);
            this.pnlMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.GroupControl pnlGeneral;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnMvto;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnDetail;
        private ControlsUC.uc_MasterFind master1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btnAccept;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Panel pnlMaster;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMaster;
        private System.Windows.Forms.TextBox txt2;
    }
}