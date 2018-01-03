using DevExpress.XtraGrid.Columns;
using NewAge.Librerias.Project;
using DevExpress.Data;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using NewAge.DTO.UDT;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class VerificacionPreliminar : SolicitudCreditoChequeo
    {
        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        #endregion

        public VerificacionPreliminar()
            : base() { }

        public VerificacionPreliminar(string mod)
            : base(mod) { }

        /// <summary>
        /// Constructor que permite filtrar la libranza y validar si es solo lectura para Modulo cf
        /// </summary>
        /// <param name="libranza"> Libranza o credito a filtrar</param>
        /// <param name="readOnly"> Si es solo lectura</param>
        public VerificacionPreliminar(int libranza, bool readOnly) : base(ModulesPrefix.cf.ToString())
        {
            this.gvDocuments.ActiveFilterString = "StartsWith([Unbound_Libranza]," + libranza.ToString() + ")";
            if (readOnly)
            {
                this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Visible = false;
                this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"].Visible = false;
                FormProvider.Master.itemSave.Enabled = false;
            }
        }

        #region Funciones Virtuales

        /// <summary>
        ///  Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.VerificacionPreliminar;

            base.SetInitParameters();
            this.tableLayoutPanel1.RowStyles[1].Height = 300;
            this.tableLayoutPanel1.RowStyles[2].Height = 250;
            this.tableLayoutPanel1.RowStyles[3].Height = 0;

            //Deshabilita los botones +- de la grilla
            this.gcDocuments.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDocuments.EmbeddedNavigator.Buttons.CustomButtons[0].Enabled = false;

            this.gvDocuments.OptionsView.ShowAutoFilterRow = true;
            this.gcTareas.Visible = false;
        }

        /// <summary>
        /// Se ejecuta luego del initializecomponents
        /// </summary>
        protected override void AfterInitialize()
        {
            //Carga la actividades a revertir
            List<DTO_glActividadFlujo> actPadres = _bc.AdministrationModel.glActividadFlujo_GetParents(this.actividadFlujoID);
            foreach(DTO_glActividadFlujo act in actPadres)
            {
                this.actividadesCombo.Add(act.Descriptivo.Value);
                this.actFlujoForReversion.Add(act.Descriptivo.Value, act.ID.Value);
            }
            this.editCmb.Items.AddRange(this.actividadesCombo);
        }

        #endregion

    }
}



