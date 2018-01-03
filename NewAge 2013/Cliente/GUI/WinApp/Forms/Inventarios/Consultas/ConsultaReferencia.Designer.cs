﻿namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaReferencia
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editBtnMvto = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editBtnDetail = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gbHeader = new DevExpress.XtraEditors.GroupControl();
            this.gcBodega = new DevExpress.XtraGrid.GridControl();
            this.gvBodega = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gbDetail = new DevExpress.XtraEditors.GroupControl();
            this.btnFilterChk = new DevExpress.XtraEditors.CheckButton();
            this.gcDetail = new DevExpress.XtraGrid.GridControl();
            this.gvDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.pnlFilter = new DevExpress.XtraEditors.PanelControl();
            this.lblRefProvee = new System.Windows.Forms.Label();
            this.txtRefProveedor = new System.Windows.Forms.TextBox();
            this.chkRefProvee = new DevExpress.XtraEditors.CheckEdit();
            this.masterReferencia = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkReferencia = new DevExpress.XtraEditors.CheckEdit();
            this.lblEstado = new System.Windows.Forms.Label();
            this.lblSerial = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.cmbEstado = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.chkEstado = new DevExpress.XtraEditors.CheckEdit();
            this.chkSerial = new DevExpress.XtraEditors.CheckEdit();
            this.masterParam2 = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkParam2 = new DevExpress.XtraEditors.CheckEdit();
            this.masterParam1 = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkParam1 = new DevExpress.XtraEditors.CheckEdit();
            this.masterEmpaqueInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkEmpaqueInv = new DevExpress.XtraEditors.CheckEdit();
            this.masterUnidadInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkUnidadInv = new DevExpress.XtraEditors.CheckEdit();
            this.masterMarcaInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterMaterialInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterSerieInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTipoInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterClaseInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterGrupoInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkMarcaInv = new DevExpress.XtraEditors.CheckEdit();
            this.chkMaterialInv = new DevExpress.XtraEditors.CheckEdit();
            this.chkSerieInv = new DevExpress.XtraEditors.CheckEdit();
            this.chkTipoInv = new DevExpress.XtraEditors.CheckEdit();
            this.chkClaseInv = new DevExpress.XtraEditors.CheckEdit();
            this.chkGrupoInv = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.gcLibroIFRS = new DevExpress.XtraEditors.GroupControl();
            this.lblCurrencyForIFRS = new System.Windows.Forms.Label();
            this.lblCurrencyLocIFRS = new System.Windows.Forms.Label();
            this.txtValorUnitMEIFRS = new DevExpress.XtraEditors.TextEdit();
            this.txtValorTotalMLIFRS = new DevExpress.XtraEditors.TextEdit();
            this.txtValorUnitMLIFRS = new DevExpress.XtraEditors.TextEdit();
            this.txtValorTotalMEIFRS = new DevExpress.XtraEditors.TextEdit();
            this.lblValorUnitIFRS = new DevExpress.XtraEditors.LabelControl();
            this.lblValorTotalIFRS = new DevExpress.XtraEditors.LabelControl();
            this.gcLibroFuncional = new DevExpress.XtraEditors.GroupControl();
            this.lblCurrencyFor = new System.Windows.Forms.Label();
            this.lblCurrencyLoc = new System.Windows.Forms.Label();
            this.txtValorUnitME = new DevExpress.XtraEditors.TextEdit();
            this.txtValorTotalML = new DevExpress.XtraEditors.TextEdit();
            this.txtValorUnitML = new DevExpress.XtraEditors.TextEdit();
            this.txtValorTotalME = new DevExpress.XtraEditors.TextEdit();
            this.lblValorUnit = new DevExpress.XtraEditors.LabelControl();
            this.lblValorTotal = new DevExpress.XtraEditors.LabelControl();
            this.scroll = new DevExpress.XtraEditors.XtraScrollableControl();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbHeader)).BeginInit();
            this.gbHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBodega)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBodega)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).BeginInit();
            this.gbDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFilter)).BeginInit();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkRefProvee.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkReferencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSerial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkParam2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkParam1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEmpaqueInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUnidadInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMarcaInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMaterialInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSerieInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTipoInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkClaseInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkGrupoInv.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLibroIFRS)).BeginInit();
            this.gcLibroIFRS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitMEIFRS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalMLIFRS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitMLIFRS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalMEIFRS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLibroFuncional)).BeginInit();
            this.gcLibroFuncional.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalME.Properties)).BeginInit();
            this.scroll.SuspendLayout();
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
            this.editValue.Mask.EditMask = "n2";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // editBtnMvto
            // 
            this.editBtnMvto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver Movimiento", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnMvto.Name = "editBtnMvto";
            this.editBtnMvto.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnMvto.ValidateOnEnterKey = true;
            this.editBtnMvto.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnMvto_ButtonClick);
            // 
            // editBtnDetail
            // 
            this.editBtnDetail.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver detalle", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "Ver detalle de la referencia", null, null, true)});
            this.editBtnDetail.Name = "editBtnDetail";
            this.editBtnDetail.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnDetail.ValidateOnEnterKey = true;
            this.editBtnDetail.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnDetail_ButtonClick);
            // 
            // gbHeader
            // 
            this.gbHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbHeader.AppearanceCaption.Options.UseFont = true;
            this.gbHeader.AppearanceCaption.Options.UseTextOptions = true;
            this.gbHeader.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbHeader.Controls.Add(this.gcBodega);
            this.gbHeader.Location = new System.Drawing.Point(12, 0);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(717, 213);
            this.gbHeader.TabIndex = 7;
            this.gbHeader.Text = "26311_gbBodega";
            // 
            // gcBodega
            // 
            this.gcBodega.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcBodega.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcBodega.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcBodega.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcBodega.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcBodega.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcBodega.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcBodega.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcBodega.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcBodega.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcBodega.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcBodega.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcBodega.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcBodega.Location = new System.Drawing.Point(2, 24);
            this.gcBodega.LookAndFeel.SkinName = "Dark Side";
            this.gcBodega.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcBodega.MainView = this.gvBodega;
            this.gcBodega.Margin = new System.Windows.Forms.Padding(4);
            this.gcBodega.Name = "gcBodega";
            this.gcBodega.Size = new System.Drawing.Size(713, 187);
            this.gcBodega.TabIndex = 1;
            this.gcBodega.UseEmbeddedNavigator = true;
            this.gcBodega.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBodega});
            // 
            // gvBodega
            // 
            this.gvBodega.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvBodega.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvBodega.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvBodega.Appearance.Empty.Options.UseBackColor = true;
            this.gvBodega.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvBodega.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvBodega.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBodega.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvBodega.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvBodega.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvBodega.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBodega.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvBodega.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvBodega.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvBodega.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvBodega.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvBodega.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvBodega.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvBodega.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvBodega.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvBodega.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvBodega.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvBodega.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvBodega.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvBodega.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvBodega.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvBodega.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvBodega.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBodega.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvBodega.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvBodega.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvBodega.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvBodega.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvBodega.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvBodega.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvBodega.Appearance.Row.Options.UseBackColor = true;
            this.gvBodega.Appearance.Row.Options.UseForeColor = true;
            this.gvBodega.Appearance.Row.Options.UseTextOptions = true;
            this.gvBodega.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvBodega.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvBodega.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvBodega.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvBodega.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvBodega.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvBodega.Appearance.VertLine.Options.UseBackColor = true;
            this.gvBodega.GridControl = this.gcBodega;
            this.gvBodega.Name = "gvBodega";
            this.gvBodega.OptionsBehavior.ReadOnly = true;
            this.gvBodega.OptionsCustomization.AllowColumnMoving = false;
            this.gvBodega.OptionsCustomization.AllowFilter = false;
            this.gvBodega.OptionsCustomization.AllowSort = false;
            this.gvBodega.OptionsDetail.EnableMasterViewMode = false;
            this.gvBodega.OptionsMenu.EnableColumnMenu = false;
            this.gvBodega.OptionsMenu.EnableFooterMenu = false;
            this.gvBodega.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvBodega.OptionsView.ShowAutoFilterRow = true;
            this.gvBodega.OptionsView.ShowGroupPanel = false;
            this.gvBodega.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvBodega_RowClick);
            this.gvBodega.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvHeader_FocusedRowChanged);
            this.gvBodega.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvHeader_CustomUnboundColumnData);
            // 
            // gbDetail
            // 
            this.gbDetail.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbDetail.AppearanceCaption.Options.UseFont = true;
            this.gbDetail.AppearanceCaption.Options.UseTextOptions = true;
            this.gbDetail.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbDetail.Controls.Add(this.btnFilterChk);
            this.gbDetail.Controls.Add(this.gcDetail);
            this.gbDetail.Location = new System.Drawing.Point(12, 219);
            this.gbDetail.Name = "gbDetail";
            this.gbDetail.Size = new System.Drawing.Size(787, 258);
            this.gbDetail.TabIndex = 8;
            this.gbDetail.Text = "26311_gbReferencia";
            // 
            // btnFilterChk
            // 
            this.btnFilterChk.Location = new System.Drawing.Point(674, 1);
            this.btnFilterChk.Name = "btnFilterChk";
            this.btnFilterChk.Size = new System.Drawing.Size(106, 22);
            this.btnFilterChk.TabIndex = 12;
            this.btnFilterChk.Text = "26311_btnFilter";
            this.btnFilterChk.CheckedChanged += new System.EventHandler(this.btnFilterChk_CheckedChanged);
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
            this.gcDetail.Size = new System.Drawing.Size(783, 232);
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
            this.gvDetail.OptionsBehavior.ReadOnly = true;
            this.gvDetail.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetail.OptionsCustomization.AllowFilter = false;
            this.gvDetail.OptionsCustomization.AllowSort = false;
            this.gvDetail.OptionsMenu.EnableColumnMenu = false;
            this.gvDetail.OptionsMenu.EnableFooterMenu = false;
            this.gvDetail.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetail.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvDetail.OptionsView.ShowGroupPanel = false;
            this.gvDetail.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDetail_CustomRowCellEdit);
            this.gvDetail.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetail_FocusedRowChanged);
            this.gvDetail.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvHeader_CustomUnboundColumnData);
            // 
            // pgGrid
            // 
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(174, 478);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(453, 26);
            this.pgGrid.TabIndex = 9;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.lblRefProvee);
            this.pnlFilter.Controls.Add(this.txtRefProveedor);
            this.pnlFilter.Controls.Add(this.chkRefProvee);
            this.pnlFilter.Controls.Add(this.masterReferencia);
            this.pnlFilter.Controls.Add(this.chkReferencia);
            this.pnlFilter.Controls.Add(this.lblEstado);
            this.pnlFilter.Controls.Add(this.lblSerial);
            this.pnlFilter.Controls.Add(this.txtSerial);
            this.pnlFilter.Controls.Add(this.cmbEstado);
            this.pnlFilter.Controls.Add(this.chkEstado);
            this.pnlFilter.Controls.Add(this.chkSerial);
            this.pnlFilter.Controls.Add(this.masterParam2);
            this.pnlFilter.Controls.Add(this.chkParam2);
            this.pnlFilter.Controls.Add(this.masterParam1);
            this.pnlFilter.Controls.Add(this.chkParam1);
            this.pnlFilter.Controls.Add(this.masterEmpaqueInv);
            this.pnlFilter.Controls.Add(this.chkEmpaqueInv);
            this.pnlFilter.Controls.Add(this.masterUnidadInv);
            this.pnlFilter.Controls.Add(this.chkUnidadInv);
            this.pnlFilter.Controls.Add(this.masterMarcaInv);
            this.pnlFilter.Controls.Add(this.masterMaterialInv);
            this.pnlFilter.Controls.Add(this.masterSerieInv);
            this.pnlFilter.Controls.Add(this.masterTipoInv);
            this.pnlFilter.Controls.Add(this.masterClaseInv);
            this.pnlFilter.Controls.Add(this.masterGrupoInv);
            this.pnlFilter.Controls.Add(this.chkMarcaInv);
            this.pnlFilter.Controls.Add(this.chkMaterialInv);
            this.pnlFilter.Controls.Add(this.chkSerieInv);
            this.pnlFilter.Controls.Add(this.chkTipoInv);
            this.pnlFilter.Controls.Add(this.chkClaseInv);
            this.pnlFilter.Controls.Add(this.chkGrupoInv);
            this.pnlFilter.Controls.Add(this.simpleButton1);
            this.pnlFilter.Location = new System.Drawing.Point(805, 219);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(228, 330);
            this.pnlFilter.TabIndex = 11;
            this.pnlFilter.Visible = false;
            // 
            // lblRefProvee
            // 
            this.lblRefProvee.BackColor = System.Drawing.Color.Transparent;
            this.lblRefProvee.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefProvee.Location = new System.Drawing.Point(21, 5);
            this.lblRefProvee.Name = "lblRefProvee";
            this.lblRefProvee.Size = new System.Drawing.Size(97, 18);
            this.lblRefProvee.TabIndex = 46;
            this.lblRefProvee.Text = "26311_lblRefProveedor";
            this.lblRefProvee.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRefProveedor
            // 
            this.txtRefProveedor.Location = new System.Drawing.Point(124, 2);
            this.txtRefProveedor.Name = "txtRefProveedor";
            this.txtRefProveedor.Size = new System.Drawing.Size(97, 21);
            this.txtRefProveedor.TabIndex = 48;
            this.txtRefProveedor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.txtRefProveedor.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // chkRefProvee
            // 
            this.chkRefProvee.Location = new System.Drawing.Point(5, 3);
            this.chkRefProvee.Name = "chkRefProvee";
            this.chkRefProvee.Properties.Caption = "";
            this.chkRefProvee.Size = new System.Drawing.Size(75, 19);
            this.chkRefProvee.TabIndex = 47;
            this.chkRefProvee.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // masterReferencia
            // 
            this.masterReferencia.BackColor = System.Drawing.Color.Transparent;
            this.masterReferencia.Filtros = null;
            this.masterReferencia.Location = new System.Drawing.Point(24, 46);
            this.masterReferencia.Name = "masterReferencia";
            this.masterReferencia.Size = new System.Drawing.Size(271, 23);
            this.masterReferencia.TabIndex = 45;
            this.masterReferencia.Value = "";
            this.masterReferencia.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterReferencia.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // chkReferencia
            // 
            this.chkReferencia.Location = new System.Drawing.Point(4, 49);
            this.chkReferencia.Name = "chkReferencia";
            this.chkReferencia.Properties.Caption = "";
            this.chkReferencia.Size = new System.Drawing.Size(75, 19);
            this.chkReferencia.TabIndex = 44;
            this.chkReferencia.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // lblEstado
            // 
            this.lblEstado.BackColor = System.Drawing.Color.Transparent;
            this.lblEstado.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado.Location = new System.Drawing.Point(22, 303);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(97, 18);
            this.lblEstado.TabIndex = 43;
            this.lblEstado.Text = "26311_lblEstado";
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSerial
            // 
            this.lblSerial.BackColor = System.Drawing.Color.Transparent;
            this.lblSerial.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSerial.Location = new System.Drawing.Point(21, 28);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(83, 18);
            this.lblSerial.TabIndex = 24;
            this.lblSerial.Text = "26311_lblSerial";
            this.lblSerial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(124, 25);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(97, 21);
            this.txtSerial.TabIndex = 42;
            // 
            // cmbEstado
            // 
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Location = new System.Drawing.Point(125, 302);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(97, 21);
            this.cmbEstado.TabIndex = 41;
            // 
            // chkEstado
            // 
            this.chkEstado.Location = new System.Drawing.Point(5, 303);
            this.chkEstado.Name = "chkEstado";
            this.chkEstado.Properties.Caption = "";
            this.chkEstado.Size = new System.Drawing.Size(75, 19);
            this.chkEstado.TabIndex = 40;
            this.chkEstado.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkSerial
            // 
            this.chkSerial.Location = new System.Drawing.Point(5, 26);
            this.chkSerial.Name = "chkSerial";
            this.chkSerial.Properties.Caption = "";
            this.chkSerial.Size = new System.Drawing.Size(75, 19);
            this.chkSerial.TabIndex = 39;
            this.chkSerial.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // masterParam2
            // 
            this.masterParam2.BackColor = System.Drawing.Color.Transparent;
            this.masterParam2.Filtros = null;
            this.masterParam2.Location = new System.Drawing.Point(25, 116);
            this.masterParam2.Name = "masterParam2";
            this.masterParam2.Size = new System.Drawing.Size(271, 23);
            this.masterParam2.TabIndex = 38;
            this.masterParam2.Value = "";
            this.masterParam2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterParam2.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // chkParam2
            // 
            this.chkParam2.Location = new System.Drawing.Point(5, 118);
            this.chkParam2.Name = "chkParam2";
            this.chkParam2.Properties.Caption = "";
            this.chkParam2.Size = new System.Drawing.Size(75, 19);
            this.chkParam2.TabIndex = 37;
            this.chkParam2.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // masterParam1
            // 
            this.masterParam1.BackColor = System.Drawing.Color.Transparent;
            this.masterParam1.Filtros = null;
            this.masterParam1.Location = new System.Drawing.Point(25, 93);
            this.masterParam1.Name = "masterParam1";
            this.masterParam1.Size = new System.Drawing.Size(271, 23);
            this.masterParam1.TabIndex = 36;
            this.masterParam1.Value = "";
            this.masterParam1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterParam1.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // chkParam1
            // 
            this.chkParam1.Location = new System.Drawing.Point(5, 95);
            this.chkParam1.Name = "chkParam1";
            this.chkParam1.Properties.Caption = "";
            this.chkParam1.Size = new System.Drawing.Size(75, 19);
            this.chkParam1.TabIndex = 35;
            this.chkParam1.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // masterEmpaqueInv
            // 
            this.masterEmpaqueInv.BackColor = System.Drawing.Color.Transparent;
            this.masterEmpaqueInv.Filtros = null;
            this.masterEmpaqueInv.Location = new System.Drawing.Point(25, 277);
            this.masterEmpaqueInv.Name = "masterEmpaqueInv";
            this.masterEmpaqueInv.Size = new System.Drawing.Size(271, 23);
            this.masterEmpaqueInv.TabIndex = 34;
            this.masterEmpaqueInv.Value = "";
            this.masterEmpaqueInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterEmpaqueInv.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // chkEmpaqueInv
            // 
            this.chkEmpaqueInv.Location = new System.Drawing.Point(5, 279);
            this.chkEmpaqueInv.Name = "chkEmpaqueInv";
            this.chkEmpaqueInv.Properties.Caption = "";
            this.chkEmpaqueInv.Size = new System.Drawing.Size(75, 19);
            this.chkEmpaqueInv.TabIndex = 33;
            this.chkEmpaqueInv.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // masterUnidadInv
            // 
            this.masterUnidadInv.BackColor = System.Drawing.Color.Transparent;
            this.masterUnidadInv.Filtros = null;
            this.masterUnidadInv.Location = new System.Drawing.Point(25, 254);
            this.masterUnidadInv.Name = "masterUnidadInv";
            this.masterUnidadInv.Size = new System.Drawing.Size(271, 23);
            this.masterUnidadInv.TabIndex = 32;
            this.masterUnidadInv.Value = "";
            this.masterUnidadInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterUnidadInv.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // chkUnidadInv
            // 
            this.chkUnidadInv.Location = new System.Drawing.Point(5, 256);
            this.chkUnidadInv.Name = "chkUnidadInv";
            this.chkUnidadInv.Properties.Caption = "";
            this.chkUnidadInv.Size = new System.Drawing.Size(75, 19);
            this.chkUnidadInv.TabIndex = 31;
            this.chkUnidadInv.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // masterMarcaInv
            // 
            this.masterMarcaInv.BackColor = System.Drawing.Color.Transparent;
            this.masterMarcaInv.Filtros = null;
            this.masterMarcaInv.Location = new System.Drawing.Point(24, 70);
            this.masterMarcaInv.Name = "masterMarcaInv";
            this.masterMarcaInv.Size = new System.Drawing.Size(271, 23);
            this.masterMarcaInv.TabIndex = 29;
            this.masterMarcaInv.Value = "";
            this.masterMarcaInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterMarcaInv.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // masterMaterialInv
            // 
            this.masterMaterialInv.BackColor = System.Drawing.Color.Transparent;
            this.masterMaterialInv.Filtros = null;
            this.masterMaterialInv.Location = new System.Drawing.Point(25, 231);
            this.masterMaterialInv.Name = "masterMaterialInv";
            this.masterMaterialInv.Size = new System.Drawing.Size(271, 23);
            this.masterMaterialInv.TabIndex = 28;
            this.masterMaterialInv.Value = "";
            this.masterMaterialInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterMaterialInv.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // masterSerieInv
            // 
            this.masterSerieInv.BackColor = System.Drawing.Color.Transparent;
            this.masterSerieInv.Filtros = null;
            this.masterSerieInv.Location = new System.Drawing.Point(25, 208);
            this.masterSerieInv.Name = "masterSerieInv";
            this.masterSerieInv.Size = new System.Drawing.Size(271, 23);
            this.masterSerieInv.TabIndex = 27;
            this.masterSerieInv.Value = "";
            this.masterSerieInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterSerieInv.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // masterTipoInv
            // 
            this.masterTipoInv.BackColor = System.Drawing.Color.Transparent;
            this.masterTipoInv.Filtros = null;
            this.masterTipoInv.Location = new System.Drawing.Point(25, 185);
            this.masterTipoInv.Name = "masterTipoInv";
            this.masterTipoInv.Size = new System.Drawing.Size(271, 23);
            this.masterTipoInv.TabIndex = 26;
            this.masterTipoInv.Value = "";
            this.masterTipoInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterTipoInv.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // masterClaseInv
            // 
            this.masterClaseInv.BackColor = System.Drawing.Color.Transparent;
            this.masterClaseInv.Filtros = null;
            this.masterClaseInv.Location = new System.Drawing.Point(25, 162);
            this.masterClaseInv.Name = "masterClaseInv";
            this.masterClaseInv.Size = new System.Drawing.Size(271, 23);
            this.masterClaseInv.TabIndex = 25;
            this.masterClaseInv.Value = "";
            this.masterClaseInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterClaseInv.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // masterGrupoInv
            // 
            this.masterGrupoInv.BackColor = System.Drawing.Color.Transparent;
            this.masterGrupoInv.Filtros = null;
            this.masterGrupoInv.Location = new System.Drawing.Point(25, 139);
            this.masterGrupoInv.Name = "masterGrupoInv";
            this.masterGrupoInv.Size = new System.Drawing.Size(271, 23);
            this.masterGrupoInv.TabIndex = 24;
            this.masterGrupoInv.Value = "";
            this.masterGrupoInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            this.masterGrupoInv.Leave += new System.EventHandler(this.masterFilter_Leave);
            // 
            // chkMarcaInv
            // 
            this.chkMarcaInv.Location = new System.Drawing.Point(4, 72);
            this.chkMarcaInv.Name = "chkMarcaInv";
            this.chkMarcaInv.Properties.Caption = "";
            this.chkMarcaInv.Size = new System.Drawing.Size(75, 19);
            this.chkMarcaInv.TabIndex = 21;
            this.chkMarcaInv.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkMaterialInv
            // 
            this.chkMaterialInv.Location = new System.Drawing.Point(5, 233);
            this.chkMaterialInv.Name = "chkMaterialInv";
            this.chkMaterialInv.Properties.Caption = "";
            this.chkMaterialInv.Size = new System.Drawing.Size(75, 19);
            this.chkMaterialInv.TabIndex = 19;
            this.chkMaterialInv.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkSerieInv
            // 
            this.chkSerieInv.Location = new System.Drawing.Point(5, 210);
            this.chkSerieInv.Name = "chkSerieInv";
            this.chkSerieInv.Properties.Caption = "";
            this.chkSerieInv.Size = new System.Drawing.Size(75, 19);
            this.chkSerieInv.TabIndex = 17;
            this.chkSerieInv.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkTipoInv
            // 
            this.chkTipoInv.Location = new System.Drawing.Point(5, 187);
            this.chkTipoInv.Name = "chkTipoInv";
            this.chkTipoInv.Properties.Caption = "";
            this.chkTipoInv.Size = new System.Drawing.Size(75, 19);
            this.chkTipoInv.TabIndex = 15;
            this.chkTipoInv.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkClaseInv
            // 
            this.chkClaseInv.Location = new System.Drawing.Point(5, 164);
            this.chkClaseInv.Name = "chkClaseInv";
            this.chkClaseInv.Properties.Caption = "";
            this.chkClaseInv.Size = new System.Drawing.Size(75, 19);
            this.chkClaseInv.TabIndex = 13;
            this.chkClaseInv.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // chkGrupoInv
            // 
            this.chkGrupoInv.Location = new System.Drawing.Point(5, 141);
            this.chkGrupoInv.Name = "chkGrupoInv";
            this.chkGrupoInv.Properties.Caption = "";
            this.chkGrupoInv.Size = new System.Drawing.Size(75, 19);
            this.chkGrupoInv.TabIndex = 11;
            this.chkGrupoInv.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Location = new System.Drawing.Point(633, 8);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(2, 23);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Text = "Filtrar";
            // 
            // gcLibroIFRS
            // 
            this.gcLibroIFRS.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gcLibroIFRS.AppearanceCaption.Options.UseFont = true;
            this.gcLibroIFRS.AppearanceCaption.Options.UseTextOptions = true;
            this.gcLibroIFRS.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcLibroIFRS.Controls.Add(this.lblCurrencyForIFRS);
            this.gcLibroIFRS.Controls.Add(this.lblCurrencyLocIFRS);
            this.gcLibroIFRS.Controls.Add(this.txtValorUnitMEIFRS);
            this.gcLibroIFRS.Controls.Add(this.txtValorTotalMLIFRS);
            this.gcLibroIFRS.Controls.Add(this.txtValorUnitMLIFRS);
            this.gcLibroIFRS.Controls.Add(this.txtValorTotalMEIFRS);
            this.gcLibroIFRS.Controls.Add(this.lblValorUnitIFRS);
            this.gcLibroIFRS.Controls.Add(this.lblValorTotalIFRS);
            this.gcLibroIFRS.Location = new System.Drawing.Point(360, 508);
            this.gcLibroIFRS.Name = "gcLibroIFRS";
            this.gcLibroIFRS.Size = new System.Drawing.Size(354, 88);
            this.gcLibroIFRS.TabIndex = 45;
            this.gcLibroIFRS.Text = "26310_gcLibroIFRS";
            // 
            // lblCurrencyForIFRS
            // 
            this.lblCurrencyForIFRS.BackColor = System.Drawing.Color.LightGray;
            this.lblCurrencyForIFRS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyForIFRS.Location = new System.Drawing.Point(217, 25);
            this.lblCurrencyForIFRS.Name = "lblCurrencyForIFRS";
            this.lblCurrencyForIFRS.Size = new System.Drawing.Size(122, 13);
            this.lblCurrencyForIFRS.TabIndex = 23;
            this.lblCurrencyForIFRS.Text = "26311_lblCurrencyForeign";
            this.lblCurrencyForIFRS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrencyLocIFRS
            // 
            this.lblCurrencyLocIFRS.BackColor = System.Drawing.Color.LightGray;
            this.lblCurrencyLocIFRS.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyLocIFRS.Location = new System.Drawing.Point(87, 25);
            this.lblCurrencyLocIFRS.Name = "lblCurrencyLocIFRS";
            this.lblCurrencyLocIFRS.Size = new System.Drawing.Size(121, 12);
            this.lblCurrencyLocIFRS.TabIndex = 22;
            this.lblCurrencyLocIFRS.Text = "26311_lblCurrencyLocal";
            this.lblCurrencyLocIFRS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtValorUnitMEIFRS
            // 
            this.txtValorUnitMEIFRS.EditValue = "0";
            this.txtValorUnitMEIFRS.Location = new System.Drawing.Point(217, 40);
            this.txtValorUnitMEIFRS.Name = "txtValorUnitMEIFRS";
            this.txtValorUnitMEIFRS.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorUnitMEIFRS.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F);
            this.txtValorUnitMEIFRS.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorUnitMEIFRS.Properties.Appearance.Options.UseFont = true;
            this.txtValorUnitMEIFRS.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorUnitMEIFRS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorUnitMEIFRS.Properties.AutoHeight = false;
            this.txtValorUnitMEIFRS.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUnitMEIFRS.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUnitMEIFRS.Properties.Mask.EditMask = "c";
            this.txtValorUnitMEIFRS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorUnitMEIFRS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorUnitMEIFRS.Properties.ReadOnly = true;
            this.txtValorUnitMEIFRS.Size = new System.Drawing.Size(122, 21);
            this.txtValorUnitMEIFRS.TabIndex = 14;
            // 
            // txtValorTotalMLIFRS
            // 
            this.txtValorTotalMLIFRS.EditValue = "0";
            this.txtValorTotalMLIFRS.Location = new System.Drawing.Point(86, 61);
            this.txtValorTotalMLIFRS.Name = "txtValorTotalMLIFRS";
            this.txtValorTotalMLIFRS.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorTotalMLIFRS.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F);
            this.txtValorTotalMLIFRS.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorTotalMLIFRS.Properties.Appearance.Options.UseFont = true;
            this.txtValorTotalMLIFRS.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorTotalMLIFRS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorTotalMLIFRS.Properties.AutoHeight = false;
            this.txtValorTotalMLIFRS.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalMLIFRS.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalMLIFRS.Properties.Mask.EditMask = "c";
            this.txtValorTotalMLIFRS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorTotalMLIFRS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorTotalMLIFRS.Properties.ReadOnly = true;
            this.txtValorTotalMLIFRS.Size = new System.Drawing.Size(122, 21);
            this.txtValorTotalMLIFRS.TabIndex = 16;
            // 
            // txtValorUnitMLIFRS
            // 
            this.txtValorUnitMLIFRS.EditValue = "0";
            this.txtValorUnitMLIFRS.Location = new System.Drawing.Point(86, 40);
            this.txtValorUnitMLIFRS.Name = "txtValorUnitMLIFRS";
            this.txtValorUnitMLIFRS.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorUnitMLIFRS.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F);
            this.txtValorUnitMLIFRS.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorUnitMLIFRS.Properties.Appearance.Options.UseFont = true;
            this.txtValorUnitMLIFRS.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorUnitMLIFRS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorUnitMLIFRS.Properties.AutoHeight = false;
            this.txtValorUnitMLIFRS.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUnitMLIFRS.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUnitMLIFRS.Properties.Mask.EditMask = "c";
            this.txtValorUnitMLIFRS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorUnitMLIFRS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorUnitMLIFRS.Properties.ReadOnly = true;
            this.txtValorUnitMLIFRS.Size = new System.Drawing.Size(122, 21);
            this.txtValorUnitMLIFRS.TabIndex = 13;
            // 
            // txtValorTotalMEIFRS
            // 
            this.txtValorTotalMEIFRS.EditValue = "0";
            this.txtValorTotalMEIFRS.Location = new System.Drawing.Point(217, 61);
            this.txtValorTotalMEIFRS.Name = "txtValorTotalMEIFRS";
            this.txtValorTotalMEIFRS.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorTotalMEIFRS.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F);
            this.txtValorTotalMEIFRS.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorTotalMEIFRS.Properties.Appearance.Options.UseFont = true;
            this.txtValorTotalMEIFRS.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorTotalMEIFRS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorTotalMEIFRS.Properties.AutoHeight = false;
            this.txtValorTotalMEIFRS.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalMEIFRS.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalMEIFRS.Properties.Mask.EditMask = "c";
            this.txtValorTotalMEIFRS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorTotalMEIFRS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorTotalMEIFRS.Properties.ReadOnly = true;
            this.txtValorTotalMEIFRS.Size = new System.Drawing.Size(122, 21);
            this.txtValorTotalMEIFRS.TabIndex = 17;
            // 
            // lblValorUnitIFRS
            // 
            this.lblValorUnitIFRS.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblValorUnitIFRS.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorUnitIFRS.Location = new System.Drawing.Point(8, 44);
            this.lblValorUnitIFRS.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorUnitIFRS.Name = "lblValorUnitIFRS";
            this.lblValorUnitIFRS.Size = new System.Drawing.Size(105, 14);
            this.lblValorUnitIFRS.TabIndex = 19;
            this.lblValorUnitIFRS.Text = "26311_lblValueUnit";
            // 
            // lblValorTotalIFRS
            // 
            this.lblValorTotalIFRS.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblValorTotalIFRS.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTotalIFRS.Location = new System.Drawing.Point(8, 64);
            this.lblValorTotalIFRS.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorTotalIFRS.Name = "lblValorTotalIFRS";
            this.lblValorTotalIFRS.Size = new System.Drawing.Size(111, 14);
            this.lblValorTotalIFRS.TabIndex = 18;
            this.lblValorTotalIFRS.Text = "26311_lblValueTotal";
            // 
            // gcLibroFuncional
            // 
            this.gcLibroFuncional.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gcLibroFuncional.AppearanceCaption.Options.UseFont = true;
            this.gcLibroFuncional.AppearanceCaption.Options.UseTextOptions = true;
            this.gcLibroFuncional.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcLibroFuncional.Controls.Add(this.lblCurrencyFor);
            this.gcLibroFuncional.Controls.Add(this.lblCurrencyLoc);
            this.gcLibroFuncional.Controls.Add(this.txtValorUnitME);
            this.gcLibroFuncional.Controls.Add(this.txtValorTotalML);
            this.gcLibroFuncional.Controls.Add(this.txtValorUnitML);
            this.gcLibroFuncional.Controls.Add(this.txtValorTotalME);
            this.gcLibroFuncional.Controls.Add(this.lblValorUnit);
            this.gcLibroFuncional.Controls.Add(this.lblValorTotal);
            this.gcLibroFuncional.Location = new System.Drawing.Point(4, 507);
            this.gcLibroFuncional.Name = "gcLibroFuncional";
            this.gcLibroFuncional.Size = new System.Drawing.Size(349, 88);
            this.gcLibroFuncional.TabIndex = 44;
            this.gcLibroFuncional.Text = "26310_gcLibroFuncional";
            // 
            // lblCurrencyFor
            // 
            this.lblCurrencyFor.BackColor = System.Drawing.Color.LightGray;
            this.lblCurrencyFor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyFor.Location = new System.Drawing.Point(216, 25);
            this.lblCurrencyFor.Name = "lblCurrencyFor";
            this.lblCurrencyFor.Size = new System.Drawing.Size(122, 14);
            this.lblCurrencyFor.TabIndex = 23;
            this.lblCurrencyFor.Text = "26311_lblCurrencyForeign";
            this.lblCurrencyFor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrencyLoc
            // 
            this.lblCurrencyLoc.BackColor = System.Drawing.Color.LightGray;
            this.lblCurrencyLoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyLoc.Location = new System.Drawing.Point(85, 26);
            this.lblCurrencyLoc.Name = "lblCurrencyLoc";
            this.lblCurrencyLoc.Size = new System.Drawing.Size(121, 13);
            this.lblCurrencyLoc.TabIndex = 22;
            this.lblCurrencyLoc.Text = "26311_lblCurrencyLocal";
            this.lblCurrencyLoc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtValorUnitME
            // 
            this.txtValorUnitME.EditValue = "0";
            this.txtValorUnitME.Location = new System.Drawing.Point(216, 40);
            this.txtValorUnitME.Name = "txtValorUnitME";
            this.txtValorUnitME.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorUnitME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F);
            this.txtValorUnitME.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorUnitME.Properties.Appearance.Options.UseFont = true;
            this.txtValorUnitME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorUnitME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorUnitME.Properties.AutoHeight = false;
            this.txtValorUnitME.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUnitME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUnitME.Properties.Mask.EditMask = "c";
            this.txtValorUnitME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorUnitME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorUnitME.Properties.ReadOnly = true;
            this.txtValorUnitME.Size = new System.Drawing.Size(122, 21);
            this.txtValorUnitME.TabIndex = 14;
            // 
            // txtValorTotalML
            // 
            this.txtValorTotalML.EditValue = "0";
            this.txtValorTotalML.Location = new System.Drawing.Point(84, 61);
            this.txtValorTotalML.Name = "txtValorTotalML";
            this.txtValorTotalML.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorTotalML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F);
            this.txtValorTotalML.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorTotalML.Properties.Appearance.Options.UseFont = true;
            this.txtValorTotalML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorTotalML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorTotalML.Properties.AutoHeight = false;
            this.txtValorTotalML.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalML.Properties.Mask.EditMask = "c";
            this.txtValorTotalML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorTotalML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorTotalML.Properties.ReadOnly = true;
            this.txtValorTotalML.Size = new System.Drawing.Size(122, 21);
            this.txtValorTotalML.TabIndex = 16;
            // 
            // txtValorUnitML
            // 
            this.txtValorUnitML.EditValue = "0";
            this.txtValorUnitML.Location = new System.Drawing.Point(84, 40);
            this.txtValorUnitML.Name = "txtValorUnitML";
            this.txtValorUnitML.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorUnitML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F);
            this.txtValorUnitML.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorUnitML.Properties.Appearance.Options.UseFont = true;
            this.txtValorUnitML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorUnitML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorUnitML.Properties.AutoHeight = false;
            this.txtValorUnitML.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUnitML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorUnitML.Properties.Mask.EditMask = "c";
            this.txtValorUnitML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorUnitML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorUnitML.Properties.ReadOnly = true;
            this.txtValorUnitML.Size = new System.Drawing.Size(122, 21);
            this.txtValorUnitML.TabIndex = 13;
            // 
            // txtValorTotalME
            // 
            this.txtValorTotalME.EditValue = "0";
            this.txtValorTotalME.Location = new System.Drawing.Point(216, 61);
            this.txtValorTotalME.Name = "txtValorTotalME";
            this.txtValorTotalME.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorTotalME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 9.5F);
            this.txtValorTotalME.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorTotalME.Properties.Appearance.Options.UseFont = true;
            this.txtValorTotalME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorTotalME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorTotalME.Properties.AutoHeight = false;
            this.txtValorTotalME.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorTotalME.Properties.Mask.EditMask = "c";
            this.txtValorTotalME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorTotalME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorTotalME.Properties.ReadOnly = true;
            this.txtValorTotalME.Size = new System.Drawing.Size(122, 21);
            this.txtValorTotalME.TabIndex = 17;
            // 
            // lblValorUnit
            // 
            this.lblValorUnit.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblValorUnit.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorUnit.Location = new System.Drawing.Point(8, 44);
            this.lblValorUnit.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorUnit.Name = "lblValorUnit";
            this.lblValorUnit.Size = new System.Drawing.Size(105, 14);
            this.lblValorUnit.TabIndex = 19;
            this.lblValorUnit.Text = "26311_lblValueUnit";
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblValorTotal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTotal.Location = new System.Drawing.Point(8, 64);
            this.lblValorTotal.Margin = new System.Windows.Forms.Padding(4);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(111, 14);
            this.lblValorTotal.TabIndex = 18;
            this.lblValorTotal.Text = "26311_lblValueTotal";
            // 
            // scroll
            // 
            this.scroll.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.scroll.Appearance.Options.UseBackColor = true;
            this.scroll.Controls.Add(this.gbHeader);
            this.scroll.Controls.Add(this.gcLibroIFRS);
            this.scroll.Controls.Add(this.pgGrid);
            this.scroll.Controls.Add(this.pnlFilter);
            this.scroll.Controls.Add(this.gbDetail);
            this.scroll.Controls.Add(this.gcLibroFuncional);
            this.scroll.Location = new System.Drawing.Point(0, 1);
            this.scroll.Name = "scroll";
            this.scroll.Size = new System.Drawing.Size(1061, 612);
            this.scroll.TabIndex = 46;
            // 
            // ConsultaReferencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1176, 636);
            this.Controls.Add(this.scroll);
            this.Name = "ConsultaReferencia";
            this.Text = "20310";
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbHeader)).EndInit();
            this.gbHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBodega)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBodega)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).EndInit();
            this.gbDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFilter)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkRefProvee.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkReferencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSerial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkParam2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkParam1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEmpaqueInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUnidadInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMarcaInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMaterialInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSerieInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTipoInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkClaseInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkGrupoInv.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLibroIFRS)).EndInit();
            this.gcLibroIFRS.ResumeLayout(false);
            this.gcLibroIFRS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitMEIFRS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalMLIFRS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitMLIFRS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalMEIFRS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLibroFuncional)).EndInit();
            this.gcLibroFuncional.ResumeLayout(false);
            this.gcLibroFuncional.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalME.Properties)).EndInit();
            this.scroll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editCant;
        private DevExpress.XtraEditors.GroupControl gbHeader;
        private DevExpress.XtraGrid.GridControl gcBodega;
        private DevExpress.XtraGrid.Views.Grid.GridView gvBodega;
        private DevExpress.XtraEditors.GroupControl gbDetail;
        private DevExpress.XtraGrid.GridControl gcDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetail;
        private ControlsUC.uc_Pagging pgGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnMvto;
        private DevExpress.XtraEditors.PanelControl pnlFilter;
        private DevExpress.XtraEditors.CheckEdit chkMarcaInv;
        private DevExpress.XtraEditors.CheckEdit chkMaterialInv;
        private DevExpress.XtraEditors.CheckEdit chkSerieInv;
        private DevExpress.XtraEditors.CheckEdit chkTipoInv;
        private DevExpress.XtraEditors.CheckEdit chkClaseInv;
        private DevExpress.XtraEditors.CheckEdit chkGrupoInv;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.CheckButton btnFilterChk;
        private ControlsUC.uc_MasterFind masterMarcaInv;
        private ControlsUC.uc_MasterFind masterMaterialInv;
        private ControlsUC.uc_MasterFind masterSerieInv;
        private ControlsUC.uc_MasterFind masterTipoInv;
        private ControlsUC.uc_MasterFind masterClaseInv;
        private ControlsUC.uc_MasterFind masterGrupoInv;
        private ControlsUC.uc_MasterFind masterEmpaqueInv;
        private DevExpress.XtraEditors.CheckEdit chkEmpaqueInv;
        private ControlsUC.uc_MasterFind masterUnidadInv;
        private DevExpress.XtraEditors.CheckEdit chkUnidadInv;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnDetail;
        private System.Windows.Forms.TextBox txtSerial;
        private Clases.ComboBoxEx cmbEstado;
        private DevExpress.XtraEditors.CheckEdit chkEstado;
        private DevExpress.XtraEditors.CheckEdit chkSerial;
        private ControlsUC.uc_MasterFind masterParam2;
        private DevExpress.XtraEditors.CheckEdit chkParam2;
        private ControlsUC.uc_MasterFind masterParam1;
        private DevExpress.XtraEditors.CheckEdit chkParam1;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblSerial;
        private DevExpress.XtraEditors.GroupControl gcLibroIFRS;
        private DevExpress.XtraEditors.LabelControl lblValorUnitIFRS;
        private DevExpress.XtraEditors.LabelControl lblValorTotalIFRS;
        private System.Windows.Forms.Label lblCurrencyForIFRS;
        private System.Windows.Forms.Label lblCurrencyLocIFRS;
        private DevExpress.XtraEditors.TextEdit txtValorUnitMEIFRS;
        private DevExpress.XtraEditors.TextEdit txtValorTotalMLIFRS;
        private DevExpress.XtraEditors.TextEdit txtValorUnitMLIFRS;
        private DevExpress.XtraEditors.TextEdit txtValorTotalMEIFRS;
        private DevExpress.XtraEditors.GroupControl gcLibroFuncional;
        private DevExpress.XtraEditors.LabelControl lblValorUnit;
        private DevExpress.XtraEditors.LabelControl lblValorTotal;
        private System.Windows.Forms.Label lblCurrencyFor;
        private System.Windows.Forms.Label lblCurrencyLoc;
        private DevExpress.XtraEditors.TextEdit txtValorUnitME;
        private DevExpress.XtraEditors.TextEdit txtValorTotalML;
        private DevExpress.XtraEditors.TextEdit txtValorUnitML;
        private DevExpress.XtraEditors.TextEdit txtValorTotalME;
        private System.Windows.Forms.Label lblRefProvee;
        private System.Windows.Forms.TextBox txtRefProveedor;
        private DevExpress.XtraEditors.CheckEdit chkRefProvee;
        private ControlsUC.uc_MasterFind masterReferencia;
        private DevExpress.XtraEditors.CheckEdit chkReferencia;
        private DevExpress.XtraEditors.XtraScrollableControl scroll;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;

    }
}