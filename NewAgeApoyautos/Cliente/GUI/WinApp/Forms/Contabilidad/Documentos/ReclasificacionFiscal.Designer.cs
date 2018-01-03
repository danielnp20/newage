namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ReclasificacionFiscal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReclasificacionFiscal));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.grpBoxOrigen = new System.Windows.Forms.GroupBox();
            this.lblContra = new System.Windows.Forms.Label();
            this.gcOrigen = new DevExpress.XtraGrid.GridControl();
            this.gvOrigen = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpBoxExcluye = new System.Windows.Forms.GroupBox();
            this.gcExcluye = new DevExpress.XtraGrid.GridControl();
            this.gvExcluye = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpBoxDestino = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtPorcentaje = new System.Windows.Forms.TextBox();
            this.gcDestino = new DevExpress.XtraGrid.GridControl();
            this.gvDestino = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.masterBalanceTipo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.RepositoryEdit = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editBtnGrid = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editCmb = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.editValue = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpBoxOrigen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcOrigen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrigen)).BeginInit();
            this.grpBoxExcluye.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcExcluye)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExcluye)).BeginInit();
            this.grpBoxDestino.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDestino)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDestino)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpBoxDestino, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.33829F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.24535F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.41636F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1080, 538);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.57728F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.42272F));
            this.tableLayoutPanel2.Controls.Add(this.grpBoxOrigen, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.grpBoxExcluye, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 64);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1074, 188);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // grpBoxOrigen
            // 
            this.grpBoxOrigen.Controls.Add(this.lblContra);
            this.grpBoxOrigen.Controls.Add(this.gcOrigen);
            this.grpBoxOrigen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxOrigen.Location = new System.Drawing.Point(3, 3);
            this.grpBoxOrigen.Name = "grpBoxOrigen";
            this.grpBoxOrigen.Size = new System.Drawing.Size(751, 182);
            this.grpBoxOrigen.TabIndex = 0;
            this.grpBoxOrigen.TabStop = false;
            this.grpBoxOrigen.Text = "20506_grpBoxOrigen";
            // 
            // lblContra
            // 
            this.lblContra.AutoSize = true;
            this.lblContra.Location = new System.Drawing.Point(433, 0);
            this.lblContra.Name = "lblContra";
            this.lblContra.Size = new System.Drawing.Size(116, 13);
            this.lblContra.TabIndex = 52;
            this.lblContra.Text = "20506_lblContrapartida";
            // 
            // gcOrigen
            // 
            this.gcOrigen.AllowDrop = true;
            this.gcOrigen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcOrigen.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcOrigen.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcOrigen.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcOrigen.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcOrigen.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcOrigen.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcOrigen.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcOrigen.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcOrigen.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcOrigen.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcOrigen.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcOrigen_EmbeddedNavigator_ButtonClick);
            this.gcOrigen.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcOrigen.Location = new System.Drawing.Point(3, 16);
            this.gcOrigen.LookAndFeel.SkinName = "Dark Side";
            this.gcOrigen.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcOrigen.MainView = this.gvOrigen;
            this.gcOrigen.Margin = new System.Windows.Forms.Padding(4);
            this.gcOrigen.Name = "gcOrigen";
            this.gcOrigen.Size = new System.Drawing.Size(745, 163);
            this.gcOrigen.TabIndex = 51;
            this.gcOrigen.UseEmbeddedNavigator = true;
            this.gcOrigen.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOrigen});
            // 
            // gvOrigen
            // 
            this.gvOrigen.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOrigen.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvOrigen.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvOrigen.Appearance.Empty.Options.UseBackColor = true;
            this.gvOrigen.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvOrigen.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvOrigen.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigen.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvOrigen.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvOrigen.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigen.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvOrigen.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvOrigen.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvOrigen.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvOrigen.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvOrigen.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvOrigen.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvOrigen.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvOrigen.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvOrigen.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvOrigen.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigen.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvOrigen.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvOrigen.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvOrigen.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvOrigen.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvOrigen.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvOrigen.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.Row.Options.UseBackColor = true;
            this.gvOrigen.Appearance.Row.Options.UseForeColor = true;
            this.gvOrigen.Appearance.Row.Options.UseTextOptions = true;
            this.gvOrigen.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvOrigen.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvOrigen.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvOrigen.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvOrigen.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvOrigen.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvOrigen.Appearance.VertLine.Options.UseBackColor = true;
            this.gvOrigen.GridControl = this.gcOrigen;
            this.gvOrigen.HorzScrollStep = 50;
            this.gvOrigen.Name = "gvOrigen";
            this.gvOrigen.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvOrigen.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvOrigen.OptionsCustomization.AllowColumnMoving = false;
            this.gvOrigen.OptionsCustomization.AllowFilter = false;
            this.gvOrigen.OptionsCustomization.AllowSort = false;
            this.gvOrigen.OptionsMenu.EnableColumnMenu = false;
            this.gvOrigen.OptionsMenu.EnableFooterMenu = false;
            this.gvOrigen.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvOrigen.OptionsView.ColumnAutoWidth = false;
            this.gvOrigen.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvOrigen.OptionsView.ShowGroupPanel = false;
            this.gvOrigen.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvOrigen.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvOrigen_CustomRowCellEdit);
            this.gvOrigen.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvOrigen_FocusedRowChanged);
            this.gvOrigen.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvOrigen_CellValueChanged);
            this.gvOrigen.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvOrigen_BeforeLeaveRow);
            this.gvOrigen.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // grpBoxExcluye
            // 
            this.grpBoxExcluye.Controls.Add(this.gcExcluye);
            this.grpBoxExcluye.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxExcluye.Location = new System.Drawing.Point(760, 3);
            this.grpBoxExcluye.Name = "grpBoxExcluye";
            this.grpBoxExcluye.Size = new System.Drawing.Size(311, 182);
            this.grpBoxExcluye.TabIndex = 1;
            this.grpBoxExcluye.TabStop = false;
            this.grpBoxExcluye.Text = "20506_grpBoxExcluye";
            // 
            // gcExcluye
            // 
            this.gcExcluye.AllowDrop = true;
            this.gcExcluye.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcExcluye.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcExcluye.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcExcluye.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcExcluye.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcExcluye.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcExcluye.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcExcluye.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcExcluye.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcExcluye.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcExcluye.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcExcluye.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcExcluye.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcExcluye.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcExcluye.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcExcluye_EmbeddedNavigator_ButtonClick);
            this.gcExcluye.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcExcluye.Location = new System.Drawing.Point(3, 16);
            this.gcExcluye.LookAndFeel.SkinName = "Dark Side";
            this.gcExcluye.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcExcluye.MainView = this.gvExcluye;
            this.gcExcluye.Margin = new System.Windows.Forms.Padding(4);
            this.gcExcluye.Name = "gcExcluye";
            this.gcExcluye.Size = new System.Drawing.Size(305, 163);
            this.gcExcluye.TabIndex = 52;
            this.gcExcluye.UseEmbeddedNavigator = true;
            this.gcExcluye.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExcluye});
            // 
            // gvExcluye
            // 
            this.gvExcluye.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvExcluye.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvExcluye.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvExcluye.Appearance.Empty.Options.UseBackColor = true;
            this.gvExcluye.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvExcluye.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvExcluye.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvExcluye.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvExcluye.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvExcluye.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvExcluye.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvExcluye.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvExcluye.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvExcluye.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvExcluye.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvExcluye.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvExcluye.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvExcluye.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvExcluye.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvExcluye.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvExcluye.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvExcluye.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvExcluye.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvExcluye.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvExcluye.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvExcluye.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvExcluye.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvExcluye.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvExcluye.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvExcluye.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvExcluye.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvExcluye.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvExcluye.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvExcluye.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvExcluye.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvExcluye.Appearance.Row.Options.UseBackColor = true;
            this.gvExcluye.Appearance.Row.Options.UseForeColor = true;
            this.gvExcluye.Appearance.Row.Options.UseTextOptions = true;
            this.gvExcluye.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvExcluye.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvExcluye.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvExcluye.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvExcluye.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvExcluye.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvExcluye.Appearance.VertLine.Options.UseBackColor = true;
            this.gvExcluye.GridControl = this.gcExcluye;
            this.gvExcluye.HorzScrollStep = 50;
            this.gvExcluye.Name = "gvExcluye";
            this.gvExcluye.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvExcluye.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvExcluye.OptionsCustomization.AllowColumnMoving = false;
            this.gvExcluye.OptionsCustomization.AllowFilter = false;
            this.gvExcluye.OptionsCustomization.AllowSort = false;
            this.gvExcluye.OptionsMenu.EnableColumnMenu = false;
            this.gvExcluye.OptionsMenu.EnableFooterMenu = false;
            this.gvExcluye.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvExcluye.OptionsView.ColumnAutoWidth = false;
            this.gvExcluye.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvExcluye.OptionsView.ShowGroupPanel = false;
            this.gvExcluye.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvExcluye.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvExcluye_CustomRowCellEdit);
            this.gvExcluye.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvExcluye_FocusedRowChanged);
            this.gvExcluye.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvExcluye_CellValueChanged);
            this.gvExcluye.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvExcluye_BeforeLeaveRow);
            this.gvExcluye.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // grpBoxDestino
            // 
            this.grpBoxDestino.Controls.Add(this.panel1);
            this.grpBoxDestino.Controls.Add(this.gcDestino);
            this.grpBoxDestino.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBoxDestino.Location = new System.Drawing.Point(3, 258);
            this.grpBoxDestino.Name = "grpBoxDestino";
            this.grpBoxDestino.Size = new System.Drawing.Size(1074, 277);
            this.grpBoxDestino.TabIndex = 1;
            this.grpBoxDestino.TabStop = false;
            this.grpBoxDestino.Text = "20506_grpBoxDestino";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTotal);
            this.panel1.Controls.Add(this.txtPorcentaje);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(419, 238);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 36);
            this.panel1.TabIndex = 55;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(16, 11);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(77, 13);
            this.lblTotal.TabIndex = 53;
            this.lblTotal.Text = "20506_lblTotal";
            // 
            // txtPorcentaje
            // 
            this.txtPorcentaje.Enabled = false;
            this.txtPorcentaje.Location = new System.Drawing.Point(103, 7);
            this.txtPorcentaje.Name = "txtPorcentaje";
            this.txtPorcentaje.Size = new System.Drawing.Size(67, 20);
            this.txtPorcentaje.TabIndex = 54;
            this.txtPorcentaje.Text = "0%";
            this.txtPorcentaje.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // gcDestino
            // 
            this.gcDestino.AllowDrop = true;
            this.gcDestino.Dock = System.Windows.Forms.DockStyle.Left;
            this.gcDestino.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDestino.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDestino.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDestino.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDestino.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDestino.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDestino.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDestino.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDestino.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDestino.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDestino.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcDestino.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDestino.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDestino.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gcDestino_EmbeddedNavigator_ButtonClick);
            this.gcDestino.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDestino.Location = new System.Drawing.Point(3, 16);
            this.gcDestino.LookAndFeel.SkinName = "Dark Side";
            this.gcDestino.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDestino.MainView = this.gvDestino;
            this.gcDestino.Margin = new System.Windows.Forms.Padding(4);
            this.gcDestino.Name = "gcDestino";
            this.gcDestino.Size = new System.Drawing.Size(416, 258);
            this.gcDestino.TabIndex = 52;
            this.gcDestino.UseEmbeddedNavigator = true;
            this.gcDestino.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDestino});
            // 
            // gvDestino
            // 
            this.gvDestino.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDestino.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvDestino.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDestino.Appearance.Empty.Options.UseBackColor = true;
            this.gvDestino.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvDestino.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDestino.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDestino.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvDestino.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDestino.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDestino.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDestino.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvDestino.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDestino.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDestino.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDestino.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDestino.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvDestino.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvDestino.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvDestino.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvDestino.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDestino.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDestino.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvDestino.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDestino.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvDestino.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvDestino.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvDestino.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDestino.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvDestino.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDestino.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDestino.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDestino.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvDestino.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvDestino.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvDestino.Appearance.Row.Options.UseBackColor = true;
            this.gvDestino.Appearance.Row.Options.UseForeColor = true;
            this.gvDestino.Appearance.Row.Options.UseTextOptions = true;
            this.gvDestino.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvDestino.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvDestino.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDestino.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvDestino.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvDestino.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvDestino.Appearance.VertLine.Options.UseBackColor = true;
            this.gvDestino.GridControl = this.gcDestino;
            this.gvDestino.HorzScrollStep = 50;
            this.gvDestino.Name = "gvDestino";
            this.gvDestino.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDestino.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDestino.OptionsCustomization.AllowColumnMoving = false;
            this.gvDestino.OptionsCustomization.AllowFilter = false;
            this.gvDestino.OptionsCustomization.AllowSort = false;
            this.gvDestino.OptionsMenu.EnableColumnMenu = false;
            this.gvDestino.OptionsMenu.EnableFooterMenu = false;
            this.gvDestino.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDestino.OptionsView.ColumnAutoWidth = false;
            this.gvDestino.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDestino.OptionsView.ShowGroupPanel = false;
            this.gvDestino.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDestino.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDestino_CustomRowCellEdit);
            this.gvDestino.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDestino_FocusedRowChanged);
            this.gvDestino.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDestino_CellValueChanged);
            this.gvDestino.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDestino_BeforeLeaveRow);
            this.gvDestino.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.36364F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.27273F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.54545F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1074, 55);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1.779026F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98.22097F));
            this.tableLayoutPanel4.Controls.Add(this.masterBalanceTipo, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 12);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1068, 31);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // masterBalanceTipo
            // 
            this.masterBalanceTipo.BackColor = System.Drawing.Color.Transparent;
            this.masterBalanceTipo.Filtros = null;
            this.masterBalanceTipo.Location = new System.Drawing.Point(21, 3);
            this.masterBalanceTipo.Name = "masterBalanceTipo";
            this.masterBalanceTipo.Size = new System.Drawing.Size(291, 25);
            this.masterBalanceTipo.TabIndex = 0;
            this.masterBalanceTipo.Value = "";
            this.masterBalanceTipo.Leave += new System.EventHandler(this.masterBalanceTipo_Leave);
            // 
            // RepositoryEdit
            // 
            this.RepositoryEdit.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editBtnGrid,
            this.editCmb,
            this.editValue});
            // 
            // editBtnGrid
            // 
            this.editBtnGrid.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("editBtnGrid.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.editBtnGrid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGrid.Name = "editBtnGrid";
            this.editBtnGrid.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.editBtnGrid_ButtonClick);
            // 
            // editCmb
            // 
            this.editCmb.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.editCmb.Name = "editCmb";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "P";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            this.editValue.Name = "editValue";
            // 
            // ReclasificacionFiscal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 538);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReclasificacionFiscal";
            this.Text = "ReclasificacionFiscal";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpBoxOrigen.ResumeLayout(false);
            this.grpBoxOrigen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcOrigen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOrigen)).EndInit();
            this.grpBoxExcluye.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcExcluye)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExcluye)).EndInit();
            this.grpBoxDestino.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDestino)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDestino)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox grpBoxOrigen;
        private System.Windows.Forms.GroupBox grpBoxExcluye;
        private System.Windows.Forms.GroupBox grpBoxDestino;
        protected DevExpress.XtraGrid.GridControl gcOrigen;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvOrigen;
        protected DevExpress.XtraGrid.GridControl gcExcluye;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvExcluye;
        protected DevExpress.XtraGrid.GridControl gcDestino;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDestino;
        private System.Windows.Forms.Label lblContra;
        private System.Windows.Forms.TextBox txtPorcentaje;
        private System.Windows.Forms.Label lblTotal;
        protected DevExpress.XtraEditors.Repository.PersistentRepository RepositoryEdit;
        protected DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGrid;
        protected DevExpress.XtraEditors.Repository.RepositoryItemComboBox editCmb;
        protected DevExpress.XtraEditors.Repository.RepositoryItemTextEdit editValue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private ControlsUC.uc_MasterFind masterBalanceTipo;

    }
}