using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.DTO.Negocio;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalNominaValidacion : Form
    {
        #region Variables

        /// <summary>
        /// Instancia Base Controller
        /// </summary>
        BaseController _bc = BaseController.GetInstance();
             
        /// <summary>
        /// Lista de Empleados Global
        /// </summary>
        List<DTO_noEmpleado> LEmpleados = new List<DTO_noEmpleado>();

        /// <summary>
        /// Lista de Empleados Global de Salida
        /// </summary>
        List<DTO_noEmpleado> LEmpleadosSalida = new List<DTO_noEmpleado>();

        /// <summary>
        /// Listado de Validaciones
        /// </summary>
        List<DTO_noValidaciones> LValidaciones = new List<DTO_noValidaciones>();

        /// <summary>
        /// Indica si continua con la ejecucion
        /// </summary>
        bool continuar = false;

        /// <summary>
        /// Existen Validaciones
        /// </summary>
        bool hayValidaciones = false;

        /// <summary>
        /// Prefijo Unbound
        /// </summary>
        protected string unboundPrefix = "Unbound_";

        #endregion

        /// <summary>
        /// Construtor de modal
        /// </summary>
        /// <param name="lEmpleados">lempleados</param>
        /// <param name="documentoID">documentoID</param>
        public ModalNominaValidacion(List<DTO_noEmpleado> lEmpleados, int documentoID)
        {
            //InitializeComponent();
            this.SetInitParameters();
            FormProvider.LoadResources(this, AppMasters.noEmpleado);

         
        }

        #region Funciones Privadas

        /// <summary>
        /// Inicializa los controles
        /// </summary>
        public void InitControls()
        {
            switch (this.DocumentoID)
            {
                case AppDocuments.Nomina:
                    { 
                        this.ValidacionNomina();
                        break;
                    }
            }

            if (LValidaciones.Count > 0)
            {
                this.hayValidaciones = true;
                this.gcValidacion.DataSource = this.LValidaciones;
            }
            else
            {
                this.hayValidaciones = false;
            }
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        protected void SetInitParameters()
        {
            InitializeComponent();
            this.DocumentoID = AppMasters.noEmpleado;
            this.InitControls();
        }


        /// <summary> 
        /// Validaciones Propias del Documento 81 de Nomina
        /// </summary>
        private void ValidacionNomina()
        {
            foreach (var emp in LEmpleados)
            {
                DTO_noValidaciones validacion = null;
                var estadoEmpl = this._bc.AdministrationModel.Nomina_GetEstadoLiquidaciones(emp.ID.Value);

                if (estadoEmpl.EnVacaciones.Value.Value == 1)
                {
                    validacion = new DTO_noValidaciones();
                    validacion.EmpleadoID.Value = emp.ID.Value;
                    validacion.EmpleadoDesc.Value = emp.Descriptivo.Value;
                    validacion.Estado.Value = this._bc.GetResource(LanguageTypes.Forms , AppMasters.noEmpleado.ToString() + "_EstadoVacaciones");
                    validacion.Descripcion.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_DescripcionVacaciones");
                }

                //Si la liquidación de Nomina ya esta aprobada
                if (estadoEmpl.EstadoLiqNomina.Value == (byte)EstadoDocControl.Aprobado)
                {
                    validacion = new DTO_noValidaciones();
                    validacion.EmpleadoID.Value = emp.ID.Value;
                    validacion.EmpleadoDesc.Value = emp.Descriptivo.Value;
                    validacion.Estado.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_EstadoLiqAprobada");
                    validacion.Descripcion.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_DescripcionLiqAprobada");
                }

                //Si la liquidación de Vacaciones Preliminar
                if (estadoEmpl.EstadoLiqVacaciones.Value == (byte)EstadoDocControl.ParaAprobacion)
                {
                    validacion = new DTO_noValidaciones();
                    validacion.EmpleadoID.Value = emp.ID.Value;
                    validacion.EmpleadoDesc.Value = emp.Descriptivo.Value;
                    validacion.Estado.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_EstadoLiqVacaciones");
                    validacion.Descripcion.Value = this._bc.GetResource(LanguageTypes.Forms, AppMasters.noEmpleado.ToString() + "_DescripcionLiqVacaciones");
                 }

                //Valida si efectivamente tiene pendientes y lo agrega
                if (validacion != null)
                {
                    this.LValidaciones.Add(validacion);
                }
                else
                {
                    this.LEmpleadosSalida.Add(emp);
                }
            }
        }

        #endregion
                
        #region Propiedades

        /// <summary>
        /// Lista de Empleados
        /// </summary>
        public List<DTO_noEmpleado> Empleados 
        { 
            get 
            {
                return LEmpleadosSalida; 
            } 
        }

        /// <summary>
        /// Indica si continua con la ejecucion
        /// </summary>
        public bool Continuar
        {
            get { return continuar; }
        }

        /// <summary>
        /// cambia si hubo validaciones 
        /// </summary>
        public bool HayValidaciones
        {
            get { return hayValidaciones; } 
        }

        /// <summary>
        /// Documento de Liquidación
        /// </summary>
        public int DocumentoID
        {
            get;
            set;
        }

        #endregion

        #region Eventos 

        /// <summary>
        /// Evento boton Continuar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContinuar_Click(object sender, EventArgs e)
        {
            this.continuar = true;
            this.Close();
        }

        /// <summary>
        /// Evento Boton Cancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.continuar = false;
            this.Close();
        }

        #endregion
    }
}
