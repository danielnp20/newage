namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReportParametersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportParametersForm));
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnResetFilter = new System.Windows.Forms.Button();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnExportToPDF = new System.Windows.Forms.Button();
            this.scrollControl = new DevExpress.XtraEditors.XtraScrollableControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelGenerar = new DevExpress.XtraEditors.PanelControl();
            this.btnExportToXLS = new System.Windows.Forms.Button();
            this.panelFecha = new DevExpress.XtraEditors.PanelControl();
            this.periodoFilter1 = new NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters.PeriodoFilter();
            this.panelFiltro = new DevExpress.XtraEditors.GroupControl();
            this.panelOptions = new System.Windows.Forms.FlowLayoutPanel();
            this.panelFondoOpt = new DevExpress.XtraEditors.PanelControl();
            this.scrollControl.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelGenerar)).BeginInit();
            this.panelGenerar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelFecha)).BeginInit();
            this.panelFecha.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelFiltro)).BeginInit();
            this.panelFiltro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoOpt)).BeginInit();
            this.panelFondoOpt.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFilter
            // 
            this.btnFilter.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.Location = new System.Drawing.Point(94, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(107, 23);
            this.btnFilter.TabIndex = 0;
            this.btnFilter.Text = "1013_btnFilter";
            this.btnFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnResetFilter
            // 
            this.btnResetFilter.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetFilter.Image = ((System.Drawing.Image)(resources.GetObject("btnResetFilter.Image")));
            this.btnResetFilter.Location = new System.Drawing.Point(205, 3);
            this.btnResetFilter.Name = "btnResetFilter";
            this.btnResetFilter.Size = new System.Drawing.Size(41, 23);
            this.btnResetFilter.TabIndex = 1;
            this.btnResetFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResetFilter.UseVisualStyleBackColor = true;
            this.btnResetFilter.Click += new System.EventHandler(this.btnResetFilter_Click);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilter.Location = new System.Drawing.Point(5, 3);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(79, 14);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "                  ";
            // 
            // btnExportToPDF
            // 
            this.btnExportToPDF.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportToPDF.Image = global::NewAge.Properties.Resources.pdf;
            this.btnExportToPDF.Location = new System.Drawing.Point(94, 2);
            this.btnExportToPDF.Name = "btnExportToPDF";
            this.btnExportToPDF.Size = new System.Drawing.Size(67, 75);
            this.btnExportToPDF.TabIndex = 4;
            this.btnExportToPDF.Text = "1013_btnExportToPDF";
            this.btnExportToPDF.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExportToPDF.UseVisualStyleBackColor = true;
            this.btnExportToPDF.Click += new System.EventHandler(this.btnExportToPDF_Click);
            // 
            // scrollControl
            // 
            this.scrollControl.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.scrollControl.Appearance.Options.UseBackColor = true;
            this.scrollControl.Controls.Add(this.lblFilter);
            this.scrollControl.Location = new System.Drawing.Point(6, 44);
            this.scrollControl.Name = "scrollControl";
            this.scrollControl.Size = new System.Drawing.Size(332, 144);
            this.scrollControl.TabIndex = 131;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelGenerar);
            this.panel1.Controls.Add(this.panelFecha);
            this.panel1.Location = new System.Drawing.Point(328, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 461);
            this.panel1.TabIndex = 6;
            // 
            // panelGenerar
            // 
            this.panelGenerar.Controls.Add(this.btnExportToXLS);
            this.panelGenerar.Controls.Add(this.btnExportToPDF);
            this.panelGenerar.Location = new System.Drawing.Point(3, 90);
            this.panelGenerar.LookAndFeel.SkinName = "McSkin";
            this.panelGenerar.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelGenerar.Name = "panelGenerar";
            this.panelGenerar.Size = new System.Drawing.Size(343, 78);
            this.panelGenerar.TabIndex = 133;
            // 
            // btnExportToXLS
            // 
            this.btnExportToXLS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportToXLS.Image = global::NewAge.Properties.Resources.Excel;
            this.btnExportToXLS.Location = new System.Drawing.Point(181, 2);
            this.btnExportToXLS.Name = "btnExportToXLS";
            this.btnExportToXLS.Size = new System.Drawing.Size(67, 75);
            this.btnExportToXLS.TabIndex = 133;
            this.btnExportToXLS.Text = "1013_btnExportToXLS";
            this.btnExportToXLS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExportToXLS.UseVisualStyleBackColor = true;
            this.btnExportToXLS.Visible = false;
            this.btnExportToXLS.Click += new System.EventHandler(this.btnExportToExcel_Click);
            // 
            // panelFecha
            // 
            this.panelFecha.ContentImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.panelFecha.Controls.Add(this.periodoFilter1);
            this.panelFecha.Controls.Add(this.panelFiltro);
            this.panelFecha.Location = new System.Drawing.Point(0, 0);
            this.panelFecha.LookAndFeel.SkinName = "McSkin";
            this.panelFecha.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelFecha.Name = "panelFecha";
            this.panelFecha.Size = new System.Drawing.Size(351, 461);
            this.panelFecha.TabIndex = 132;
            // 
            // periodoFilter1
            // 
            this.periodoFilter1.FilterOptions = NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters.PeriodFilterOptions.None;
            this.periodoFilter1.Location = new System.Drawing.Point(52, 12);
            this.periodoFilter1.Margin = new System.Windows.Forms.Padding(0);
            this.periodoFilter1.Name = "periodoFilter1";
            this.periodoFilter1.Size = new System.Drawing.Size(300, 54);
            this.periodoFilter1.TabIndex = 3;
            // 
            // panelFiltro
            // 
            this.panelFiltro.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.panelFiltro.Appearance.Options.UseFont = true;
            this.panelFiltro.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 13F);
            this.panelFiltro.AppearanceCaption.Options.UseFont = true;
            this.panelFiltro.Controls.Add(this.btnFilter);
            this.panelFiltro.Controls.Add(this.scrollControl);
            this.panelFiltro.Controls.Add(this.btnResetFilter);
            this.panelFiltro.Location = new System.Drawing.Point(3, 174);
            this.panelFiltro.Name = "panelFiltro";
            this.panelFiltro.Size = new System.Drawing.Size(343, 193);
            this.panelFiltro.TabIndex = 2;
            // 
            // panelOptions
            // 
            this.panelOptions.BackColor = System.Drawing.Color.Transparent;
            this.panelOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOptions.Location = new System.Drawing.Point(2, 2);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(304, 459);
            this.panelOptions.TabIndex = 7;
            // 
            // panelFondoOpt
            // 
            this.panelFondoOpt.Controls.Add(this.panelOptions);
            this.panelFondoOpt.Location = new System.Drawing.Point(15, 12);
            this.panelFondoOpt.LookAndFeel.SkinName = "McSkin";
            this.panelFondoOpt.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelFondoOpt.Name = "panelFondoOpt";
            this.panelFondoOpt.Size = new System.Drawing.Size(308, 463);
            this.panelFondoOpt.TabIndex = 135;
            // 
            // ReportParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 487);
            this.Controls.Add(this.panelFondoOpt);
            this.Controls.Add(this.panel1);
            this.Name = "ReportParametersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.scrollControl.ResumeLayout(false);
            this.scrollControl.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelGenerar)).EndInit();
            this.panelGenerar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelFecha)).EndInit();
            this.panelFecha.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelFiltro)).EndInit();
            this.panelFiltro.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelFondoOpt)).EndInit();
            this.panelFondoOpt.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Button btnFilter;
        protected System.Windows.Forms.Button btnResetFilter;
        protected Componentes.ReportParameters.PeriodoFilter periodoFilter1;
        protected System.Windows.Forms.Button btnExportToPDF;
        private System.Windows.Forms.Label lblFilter;
        private DevExpress.XtraEditors.XtraScrollableControl scrollControl;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.PanelControl panelGenerar;
        private DevExpress.XtraEditors.PanelControl panelFecha;
        private System.Windows.Forms.FlowLayoutPanel panelOptions;
        private DevExpress.XtraEditors.PanelControl panelFondoOpt;
        protected System.Windows.Forms.Button btnExportToXLS;
        private DevExpress.XtraEditors.GroupControl panelFiltro;

    }
}