using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.DTO.Resultados;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Base;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    class ccCarteraComponente : MasterSimpleForm
    {
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();

        ///<summary>
        /// Constructor 
        /// </summary>
        public ccCarteraComponente() : base() { }

        ///<summary>
        /// Constructor 
        /// </summary>
        public ccCarteraComponente(string mod) : base(mod) { }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void InitForm()
        {
            this.DocumentID = AppMasters.ccCarteraComponente;
            base.InitForm();
        }

        #region Validaciones Maestra
        /// <summary>
        /// Coloca en la celda booleana el editor de celda(check) requerido
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        protected override void gvRecordEdit_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            base.gvRecordEdit_CustomRowCellEditForEditing(sender, e);
            GridProperty gpHandle = (GridProperty)e.Column.View.GetFocusedRow();
            FieldConfiguration config = this.GetFieldConfigByCaption(gpHandle.Campo);
            if (config.FieldName == "PagoTotalInd")
            {
                DTO_ccCarteraComponente componete = (DTO_ccCarteraComponente)this.gvModule.GetFocusedRow();
                if (componete !=  null && componete.Descriptivo.Value == "CAPITAL")
                {
                    CheckFieldConfiguration cfc = (CheckFieldConfiguration)config;
                    chkRecordEdit.ReadOnly = true;
                    e.RepositoryItem = this.chkRecordEdit;
                }
                else
                {
                    CheckFieldConfiguration cfc = (CheckFieldConfiguration)config;
                    chkRecordEdit.ReadOnly = false;
                    e.RepositoryItem = this.chkRecordEdit;
                }
            }
        }
        #endregion
    }
}
