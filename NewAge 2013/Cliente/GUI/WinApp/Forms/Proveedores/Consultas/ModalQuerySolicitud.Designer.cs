namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalQuerySolicitud
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
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.lblDestino = new System.Windows.Forms.Label();
            this.cmbDestino = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDestino.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbFilterByDocumento)).BeginInit();
            this.gbFilterByDocumento.SuspendLayout();
            //
            this.gbFilterByDocumento.Controls.Add(this.lblDestino);
            this.gbFilterByDocumento.Controls.Add(this.cmbDestino);
            // 
            // lblDestino
            // 
            this.lblDestino.AutoSize = true;
            this.lblDestino.Location = new System.Drawing.Point(24, 30);
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Size = new System.Drawing.Size(94, 14);
            this.lblDestino.TabIndex = 0;
            this.lblDestino.Text = "1034_lblDestino";
            // 
            // cmbDestino
            // 
            this.cmbDestino.Location = new System.Drawing.Point(96, 27);
            this.cmbDestino.Name = "cmbDestino";
            this.cmbDestino.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDestino.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 10, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.cmbDestino.Properties.DisplayMember = "Value";
            this.cmbDestino.Properties.ValueMember = "Key";
            this.cmbDestino.Size = new System.Drawing.Size(100, 20);


            ((System.ComponentModel.ISupportInitialize)(this.gbFilterByDocumento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDestino.Properties)).EndInit();
            this.gbFilterByDocumento.ResumeLayout(false);
            this.gbFilterByDocumento.PerformLayout();
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "ModalQuerySolicitud";
        }
        #endregion

        private DevExpress.XtraEditors.LookUpEdit cmbDestino;
        private System.Windows.Forms.Label lblDestino;

    }
}