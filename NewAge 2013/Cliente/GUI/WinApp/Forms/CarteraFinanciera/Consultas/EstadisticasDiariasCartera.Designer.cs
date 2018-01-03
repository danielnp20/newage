namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class EstadisticasDiariasCartera
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
            DevExpress.XtraCharts.StackedLineSeriesLabel stackedLineSeriesLabel1 = new DevExpress.XtraCharts.StackedLineSeriesLabel();
            DevExpress.XtraCharts.FullStackedLineSeriesView fullStackedLineSeriesView1 = new DevExpress.XtraCharts.FullStackedLineSeriesView();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chartCierre = new DevExpress.XtraCharts.ChartControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTipo = new DevExpress.XtraEditors.LookUpEdit();
            this.masterLineaCredito = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.label1 = new System.Windows.Forms.Label();
            this.masterAsesor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCentroPago = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterZona = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.masterComprador = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.comboPlazo = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.lblPlazo = new System.Windows.Forms.Label();
            this.rbDetails = new DevExpress.XtraEditors.RadioGroup();
            this.chartDetail = new DevExpress.XtraCharts.ChartControl();
            this.lblDia = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCierre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stackedLineSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(fullStackedLineSeriesView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbDetails.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.chartCierre, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.39968F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.60032F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(995, 623);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // chartCierre
            // 
            this.chartCierre.AppearanceNameSerializable = "Gray";
            this.chartCierre.BackColor = System.Drawing.Color.Transparent;
            this.chartCierre.BorderOptions.Color = System.Drawing.Color.Silver;
            this.chartCierre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCierre.Location = new System.Drawing.Point(3, 236);
            this.chartCierre.Name = "chartCierre";
            this.chartCierre.PaletteName = "Office";
            this.chartCierre.RuntimeHitTesting = true;
            this.chartCierre.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            stackedLineSeriesLabel1.LineVisible = true;
            this.chartCierre.SeriesTemplate.Label = stackedLineSeriesLabel1;
            this.chartCierre.SeriesTemplate.View = fullStackedLineSeriesView1;
            this.chartCierre.Size = new System.Drawing.Size(989, 384);
            this.chartCierre.TabIndex = 9;
            this.chartCierre.BoundDataChanged += new DevExpress.XtraCharts.BoundDataChangedEventHandler(this.chartCierre_BoundDataChanged);
            this.chartCierre.Click += new System.EventHandler(this.chartCierre_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(989, 227);
            this.panel1.TabIndex = 10;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblDia);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.cmbTipo);
            this.splitContainer1.Panel1.Controls.Add(this.masterLineaCredito);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.masterAsesor);
            this.splitContainer1.Panel1.Controls.Add(this.masterCentroPago);
            this.splitContainer1.Panel1.Controls.Add(this.masterZona);
            this.splitContainer1.Panel1.Controls.Add(this.dtPeriod);
            this.splitContainer1.Panel1.Controls.Add(this.masterComprador);
            this.splitContainer1.Panel1.Controls.Add(this.comboPlazo);
            this.splitContainer1.Panel1.Controls.Add(this.lblPlazo);
            this.splitContainer1.Panel1.Controls.Add(this.rbDetails);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chartDetail);
            this.splitContainer1.Size = new System.Drawing.Size(989, 227);
            this.splitContainer1.SplitterDistance = 339;
            this.splitContainer1.TabIndex = 86;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 14);
            this.label2.TabIndex = 84;
            this.label2.Text = "32312_Periodo";
            // 
            // cmbTipo
            // 
            this.cmbTipo.Location = new System.Drawing.Point(119, 29);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipo.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbTipo.Properties.DisplayMember = "Value";
            this.cmbTipo.Properties.NullText = " ";
            this.cmbTipo.Properties.ValueMember = "Key";
            this.cmbTipo.Size = new System.Drawing.Size(117, 20);
            this.cmbTipo.TabIndex = 24;
            // 
            // masterLineaCredito
            // 
            this.masterLineaCredito.BackColor = System.Drawing.Color.Transparent;
            this.masterLineaCredito.Filtros = null;
            this.masterLineaCredito.Location = new System.Drawing.Point(45, 54);
            this.masterLineaCredito.Name = "masterLineaCredito";
            this.masterLineaCredito.Size = new System.Drawing.Size(291, 25);
            this.masterLineaCredito.TabIndex = 0;
            this.masterLineaCredito.Value = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 14);
            this.label1.TabIndex = 22;
            this.label1.Text = "32312_Tipo";
            // 
            // masterAsesor
            // 
            this.masterAsesor.BackColor = System.Drawing.Color.Transparent;
            this.masterAsesor.Filtros = null;
            this.masterAsesor.Location = new System.Drawing.Point(45, 82);
            this.masterAsesor.Name = "masterAsesor";
            this.masterAsesor.Size = new System.Drawing.Size(291, 25);
            this.masterAsesor.TabIndex = 1;
            this.masterAsesor.Value = "";
            // 
            // masterCentroPago
            // 
            this.masterCentroPago.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroPago.Filtros = null;
            this.masterCentroPago.Location = new System.Drawing.Point(45, 108);
            this.masterCentroPago.Name = "masterCentroPago";
            this.masterCentroPago.Size = new System.Drawing.Size(291, 25);
            this.masterCentroPago.TabIndex = 2;
            this.masterCentroPago.Value = "";
            // 
            // masterZona
            // 
            this.masterZona.BackColor = System.Drawing.Color.Transparent;
            this.masterZona.Filtros = null;
            this.masterZona.Location = new System.Drawing.Point(45, 135);
            this.masterZona.Name = "masterZona";
            this.masterZona.Size = new System.Drawing.Size(291, 25);
            this.masterZona.TabIndex = 3;
            this.masterZona.Value = "";
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(118, 4);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 18);
            this.dtPeriod.TabIndex = 83;
            this.dtPeriod.Leave += new System.EventHandler(this.dtPeriod_Leave);
            // 
            // masterComprador
            // 
            this.masterComprador.BackColor = System.Drawing.Color.Transparent;
            this.masterComprador.Filtros = null;
            this.masterComprador.Location = new System.Drawing.Point(45, 162);
            this.masterComprador.Name = "masterComprador";
            this.masterComprador.Size = new System.Drawing.Size(291, 25);
            this.masterComprador.TabIndex = 4;
            this.masterComprador.Value = "";
            // 
            // comboPlazo
            // 
            this.comboPlazo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPlazo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboPlazo.FormattingEnabled = true;
            this.comboPlazo.Items.AddRange(new object[] {
            "",
            "12",
            "18",
            "24",
            "36",
            "42",
            "48",
            "60"});
            this.comboPlazo.Location = new System.Drawing.Point(146, 193);
            this.comboPlazo.Name = "comboPlazo";
            this.comboPlazo.Size = new System.Drawing.Size(47, 22);
            this.comboPlazo.TabIndex = 21;
            // 
            // lblPlazo
            // 
            this.lblPlazo.AutoSize = true;
            this.lblPlazo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlazo.Location = new System.Drawing.Point(42, 194);
            this.lblPlazo.Name = "lblPlazo";
            this.lblPlazo.Size = new System.Drawing.Size(76, 14);
            this.lblPlazo.TabIndex = 20;
            this.lblPlazo.Text = "32312_Plazo";
            // 
            // rbDetails
            // 
            this.rbDetails.Location = new System.Drawing.Point(12, 48);
            this.rbDetails.Name = "rbDetails";
            this.rbDetails.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rbDetails.Properties.Appearance.Options.UseBackColor = true;
            this.rbDetails.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rbDetails.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(),
            new DevExpress.XtraEditors.Controls.RadioGroupItem()});
            this.rbDetails.Size = new System.Drawing.Size(24, 171);
            this.rbDetails.TabIndex = 0;
            // 
            // chartDetail
            // 
            this.chartDetail.BackColor = System.Drawing.Color.Transparent;
            this.chartDetail.BorderOptions.Color = System.Drawing.Color.DarkGray;
            this.chartDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDetail.Location = new System.Drawing.Point(0, 0);
            this.chartDetail.Name = "chartDetail";
            this.chartDetail.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            sideBySideBarSeriesLabel1.LineVisible = true;
            this.chartDetail.SeriesTemplate.Label = sideBySideBarSeriesLabel1;
            this.chartDetail.Size = new System.Drawing.Size(646, 227);
            this.chartDetail.TabIndex = 0;
            // 
            // lblDia
            // 
            this.lblDia.AutoSize = true;
            this.lblDia.Location = new System.Drawing.Point(212, 198);
            this.lblDia.Name = "lblDia";
            this.lblDia.Size = new System.Drawing.Size(59, 13);
            this.lblDia.TabIndex = 85;
            this.lblDia.Text = "32312_Dia";
            this.lblDia.Visible = false;
            // 
            // EstadisticasDiariasCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 623);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EstadisticasDiariasCartera";
            this.Text = "DashBoard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(stackedLineSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(fullStackedLineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCierre)).EndInit();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbDetails.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraCharts.ChartControl chartCierre;
        private System.Windows.Forms.Panel panel1;
        private ControlsUC.uc_MasterFind masterComprador;
        private ControlsUC.uc_MasterFind masterZona;
        private ControlsUC.uc_MasterFind masterCentroPago;
        private ControlsUC.uc_MasterFind masterAsesor;
        private ControlsUC.uc_MasterFind masterLineaCredito;
        private Clases.ComboBoxEx comboPlazo;
        private System.Windows.Forms.Label lblPlazo;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.LookUpEdit cmbTipo;
        private System.Windows.Forms.Label label2;
        protected ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraEditors.RadioGroup rbDetails;
        private DevExpress.XtraCharts.ChartControl chartDetail;
        private System.Windows.Forms.Label lblDia;




    }
}