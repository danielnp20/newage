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
        private List<DTO_SolicitudAprobacionCartera> _solAprob = null;
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

            #region Generales
            if (fieldName == "Aprobado")
            {
                if ((bool)e.Value)
                {
                    this.solicitudCredito[e.RowHandle].Aprobado.Value = true;
                    this.solicitudCredito[e.RowHandle].Rechazado.Value = false;
                }
            }

            if (fieldName == "Rechazado")
            {
                if ((bool)e.Value)
                {
                    this.solicitudCredito[e.RowHandle].Aprobado.Value = false;
                    this.solicitudCredito[e.RowHandle].Rechazado.Value = true;
                }
            }

            if (fieldName == "Observacion")
            {
                this.solicitudCredito[this.currentRow].ActividadFlujoDesc = this.msgAnulado;
                this.solicitudCredito[this.currentRow].ActividadFlujoReversion.Value = string.Empty;
                this.solicitudCredito[this.currentRow].Observacion.Value = e.Value.ToString();
            }
            #endregion
            this.ValidateDocRow(e.RowHandle);
            this.gcDocuments.RefreshDataSource();
        }

        #endregion

    }
}
