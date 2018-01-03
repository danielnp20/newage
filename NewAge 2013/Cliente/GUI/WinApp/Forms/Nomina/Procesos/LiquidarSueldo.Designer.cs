namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class LiquidarSueldo
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
            this.btnLLiquidarSueldo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLLiquidarSueldo
            // 
            this.btnLLiquidarSueldo.Location = new System.Drawing.Point(12, 84);
            this.btnLLiquidarSueldo.Name = "btnLLiquidarSueldo";
            this.btnLLiquidarSueldo.Size = new System.Drawing.Size(327, 23);
            this.btnLLiquidarSueldo.TabIndex = 0;
            this.btnLLiquidarSueldo.Text = "btnLLiquidarSueldo";
            this.btnLLiquidarSueldo.UseVisualStyleBackColor = true;
            this.btnLLiquidarSueldo.Click += new System.EventHandler(this.btnLLiquidarSueldo_Click);
            // 
            // LiquidarSueldo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 208);
            this.Controls.Add(this.btnLLiquidarSueldo);
            this.Name = "LiquidarSueldo";
            this.Text = "LiquidarSueldo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLLiquidarSueldo;
    }
}