namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalCostosInsumo
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
        protected virtual void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gcRecursos = new DevExpress.XtraGrid.GridControl();
            this.gvRecursos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tbFilter = new System.Windows.Forms.TableLayoutPanel();
            this.pnGrid = new DevExpress.XtraEditors.PanelControl();
            this.pnPageGrid = new System.Windows.Forms.Panel();
            this.lblTareas = new System.Windows.Forms.Label();
            this.masterUnidad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editSpin = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editCant2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editSpinPorcen = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.toolTipGrid = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotalPresup = new DevExpress.XtraEditors.TextEdit();
            this.txtTotalPresupAIU = new DevExpress.XtraEditors.TextEdit();
            this.lblTotalAIU = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gcRecursos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecursos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnGrid)).BeginInit();
            this.pnGrid.SuspendLayout();
            this.pnPageGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPresup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPresupAIU.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gcRecursos
            // 
            this.gcRecursos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gcRecursos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcRecursos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcRecursos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcRecursos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcRecursos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcRecursos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcRecursos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcRecursos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcRecursos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcRecursos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcRecursos.EmbeddedNavigator.Buttons.Remove.ImageIndex = 7;
            this.gcRecursos.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcRecursos.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 6, true, false, "", null)});
            this.gcRecursos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcRecursos.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcRecursos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcRecursos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcRecursos.Location = new System.Drawing.Point(12, 27);
            this.gcRecursos.LookAndFeel.SkinName = "Dark Side";
            this.gcRecursos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcRecursos.MainView = this.gvRecursos;
            this.gcRecursos.Margin = new System.Windows.Forms.Padding(4, 4, 100, 4);
            this.gcRecursos.Name = "gcRecursos";
            this.gcRecursos.Size = new System.Drawing.Size(984, 555);
            this.gcRecursos.TabIndex = 0;
            this.toolTipGrid.SetToolTip(this.gcRecursos, "Haga doble clic para copiar el ítem");
            this.gcRecursos.UseEmbeddedNavigator = true;
            this.gcRecursos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRecursos});
            this.gcRecursos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gcRecurso_KeyDown);
            // 
            // gvRecursos
            // 
            this.gvRecursos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecursos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvRecursos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvRecursos.Appearance.Empty.Options.UseBackColor = true;
            this.gvRecursos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvRecursos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvRecursos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecursos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvRecursos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvRecursos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvRecursos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecursos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecursos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvRecursos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvRecursos.Appearance.GroupRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvRecursos.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvRecursos.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvRecursos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvRecursos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvRecursos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvRecursos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvRecursos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvRecursos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvRecursos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvRecursos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvRecursos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvRecursos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvRecursos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvRecursos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvRecursos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecursos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvRecursos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRecursos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvRecursos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecursos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvRecursos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvRecursos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvRecursos.Appearance.Row.Options.UseBackColor = true;
            this.gvRecursos.Appearance.Row.Options.UseForeColor = true;
            this.gvRecursos.Appearance.Row.Options.UseTextOptions = true;
            this.gvRecursos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvRecursos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvRecursos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRecursos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvRecursos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvRecursos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvRecursos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvRecursos.GridControl = this.gcRecursos;
            this.gvRecursos.GroupFormat = "[#image]{1} {2}";
            this.gvRecursos.HorzScrollStep = 50;
            this.gvRecursos.Name = "gvRecursos";
            this.gvRecursos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvRecursos.OptionsCustomization.AllowColumnMoving = false;
            this.gvRecursos.OptionsCustomization.AllowFilter = false;
            this.gvRecursos.OptionsCustomization.AllowSort = false;
            this.gvRecursos.OptionsDetail.EnableMasterViewMode = false;
            this.gvRecursos.OptionsFind.AlwaysVisible = true;
            this.gvRecursos.OptionsMenu.EnableColumnMenu = false;
            this.gvRecursos.OptionsMenu.EnableFooterMenu = false;
            this.gvRecursos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvRecursos.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvRecursos.OptionsView.ShowGroupPanel = false;
            this.gvRecursos.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvRecursos.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvRecurso_FocusedRowChanged);
            this.gvRecursos.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gRecurso_CellValueChanged);
            this.gvRecursos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gv_CustomUnboundColumnData);
            this.gvRecursos.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvRecurso_CustomColumnDisplayText);
            // 
            // tbFilter
            // 
            this.tbFilter.ColumnCount = 4;
            this.tbFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.875F));
            this.tbFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 93.125F));
            this.tbFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tbFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tbFilter.Location = new System.Drawing.Point(24, 81);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.RowCount = 4;
            this.tbFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbFilter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbFilter.Size = new System.Drawing.Size(607, 122);
            this.tbFilter.TabIndex = 0;
            // 
            // pnGrid
            // 
            this.pnGrid.Controls.Add(this.gcRecursos);
            this.pnGrid.Controls.Add(this.pnPageGrid);
            this.pnGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnGrid.Location = new System.Drawing.Point(0, 0);
            this.pnGrid.Name = "pnGrid";
            this.pnGrid.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.pnGrid.Size = new System.Drawing.Size(1008, 584);
            this.pnGrid.TabIndex = 36;
            // 
            // pnPageGrid
            // 
            this.pnPageGrid.BackColor = System.Drawing.Color.Transparent;
            this.pnPageGrid.Controls.Add(this.lblTareas);
            this.pnPageGrid.Controls.Add(this.masterUnidad);
            this.pnPageGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnPageGrid.Location = new System.Drawing.Point(12, 2);
            this.pnPageGrid.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.pnPageGrid.Name = "pnPageGrid";
            this.pnPageGrid.Size = new System.Drawing.Size(984, 25);
            this.pnPageGrid.TabIndex = 55;
            // 
            // lblTareas
            // 
            this.lblTareas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTareas.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTareas.Location = new System.Drawing.Point(0, 5);
            this.lblTareas.Name = "lblTareas";
            this.lblTareas.Size = new System.Drawing.Size(984, 16);
            this.lblTareas.TabIndex = 4;
            this.lblTareas.Text = "1044_lblInsumos";
            this.lblTareas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // masterUnidad
            // 
            this.masterUnidad.BackColor = System.Drawing.Color.Transparent;
            this.masterUnidad.Filtros = null;
            this.masterUnidad.Location = new System.Drawing.Point(15, 96);
            this.masterUnidad.Margin = new System.Windows.Forms.Padding(4);
            this.masterUnidad.Name = "masterUnidad";
            this.masterUnidad.Size = new System.Drawing.Size(352, 29);
            this.masterUnidad.TabIndex = 3;
            this.masterUnidad.Value = "";
            this.masterUnidad.Visible = false;
            this.masterUnidad.Leave += new System.EventHandler(this.master_Leave);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(865, 598);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(123, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "1044_btnCancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccept.Location = new System.Drawing.Point(738, 598);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(123, 24);
            this.btnAccept.TabIndex = 4;
            this.btnAccept.Text = "1044_btnAccept";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editSpin,
            this.editCant2,
            this.editSpinPorcen,
            this.editBtnGrid});
            // 
            // editChkBox
            // 
            this.editChkBox.Caption = "";
            this.editChkBox.DisplayValueChecked = "True";
            this.editChkBox.DisplayValueUnchecked = "False";
            this.editChkBox.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editChkBox.Name = "editChkBox";
            this.editChkBox.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.editChkBox.CheckedChanged += new System.EventHandler(this.editChek_CheckedChanged);
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
            // editCant2
            // 
            this.editCant2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editCant2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editCant2.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editCant2.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editCant2.Mask.EditMask = "n2";
            this.editCant2.Mask.UseMaskAsDisplayFormat = true;
            this.editCant2.Name = "editCant2";
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorcen.Name = "editSpinPorcen";
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnGrid_ButtonClick);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.pnGrid);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1008, 589);
            this.splitContainerControl1.SplitterPosition = 0;
            this.splitContainerControl1.TabIndex = 37;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(24, 603);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(120, 14);
            this.lblTotal.TabIndex = 110;
            this.lblTotal.Text = "1044_TotalPresup";
            this.lblTotal.Visible = false;
            // 
            // txtTotalPresup
            // 
            this.txtTotalPresup.EditValue = "0,00 ";
            this.txtTotalPresup.Location = new System.Drawing.Point(156, 601);
            this.txtTotalPresup.Name = "txtTotalPresup";
            this.txtTotalPresup.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.txtTotalPresup.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalPresup.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalPresup.Properties.Appearance.Options.UseBackColor = true;
            this.txtTotalPresup.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalPresup.Properties.Appearance.Options.UseFont = true;
            this.txtTotalPresup.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalPresup.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.txtTotalPresup.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtTotalPresup.Properties.AutoHeight = false;
            this.txtTotalPresup.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtTotalPresup.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalPresup.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalPresup.Properties.Mask.EditMask = "c2";
            this.txtTotalPresup.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalPresup.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalPresup.Properties.ReadOnly = true;
            this.txtTotalPresup.Size = new System.Drawing.Size(147, 18);
            this.txtTotalPresup.TabIndex = 111;
            this.txtTotalPresup.Visible = false;
            // 
            // txtTotalPresupAIU
            // 
            this.txtTotalPresupAIU.EditValue = "0,00 ";
            this.txtTotalPresupAIU.Location = new System.Drawing.Point(492, 601);
            this.txtTotalPresupAIU.Name = "txtTotalPresupAIU";
            this.txtTotalPresupAIU.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.txtTotalPresupAIU.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtTotalPresupAIU.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalPresupAIU.Properties.Appearance.Options.UseBackColor = true;
            this.txtTotalPresupAIU.Properties.Appearance.Options.UseBorderColor = true;
            this.txtTotalPresupAIU.Properties.Appearance.Options.UseFont = true;
            this.txtTotalPresupAIU.Properties.Appearance.Options.UseTextOptions = true;
            this.txtTotalPresupAIU.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.txtTotalPresupAIU.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.txtTotalPresupAIU.Properties.AutoHeight = false;
            this.txtTotalPresupAIU.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtTotalPresupAIU.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalPresupAIU.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtTotalPresupAIU.Properties.Mask.EditMask = "c2";
            this.txtTotalPresupAIU.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotalPresupAIU.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtTotalPresupAIU.Properties.ReadOnly = true;
            this.txtTotalPresupAIU.Size = new System.Drawing.Size(147, 18);
            this.txtTotalPresupAIU.TabIndex = 113;
            this.txtTotalPresupAIU.Visible = false;
            // 
            // lblTotalAIU
            // 
            this.lblTotalAIU.AutoSize = true;
            this.lblTotalAIU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAIU.Location = new System.Drawing.Point(315, 603);
            this.lblTotalAIU.Name = "lblTotalAIU";
            this.lblTotalAIU.Size = new System.Drawing.Size(142, 14);
            this.lblTotalAIU.TabIndex = 112;
            this.lblTotalAIU.Text = "1044_TotalPresupAIU";
            this.lblTotalAIU.Visible = false;
            // 
            // ModalCostosInsumo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1008, 631);
            this.Controls.Add(this.txtTotalPresupAIU);
            this.Controls.Add(this.lblTotalAIU);
            this.Controls.Add(this.txtTotalPresup);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.btnAccept);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimizeBox = false;
            this.Name = "ModalCostosInsumo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "1044_frmResumenIns";
            ((System.ComponentModel.ISupportInitialize)(this.gcRecursos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecursos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnGrid)).EndInit();
            this.pnGrid.ResumeLayout(false);
            this.pnPageGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPresup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalPresupAIU.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.TableLayoutPanel tbFilter;
        protected DevExpress.XtraEditors.PanelControl pnGrid;
        protected DevExpress.XtraGrid.GridControl gcRecursos;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvRecursos;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpin;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editCant2;
        protected DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorcen;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        private System.Windows.Forms.ToolTip toolTipGrid;
        protected System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Panel pnPageGrid;
        private ControlsUC.uc_MasterFind masterUnidad;
        private System.Windows.Forms.Label lblTareas;
        protected System.Windows.Forms.Button btnCancel;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private System.Windows.Forms.Label lblTotal;
        private DevExpress.XtraEditors.TextEdit txtTotalPresup;
        private DevExpress.XtraEditors.TextEdit txtTotalPresupAIU;
        private System.Windows.Forms.Label lblTotalAIU;

    }
}