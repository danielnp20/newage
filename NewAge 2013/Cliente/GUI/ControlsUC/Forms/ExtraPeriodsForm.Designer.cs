namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    partial class ExtraPeriodsForm
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
            this.rdGroup = new DevExpress.XtraEditors.RadioGroup();
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.lblSelect = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rdGroup.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // rdGroup
            // 
            this.rdGroup.Location = new System.Drawing.Point(13, 45);
            this.rdGroup.Name = "rdGroup";
            this.rdGroup.Size = new System.Drawing.Size(259, 107);
            this.rdGroup.TabIndex = 0;
            this.rdGroup.SelectedIndexChanged += new System.EventHandler(this.rdGroup_SelectedIndexChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(85, 162);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(101, 23);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "btnAccept";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelect.Location = new System.Drawing.Point(12, 22);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(81, 14);
            this.lblSelect.TabIndex = 2;
            this.lblSelect.Text = "lblTittleSelect";
            // 
            // ExtraPeriodsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 195);
            this.ControlBox = false;
            this.Controls.Add(this.lblSelect);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.rdGroup);
            this.Name = "ExtraPeriodsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "titPeriod";
            ((System.ComponentModel.ISupportInitialize)(this.rdGroup.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.RadioGroup rdGroup;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
        private System.Windows.Forms.Label lblSelect;
    }
}