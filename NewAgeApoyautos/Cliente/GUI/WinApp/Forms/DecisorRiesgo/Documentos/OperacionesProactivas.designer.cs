namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class OperacionesProactivas
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
            this.anexosContainer = new System.Windows.Forms.GroupBox();
            this.gbGridDocument = new System.Windows.Forms.GroupBox();
            this.gcDocument = new DevExpress.XtraGrid.GridControl();
            this.gvDocument = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.persistentRepository = new DevExpress.XtraEditors.Repository.PersistentRepository(this.components);
            this.editChkBox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.riPopup = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.PopupContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.xtraScrollableControl1 = new DevExpress.XtraEditors.XtraScrollableControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCodeudor2 = new System.Windows.Forms.Label();
            this.masterCodeudor2 = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblCodeudor1 = new System.Windows.Forms.Label();
            this.masterCodeudor1 = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterTipoCredito = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.chkCompraCartera = new System.Windows.Forms.CheckBox();
            this.lblCompraCartera = new System.Windows.Forms.Label();
            this.masterLineaCredito = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCooperativa = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterComercio = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lkp_TipoGarantia = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtObsReversion = new System.Windows.Forms.TextBox();
            this.lblObservacionReversion = new System.Windows.Forms.Label();
            this.masterCentroPago = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.comboPlazo = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.btnCambiarLibranza = new System.Windows.Forms.Button();
            this.txtLibranza = new System.Windows.Forms.TextBox();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.lblLibranza = new System.Windows.Forms.Label();
            this.txtObservacion = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.lblValor = new System.Windows.Forms.Label();
            this.lblPlazo = new System.Windows.Forms.Label();
            this.masterPagaduria = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterCiudad = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterAsesor = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblCliente = new System.Windows.Forms.Label();
            this.masterCliente = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.txtPriNombre = new System.Windows.Forms.TextBox();
            this.txtSdoNombre = new System.Windows.Forms.TextBox();
            this.txtSdoApellido = new System.Windows.Forms.TextBox();
            this.txtPriApellido = new System.Windows.Forms.TextBox();
            this.lblSdoNombre = new System.Windows.Forms.Label();
            this.lblPriNombre = new System.Windows.Forms.Label();
            this.lblSdoApellido = new System.Windows.Forms.Label();
            this.lblPriApellido = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCodeudor3 = new System.Windows.Forms.Label();
            this.masterCodeudor3 = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.anexosContainer.SuspendLayout();
            this.gbGridDocument.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).BeginInit();
            this.PopupContainerControl.SuspendLayout();
            this.xtraScrollableControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoGarantia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // anexosContainer
            // 
            this.anexosContainer.Controls.Add(this.gbGridDocument);
            this.anexosContainer.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.anexosContainer.Location = new System.Drawing.Point(12, 375);
            this.anexosContainer.Name = "anexosContainer";
            this.anexosContainer.Size = new System.Drawing.Size(1095, 194);
            this.anexosContainer.TabIndex = 1;
            this.anexosContainer.TabStop = false;
            this.anexosContainer.Text = "32551_DocumentoAnexo";
            // 
            // gbGridDocument
            // 
            this.gbGridDocument.Controls.Add(this.gcDocument);
            this.gbGridDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGridDocument.Location = new System.Drawing.Point(3, 18);
            this.gbGridDocument.Name = "gbGridDocument";
            this.gbGridDocument.Padding = new System.Windows.Forms.Padding(7, 0, 7, 3);
            this.gbGridDocument.Size = new System.Drawing.Size(1089, 173);
            this.gbGridDocument.TabIndex = 0;
            this.gbGridDocument.TabStop = false;
            // 
            // gcDocument
            // 
            this.gcDocument.AllowDrop = true;
            this.gcDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDocument.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDocument.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcDocument.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcDocument.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDocument.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDocument.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcDocument.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcDocument.EmbeddedNavigator.TextStringFormat = "Registro {0} of {1}";
            this.gcDocument.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcDocument.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDocument.Location = new System.Drawing.Point(7, 15);
            this.gcDocument.LookAndFeel.SkinName = "Dark Side";
            this.gcDocument.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcDocument.MainView = this.gvDocument;
            this.gcDocument.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gcDocument.Name = "gcDocument";
            this.gcDocument.Size = new System.Drawing.Size(1075, 155);
            this.gcDocument.TabIndex = 0;
            this.gcDocument.UseEmbeddedNavigator = true;
            this.gcDocument.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDocument});
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
            this.gvDocument.OptionsCustomization.AllowSort = false;
            this.gvDocument.OptionsMenu.EnableColumnMenu = false;
            this.gvDocument.OptionsMenu.EnableFooterMenu = false;
            this.gvDocument.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvDocument.OptionsView.ShowGroupPanel = false;
            this.gvDocument.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveHorzScroll;
            this.gvDocument.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocument_CustomRowCellEdit);
            this.gvDocument.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDocument_CustomRowCellEditForEditing);
            this.gvDocument.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gvDocument_CustomUnboundColumnData);
            // 
            // persistentRepository
            // 
            this.persistentRepository.Items.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.editChkBox,
            this.riPopup});
            // 
            // editChkBox
            // 
            this.editChkBox.Name = "editChkBox";
            // riPopup
            // 
            this.riPopup.AutoHeight = false;
            this.riPopup.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riPopup.Name = "riPopup";
            this.riPopup.PopupControl = this.PopupContainerControl;
            this.riPopup.PopupFormMinSize = new System.Drawing.Size(500, 0);
            this.riPopup.PopupFormSize = new System.Drawing.Size(500, 300);
            this.riPopup.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
            this.riPopup.QueryResultValue += new DevExpress.XtraEditors.Controls.QueryResultValueEventHandler(this.riPopup_QueryResultValue);
            this.riPopup.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.riPopup_QueryPopUp);
            // 
            // PopupContainerControl
            // 
            this.PopupContainerControl.Controls.Add(this.richEditControl);
            this.PopupContainerControl.Location = new System.Drawing.Point(3, 48);
            this.PopupContainerControl.Name = "PopupContainerControl";
            this.PopupContainerControl.Size = new System.Drawing.Size(12, 40);
            this.PopupContainerControl.TabIndex = 5;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Size = new System.Drawing.Size(12, 40);
            this.richEditControl.TabIndex = 2;
            this.richEditControl.Text = "myRichEditControl";
            // 
            // xtraScrollableControl1
            // 
            this.xtraScrollableControl1.Controls.Add(this.groupBox1);
            this.xtraScrollableControl1.Controls.Add(this.anexosContainer);
            this.xtraScrollableControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraScrollableControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraScrollableControl1.Name = "xtraScrollableControl1";
            this.xtraScrollableControl1.Size = new System.Drawing.Size(1145, 581);
            this.xtraScrollableControl1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCodeudor3);
            this.groupBox1.Controls.Add(this.masterCodeudor3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblCodeudor2);
            this.groupBox1.Controls.Add(this.masterCodeudor2);
            this.groupBox1.Controls.Add(this.lblCodeudor1);
            this.groupBox1.Controls.Add(this.masterCodeudor1);
            this.groupBox1.Controls.Add(this.masterTipoCredito);
            this.groupBox1.Controls.Add(this.chkCompraCartera);
            this.groupBox1.Controls.Add(this.lblCompraCartera);
            this.groupBox1.Controls.Add(this.masterLineaCredito);
            this.groupBox1.Controls.Add(this.masterCooperativa);
            this.groupBox1.Controls.Add(this.masterComercio);
            this.groupBox1.Controls.Add(this.lkp_TipoGarantia);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtObsReversion);
            this.groupBox1.Controls.Add(this.lblObservacionReversion);
            this.groupBox1.Controls.Add(this.masterCentroPago);
            this.groupBox1.Controls.Add(this.comboPlazo);
            this.groupBox1.Controls.Add(this.btnCambiarLibranza);
            this.groupBox1.Controls.Add(this.txtLibranza);
            this.groupBox1.Controls.Add(this.txtValor);
            this.groupBox1.Controls.Add(this.lblLibranza);
            this.groupBox1.Controls.Add(this.txtObservacion);
            this.groupBox1.Controls.Add(this.lblObservaciones);
            this.groupBox1.Controls.Add(this.lblValor);
            this.groupBox1.Controls.Add(this.lblPlazo);
            this.groupBox1.Controls.Add(this.masterPagaduria);
            this.groupBox1.Controls.Add(this.masterCiudad);
            this.groupBox1.Controls.Add(this.masterAsesor);
            this.groupBox1.Controls.Add(this.lblCliente);
            this.groupBox1.Controls.Add(this.masterCliente);
            this.groupBox1.Controls.Add(this.txtPriNombre);
            this.groupBox1.Controls.Add(this.txtSdoNombre);
            this.groupBox1.Controls.Add(this.txtSdoApellido);
            this.groupBox1.Controls.Add(this.txtPriApellido);
            this.groupBox1.Controls.Add(this.lblSdoNombre);
            this.groupBox1.Controls.Add(this.lblPriNombre);
            this.groupBox1.Controls.Add(this.lblSdoApellido);
            this.groupBox1.Controls.Add(this.lblPriApellido);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1095, 357);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "32551_DatosGenerales";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(356, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 14);
            this.label3.TabIndex = 20;
            this.label3.Text = "32555_Agencia";
            // 
            // lblCodeudor2
            // 
            this.lblCodeudor2.AutoSize = true;
            this.lblCodeudor2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodeudor2.Location = new System.Drawing.Point(356, 222);
            this.lblCodeudor2.Name = "lblCodeudor2";
            this.lblCodeudor2.Size = new System.Drawing.Size(109, 14);
            this.lblCodeudor2.TabIndex = 19;
            this.lblCodeudor2.Text = "32555_Codeudor2";
            // 
            // masterCodeudor2
            // 
            this.masterCodeudor2.BackColor = System.Drawing.Color.Transparent;
            this.masterCodeudor2.Filtros = null;
            this.masterCodeudor2.Location = new System.Drawing.Point(359, 216);
            this.masterCodeudor2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.masterCodeudor2.Name = "masterCodeudor2";
            this.masterCodeudor2.Size = new System.Drawing.Size(349, 29);
            this.masterCodeudor2.TabIndex = 19;
            this.masterCodeudor2.Value = "";
            // 
            // lblCodeudor1
            // 
            this.lblCodeudor1.AutoSize = true;
            this.lblCodeudor1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodeudor1.Location = new System.Drawing.Point(7, 222);
            this.lblCodeudor1.Name = "lblCodeudor1";
            this.lblCodeudor1.Size = new System.Drawing.Size(109, 14);
            this.lblCodeudor1.TabIndex = 34;
            this.lblCodeudor1.Text = "32555_Codeudor1";
            // 
            // masterCodeudor1
            // 
            this.masterCodeudor1.BackColor = System.Drawing.Color.Transparent;
            this.masterCodeudor1.Filtros = null;
            this.masterCodeudor1.Location = new System.Drawing.Point(9, 216);
            this.masterCodeudor1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.masterCodeudor1.Name = "masterCodeudor1";
            this.masterCodeudor1.Size = new System.Drawing.Size(348, 27);
            this.masterCodeudor1.TabIndex = 18;
            this.masterCodeudor1.Value = "";
            // 
            // masterTipoCredito
            // 
            this.masterTipoCredito.BackColor = System.Drawing.Color.Transparent;
            this.masterTipoCredito.Filtros = null;
            this.masterTipoCredito.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterTipoCredito.Location = new System.Drawing.Point(734, 49);
            this.masterTipoCredito.Margin = new System.Windows.Forms.Padding(7);
            this.masterTipoCredito.Name = "masterTipoCredito";
            this.masterTipoCredito.Size = new System.Drawing.Size(350, 27);
            this.masterTipoCredito.TabIndex = 4;
            this.masterTipoCredito.Value = "";
            this.masterTipoCredito.Leave += new System.EventHandler(this.masterTipoCredito_Leave);
            // 
            // chkCompraCartera
            // 
            this.chkCompraCartera.AutoSize = true;
            this.chkCompraCartera.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCompraCartera.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCompraCartera.Location = new System.Drawing.Point(388, 87);
            this.chkCompraCartera.Name = "chkCompraCartera";
            this.chkCompraCartera.Size = new System.Drawing.Size(15, 14);
            this.chkCompraCartera.TabIndex = 6;
            this.chkCompraCartera.UseVisualStyleBackColor = true;
            // 
            // lblCompraCartera
            // 
            this.lblCompraCartera.AutoSize = true;
            this.lblCompraCartera.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompraCartera.Location = new System.Drawing.Point(418, 87);
            this.lblCompraCartera.Name = "lblCompraCartera";
            this.lblCompraCartera.Size = new System.Drawing.Size(129, 14);
            this.lblCompraCartera.TabIndex = 7;
            this.lblCompraCartera.Text = "32555_CompraCartera";
            // 
            // masterLineaCredito
            // 
            this.masterLineaCredito.BackColor = System.Drawing.Color.Transparent;
            this.masterLineaCredito.Filtros = null;
            this.masterLineaCredito.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterLineaCredito.Location = new System.Drawing.Point(383, 49);
            this.masterLineaCredito.Margin = new System.Windows.Forms.Padding(7);
            this.masterLineaCredito.Name = "masterLineaCredito";
            this.masterLineaCredito.Size = new System.Drawing.Size(350, 27);
            this.masterLineaCredito.TabIndex = 3;
            this.masterLineaCredito.Value = "";
            // 
            // masterCooperativa
            // 
            this.masterCooperativa.BackColor = System.Drawing.Color.Transparent;
            this.masterCooperativa.Filtros = null;
            this.masterCooperativa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCooperativa.Location = new System.Drawing.Point(707, 145);
            this.masterCooperativa.Margin = new System.Windows.Forms.Padding(7);
            this.masterCooperativa.Name = "masterCooperativa";
            this.masterCooperativa.Size = new System.Drawing.Size(343, 27);
            this.masterCooperativa.TabIndex = 14;
            this.masterCooperativa.Value = "";
            // 
            // masterComercio
            // 
            this.masterComercio.BackColor = System.Drawing.Color.Transparent;
            this.masterComercio.Filtros = null;
            this.masterComercio.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterComercio.Location = new System.Drawing.Point(359, 145);
            this.masterComercio.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.masterComercio.Name = "masterComercio";
            this.masterComercio.Size = new System.Drawing.Size(348, 27);
            this.masterComercio.TabIndex = 13;
            this.masterComercio.Value = "";
            // 
            // lkp_TipoGarantia
            // 
            this.lkp_TipoGarantia.Location = new System.Drawing.Point(126, 23);
            this.lkp_TipoGarantia.Name = "lkp_TipoGarantia";
            this.lkp_TipoGarantia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkp_TipoGarantia.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Key", "ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", 40, "Descriptivo")});
            this.lkp_TipoGarantia.Properties.DisplayMember = "Value";
            this.lkp_TipoGarantia.Properties.NullText = " ";
            this.lkp_TipoGarantia.Properties.ValueMember = "Key";
            this.lkp_TipoGarantia.Size = new System.Drawing.Size(117, 20);
            this.lkp_TipoGarantia.TabIndex = 0;
            this.lkp_TipoGarantia.EditValueChanged += new System.EventHandler(this.lkp_TipoGarantia_EditValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 14);
            this.label2.TabIndex = 27;
            this.label2.Text = "32551_TipoGarantia";
            // 
            // txtObsReversion
            // 
            this.txtObsReversion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObsReversion.Location = new System.Drawing.Point(635, 289);
            this.txtObsReversion.Multiline = true;
            this.txtObsReversion.Name = "txtObsReversion";
            this.txtObsReversion.Size = new System.Drawing.Size(303, 50);
            this.txtObsReversion.TabIndex = 23;
            // 
            // lblObservacionReversion
            // 
            this.lblObservacionReversion.AutoSize = true;
            this.lblObservacionReversion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservacionReversion.Location = new System.Drawing.Point(434, 302);
            this.lblObservacionReversion.Name = "lblObservacionReversion";
            this.lblObservacionReversion.Size = new System.Drawing.Size(167, 14);
            this.lblObservacionReversion.TabIndex = 25;
            this.lblObservacionReversion.Text = "32551_ObservacionReversion";
            // 
            // masterCentroPago
            // 
            this.masterCentroPago.BackColor = System.Drawing.Color.Transparent;
            this.masterCentroPago.Filtros = null;
            this.masterCentroPago.Location = new System.Drawing.Point(359, 179);
            this.masterCentroPago.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.masterCentroPago.Name = "masterCentroPago";
            this.masterCentroPago.Size = new System.Drawing.Size(349, 25);
            this.masterCentroPago.TabIndex = 16;
            this.masterCentroPago.Value = "";
            this.masterCentroPago.Leave += new System.EventHandler(this.masterCentroPago_Leave);
            // 
            // comboPlazo
            // 
            this.comboPlazo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPlazo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboPlazo.FormattingEnabled = true;
            this.comboPlazo.Items.AddRange(new object[] {
            "12",
            "18",
            "24",
            "36",
            "42",
            "48",
            "60",
            "72",
            "84"});
            this.comboPlazo.Location = new System.Drawing.Point(359, 251);
            this.comboPlazo.Name = "comboPlazo";
            this.comboPlazo.Size = new System.Drawing.Size(47, 22);
            this.comboPlazo.TabIndex = 21;
            // 
            // btnCambiarLibranza
            // 
            this.btnCambiarLibranza.Location = new System.Drawing.Point(256, 51);
            this.btnCambiarLibranza.Name = "btnCambiarLibranza";
            this.btnCambiarLibranza.Size = new System.Drawing.Size(118, 23);
            this.btnCambiarLibranza.TabIndex = 2;
            this.btnCambiarLibranza.Text = "32551_CambiarLibranza";
            this.btnCambiarLibranza.UseVisualStyleBackColor = true;
            this.btnCambiarLibranza.Visible = false;
            this.btnCambiarLibranza.Click += new System.EventHandler(this.btnCambiarLibranza_Click);
            // 
            // txtLibranza
            // 
            this.txtLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLibranza.Location = new System.Drawing.Point(126, 51);
            this.txtLibranza.Name = "txtLibranza";
            this.txtLibranza.Size = new System.Drawing.Size(121, 22);
            this.txtLibranza.TabIndex = 1;
            this.txtLibranza.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLibranza_KeyPress);
            this.txtLibranza.Leave += new System.EventHandler(this.txtLibranza_Leave);
            // 
            // txtValor
            // 
            this.txtValor.EditValue = "$ 0";
            this.txtValor.Location = new System.Drawing.Point(126, 251);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Properties.Appearance.Options.UseFont = true;
            this.txtValor.Properties.Appearance.Options.UseTextOptions = true;
            this.txtValor.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValor.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtValor.Properties.Mask.EditMask = "c0";
            this.txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtValor.Size = new System.Drawing.Size(121, 20);
            this.txtValor.TabIndex = 20;
            this.txtValor.Spin += new DevExpress.XtraEditors.Controls.SpinEventHandler(this.txtValor_Spin);
            // 
            // lblLibranza
            // 
            this.lblLibranza.AutoSize = true;
            this.lblLibranza.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLibranza.Location = new System.Drawing.Point(7, 54);
            this.lblLibranza.Name = "lblLibranza";
            this.lblLibranza.Size = new System.Drawing.Size(92, 14);
            this.lblLibranza.TabIndex = 0;
            this.lblLibranza.Text = "32551_Libranza";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacion.Location = new System.Drawing.Point(126, 289);
            this.txtObservacion.Multiline = true;
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Size = new System.Drawing.Size(303, 50);
            this.txtObservacion.TabIndex = 22;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObservaciones.Location = new System.Drawing.Point(7, 302);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(127, 14);
            this.lblObservaciones.TabIndex = 21;
            this.lblObservaciones.Text = "32551_Observaciones";
            // 
            // lblValor
            // 
            this.lblValor.AutoSize = true;
            this.lblValor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValor.Location = new System.Drawing.Point(7, 254);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(76, 14);
            this.lblValor.TabIndex = 16;
            this.lblValor.Text = "32551_Valor";
            // 
            // lblPlazo
            // 
            this.lblPlazo.AutoSize = true;
            this.lblPlazo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlazo.Location = new System.Drawing.Point(276, 254);
            this.lblPlazo.Name = "lblPlazo";
            this.lblPlazo.Size = new System.Drawing.Size(76, 14);
            this.lblPlazo.TabIndex = 18;
            this.lblPlazo.Text = "32551_Plazo";
            // 
            // masterPagaduria
            // 
            this.masterPagaduria.BackColor = System.Drawing.Color.Transparent;
            this.masterPagaduria.Enabled = false;
            this.masterPagaduria.Filtros = null;
            this.masterPagaduria.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterPagaduria.Location = new System.Drawing.Point(707, 179);
            this.masterPagaduria.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.masterPagaduria.Name = "masterPagaduria";
            this.masterPagaduria.Size = new System.Drawing.Size(348, 27);
            this.masterPagaduria.TabIndex = 17;
            this.masterPagaduria.Value = "";
            this.masterPagaduria.Leave += new System.EventHandler(this.masterPagaduria_Leave);
            // 
            // masterCiudad
            // 
            this.masterCiudad.BackColor = System.Drawing.Color.Transparent;
            this.masterCiudad.Filtros = null;
            this.masterCiudad.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCiudad.Location = new System.Drawing.Point(9, 179);
            this.masterCiudad.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.masterCiudad.Name = "masterCiudad";
            this.masterCiudad.Size = new System.Drawing.Size(348, 27);
            this.masterCiudad.TabIndex = 15;
            this.masterCiudad.Value = "";
            // 
            // masterAsesor
            // 
            this.masterAsesor.BackColor = System.Drawing.Color.Transparent;
            this.masterAsesor.Filtros = null;
            this.masterAsesor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterAsesor.Location = new System.Drawing.Point(10, 144);
            this.masterAsesor.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.masterAsesor.Name = "masterAsesor";
            this.masterAsesor.Size = new System.Drawing.Size(357, 27);
            this.masterAsesor.TabIndex = 12;
            this.masterAsesor.Value = "";
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCliente.Location = new System.Drawing.Point(7, 87);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(122, 14);
            this.lblCliente.TabIndex = 2;
            this.lblCliente.Text = "32551_CedulaCliente";
            // 
            // masterCliente
            // 
            this.masterCliente.BackColor = System.Drawing.Color.Transparent;
            this.masterCliente.Filtros = null;
            this.masterCliente.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.masterCliente.Location = new System.Drawing.Point(10, 80);
            this.masterCliente.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.masterCliente.Name = "masterCliente";
            this.masterCliente.Size = new System.Drawing.Size(357, 27);
            this.masterCliente.TabIndex = 5;
            this.masterCliente.Value = "";
            this.masterCliente.Leave += new System.EventHandler(this.masterCliente_Leave);
            // 
            // txtPriNombre
            // 
            this.txtPriNombre.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPriNombre.Location = new System.Drawing.Point(635, 112);
            this.txtPriNombre.Name = "txtPriNombre";
            this.txtPriNombre.Size = new System.Drawing.Size(117, 22);
            this.txtPriNombre.TabIndex = 10;
            // 
            // txtSdoNombre
            // 
            this.txtSdoNombre.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSdoNombre.Location = new System.Drawing.Point(888, 113);
            this.txtSdoNombre.Name = "txtSdoNombre";
            this.txtSdoNombre.Size = new System.Drawing.Size(117, 22);
            this.txtSdoNombre.TabIndex = 11;
            // 
            // txtSdoApellido
            // 
            this.txtSdoApellido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSdoApellido.Location = new System.Drawing.Point(388, 112);
            this.txtSdoApellido.Name = "txtSdoApellido";
            this.txtSdoApellido.Size = new System.Drawing.Size(117, 22);
            this.txtSdoApellido.TabIndex = 9;
            // 
            // txtPriApellido
            // 
            this.txtPriApellido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPriApellido.Location = new System.Drawing.Point(126, 113);
            this.txtPriApellido.Name = "txtPriApellido";
            this.txtPriApellido.Size = new System.Drawing.Size(121, 22);
            this.txtPriApellido.TabIndex = 8;
            // 
            // lblSdoNombre
            // 
            this.lblSdoNombre.AutoSize = true;
            this.lblSdoNombre.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSdoNombre.Location = new System.Drawing.Point(776, 116);
            this.lblSdoNombre.Name = "lblSdoNombre";
            this.lblSdoNombre.Size = new System.Drawing.Size(141, 14);
            this.lblSdoNombre.TabIndex = 10;
            this.lblSdoNombre.Text = "32551_SegundoNombre";
            // 
            // lblPriNombre
            // 
            this.lblPriNombre.AutoSize = true;
            this.lblPriNombre.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriNombre.Location = new System.Drawing.Point(539, 116);
            this.lblPriNombre.Name = "lblPriNombre";
            this.lblPriNombre.Size = new System.Drawing.Size(126, 14);
            this.lblPriNombre.TabIndex = 8;
            this.lblPriNombre.Text = "32551_PrimerNombre";
            // 
            // lblSdoApellido
            // 
            this.lblSdoApellido.AutoSize = true;
            this.lblSdoApellido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSdoApellido.Location = new System.Drawing.Point(276, 116);
            this.lblSdoApellido.Name = "lblSdoApellido";
            this.lblSdoApellido.Size = new System.Drawing.Size(140, 14);
            this.lblSdoApellido.TabIndex = 6;
            this.lblSdoApellido.Text = "32551_SegundoApellido";
            // 
            // lblPriApellido
            // 
            this.lblPriApellido.AutoSize = true;
            this.lblPriApellido.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriApellido.Location = new System.Drawing.Point(7, 117);
            this.lblPriApellido.Name = "lblPriApellido";
            this.lblPriApellido.Size = new System.Drawing.Size(125, 14);
            this.lblPriApellido.TabIndex = 4;
            this.lblPriApellido.Text = "32551_PrimerApellido";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(-213, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Observaciones";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(-220, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Cedula Cliente";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-210, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Primer Apellido";
            // 
            // lblCodeudor3
            // 
            this.lblCodeudor3.AutoSize = true;
            this.lblCodeudor3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodeudor3.Location = new System.Drawing.Point(706, 222);
            this.lblCodeudor3.Name = "lblCodeudor3";
            this.lblCodeudor3.Size = new System.Drawing.Size(109, 14);
            this.lblCodeudor3.TabIndex = 35;
            this.lblCodeudor3.Text = "32555_Codeudor3";
            // 
            // masterCodeudor3
            // 
            this.masterCodeudor3.BackColor = System.Drawing.Color.Transparent;
            this.masterCodeudor3.Filtros = null;
            this.masterCodeudor3.Location = new System.Drawing.Point(705, 215);
            this.masterCodeudor3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.masterCodeudor3.Name = "masterCodeudor3";
            this.masterCodeudor3.Size = new System.Drawing.Size(407, 31);
            this.masterCodeudor3.TabIndex = 36;
            this.masterCodeudor3.Value = "";
            // 
            // OperacionesProactivas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 581);
            this.Controls.Add(this.xtraScrollableControl1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OperacionesProactivas";
            this.Text = "Solicitud Libranza";
            this.anexosContainer.ResumeLayout(false);
            this.gbGridDocument.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPopup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PopupContainerControl)).EndInit();
            this.PopupContainerControl.ResumeLayout(false);
            this.xtraScrollableControl1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkp_TipoGarantia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox anexosContainer;
        protected System.Windows.Forms.GroupBox gbGridDocument;
        protected DevExpress.XtraGrid.GridControl gcDocument;
        protected DevExpress.XtraGrid.Views.Grid.GridView gvDocument;
        private DevExpress.XtraEditors.Repository.PersistentRepository persistentRepository;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit editChkBox;
        private DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblLibranza;
        private System.Windows.Forms.TextBox txtObservacion;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.Label lblPlazo;
        private ControlsUC.uc_MasterFind masterPagaduria;
        private ControlsUC.uc_MasterFind masterAsesor;
        private System.Windows.Forms.Label lblCliente;
        private ControlsUC.uc_MasterFind masterCliente;
        private System.Windows.Forms.TextBox txtPriNombre;
        private System.Windows.Forms.TextBox txtSdoNombre;
        private System.Windows.Forms.TextBox txtSdoApellido;
        private System.Windows.Forms.TextBox txtPriApellido;
        private System.Windows.Forms.Label lblSdoNombre;
        private System.Windows.Forms.Label lblPriNombre;
        private System.Windows.Forms.Label lblSdoApellido;
        private System.Windows.Forms.Label lblPriApellido;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private System.Windows.Forms.TextBox txtLibranza;
        protected DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit riPopup;
        protected DevExpress.XtraEditors.PopupContainerControl PopupContainerControl;
        protected DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private System.Windows.Forms.Button btnCambiarLibranza;
        private Clases.ComboBoxEx comboPlazo;
        private ControlsUC.uc_MasterFind masterCentroPago;
        private System.Windows.Forms.TextBox txtObsReversion;
        private System.Windows.Forms.Label lblObservacionReversion;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.LookUpEdit lkp_TipoGarantia;
        private ControlsUC.uc_MasterFind masterComercio;
        private ControlsUC.uc_MasterFind masterCiudad;
        private ControlsUC.uc_MasterFind masterCooperativa;
        private ControlsUC.uc_MasterFind masterLineaCredito;
        private System.Windows.Forms.CheckBox chkCompraCartera;
        private System.Windows.Forms.Label lblCompraCartera;
        private ControlsUC.uc_MasterFind masterCodeudor1;
        private ControlsUC.uc_MasterFind masterTipoCredito;
        private System.Windows.Forms.Label lblCodeudor1;
        private System.Windows.Forms.Label lblCodeudor2;
        private ControlsUC.uc_MasterFind masterCodeudor2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCodeudor3;
        private ControlsUC.uc_MasterFind masterCodeudor3;

    }
}