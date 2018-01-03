namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DigitacionCuentas
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
            this.masterConceptoCargo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterProyecto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroCosto = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLineaPre = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterLugarGeo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCuenta = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gbParameters = new System.Windows.Forms.GroupBox();
            this.glValores = new System.Windows.Forms.GroupBox();
            this.tlpValores = new System.Windows.Forms.TableLayoutPanel();
            this.tbValorME = new DevExpress.XtraEditors.TextEdit();
            this.tbBaseME = new DevExpress.XtraEditors.TextEdit();
            this.tbValorML = new DevExpress.XtraEditors.TextEdit();
            this.lblBaseML = new System.Windows.Forms.Label();
            this.lblValorML = new System.Windows.Forms.Label();
            this.lblBaseME = new System.Windows.Forms.Label();
            this.lblValorME = new System.Windows.Forms.Label();
            this.tbBaseML = new DevExpress.XtraEditors.TextEdit();
            this.btnAccept = new System.Windows.Forms.Button();
            this.gbParameters.SuspendLayout();
            this.glValores.SuspendLayout();
            this.tlpValores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbValorME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBaseME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbValorML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBaseML.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // masterConceptoCargo
            // 
            this.masterConceptoCargo.BackColor = System.Drawing.Color.Transparent;
            this.masterConceptoCargo.Filtros = null;
            this.masterConceptoCargo.Location = new System.Drawing.Point(29, 26);
            this.masterConceptoCargo.Name = "masterConceptoCargo";
            this.masterConceptoCargo.Size = new System.Drawing.Size(291, 25);
            this.masterConceptoCargo.TabIndex = 1;
            this.masterConceptoCargo.Value = "";
            this.masterConceptoCargo.Leave += new System.EventHandler(this.masterConceptoCargo_Leave);
            // 
            // masterProyecto
            // 
            this.masterProyecto.BackColor = System.Drawing.Color.Transparent;
            this.masterProyecto.Filtros = null;
            this.masterProyecto.Location = new System.Drawing.Point(29, 57);
            this.masterProyecto.Name = "masterProyecto";
            this.masterProyecto.Size = new System.Drawing.Size(291, 25);
            this.masterProyecto.TabIndex = 2;
            this.masterProyecto.Value = "";
            this.masterProyecto.Leave += new System.EventHandler(this.masterProyecto_Leave);
            // 
            // masterCentroCosto
            // 
            this.masterCentroCosto.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroCosto.Filtros = null;
            this.masterCentroCosto.Location = new System.Drawing.Point(29, 88);
            this.masterCentroCosto.Name = "masterCentroCosto";
            this.masterCentroCosto.Size = new System.Drawing.Size(291, 25);
            this.masterCentroCosto.TabIndex = 3;
            this.masterCentroCosto.Value = "";
            this.masterCentroCosto.Leave += new System.EventHandler(this.masterCentroCosto_Leave);
            // 
            // masterLineaPre
            // 
            this.masterLineaPre.BackColor = System.Drawing.Color.Transparent;
            this.masterLineaPre.Filtros = null;
            this.masterLineaPre.Location = new System.Drawing.Point(29, 119);
            this.masterLineaPre.Name = "masterLineaPre";
            this.masterLineaPre.Size = new System.Drawing.Size(291, 25);
            this.masterLineaPre.TabIndex = 4;
            this.masterLineaPre.Value = "";
            this.masterLineaPre.Leave += new System.EventHandler(this.masterLineaPre_Leave);
            // 
            // masterLugarGeo
            // 
            this.masterLugarGeo.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarGeo.Filtros = null;
            this.masterLugarGeo.Location = new System.Drawing.Point(29, 150);
            this.masterLugarGeo.Name = "masterLugarGeo";
            this.masterLugarGeo.Size = new System.Drawing.Size(291, 25);
            this.masterLugarGeo.TabIndex = 5;
            this.masterLugarGeo.Value = "";
            this.masterLugarGeo.Leave += new System.EventHandler(this.masterLugarGeo_Leave);
            // 
            // masterCuenta
            // 
            this.masterCuenta.BackColor = System.Drawing.Color.Transparent;
            this.masterCuenta.Filtros = null;
            this.masterCuenta.Location = new System.Drawing.Point(29, 181);
            this.masterCuenta.Name = "masterCuenta";
            this.masterCuenta.Size = new System.Drawing.Size(291, 25);
            this.masterCuenta.TabIndex = 6;
            this.masterCuenta.Value = "";
            // 
            // gbParameters
            // 
            this.gbParameters.Controls.Add(this.masterConceptoCargo);
            this.gbParameters.Controls.Add(this.masterCuenta);
            this.gbParameters.Controls.Add(this.masterProyecto);
            this.gbParameters.Controls.Add(this.masterLugarGeo);
            this.gbParameters.Controls.Add(this.masterCentroCosto);
            this.gbParameters.Controls.Add(this.masterLineaPre);
            this.gbParameters.Location = new System.Drawing.Point(21, 12);
            this.gbParameters.Name = "gbParameters";
            this.gbParameters.Size = new System.Drawing.Size(370, 222);
            this.gbParameters.TabIndex = 6;
            this.gbParameters.TabStop = false;
            this.gbParameters.Text = "1021_gbParameters";
            // 
            // glValores
            // 
            this.glValores.Controls.Add(this.tlpValores);
            this.glValores.Location = new System.Drawing.Point(21, 251);
            this.glValores.Name = "glValores";
            this.glValores.Size = new System.Drawing.Size(370, 133);
            this.glValores.TabIndex = 17;
            this.glValores.TabStop = false;
            this.glValores.Text = "1021_glValores";
            // 
            // tlpValores
            // 
            this.tlpValores.ColumnCount = 2;
            this.tlpValores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.23711F));
            this.tlpValores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.76289F));
            this.tlpValores.Controls.Add(this.tbValorME, 1, 3);
            this.tlpValores.Controls.Add(this.tbBaseME, 1, 2);
            this.tlpValores.Controls.Add(this.tbValorML, 1, 1);
            this.tlpValores.Controls.Add(this.lblBaseML, 0, 0);
            this.tlpValores.Controls.Add(this.lblValorML, 0, 1);
            this.tlpValores.Controls.Add(this.lblBaseME, 0, 2);
            this.tlpValores.Controls.Add(this.lblValorME, 0, 3);
            this.tlpValores.Controls.Add(this.tbBaseML, 1, 0);
            this.tlpValores.Location = new System.Drawing.Point(29, 29);
            this.tlpValores.Name = "tlpValores";
            this.tlpValores.RowCount = 4;
            this.tlpValores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpValores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpValores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpValores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpValores.Size = new System.Drawing.Size(291, 80);
            this.tlpValores.TabIndex = 10;
            // 
            // tbValorME
            // 
            this.tbValorME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbValorME.EditValue = "0";
            this.tbValorME.Location = new System.Drawing.Point(122, 63);
            this.tbValorME.Name = "tbValorME";
            this.tbValorME.Properties.Appearance.Options.UseTextOptions = true;
            this.tbValorME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.tbValorME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tbValorME.Properties.Mask.EditMask = "c";
            this.tbValorME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.tbValorME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.tbValorME.Size = new System.Drawing.Size(166, 20);
            this.tbValorME.TabIndex = 10;
            // 
            // tbBaseME
            // 
            this.tbBaseME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBaseME.EditValue = "0";
            this.tbBaseME.Location = new System.Drawing.Point(122, 43);
            this.tbBaseME.Name = "tbBaseME";
            this.tbBaseME.Properties.Appearance.Options.UseTextOptions = true;
            this.tbBaseME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.tbBaseME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tbBaseME.Properties.Mask.EditMask = "c";
            this.tbBaseME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.tbBaseME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.tbBaseME.Size = new System.Drawing.Size(166, 20);
            this.tbBaseME.TabIndex = 9;
            this.tbBaseME.Leave += new System.EventHandler(this.tbBaseME_Leave);
            // 
            // tbValorML
            // 
            this.tbValorML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbValorML.EditValue = "0";
            this.tbValorML.Location = new System.Drawing.Point(122, 23);
            this.tbValorML.Name = "tbValorML";
            this.tbValorML.Properties.Appearance.Options.UseTextOptions = true;
            this.tbValorML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.tbValorML.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tbValorML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tbValorML.Properties.Mask.EditMask = "c";
            this.tbValorML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.tbValorML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.tbValorML.Size = new System.Drawing.Size(166, 20);
            this.tbValorML.TabIndex = 8;
            // 
            // lblBaseML
            // 
            this.lblBaseML.AutoSize = true;
            this.lblBaseML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBaseML.Location = new System.Drawing.Point(3, 0);
            this.lblBaseML.Name = "lblBaseML";
            this.lblBaseML.Size = new System.Drawing.Size(113, 20);
            this.lblBaseML.TabIndex = 0;
            this.lblBaseML.Text = "1021_lblBaseML";
            this.lblBaseML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValorML
            // 
            this.lblValorML.AutoSize = true;
            this.lblValorML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorML.Location = new System.Drawing.Point(3, 20);
            this.lblValorML.Name = "lblValorML";
            this.lblValorML.Size = new System.Drawing.Size(113, 20);
            this.lblValorML.TabIndex = 1;
            this.lblValorML.Text = "1021_lblValorML";
            this.lblValorML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBaseME
            // 
            this.lblBaseME.AutoSize = true;
            this.lblBaseME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBaseME.Location = new System.Drawing.Point(3, 40);
            this.lblBaseME.Name = "lblBaseME";
            this.lblBaseME.Size = new System.Drawing.Size(113, 20);
            this.lblBaseME.TabIndex = 2;
            this.lblBaseME.Text = "1021_lblBaseME";
            this.lblBaseME.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValorME
            // 
            this.lblValorME.AutoSize = true;
            this.lblValorME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorME.Location = new System.Drawing.Point(3, 60);
            this.lblValorME.Name = "lblValorME";
            this.lblValorME.Size = new System.Drawing.Size(113, 20);
            this.lblValorME.TabIndex = 3;
            this.lblValorME.Text = "1021_lblValorME";
            this.lblValorME.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbBaseML
            // 
            this.tbBaseML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBaseML.EditValue = "0";
            this.tbBaseML.Location = new System.Drawing.Point(122, 3);
            this.tbBaseML.Name = "tbBaseML";
            this.tbBaseML.Properties.Appearance.Options.UseTextOptions = true;
            this.tbBaseML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.tbBaseML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tbBaseML.Properties.Mask.EditMask = "c";
            this.tbBaseML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.tbBaseML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.tbBaseML.Size = new System.Drawing.Size(166, 20);
            this.tbBaseML.TabIndex = 7;
            this.tbBaseML.Leave += new System.EventHandler(this.tbBaseML_Leave);
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(299, 390);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(92, 23);
            this.btnAccept.TabIndex = 11;
            this.btnAccept.Text = "1021_btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // DigitacionCuentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 426);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.glValores);
            this.Controls.Add(this.gbParameters);
            this.Name = "DigitacionCuentas";
            this.Text = "1021";
            this.gbParameters.ResumeLayout(false);
            this.glValores.ResumeLayout(false);
            this.tlpValores.ResumeLayout(false);
            this.tlpValores.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbValorME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBaseME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbValorML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBaseML.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterConceptoCargo;
        private ControlsUC.uc_MasterFind masterProyecto;
        private ControlsUC.uc_MasterFind masterCentroCosto;
        private ControlsUC.uc_MasterFind masterLineaPre;
        private ControlsUC.uc_MasterFind masterLugarGeo;
        private ControlsUC.uc_MasterFind masterCuenta;
        private System.Windows.Forms.GroupBox gbParameters;
        private System.Windows.Forms.GroupBox glValores;
        private System.Windows.Forms.TableLayoutPanel tlpValores;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Label lblBaseML;
        private System.Windows.Forms.Label lblValorML;
        private System.Windows.Forms.Label lblBaseME;
        private System.Windows.Forms.Label lblValorME;
        private DevExpress.XtraEditors.TextEdit tbValorME;
        private DevExpress.XtraEditors.TextEdit tbBaseME;
        private DevExpress.XtraEditors.TextEdit tbValorML;
        private DevExpress.XtraEditors.TextEdit tbBaseML;
    }
}