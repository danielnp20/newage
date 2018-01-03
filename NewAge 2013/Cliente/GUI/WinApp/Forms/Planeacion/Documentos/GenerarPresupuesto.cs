using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
using NewAge.DTO.UDT;
using NewAge.Cliente.GUI.WinApp.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using NewAge.DTO.Attributes;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class GenerarPresupuesto : DocumentPresupuesto
    {
        /// <summary>
        /// Constructor del Documento
        /// </summary>
        public GenerarPresupuesto()
        {
            try
            {
                //InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GenerarPresupuesto.cs", "GenerarPresupuesto"));
            }
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            try
            {
                this.documentID = AppDocuments.Presupuesto;
                this._frmModule = ModulesPrefix.pl;

                base.SetInitParameters();
                this.format = _bc.GetImportExportFormat(typeof(DTO_plPresupuestoDeta), this.documentID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-GenerarPresupuesto.cs", "SetInitParameters"));
            }
        }

        #endregion

    }
}
