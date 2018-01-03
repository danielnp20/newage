﻿namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalTrasladoInv
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editBtnMvto = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editBtnDetail = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gcDetalle = new DevExpress.XtraGrid.GridControl();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.tpTrasladoNormal = new DevExpress.XtraTab.XtraTabPage();
            this.tpTrasladoSolicitud = new DevExpress.XtraTab.XtraTabPage();
            this.masterBodegaIni = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbTipoTraslado = new DevExpress.XtraEditors.LookUpEdit();
            this.lblTipoSolicitud = new DevExpress.XtraEditors.LabelControl();
            this.masterProyectoOrigen = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblB2 = new System.Windows.Forms.Label();
            this.lblB1 = new System.Windows.Forms.Label();
            this.masterBodegaFin = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkSelect = new DevExpress.XtraEditors.CheckEdit();
            this.masterProyectoDestino = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tpTrasladoNormal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoTraslado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelect.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editValue,
            this.editBtnMvto,
            this.editBtnDetail});
            // 
            // editValue
            // 
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c4";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editBtnMvto
            // 
            this.editBtnMvto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver Movimiento", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.editBtnMvto.Name = "editBtnMvto";
            this.editBtnMvto.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnMvto.ValidateOnEnterKey = true;
            // 
            // editBtnDetail
            // 
            this.editBtnDetail.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver detalle", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "Ver detalle de la referencia", null, null, true)});
            this.editBtnDetail.Name = "editBtnDetail";
            this.editBtnDetail.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnDetail.ValidateOnEnterKey = true;
            // 
            // gcDetalle
            // 
            this.gcDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetalle.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetalle.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetalle.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetalle.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetalle.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDetalle.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetalle.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDetalle.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetalle.Location = new System.Drawing.Point(0, 0);
            this.gcDetalle.LookAndFeel.SkinName = "Dark Side";
            this.gcDetalle.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetalle.MainView = this.gvDetalle;
            this.gcDetalle.Margin = new System.Windows.Forms.Padding(4);
            this.gcDetalle.Name = "gcDetalle";
            this.gcDetalle.Size = new System.Drawing.Size(1032, 252);
            this.gcDetalle.TabIndex = 2;
            this.gcDetalle.UseEmbeddedNavigator = true;
            this.gcDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalle});
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetalle.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.ViewCaption.Font = new System.Drawing.Font("Arial Narrow", 8F);
            this.gvDetalle.Appearance.ViewCaption.Options.UseFont = true;
            this.gvDetalle.GridControl = this.gcDetalle;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsDetail.EnableMasterViewMode = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ShowAutoFilterRow = true;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvReferencia_RowClick);
            this.gvDetalle.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvReferencia_ShowingEditor);
            this.gvDetalle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvReferencias_FocusedRowChanged);
            this.gvDetalle.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvReferencia_CellValueChanged);
            this.gvDetalle.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvReferencia_BeforeLeaveRow);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvReferencias_CustomUnboundColumnData);
            this.gvDetalle.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvReferencia_CustomColumnDisplayText);
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // btnAccept
            // 
            this.btnAccept.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnAccept.Appearance.Options.UseFont = true;
            this.btnAccept.Location = new System.Drawing.Point(881, 328);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(81, 23);
            this.btnAccept.TabIndex = 13;
            this.btnAccept.Text = "Aceptar";
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(966, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 70);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedTabPage = this.tpTrasladoNormal;
            this.tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.tabControl.Size = new System.Drawing.Size(1038, 258);
            this.tabControl.TabIndex = 52;
            this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpTrasladoNormal,
            this.tpTrasladoSolicitud});
            // 
            // tpTrasladoNormal
            // 
            this.tpTrasladoNormal.Controls.Add(this.gcDetalle);
            this.tpTrasladoNormal.Name = "tpTrasladoNormal";
            this.tpTrasladoNormal.Size = new System.Drawing.Size(1032, 252);
            this.tpTrasladoNormal.Text = "Traslado";
            // 
            // tpTrasladoSolicitud
            // 
            this.tpTrasladoSolicitud.Name = "tpTrasladoSolicitud";
            this.tpTrasladoSolicitud.Size = new System.Drawing.Size(1032, 252);
            this.tpTrasladoSolicitud.Text = "Traslado Solicitud Prov";
            // 
            // masterBodegaIni
            // 
            this.masterBodegaIni.BackColor = System.Drawing.Color.Transparent;
            this.masterBodegaIni.Filtros = null;
            this.masterBodegaIni.Location = new System.Drawing.Point(19, 17);
            this.masterBodegaIni.Name = "masterBodegaIni";
            this.masterBodegaIni.Size = new System.Drawing.Size(299, 25);
            this.masterBodegaIni.TabIndex = 53;
            this.masterBodegaIni.Value = "";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.masterProyectoDestino);
            this.panelControl1.Controls.Add(this.cmbTipoTraslado);
            this.panelControl1.Controls.Add(this.lblTipoSolicitud);
            this.panelControl1.Controls.Add(this.masterProyectoOrigen);
            this.panelControl1.Controls.Add(this.lblB2);
            this.panelControl1.Controls.Add(this.lblB1);
            this.panelControl1.Controls.Add(this.masterBodegaFin);
            this.panelControl1.Controls.Add(this.masterBodegaIni);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1038, 70);
            this.panelControl1.TabIndex = 54;
            // 
            // cmbTipoTraslado
            // 
            this.cmbTipoTraslado.Location = new System.Drawing.Point(785, 19);
            this.cmbTipoTraslado.Name = "cmbTipoTraslado";
            this.cmbTipoTraslado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoTraslado.Size = new System.Drawing.Size(177, 20);
            this.cmbTipoTraslado.TabIndex = 98;
            this.cmbTipoTraslado.EditValueChanged += new System.EventHandler(this.cmbTipoTraslado_EditValueChanged);
            // 
            // lblTipoSolicitud
            // 
            this.lblTipoSolicitud.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblTipoSolicitud.Location = new System.Drawing.Point(660, 22);
            this.lblTipoSolicitud.Name = "lblTipoSolicitud";
            this.lblTipoSolicitud.Size = new System.Drawing.Size(123, 13);
            this.lblTipoSolicitud.TabIndex = 97;
            this.lblTipoSolicitud.Text = "Tipo Traslado o Salida";
            // 
            // masterProyectoOrigen
            // 
            this.masterProyectoOrigen.BackColor = System.Drawing.Color.Transparent;
            this.masterProyectoOrigen.Filtros = null;
            this.masterProyectoOrigen.Location = new System.Drawing.Point(18, 42);
            this.masterProyectoOrigen.Name = "masterProyectoOrigen";
            this.masterProyectoOrigen.Size = new System.Drawing.Size(299, 25);
            this.masterProyectoOrigen.TabIndex = 58;
            this.masterProyectoOrigen.Value = "";
            this.masterProyectoOrigen.Visible = false;
            // 
            // lblB2
            // 
            this.lblB2.AutoSize = true;
            this.lblB2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblB2.Location = new System.Drawing.Point(330, 23);
            this.lblB2.Name = "lblB2";
            this.lblB2.Size = new System.Drawing.Size(124, 14);
            this.lblB2.TabIndex = 57;
            this.lblB2.Text = "1022_lblBodegaFin";
            // 
            // lblB1
            // 
            this.lblB1.AutoSize = true;
            this.lblB1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblB1.Location = new System.Drawing.Point(14, 23);
            this.lblB1.Name = "lblB1";
            this.lblB1.Size = new System.Drawing.Size(123, 14);
            this.lblB1.TabIndex = 56;
            this.lblB1.Text = "1022_lblBodegaIni";
            // 
            // masterBodegaFin
            // 
            this.masterBodegaFin.BackColor = System.Drawing.Color.Transparent;
            this.masterBodegaFin.Filtros = null;
            this.masterBodegaFin.Location = new System.Drawing.Point(334, 17);
            this.masterBodegaFin.Name = "masterBodegaFin";
            this.masterBodegaFin.Size = new System.Drawing.Size(299, 25);
            this.masterBodegaFin.TabIndex = 54;
            this.masterBodegaFin.Value = "";
            // 
            // chkSelect
            // 
            this.chkSelect.Location = new System.Drawing.Point(6, 329);
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.chkSelect.Properties.Appearance.Options.UseFont = true;
            this.chkSelect.Properties.Caption = "1022_chkSelectAll";
            this.chkSelect.Size = new System.Drawing.Size(147, 19);
            this.chkSelect.TabIndex = 81;
            this.chkSelect.CheckedChanged += new System.EventHandler(this.chkSelect_CheckedChanged);
            // 
            // masterProyectoDestino
            // 
            this.masterProyectoDestino.BackColor = System.Drawing.Color.Transparent;
            this.masterProyectoDestino.Filtros = null;
            this.masterProyectoDestino.Location = new System.Drawing.Point(334, 42);
            this.masterProyectoDestino.Name = "masterProyectoDestino";
            this.masterProyectoDestino.Size = new System.Drawing.Size(299, 25);
            this.masterProyectoDestino.TabIndex = 99;
            this.masterProyectoDestino.Value = "";
            this.masterProyectoDestino.Visible = false;
            // 
            // ModalTrasladoInv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1038, 353);
            this.Controls.Add(this.chkSelect);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalTrasladoInv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTraslados";
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tpTrasladoNormal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoTraslado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelect.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnMvto;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gcDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraEditors.SimpleButton btnAccept;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraTab.XtraTabControl tabControl;
        private DevExpress.XtraTab.XtraTabPage tpTrasladoNormal;
        private DevExpress.XtraTab.XtraTabPage tpTrasladoSolicitud;
        private ControlsUC.uc_MasterFind masterBodegaIni;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private ControlsUC.uc_MasterFind masterBodegaFin;
        private DevExpress.XtraEditors.CheckEdit chkSelect;
        private System.Windows.Forms.Label lblB2;
        private System.Windows.Forms.Label lblB1;
        private ControlsUC.uc_MasterFind masterProyectoOrigen;
        private DevExpress.XtraEditors.LookUpEdit cmbTipoTraslado;
        private DevExpress.XtraEditors.LabelControl lblTipoSolicitud;
        private ControlsUC.uc_MasterFind masterProyectoDestino;
    }
}