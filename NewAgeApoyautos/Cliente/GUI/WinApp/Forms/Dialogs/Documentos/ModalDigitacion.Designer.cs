namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalDigitacion
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
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editBtnMvto = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editBtnDetail = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.pnlGeneral = new DevExpress.XtraEditors.GroupControl();
            this.pnlMaster = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMaster = new System.Windows.Forms.Label();
            this.master2 = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.master1 = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.pnlNumeric = new System.Windows.Forms.Panel();
            this.numeric2 = new DevExpress.XtraEditors.TextEdit();
            this.numeric1 = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.lblNumerico = new System.Windows.Forms.Label();
            this.pnlCombo = new System.Windows.Forms.Panel();
            this.cmb2 = new DevExpress.XtraEditors.LookUpEdit();
            this.cmb1 = new DevExpress.XtraEditors.LookUpEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCombo = new System.Windows.Forms.Label();
            this.pnlDate = new System.Windows.Forms.Panel();
            this.dt2 = new DevExpress.XtraEditors.DateEdit();
            this.dt1 = new DevExpress.XtraEditors.DateEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.lblFecha = new System.Windows.Forms.Label();
            this.pnlText = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txt2 = new System.Windows.Forms.TextBox();
            this.lblTexto = new System.Windows.Forms.Label();
            this.txt1 = new System.Windows.Forms.TextBox();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlGeneral)).BeginInit();
            this.pnlGeneral.SuspendLayout();
            this.pnlMaster.SuspendLayout();
            this.pnlNumeric.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric1.Properties)).BeginInit();
            this.pnlCombo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmb2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb1.Properties)).BeginInit();
            this.pnlDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt2.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt1.Properties)).BeginInit();
            this.pnlText.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editBtnMvto,
            this.editBtnDetail});
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
            // pnlGeneral
            // 
            this.pnlGeneral.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.pnlGeneral.AppearanceCaption.Options.UseFont = true;
            this.pnlGeneral.AppearanceCaption.Options.UseTextOptions = true;
            this.pnlGeneral.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pnlGeneral.Controls.Add(this.pnlMaster);
            this.pnlGeneral.Controls.Add(this.pnlNumeric);
            this.pnlGeneral.Controls.Add(this.pnlCombo);
            this.pnlGeneral.Controls.Add(this.pnlDate);
            this.pnlGeneral.Controls.Add(this.pnlText);
            this.pnlGeneral.Location = new System.Drawing.Point(12, 10);
            this.pnlGeneral.Name = "pnlGeneral";
            this.pnlGeneral.ShowCaption = false;
            this.pnlGeneral.Size = new System.Drawing.Size(348, 87);
            this.pnlGeneral.TabIndex = 7;
            this.pnlGeneral.Text = "51_gbReferencia";
            // 
            // pnlMaster
            // 
            this.pnlMaster.Controls.Add(this.label3);
            this.pnlMaster.Controls.Add(this.lblMaster);
            this.pnlMaster.Controls.Add(this.master2);
            this.pnlMaster.Controls.Add(this.master1);
            this.pnlMaster.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMaster.Location = new System.Drawing.Point(1338, 2);
            this.pnlMaster.Name = "pnlMaster";
            this.pnlMaster.Size = new System.Drawing.Size(515, 83);
            this.pnlMaster.TabIndex = 0;
            this.pnlMaster.Visible = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label3.Location = new System.Drawing.Point(2, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 18);
            this.label3.TabIndex = 48;
            this.label3.Text = "Confirmación\r\n";
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
            // master2
            // 
            this.master2.BackColor = System.Drawing.Color.Transparent;
            this.master2.Filtros = null;
            this.master2.Location = new System.Drawing.Point(43, 46);
            this.master2.Name = "master2";
            this.master2.Size = new System.Drawing.Size(301, 23);
            this.master2.TabIndex = 2;
            this.master2.Value = "";
            this.master2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
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
            // pnlNumeric
            // 
            this.pnlNumeric.Controls.Add(this.numeric2);
            this.pnlNumeric.Controls.Add(this.numeric1);
            this.pnlNumeric.Controls.Add(this.label9);
            this.pnlNumeric.Controls.Add(this.lblNumerico);
            this.pnlNumeric.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNumeric.Location = new System.Drawing.Point(1033, 2);
            this.pnlNumeric.Name = "pnlNumeric";
            this.pnlNumeric.Size = new System.Drawing.Size(305, 83);
            this.pnlNumeric.TabIndex = 4;
            this.pnlNumeric.Visible = false;
            // 
            // numeric2
            // 
            this.numeric2.EditValue = "0";
            this.numeric2.Location = new System.Drawing.Point(126, 47);
            this.numeric2.Name = "numeric2";
            this.numeric2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeric2.Properties.Appearance.Options.UseFont = true;
            this.numeric2.Properties.Appearance.Options.UseTextOptions = true;
            this.numeric2.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.numeric2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.numeric2.Properties.Mask.EditMask = "c0";
            this.numeric2.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.numeric2.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.numeric2.Size = new System.Drawing.Size(111, 20);
            this.numeric2.TabIndex = 2;
            // 
            // numeric1
            // 
            this.numeric1.EditValue = "0";
            this.numeric1.Location = new System.Drawing.Point(126, 14);
            this.numeric1.Name = "numeric1";
            this.numeric1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeric1.Properties.Appearance.Options.UseFont = true;
            this.numeric1.Properties.Appearance.Options.UseTextOptions = true;
            this.numeric1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.numeric1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.numeric1.Properties.Mask.EditMask = "c0";
            this.numeric1.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.numeric1.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.numeric1.Size = new System.Drawing.Size(111, 20);
            this.numeric1.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label9.Location = new System.Drawing.Point(11, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 18);
            this.label9.TabIndex = 50;
            this.label9.Text = "Confirmación\r\n";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNumerico
            // 
            this.lblNumerico.BackColor = System.Drawing.Color.Transparent;
            this.lblNumerico.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblNumerico.Location = new System.Drawing.Point(11, 16);
            this.lblNumerico.Name = "lblNumerico";
            this.lblNumerico.Size = new System.Drawing.Size(109, 27);
            this.lblNumerico.TabIndex = 48;
            this.lblNumerico.Text = "campo";
            // 
            // pnlCombo
            // 
            this.pnlCombo.Controls.Add(this.cmb2);
            this.pnlCombo.Controls.Add(this.cmb1);
            this.pnlCombo.Controls.Add(this.label7);
            this.pnlCombo.Controls.Add(this.lblCombo);
            this.pnlCombo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCombo.Location = new System.Drawing.Point(707, 2);
            this.pnlCombo.Name = "pnlCombo";
            this.pnlCombo.Size = new System.Drawing.Size(326, 83);
            this.pnlCombo.TabIndex = 3;
            this.pnlCombo.Visible = false;
            // 
            // cmb2
            // 
            this.cmb2.Location = new System.Drawing.Point(143, 47);
            this.cmb2.Name = "cmb2";
            this.cmb2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb2.Size = new System.Drawing.Size(125, 20);
            this.cmb2.TabIndex = 2;
            // 
            // cmb1
            // 
            this.cmb1.Location = new System.Drawing.Point(143, 14);
            this.cmb1.Name = "cmb1";
            this.cmb1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmb1.Properties.PasswordChar = '*';
            this.cmb1.Size = new System.Drawing.Size(125, 20);
            this.cmb1.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label7.Location = new System.Drawing.Point(8, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 18);
            this.label7.TabIndex = 50;
            this.label7.Text = "Confirmación\r\n";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCombo
            // 
            this.lblCombo.BackColor = System.Drawing.Color.Transparent;
            this.lblCombo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCombo.Location = new System.Drawing.Point(8, 16);
            this.lblCombo.Name = "lblCombo";
            this.lblCombo.Size = new System.Drawing.Size(125, 27);
            this.lblCombo.TabIndex = 48;
            this.lblCombo.Text = "campo";
            // 
            // pnlDate
            // 
            this.pnlDate.Controls.Add(this.dt2);
            this.pnlDate.Controls.Add(this.dt1);
            this.pnlDate.Controls.Add(this.label5);
            this.pnlDate.Controls.Add(this.lblFecha);
            this.pnlDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlDate.Location = new System.Drawing.Point(348, 2);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(359, 83);
            this.pnlDate.TabIndex = 2;
            this.pnlDate.Visible = false;
            // 
            // dt2
            // 
            this.dt2.EditValue = null;
            this.dt2.Location = new System.Drawing.Point(148, 47);
            this.dt2.Name = "dt2";
            this.dt2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt2.Size = new System.Drawing.Size(123, 20);
            this.dt2.TabIndex = 2;
            // 
            // dt1
            // 
            this.dt1.EditValue = null;
            this.dt1.Location = new System.Drawing.Point(148, 14);
            this.dt1.Name = "dt1";
            this.dt1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dt1.Properties.PasswordChar = '*';
            this.dt1.Size = new System.Drawing.Size(123, 20);
            this.dt1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label5.Location = new System.Drawing.Point(10, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 18);
            this.label5.TabIndex = 50;
            this.label5.Text = "Confirmación\r\n";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFecha
            // 
            this.lblFecha.BackColor = System.Drawing.Color.Transparent;
            this.lblFecha.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblFecha.Location = new System.Drawing.Point(10, 16);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(132, 27);
            this.lblFecha.TabIndex = 48;
            this.lblFecha.Text = "campo";
            // 
            // pnlText
            // 
            this.pnlText.Controls.Add(this.label4);
            this.pnlText.Controls.Add(this.txt2);
            this.pnlText.Controls.Add(this.lblTexto);
            this.pnlText.Controls.Add(this.txt1);
            this.pnlText.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlText.Location = new System.Drawing.Point(2, 2);
            this.pnlText.Name = "pnlText";
            this.pnlText.Size = new System.Drawing.Size(346, 83);
            this.pnlText.TabIndex = 1;
            this.pnlText.Visible = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label4.Location = new System.Drawing.Point(3, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 18);
            this.label4.TabIndex = 50;
            this.label4.Text = "Confirmación\r\n";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txt2
            // 
            this.txt2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.txt2.Location = new System.Drawing.Point(116, 47);
            this.txt2.Multiline = true;
            this.txt2.Name = "txt2";
            this.txt2.Size = new System.Drawing.Size(190, 21);
            this.txt2.TabIndex = 2;
            // 
            // lblTexto
            // 
            this.lblTexto.BackColor = System.Drawing.Color.Transparent;
            this.lblTexto.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTexto.Location = new System.Drawing.Point(3, 15);
            this.lblTexto.Name = "lblTexto";
            this.lblTexto.Size = new System.Drawing.Size(107, 28);
            this.lblTexto.TabIndex = 48;
            this.lblTexto.Text = "campo";
            // 
            // txt1
            // 
            this.txt1.Location = new System.Drawing.Point(116, 14);
            this.txt1.Name = "txt1";
            this.txt1.PasswordChar = '*';
            this.txt1.Size = new System.Drawing.Size(190, 21);
            this.txt1.TabIndex = 1;
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // btnAccept
            // 
            this.btnAccept.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnAccept.Appearance.Options.UseFont = true;
            this.btnAccept.Location = new System.Drawing.Point(200, 103);
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
            this.btnCancel.Location = new System.Drawing.Point(285, 103);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ModalDigitacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(369, 135);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.pnlGeneral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(887, 424);
            this.MinimizeBox = false;
            this.Name = "ModalDigitacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editar campo";
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlGeneral)).EndInit();
            this.pnlGeneral.ResumeLayout(false);
            this.pnlMaster.ResumeLayout(false);
            this.pnlNumeric.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numeric2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric1.Properties)).EndInit();
            this.pnlCombo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmb2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb1.Properties)).EndInit();
            this.pnlDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dt2.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt1.Properties)).EndInit();
            this.pnlText.ResumeLayout(false);
            this.pnlText.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.GroupControl pnlGeneral;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnMvto;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnDetail;
        private ControlsUC.uc_MasterFind master1;
        private ControlsUC.uc_MasterFind master2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btnAccept;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.Panel pnlMaster;
        private System.Windows.Forms.Panel pnlText;
        private System.Windows.Forms.TextBox txt1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMaster;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt2;
        private System.Windows.Forms.Label lblTexto;
        private System.Windows.Forms.Panel pnlDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblFecha;
        private DevExpress.XtraEditors.DateEdit dt2;
        private DevExpress.XtraEditors.DateEdit dt1;
        private System.Windows.Forms.Panel pnlCombo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCombo;
        private DevExpress.XtraEditors.LookUpEdit cmb2;
        private DevExpress.XtraEditors.LookUpEdit cmb1;
        private System.Windows.Forms.Panel pnlNumeric;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblNumerico;
        private DevExpress.XtraEditors.TextEdit numeric2;
        private DevExpress.XtraEditors.TextEdit numeric1;
    }
}