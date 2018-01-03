namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalSolicitudProcesoLiquidacion
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
            this.label1 = new System.Windows.Forms.Label();
            this.ctrl_NombreCliente = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ctrl_Cedula = new System.Windows.Forms.TextBox();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "lbl_NombreCliente";
            // 
            // ctrl_NombreCliente
            // 
            this.ctrl_NombreCliente.Location = new System.Drawing.Point(91, 41);
            this.ctrl_NombreCliente.Name = "ctrl_NombreCliente";
            this.ctrl_NombreCliente.ReadOnly = true;
            this.ctrl_NombreCliente.Size = new System.Drawing.Size(197, 20);
            this.ctrl_NombreCliente.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(303, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "lbl_Cedula";
            // 
            // ctrl_Cedula
            // 
            this.ctrl_Cedula.Location = new System.Drawing.Point(356, 41);
            this.ctrl_Cedula.Name = "ctrl_Cedula";
            this.ctrl_Cedula.ReadOnly = true;
            this.ctrl_Cedula.Size = new System.Drawing.Size(222, 20);
            this.ctrl_Cedula.TabIndex = 3;
            // 
            // gcData
            // 
            this.gcData.Location = new System.Drawing.Point(52, 87);
            this.gcData.MainView = this.gvData;
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(746, 200);
            this.gcData.TabIndex = 4;
            this.gcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.GridControl = this.gcData;
            this.gvData.Name = "gvData";
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(761, 18);
            this.lblTitle.TabIndex = 78;
            this.lblTitle.Text = "1027_ProcesoLiquidacion";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SolicitudProcesoLiquidacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 327);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.gcData);
            this.Controls.Add(this.ctrl_Cedula);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ctrl_NombreCliente);
            this.Controls.Add(this.label1);
            this.Name = "SolicitudProcesoLiquidacion";
            this.Text = "SolicitudProcedoLiquidacion";
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ctrl_NombreCliente;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ctrl_Cedula;
        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private System.Windows.Forms.Label lblTitle;
    }
}