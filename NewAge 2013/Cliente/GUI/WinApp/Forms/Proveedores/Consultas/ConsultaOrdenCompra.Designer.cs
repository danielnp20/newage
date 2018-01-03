namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ConsultaOrdenCompra
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gbDetail = new DevExpress.XtraEditors.GroupControl();
            this.gcDocRelacion = new DevExpress.XtraGrid.GridControl();
            this.gvDocRelacion = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pgGrid = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Pagging();
            this.pnlValor = new DevExpress.XtraEditors.PanelControl();
            this.lblCurrencyFor = new System.Windows.Forms.Label();
            this.lblCurrencyLoc = new System.Windows.Forms.Label();
            this.lblValorUnit = new DevExpress.XtraEditors.LabelControl();
            this.lblValorTotal = new DevExpress.XtraEditors.LabelControl();
            this.txtValorTotalME = new DevExpress.XtraEditors.TextEdit();
            this.txtValorUnitML = new DevExpress.XtraEditors.TextEdit();
            this.txtValorTotalML = new DevExpress.XtraEditors.TextEdit();
            this.txtValorUnitME = new DevExpress.XtraEditors.TextEdit();
            this.gbQueryDoc = new DevExpress.XtraEditors.PanelControl();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.txtNro = new System.Windows.Forms.TextBox();
            this.lblNro = new System.Windows.Forms.Label();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.tlSeparatorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grpctrlHeader = new DevExpress.XtraEditors.GroupControl();
            this.grpboxHeader = new System.Windows.Forms.GroupBox();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.grpboxDetail = new System.Windows.Forms.GroupBox();
            this.gcDetCargos = new DevExpress.XtraGrid.GridControl();
            this.gvDetCargos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlGrids = new System.Windows.Forms.Panel();
            this.gbGridDocument = new System.Windows.Forms.GroupBox();
            this.gbDocHeader = new DevExpress.XtraEditors.GroupControl();
            this.gbGridProvider = new System.Windows.Forms.GroupBox();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.editCant = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.LinkEdit = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.LinkEditDocRelacion = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).BeginInit();
            this.gbDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocRelacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocRelacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlValor)).BeginInit();
            this.pnlValor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbQueryDoc)).BeginInit();
            this.gbQueryDoc.SuspendLayout();
            this.tlSeparatorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).BeginInit();
            this.grpctrlHeader.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetCargos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetCargos)).BeginInit();
            this.pnlGrids.SuspendLayout();
            this.gbGridDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbDocHeader)).BeginInit();
            this.gbDocHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEditDocRelacion)).BeginInit();
            this.SuspendLayout();
            // 
            // gvDetalle
            // 
            this.gvDetalle.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetalle.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Empty.Options.UseFont = true;
            this.gvDetalle.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetalle.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetalle.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvDetalle.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetalle.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetalle.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetalle.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetalle.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetalle.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetalle.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvDetalle.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvDetalle.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.Row.Options.UseBackColor = true;
            this.gvDetalle.Appearance.Row.Options.UseFont = true;
            this.gvDetalle.Appearance.Row.Options.UseForeColor = true;
            this.gvDetalle.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetalle.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetalle.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvDetalle.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetalle.Appearance.SelectedRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetalle.Appearance.TopNewRow.Options.UseFont = true;
            this.gvDetalle.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetalle.GridControl = this.gcDocument;
            this.gvDetalle.HorzScrollStep = 50;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetalle.OptionsBehavior.Editable = false;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsMenu.EnableColumnMenu = false;
            this.gvDetalle.OptionsMenu.EnableFooterMenu = false;
            this.gvDetalle.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetalle.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gvDetalle_RowClick);
            this.gvDetalle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalle_FocusedRowChanged);
            this.gvDetalle.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetalle_CustomUnboundColumnData);
            // 
            // gcDocument
            // 
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocument.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocument.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocument.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocument.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6, true, false, "", null),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvDetalle;
            gridLevelNode1.RelationName = "Detalle";
            this.gcDocument.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcDocument.Location = new System.Drawing.Point(2, 24);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(972, 265);
            this.gcDocument.TabIndex = 50;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument,
            this.gvDetalle});
            // 
            // gvDocument
            // 
            this.gvDocument.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocument.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocument.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocument.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocument.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocument.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocument.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocument.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocument.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocument.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocument.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocument.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocument.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.Row.Options.UseBackColor = true;
            this.gvDocument.Appearance.Row.Options.UseForeColor = true;
            this.gvDocument.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocument.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocument.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocument.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocument.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocument.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocument.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocument.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocument.GridControl = this.gcDocument;
            this.gvDocument.HorzScrollStep = 50;
            this.gvDocument.Name = "gvDocument";
            this.gvDocument.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDocument.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDocument.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocument.OptionsCustomization.AllowFilter = false;
            this.gvDocument.OptionsMenu.EnableColumnMenu = false;
            this.gvDocument.OptionsMenu.EnableFooterMenu = false;
            this.gvDocument.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocument.OptionsView.ColumnAutoWidth = false;
            this.gvDocument.OptionsView.ShowAutoFilterRow = true;
            this.gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocument.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDocument_FocusedRowChanged);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDocument.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocument_CustomColumnDisplayText);
            // 
            // gbDetail
            // 
            this.gbDetail.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbDetail.AppearanceCaption.Options.UseFont = true;
            this.gbDetail.Controls.Add(this.gcDocRelacion);
            this.gbDetail.Location = new System.Drawing.Point(6, 10);
            this.gbDetail.Name = "gbDetail";
            this.gbDetail.Size = new System.Drawing.Size(849, 206);
            this.gbDetail.TabIndex = 8;
            this.gbDetail.Text = "27311_gbDoc";
            // 
            // gcDocRelacion
            // 
            this.gcDocRelacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocRelacion.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocRelacion.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDocRelacion.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocRelacion.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocRelacion.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocRelacion.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocRelacion.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocRelacion.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocRelacion.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDocRelacion.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDocRelacion.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocRelacion.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocRelacion.Location = new System.Drawing.Point(2, 24);
            this.gcDocRelacion.LookAndFeel.SkinName = "Dark Side";
            this.gcDocRelacion.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocRelacion.MainView = this.gvDocRelacion;
            this.gcDocRelacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDocRelacion.Name = "gcDocRelacion";
            this.gcDocRelacion.Size = new System.Drawing.Size(845, 180);
            this.gcDocRelacion.TabIndex = 12;
            this.gcDocRelacion.UseEmbeddedNavigator = true;
            this.gcDocRelacion.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocRelacion});
            // 
            // gvDocRelacion
            // 
            this.gvDocRelacion.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocRelacion.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDocRelacion.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDocRelacion.Appearance.Empty.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDocRelacion.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocRelacion.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDocRelacion.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDocRelacion.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocRelacion.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocRelacion.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDocRelacion.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocRelacion.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDocRelacion.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDocRelacion.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDocRelacion.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDocRelacion.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDocRelacion.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDocRelacion.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDocRelacion.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDocRelacion.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDocRelacion.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDocRelacion.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDocRelacion.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocRelacion.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDocRelacion.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDocRelacion.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocRelacion.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDocRelacion.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDocRelacion.Appearance.Row.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.Row.Options.UseForeColor = true;
            this.gvDocRelacion.Appearance.Row.Options.UseTextOptions = true;
            this.gvDocRelacion.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDocRelacion.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDocRelacion.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDocRelacion.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDocRelacion.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDocRelacion.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDocRelacion.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDocRelacion.GridControl = this.gcDocRelacion;
            this.gvDocRelacion.Name = "gvDocRelacion";
            this.gvDocRelacion.OptionsCustomization.AllowColumnMoving = false;
            this.gvDocRelacion.OptionsCustomization.AllowFilter = false;
            this.gvDocRelacion.OptionsCustomization.AllowSort = false;
            this.gvDocRelacion.OptionsMenu.EnableColumnMenu = false;
            this.gvDocRelacion.OptionsMenu.EnableFooterMenu = false;
            this.gvDocRelacion.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocRelacion.OptionsView.ShowGroupPanel = false;
            this.gvDocRelacion.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            this.gvDocRelacion.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.gvDocument_CustomColumnDisplayText);
            // 
            // pgGrid
            // 
            this.pgGrid.Count = ((long)(0));
            this.pgGrid.Location = new System.Drawing.Point(-1, 315);
            this.pgGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pgGrid.Name = "pgGrid";
            this.pgGrid.PageCount = 0;
            this.pgGrid.PageNumber = 0;
            this.pgGrid.PageSize = 0;
            this.pgGrid.Size = new System.Drawing.Size(472, 26);
            this.pgGrid.TabIndex = 9;
            this.pgGrid.Visible = false;
            // 
            // pnlValor
            // 
            this.pnlValor.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.pnlValor.Appearance.Options.UseBackColor = true;
            this.pnlValor.Controls.Add(this.lblCurrencyFor);
            this.pnlValor.Controls.Add(this.lblCurrencyLoc);
            this.pnlValor.Controls.Add(this.lblValorUnit);
            this.pnlValor.Controls.Add(this.lblValorTotal);
            this.pnlValor.Controls.Add(this.txtValorTotalME);
            this.pnlValor.Controls.Add(this.txtValorUnitML);
            this.pnlValor.Controls.Add(this.txtValorTotalML);
            this.pnlValor.Controls.Add(this.txtValorUnitME);
            this.pnlValor.Location = new System.Drawing.Point(30, 558);
            this.pnlValor.LookAndFeel.SkinName = "McSkin";
            this.pnlValor.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnlValor.Name = "pnlValor";
            this.pnlValor.Size = new System.Drawing.Size(880, 69);
            this.pnlValor.TabIndex = 10;
            // 
            // lblCurrencyFor
            // 
            this.lblCurrencyFor.BackColor = System.Drawing.Color.LightGray;
            this.lblCurrencyFor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyFor.Location = new System.Drawing.Point(289, 6);
            this.lblCurrencyFor.Name = "lblCurrencyFor";
            this.lblCurrencyFor.Size = new System.Drawing.Size(150, 14);
            this.lblCurrencyFor.TabIndex = 23;
            this.lblCurrencyFor.Text = "26310_lblCurrencyForeign";
            this.lblCurrencyFor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrencyLoc
            // 
            this.lblCurrencyLoc.BackColor = System.Drawing.Color.LightGray;
            this.lblCurrencyLoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrencyLoc.Location = new System.Drawing.Point(123, 6);
            this.lblCurrencyLoc.Name = "lblCurrencyLoc";
            this.lblCurrencyLoc.Size = new System.Drawing.Size(150, 14);
            this.lblCurrencyLoc.TabIndex = 22;
            this.lblCurrencyLoc.Text = "26310_lblCurrencyLocal";
            this.lblCurrencyLoc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblValorUnit
            // 
            this.lblValorUnit.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblValorUnit.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorUnit.Location = new System.Drawing.Point(22, 25);
            this.lblValorUnit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblValorUnit.Name = "lblValorUnit";
            this.lblValorUnit.Size = new System.Drawing.Size(105, 14);
            this.lblValorUnit.TabIndex = 19;
            this.lblValorUnit.Text = "26310_lblValueUnit";
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lblValorTotal.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTotal.Location = new System.Drawing.Point(22, 45);
            this.lblValorTotal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(111, 14);
            this.lblValorTotal.TabIndex = 18;
            this.lblValorTotal.Text = "26310_lblValueTotal";
            // 
            // txtValorTotalME
            // 
            this.txtValorTotalME.EditValue = "0";
            this.txtValorTotalME.Location = new System.Drawing.Point(289, 42);
            this.txtValorTotalME.Name = "txtValorTotalME";
            this.txtValorTotalME.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorTotalME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
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
            this.txtValorTotalME.Size = new System.Drawing.Size(150, 21);
            this.txtValorTotalME.TabIndex = 17;
            // 
            // txtValorUnitML
            // 
            this.txtValorUnitML.EditValue = "0";
            this.txtValorUnitML.Location = new System.Drawing.Point(122, 21);
            this.txtValorUnitML.Name = "txtValorUnitML";
            this.txtValorUnitML.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorUnitML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
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
            this.txtValorUnitML.Size = new System.Drawing.Size(150, 21);
            this.txtValorUnitML.TabIndex = 13;
            // 
            // txtValorTotalML
            // 
            this.txtValorTotalML.EditValue = "0";
            this.txtValorTotalML.Location = new System.Drawing.Point(122, 42);
            this.txtValorTotalML.Name = "txtValorTotalML";
            this.txtValorTotalML.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorTotalML.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
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
            this.txtValorTotalML.Size = new System.Drawing.Size(150, 21);
            this.txtValorTotalML.TabIndex = 16;
            // 
            // txtValorUnitME
            // 
            this.txtValorUnitME.EditValue = "0";
            this.txtValorUnitME.Location = new System.Drawing.Point(289, 21);
            this.txtValorUnitME.Name = "txtValorUnitME";
            this.txtValorUnitME.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorUnitME.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 10F);
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
            this.txtValorUnitME.Size = new System.Drawing.Size(150, 21);
            this.txtValorUnitME.TabIndex = 14;
            // 
            // gbQueryDoc
            // 
            this.gbQueryDoc.Controls.Add(this.btnQueryDoc);
            this.gbQueryDoc.Controls.Add(this.txtNro);
            this.gbQueryDoc.Controls.Add(this.lblNro);
            this.gbQueryDoc.Controls.Add(this.masterPrefijo);
            this.gbQueryDoc.Location = new System.Drawing.Point(6, 14);
            this.gbQueryDoc.Name = "gbQueryDoc";
            this.gbQueryDoc.Size = new System.Drawing.Size(554, 32);
            this.gbQueryDoc.TabIndex = 32;
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(507, 6);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(28, 20);
            this.btnQueryDoc.TabIndex = 21427;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // txtNro
            // 
            this.txtNro.Location = new System.Drawing.Point(429, 6);
            this.txtNro.Name = "txtNro";
            this.txtNro.Size = new System.Drawing.Size(74, 21);
            this.txtNro.TabIndex = 12;
            this.txtNro.Text = "0";
            this.txtNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNro_KeyPress);
            this.txtNro.Leave += new System.EventHandler(this.txtNro_Leave);
            // 
            // lblNro
            // 
            this.lblNro.AutoSize = true;
            this.lblNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNro.Location = new System.Drawing.Point(305, 9);
            this.lblNro.Name = "lblNro";
            this.lblNro.Size = new System.Drawing.Size(154, 14);
            this.lblNro.TabIndex = 11;
            this.lblNro.Text = "27311_lblOrdenCompraNro";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(7, 4);
            this.masterPrefijo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(308, 25);
            this.masterPrefijo.TabIndex = 10;
            this.masterPrefijo.Value = "";
            // 
            // tlSeparatorPanel
            // 
            this.tlSeparatorPanel.ColumnCount = 3;
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlSeparatorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlSeparatorPanel.Controls.Add(this.grpctrlHeader, 1, 0);
            this.tlSeparatorPanel.Controls.Add(this.pnlDetail, 1, 2);
            this.tlSeparatorPanel.Controls.Add(this.pnlGrids, 1, 1);
            this.tlSeparatorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlSeparatorPanel.Location = new System.Drawing.Point(0, 0);
            this.tlSeparatorPanel.Name = "tlSeparatorPanel";
            this.tlSeparatorPanel.RowCount = 3;
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.76471F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.23529F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlSeparatorPanel.Size = new System.Drawing.Size(1022, 602);
            this.tlSeparatorPanel.TabIndex = 55;
            // 
            // grpctrlHeader
            // 
            this.grpctrlHeader.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.grpctrlHeader.Appearance.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.grpctrlHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.grpctrlHeader.AppearanceCaption.Options.UseBackColor = true;
            this.grpctrlHeader.AppearanceCaption.Options.UseFont = true;
            this.grpctrlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.grpctrlHeader.Controls.Add(this.grpboxHeader);
            this.grpctrlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpctrlHeader.Location = new System.Drawing.Point(13, 3);
            this.grpctrlHeader.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grpctrlHeader.Name = "grpctrlHeader";
            this.grpctrlHeader.ShowCaption = false;
            this.grpctrlHeader.Size = new System.Drawing.Size(998, 52);
            this.grpctrlHeader.TabIndex = 0;
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.BackColor = System.Drawing.Color.Transparent;
            this.grpboxHeader.Controls.Add(this.gbQueryDoc);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 2);
            this.grpboxHeader.Name = "grpboxHeader";
            this.grpboxHeader.Size = new System.Drawing.Size(994, 48);
            this.grpboxHeader.TabIndex = 8;
            this.grpboxHeader.TabStop = false;
            // 
            // pnlDetail
            // 
            this.pnlDetail.Controls.Add(this.grpboxDetail);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(13, 374);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(998, 225);
            this.pnlDetail.TabIndex = 112;
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.BackColor = System.Drawing.Color.Transparent;
            this.grpboxDetail.Controls.Add(this.gcDetCargos);
            this.grpboxDetail.Controls.Add(this.gbDetail);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Name = "grpboxDetail";
            this.grpboxDetail.Size = new System.Drawing.Size(998, 225);
            this.grpboxDetail.TabIndex = 68;
            this.grpboxDetail.TabStop = false;
            // 
            // gcDetCargos
            // 
            this.gcDetCargos.AllowDrop = true;
            this.gcDetCargos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetCargos.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetCargos.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetCargos.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetCargos.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetCargos.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetCargos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetCargos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetCargos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetCargos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetCargos.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDetCargos.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcDetCargos.EmbeddedNavigator.TextStringFormat = " Registro {0} de {1}";
            this.gcDetCargos.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetCargos.Location = new System.Drawing.Point(861, 40);
            this.gcDetCargos.LookAndFeel.SkinName = "Dark Side";
            this.gcDetCargos.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetCargos.MainView = this.gvDetCargos;
            this.gcDetCargos.Name = "gcDetCargos";
            this.gcDetCargos.Size = new System.Drawing.Size(137, 177);
            this.gcDetCargos.TabIndex = 11;
            this.gcDetCargos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetCargos});
            // 
            // gvDetCargos
            // 
            this.gvDetCargos.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetCargos.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDetCargos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDetCargos.Appearance.Empty.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDetCargos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetCargos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDetCargos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDetCargos.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetCargos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetCargos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDetCargos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetCargos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDetCargos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDetCargos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDetCargos.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDetCargos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDetCargos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDetCargos.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDetCargos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDetCargos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDetCargos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDetCargos.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDetCargos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetCargos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDetCargos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDetCargos.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetCargos.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDetCargos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDetCargos.Appearance.Row.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.Row.Options.UseForeColor = true;
            this.gvDetCargos.Appearance.Row.Options.UseTextOptions = true;
            this.gvDetCargos.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDetCargos.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDetCargos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDetCargos.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDetCargos.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDetCargos.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDetCargos.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDetCargos.GridControl = this.gcDetCargos;
            this.gvDetCargos.Name = "gvDetCargos";
            this.gvDetCargos.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetCargos.OptionsCustomization.AllowFilter = false;
            this.gvDetCargos.OptionsCustomization.AllowSort = false;
            this.gvDetCargos.OptionsView.ShowGroupPanel = false;
            this.gvDetCargos.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // pnlGrids
            // 
            this.pnlGrids.Controls.Add(this.gbGridDocument);
            this.pnlGrids.Controls.Add(this.gbGridProvider);
            this.pnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrids.Location = new System.Drawing.Point(13, 61);
            this.pnlGrids.Name = "pnlGrids";
            this.pnlGrids.Size = new System.Drawing.Size(998, 307);
            this.pnlGrids.TabIndex = 113;
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.gbDocHeader);
            this.gbGridDocument.Controls.Add(this.pgGrid);
            this.gbGridDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGridDocument.Location = new System.Drawing.Point(0, 0);
            this.gbGridDocument.Name = "gbGridDocument";
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(6, 0, 6, 3);
            this.gbGridDocument.Size = new System.Drawing.Size(988, 307);
            this.gbGridDocument.TabIndex = 54;
            this.gbGridDocument.TabStop = false;
            // 
            // gbDocHeader
            // 
            this.gbDocHeader.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.gbDocHeader.AppearanceCaption.Options.UseFont = true;
            this.gbDocHeader.Controls.Add(this.gcDocument);
            this.gbDocHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDocHeader.Location = new System.Drawing.Point(6, 13);
            this.gbDocHeader.Name = "gbDocHeader";
            this.gbDocHeader.Size = new System.Drawing.Size(976, 291);
            this.gbDocHeader.TabIndex = 51;
            this.gbDocHeader.Text = "27311_gbDocHeader";
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbGridProvider.Location = new System.Drawing.Point(988, 0);
            this.gbGridProvider.Name = "gbGridProvider";
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.gbGridProvider.Size = new System.Drawing.Size(10, 307);
            this.gbGridProvider.TabIndex = 53;
            this.gbGridProvider.TabStop = false;
            this.gbGridProvider.Visible = false;
            // 
            // editValue
            // 
            this.editValue.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editValue.Mask.EditMask = "c2";
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.editCant,
            this.LinkEdit,
            this.LinkEditDocRelacion,
            this.editValue});
            // 
            // editChkBox
            // 
            this.editChkBox.DisplayValueChecked = "True";
            this.editChkBox.DisplayValueUnchecked = "False";
            this.editChkBox.GlyphAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.editChkBox.Name = "editChkBox";
            this.editChkBox.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // editCant
            // 
            this.editCant.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.editCant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editCant.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editCant.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Value;
            this.editCant.Mask.EditMask = "n2";
            this.editCant.Mask.UseMaskAsDisplayFormat = true;
            this.editCant.Name = "editCant";
            // 
            // LinkEdit
            // 
            this.LinkEdit.Caption = "27311_ViewDocument";
            this.LinkEdit.Name = "LinkEdit";
            this.LinkEdit.Click += new System.EventHandler(this.LinkEdit_Click);
            // 
            // LinkEditDocRelacion
            // 
            this.LinkEditDocRelacion.Caption = "27311_ViewDocument";
            this.LinkEditDocRelacion.Name = "LinkEditDocRelacion";
            this.LinkEditDocRelacion.Click += new System.EventHandler(this.LinkEditDocRelacion_Click);
            // 
            // ConsultaOrdenCompra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1022, 602);
            this.Controls.Add(this.tlSeparatorPanel);
            this.Controls.Add(this.pnlValor);
            this.Name = "ConsultaOrdenCompra";
            this.Text = "27311";
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbDetail)).EndInit();
            this.gbDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocRelacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocRelacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlValor)).EndInit();
            this.pnlValor.ResumeLayout(false);
            this.pnlValor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorTotalML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorUnitME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gbQueryDoc)).EndInit();
            this.gbQueryDoc.ResumeLayout(false);
            this.gbQueryDoc.PerformLayout();
            this.tlSeparatorPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpctrlHeader)).EndInit();
            this.grpctrlHeader.ResumeLayout(false);
            this.grpboxHeader.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            this.grpboxDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetCargos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetCargos)).EndInit();
            this.pnlGrids.ResumeLayout(false);
            this.gbGridDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbDocHeader)).EndInit();
            this.gbDocHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCant)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LinkEditDocRelacion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gbDetail;
        private ControlsUC.uc_Pagging pgGrid;
        private DevExpress.XtraEditors.PanelControl pnlValor;
        private DevExpress.XtraEditors.LabelControl lblValorUnit;
        private DevExpress.XtraEditors.LabelControl lblValorTotal;
        private DevExpress.XtraEditors.TextEdit txtValorTotalME;
        private DevExpress.XtraEditors.TextEdit txtValorUnitML;
        private DevExpress.XtraEditors.TextEdit txtValorTotalML;
        private DevExpress.XtraEditors.TextEdit txtValorUnitME;
        private System.Windows.Forms.Label lblCurrencyFor;
        private System.Windows.Forms.Label lblCurrencyLoc;
        private DevExpress.XtraGrid.GridControl gcDocRelacion;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocRelacion;
        private DevExpress.XtraEditors.PanelControl gbQueryDoc;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private System.Windows.Forms.TextBox txtNro;
        private System.Windows.Forms.Label lblNro;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private System.Windows.Forms.TableLayoutPanel tlSeparatorPanel;
        private DevExpress.XtraEditors.GroupControl grpctrlHeader;
        private System.Windows.Forms.GroupBox grpboxHeader;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.GroupBox grpboxDetail;
        private System.Windows.Forms.Panel pnlGrids;
        private System.Windows.Forms.GroupBox gbGridDocument;
        private DevExpress.XtraGrid.GridControl gcDocument;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        private System.Windows.Forms.GroupBox gbGridProvider;
        private DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editCant;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit LinkEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit LinkEditDocRelacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editValue;
        private DevExpress.XtraEditors.GroupControl gbDocHeader;
        private DevExpress.XtraGrid.GridControl gcDetCargos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetCargos;

    }
}