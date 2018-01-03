using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewAge.Librerias.Project;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.DTO.Negocio;
using System.Threading;
using NewAge.DTO.Resultados;
using System.Diagnostics;
using SentenceTransformer;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public partial class CierreDiaCartera : ProcessForm
    {
        #region Delegados

        /// <summary>
        /// Delegado que finaliza el proceso de generacion de preliminares
        /// </summary>
        private delegate void EndProcesar();
        private EndProcesar EndProcesarDelegate;
        private void EndProcesarMethod()
        {
            if (this._resProcesar.Result == ResultValue.OK)
                this.Close();
            else
            {
                this.btnCierre.Enabled = true;
                this.ControlBox = true;
                this.Enabled = true;

                //string numEmpresa = _bc.AdministrationModel.Empresa.NumeroControl.Value;
                //_bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, numEmpresa).ToList();
                //this.LoadFechaCierre();
            }
        }

        #endregion

        //public CierreDiaCartera()
        //{
        //    this.InitializeComponent();
        //}

        #region Variables

        BaseController _bc = BaseController.GetInstance();
        private DTO_TxResult _resProcesar = null;
        private bool _cierreDiarioInd = true;

        private DateTime maxDate;
        #endregion

        #region Funciones Privadas

        /// <summary>
        /// Carga la información de la fecha de cierre
        /// </summary>
        private void LoadFechaCierre()
        {
            //Fecha inicial
            string diaIniStr = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_DiaUltimoCierre);
            int diaIni = string.IsNullOrWhiteSpace(diaIniStr) ? 1 : Convert.ToInt16(diaIniStr) + 1;
            if (diaIni <= this.maxDate.Day)
            {
                diaIni = !this._cierreDiarioInd ? this.maxDate.Day : diaIni; //valida si no maneja CIerreDiario le pone el ultimo dia del mes
                DateTime minDate = new DateTime(this.dtPeriod.DateTime.Year, this.dtPeriod.DateTime.Month, diaIni);

                this.dtFechaCierre.Properties.MinValue = minDate;
                this.dtFechaCierre.DateTime = minDate;
            }
            else
            {
                this.dtFechaCierre.DateTime = this.maxDate;
                this.dtFechaCierre.Enabled = false;
                this.btnCierre.Enabled = false;
            }
        }

        /// <summary>
        /// Envia correos a los clientes que cumplan años el dia del cierre
        /// </summary>
        private void SendEmailCumpleaños()
        {
            try
            {
                DateTime cumple = DateTime.Today.Date;//new DateTime(2017, 04, 26);// 
                                                      //Trae los datos de Historico de Cobranzas del dia de cierre y obtiene correos
                List<DTO_ccHistoricoGestionCobranza> histGestionCob = this._bc.AdministrationModel.HistoricoGestionCobranza_GetGestion(this.documentID, cumple, string.Empty, null, null);

                long count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.ccCliente, null, null, true);
                List<DTO_ccCliente> clientes = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.ccCliente, count, 1, null, null, true).Cast<DTO_ccCliente>().ToList();

                //Filtra los que tengan fecha de nacimiento del dia actual o fin de semana
                if (cumple.DayOfWeek != DayOfWeek.Friday)
                    clientes = clientes.FindAll(x => x.FechaNacimiento.Value.Value.Month == cumple.Date.Month && x.FechaNacimiento.Value.Value.Day == cumple.Day);
                else
                    clientes = clientes.FindAll(x => x.FechaNacimiento.Value.Value.Month == cumple.Date.Month && (x.FechaNacimiento.Value.Value.Day >= cumple.Day && x.FechaNacimiento.Value.Value.Day <= cumple.Day + 2));

                foreach (DTO_ccCliente cli in clientes)
                {
                    #region Envia correos
                    if (histGestionCob.Exists(x => x.ClienteID.Value == cli.ID.Value))
                    {
                        #region Envia el correo
                        #region Crea Filtros 
                        List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                        filtros.Add(new DTO_glConsultaFiltro()
                        {
                            CampoFisico = "TipoCorreo",
                            OperadorFiltro = OperadorFiltro.Igual,
                            ValorFiltro = "2"//Cumpleaños
                        });
                        DTO_glConsulta consulta = new DTO_glConsulta();
                        consulta.Filtros = filtros;
                        #endregion
                        //Trae con filtros  
                        count = _bc.AdministrationModel.MasterSimple_Count(AppMasters.glCorreosEspeciales, consulta, null, true);
                        List<DTO_glCorreosEspeciales> dtos = _bc.AdministrationModel.MasterSimple_GetPaged(AppMasters.glCorreosEspeciales, count, 1, consulta, null, true).Cast<DTO_glCorreosEspeciales>().ToList();
                        DTO_ccCliente cliente = (DTO_ccCliente)this._bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, cli.ID.Value, true);
                        if (dtos.Count > 0)
                        {
                            string asunto = dtos.First().Asunto.Value;
                            string mensaje = dtos.First().PlantillaEMail.Value;
                            this._bc.SendMail(this.documentID, asunto, mensaje, cliente.Correo.Value, dtos.First().CuentaOrigen.Value.Value);
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreDiaCartera.cs", "SendEmailCumpleaños"));
                this.StopProgressBarThread();
            }

        }

        #endregion

        #region Funciones Virtuales

        /// <summary>
        /// Inicia las variables del formulario
        /// </summary>
        protected override void InitForm()
        {
            try
            {
                this.documentID = AppProcess.CierreDiaCartera;

                InitializeComponent();
                FormProvider.LoadResources(this, documentID);

                this.EndProcesarDelegate = new EndProcesar(EndProcesarMethod);

                //Periodo
                _bc.InitPeriodUC(this.dtPeriod, 0);
                DateTime periodo = DateTime.Parse(_bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_Periodo));
                this.dtPeriod.DateTime = periodo;

                //Habilita el boton y fecha de cierre
                this._cierreDiarioInd = _bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_CierreDiarioInd) != "1"? false : true;
                if (this._cierreDiarioInd)
                {
                    this.dtFechaCierre.Enabled = false;
                    //this.btnCierre.Enabled = false;
                }

                //Fecha Final
                int diaMax = DateTime.Now.Day;
                this.maxDate = DateTime.Now;
                if (periodo.Year != this.maxDate.Year || periodo.Month != this.maxDate.Month)
                {
                    diaMax = DateTime.DaysInMonth(periodo.Year, periodo.Month);
                    this.maxDate = new DateTime(periodo.Year, periodo.Month, diaMax);
                }
                this.dtFechaCierre.DateTime = maxDate;
                this.dtFechaCierre.Properties.MaxValue = maxDate;

                //Carga el control de fecha de cierre
                this.LoadFechaCierre();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreDiaCartera.cs", "InitForm"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Boton de cierre
        /// </summary>
        /// <param name="sender">Objeto que envia el evento</param>
        /// <param name="e">Evento</param>
        private void btnCierre_Click(object sender, EventArgs e)
        {
            this.ControlBox = false;
            this.btnCierre.Enabled = false;
            new Thread(CierreDiaThread).Start();
        }

        #endregion

        #region Hilos

        /// <summary>
        /// Hilo de Ajuste en Cambio
        /// </summary>
        private void CierreDiaThread()
        {
            try
            {
                this.FuncProgressBarThread = () => (_bc.AdministrationModel.ConsultarProgresoCartera(this.documentID));
                this.ProgressBarThread = new Thread(this.CheckServerProcessStatus);
                this.ProgressBarThread.Start();

                this._resProcesar = this._bc.AdministrationModel.Cartera_CerrarDia(this.dtFechaCierre.DateTime.Date);
                this._bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, _bc.AdministrationModel.Empresa.NumeroControl.Value).ToList();

                if (this._resProcesar.Result == ResultValue.OK)
                {
                    this.SendEmailCumpleaños();
                }
                this.StopProgressBarThread();

                MessageForm frm = new MessageForm(this._resProcesar);
                frm.ShowDialog();

                this.Invoke(this.EndProcesarDelegate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-CierreDiaCartera.cs", "btnCierre_Click"));
                this.StopProgressBarThread();
            }
        }

        #endregion

    }
}
