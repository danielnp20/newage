namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class UC_Proyecto
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Proyecto));
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editCheckBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.groupProy = new DevExpress.XtraEditors.GroupControl();
            this.lblNro = new DevExpress.XtraEditors.LabelControl();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblLicitacion = new DevExpress.XtraEditors.LabelControl();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.txtNro = new DevExpress.XtraEditors.TextEdit();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.txtLicitacion = new DevExpress.XtraEditors.MemoEdit();
            this.txtDescripcion = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheckBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).BeginInit();
            this.groupProy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editCheckBox});
            // 
            // editCheckBox
            // 
            this.editCheckBox.Caption = "";
            this.editCheckBox.DisplayValueChecked = "True";
            this.editCheckBox.DisplayValueUnchecked = "False";
            this.editCheckBox.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editCheckBox.Name = "editCheckBox";
            this.editCheckBox.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // groupProy
            // 
            this.groupProy.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold);
            this.groupProy.AppearanceCaption.Options.UseFont = true;
            this.groupProy.Controls.Add(this.lblNro);
            this.groupProy.Controls.Add(this.lblLicitacion);
            this.groupProy.Controls.Add(this.btnQueryDoc);
            this.groupProy.Controls.Add(this.txtNro);
            this.groupProy.Controls.Add(this.lblDescripcion);
            this.groupProy.Controls.Add(this.txtLicitacion);
            this.groupProy.Controls.Add(this.txtDescripcion);
            this.groupProy.Controls.Add(this.masterPrefijo);
            this.groupProy.Controls.Add(this.masterProyecto);
            this.groupProy.Controls.Add(this.masterCliente);
            this.groupProy.Location = new System.Drawing.Point(0, 0);
            this.groupProy.Margin = new System.Windows.Forms.Padding(2);
            this.groupProy.Name = "groupProy";
            this.groupProy.Size = new System.Drawing.Size(718, 93);
            this.groupProy.TabIndex = 113;
            this.groupProy.Text = "Seleccionar Proyecto";
            // 
            // lblNro
            // 
            this.lblNro.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNro.Location = new System.Drawing.Point(617, 27);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(51, 13);
            this.lblNro.TabIndex = 46;
            this.lblNro.Text = "110_lblNro";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(9, 67);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(307, 21);
            this.masterCliente.TabIndex = 113;
            this.masterCliente.Value = "";
            // 
            // lblLicitacion
            // 
            this.lblLicitacion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblLicitacion.Location = new System.Drawing.Point(9, 48);
            this.lblLicitacion.Name = "lblLicitacion";
            this.lblLicitacion.Size = new System.Drawing.Size(77, 13);
            this.lblLicitacion.TabIndex = 112;
            this.lblLicitacion.Text = "110_lblLicitacion";
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(10, 19);
            this.masterProyecto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(306, 21);
            this.masterProyecto.TabIndex = 110;
            this.masterProyecto.Value = "";
            this.masterProyecto.Leave += new System.EventHandler(this.masterProyecto_Leave);
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(320, 20);
            this.masterPrefijo.Margin = new System.Windows.Forms.Padding(4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(307, 21);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = ((System.Drawing.Image)(resources.GetObject("btnQueryDoc.Image")));
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(688, 23);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(19, 20);
            this.btnQueryDoc.TabIndex = 3;
            this.btnQueryDoc.Text = "btnQueryDoc";
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // txtNro
            // 
            this.txtNro.Location = new System.Drawing.Point(656, 23);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(30, 20);
            this.txtNro.TabIndex = 2;
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblDescripcion.Location = new System.Drawing.Point(319, 46);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(88, 13);
            this.lblDescripcion.TabIndex = 53;
            this.lblDescripcion.Text = "110_lblDescripcion";
            // 
            // txtLicitacion
            // 
            this.txtLicitacion.Location = new System.Drawing.Point(110, 44);
            this.txtLicitacion.Name = "txtLicitacion";
            this.txtLicitacion.Properties.ReadOnly = true;
            this.txtLicitacion.Size = new System.Drawing.Size(195, 22);
            this.txtLicitacion.TabIndex = 111;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(420, 45);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Properties.ReadOnly = true;
            this.txtDescripcion.Size = new System.Drawing.Size(288, 43);
            this.txtDescripcion.TabIndex = 16;
            // 
            // UC_Proyecto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupProy);
            this.Name = "UC_Proyecto";
            this.Size = new System.Drawing.Size(721, 104);
            ((System.ComponentModel.ISupportInitialize)(this.editCheckBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).EndInit();
            this.groupProy.ResumeLayout(false);
            this.groupProy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicitacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

       

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheckBox;
        private DevExpress.XtraEditors.GroupControl groupProy;
        private ControlsUC.uc_MasterFind masterCliente;
        private DevExpress.XtraEditors.LabelControl lblLicitacion;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.LabelControl lblNro;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private DevExpress.XtraEditors.MemoEdit txtLicitacion;
        private DevExpress.XtraEditors.MemoEdit txtDescripcion;
        private ControlsUC.uc_MasterFind masterProyecto;
        private DevExpress.XtraEditors.TextEdit txtNro;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
    }
}
