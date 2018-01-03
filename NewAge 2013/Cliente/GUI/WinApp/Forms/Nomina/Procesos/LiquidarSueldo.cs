using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Resultados;
using System.Threading;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class LiquidarSueldo : ProcessForm
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _resPreliminar = null;
  

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void LiquidarSueldoProcess();
        public LiquidarSueldoProcess liquidarSueldo;
        public void LiquidarSueldoMethod()
        {
            //Inicializa la data del control
            long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.noEmpleado, null, null, true);
            List<DTO_MasterBasic> lEmpleados = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.noEmpleado, Convert.ToInt32(count), 1, null, null, true).ToList();
            _bc.AdministrationModel.LiquidarPrestamos(lEmpleados);
        }
       

        #endregion

        public LiquidarSueldo()
        {
            InitializeComponent();
            this.liquidarSueldo = new LiquidarSueldoProcess(LiquidarSueldoMethod);
        }

        #region Eventos

        private void btnLLiquidarSueldo_Click(object sender, EventArgs e)
        {
            new Thread(ProcesarThread).Start();
        }

        #endregion

        #region Hilos
              

        /// <summary>
        /// Hilo de Procesar Cierre
        /// </summary>
        private void ProcesarThread()
        {
            try
            {
                this.Invoke(this.liquidarSueldo);
            }
            catch (Exception ex)
            { throw ex; }           
        }

        #endregion

      
    }
}
