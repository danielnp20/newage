namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class RenewPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenewPassword));
            this.lblOldPassword = new System.Windows.Forms.Label();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.btnUpdatePassword = new System.Windows.Forms.Button();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.pictPassword = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictPassword)).BeginInit();
            this.SuspendLayout();
            // 
            // lblOldPassword
            // 
            this.lblOldPassword.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOldPassword.Location = new System.Drawing.Point(44, 79);
            this.lblOldPassword.Name = "lblOldPassword";
            this.lblOldPassword.Size = new System.Drawing.Size(131, 16);
            this.lblOldPassword.TabIndex = 7;
            this.lblOldPassword.Text = "1006_lblOldPassword";
            this.lblOldPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblNewPassword
            // 
            this.lblNewPassword.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewPassword.Location = new System.Drawing.Point(61, 109);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(114, 13);
            this.lblNewPassword.TabIndex = 9;
            this.lblNewPassword.Text = "1006_lblNewPassword";
            this.lblNewPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmPassword.Location = new System.Drawing.Point(2, 137);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(174, 18);
            this.lblConfirmPassword.TabIndex = 13;
            this.lblConfirmPassword.Text = "1006_lblConfirmPassword";
            this.lblConfirmPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.Location = new System.Drawing.Point(177, 76);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PasswordChar = '●';
            this.txtOldPassword.Size = new System.Drawing.Size(127, 20);
            this.txtOldPassword.TabIndex = 8;
            this.txtOldPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBoxes_KeyDown);
            // 
            // btnUpdatePassword
            // 
            this.btnUpdatePassword.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnUpdatePassword.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdatePassword.Location = new System.Drawing.Point(78, 175);
            this.btnUpdatePassword.Name = "btnUpdatePassword";
            this.btnUpdatePassword.Size = new System.Drawing.Size(175, 27);
            this.btnUpdatePassword.TabIndex = 11;
            this.btnUpdatePassword.Text = "1006_btnUpdatePassword";
            this.btnUpdatePassword.UseVisualStyleBackColor = false;
            this.btnUpdatePassword.Click += new System.EventHandler(this.btnUpdatePassword_Click);
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(177, 106);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '●';
            this.txtNewPassword.Size = new System.Drawing.Size(127, 20);
            this.txtNewPassword.TabIndex = 9;
            this.txtNewPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBoxes_KeyDown);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(125, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(101, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "1006_lblTitle";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Location = new System.Drawing.Point(177, 135);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '●';
            this.txtConfirmPassword.Size = new System.Drawing.Size(127, 20);
            this.txtConfirmPassword.TabIndex = 10;
            this.txtConfirmPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBoxes_KeyDown);
            // 
            // pictPassword
            // 
            this.pictPassword.Image = ((System.Drawing.Image)(resources.GetObject("pictPassword.Image")));
            this.pictPassword.Location = new System.Drawing.Point(58, 15);
            this.pictPassword.Name = "pictPassword";
            this.pictPassword.Size = new System.Drawing.Size(52, 48);
            this.pictPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictPassword.TabIndex = 15;
            this.pictPassword.TabStop = false;
            // 
            // RenewPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 221);
            this.Controls.Add(this.pictPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblOldPassword);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtOldPassword);
            this.Controls.Add(this.btnUpdatePassword);
            this.Controls.Add(this.txtNewPassword);
            this.Name = "RenewPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "1006";
            ((System.ComponentModel.ISupportInitialize)(this.pictPassword)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblOldPassword;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtOldPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Button btnUpdatePassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.PictureBox pictPassword;
    }
}