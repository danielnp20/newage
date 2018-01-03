namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class IncorporaDescuento
    {

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.masterPagaduria = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.dtPeriodo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.masterNovedad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.lkp_EstadoCruce = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.lkp_Seleccion = new DevExpress.XtraEditors.LookUpEdit();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.richText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_EstadoCruce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Seleccion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryDocuments
            // 
            this.RepositoryDocuments.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.richText,
            this.riPopup,
            this.editChkBox,
            this.editSpin,
            this.editSpin4,
            this.editLink,
            this.editSpinPorc,
            this.editBtnGrid,
            this.editSpin0});
            // 
            // richEditControl
            // 
            this.richEditControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            // 
            // editSpin
            // 
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin4
            // 
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // cmbUserTareas
            // 
            this.cmbUserTareas.Location = new System.Drawing.Point(127, 13);
            this.cmbUserTareas.Margin = new System.Windows.Forms.Padding(2);
            this.cmbUserTareas.Size = new System.Drawing.Size(113, 22);
            // 
            // lblUserTareas
            // 
            this.lblUserTareas.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserTareas.Location = new System.Drawing.Point(34, 16);
            this.lblUserTareas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lkp_Seleccion);
            this.grpboxHeader.Controls.Add(this.label3);
            this.grpboxHeader.Controls.Add(this.lkp_EstadoCruce);
            this.grpboxHeader.Controls.Add(this.label2);
            this.grpboxHeader.Controls.Add(this.txtLibranza);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.masterNovedad);
            this.grpboxHeader.Controls.Add(this.dtFecha);
            this.grpboxHeader.Controls.Add(this.label1);
            this.grpboxHeader.Controls.Add(this.dtPeriodo);
            this.grpboxHeader.Controls.Add(this.lblPeriodo);
            this.grpboxHeader.Controls.Add(this.masterPagaduria);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(2);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(2);
            this.grpboxHeader.Size = new System.Drawing.Size(1043, 73);
            this.grpboxHeader.Visible = true;
            this.grpboxHeader.Controls.SetChildIndex(this.masterPagaduria, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.chkSeleccionar, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.cmbUserTareas, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblPeriodo, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.dtPeriodo, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.label1, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.dtFecha, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.masterNovedad, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lblLibranza, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.txtLibranza, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.label2, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lkp_EstadoCruce, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.label3, 0);
            this.grpboxHeader.Controls.SetChildIndex(this.lkp_Seleccion, 0);
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.Location = new System.Drawing.Point(23, 14);
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P";
            // 
            // gcDetails
            // 
            this.gcDetails.Size = new System.Drawing.Size(1047, 85);
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "n0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // masterCentroPago
            // 
            this.masterPagaduria.BackColor = System.Drawing.Color.Transparent;
            this.masterPagaduria.Filtros = null;
            this.masterPagaduria.Location = new System.Drawing.Point(470, 12);
            this.masterPagaduria.Margin = new System.Windows.Forms.Padding(9);
            this.masterPagaduria.Name = "masterCentroPago";
            this.masterPagaduria.Size = new System.Drawing.Size(307, 25);
            this.masterPagaduria.TabIndex = 2;
            this.masterPagaduria.Value = "";
            this.masterPagaduria.Leave += new System.EventHandler(this.FilterData);
            // 
            // lblPlazo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriodo.Location = new System.Drawing.Point(20, 16);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(87, 14);
            this.lblPeriodo.TabIndex = 1;
            this.lblPeriodo.Text = "1005_lblPeriod";
            // 
            // dtPeriodo
            // 
            this.dtPeriodo.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriodo.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriodo.EnabledControl = true;
            this.dtPeriodo.ExtraPeriods = 0;
            this.dtPeriodo.Location = new System.Drawing.Point(119, 14);
            this.dtPeriodo.Margin = new System.Windows.Forms.Padding(6);
            this.dtPeriodo.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriodo.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriodo.Name = "dtPeriodo";
            this.dtPeriodo.Size = new System.Drawing.Size(130, 18);
            this.dtPeriodo.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(258, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "1005_lblFecha";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Location = new System.Drawing.Point(349, 15);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            this.dtFecha.TabIndex = 7;
            // 
            // masterNovedad
            // 
            this.masterNovedad.BackColor = System.Drawing.Color.Transparent;
            this.masterNovedad.Filtros = null;
            this.masterNovedad.Location = new System.Drawing.Point(23, 44);
            this.masterNovedad.Margin = new System.Windows.Forms.Padding(9);
            this.masterNovedad.Name = "masterNovedad";
            this.masterNovedad.Size = new System.Drawing.Size(307, 25);
            this.masterNovedad.TabIndex = 8;
            this.masterNovedad.Value = "";
            this.masterNovedad.Leave += new System.EventHandler(this.FilterData);
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(908, 14);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(121, 22);
            this.txtLibranza.TabIndex = 10;
            this.txtLibranza.Leave += new System.EventHandler(this.FilterData);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(789, 17);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(92, 14);
            this.lblLibranza.TabIndex = 9;
            this.lblLibranza.Text = "32551_Libranza";
            // 
            // lkp_EstadoCruce
            // 
            this.lkp_EstadoCruce.Location = new System.Drawing.Point(471, 48);
            this.lkp_EstadoCruce.Name = "lkp_EstadoCruce";
            this.lkp_EstadoCruce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_EstadoCruce.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", 20, "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 60, "Descriptivo")});
            this.lkp_EstadoCruce.Properties.DisplayMember = "Value";
            this.lkp_EstadoCruce.Properties.NullText = " ";
            this.lkp_EstadoCruce.Properties.ValueMember = "Key";
            this.lkp_EstadoCruce.Size = new System.Drawing.Size(117, 20);
            this.lkp_EstadoCruce.TabIndex = 28;
            this.lkp_EstadoCruce.Leave += new System.EventHandler(this.FilterData);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(352, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 14);
            this.label2.TabIndex = 29;
            this.label2.Text = "173_EstadoCruce";
            // 
            // lkp_Seleccion
            // 
            this.lkp_Seleccion.Location = new System.Drawing.Point(721, 48);
            this.lkp_Seleccion.Name = "lkp_Seleccion";
            this.lkp_Seleccion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_Seleccion.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 60, "Descripción")});
            this.lkp_Seleccion.Properties.DisplayMember = "Value";
            this.lkp_Seleccion.Properties.NullText = " ";
            this.lkp_Seleccion.Properties.ValueMember = "Key";
            this.lkp_Seleccion.Size = new System.Drawing.Size(117, 20);
            this.lkp_Seleccion.TabIndex = 30;
            this.lkp_Seleccion.Leave += new System.EventHandler(this.FilterData);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(630, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 14);
            this.label3.TabIndex = 31;
            this.label3.Text = "173_Seleccion";
            // 
            // IncorporaDescuento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 493);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "IncorporaDescuento";
            this.Text = "32561_AprobacionGiro";
            ((System.ComponentModel.ISupportInitialize)(this.richText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_EstadoCruce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_Seleccion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterPagaduria;
        private System.Windows.Forms.Label lblPeriodo;
        protected ControlsUC.uc_PeriodoEdit dtPeriodo;
        private System.Windows.Forms.Label label1;
        protected DevExpress.XtraEditors.DateEdit dtFecha;
        private ControlsUC.uc_MasterFind masterNovedad;
        private System.Windows.Forms.TextBox txtLibranza;
        private System.Windows.Forms.Label lblLibranza;
        private DevExpress.XtraEditors.LookUpEdit lkp_Seleccion;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.LookUpEdit lkp_EstadoCruce;
        private System.Windows.Forms.Label label2;

    }
}