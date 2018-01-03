namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class FindDocument
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
            this.txtNumDocInterno = new System.Windows.Forms.TextBox();
            this.txtDocExterno = new System.Windows.Forms.TextBox();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.lblPrefijo = new System.Windows.Forms.Label();
            this.lblTercero = new System.Windows.Forms.Label();
            this.rbtPrefijo = new System.Windows.Forms.RadioButton();
            this.rbtTercero = new System.Windows.Forms.RadioButton();
            this.rtbComprobante = new System.Windows.Forms.RadioButton();
            this.lblComprobante = new System.Windows.Forms.Label();
            this.gb_Filters = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtNumComprobante = new System.Windows.Forms.TextBox();
            this.pnlPeriod = new System.Windows.Forms.Panel();
            this.lbl_TipoDocumento = new System.Windows.Forms.Label();
            this.mfTercero = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.mfPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.mfComprobante = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.periodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.mfDocument = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.pnlPefijo = new System.Windows.Forms.Panel();
            this.gb_Filters.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlPeriod.SuspendLayout();
            this.pnlPefijo.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNumDocInterno
            // 
            this.txtNumDocInterno.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNumDocInterno.Location = new System.Drawing.Point(4, 2);
            this.txtNumDocInterno.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.txtNumDocInterno.Name = "txtNumDocInterno";
            this.txtNumDocInterno.Size = new System.Drawing.Size(90, 22);
            this.txtNumDocInterno.TabIndex = 1;
            this.txtNumDocInterno.Click += new System.EventHandler(this.txtNumDoc_Click);
            // 
            // txtDocExterno
            // 
            this.txtDocExterno.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDocExterno.Location = new System.Drawing.Point(512, 36);
            this.txtDocExterno.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.txtDocExterno.Name = "txtDocExterno";
            this.txtDocExterno.Size = new System.Drawing.Size(116, 22);
            this.txtDocExterno.TabIndex = 3;
            this.txtDocExterno.Click += new System.EventHandler(this.txtNumDoc_Click);
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodo.Location = new System.Drawing.Point(-3, 4);
            this.lblPeriodo.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(94, 14);
            this.lblPeriodo.TabIndex = 0;
            this.lblPeriodo.Text = "1015_lblPeriodo";
            // 
            // btnAbrir
            // 
            this.btnAbrir.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrir.Location = new System.Drawing.Point(544, 227);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(116, 23);
            this.btnAbrir.TabIndex = 2;
            this.btnAbrir.Text = "1015_btnOpen";
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblPrefijo
            // 
            this.lblPrefijo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPrefijo.AutoSize = true;
            this.lblPrefijo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefijo.Location = new System.Drawing.Point(389, 9);
            this.lblPrefijo.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblPrefijo.Name = "lblPrefijo";
            this.lblPrefijo.Size = new System.Drawing.Size(87, 14);
            this.lblPrefijo.TabIndex = 0;
            this.lblPrefijo.Text = "1015_lblPrefijo";
            // 
            // lblTercero
            // 
            this.lblTercero.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTercero.AutoSize = true;
            this.lblTercero.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTercero.Location = new System.Drawing.Point(389, 40);
            this.lblTercero.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblTercero.Name = "lblTercero";
            this.lblTercero.Size = new System.Drawing.Size(96, 14);
            this.lblTercero.TabIndex = 0;
            this.lblTercero.Text = "1015_lblTercero";
            // 
            // rbtPrefijo
            // 
            this.rbtPrefijo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbtPrefijo.AutoSize = true;
            this.rbtPrefijo.Checked = true;
            this.rbtPrefijo.Location = new System.Drawing.Point(5, 8);
            this.rbtPrefijo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.rbtPrefijo.Name = "rbtPrefijo";
            this.rbtPrefijo.Size = new System.Drawing.Size(14, 13);
            this.rbtPrefijo.TabIndex = 10;
            this.rbtPrefijo.TabStop = true;
            this.rbtPrefijo.UseVisualStyleBackColor = true;
            this.rbtPrefijo.CheckedChanged += new System.EventHandler(this.rbtPrefijo_CheckedChanged);
            // 
            // rbtTercero
            // 
            this.rbtTercero.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbtTercero.AutoSize = true;
            this.rbtTercero.Location = new System.Drawing.Point(5, 39);
            this.rbtTercero.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.rbtTercero.Name = "rbtTercero";
            this.rbtTercero.Size = new System.Drawing.Size(14, 13);
            this.rbtTercero.TabIndex = 20;
            this.rbtTercero.UseVisualStyleBackColor = true;
            this.rbtTercero.CheckedChanged += new System.EventHandler(this.rbtTercero_CheckedChanged);
            // 
            // rtbComprobante
            // 
            this.rtbComprobante.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtbComprobante.AutoSize = true;
            this.rtbComprobante.Location = new System.Drawing.Point(5, 69);
            this.rtbComprobante.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.rtbComprobante.Name = "rtbComprobante";
            this.rtbComprobante.Size = new System.Drawing.Size(14, 13);
            this.rtbComprobante.TabIndex = 30;
            this.rtbComprobante.UseVisualStyleBackColor = true;
            this.rtbComprobante.CheckedChanged += new System.EventHandler(this.rtbComprobante_CheckedChanged);
            // 
            // lblComprobante
            // 
            this.lblComprobante.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblComprobante.AutoSize = true;
            this.lblComprobante.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComprobante.Location = new System.Drawing.Point(389, 92);
            this.lblComprobante.Margin = new System.Windows.Forms.Padding(15, 0, 3, 0);
            this.lblComprobante.Name = "lblComprobante";
            this.lblComprobante.Size = new System.Drawing.Size(108, 28);
            this.lblComprobante.TabIndex = 0;
            this.lblComprobante.Text = "1015_lblComprobante";
            // 
            // gb_Filters
            // 
            this.gb_Filters.Controls.Add(this.tableLayoutPanel1);
            this.gb_Filters.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb_Filters.Location = new System.Drawing.Point(12, 52);
            this.gb_Filters.Name = "gb_Filters";
            this.gb_Filters.Size = new System.Drawing.Size(666, 169);
            this.gb_Filters.TabIndex = 0;
            this.gb_Filters.TabStop = false;
            this.gb_Filters.Text = "1015_gb_Filters";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.875F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 93.125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableLayoutPanel1.Controls.Add(this.pnlPefijo, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblComprobante, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDocExterno, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTercero, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.rbtTercero, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.mfTercero, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPrefijo, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.mfPrefijo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbtPrefijo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtNumComprobante, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.mfComprobante, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.pnlPeriod, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.rtbComprobante, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(11, 29);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(640, 122);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtNumComprobante
            // 
            this.txtNumComprobante.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNumComprobante.Location = new System.Drawing.Point(512, 95);
            this.txtNumComprobante.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
            this.txtNumComprobante.Name = "txtNumComprobante";
            this.txtNumComprobante.Size = new System.Drawing.Size(116, 22);
            this.txtNumComprobante.TabIndex = 5;
            // 
            // pnlPeriod
            // 
            this.pnlPeriod.Controls.Add(this.lblPeriodo);
            this.pnlPeriod.Controls.Add(this.periodo);
            this.pnlPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPeriod.Location = new System.Drawing.Point(28, 66);
            this.pnlPeriod.Name = "pnlPeriod";
            this.pnlPeriod.Size = new System.Drawing.Size(343, 22);
            this.pnlPeriod.TabIndex = 35;
            // 
            // lbl_TipoDocumento
            // 
            this.lbl_TipoDocumento.AutoSize = true;
            this.lbl_TipoDocumento.Location = new System.Drawing.Point(20, 24);
            this.lbl_TipoDocumento.Name = "lbl_TipoDocumento";
            this.lbl_TipoDocumento.Size = new System.Drawing.Size(88, 13);
            this.lbl_TipoDocumento.TabIndex = 3;
            this.lbl_TipoDocumento.Text = "1015_lblTipoDoc";
            // 
            // mfTercero
            // 
            this.mfTercero.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mfTercero.BackColor = System.Drawing.Color.Transparent;
            this.mfTercero.Filtros = null;
            this.mfTercero.Location = new System.Drawing.Point(28, 35);
            this.mfTercero.Name = "mfTercero";
            this.mfTercero.Size = new System.Drawing.Size(302, 25);
            this.mfTercero.TabIndex = 2;
            this.mfTercero.Value = "";
            // 
            // mfPrefijo
            // 
            this.mfPrefijo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mfPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.mfPrefijo.Filtros = null;
            this.mfPrefijo.Location = new System.Drawing.Point(28, 4);
            this.mfPrefijo.Name = "mfPrefijo";
            this.mfPrefijo.Size = new System.Drawing.Size(302, 24);
            this.mfPrefijo.TabIndex = 0;
            this.mfPrefijo.Value = "";
            // 
            // mfComprobante
            // 
            this.mfComprobante.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.mfComprobante.BackColor = System.Drawing.Color.Transparent;
            this.mfComprobante.Filtros = null;
            this.mfComprobante.Location = new System.Drawing.Point(28, 94);
            this.mfComprobante.Name = "mfComprobante";
            this.mfComprobante.Size = new System.Drawing.Size(302, 24);
            this.mfComprobante.TabIndex = 4;
            this.mfComprobante.Value = "";
            // 
            // periodo
            // 
            this.periodo.BackColor = System.Drawing.Color.Transparent;
            this.periodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.periodo.EnabledControl = true;
            this.periodo.ExtraPeriods = 0;
            this.periodo.Location = new System.Drawing.Point(114, 0);
            this.periodo.MaxValue = new System.DateTime(((long)(0)));
            this.periodo.MinValue = new System.DateTime(((long)(0)));
            this.periodo.Name = "periodo";
            this.periodo.Size = new System.Drawing.Size(120, 22);
            this.periodo.TabIndex = 0;
            // 
            // mfDocument
            // 
            this.mfDocument.BackColor = System.Drawing.Color.Transparent;
            this.mfDocument.Filtros = null;
            this.mfDocument.Location = new System.Drawing.Point(36, 21);
            this.mfDocument.Name = "mfDocument";
            this.mfDocument.Size = new System.Drawing.Size(291, 25);
            this.mfDocument.TabIndex = 0;
            this.mfDocument.Value = "";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(97, 3);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(28, 20);
            this.btnQueryDoc.TabIndex = 21428;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // pnlPefijo
            // 
            this.pnlPefijo.Controls.Add(this.txtNumDocInterno);
            this.pnlPefijo.Controls.Add(this.btnQueryDoc);
            this.pnlPefijo.Location = new System.Drawing.Point(508, 3);
            this.pnlPefijo.Name = "pnlPefijo";
            this.pnlPefijo.Size = new System.Drawing.Size(129, 26);
            this.pnlPefijo.TabIndex = 21429;
            // 
            // FindDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 262);
            this.Controls.Add(this.lbl_TipoDocumento);
            this.Controls.Add(this.gb_Filters);
            this.Controls.Add(this.mfDocument);
            this.Controls.Add(this.btnAbrir);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindDocument";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1015";
            this.gb_Filters.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlPeriod.ResumeLayout(false);
            this.pnlPeriod.PerformLayout();
            this.pnlPefijo.ResumeLayout(false);
            this.pnlPefijo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPeriodo;
        private ControlsUC.uc_MasterFind mfDocument;
        private ControlsUC.uc_MasterFind mfPrefijo;
        private ControlsUC.uc_MasterFind mfTercero;
        private ControlsUC.uc_MasterFind mfComprobante;
        private System.Windows.Forms.TextBox txtNumDocInterno;
        private System.Windows.Forms.TextBox txtDocExterno;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.RadioButton rbtPrefijo;
        private System.Windows.Forms.Label lblPrefijo;
        private System.Windows.Forms.RadioButton rbtTercero;
        private System.Windows.Forms.Label lblTercero;
        private System.Windows.Forms.Label lblComprobante;
        private System.Windows.Forms.RadioButton rtbComprobante;
        private System.Windows.Forms.GroupBox gb_Filters;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtNumComprobante;
        private System.Windows.Forms.Panel pnlPeriod;
        private ControlsUC.uc_PeriodoEdit periodo;
        private System.Windows.Forms.Label lbl_TipoDocumento;
        private System.Windows.Forms.Panel pnlPefijo;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
    }
}