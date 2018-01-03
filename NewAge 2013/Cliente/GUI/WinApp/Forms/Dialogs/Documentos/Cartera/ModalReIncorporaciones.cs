using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Librerias.Project;
using NewAge.DTO.Negocio;
using SentenceTransformer;
using NewAge.DTO.Resultados;
using System.Globalization;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class ModalReIncorporaciones : Form
    {
        public ModalReIncorporaciones()
        {
            InitializeComponent();
            this.SetInitParameters();
            FormProvider.LoadResources(this, AppDocuments.ReIncorporaciones);
        }

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        DTO_noEmpleado _empleado = null;

        #endregion

        #region Delegados
              

        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Inicializa las controles
        /// </summary>
        private void InitControls()
        {           

            this.uc_Empleados.IsMultipleSeleccion = false;
            List<DTO_noEmpleado> lempleados = new List<DTO_noEmpleado>();
            List<DTO_glConsultaFiltro> lfiltros = new List<DTO_glConsultaFiltro>();
            lfiltros.Add(new DTO_glConsultaFiltro()
            {
                CampoFisico = "Estado",
                ValorFiltro = "3",
                OperadorFiltro = OperadorFiltro.Igual,
                OperadorSentencia = "AND"
            });

            long count = this._bc.AdministrationModel.MasterSimple_Count(AppMasters.noEmpleado, null, lfiltros, true);
            var ltemp = this._bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.noEmpleado, count, 1, null, lfiltros, true);

            foreach (var item in ltemp)
                lempleados.Add((DTO_noEmpleado)item);

            this.uc_Empleados.Init(lempleados);
            this.uc_Empleados.SelectRowEmpleado_Click += new UC_Empleados.EventHandler(uc_Empleados_SelectRowEmpleado_Click);
            _bc.InitMasterUC(this.uc_MasterDocumento, AppMasters.glDocumento, true, true, false, true, null);
            this.uc_MasterDocumento.Value = AppDocuments.ReIncorporaciones.ToString();
            string periodo = _bc.GetControlValueByCompany(ModulesPrefix.no, AppControl.co_Periodo);
            this.dtPeriodo.DateTime = Convert.ToDateTime(periodo);
            this.dtFechaIngreso.DateTime = DateTime.Now;
            if (this.uc_Empleados.empleadoActivo != null)
                this.txtSueldoIngreso.Text = this.uc_Empleados.empleadoActivo.Sueldo.Value.ToString();
            else
            {
                this.btnReincoporar.Enabled = false;
                this.txtSueldoIngreso.Enabled = false;
            }
        } 
        

        /// <summary>
        /// Campos para habilitar o deshabilitar los controles
        /// </summary>
        /// <param name="estado">estado true o false</param>
        private void FieldsEnabled(bool estado)
        {

        }

        /// <summary>
        /// Limpia los campos y objetos del documento
        /// </summary>
        private void RefreshDocument()
        {
            this.txtSueldoIngreso.Text = string.Empty;
            this.uc_Empleados._empleados = null;
            this.InitControls();
        }

        /// <summary>
        /// Inicia las variables antes de iniciar el constructor
        /// </summary>
        private void SetInitParameters()
        {
            this.InitControls();
            this._empleado = this.uc_Empleados.empleadoActivo;
        }

        /// <summary>
        /// Agrega las columnas a la grilla
        /// </summary>
        private void AddGridCols()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-ModalReIncorporaciones", "AddGridCols"));
            }
        }

        /// <summary>
        /// Agrega una nueva fila
        /// </summary>
        private void AddNewRow()
        {
           
        }

        /// <summary>
        /// Funcion que se ejecuta despues de inicilizar el documento
        /// </summary>
        private void AfterInitialize()
        {
         
        }

        #endregion
        
        #region Eventos Control Empleados
        
        /// <summary>
        /// Eventos click en control de empleados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void uc_Empleados_SelectRowEmpleado_Click(object sender, EventArgs e)
        {
            this._empleado = this.uc_Empleados.empleadoActivo;
            this.txtSueldoIngreso.Text = this._empleado.Sueldo.Value.ToString();
        }

        #endregion

        #region Eventos


        /// <summary>
        /// ReIncorpora un empleado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReincoporar_Click(object sender, EventArgs e)
        {
            this._empleado.FechaIngreso.Value = this.dtFechaIngreso.DateTime;
            this._empleado.Sueldo.Value = Convert.ToDecimal(this.txtSueldoIngreso.EditValue, CultureInfo.InvariantCulture);
            var result = _bc.AdministrationModel.Nomina_ReinCorporacionEmpleado(this._empleado);
            if (result.Result == ResultValue.OK)
            {
                this.RefreshDocument();
            }
          
            MessageForm frm = new MessageForm(result);
            frm.ShowDialog();
        }

        #endregion

      
    }
}
