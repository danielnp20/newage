namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class GestionDatacredito
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GestionDatacredito));
            this.lblMessages = new System.Windows.Forms.Label();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.lblLeyenda = new System.Windows.Forms.Label();
            this.btnGenerateSol = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnInconsistencias = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 244);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(401, 18);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(180, 332);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 13);
            this.lblMessages.TabIndex = 11;
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(3, -1);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            this.dtFecha.TabIndex = 2;
            this.dtFecha.Visible = false;
            // 
            // lblLeyenda
            // 
            this.lblLeyenda.BackColor = System.Drawing.Color.Transparent;
            this.lblLeyenda.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeyenda.Location = new System.Drawing.Point(0, 228);
            this.lblLeyenda.Name = "lblLeyenda";
            this.lblLeyenda.Size = new System.Drawing.Size(401, 13);
            this.lblLeyenda.TabIndex = 115;
            this.lblLeyenda.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGenerateSol
            // 
            this.btnGenerateSol.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateSol.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerateSol.Image")));
            this.btnGenerateSol.Location = new System.Drawing.Point(93, 37);
            this.btnGenerateSol.Name = "btnGenerateSol";
            this.btnGenerateSol.Size = new System.Drawing.Size(207, 39);
            this.btnGenerateSol.TabIndex = 116;
            this.btnGenerateSol.Text = "   Generar Solicitud";
            this.btnGenerateSol.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGenerateSol.UseVisualStyleBackColor = true;
            this.btnGenerateSol.Click += new System.EventHandler(this.btnGenerateSol_Click);
            // 
            // btnClean
            // 
            this.btnClean.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClean.Image = ((System.Drawing.Image)(resources.GetObject("btnClean.Image")));
            this.btnClean.Location = new System.Drawing.Point(0, 12);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(41, 23);
            this.btnClean.TabIndex = 11;
            this.btnClean.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Visible = false;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnInconsistencias
            // 
            this.btnInconsistencias.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInconsistencias.Image = global::NewAge.Properties.Resources.errorIcon;
            this.btnInconsistencias.Location = new System.Drawing.Point(93, 162);
            this.btnInconsistencias.Name = "btnInconsistencias";
            this.btnInconsistencias.Size = new System.Drawing.Size(207, 33);
            this.btnInconsistencias.TabIndex = 10;
            this.btnInconsistencias.Text = "1123_btnInconsisten";
            this.btnInconsistencias.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInconsistencias.UseVisualStyleBackColor = true;
            this.btnInconsistencias.Click += new System.EventHandler(this.btnInconsistencias_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportar.Image = global::NewAge.Properties.Resources.Excel;
            this.btnImportar.Location = new System.Drawing.Point(93, 101);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(207, 38);
            this.btnImportar.TabIndex = 6;
            this.btnImportar.Text = "     Migrar Archivo";
            this.btnImportar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnImportar, "Orden Hojas Excel:\r\n-Datos\r\n-Score\r\n-Ubicación\r\n-Quanto");
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Excel";
            // 
            // GestionDatacredito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(401, 262);
            this.Controls.Add(this.btnGenerateSol);
            this.Controls.Add(this.lblLeyenda);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.dtFecha);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnImportar);
            this.Name = "GestionDatacredito";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.SetChildIndex(this.btnImportar, 0);
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            this.Controls.SetChildIndex(this.dtFecha, 0);
            this.Controls.SetChildIndex(this.btnClean, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.lblLeyenda, 0);
            this.Controls.SetChildIndex(this.btnGenerateSol, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Button btnInconsistencias;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        protected System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.Label lblLeyenda;
        private System.Windows.Forms.Button btnGenerateSol;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;
    }
}