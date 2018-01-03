namespace NewAge.Cliente.GUI.WinApp.ControlsUC
{
    partial class uc_MasterFind
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uc_MasterFind));
            this.btnFind = new System.Windows.Forms.Button();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblMaster = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFind
            // 
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.Location = new System.Drawing.Point(176, 2);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(23, 20);
            this.btnFind.TabIndex = 12;
            this.btnFind.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(200, 2);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ReadOnly = true;
            this.txtDesc.Size = new System.Drawing.Size(94, 20);
            this.txtDesc.TabIndex = 13;
            this.txtDesc.TabStop = false;
            // 
            // txtCode
            // 
            this.txtCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCode.Location = new System.Drawing.Point(100, 2);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(75, 20);
            this.txtCode.TabIndex = 1;
            this.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // lblMaster
            // 
            this.lblMaster.AutoEllipsis = true;
            this.lblMaster.AutoSize = true;
            this.lblMaster.BackColor = System.Drawing.Color.Transparent;
            this.lblMaster.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblMaster.Location = new System.Drawing.Point(-3, 6);
            this.lblMaster.Margin = new System.Windows.Forms.Padding(30, 20, 3, 0);
            this.lblMaster.Name = "lblMaster";
            this.lblMaster.Size = new System.Drawing.Size(31, 13);
            this.lblMaster.TabIndex = 11;
            this.lblMaster.Text = "1006";
            this.lblMaster.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMaster.UseMnemonic = false;
            // 
            // uc_MasterFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblMaster);
            this.Name = "uc_MasterFind";
            this.Size = new System.Drawing.Size(299, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtDesc;
        public System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblMaster;
    }
}
