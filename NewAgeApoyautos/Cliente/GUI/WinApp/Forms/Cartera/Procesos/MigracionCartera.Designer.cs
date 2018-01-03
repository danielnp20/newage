namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MigracionCartera
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
            base.InitializeComponent();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.btnTemplate = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblMessages = new System.Windows.Forms.Label();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.masterComodin = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblComodin = new System.Windows.Forms.Label();
            this.btnInconsistencias = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(0, 360);
            this.pbProcess.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.pbProcess.Properties.LookAndFeel.SkinName = "McSkin";
            this.pbProcess.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbProcess.Size = new System.Drawing.Size(546, 28);
            // 
            // btnGenerar
            // 
            this.btnGenerar.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.Location = new System.Drawing.Point(160, 243);
            this.btnGenerar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(228, 51);
            this.btnGenerar.TabIndex = 4;
            this.btnGenerar.Text = "1102_btnGenerar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerarDocumentos_Click);
            // 
            // btnTemplate
            // 
            this.btnTemplate.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTemplate.Location = new System.Drawing.Point(160, 123);
            this.btnTemplate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(228, 51);
            this.btnTemplate.TabIndex = 5;
            this.btnTemplate.Text = "1102_btnTemplate";
            this.btnTemplate.UseVisualStyleBackColor = true;
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(160, 183);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(228, 51);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "1102_btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(286, 260);
            this.lblMessages.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(0, 20);
            this.lblMessages.TabIndex = 7;
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(194, 14);
            this.dtPeriod.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(195, 31);
            this.dtPeriod.TabIndex = 11;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(36, 20);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(114, 20);
            this.lblPeriod.TabIndex = 10;
            this.lblPeriod.Text = "1111_lblPeriod";
            // 
            // masterComodin
            // 
            this.masterComodin.BackColor = System.Drawing.Color.Transparent;
            this.masterComodin.Filtros = null;
            this.masterComodin.Location = new System.Drawing.Point(45, 69);
            this.masterComodin.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.masterComodin.Name = "masterComodin";
            this.masterComodin.Size = new System.Drawing.Size(436, 38);
            this.masterComodin.TabIndex = 12;
            this.masterComodin.Value = "";
            // 
            // lblComodin
            // 
            this.lblComodin.AutoSize = true;
            this.lblComodin.Location = new System.Drawing.Point(36, 77);
            this.lblComodin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblComodin.Name = "lblComodin";
            this.lblComodin.Size = new System.Drawing.Size(132, 20);
            this.lblComodin.TabIndex = 13;
            this.lblComodin.Text = "1111_lblComodin";
            // 
            // btnInconsistencias
            // 
            this.btnInconsistencias.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInconsistencias.Location = new System.Drawing.Point(160, 300);
            this.btnInconsistencias.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnInconsistencias.Name = "btnInconsistencias";
            this.btnInconsistencias.Size = new System.Drawing.Size(228, 51);
            this.btnInconsistencias.TabIndex = 14;
            this.btnInconsistencias.Text = "1102_btnInconsisten";
            this.btnInconsistencias.UseVisualStyleBackColor = true;
            this.btnInconsistencias.Click += new System.EventHandler(this.btnInconsistencias_Click);
            // 
            // MigracionCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 388);
            this.Controls.Add(this.btnInconsistencias);
            this.Controls.Add(this.lblComodin);
            this.Controls.Add(this.masterComodin);
            this.Controls.Add(this.dtPeriod);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lblMessages);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.btnGenerar);
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "MigracionCartera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1102";
            this.Controls.SetChildIndex(this.btnGenerar, 0);
            this.Controls.SetChildIndex(this.btnTemplate, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
            this.Controls.SetChildIndex(this.lblMessages, 0);
            this.Controls.SetChildIndex(this.lblPeriod, 0);
            this.Controls.SetChildIndex(this.dtPeriod, 0);
            this.Controls.SetChildIndex(this.pbProcess, 0);
            this.Controls.SetChildIndex(this.masterComodin, 0);
            this.Controls.SetChildIndex(this.lblComodin, 0);
            this.Controls.SetChildIndex(this.btnInconsistencias, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pbProcess.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Button btnTemplate;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label lblMessages;
        private ControlsUC.uc_PeriodoEdit dtPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private ControlsUC.uc_MasterFind masterComodin;
        private System.Windows.Forms.Label lblComodin;
        private System.Windows.Forms.Button btnInconsistencias;
    }
}