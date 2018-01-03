namespace NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters
{
    partial class ReportParameterTextBox
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
            this.tbLyPn = new System.Windows.Forms.TableLayoutPanel();
            this.flPn = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFrom = new System.Windows.Forms.Label();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.lblUntil = new System.Windows.Forms.Label();
            this.txtUntil = new System.Windows.Forms.TextBox();
            this.lblOption = new System.Windows.Forms.Label();
            this.tbLyPn.SuspendLayout();
            this.flPn.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLyPn
            // 
            this.tbLyPn.ColumnCount = 2;
            this.tbLyPn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.60886F));
            this.tbLyPn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.39114F));
            this.tbLyPn.Controls.Add(this.flPn, 1, 0);
            this.tbLyPn.Controls.Add(this.lblOption, 0, 0);
            this.tbLyPn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLyPn.Location = new System.Drawing.Point(0, 0);
            this.tbLyPn.Name = "tbLyPn";
            this.tbLyPn.RowCount = 1;
            this.tbLyPn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tbLyPn.Size = new System.Drawing.Size(299, 31);
            this.tbLyPn.TabIndex = 0;
            // 
            // flPn
            // 
            this.flPn.Controls.Add(this.lblFrom);
            this.flPn.Controls.Add(this.txtFrom);
            this.flPn.Controls.Add(this.lblUntil);
            this.flPn.Controls.Add(this.txtUntil);
            this.flPn.Location = new System.Drawing.Point(91, 3);
            this.flPn.Name = "flPn";
            this.flPn.Size = new System.Drawing.Size(205, 24);
            this.flPn.TabIndex = 10;
            // 
            // lblFrom
            // 
            this.lblFrom.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(1, 0);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblFrom.Size = new System.Drawing.Size(42, 24);
            this.lblFrom.TabIndex = 11;
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(49, 3);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(50, 20);
            this.txtFrom.TabIndex = 4;
            this.txtFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOption_KeyPress);
            // 
            // lblUntil
            // 
            this.lblUntil.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUntil.Location = new System.Drawing.Point(103, 0);
            this.lblUntil.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.lblUntil.Name = "lblUntil";
            this.lblUntil.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblUntil.Size = new System.Drawing.Size(40, 24);
            this.lblUntil.TabIndex = 5;
            this.lblUntil.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUntil
            // 
            this.txtUntil.Location = new System.Drawing.Point(149, 3);
            this.txtUntil.Name = "txtUntil";
            this.txtUntil.Size = new System.Drawing.Size(50, 20);
            this.txtUntil.TabIndex = 6;
            this.txtUntil.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOption_KeyPress);
            // 
            // lblOption
            // 
            this.lblOption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOption.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOption.Location = new System.Drawing.Point(3, 0);
            this.lblOption.Name = "lblOption";
            this.lblOption.Size = new System.Drawing.Size(82, 30);
            this.lblOption.TabIndex = 12;
            this.lblOption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ReportParameterTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbLyPn);
            this.Name = "ReportParameterTextBox";
            this.Size = new System.Drawing.Size(299, 31);
            this.tbLyPn.ResumeLayout(false);
            this.flPn.ResumeLayout(false);
            this.flPn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbLyPn;
        private System.Windows.Forms.FlowLayoutPanel flPn;
        public System.Windows.Forms.TextBox txtFrom;
        public System.Windows.Forms.Label lblUntil;
        public System.Windows.Forms.TextBox txtUntil;
        public System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblOption;

    }
}
