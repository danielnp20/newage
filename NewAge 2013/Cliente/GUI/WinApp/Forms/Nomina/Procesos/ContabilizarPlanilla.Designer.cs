namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ContabilizarPlanilla
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
            this.btnProcesar = new System.Windows.Forms.Button();
            this.uc_Periodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.label1 = new System.Windows.Forms.Label();
            // 
            // btnProcesar
            // 
            this.btnProcesar.Location = new System.Drawing.Point(57, 201);
            this.btnProcesar.Name = "btnProcesar";
            this.btnProcesar.Size = new System.Drawing.Size(164, 23);
            this.btnProcesar.TabIndex = 1;
            this.btnProcesar.Text = "1128_btnProcesar";
            this.btnProcesar.UseVisualStyleBackColor = true;
            this.btnProcesar.Click += new System.EventHandler(this.btnProcesar_Click);
            // 
            // uc_PeriodoEdit1
            // 
            this.uc_Periodo.BackColor = System.Drawing.Color.Transparent;
            this.uc_Periodo.DateTime = new System.DateTime(((long)(0)));
            this.uc_Periodo.EnabledControl = false;
            this.uc_Periodo.ExtraPeriods = 0;
            this.uc_Periodo.Location = new System.Drawing.Point(118, 40);
            this.uc_Periodo.MaxValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.MinValue = new System.DateTime(((long)(0)));
            this.uc_Periodo.Name = "uc_PeriodoEdit1";
            this.uc_Periodo.Size = new System.Drawing.Size(130, 20);
            this.uc_Periodo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "1128_lblPeriodo";
            // 
            // ContabilizarNomina
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uc_Periodo);
            this.Controls.Add(this.btnProcesar);
            this.Name = "ContabilizarPlanilla";
            this.Text = "ContabilizarPlanilla";
            this.Controls.SetChildIndex(this.btnProcesar, 0);
            this.Controls.SetChildIndex(this.uc_Periodo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcesar;
        private ControlsUC.uc_PeriodoEdit uc_Periodo;
        private System.Windows.Forms.Label label1;

    }
}