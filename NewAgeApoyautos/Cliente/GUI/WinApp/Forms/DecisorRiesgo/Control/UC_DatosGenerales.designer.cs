namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class UC_DatosGenerales
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
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editCheckBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.groupProy = new DevExpress.XtraEditors.GroupControl();
            this.label40 = new System.Windows.Forms.Label();
            this.masterZona = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbServicioGeneral = new DevExpress.XtraEditors.LookUpEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.masterCiudadGeneral = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtCedulaDeudor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDeudor = new System.Windows.Forms.TextBox();
            this.lblDeudor = new System.Windows.Forms.Label();
            this.masterVitrina = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.editCheckBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).BeginInit();
            this.groupProy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbServicioGeneral.Properties)).BeginInit();
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
            this.groupProy.Controls.Add(this.label1);
            this.groupProy.Controls.Add(this.label3);
            this.groupProy.Controls.Add(this.label40);
            this.groupProy.Controls.Add(this.masterZona);
            this.groupProy.Controls.Add(this.cmbServicioGeneral);
            this.groupProy.Controls.Add(this.label8);
            this.groupProy.Controls.Add(this.label4);
            this.groupProy.Controls.Add(this.masterCiudadGeneral);
            this.groupProy.Controls.Add(this.txtCedulaDeudor);
            this.groupProy.Controls.Add(this.label2);
            this.groupProy.Controls.Add(this.txtDeudor);
            this.groupProy.Controls.Add(this.lblDeudor);
            this.groupProy.Controls.Add(this.masterVitrina);
            this.groupProy.Location = new System.Drawing.Point(0, 0);
            this.groupProy.LookAndFeel.SkinName = "Dark Side";
            this.groupProy.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupProy.Margin = new System.Windows.Forms.Padding(2);
            this.groupProy.Name = "groupProy";
            this.groupProy.Size = new System.Drawing.Size(1023, 82);
            this.groupProy.TabIndex = 113;
            this.groupProy.Text = "Seleccionar Proyecto";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.White;
            this.label40.Location = new System.Drawing.Point(703, 56);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(34, 14);
            this.label40.TabIndex = 91;
            this.label40.Text = "Zona";
            // 
            // masterZona
            // 
            this.masterZona.BackColor = System.Drawing.Color.Transparent;
            this.masterZona.Filtros = null;
            this.masterZona.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterZona.ForeColor = System.Drawing.Color.White;
            this.masterZona.Location = new System.Drawing.Point(669, 50);
            this.masterZona.Margin = new System.Windows.Forms.Padding(7);
            this.masterZona.Name = "masterZona";
            this.masterZona.Size = new System.Drawing.Size(343, 26);
            this.masterZona.TabIndex = 90;
            this.masterZona.Value = "";
            // 
            // cmbServicioGeneral
            // 
            this.cmbServicioGeneral.Location = new System.Drawing.Point(421, 55);
            this.cmbServicioGeneral.Name = "cmbServicioGeneral";
            this.cmbServicioGeneral.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbServicioGeneral.Size = new System.Drawing.Size(97, 20);
            this.cmbServicioGeneral.TabIndex = 88;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(301, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 14);
            this.label8.TabIndex = 87;
            this.label8.Text = "Servicio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(338, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 14);
            this.label4.TabIndex = 86;
            this.label4.Text = "Ciudad";
            // 
            // masterCiudadGeneral
            // 
            this.masterCiudadGeneral.BackColor = System.Drawing.Color.Transparent;
            this.masterCiudadGeneral.Filtros = null;
            this.masterCiudadGeneral.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCiudadGeneral.ForeColor = System.Drawing.Color.White;
            this.masterCiudadGeneral.Location = new System.Drawing.Point(304, 28);
            this.masterCiudadGeneral.Margin = new System.Windows.Forms.Padding(7);
            this.masterCiudadGeneral.Name = "masterCiudadGeneral";
            this.masterCiudadGeneral.Size = new System.Drawing.Size(343, 26);
            this.masterCiudadGeneral.TabIndex = 85;
            this.masterCiudadGeneral.Value = "";
            // 
            // txtCedulaDeudor
            // 
            this.txtCedulaDeudor.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCedulaDeudor.Location = new System.Drawing.Point(72, 55);
            this.txtCedulaDeudor.Margin = new System.Windows.Forms.Padding(1);
            this.txtCedulaDeudor.Name = "txtCedulaDeudor";
            this.txtCedulaDeudor.Size = new System.Drawing.Size(138, 22);
            this.txtCedulaDeudor.TabIndex = 84;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(-177, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 83;
            this.label2.Text = "Cedula";
            // 
            // txtDeudor
            // 
            this.txtDeudor.Font = new System.Drawing.Font("Tahoma", 9.047121F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeudor.Location = new System.Drawing.Point(72, 32);
            this.txtDeudor.Margin = new System.Windows.Forms.Padding(1);
            this.txtDeudor.Name = "txtDeudor";
            this.txtDeudor.Size = new System.Drawing.Size(224, 22);
            this.txtDeudor.TabIndex = 82;
            // 
            // lblDeudor
            // 
            this.lblDeudor.AutoSize = true;
            this.lblDeudor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeudor.ForeColor = System.Drawing.Color.White;
            this.lblDeudor.Location = new System.Drawing.Point(-177, 30);
            this.lblDeudor.Name = "lblDeudor";
            this.lblDeudor.Size = new System.Drawing.Size(47, 14);
            this.lblDeudor.TabIndex = 81;
            this.lblDeudor.Text = "Deudor";
            // 
            // masterVitrina
            // 
            this.masterVitrina.BackColor = System.Drawing.Color.Transparent;
            this.masterVitrina.Filtros = null;
            this.masterVitrina.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterVitrina.ForeColor = System.Drawing.Color.White;
            this.masterVitrina.Location = new System.Drawing.Point(670, 25);
            this.masterVitrina.Margin = new System.Windows.Forms.Padding(7);
            this.masterVitrina.Name = "masterVitrina";
            this.masterVitrina.Size = new System.Drawing.Size(343, 26);
            this.masterVitrina.TabIndex = 80;
            this.masterVitrina.Value = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 93;
            this.label1.Text = "Cedula";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 14);
            this.label3.TabIndex = 92;
            this.label3.Text = "Deudor";
            // 
            // UC_DatosGenerales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupProy);
            this.Name = "UC_DatosGenerales";
            this.Size = new System.Drawing.Size(1037, 81);
            ((System.ComponentModel.ISupportInitialize)(this.editCheckBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupProy)).EndInit();
            this.groupProy.ResumeLayout(false);
            this.groupProy.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbServicioGeneral.Properties)).EndInit();
            this.ResumeLayout(false);

        }

       

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheckBox;
        private DevExpress.XtraEditors.GroupControl groupProy;
        private System.Windows.Forms.Label label40;
        private ControlsUC.uc_MasterFind masterZona;
        private DevExpress.XtraEditors.LookUpEdit cmbServicioGeneral;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private ControlsUC.uc_MasterFind masterCiudadGeneral;
        private System.Windows.Forms.TextBox txtCedulaDeudor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDeudor;
        private System.Windows.Forms.Label lblDeudor;
        private ControlsUC.uc_MasterFind masterVitrina;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}
