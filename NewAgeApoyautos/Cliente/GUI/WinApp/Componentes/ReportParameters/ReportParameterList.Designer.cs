namespace NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters
{
    partial class ReportParameterList
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
            this.comboEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.comboEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // comboEdit
            // 
            this.comboEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.comboEdit.Location = new System.Drawing.Point(108, 0);
            this.comboEdit.Name = "comboEdit";
            this.comboEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboEdit.Size = new System.Drawing.Size(140, 20);
            this.comboEdit.TabIndex = 0;
            this.comboEdit.SelectedValueChanged += new System.EventHandler(this.comboEdit_SelectedValueChanged);
            // 
            // lbl
            // 
            this.lbl.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Location = new System.Drawing.Point(0, 0);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(100, 22);
            this.lbl.TabIndex = 0;
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReportParameterList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.comboEdit);
            this.Name = "ReportParameterList";
            this.Size = new System.Drawing.Size(208, 22);
            ((System.ComponentModel.ISupportInitialize)(this.comboEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit comboEdit;
        private System.Windows.Forms.Label lbl;
    }
}
