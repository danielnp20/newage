namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class DistribucionCostos
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.gcDetail = new DevExpress.XtraGrid.GridControl();
            this.gvDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pRecibido = new DevExpress.XtraEditors.PanelControl();
            this.txtNroDocInv = new System.Windows.Forms.TextBox();
            this.lblNroDocInv = new System.Windows.Forms.Label();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtValorMvtoML = new DevExpress.XtraEditors.TextEdit();
            this.lblValorMvtoML = new DevExpress.XtraEditors.LabelControl();
            this.txtValorDistribucionML = new DevExpress.XtraEditors.TextEdit();
            this.lblValorDistribucionML = new DevExpress.XtraEditors.LabelControl();
            this.txtValorDistribucionME = new DevExpress.XtraEditors.TextEdit();
            this.lblValorDistribucionME = new DevExpress.XtraEditors.LabelControl();
            this.txtValorMvtoME = new DevExpress.XtraEditors.TextEdit();
            this.lblValorMvtoME = new DevExpress.XtraEditors.LabelControl();
            this.lblResumenMvto = new System.Windows.Forms.Label();
            this.lblFacturas = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblObservacion = new System.Windows.Forms.Label();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.grpboxDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.gbGridDocument.SuspendLayout();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pRecibido)).BeginInit();
            this.pRecibido.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorMvtoML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDistribucionML.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDistribucionME.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorMvtoME.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // grpboxDetail
            // 
            this.grpboxDetail.Controls.Add(this.lblFacturas);
            this.grpboxDetail.Controls.Add(this.gcDetail);
            this.grpboxDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxDetail.Location = new System.Drawing.Point(0, 0);
            this.grpboxDetail.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxDetail.Size = new System.Drawing.Size(1016, 167);
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
            this.editValue,
            this.editValue4,
            this.editSpinPorcen});
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
            this.btnMark.Margin = new System.Windows.Forms.Padding(6);
            this.btnMark.Size = new System.Drawing.Size(49, 20);
            // 
            // txtDocumentoID
            // 
            this.txtDocumentoID.Margin = new System.Windows.Forms.Padding(6);
            // 
            // txtNumeroDoc
            // 
            this.txtNumeroDoc.Margin = new System.Windows.Forms.Padding(6);
            this.txtNumeroDoc.Size = new System.Drawing.Size(55, 20);
            // 
            // dtFecha
            // 
            this.dtFecha.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFecha.Margin = new System.Windows.Forms.Padding(6);
            this.dtFecha.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFecha.Properties.Appearance.Options.UseBackColor = true;
            this.dtFecha.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFecha.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFecha.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFecha.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFecha.Size = new System.Drawing.Size(100, 20);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(6);
            this.txtPrefix.Size = new System.Drawing.Size(50, 20);
            // 
            // editText
            // 
            this.editText.Mask.EditMask = "c";
            this.editText.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.editText.Mask.UseMaskAsDisplayFormat = true;
            // 
            // txtAF
            // 
            this.txtAF.Margin = new System.Windows.Forms.Padding(6);
            this.txtAF.Size = new System.Drawing.Size(91, 20);
            // 
            // lblAF
            // 
            this.lblAF.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.lblResumenMvto);
            this.gbGridDocument.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(12, 0, 12, 6);
            this.gbGridDocument.Size = new System.Drawing.Size(720, 150);
            this.gbGridDocument.TabIndex = 3;
            this.gbGridDocument.Controls.SetChildIndex(this.lblResumenMvto, 0);
            // 
            // gbGridProvider
            // 
            this.gbGridProvider.Location = new System.Drawing.Point(720, 0);
            this.gbGridProvider.Margin = new System.Windows.Forms.Padding(6);
            this.gbGridProvider.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.gbGridProvider.Size = new System.Drawing.Size(296, 150);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.grpboxHeader.Controls.Add(this.lblObservacion);
            this.grpboxHeader.Controls.Add(this.txtObservacion);
            this.grpboxHeader.Controls.Add(this.txtValorDistribucionME);
            this.grpboxHeader.Controls.Add(this.lblValorDistribucionME);
            this.grpboxHeader.Controls.Add(this.txtValorMvtoME);
            this.grpboxHeader.Controls.Add(this.lblValorMvtoME);
            this.grpboxHeader.Controls.Add(this.txtValorDistribucionML);
            this.grpboxHeader.Controls.Add(this.lblValorDistribucionML);
            this.grpboxHeader.Controls.Add(this.txtValorMvtoML);
            this.grpboxHeader.Controls.Add(this.lblValorMvtoML);
            this.grpboxHeader.Controls.Add(this.pRecibido);
            this.grpboxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpboxHeader.Location = new System.Drawing.Point(2, 22);
            this.grpboxHeader.Margin = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Padding = new System.Windows.Forms.Padding(6);
            this.grpboxHeader.Size = new System.Drawing.Size(1012, 67);
            this.grpboxHeader.TabIndex = 0;
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // gcDetail
            // 
            this.gcDetail.AllowDrop = true;
            this.gcDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetail.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDetail.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDetail.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcDetail.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDetail.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDetail.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDetail.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gcDetail.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDetail.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDetail.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDetail.Location = new System.Drawing.Point(6, 19);
            this.gcDetail.LookAndFeel.SkinName = "Dark Side";
            this.gcDetail.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDetail.MainView = this.gvDetail;
            this.gcDetail.Margin = new System.Windows.Forms.Padding(2);
            this.gcDetail.Name = "gcDetail";
            this.gcDetail.Size = new System.Drawing.Size(1004, 142);
            this.gcDetail.TabIndex = 4;
            this.gcDetail.UseEmbeddedNavigator = true;
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
            this.gvDetail.HorzScrollStep = 50;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetail.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDetail.OptionsBehavior.Editable = false;
            this.gvDetail.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetail.OptionsCustomization.AllowFilter = false;
            this.gvDetail.OptionsCustomization.AllowSort = false;
            this.gvDetail.OptionsMenu.EnableColumnMenu = false;
            this.gvDetail.OptionsMenu.EnableFooterMenu = false;
            this.gvDetail.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDetail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDetail.OptionsView.ShowGroupPanel = false;
            this.gvDetail.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDetail.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDetail_CustomUnboundColumnData);
            // 
            // pRecibido
            // 
            this.pRecibido.Controls.Add(this.btnQueryDoc);
            this.pRecibido.Controls.Add(this.txtNroDocInv);
            this.pRecibido.Controls.Add(this.lblNroDocInv);
            this.pRecibido.Controls.Add(this.masterPrefijo);
            this.pRecibido.Location = new System.Drawing.Point(12, 13);
            this.pRecibido.Name = "pRecibido";
            this.pRecibido.Size = new System.Drawing.Size(485, 32);
            this.pRecibido.TabIndex = 8;
            // 
            // txtNroDocInv
            // 
            this.txtNroDocInv.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNroDocInv.Location = new System.Drawing.Point(369, 6);
            this.txtNroDocInv.Name = "txtNroDocInv";
            this.txtNroDocInv.Size = new System.Drawing.Size(69, 21);
            this.txtNroDocInv.TabIndex = 2;
            this.txtNroDocInv.Enter += new System.EventHandler(this.txtNroDocInv_Enter);
            this.txtNroDocInv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNroDocInv_KeyPress);
            this.txtNroDocInv.Leave += new System.EventHandler(this.txtNroDocInv_Leave);
            // 
            // lblNroDocInv
            // 
            this.lblNroDocInv.AutoSize = true;
            this.lblNroDocInv.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroDocInv.Location = new System.Drawing.Point(310, 9);
            this.lblNroDocInv.Name = "lblNroDocInv";
            this.lblNroDocInv.Size = new System.Drawing.Size(58, 14);
            this.lblNroDocInv.TabIndex = 3;
            this.lblNroDocInv.Text = "54_lblNro";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(7, 4);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(297, 25);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            // 
            // txtValorMvtoML
            // 
            this.txtValorMvtoML.EditValue = 0;
            this.txtValorMvtoML.Enabled = false;
            this.txtValorMvtoML.Location = new System.Drawing.Point(718, 18);
            this.txtValorMvtoML.Name = "txtValorMvtoML";
            this.txtValorMvtoML.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtValorMvtoML.Properties.Appearance.Options.UseFont = true;
            this.txtValorMvtoML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorMvtoML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorMvtoML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorMvtoML.Properties.Mask.EditMask = "c";
            this.txtValorMvtoML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorMvtoML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorMvtoML.Size = new System.Drawing.Size(127, 20);
            this.txtValorMvtoML.TabIndex = 6;
            // 
            // lblValorMvtoML
            // 
            this.lblValorMvtoML.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorMvtoML.Location = new System.Drawing.Point(589, 21);
            this.lblValorMvtoML.Name = "lblValorMvtoML";
            this.lblValorMvtoML.Size = new System.Drawing.Size(119, 14);
            this.lblValorMvtoML.TabIndex = 7;
            this.lblValorMvtoML.Text = "54_lblValorMvtoML";
            // 
            // txtValorDistribucionML
            // 
            this.txtValorDistribucionML.EditValue = 0;
            this.txtValorDistribucionML.Enabled = false;
            this.txtValorDistribucionML.Location = new System.Drawing.Point(718, 39);
            this.txtValorDistribucionML.Name = "txtValorDistribucionML";
            this.txtValorDistribucionML.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtValorDistribucionML.Properties.Appearance.Options.UseFont = true;
            this.txtValorDistribucionML.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorDistribucionML.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorDistribucionML.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorDistribucionML.Properties.Mask.EditMask = "c";
            this.txtValorDistribucionML.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorDistribucionML.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorDistribucionML.Size = new System.Drawing.Size(127, 20);
            this.txtValorDistribucionML.TabIndex = 4;
            // 
            // lblValorDistribucionML
            // 
            this.lblValorDistribucionML.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorDistribucionML.Location = new System.Drawing.Point(589, 42);
            this.lblValorDistribucionML.Name = "lblValorDistribucionML";
            this.lblValorDistribucionML.Size = new System.Drawing.Size(160, 14);
            this.lblValorDistribucionML.TabIndex = 5;
            this.lblValorDistribucionML.Text = "54_lblValorDistribucionML";
            // 
            // txtValorDistribucionME
            // 
            this.txtValorDistribucionME.EditValue = 0;
            this.txtValorDistribucionME.Enabled = false;
            this.txtValorDistribucionME.Location = new System.Drawing.Point(981, 39);
            this.txtValorDistribucionME.Name = "txtValorDistribucionME";
            this.txtValorDistribucionME.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtValorDistribucionME.Properties.Appearance.Options.UseFont = true;
            this.txtValorDistribucionME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorDistribucionME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorDistribucionME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorDistribucionME.Properties.Mask.EditMask = "c";
            this.txtValorDistribucionME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorDistribucionME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorDistribucionME.Size = new System.Drawing.Size(127, 20);
            this.txtValorDistribucionME.TabIndex = 0;
            this.txtValorDistribucionME.Visible = false;
            // 
            // lblValorDistribucionME
            // 
            this.lblValorDistribucionME.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorDistribucionME.Location = new System.Drawing.Point(851, 42);
            this.lblValorDistribucionME.Name = "lblValorDistribucionME";
            this.lblValorDistribucionME.Size = new System.Drawing.Size(160, 14);
            this.lblValorDistribucionME.TabIndex = 1;
            this.lblValorDistribucionME.Text = "54_lblValorDistribucionME";
            this.lblValorDistribucionME.Visible = false;
            // 
            // txtValorMvtoME
            // 
            this.txtValorMvtoME.EditValue = 0;
            this.txtValorMvtoME.Enabled = false;
            this.txtValorMvtoME.Location = new System.Drawing.Point(981, 18);
            this.txtValorMvtoME.Name = "txtValorMvtoME";
            this.txtValorMvtoME.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtValorMvtoME.Properties.Appearance.Options.UseFont = true;
            this.txtValorMvtoME.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValorMvtoME.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValorMvtoME.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValorMvtoME.Properties.Mask.EditMask = "c";
            this.txtValorMvtoME.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValorMvtoME.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValorMvtoME.Size = new System.Drawing.Size(127, 20);
            this.txtValorMvtoME.TabIndex = 2;
            this.txtValorMvtoME.Visible = false;
            // 
            // lblValorMvtoME
            // 
            this.lblValorMvtoME.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorMvtoME.Location = new System.Drawing.Point(851, 21);
            this.lblValorMvtoME.Name = "lblValorMvtoME";
            this.lblValorMvtoME.Size = new System.Drawing.Size(119, 14);
            this.lblValorMvtoME.TabIndex = 3;
            this.lblValorMvtoME.Text = "54_lblValorMvtoME";
            this.lblValorMvtoME.Visible = false;
            // 
            // lblResumenMvto
            // 
            this.lblResumenMvto.AutoSize = true;
            this.lblResumenMvto.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResumenMvto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumenMvto.Location = new System.Drawing.Point(14, -3);
            this.lblResumenMvto.Name = "lblResumenMvto";
            this.lblResumenMvto.Size = new System.Drawing.Size(173, 14);
            this.lblResumenMvto.TabIndex = 51;
            this.lblResumenMvto.Text = "54_lblFacturas";
            // 
            // lblFacturas
            // 
            this.lblFacturas.AutoSize = true;
            this.lblFacturas.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblFacturas.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFacturas.Location = new System.Drawing.Point(8, 1);
            this.lblFacturas.Name = "lblFacturas";
            this.lblFacturas.Size = new System.Drawing.Size(96, 14);
            this.lblFacturas.TabIndex = 0;
            this.lblFacturas.Text = "54_lblResumenMovimiento";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacion.Location = new System.Drawing.Point(10, 63);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(487, 35);
            this.txtObservacion.TabIndex = 4;
            // 
            // lblObservacion
            // 
            this.lblObservacion.AutoSize = true;
            this.lblObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacion.Location = new System.Drawing.Point(13, 49);
            this.lblObservacion.Name = "lblObservacion";
            this.lblObservacion.Size = new System.Drawing.Size(100, 14);
            this.lblObservacion.TabIndex = 4;
            this.lblObservacion.Text = "54_lblDescripcion";
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(440, 6);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(32, 21);
            this.btnQueryDoc.TabIndex = 25;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // DistribucionCostos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(1044, 430);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "DistribucionCostos";
            this.grpboxDetail.ResumeLayout(false);
            this.grpboxDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.gbGridDocument.ResumeLayout(false);
            this.gbGridDocument.PerformLayout();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pRecibido)).EndInit();
            this.pRecibido.ResumeLayout(false);
            this.pRecibido.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorMvtoML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDistribucionML.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorDistribucionME.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorMvtoME.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        protected DevExpress.XtraGrid.GridControl gcDetail;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDetail;
        private DevExpress.XtraEditors.PanelControl pRecibido;
        private System.Windows.Forms.TextBox txtNroDocInv;
        private System.Windows.Forms.Label lblNroDocInv;
        private ControlsUC.uc_MasterFind masterPrefijo;
        private DevExpress.XtraEditors.TextEdit txtValorDistribucionML;
        private DevExpress.XtraEditors.LabelControl lblValorDistribucionML;
        private DevExpress.XtraEditors.TextEdit txtValorMvtoML;
        private DevExpress.XtraEditors.LabelControl lblValorMvtoML;
        private DevExpress.XtraEditors.TextEdit txtValorDistribucionME;
        private DevExpress.XtraEditors.LabelControl lblValorDistribucionME;
        private DevExpress.XtraEditors.TextEdit txtValorMvtoME;
        private DevExpress.XtraEditors.LabelControl lblValorMvtoME;
        private System.Windows.Forms.Label lblFacturas;
        private System.Windows.Forms.Label lblResumenMvto;
        private System.Windows.Forms.Label lblObservacion;
        private System.Windows.Forms.TextBox txtObservacion;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;  
    }
}