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
    public partial class LiquidarAutomatica : Form
    {
        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _resPreliminar = null;
        private DTO_glDocumentoControl _dtoControl = null;
        private DTO_noLiquidacionesDocu _dtoLiquidacion = null;

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        public delegate void LiquidarSueldoProcess();
        public LiquidarSueldoProcess liquidarSueldo;
        public void LiquidarSueldoMethod()
        {
           //Invoca el metodo encargado de liquidar el sueldo
            //_bc.AdministrationModel.LiquidarSueldoNomina();
        }
       

        #endregion

        public LiquidarAutomatica()
        {
            InitializeComponent();
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
