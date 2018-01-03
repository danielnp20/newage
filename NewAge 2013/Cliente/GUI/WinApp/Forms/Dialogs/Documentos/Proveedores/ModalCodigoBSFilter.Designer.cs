namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalCodigoBSFilter
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
            this.components = new System.ComponentModel.Container();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.editBtnMvto = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editBtnDetail = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gbHeader = new DevExpress.XtraEditors.GroupControl();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.masterTipoInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterClaseInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterGrupoInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblModelo = new System.Windows.Forms.Label();
            this.txtRefProveed = new System.Windows.Forms.TextBox();
            this.btnFilter = new DevExpress.XtraEditors.SimpleButton();
            this.masterEmpaqueInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterUnidadInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterMarcaInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterMaterialInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterSerieInv = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gbDetail = new DevExpress.XtraEditors.GroupControl();
            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbHeader)).BeginInit();
            this.gbHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).BeginInit();
            this.gbDetail.SuspendLayout();
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver Movimiento", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnMvto.Name = "editBtnMvto";
            this.editBtnMvto.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnMvto.ValidateOnEnterKey = true;
            // 
            // editBtnDetail
            // 
            this.editBtnDetail.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Ver detalle", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "Ver detalle de la referencia", null, null, true)});
            this.editBtnDetail.Name = "editBtnDetail";
            this.editBtnDetail.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.editBtnDetail.ValidateOnEnterKey = true;
            // 
            // gbHeader
            // 
            this.gbHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbHeader.AppearanceCaption.Options.UseFont = true;
            this.gbHeader.AppearanceCaption.Options.UseTextOptions = true;
            this.gbHeader.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbHeader.Controls.Add(this.gcData);
            this.gbHeader.Location = new System.Drawing.Point(4, 1);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(400, 283);
            this.gbHeader.TabIndex = 7;
            this.gbHeader.Text = "Códigos Bien Servicio";
            // 
            // gcData
            // 
            this.gcData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcData.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcData.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcData.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcData.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcData.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcData.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcData.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcData.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcData.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcData.Location = new System.Drawing.Point(2, 24);
            this.gcData.LookAndFeel.SkinName = "Dark Side";
            this.gcData.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcData.MainView = this.gvData;
            this.gcData.Margin = new System.Windows.Forms.Padding(4);
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(396, 257);
            this.gcData.TabIndex = 2;
            this.gcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvData.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvData.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvData.Appearance.Empty.Options.UseBackColor = true;
            this.gvData.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvData.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvData.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvData.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvData.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvData.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvData.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvData.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvData.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvData.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvData.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvData.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvData.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvData.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvData.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvData.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvData.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvData.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvData.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvData.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvData.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvData.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvData.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvData.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvData.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvData.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvData.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvData.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.Row.Options.UseBackColor = true;
            this.gvData.Appearance.Row.Options.UseForeColor = true;
            this.gvData.Appearance.Row.Options.UseTextOptions = true;
            this.gvData.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvData.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvData.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvData.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvData.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvData.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvData.Appearance.VertLine.Options.UseBackColor = true;
            this.gvData.GridControl = this.gcData;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsCustomization.AllowColumnMoving = false;
            this.gvData.OptionsCustomization.AllowFilter = false;
            this.gvData.OptionsDetail.EnableMasterViewMode = false;
            this.gvData.OptionsMenu.EnableColumnMenu = false;
            this.gvData.OptionsMenu.EnableFooterMenu = false;
            this.gvData.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvData.OptionsView.ShowGroupPanel = false;
            this.gvData.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvTarea_RowClick);
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvTarea_FocusedRowChanged);
            this.gvData.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvTarea_CustomUnboundColumnData);
            this.gvData.DoubleClick += new System.EventHandler(this.gvTarea_DoubleClick);
            // 
            // pgGrid
            // 
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(13, 282);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(419, 24);
            this.pgGrid.TabIndex = 9;
            // 
            // masterTipoInv
            // 
            this.masterTipoInv.BackColor = System.Drawing.Color.Transparent;
            this.masterTipoInv.Filtros = null;
            this.masterTipoInv.Location = new System.Drawing.Point(8, 208);
            this.masterTipoInv.Name = "masterTipoInv";
            this.masterTipoInv.Size = new System.Drawing.Size(301, 23);
            this.masterTipoInv.TabIndex = 26;
            this.masterTipoInv.Value = "";
            this.masterTipoInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterClaseInv
            // 
            this.masterClaseInv.BackColor = System.Drawing.Color.Transparent;
            this.masterClaseInv.Filtros = null;
            this.masterClaseInv.Location = new System.Drawing.Point(8, 185);
            this.masterClaseInv.Name = "masterClaseInv";
            this.masterClaseInv.Size = new System.Drawing.Size(301, 23);
            this.masterClaseInv.TabIndex = 25;
            this.masterClaseInv.Value = "";
            this.masterClaseInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterGrupoInv
            // 
            this.masterGrupoInv.BackColor = System.Drawing.Color.Transparent;
            this.masterGrupoInv.Filtros = null;
            this.masterGrupoInv.Location = new System.Drawing.Point(8, 162);
            this.masterGrupoInv.Name = "masterGrupoInv";
            this.masterGrupoInv.Size = new System.Drawing.Size(301, 23);
            this.masterGrupoInv.TabIndex = 24;
            this.masterGrupoInv.Value = "";
            this.masterGrupoInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // lblModelo
            // 
            this.lblModelo.BackColor = System.Drawing.Color.Transparent;
            this.lblModelo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblModelo.Location = new System.Drawing.Point(5, 29);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(78, 18);
            this.lblModelo.TabIndex = 43;
            this.lblModelo.Text = "110_lblModelo";
            this.lblModelo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRefProveed
            // 
            this.txtRefProveed.Location = new System.Drawing.Point(107, 26);
            this.txtRefProveed.Name = "txtRefProveed";
            this.txtRefProveed.Size = new System.Drawing.Size(97, 21);
            this.txtRefProveed.TabIndex = 45;
            this.txtRefProveed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnFilter
            // 
            this.btnFilter.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnFilter.Appearance.Options.UseFont = true;
            this.btnFilter.Location = new System.Drawing.Point(164, 237);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(137, 23);
            this.btnFilter.TabIndex = 8;
            this.btnFilter.Text = "Filtrar";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // masterEmpaqueInv
            // 
            this.masterEmpaqueInv.BackColor = System.Drawing.Color.Transparent;
            this.masterEmpaqueInv.Filtros = null;
            this.masterEmpaqueInv.Location = new System.Drawing.Point(8, 139);
            this.masterEmpaqueInv.Name = "masterEmpaqueInv";
            this.masterEmpaqueInv.Size = new System.Drawing.Size(301, 23);
            this.masterEmpaqueInv.TabIndex = 34;
            this.masterEmpaqueInv.Value = "";
            this.masterEmpaqueInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterUnidadInv
            // 
            this.masterUnidadInv.BackColor = System.Drawing.Color.Transparent;
            this.masterUnidadInv.Filtros = null;
            this.masterUnidadInv.Location = new System.Drawing.Point(8, 116);
            this.masterUnidadInv.Name = "masterUnidadInv";
            this.masterUnidadInv.Size = new System.Drawing.Size(301, 23);
            this.masterUnidadInv.TabIndex = 32;
            this.masterUnidadInv.Value = "";
            this.masterUnidadInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterMarcaInv
            // 
            this.masterMarcaInv.BackColor = System.Drawing.Color.Transparent;
            this.masterMarcaInv.Filtros = null;
            this.masterMarcaInv.Location = new System.Drawing.Point(8, 47);
            this.masterMarcaInv.Name = "masterMarcaInv";
            this.masterMarcaInv.Size = new System.Drawing.Size(301, 23);
            this.masterMarcaInv.TabIndex = 29;
            this.masterMarcaInv.Value = "";
            this.masterMarcaInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterMaterialInv
            // 
            this.masterMaterialInv.BackColor = System.Drawing.Color.Transparent;
            this.masterMaterialInv.Filtros = null;
            this.masterMaterialInv.Location = new System.Drawing.Point(8, 70);
            this.masterMaterialInv.Name = "masterMaterialInv";
            this.masterMaterialInv.Size = new System.Drawing.Size(301, 23);
            this.masterMaterialInv.TabIndex = 28;
            this.masterMaterialInv.Value = "";
            this.masterMaterialInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterSerieInv
            // 
            this.masterSerieInv.BackColor = System.Drawing.Color.Transparent;
            this.masterSerieInv.Filtros = null;
            this.masterSerieInv.Location = new System.Drawing.Point(8, 93);
            this.masterSerieInv.Name = "masterSerieInv";
            this.masterSerieInv.Size = new System.Drawing.Size(301, 23);
            this.masterSerieInv.TabIndex = 27;
            this.masterSerieInv.Value = "";
            this.masterSerieInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // gbDetail
            // 
            this.gbDetail.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbDetail.AppearanceCaption.Options.UseFont = true;
            this.gbDetail.Controls.Add(this.masterTipoInv);
            this.gbDetail.Controls.Add(this.lblModelo);
            this.gbDetail.Controls.Add(this.masterClaseInv);
            this.gbDetail.Controls.Add(this.masterSerieInv);
            this.gbDetail.Controls.Add(this.masterGrupoInv);
            this.gbDetail.Controls.Add(this.masterMaterialInv);
            this.gbDetail.Controls.Add(this.masterMarcaInv);
            this.gbDetail.Controls.Add(this.txtRefProveed);
            this.gbDetail.Controls.Add(this.masterUnidadInv);
            this.gbDetail.Controls.Add(this.btnFilter);
            this.gbDetail.Controls.Add(this.masterEmpaqueInv);
            this.gbDetail.Location = new System.Drawing.Point(407, 1);
            this.gbDetail.Name = "gbDetail";
            this.gbDetail.Size = new System.Drawing.Size(307, 283);
            this.gbDetail.TabIndex = 8;
            this.gbDetail.Text = "Filtros";
            // 
            // btnAccept
            // 
            this.btnAccept.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnAccept.Appearance.Options.UseFont = true;
            this.btnAccept.Location = new System.Drawing.Point(567, 315);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(81, 23);
            this.btnAccept.TabIndex = 13;
            this.btnAccept.Text = "Aceptar";
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // lblCodigo
            // 
            this.lblCodigo.BackColor = System.Drawing.Color.Transparent;
            this.lblCodigo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.Location = new System.Drawing.Point(22, 319);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(83, 18);
            this.lblCodigo.TabIndex = 46;
            this.lblCodigo.Text = "110_lblCodigo";
            this.lblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(95, 318);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(69, 20);
            this.txtCodigo.TabIndex = 47;
            this.txtCodigo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // lblDesc
            // 
            this.lblDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.Location = new System.Drawing.Point(169, 319);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(83, 18);
            this.lblDesc.TabIndex = 48;
            this.lblDesc.Text = "110_lblDesc";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(255, 318);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(135, 20);
            this.txtDesc.TabIndex = 49;
            this.txtDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 303);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 14);
            this.label1.TabIndex = 50;
            this.label1.Text = "Buscar:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(652, 315);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ModalCodigoBSFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(722, 345);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.gbDetail);
            this.Controls.Add(this.gbHeader);
            this.Controls.Add(this.pgGrid);
            this.Controls.Add(this.lblCodigo);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(887, 424);
            this.MinimizeBox = false;
            this.Name = "ModalCodigoBSFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "110_frmReferencias";
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbHeader)).EndInit();
            this.gbHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).EndInit();
            this.gbDetail.ResumeLayout(false);
            this.gbDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private DevExpress.XtraEditors.GroupControl gbHeader;
        private ControlsUC.uc_Pagging pgGrid;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnMvto;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnDetail;
        private ControlsUC.uc_MasterFind masterEmpaqueInv;
        private ControlsUC.uc_MasterFind masterUnidadInv;
        private ControlsUC.uc_MasterFind masterMarcaInv;
        private ControlsUC.uc_MasterFind masterMaterialInv;
        private ControlsUC.uc_MasterFind masterSerieInv;
        private ControlsUC.uc_MasterFind masterTipoInv;
        private ControlsUC.uc_MasterFind masterClaseInv;
        private ControlsUC.uc_MasterFind masterGrupoInv;
        private DevExpress.XtraEditors.SimpleButton btnFilter;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.GroupControl gbDetail;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.TextBox txtRefProveed;
        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraEditors.SimpleButton btnAccept;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;

    }
}