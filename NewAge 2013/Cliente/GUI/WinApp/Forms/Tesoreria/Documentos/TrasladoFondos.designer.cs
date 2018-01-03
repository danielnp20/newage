using NewAge.Cliente.GUI.WinApp.ControlsUC;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class TrasladoFondos
    {
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
        protected virtual void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrasladoFondos));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.pnlMainContainer = new DevExpress.XtraEditors.PanelControl();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gctrlBody = new DevExpress.XtraEditors.GroupControl();
            this.dtPeriod = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_PeriodoEdit();
            this.lblPeriod = new DevExpress.XtraEditors.LabelControl();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.chkGenerarOrdenPago = new System.Windows.Forms.CheckBox();
            this.lblValor = new DevExpress.XtraEditors.LabelControl();
            this.txtTasaCambio = new DevExpress.XtraEditors.TextEdit();
            this.lblTasaCambio = new DevExpress.XtraEditors.LabelControl();
            this.lblFecha = new DevExpress.XtraEditors.LabelControl();
            this.dtFecha = new DevExpress.XtraEditors.DateEdit();
            this.lblBandoDestino = new DevExpress.XtraEditors.LabelControl();
            this.masterCuentaDestino = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblBancoOrigen = new DevExpress.XtraEditors.LabelControl();
            this.masterCuentaOrigen = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpin4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.editCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editValue4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).BeginInit();
            this.pnlMainContainer.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            this.pnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gctrlBody)).BeginInit();
            this.gctrlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.tlSeparatorPanel);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.FireScrollEventOnMouseWheel = true;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Margin = new System.Windows.Forms.Padding(7);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Size = new System.Drawing.Size(668, 296);
            this.pnlMainContainer.TabIndex = 46;
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(2, 2);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.453958F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98.54604F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(664, 292);
            this.tlSeparatorPanel.TabIndex = 54;
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gctrlBody);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(13, 7);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(640, 275);
            this.pnlGrids.TabIndex = 113;
            // 
            // gctrlBody
            // 
            this.gctrlBody.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.gctrlBody.Appearance.Options.UseBackColor = true;
            this.gctrlBody.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.gctrlBody.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.gctrlBody.AppearanceCaption.Options.UseBackColor = true;
            this.gctrlBody.AppearanceCaption.Options.UseFont = true;
            this.gctrlBody.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.gctrlBody.Controls.Add(this.dtPeriod);
            this.gctrlBody.Controls.Add(this.lblPeriod);
            this.gctrlBody.Controls.Add(this.txtValor);
            this.gctrlBody.Controls.Add(this.chkGenerarOrdenPago);
            this.gctrlBody.Controls.Add(this.lblValor);
            this.gctrlBody.Controls.Add(this.txtTasaCambio);
            this.gctrlBody.Controls.Add(this.lblTasaCambio);
            this.gctrlBody.Controls.Add(this.lblFecha);
            this.gctrlBody.Controls.Add(this.dtFecha);
            this.gctrlBody.Controls.Add(this.lblBandoDestino);
            this.gctrlBody.Controls.Add(this.masterCuentaDestino);
            this.gctrlBody.Controls.Add(this.lblBancoOrigen);
            this.gctrlBody.Controls.Add(this.masterCuentaOrigen);
            this.gctrlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gctrlBody.Location = new System.Drawing.Point(0, 0);
            this.gctrlBody.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gctrlBody.Name = "gctrlBody";
            this.gctrlBody.Size = new System.Drawing.Size(640, 275);
            this.gctrlBody.TabIndex = 0;
            // 
            // dtPeriod
            // 
            this.dtPeriod.BackColor = System.Drawing.Color.Transparent;
            this.dtPeriod.DateTime = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtPeriod.Enabled = false;
            this.dtPeriod.EnabledControl = true;
            this.dtPeriod.ExtraPeriods = 0;
            this.dtPeriod.Location = new System.Drawing.Point(109, 35);
            this.dtPeriod.MaxValue = new System.DateTime(((long)(0)));
            this.dtPeriod.MinValue = new System.DateTime(((long)(0)));
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(130, 18);
            this.dtPeriod.TabIndex = 125;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.Location = new System.Drawing.Point(47, 38);
            this.lblPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(80, 14);
            this.lblPeriod.TabIndex = 126;
            this.lblPeriod.Text = "1005_lblPeriod";
            // 
            // txtValor
            // 
            this.txtValor.EditValue = "";
            this.txtValor.Location = new System.Drawing.Point(159, 192);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtValor.Properties.Appearance.Options.UseFont = true;
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.Mask.EditMask = "c";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Properties.NullText = "0";
            this.txtValor.Size = new System.Drawing.Size(103, 20);
            this.txtValor.TabIndex = 3;
            // 
            // chkGenerarOrdenPago
            // 
            this.chkGenerarOrdenPago.AutoSize = true;
            this.chkGenerarOrdenPago.Location = new System.Drawing.Point(47, 229);
            this.chkGenerarOrdenPago.Margin = new System.Windows.Forms.Padding(2);
            this.chkGenerarOrdenPago.Name = "chkGenerarOrdenPago";
            this.chkGenerarOrdenPago.Size = new System.Drawing.Size(153, 17);
            this.chkGenerarOrdenPago.TabIndex = 4;
            this.chkGenerarOrdenPago.Text = "33_chkGenerarOrdenPago";
            this.chkGenerarOrdenPago.UseVisualStyleBackColor = true;
            // 
            // lblValor
            // 
            this.lblValor.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblValor.Location = new System.Drawing.Point(47, 195);
            this.lblValor.Margin = new System.Windows.Forms.Padding(4);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(59, 14);
            this.lblValor.TabIndex = 118;
            this.lblValor.Text = "33_lblValor";
            // 
            // txtTasaCambio
            // 
            this.txtTasaCambio.Location = new System.Drawing.Point(397, 192);
            this.txtTasaCambio.Name = "txtTasaCambio";
            this.txtTasaCambio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTasaCambio.Properties.Mask.EditMask = "c";
            this.txtTasaCambio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTasaCambio.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTasaCambio.Properties.ReadOnly = true;
            this.txtTasaCambio.Size = new System.Drawing.Size(95, 20);
            this.txtTasaCambio.TabIndex = 117;
            // 
            // lblTasaCambio
            // 
            this.lblTasaCambio.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTasaCambio.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblTasaCambio.Location = new System.Drawing.Point(281, 195);
            this.lblTasaCambio.Margin = new System.Windows.Forms.Padding(4);
            this.lblTasaCambio.Name = "lblTasaCambio";
            this.lblTasaCambio.Size = new System.Drawing.Size(96, 14);
            this.lblTasaCambio.TabIndex = 116;
            this.lblTasaCambio.Text = "33_lblTasaCambio";
            // 
            // lblFecha
            // 
            this.lblFecha.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblFecha.Location = new System.Drawing.Point(47, 153);
            this.lblFecha.Margin = new System.Windows.Forms.Padding(4);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(64, 14);
            this.lblFecha.TabIndex = 115;
            this.lblFecha.Text = "33_lblFecha";
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = null;
            this.dtFecha.Location = new System.Drawing.Point(159, 151);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(2);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFecha.Size = new System.Drawing.Size(103, 20);
            this.dtFecha.TabIndex = 2;
            this.dtFecha.DateTimeChanged += new System.EventHandler(this.dtFecha_DateTimeChanged);
            // 
            // lblBandoDestino
            // 
            this.lblBandoDestino.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBandoDestino.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblBandoDestino.Location = new System.Drawing.Point(47, 110);
            this.lblBandoDestino.Margin = new System.Windows.Forms.Padding(4);
            this.lblBandoDestino.Name = "lblBandoDestino";
            this.lblBandoDestino.Size = new System.Drawing.Size(106, 14);
            this.lblBandoDestino.TabIndex = 113;
            this.lblBandoDestino.Text = "33_lblBancoDestino";
            // 
            // masterCuentaDestino
            // 
            this.masterCuentaDestino.BackColor = System.Drawing.Color.Transparent;
            this.masterCuentaDestino.Filtros = null;
            this.masterCuentaDestino.Location = new System.Drawing.Point(59, 106);
            this.masterCuentaDestino.Margin = new System.Windows.Forms.Padding(4);
            this.masterCuentaDestino.Name = "masterCuentaDestino";
            this.masterCuentaDestino.Size = new System.Drawing.Size(291, 25);
            this.masterCuentaDestino.TabIndex = 1;
            this.masterCuentaDestino.Value = "";
            this.masterCuentaDestino.Leave += new System.EventHandler(this.masterCuentaOrigenDestino_Leave);
            // 
            // lblBancoOrigen
            // 
            this.lblBancoOrigen.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBancoOrigen.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblBancoOrigen.Location = new System.Drawing.Point(47, 70);
            this.lblBancoOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.lblBancoOrigen.Name = "lblBancoOrigen";
            this.lblBancoOrigen.Size = new System.Drawing.Size(101, 14);
            this.lblBancoOrigen.TabIndex = 111;
            this.lblBancoOrigen.Text = "33_lblBancoOrigen";
            // 
            // masterCuentaOrigen
            // 
            this.masterCuentaOrigen.BackColor = System.Drawing.Color.Transparent;
            this.masterCuentaOrigen.Filtros = null;
            this.masterCuentaOrigen.Location = new System.Drawing.Point(59, 66);
            this.masterCuentaOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.masterCuentaOrigen.Name = "masterCuentaOrigen";
            this.masterCuentaOrigen.Size = new System.Drawing.Size(291, 25);
            this.masterCuentaOrigen.TabIndex = 0;
            this.masterCuentaOrigen.Value = "";
            this.masterCuentaOrigen.Leave += new System.EventHandler(this.masterCuentaOrigenDestino_Leave);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editBtnGrid,
            this.editCmb,
            this.editText,
            this.editSpin,
            this.editSpin4,
            this.editDate,
            this.editCheck,
            this.editValue,
            this.editValue4});
            // 
            // editChkBox
            // 
            this.editChkBox.Caption = "";
            this.editChkBox.DisplayValueChecked = "True";
            this.editChkBox.DisplayValueUnchecked = "False";
            this.editChkBox.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editChkBox.Name = "editChkBox";
            this.editChkBox.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("editBtnGrid.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            // 
            // editCmb
            // 
            this.editCmb.Name = "editCmb";
            // 
            // editText
            // 
            this.editText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            this.editText.Name = "editText";
            // 
            // editSpin
            // 
            this.editSpin.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin.Mask.EditMask = "c";
            this.editSpin.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin.Name = "editSpin";
            // 
            // editSpin4
            // 
            this.editSpin4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpin4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin4.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpin4.Mask.EditMask = "c4";
            this.editSpin4.Mask.UseMaskAsDisplayFormat = true;
            this.editSpin4.Name = "editSpin4";
            // 
            // editDate
            // 
            this.editDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            this.editDate.Name = "editDate";
            // 
            // editCheck
            // 
            this.editCheck.Name = "editCheck";
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editValue4
            // 
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            this.editValue4.Name = "editValue4";
            // 
            // TrasladoFondos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 296);
            this.Controls.Add(this.pnlMainContainer);
            this.Name = "TrasladoFondos";
            this.Text = "33";
            ((System.ComponentModel.ISupportInitialize)(this.pnlMainContainer)).EndInit();
            this.pnlMainContainer.ResumeLayout(false);
            this.tlSeparatorPanel.ResumeLayout(false);
            this.pnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gctrlBody)).EndInit();
            this.gctrlBody.ResumeLayout(false);
            this.gctrlBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTasaCambio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlMainContainer;
        protected System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        protected DevExpress.XtraEditors.GroupControl gctrlBody;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemDateEdit editDate;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue4;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editCheck;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editText;
        protected System.Windows.Forms.Panel pnlGrids;
        private System.ComponentModel.IContainer components;
        private DevExpress.XtraEditors.LabelControl lblBancoOrigen;
        private uc_MasterFind masterCuentaOrigen;
        private DevExpress.XtraEditors.LabelControl lblBandoDestino;
        private uc_MasterFind masterCuentaDestino;
        private DevExpress.XtraEditors.LabelControl lblFecha;
        private DevExpress.XtraEditors.DateEdit dtFecha;
        private DevExpress.XtraEditors.LabelControl lblTasaCambio;
        private System.Windows.Forms.CheckBox chkGenerarOrdenPago;
        private DevExpress.XtraEditors.LabelControl lblValor;
        private DevExpress.XtraEditors.TextEdit txtTasaCambio;
        private DevExpress.XtraEditors.TextEdit txtValor;
        protected uc_PeriodoEdit dtPeriod;
        private DevExpress.XtraEditors.LabelControl lblPeriod;       
    }
}