namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaSerial
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
            this.gbDetail = new DevExpress.XtraEditors.GroupControl();
            this.gcDetail = new DevExpress.XtraGrid.GridControl();
            this.gvDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gb_Header = new System.Windows.Forms.GroupBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.uc_MF_Cliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_MF_Referencia = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.uc_MF_Bodega = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txt_Serial = new System.Windows.Forms.TextBox();
            this.lbl_Serial = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).BeginInit();
            this.gbDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            this.gb_Header.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDetail
            // 
            this.gbDetail.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbDetail.AppearanceCaption.Options.UseFont = true;
            this.gbDetail.AppearanceCaption.Options.UseTextOptions = true;
            this.gbDetail.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbDetail.Controls.Add(this.gcDetail);
            this.gbDetail.Location = new System.Drawing.Point(12, 124);
            this.gbDetail.Name = "gbDetail";
            this.gbDetail.Size = new System.Drawing.Size(864, 363);
            this.gbDetail.TabIndex = 9;
            // 
            // gcDetail
            // 
            this.gcDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetail.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetail.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetail.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetail.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetail.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetail.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDetail.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetail.Location = new System.Drawing.Point(2, 24);
            this.gcDetail.LookAndFeel.SkinName = "Dark Side";
            this.gcDetail.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetail.MainView = this.gvDetail;
            this.gcDetail.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetail.Name = "gcDetail";
            this.gcDetail.Size = new System.Drawing.Size(860, 337);
            this.gcDetail.TabIndex = 1;
            this.gcDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetail});
            // 
            // gvDetail
            // 
            this.gvDetail.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetail.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetail.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetail.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetail.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetail.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetail.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetail.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetail.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetail.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetail.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetail.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetail.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetail.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetail.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetail.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetail.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetail.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetail.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetail.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetail.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetail.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetail.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetail.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetail.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetail.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetail.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetail.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetail.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetail.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetail.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetail.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.Row.Options.UseBackColor = true;
            this.gvDetail.Appearance.Row.Options.UseForeColor = true;
            this.gvDetail.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetail.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetail.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetail.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetail.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetail.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetail.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetail.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetail.GridControl = this.gcDetail;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetail.OptionsCustomization.AllowFilter = false;
            this.gvDetail.OptionsCustomization.AllowSort = false;
            this.gvDetail.OptionsMenu.EnableColumnMenu = false;
            this.gvDetail.OptionsMenu.EnableFooterMenu = false;
            this.gvDetail.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetail.OptionsView.ShowGroupPanel = false;
            this.gvDetail.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetail_CustomUnboundColumnData);
            // 
            // gb_Header
            // 
            this.gb_Header.Controls.Add(this.btnBuscar);
            this.gb_Header.Controls.Add(this.uc_MF_Cliente);
            this.gb_Header.Controls.Add(this.uc_MF_Referencia);
            this.gb_Header.Controls.Add(this.uc_MF_Bodega);
            this.gb_Header.Controls.Add(this.txt_Serial);
            this.gb_Header.Controls.Add(this.lbl_Serial);
            this.gb_Header.Location = new System.Drawing.Point(14, 12);
            this.gb_Header.Name = "gb_Header";
            this.gb_Header.Size = new System.Drawing.Size(862, 106);
            this.gb_Header.TabIndex = 13;
            this.gb_Header.TabStop = false;
            this.gb_Header.Text = "26313_gb_Seleccionar";
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(764, 77);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(92, 23);
            this.btnBuscar.TabIndex = 5;
            this.btnBuscar.Text = "26313_btn_Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // uc_MF_Cliente
            // 
            this.uc_MF_Cliente.BackColor = System.Drawing.Color.Transparent;
            this.uc_MF_Cliente.Filtros = null;
            this.uc_MF_Cliente.Location = new System.Drawing.Point(467, 51);
            this.uc_MF_Cliente.Name = "uc_MF_Cliente";
            this.uc_MF_Cliente.Size = new System.Drawing.Size(291, 25);
            this.uc_MF_Cliente.TabIndex = 4;
            this.uc_MF_Cliente.Value = "";
            // 
            // uc_MF_Referencia
            // 
            this.uc_MF_Referencia.BackColor = System.Drawing.Color.Transparent;
            this.uc_MF_Referencia.Filtros = null;
            this.uc_MF_Referencia.Location = new System.Drawing.Point(85, 51);
            this.uc_MF_Referencia.Name = "uc_MF_Referencia";
            this.uc_MF_Referencia.Size = new System.Drawing.Size(291, 25);
            this.uc_MF_Referencia.TabIndex = 3;
            this.uc_MF_Referencia.Value = "";
            // 
            // uc_MF_Bodega
            // 
            this.uc_MF_Bodega.BackColor = System.Drawing.Color.Transparent;
            this.uc_MF_Bodega.Filtros = null;
            this.uc_MF_Bodega.Location = new System.Drawing.Point(467, 20);
            this.uc_MF_Bodega.Name = "uc_MF_Bodega";
            this.uc_MF_Bodega.Size = new System.Drawing.Size(291, 25);
            this.uc_MF_Bodega.TabIndex = 2;
            this.uc_MF_Bodega.Value = "";
            // 
            // txt_Serial
            // 
            this.txt_Serial.Location = new System.Drawing.Point(186, 24);
            this.txt_Serial.Name = "txt_Serial";
            this.txt_Serial.Size = new System.Drawing.Size(100, 20);
            this.txt_Serial.TabIndex = 1;
            // 
            // lbl_Serial
            // 
            this.lbl_Serial.AutoSize = true;
            this.lbl_Serial.Location = new System.Drawing.Point(82, 31);
            this.lbl_Serial.Name = "lbl_Serial";
            this.lbl_Serial.Size = new System.Drawing.Size(85, 13);
            this.lbl_Serial.TabIndex = 0;
            this.lbl_Serial.Text = "26313_lbl_Serial";
            // 
            // ConsultaSerial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 513);
            this.Controls.Add(this.gb_Header);
            this.Controls.Add(this.gbDetail);
            this.MaximizeBox = false;
            this.Name = "ConsultaSerial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConsultaSerial";
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).EndInit();
            this.gbDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            this.gb_Header.ResumeLayout(false);
            this.gb_Header.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gbDetail;
        private DevExpress.XtraGrid.GridControl gcDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetail;
        private System.Windows.Forms.GroupBox gb_Header;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_MF_Cliente;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_MF_Referencia;
        private Cliente.GUI.WinApp.ControlsUC.uc_MasterFind uc_MF_Bodega;
        private System.Windows.Forms.TextBox txt_Serial;
        private System.Windows.Forms.Label lbl_Serial;
        private System.Windows.Forms.Button btnBuscar;
    }
}