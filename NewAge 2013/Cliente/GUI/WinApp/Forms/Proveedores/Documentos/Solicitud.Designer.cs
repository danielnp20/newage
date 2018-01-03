namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class Solicitud
    {
    
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            base.InitializeComponent();
            this.editBtnGridCargos = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.editSpinPorc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.btnQueryDoc = new DevExpress.XtraEditors.SimpleButton();
            this.masterLugarEntr = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.masterAreaAprob = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaEntr = new DevExpress.XtraEditors.DateEdit();
            this.lblFechaEntr = new DevExpress.XtraEditors.LabelControl();
            this.lblFechaSol = new DevExpress.XtraEditors.LabelControl();
            this.txtSolicitudNro = new System.Windows.Forms.TextBox();
            this.lblSolicitudNro = new System.Windows.Forms.Label();
            this.masterPrefijo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.dtFechaSol = new DevExpress.XtraEditors.DateEdit();
            this.lblDescDoc = new System.Windows.Forms.Label();
            this.lblPrioridad = new System.Windows.Forms.Label();
            this.lblLugarEntrega = new System.Windows.Forms.Label();
            this.txtDescDoc = new System.Windows.Forms.TextBox();
            this.cmbPrioridad = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.masterUsuario = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.cmbDestino = new NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx();
            this.lblDestino = new System.Windows.Forms.Label();
            this.btnProyectoNro = new DevExpress.XtraEditors.SimpleButton();
            this.txtOrdenTrabajoNro = new System.Windows.Forms.TextBox();
            this.lblNroProyecto = new System.Windows.Forms.Label();
            this.masterPrefijoOrdenTrabajo = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_MasterFind();
            this.lblPrefijoProy = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).BeginInit();
            this.grpboxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaEntr.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaEntr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSol.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSol.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // RepositoryEdit
            // 
            // 
            // editBtnGridCargos
            // 
            this.editBtnGridCargos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)});
            this.editBtnGridCargos.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.editBtnGridCargos.Name = "editBtnGridCargos";
            // 
            // editSpinPorc
            // 
            this.editSpinPorc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorc.Mask.EditMask = "P";
            this.editSpinPorc.Mask.UseMaskAsDisplayFormat = true;
            this.editSpinPorc.Name = "editSpinPorc";
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
            this.editSpinPorcen,
            this.editBtnGridCargos,
            this.editSpinPorc});
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
            this.editValue.Mask.EditMask = "c4";
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
            this.gbGridDocument.Size = new System.Drawing.Size(820, 243);
            // 
            // grpboxHeader
            // 
            this.grpboxHeader.Controls.Add(this.lblLugarEntrega);
            this.grpboxHeader.Controls.Add(this.lblPrefijoProy);
            this.grpboxHeader.Controls.Add(this.btnProyectoNro);
            this.grpboxHeader.Controls.Add(this.txtOrdenTrabajoNro);
            this.grpboxHeader.Controls.Add(this.lblNroProyecto);
            this.grpboxHeader.Controls.Add(this.masterPrefijoOrdenTrabajo);
            this.grpboxHeader.Controls.Add(this.btnQueryDoc);
            this.grpboxHeader.Controls.Add(this.cmbDestino);
            this.grpboxHeader.Controls.Add(this.lblDestino);
            this.grpboxHeader.Controls.Add(this.masterUsuario);
            this.grpboxHeader.Controls.Add(this.cmbPrioridad);
            this.grpboxHeader.Controls.Add(this.txtDescDoc);
            this.grpboxHeader.Controls.Add(this.lblDescDoc);
            this.grpboxHeader.Controls.Add(this.dtFechaSol);
            this.grpboxHeader.Controls.Add(this.masterLugarEntr);
            this.grpboxHeader.Controls.Add(this.masterAreaAprob);
            this.grpboxHeader.Controls.Add(this.lblPrioridad);
            this.grpboxHeader.Controls.Add(this.dtFechaEntr);
            this.grpboxHeader.Controls.Add(this.lblFechaEntr);
            this.grpboxHeader.Controls.Add(this.lblFechaSol);
            this.grpboxHeader.Controls.Add(this.txtSolicitudNro);
            this.grpboxHeader.Controls.Add(this.lblSolicitudNro);
            this.grpboxHeader.Controls.Add(this.masterPrefijo);
            this.grpboxHeader.Size = new System.Drawing.Size(1117, 126);
            // 
            // editSpinPorcen
            // 
            this.editSpinPorcen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.editSpinPorcen.Mask.EditMask = "P3";
            this.editSpinPorcen.Mask.UseMaskAsDisplayFormat = true;
            // 
            // btnQueryDoc
            // 
            this.btnQueryDoc.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnQueryDoc.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnQueryDoc.Location = new System.Drawing.Point(546, 14);
            this.btnQueryDoc.Name = "btnQueryDoc";
            this.btnQueryDoc.Size = new System.Drawing.Size(28, 20);
            this.btnQueryDoc.TabIndex = 21426;
            this.btnQueryDoc.ToolTip = "1005_btnQueryDoc";
            this.btnQueryDoc.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // masterLugarEntr
            // 
            this.masterLugarEntr.BackColor = System.Drawing.Color.Transparent;
            this.masterLugarEntr.Filtros = null;
            this.masterLugarEntr.Location = new System.Drawing.Point(20, 34);
            this.masterLugarEntr.Name = "masterLugarEntr";
            this.masterLugarEntr.Size = new System.Drawing.Size(291, 22);
            this.masterLugarEntr.TabIndex = 4;
            this.masterLugarEntr.Value = "";
            // 
            // masterAreaAprob
            // 
            this.masterAreaAprob.BackColor = System.Drawing.Color.Transparent;
            this.masterAreaAprob.Filtros = null;
            this.masterAreaAprob.Location = new System.Drawing.Point(20, 78);
            this.masterAreaAprob.Name = "masterAreaAprob";
            this.masterAreaAprob.Size = new System.Drawing.Size(291, 22);
            this.masterAreaAprob.TabIndex = 8;
            this.masterAreaAprob.Value = "";
            // 
            // dtFechaEntr
            // 
            this.dtFechaEntr.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaEntr.Location = new System.Drawing.Point(465, 59);
            this.dtFechaEntr.Name = "dtFechaEntr";
            this.dtFechaEntr.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaEntr.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaEntr.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaEntr.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaEntr.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaEntr.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaEntr.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaEntr.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaEntr.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaEntr.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaEntr.Size = new System.Drawing.Size(108, 20);
            this.dtFechaEntr.TabIndex = 7;
            // 
            // lblFechaEntr
            // 
            this.lblFechaEntr.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaEntr.Location = new System.Drawing.Point(360, 62);
            this.lblFechaEntr.Margin = new System.Windows.Forms.Padding(4);
            this.lblFechaEntr.Name = "lblFechaEntr";
            this.lblFechaEntr.Size = new System.Drawing.Size(87, 14);
            this.lblFechaEntr.TabIndex = 187;
            this.lblFechaEntr.Text = "71_lblFechaEntr";
            // 
            // lblFechaSol
            // 
            this.lblFechaSol.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFechaSol.Location = new System.Drawing.Point(360, 39);
            this.lblFechaSol.Margin = new System.Windows.Forms.Padding(4);
            this.lblFechaSol.Name = "lblFechaSol";
            this.lblFechaSol.Size = new System.Drawing.Size(80, 14);
            this.lblFechaSol.TabIndex = 185;
            this.lblFechaSol.Text = "71_lblFechaSol";
            // 
            // txtSolicitudNro
            // 
            this.txtSolicitudNro.Location = new System.Drawing.Point(465, 14);
            this.txtSolicitudNro.Name = "txtSolicitudNro";
            this.txtSolicitudNro.Size = new System.Drawing.Size(77, 20);
            this.txtSolicitudNro.TabIndex = 2;
            this.txtSolicitudNro.Enter += new System.EventHandler(this.txtSolicitudNro_Enter);
            this.txtSolicitudNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSolicitudNro_KeyPress);
            this.txtSolicitudNro.Leave += new System.EventHandler(this.txtSolicitudNro_Leave);
            // 
            // lblSolicitudNro
            // 
            this.lblSolicitudNro.AutoSize = true;
            this.lblSolicitudNro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolicitudNro.Location = new System.Drawing.Point(357, 16);
            this.lblSolicitudNro.Name = "lblSolicitudNro";
            this.lblSolicitudNro.Size = new System.Drawing.Size(103, 14);
            this.lblSolicitudNro.TabIndex = 180;
            this.lblSolicitudNro.Text = "71_lblSolicitudNro";
            // 
            // masterPrefijo
            // 
            this.masterPrefijo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijo.Filtros = null;
            this.masterPrefijo.Location = new System.Drawing.Point(20, 12);
            this.masterPrefijo.Name = "masterPrefijo";
            this.masterPrefijo.Size = new System.Drawing.Size(291, 22);
            this.masterPrefijo.TabIndex = 1;
            this.masterPrefijo.Value = "";
            this.masterPrefijo.Enter += new System.EventHandler(this.masterPrefijo_Enter);
            this.masterPrefijo.Leave += new System.EventHandler(this.masterPrefijo_Leave);
            // 
            // dtFechaSol
            // 
            this.dtFechaSol.EditValue = new System.DateTime(2011, 9, 21, 0, 0, 0, 0);
            this.dtFechaSol.Location = new System.Drawing.Point(465, 36);
            this.dtFechaSol.Name = "dtFechaSol";
            this.dtFechaSol.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.dtFechaSol.Properties.Appearance.Options.UseBackColor = true;
            this.dtFechaSol.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtFechaSol.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaSol.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaSol.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtFechaSol.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.dtFechaSol.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtFechaSol.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtFechaSol.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtFechaSol.Size = new System.Drawing.Size(108, 20);
            this.dtFechaSol.TabIndex = 5;
            this.dtFechaSol.DateTimeChanged += new System.EventHandler(this.dtFechas_DateTimeChanged);
            // 
            // lblDescDoc
            // 
            this.lblDescDoc.AutoSize = true;
            this.lblDescDoc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescDoc.Location = new System.Drawing.Point(598, 59);
            this.lblDescDoc.Name = "lblDescDoc";
            this.lblDescDoc.Size = new System.Drawing.Size(86, 14);
            this.lblDescDoc.TabIndex = 21423;
            this.lblDescDoc.Text = "71_lblDescDoc";
            // 
            // lblPrioridad
            // 
            this.lblPrioridad.AutoSize = true;
            this.lblPrioridad.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrioridad.Location = new System.Drawing.Point(598, 37);
            this.lblPrioridad.Name = "lblPrioridad";
            this.lblPrioridad.Size = new System.Drawing.Size(85, 14);
            this.lblPrioridad.TabIndex = 188;
            this.lblPrioridad.Text = "71_lblPrioridad";
            // 
            // txtDescDoc
            // 
            this.txtDescDoc.Location = new System.Drawing.Point(599, 74);
            this.txtDescDoc.Multiline = true;
            this.txtDescDoc.Name = "txtDescDoc";
            this.txtDescDoc.Size = new System.Drawing.Size(238, 42);
            this.txtDescDoc.TabIndex = 10;
            // 
            // cmbPrioridad
            // 
            this.cmbPrioridad.AllowDrop = true;
            this.cmbPrioridad.FormattingEnabled = true;
            this.cmbPrioridad.Location = new System.Drawing.Point(701, 35);
            this.cmbPrioridad.Name = "cmbPrioridad";
            this.cmbPrioridad.Size = new System.Drawing.Size(136, 21);
            this.cmbPrioridad.TabIndex = 3;
            // 
            // masterUsuario
            // 
            this.masterUsuario.BackColor = System.Drawing.Color.Transparent;
            this.masterUsuario.Filtros = null;
            this.masterUsuario.Location = new System.Drawing.Point(20, 56);
            this.masterUsuario.Name = "masterUsuario";
            this.masterUsuario.Size = new System.Drawing.Size(291, 22);
            this.masterUsuario.TabIndex = 6;
            this.masterUsuario.Value = "";
            this.masterUsuario.Enter += new System.EventHandler(this.masterUsuario_Enter);
            this.masterUsuario.Leave += new System.EventHandler(this.masterUsuario_Leave);
            // 
            // cmbDestino
            // 
            this.cmbDestino.AllowDrop = true;
            this.cmbDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestino.FormattingEnabled = true;
            this.cmbDestino.Location = new System.Drawing.Point(465, 81);
            this.cmbDestino.Name = "cmbDestino";
            this.cmbDestino.Size = new System.Drawing.Size(108, 21);
            this.cmbDestino.TabIndex = 9;
            // 
            // lblDestino
            // 
            this.lblDestino.AutoSize = true;
            this.lblDestino.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestino.Location = new System.Drawing.Point(357, 84);
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Size = new System.Drawing.Size(80, 14);
            this.lblDestino.TabIndex = 21425;
            this.lblDestino.Text = "71_lblDestino";
            // 
            // btnProyectoNro
            // 
            this.btnProyectoNro.Image = global::NewAge.Properties.Resources.FindFkHierarchy;
            this.btnProyectoNro.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnProyectoNro.Location = new System.Drawing.Point(1010, 10);
            this.btnProyectoNro.Name = "btnProyectoNro";
            this.btnProyectoNro.Size = new System.Drawing.Size(28, 20);
            this.btnProyectoNro.TabIndex = 21430;
            this.btnProyectoNro.ToolTip = "1005_btnQueryDoc";
            this.btnProyectoNro.Visible = false;
            this.btnProyectoNro.Click += new System.EventHandler(this.btnQueryDoc_Click);
            // 
            // txtProyectoNro
            // 
            this.txtOrdenTrabajoNro.Location = new System.Drawing.Point(960, 10);
            this.txtOrdenTrabajoNro.Name = "txtProyectoNro";
            this.txtOrdenTrabajoNro.Size = new System.Drawing.Size(47, 20);
            this.txtOrdenTrabajoNro.TabIndex = 21428;
            this.txtOrdenTrabajoNro.Visible = false;
            this.txtOrdenTrabajoNro.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSolicitudNro_KeyPress);
            this.txtOrdenTrabajoNro.Leave += new System.EventHandler(this.txtProyectoNro_Leave);
            // 
            // lblNroProyecto
            // 
            this.lblNroProyecto.AutoSize = true;
            this.lblNroProyecto.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroProyecto.Location = new System.Drawing.Point(898, 14);
            this.lblNroProyecto.Name = "lblNroProyecto";
            this.lblNroProyecto.Size = new System.Drawing.Size(107, 14);
            this.lblNroProyecto.TabIndex = 21429;
            this.lblNroProyecto.Text = "71_lblProyectoNro";
            this.lblNroProyecto.Visible = false;
            // 
            // masterPrefijoProyecto
            // 
            this.masterPrefijoOrdenTrabajo.BackColor = System.Drawing.Color.Transparent;
            this.masterPrefijoOrdenTrabajo.Filtros = null;
            this.masterPrefijoOrdenTrabajo.Location = new System.Drawing.Point(601, 9);
            this.masterPrefijoOrdenTrabajo.Name = "masterPrefijoProyecto";
            this.masterPrefijoOrdenTrabajo.Size = new System.Drawing.Size(300, 22);
            this.masterPrefijoOrdenTrabajo.TabIndex = 21427;
            this.masterPrefijoOrdenTrabajo.Value = "";
            this.masterPrefijoOrdenTrabajo.Visible = false;
            // 
            // lblPrefijoProy
            // 
            this.lblPrefijoProy.AutoSize = true;
            this.lblPrefijoProy.Visible = false;
            this.lblPrefijoProy.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrefijoProy.Location = new System.Drawing.Point(598, 14);
            this.lblPrefijoProy.Name = "lblPrefijoProy";
            this.lblPrefijoProy.Size = new System.Drawing.Size(97, 14);
            this.lblPrefijoProy.TabIndex = 21431;
            this.lblPrefijoProy.Text = "71_lblPrefijoProy";
            // 
            // lblLugarEntrega
            // 
            this.lblLugarEntrega.AutoSize = true;
            this.lblLugarEntrega.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLugarEntrega.Location = new System.Drawing.Point(17, 39); 
            this.lblLugarEntrega.Name = "lblLugarEntrega";
            this.lblLugarEntrega.Size = new System.Drawing.Size(108, 14);
            this.lblLugarEntrega.TabIndex = 21432;
            this.lblLugarEntrega.Text = "71_lblLugarEntrega";
            // 
            // Solicitud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 581);
            this.Name = "Solicitud";
            this.Text = "Solicitud";
            ((System.ComponentModel.ISupportInitialize)(this.editChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtnGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCmb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editSpin7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editValue4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMark.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editText)).EndInit();
            this.grpboxHeader.ResumeLayout(false);
            this.grpboxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSpinPorcen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaEntr.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaEntr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSol.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFechaSol.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlsUC.uc_MasterFind masterLugarEntr;
        private ControlsUC.uc_MasterFind masterAreaAprob;
        protected DevExpress.XtraEditors.DateEdit dtFechaEntr;
        protected DevExpress.XtraEditors.LabelControl lblFechaEntr;
        protected DevExpress.XtraEditors.LabelControl lblFechaSol;
        private System.Windows.Forms.TextBox txtSolicitudNro;
        private System.Windows.Forms.Label lblSolicitudNro;
        private ControlsUC.uc_MasterFind masterPrefijo;
        protected DevExpress.XtraEditors.DateEdit dtFechaSol;
        private System.Windows.Forms.Label lblDescDoc;
        private System.Windows.Forms.Label lblPrioridad;
        private System.Windows.Forms.TextBox txtDescDoc;
        private NewAge.Cliente.GUI.WinApp.Clases.ComboBoxEx cmbPrioridad;
        private ControlsUC.uc_MasterFind masterUsuario;
        private Clases.ComboBoxEx cmbDestino;
        private System.Windows.Forms.Label lblDestino;
        private DevExpress.XtraEditors.SimpleButton btnQueryDoc;
        private DevExpress.XtraEditors.SimpleButton btnProyectoNro;
        private System.Windows.Forms.TextBox txtOrdenTrabajoNro;
        private System.Windows.Forms.Label lblNroProyecto;
        private ControlsUC.uc_MasterFind masterPrefijoOrdenTrabajo;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit editBtnGridCargos;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit editSpinPorc;
        private System.Windows.Forms.Label lblPrefijoProy;
        private System.Windows.Forms.Label lblLugarEntrega;
    }
}