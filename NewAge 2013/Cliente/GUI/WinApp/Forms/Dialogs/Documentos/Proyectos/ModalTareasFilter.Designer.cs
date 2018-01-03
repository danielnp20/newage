namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalTareasFilter
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
            this.gcTarea = new DevExpress.XtraGrid.GridControl();
            this.gvTarea = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.pnlFilter = new DevExpress.XtraEditors.PanelControl();
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
            this.gcRecurso = new DevExpress.XtraGrid.GridControl();
            this.gvRecurso = new DevExpress.XtraGrid.Views.Grid.GridView();
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
            ((System.ComponentModel.ISupportInitialize)(this.gcTarea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFilter)).BeginInit();
            this.pnlFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).BeginInit();
            this.gbDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).BeginInit();
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
            this.gbHeader.Controls.Add(this.gcTarea);
            this.gbHeader.Location = new System.Drawing.Point(4, 1);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(400, 314);
            this.gbHeader.TabIndex = 7;
            this.gbHeader.Text = "110_gbTarea";
            // 
            // gcTarea
            // 
            this.gcTarea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTarea.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcTarea.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcTarea.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcTarea.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcTarea.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcTarea.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcTarea.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcTarea.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcTarea.Location = new System.Drawing.Point(2, 24);
            this.gcTarea.LookAndFeel.SkinName = "Dark Side";
            this.gcTarea.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcTarea.MainView = this.gvTarea;
            this.gcTarea.Margin = new System.Windows.Forms.Padding(4);
            this.gcTarea.Name = "gcTarea";
            this.gcTarea.Size = new System.Drawing.Size(396, 288);
            this.gcTarea.TabIndex = 2;
            this.gcTarea.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTarea});
            // 
            // gvTarea
            // 
            this.gvTarea.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTarea.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvTarea.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.Empty.Options.UseBackColor = true;
            this.gvTarea.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvTarea.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvTarea.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvTarea.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvTarea.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvTarea.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvTarea.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvTarea.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvTarea.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvTarea.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvTarea.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTarea.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvTarea.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvTarea.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.Row.Options.UseBackColor = true;
            this.gvTarea.Appearance.Row.Options.UseForeColor = true;
            this.gvTarea.Appearance.Row.Options.UseTextOptions = true;
            this.gvTarea.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvTarea.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvTarea.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTarea.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvTarea.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvTarea.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvTarea.Appearance.VertLine.Options.UseBackColor = true;
            this.gvTarea.GridControl = this.gcTarea;
            this.gvTarea.Name = "gvTarea";
            this.gvTarea.OptionsCustomization.AllowColumnMoving = false;
            this.gvTarea.OptionsCustomization.AllowFilter = false;
            this.gvTarea.OptionsDetail.EnableMasterViewMode = false;
            this.gvTarea.OptionsMenu.EnableColumnMenu = false;
            this.gvTarea.OptionsMenu.EnableFooterMenu = false;
            this.gvTarea.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvTarea.OptionsView.ShowGroupPanel = false;
            this.gvTarea.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvTarea_RowClick);
            this.gvTarea.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvTarea_FocusedRowChanged);
            this.gvTarea.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvTarea_CustomUnboundColumnData);
            this.gvTarea.DoubleClick += new System.EventHandler(this.gvTarea_DoubleClick);
            // 
            // pgGrid
            // 
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(13, 314);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(419, 24);
            this.pgGrid.TabIndex = 9;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.masterTipoInv);
            this.pnlFilter.Controls.Add(this.masterClaseInv);
            this.pnlFilter.Controls.Add(this.masterGrupoInv);
            this.pnlFilter.Controls.Add(this.lblModelo);
            this.pnlFilter.Controls.Add(this.txtRefProveed);
            this.pnlFilter.Controls.Add(this.btnFilter);
            this.pnlFilter.Controls.Add(this.masterEmpaqueInv);
            this.pnlFilter.Controls.Add(this.masterUnidadInv);
            this.pnlFilter.Controls.Add(this.masterMarcaInv);
            this.pnlFilter.Controls.Add(this.masterMaterialInv);
            this.pnlFilter.Controls.Add(this.masterSerieInv);
            this.pnlFilter.Location = new System.Drawing.Point(407, 1);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(447, 143);
            this.pnlFilter.TabIndex = 12;
            // 
            // masterTipoInv
            // 
            this.masterTipoInv.BackColor = System.Drawing.Color.Transparent;
            this.masterTipoInv.Filtros = null;
            this.masterTipoInv.Location = new System.Drawing.Point(220, 71);
            this.masterTipoInv.Name = "masterTipoInv";
            this.masterTipoInv.Size = new System.Drawing.Size(271, 23);
            this.masterTipoInv.TabIndex = 26;
            this.masterTipoInv.Value = "";
            this.masterTipoInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterClaseInv
            // 
            this.masterClaseInv.BackColor = System.Drawing.Color.Transparent;
            this.masterClaseInv.Filtros = null;
            this.masterClaseInv.Location = new System.Drawing.Point(220, 48);
            this.masterClaseInv.Name = "masterClaseInv";
            this.masterClaseInv.Size = new System.Drawing.Size(271, 23);
            this.masterClaseInv.TabIndex = 25;
            this.masterClaseInv.Value = "";
            this.masterClaseInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterGrupoInv
            // 
            this.masterGrupoInv.BackColor = System.Drawing.Color.Transparent;
            this.masterGrupoInv.Filtros = null;
            this.masterGrupoInv.Location = new System.Drawing.Point(220, 25);
            this.masterGrupoInv.Name = "masterGrupoInv";
            this.masterGrupoInv.Size = new System.Drawing.Size(271, 23);
            this.masterGrupoInv.TabIndex = 24;
            this.masterGrupoInv.Value = "";
            this.masterGrupoInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // lblModelo
            // 
            this.lblModelo.BackColor = System.Drawing.Color.Transparent;
            this.lblModelo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblModelo.Location = new System.Drawing.Point(6, 7);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(78, 18);
            this.lblModelo.TabIndex = 43;
            this.lblModelo.Text = "110_lblModelo";
            this.lblModelo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRefProveed
            // 
            this.txtRefProveed.Location = new System.Drawing.Point(108, 4);
            this.txtRefProveed.Name = "txtRefProveed";
            this.txtRefProveed.Size = new System.Drawing.Size(97, 21);
            this.txtRefProveed.TabIndex = 45;
            this.txtRefProveed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // btnFilter
            // 
            this.btnFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilter.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnFilter.Appearance.Options.UseFont = true;
            this.btnFilter.Location = new System.Drawing.Point(318, 113);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(98, 23);
            this.btnFilter.TabIndex = 8;
            this.btnFilter.Text = "Filtrar";
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // masterEmpaqueInv
            // 
            this.masterEmpaqueInv.BackColor = System.Drawing.Color.Transparent;
            this.masterEmpaqueInv.Filtros = null;
            this.masterEmpaqueInv.Location = new System.Drawing.Point(9, 113);
            this.masterEmpaqueInv.Name = "masterEmpaqueInv";
            this.masterEmpaqueInv.Size = new System.Drawing.Size(271, 23);
            this.masterEmpaqueInv.TabIndex = 34;
            this.masterEmpaqueInv.Value = "";
            this.masterEmpaqueInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterUnidadInv
            // 
            this.masterUnidadInv.BackColor = System.Drawing.Color.Transparent;
            this.masterUnidadInv.Filtros = null;
            this.masterUnidadInv.Location = new System.Drawing.Point(9, 91);
            this.masterUnidadInv.Name = "masterUnidadInv";
            this.masterUnidadInv.Size = new System.Drawing.Size(271, 23);
            this.masterUnidadInv.TabIndex = 32;
            this.masterUnidadInv.Value = "";
            this.masterUnidadInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterMarcaInv
            // 
            this.masterMarcaInv.BackColor = System.Drawing.Color.Transparent;
            this.masterMarcaInv.Filtros = null;
            this.masterMarcaInv.Location = new System.Drawing.Point(9, 25);
            this.masterMarcaInv.Name = "masterMarcaInv";
            this.masterMarcaInv.Size = new System.Drawing.Size(271, 23);
            this.masterMarcaInv.TabIndex = 29;
            this.masterMarcaInv.Value = "";
            this.masterMarcaInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterMaterialInv
            // 
            this.masterMaterialInv.BackColor = System.Drawing.Color.Transparent;
            this.masterMaterialInv.Filtros = null;
            this.masterMaterialInv.Location = new System.Drawing.Point(9, 48);
            this.masterMaterialInv.Name = "masterMaterialInv";
            this.masterMaterialInv.Size = new System.Drawing.Size(271, 23);
            this.masterMaterialInv.TabIndex = 28;
            this.masterMaterialInv.Value = "";
            this.masterMaterialInv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // masterSerieInv
            // 
            this.masterSerieInv.BackColor = System.Drawing.Color.Transparent;
            this.masterSerieInv.Filtros = null;
            this.masterSerieInv.Location = new System.Drawing.Point(9, 69);
            this.masterSerieInv.Name = "masterSerieInv";
            this.masterSerieInv.Size = new System.Drawing.Size(271, 23);
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
            this.gbDetail.AppearanceCaption.Options.UseTextOptions = true;
            this.gbDetail.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gbDetail.Controls.Add(this.gcRecurso);
            this.gbDetail.Location = new System.Drawing.Point(407, 147);
            this.gbDetail.Name = "gbDetail";
            this.gbDetail.Size = new System.Drawing.Size(449, 205);
            this.gbDetail.TabIndex = 8;
            this.gbDetail.Text = "110_gbRecurso";
            // 
            // gcRecurso
            // 
            this.gcRecurso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcRecurso.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcRecurso.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcRecurso.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcRecurso.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcRecurso.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcRecurso.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcRecurso.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcRecurso.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcRecurso.Location = new System.Drawing.Point(2, 24);
            this.gcRecurso.LookAndFeel.SkinName = "Dark Side";
            this.gcRecurso.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcRecurso.MainView = this.gvRecurso;
            this.gcRecurso.Margin = new System.Windows.Forms.Padding(4);
            this.gcRecurso.Name = "gcRecurso";
            this.gcRecurso.Size = new System.Drawing.Size(445, 179);
            this.gcRecurso.TabIndex = 3;
            this.gcRecurso.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRecurso});
            // 
            // gvRecurso
            // 
            this.gvRecurso.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecurso.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvRecurso.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.Empty.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvRecurso.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvRecurso.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvRecurso.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvRecurso.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecurso.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvRecurso.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvRecurso.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvRecurso.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecurso.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvRecurso.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvRecurso.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.Row.Options.UseBackColor = true;
            this.gvRecurso.Appearance.Row.Options.UseForeColor = true;
            this.gvRecurso.Appearance.Row.Options.UseTextOptions = true;
            this.gvRecurso.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvRecurso.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecurso.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRecurso.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecurso.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvRecurso.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecurso.Appearance.VertLine.Options.UseBackColor = true;
            this.gvRecurso.GridControl = this.gcRecurso;
            this.gvRecurso.Name = "gvRecurso";
            this.gvRecurso.OptionsBehavior.Editable = false;
            this.gvRecurso.OptionsCustomization.AllowColumnMoving = false;
            this.gvRecurso.OptionsCustomization.AllowFilter = false;
            this.gvRecurso.OptionsDetail.EnableMasterViewMode = false;
            this.gvRecurso.OptionsMenu.EnableColumnMenu = false;
            this.gvRecurso.OptionsMenu.EnableFooterMenu = false;
            this.gvRecurso.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvRecurso.OptionsView.ColumnAutoWidth = false;
            this.gvRecurso.OptionsView.ShowGroupPanel = false;
            this.gvRecurso.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvTarea_CustomUnboundColumnData);
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccept.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnAccept.Appearance.Options.UseFont = true;
            this.btnAccept.Location = new System.Drawing.Point(676, 356);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(95, 23);
            this.btnAccept.TabIndex = 13;
            this.btnAccept.Text = "Aceptar";
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // lblCodigo
            // 
            this.lblCodigo.BackColor = System.Drawing.Color.Transparent;
            this.lblCodigo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.Location = new System.Drawing.Point(22, 353);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(83, 18);
            this.lblCodigo.TabIndex = 46;
            this.lblCodigo.Text = "110_lblCodigo";
            this.lblCodigo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(95, 352);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(69, 20);
            this.txtCodigo.TabIndex = 47;
            this.txtCodigo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // lblDesc
            // 
            this.lblDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblDesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.Location = new System.Drawing.Point(169, 353);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(83, 18);
            this.lblDesc.TabIndex = 48;
            this.lblDesc.Text = "110_lblDesc";
            this.lblDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(255, 352);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(135, 20);
            this.txtDesc.TabIndex = 49;
            this.txtDesc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 337);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Filtro:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(778, 356);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ModalTareasFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(871, 385);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.txtDesc);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.btnAccept);
            this.Controls.Add(this.gbDetail);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.gbHeader);
            this.Controls.Add(this.pgGrid);
            this.Controls.Add(this.lblCodigo);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(887, 424);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(887, 424);
            this.Name = "ModalTareasFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "110_frmTareas";
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnMvto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbHeader)).EndInit();
            this.gbHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcTarea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTarea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFilter)).EndInit();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).EndInit();
            this.gbDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcRecurso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecurso)).EndInit();
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
        private DevExpress.XtraEditors.PanelControl pnlFilter;
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
        private DevExpress.XtraGrid.GridControl gcTarea;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTarea;
        private DevExpress.XtraEditors.SimpleButton btnAccept;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraGrid.GridControl gcRecurso;
        private DevExpress.XtraGrid.Views.Grid.GridView gvRecurso;

    }
}