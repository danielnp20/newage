namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class NotaCreditoCartera
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotaCreditoCartera));
            this.gvComponentes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcCuotas = new DevExpress.XtraGrid.GridControl();
            this.gvCuotas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dtFechaAplica = new DevExpress.XtraEditors.DateEdit();
            this.dtFechaDoc = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaDoc = new System.Windows.Forms.Label();
            this.lblFechaAplica = new System.Windows.Forms.Label();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLibranza = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.lblValorCuota = new DevExpress.XtraEditors.LabelControl();
            this.txtValorCuota = new DevExpress.XtraEditors.TextEdit();
            this.masterReintegroSaldo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblNC = new DevExpress.XtraEditors.LabelControl();
            this.txtVlrNC = new DevExpress.XtraEditors.TextEdit();
            this.lblNCObligacion = new DevExpress.XtraEditors.LabelControl();
            this.txtVlrNCObligac = new DevExpress.XtraEditors.TextEdit();
            this.lblNCOtros = new DevExpress.XtraEditors.LabelControl();
            this.txtVlrNCOtros = new DevExpress.XtraEditors.TextEdit();
            this.lblAjuste = new DevExpress.XtraEditors.LabelControl();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvComponentes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCuotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCuotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLibranza.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCuota.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrNC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrNCObligac.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrNCOtros.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.gcCuotas);
            this.grpboxDetail.Controls.Add(this.label1);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Size = new System.Drawing.Size(1116, 104);
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
            this.editSpin7,
            this.editDate,
            this.editValue,
            this.editValue4,
            this.editLink,
            this.editSpinPorcen,
            this.editSpin0});
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
            // editSpin7
            // 
            this.editSpin7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin7.Mask.EditMask = "c7";
            this.editSpin7.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editDate
            // 
            this.editDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.editDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.editDate.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.editDate.Mask.EditMask = "dd/MM/yyyy";
            // 
            // editValue
            // 
            this.editValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue.Mask.EditMask = "c";
            this.editValue.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editValue4
            // 
            this.editValue4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editValue4.Mask.EditMask = "c4";
            this.editValue4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editValue4.Mask.UseMaskAsDisplayFormat = true;
            // 
            // btnMark
            // 
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Size = new System.Drawing.Size(820, 357);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(820, 0);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 357);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.txtObservacion);
            this.grpboxHeader.Controls.Add(this.lblAjuste);
            this.grpboxHeader.Controls.Add(this.lblNCOtros);
            this.grpboxHeader.Controls.Add(this.txtVlrNCOtros);
            this.grpboxHeader.Controls.Add(this.lblNCObligacion);
            this.grpboxHeader.Controls.Add(this.txtVlrNCObligac);
            this.grpboxHeader.Controls.Add(this.lblNC);
            this.grpboxHeader.Controls.Add(this.txtVlrNC);
            this.grpboxHeader.Controls.Add(this.masterReintegroSaldo);
            this.grpboxHeader.Controls.Add(this.lblValorCuota);
            this.grpboxHeader.Controls.Add(this.txtValorCuota);
            this.grpboxHeader.Controls.Add(this.label2);
            this.grpboxHeader.Controls.Add(this.dtFechaAplica);
            this.grpboxHeader.Controls.Add(this.dtFechaDoc);
            this.grpboxHeader.Controls.Add(this.lblFechaDoc);
            this.grpboxHeader.Controls.Add(this.lblFechaAplica);
            this.grpboxHeader.Controls.Add(this.lblLibranza);
            this.grpboxHeader.Controls.Add(this.masterCliente);
            this.grpboxHeader.Controls.Add(this.txtLibranza);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Size = new System.Drawing.Size(1112, 217);
            this.grpboxHeader.TabIndex = 0;
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // editSpin0
            // 
            this.editSpin0.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpin0.Mask.EditMask = "c0";
            this.editSpin0.Mask.UseMaskAsDisplayFormat = true;
            // 
            // gvComponentes
            // 
            this.gvComponentes.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvComponentes.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvComponentes.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvComponentes.Appearance.Empty.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvComponentes.Appearance.Empty.Options.UseBackColor = true;
            this.gvComponentes.Appearance.Empty.Options.UseFont = true;
            this.gvComponentes.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvComponentes.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvComponentes.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvComponentes.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvComponentes.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvComponentes.Appearance.FocusedCell.Options.UseFont = true;
            this.gvComponentes.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvComponentes.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvComponentes.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvComponentes.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvComponentes.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvComponentes.Appearance.FocusedRow.Options.UseFont = true;
            this.gvComponentes.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvComponentes.Appearance.GroupRow.BackColor = System.Drawing.Color.SteelBlue;
            this.gvComponentes.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Black;
            this.gvComponentes.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvComponentes.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvComponentes.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvComponentes.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvComponentes.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvComponentes.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvComponentes.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvComponentes.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvComponentes.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvComponentes.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvComponentes.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvComponentes.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvComponentes.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvComponentes.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvComponentes.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvComponentes.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvComponentes.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvComponentes.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gvComponentes.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvComponentes.Appearance.Row.Options.UseBackColor = true;
            this.gvComponentes.Appearance.Row.Options.UseFont = true;
            this.gvComponentes.Appearance.Row.Options.UseForeColor = true;
            this.gvComponentes.Appearance.Row.Options.UseTextOptions = true;
            this.gvComponentes.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvComponentes.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gvComponentes.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvComponentes.Appearance.SelectedRow.Options.UseFont = true;
            this.gvComponentes.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvComponentes.Appearance.TopNewRow.Options.UseFont = true;
            this.gvComponentes.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvComponentes.GridControl = this.gcCuotas;
            this.gvComponentes.GroupFormat = "[#image]{1} {2}";
            this.gvComponentes.HorzScrollStep = 50;
            this.gvComponentes.Name = "gvComponentes";
            this.gvComponentes.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvComponentes.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvComponentes.OptionsBehavior.Editable = false;
            this.gvComponentes.OptionsCustomization.AllowColumnMoving = false;
            this.gvComponentes.OptionsCustomization.AllowFilter = false;
            this.gvComponentes.OptionsCustomization.AllowSort = false;
            this.gvComponentes.OptionsMenu.EnableColumnMenu = false;
            this.gvComponentes.OptionsMenu.EnableFooterMenu = false;
            this.gvComponentes.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvComponentes.OptionsView.ColumnAutoWidth = false;
            this.gvComponentes.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvComponentes.OptionsView.ShowGroupPanel = false;
            this.gvComponentes.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvComponentes.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // gcCuotas
            // 
            this.gcCuotas.AllowDrop = true;
            this.gcCuotas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCuotas.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCuotas.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcCuotas.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcCuotas.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcCuotas.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcCuotas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcCuotas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcCuotas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcCuotas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcCuotas.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6)});
            this.gcCuotas.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
            this.gcCuotas.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcCuotas.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcCuotas.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridLevelNode1.LevelTemplate = this.gvComponentes;
            gridLevelNode1.RelationName = "DetalleComp";
            this.gcCuotas.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcCuotas.Location = new System.Drawing.Point(3, 31);
            this.gcCuotas.LookAndFeel.SkinName = "Dark Side";
            this.gcCuotas.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcCuotas.MainView = this.gvCuotas;
            this.gcCuotas.Margin = new System.Windows.Forms.Padding(5);
            this.gcCuotas.Name = "gcCuotas";
            this.gcCuotas.Size = new System.Drawing.Size(1110, 70);
            this.gcCuotas.TabIndex = 52;
            this.gcCuotas.UseEmbeddedNavigator = true;
            this.gcCuotas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCuotas,
            this.gvComponentes});
            // 
            // gvCuotas
            // 
            this.gvCuotas.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCuotas.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvCuotas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvCuotas.Appearance.Empty.Options.UseBackColor = true;
            this.gvCuotas.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvCuotas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvCuotas.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuotas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvCuotas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvCuotas.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuotas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvCuotas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvCuotas.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.gvCuotas.Appearance.FooterPanel.Options.UseFont = true;
            this.gvCuotas.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gvCuotas.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.GroupFooter.Image = ((System.Drawing.Image)(resources.GetObject("gvCuotas.Appearance.GroupFooter.Image")));
            this.gvCuotas.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvCuotas.Appearance.GroupFooter.Options.UseImage = true;
            this.gvCuotas.Appearance.GroupFooter.Options.UseTextOptions = true;
            this.gvCuotas.Appearance.GroupRow.BackColor = System.Drawing.Color.SteelBlue;
            this.gvCuotas.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.GroupRow.ForeColor = System.Drawing.Color.White;
            this.gvCuotas.Appearance.GroupRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvCuotas.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvCuotas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvCuotas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvCuotas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvCuotas.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvCuotas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvCuotas.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvCuotas.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCuotas.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvCuotas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuotas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvCuotas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvCuotas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvCuotas.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCuotas.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvCuotas.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvCuotas.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9.5F);
            this.gvCuotas.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.Row.Options.UseBackColor = true;
            this.gvCuotas.Appearance.Row.Options.UseFont = true;
            this.gvCuotas.Appearance.Row.Options.UseForeColor = true;
            this.gvCuotas.Appearance.Row.Options.UseTextOptions = true;
            this.gvCuotas.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvCuotas.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvCuotas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvCuotas.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvCuotas.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvCuotas.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvCuotas.Appearance.VertLine.Options.UseBackColor = true;
            this.gvCuotas.GridControl = this.gcCuotas;
            this.gvCuotas.GroupFormat = "[#image]{1} {2}";
            this.gvCuotas.GroupRowHeight = 15;
            this.gvCuotas.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Unbound_CostoLocalTOT", null, "(SubTotal={0:c2})")});
            this.gvCuotas.HorzScrollStep = 50;
            this.gvCuotas.Name = "gvCuotas";
            this.gvCuotas.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.gvCuotas.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvCuotas.OptionsCustomization.AllowFilter = false;
            this.gvCuotas.OptionsCustomization.AllowSort = false;
            this.gvCuotas.OptionsMenu.EnableColumnMenu = false;
            this.gvCuotas.OptionsMenu.EnableFooterMenu = false;
            this.gvCuotas.OptionsView.ColumnAutoWidth = false;
            this.gvCuotas.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvCuotas.OptionsView.ShowGroupPanel = false;
            this.gvCuotas.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvCuotas.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvCuotas_FocusedRowChanged);
            this.gvCuotas.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // dtFechaAplica
            // 
            this.dtFechaAplica.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaAplica.Location = new System.Drawing.Point(504, 36);
            this.dtFechaAplica.Name = "dtFechaAplica";
            this.dtFechaAplica.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaAplica.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaAplica.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaAplica.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaAplica.Properties.Appearance.Options.UseFont = true;
            this.dtFechaAplica.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaAplica.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaAplica.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaAplica.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaAplica.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaAplica.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaAplica.Size = new System.Drawing.Size(117, 20);
            this.dtFechaAplica.TabIndex = 3;
            // 
            // dtFechaDoc
            // 
            this.dtFechaDoc.EditValue = new System.DateTime(2013, 8, 18, 0, 0, 0, 0);
            this.dtFechaDoc.Location = new System.Drawing.Point(503, 12);
            this.dtFechaDoc.Name = "dtFechaDoc";
            this.dtFechaDoc.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtFechaDoc.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaDoc.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFechaDoc.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaDoc.Properties.Appearance.Options.UseFont = true;
            this.dtFechaDoc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaDoc.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaDoc.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaDoc.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaDoc.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaDoc.Size = new System.Drawing.Size(117, 20);
            this.dtFechaDoc.TabIndex = 5;
            // 
            // lblFechaDoc
            // 
            this.lblFechaDoc.AutoSize = true;
            this.lblFechaDoc.Location = new System.Drawing.Point(401, 16);
            this.lblFechaDoc.Name = "lblFechaDoc";
            this.lblFechaDoc.Size = new System.Drawing.Size(78, 13);
            this.lblFechaDoc.TabIndex = 4;
            this.lblFechaDoc.Text = "184_FechaDoc";
            // 
            // lblFechaAplica
            // 
            this.lblFechaAplica.AutoSize = true;
            this.lblFechaAplica.Location = new System.Drawing.Point(401, 39);
            this.lblFechaAplica.Name = "lblFechaAplica";
            this.lblFechaAplica.Size = new System.Drawing.Size(88, 13);
            this.lblFechaAplica.TabIndex = 2;
            this.lblFechaAplica.Text = "184_FechaAplica";
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Location = new System.Drawing.Point(10, 15);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(71, 13);
            this.lblLibranza.TabIndex = 7;
            this.lblLibranza.Text = "184_Libranza";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Location = new System.Drawing.Point(14, 34);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(305, 25);
            this.masterCliente.TabIndex = 6;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "Detalle Cuotas";
            // 
            // txtLibranza
            // 
            this.txtLibranza.Location = new System.Drawing.Point(115, 12);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Properties.NullText = " ";
            this.txtLibranza.Size = new System.Drawing.Size(99, 20);
            this.txtLibranza.TabIndex = 8;
            this.txtLibranza.Leave += new System.EventHandler(this.txtLibranzas_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "184_lblObservacion";
            // 
            // lblValorCuota
            // 
            this.lblValorCuota.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblValorCuota.Location = new System.Drawing.Point(404, 63);
            this.lblValorCuota.Name = "lblValorCuota";
            this.lblValorCuota.Size = new System.Drawing.Size(87, 13);
            this.lblValorCuota.TabIndex = 96;
            this.lblValorCuota.Text = "184_lblValorCuota";
            // 
            // txtValorCuota
            // 
            this.txtValorCuota.EditValue = "0,00 ";
            this.txtValorCuota.Location = new System.Drawing.Point(504, 60);
            this.txtValorCuota.Name = "txtValorCuota";
            this.txtValorCuota.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtValorCuota.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorCuota.Properties.Appearance.Options.UseBorderColor = true;
            this.txtValorCuota.Properties.Appearance.Options.UseFont = true;
            this.txtValorCuota.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorCuota.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorCuota.Properties.AutoHeight = false;
            this.txtValorCuota.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorCuota.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorCuota.Properties.Mask.EditMask = "c";
            this.txtValorCuota.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorCuota.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorCuota.Size = new System.Drawing.Size(116, 20);
            this.txtValorCuota.TabIndex = 95;
            // 
            // masterReintegroSaldo
            // 
            this.masterReintegroSaldo.BackColor = System.Drawing.Color.Transparent;
            this.masterReintegroSaldo.Filtros = null;
            this.masterReintegroSaldo.Location = new System.Drawing.Point(404, 84);
            this.masterReintegroSaldo.Name = "masterReintegroSaldo";
            this.masterReintegroSaldo.Size = new System.Drawing.Size(305, 25);
            this.masterReintegroSaldo.TabIndex = 97;
            this.masterReintegroSaldo.Value = "";
            // 
            // lblNC
            // 
            this.lblNC.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNC.Location = new System.Drawing.Point(670, 16);
            this.lblNC.Name = "lblNC";
            this.lblNC.Size = new System.Drawing.Size(48, 13);
            this.lblNC.TabIndex = 99;
            this.lblNC.Text = "184_lblNC";
            // 
            // txtNC
            // 
            this.txtVlrNC.EditValue = "0,00 ";
            this.txtVlrNC.Location = new System.Drawing.Point(770, 12);
            this.txtVlrNC.Name = "txtNC";
            this.txtVlrNC.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrNC.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrNC.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrNC.Properties.Appearance.Options.UseFont = true;
            this.txtVlrNC.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrNC.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrNC.Properties.AutoHeight = false;
            this.txtVlrNC.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrNC.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrNC.Properties.Mask.EditMask = "c";
            this.txtVlrNC.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrNC.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrNC.Size = new System.Drawing.Size(116, 20);
            this.txtVlrNC.TabIndex = 98;
            this.txtVlrNC.EditValueChanged += new System.EventHandler(this.txtNC_EditValueChanged);
            // 
            // lblNCObligacion
            // 
            this.lblNCObligacion.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNCObligacion.Location = new System.Drawing.Point(670, 41);
            this.lblNCObligacion.Name = "lblNCObligacion";
            this.lblNCObligacion.Size = new System.Drawing.Size(83, 13);
            this.lblNCObligacion.TabIndex = 101;
            this.lblNCObligacion.Text = "184_lblNCObligac";
            // 
            // txtNCObligac
            // 
            this.txtVlrNCObligac.EditValue = "0,00 ";
            this.txtVlrNCObligac.Location = new System.Drawing.Point(770, 37);
            this.txtVlrNCObligac.Name = "txtNCObligac";
            this.txtVlrNCObligac.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrNCObligac.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrNCObligac.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrNCObligac.Properties.Appearance.Options.UseFont = true;
            this.txtVlrNCObligac.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrNCObligac.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrNCObligac.Properties.AutoHeight = false;
            this.txtVlrNCObligac.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrNCObligac.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrNCObligac.Properties.Mask.EditMask = "c";
            this.txtVlrNCObligac.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrNCObligac.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrNCObligac.Size = new System.Drawing.Size(116, 20);
            this.txtVlrNCObligac.TabIndex = 100;
            // 
            // lblNCOtros
            // 
            this.lblNCOtros.Appearance.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.lblNCOtros.Location = new System.Drawing.Point(670, 64);
            this.lblNCOtros.Name = "lblNCOtros";
            this.lblNCOtros.Size = new System.Drawing.Size(75, 13);
            this.lblNCOtros.TabIndex = 103;
            this.lblNCOtros.Text = "184_lblNCOtros";
            // 
            // txtNCOtros
            // 
            this.txtVlrNCOtros.EditValue = "0,00 ";
            this.txtVlrNCOtros.Location = new System.Drawing.Point(770, 62);
            this.txtVlrNCOtros.Name = "txtNCOtros";
            this.txtVlrNCOtros.Properties.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.txtVlrNCOtros.Properties.Appearance.Font = new System.Drawing.Font("Arial Narrow", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVlrNCOtros.Properties.Appearance.Options.UseBorderColor = true;
            this.txtVlrNCOtros.Properties.Appearance.Options.UseFont = true;
            this.txtVlrNCOtros.Properties.Appearance.Options.UseTextOptions = true;
            this.txtVlrNCOtros.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtVlrNCOtros.Properties.AutoHeight = false;
            this.txtVlrNCOtros.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrNCOtros.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtVlrNCOtros.Properties.Mask.EditMask = "c";
            this.txtVlrNCOtros.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtVlrNCOtros.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtVlrNCOtros.Size = new System.Drawing.Size(116, 20);
            this.txtVlrNCOtros.TabIndex = 102;
            // 
            // lblAjuste
            // 
            this.lblAjuste.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblAjuste.Location = new System.Drawing.Point(11, 135);
            this.lblAjuste.Name = "lblAjuste";
            this.lblAjuste.Size = new System.Drawing.Size(154, 14);
            this.lblAjuste.TabIndex = 104;
            this.lblAjuste.Text = "Ajuste por Componentes";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(114, 62);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(268, 62);
            this.txtObservacion.TabIndex = 105;
            // 
            // NotaCreditoCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1144, 724);
            this.Name = "NotaCreditoCartera";
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvComponentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCuotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCuotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaAplica.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaDoc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLibranza.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCuota.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrNC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrNCObligac.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtVlrNCOtros.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.DateEdit dtFechaAplica;
        protected DevExpress.XtraEditors.DateEdit dtFechaDoc;
        private System.Windows.Forms.Label lblFechaDoc;
        private System.Windows.Forms.Label lblFechaAplica;
        private System.Windows.Forms.Label lblLibranza;
        private ControlsUC.uc_MasterFind masterCliente;
        protected DevExpress.XtraGrid.GridControl gcCuotas;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvComponentes;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvCuotas;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtLibranza;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.LabelControl lblNCOtros;
        private DevExpress.XtraEditors.TextEdit txtVlrNCOtros;
        private DevExpress.XtraEditors.LabelControl lblNCObligacion;
        private DevExpress.XtraEditors.TextEdit txtVlrNCObligac;
        private DevExpress.XtraEditors.LabelControl lblNC;
        private DevExpress.XtraEditors.TextEdit txtVlrNC;
        private ControlsUC.uc_MasterFind masterReintegroSaldo;
        private DevExpress.XtraEditors.LabelControl lblValorCuota;
        private DevExpress.XtraEditors.TextEdit txtValorCuota;
        private DevExpress.XtraEditors.LabelControl lblAjuste;
        private System.Windows.Forms.TextBox txtObservacion;


    }
}