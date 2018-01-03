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
using System.Threading;
using NewAge.DTO.Resultados;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class EnvioSolicitudLibranza : SolicitudCreditoChequeo
    {

        #region Variables formulario
        //Obtiene la instancia del controlador base
        private BaseController _bc = BaseController.GetInstance();
        //Listas de datos
        #endregion

        public EnvioSolicitudLibranza()
            : base()
        {
            //InitializeComponent();
        }

        public EnvioSolicitudLibranza(string mod)
            : base(mod)
        {
        }

        /// <summary>
        /// Constructor que permite filtrar la libranza y validar si es solo lectura para Modulo cf
        /// </summary>
        /// <param name="libranza"> Libranza o credito a filtrar</param>
        /// <param name="readOnly"> Si es solo lectura</param>
        public EnvioSolicitudLibranza(int libranza, bool readOnly) : base(ModulesPrefix.cf.ToString())
        {
            this.gvDocuments.ActiveFilterString = "StartsWith([Unbound_Libranza]," + libranza.ToString() +")";
            if (readOnly)
            {
                this.gvDocuments.Columns[this.unboundPrefix + "Aprobado"].Visible = false;
                this.gvDocuments.Columns[this.unboundPrefix + "Rechazado"].Visible = false;
                FormProvider.Master.itemSave.Enabled = false;
            }
        }

        #region Funciones Virtuales

        /// <summary>
        /// Inicializa los parametros de la pantalla
        /// </summary>
        protected override void SetInitParameters()
        {
            this.documentID = AppDocuments.EnvioSolicitudLibranza;
            this.frmModule = ModulesPrefix.cc;

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
            foreach (DTO_glActividadFlujo act in actPadres)
            {
                this.actividadesCombo.Add(act.Descriptivo.Value);
                this.actFlujoForReversion.Add(act.Descriptivo.Value, act.ID.Value);
            }
            this.editCmb.Items.AddRange(this.actividadesCombo);

        }

        #endregion

        #region Eventos Grilla

        /// <summary>
        /// Calcula los valores y hace operacines al momento mientras ingreso valores
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>  
        protected override void gvDocuments_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Le quita los caracteres de unbound para diferenciarlas de las columnas normales
            string fieldName = e.Column.FieldName.Substring(this.unboundPrefix.Length);
            DTO_SolicitudAprobacionCartera row = (DTO_SolicitudAprobacionCartera)this.gvDocuments.GetRow(e.RowHandle);

            #region Generales
            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                {
                    row.Aprobado.Value = true;
                    row.Rechazado.Value = false;
                }
            }

            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                {
                    row.Aprobado.Value = false;
                    row.Rechazado.Value = true;
                }
            }

            if (fieldName == "Observacion")
            {
                row.ActividadFlujoDesc = this.msgRechazo;
                row.ActividadFlujoReversion.Value = this.actFlujoForReversion[this.actividadesCombo.Count > 0 ? this.actividadesCombo.Last() : string.Empty];
                row.Observacion.Value = e.Value.ToString();
            }
            #endregion
            this.ValidateDocRow(e.RowHandle);
            this.gcDocuments.RefreshDataSource();
        }

        #endregion

    }
}
