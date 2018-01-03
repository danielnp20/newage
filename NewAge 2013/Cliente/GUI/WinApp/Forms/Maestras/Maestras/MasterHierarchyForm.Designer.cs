using System;
namespace NewAge.Cliente.GUI.WinApp.Forms
{
    partial class MasterHierarchyForm
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            base.InitializeComponent();
            this.groupHierarchy = new DevExpress.XtraEditors.GroupControl();
            this.hierarchyCtrl = new NewAge.Cliente.GUI.WinApp.ControlsUC.uc_Hierarchy();
            this.btnResetJerarquia = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.groupHierarchy)).BeginInit();
            this.groupHierarchy.SuspendLayout();
            this.pnlRecordEdit.Controls.Add(this.groupHierarchy);
            // 
            // groupHierarchy
            // 
            this.groupHierarchy.Appearance.BackColor = System.Drawing.Color.Honeydew;
            this.groupHierarchy.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.groupHierarchy.Appearance.Options.UseBackColor = true;
            this.groupHierarchy.Appearance.Options.UseFont = true;
            this.groupHierarchy.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 11F);
            this.groupHierarchy.AppearanceCaption.Options.UseFont = true;
            this.groupHierarchy.Controls.Add(this.hierarchyCtrl);
            this.groupHierarchy.Controls.Add(this.btnResetJerarquia);
            this.groupHierarchy.Location = new System.Drawing.Point(415, 35);
            this.groupHierarchy.LookAndFeel.SkinName = "iMaginary";
            this.groupHierarchy.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupHierarchy.Name = "groupHierarchy";
            this.groupHierarchy.Size = new System.Drawing.Size(397, 220);
            this.groupHierarchy.TabIndex = 0;
            this.groupHierarchy.Text = "Jerarquia";
            // 
            // hierarchyCtrl
            // 
            this.hierarchyCtrl.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.hierarchyCtrl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.hierarchyCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hierarchyCtrl.Location = new System.Drawing.Point(2, 26);
            this.hierarchyCtrl.Name = "hierarchyCtrl";
            this.hierarchyCtrl.Size = new System.Drawing.Size(393, 184);
            this.hierarchyCtrl.TabIndex = 0;
            this.hierarchyCtrl.Leave += new System.EventHandler(this.hierarchyCtrl_Leave);
            // 
            // btnResetJerarquia
            // 
            this.btnResetJerarquia.Location = new System.Drawing.Point(326, 1);
            this.btnResetJerarquia.Name = "btnResetJerarquia";
            this.btnResetJerarquia.Size = new System.Drawing.Size(55, 23);
            this.btnResetJerarquia.TabIndex = 74;
            this.btnResetJerarquia.Text = "Reset";
            this.btnResetJerarquia.UseVisualStyleBackColor = true;
            this.btnResetJerarquia.Click += new System.EventHandler(this.btnResetJerarquia_Click);
            ((System.ComponentModel.ISupportInitialize)(this.groupHierarchy)).EndInit();
            this.groupHierarchy.ResumeLayout(false);
        }

        #endregion

        protected DevExpress.XtraEditors.GroupControl groupHierarchy;
        protected System.Windows.Forms.Button btnResetJerarquia;
        protected ControlsUC.uc_Hierarchy hierarchyCtrl;
    }
}