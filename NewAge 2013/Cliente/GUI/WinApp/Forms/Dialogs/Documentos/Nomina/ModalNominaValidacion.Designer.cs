using DevExpress.Data;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class ModalNominaValidacion
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
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.gcValidacion = new DevExpress.XtraGrid.GridControl();
            this.gvValidacion = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.EmpleadoID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EmpleadoDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Estado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Descripcion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Liquidar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemValidar = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.gcValidacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvValidacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemValidar)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(681, 366);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(107, 23);
            this.btnCancelar.TabIndex = 0;
            this.btnCancelar.Text = "29001_btnCancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnContinuar
            // 
            this.btnContinuar.Location = new System.Drawing.Point(600, 366);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(75, 23);
            this.btnContinuar.TabIndex = 1;
            this.btnContinuar.Text = "29001_btnContinuar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click);
            // 
            // gcValidacion
            // 
            this.gcValidacion.AllowDrop = true;
            this.gcValidacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcValidacion.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcValidacion.EmbeddedNavigator.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.gcValidacion.EmbeddedNavigator.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gcValidacion.EmbeddedNavigator.Appearance.Options.UseBackColor = true;
            this.gcValidacion.EmbeddedNavigator.Appearance.Options.UseFont = true;
            this.gcValidacion.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcValidacion.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcValidacion.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcValidacion.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcValidacion.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 6),
            new DevExpress.XtraEditors.NavigatorCustomButton(7, 7, false, false, "", null)});
            this.gcValidacion.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gcValidacion.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcValidacion.EmbeddedNavigator.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Exclamation;
            this.gcValidacion.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcValidacion.Location = new System.Drawing.Point(3, 16);
            this.gcValidacion.LookAndFeel.SkinName = "Dark Side";
            this.gcValidacion.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gcValidacion.MainView = this.gvValidacion;
            this.gcValidacion.Margin = new System.Windows.Forms.Padding(4);
            this.gcValidacion.Name = "gcValidacion";
            this.gcValidacion.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemValidar});
            this.gcValidacion.Size = new System.Drawing.Size(793, 340);
            this.gcValidacion.TabIndex = 2;
            this.gcValidacion.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvValidacion});
            // 
            // gvValidacion
            // 
            this.gvValidacion.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvValidacion.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.gvValidacion.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvValidacion.Appearance.Empty.Options.UseBackColor = true;
            this.gvValidacion.Appearance.FixedLine.BackColor = System.Drawing.Color.DimGray;
            this.gvValidacion.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvValidacion.Appearance.FocusedCell.BackColor = System.Drawing.Color.Gainsboro;
            this.gvValidacion.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.gvValidacion.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvValidacion.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvValidacion.Appearance.FocusedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvValidacion.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.gvValidacion.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvValidacion.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvValidacion.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvValidacion.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvValidacion.Appearance.HeaderPanel.BackColor = System.Drawing.Color.MidnightBlue;
            this.gvValidacion.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.RoyalBlue;
            this.gvValidacion.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gvValidacion.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.White;
            this.gvValidacion.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvValidacion.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvValidacion.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvValidacion.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvValidacion.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvValidacion.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvValidacion.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvValidacion.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvValidacion.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.gvValidacion.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvValidacion.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvValidacion.Appearance.HorzLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvValidacion.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvValidacion.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.gvValidacion.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvValidacion.Appearance.Row.Options.UseBackColor = true;
            this.gvValidacion.Appearance.Row.Options.UseForeColor = true;
            this.gvValidacion.Appearance.Row.Options.UseTextOptions = true;
            this.gvValidacion.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvValidacion.Appearance.SelectedRow.BackColor = System.Drawing.Color.Gainsboro;
            this.gvValidacion.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvValidacion.Appearance.TopNewRow.ForeColor = System.Drawing.Color.Black;
            this.gvValidacion.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvValidacion.Appearance.VertLine.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.gvValidacion.Appearance.VertLine.Options.UseBackColor = true;
            this.gvValidacion.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.EmpleadoID,
            this.EmpleadoDesc,
            this.Estado,
            this.Descripcion,
            this.Liquidar});
            this.gvValidacion.GridControl = this.gcValidacion;
            this.gvValidacion.HorzScrollStep = 50;
            this.gvValidacion.Name = "gvValidacion";
            this.gvValidacion.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.gvValidacion.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvValidacion.OptionsCustomization.AllowColumnMoving = false;
            this.gvValidacion.OptionsCustomization.AllowFilter = false;
            this.gvValidacion.OptionsCustomization.AllowSort = false;
            this.gvValidacion.OptionsMenu.EnableColumnMenu = false;
            this.gvValidacion.OptionsMenu.EnableFooterMenu = false;
            this.gvValidacion.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvValidacion.OptionsView.ColumnAutoWidth = false;
            this.gvValidacion.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gvValidacion.OptionsView.ShowGroupPanel = false;
            // 
            // EmpleadoID
            // 
            this.EmpleadoID.Caption = "EmpleadoID";
            this.EmpleadoID.FieldName = "EmpleadoID";
            this.EmpleadoID.Name = "EmpleadoID";
            this.EmpleadoID.OptionsColumn.ReadOnly = true;
            this.EmpleadoID.Visible = true;
            this.EmpleadoID.VisibleIndex = 0;
            this.EmpleadoID.Width = 46;
            // 
            // EmpleadoDesc
            // 
            this.EmpleadoDesc.Caption = "29001_EmpleadoDesc";
            this.EmpleadoDesc.FieldName = "EmpleadoDesc";
            this.EmpleadoDesc.Name = "EmpleadoDesc";
            this.EmpleadoDesc.OptionsColumn.ReadOnly = true;
            this.EmpleadoDesc.Visible = true;
            this.EmpleadoDesc.VisibleIndex = 1;
            this.EmpleadoDesc.Width = 156;
            // 
            // Estado
            // 
            this.Estado.Caption = "29001_Estado";
            this.Estado.FieldName = "Estado";
            this.Estado.Name = "Estado";
            this.Estado.OptionsColumn.ReadOnly = true;
            this.Estado.Visible = true;
            this.Estado.VisibleIndex = 2;
            this.Estado.Width = 182;
            // 
            // Descripcion
            // 
            this.Descripcion.Caption = "29001_Descripcion";
            this.Descripcion.FieldName = "Descripcion";
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.OptionsColumn.ReadOnly = true;
            this.Descripcion.Visible = true;
            this.Descripcion.VisibleIndex = 3;
            this.Descripcion.Width = 345;
            // 
            // Liquidar
            // 
            this.Liquidar.Caption = "29001_Liquidar";
            this.Liquidar.ColumnEdit = this.repositoryItemValidar;
            this.Liquidar.FieldName = "Liquidar";
            this.Liquidar.Name = "Liquidar";
            this.Liquidar.Visible = true;
            this.Liquidar.VisibleIndex = 4;
            this.Liquidar.Width = 50;
            // 
            // repositoryItemValidar
            // 
            this.repositoryItemValidar.AutoHeight = false;
            this.repositoryItemValidar.Name = "repositoryItemValidar";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gcValidacion);
            this.groupBox1.Location = new System.Drawing.Point(1, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(799, 359);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // ModalNominaValidacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 397);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModalNominaValidacion";
            this.Text = "29001_ModalNominaValidacion";
            ((System.ComponentModel.ISupportInitialize)(this.gcValidacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvValidacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemValidar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnContinuar;
        private DevExpress.XtraGrid.GridControl gcValidacion;
        private DevExpress.XtraGrid.Views.Grid.GridView gvValidacion;
        private DevExpress.XtraGrid.Columns.GridColumn EmpleadoID;
        private DevExpress.XtraGrid.Columns.GridColumn EmpleadoDesc;
        private DevExpress.XtraGrid.Columns.GridColumn Estado;
        private DevExpress.XtraGrid.Columns.GridColumn Descripcion;
        private DevExpress.XtraGrid.Columns.GridColumn Liquidar;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemValidar;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}