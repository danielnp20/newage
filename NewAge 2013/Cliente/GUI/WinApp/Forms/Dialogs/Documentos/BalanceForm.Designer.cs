namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class BalanceForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.TextBox();
            this.lblPeriodTit = new System.Windows.Forms.Label();
            this.lblDocTit = new System.Windows.Forms.Label();
            this.lblTerceroTit = new System.Windows.Forms.Label();
            this.lblCuentaTit = new System.Windows.Forms.Label();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.lblTercero = new System.Windows.Forms.Label();
            this.lblDoc = new System.Windows.Forms.Label();
            this.lblCuenta = new System.Windows.Forms.Label();
            this.grCtrlSaldo = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.grCtrlSaldo)).BeginInit();
            this.grCtrlSaldo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(89, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(151, 16);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "1011_lblQueryBalance";
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(2, 21);
            this.lblMessage.Multiline = true;
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.ReadOnly = true;
            this.lblMessage.Size = new System.Drawing.Size(285, 148);
            this.lblMessage.TabIndex = 2;
            // 
            // lblPeriodTit
            // 
            this.lblPeriodTit.AutoSize = true;
            this.lblPeriodTit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodTit.Location = new System.Drawing.Point(23, 38);
            this.lblPeriodTit.Name = "lblPeriodTit";
            this.lblPeriodTit.Size = new System.Drawing.Size(103, 16);
            this.lblPeriodTit.TabIndex = 3;
            this.lblPeriodTit.Text = "1011_lblPeriod";
            // 
            // lblDocTit
            // 
            this.lblDocTit.AutoSize = true;
            this.lblDocTit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocTit.Location = new System.Drawing.Point(23, 77);
            this.lblDocTit.Name = "lblDocTit";
            this.lblDocTit.Size = new System.Drawing.Size(86, 16);
            this.lblDocTit.TabIndex = 4;
            this.lblDocTit.Text = "1011_lblDoc";
            // 
            // lblTerceroTit
            // 
            this.lblTerceroTit.AutoSize = true;
            this.lblTerceroTit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTerceroTit.Location = new System.Drawing.Point(23, 56);
            this.lblTerceroTit.Name = "lblTerceroTit";
            this.lblTerceroTit.Size = new System.Drawing.Size(112, 16);
            this.lblTerceroTit.TabIndex = 5;
            this.lblTerceroTit.Text = "1011_lblTercero";
            // 
            // lblCuentaTit
            // 
            this.lblCuentaTit.AutoSize = true;
            this.lblCuentaTit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCuentaTit.Location = new System.Drawing.Point(24, 97);
            this.lblCuentaTit.Name = "lblCuentaTit";
            this.lblCuentaTit.Size = new System.Drawing.Size(108, 16);
            this.lblCuentaTit.TabIndex = 6;
            this.lblCuentaTit.Text = "1011_lblCuenta";
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(133, 38);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(51, 16);
            this.lblPeriod.TabIndex = 7;
            this.lblPeriod.Text = "Periodo";
            // 
            // lblTercero
            // 
            this.lblTercero.AutoSize = true;
            this.lblTercero.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTercero.Location = new System.Drawing.Point(133, 58);
            this.lblTercero.Name = "lblTercero";
            this.lblTercero.Size = new System.Drawing.Size(53, 16);
            this.lblTercero.TabIndex = 8;
            this.lblTercero.Text = "Tercero";
            // 
            // lblDoc
            // 
            this.lblDoc.AutoSize = true;
            this.lblDoc.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDoc.Location = new System.Drawing.Point(133, 79);
            this.lblDoc.Name = "lblDoc";
            this.lblDoc.Size = new System.Drawing.Size(29, 16);
            this.lblDoc.TabIndex = 9;
            this.lblDoc.Text = "Doc";
            // 
            // lblCuenta
            // 
            this.lblCuenta.AutoSize = true;
            this.lblCuenta.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCuenta.Location = new System.Drawing.Point(133, 99);
            this.lblCuenta.Name = "lblCuenta";
            this.lblCuenta.Size = new System.Drawing.Size(48, 16);
            this.lblCuenta.TabIndex = 10;
            this.lblCuenta.Text = "Cuenta";
            // 
            // grCtrlSaldo
            // 
            this.grCtrlSaldo.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.grCtrlSaldo.AppearanceCaption.Options.UseFont = true;
            this.grCtrlSaldo.Controls.Add(this.lblMessage);
            this.grCtrlSaldo.Location = new System.Drawing.Point(27, 126);
            this.grCtrlSaldo.Name = "grCtrlSaldo";
            this.grCtrlSaldo.Size = new System.Drawing.Size(289, 171);
            this.grCtrlSaldo.TabIndex = 11;
            this.grCtrlSaldo.Text = "1011_pnSaldos";
            // 
            // BalanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 333);
            this.Controls.Add(this.grCtrlSaldo);
            this.Controls.Add(this.lblCuenta);
            this.Controls.Add(this.lblDoc);
            this.Controls.Add(this.lblTercero);
            this.Controls.Add(this.lblPeriod);
            this.Controls.Add(this.lblCuentaTit);
            this.Controls.Add(this.lblTerceroTit);
            this.Controls.Add(this.lblDocTit);
            this.Controls.Add(this.lblPeriodTit);
            this.Controls.Add(this.lblTitle);
            this.Name = "BalanceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "1011";
            ((System.ComponentModel.ISupportInitialize)(this.grCtrlSaldo)).EndInit();
            this.grCtrlSaldo.ResumeLayout(false);
            this.grCtrlSaldo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox lblMessage;
        private System.Windows.Forms.Label lblPeriodTit;
        private System.Windows.Forms.Label lblDocTit;
        private System.Windows.Forms.Label lblTerceroTit;
        private System.Windows.Forms.Label lblCuentaTit;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Label lblTercero;
        private System.Windows.Forms.Label lblDoc;
        private System.Windows.Forms.Label lblCuenta;
        private DevExpress.XtraEditors.GroupControl grCtrlSaldo;
    }
}