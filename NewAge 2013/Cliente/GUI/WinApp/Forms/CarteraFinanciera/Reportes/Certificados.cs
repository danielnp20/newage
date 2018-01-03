using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using NewAge.Cliente.GUI.WinApp.Clases;
using NewAge.Cliente.GUI.WinApp.Componentes.ReportParameters;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using SentenceTransformer;
using NewAge.Cliente.GUI.WinApp.ControlsUC;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class Certificados : ReportParametersForm, IFiltrable
    {
        #region Variables
        private string _report;
        //Variables del hilo
        private DateTime _fechaIni;
        private DateTime _fechaFin;
        //Filtro
        private string _libranza;
        //MasterFind
        private DTO_ccCliente _dtoCliente = null;
        private List<DTO_ccCreditoDocu> _creditosXCliente;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Certificados() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.co;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.ccCertificados).ToString());
            try
            {

                #region Configurar Opciones
                //Se establece la lista del combo con sus respectivos valores
                List<ReportParameterListItem> ccCertificadosType = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = AppTemplates.cc_CertificadoDeuda, Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CertificadoDeuda) }, 
                    new ReportParameterListItem() { Key = AppTemplates.cc_PazYSalvo, Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PazYSalvo) }
                };
                //Determina el nombre del combo y el item donde debe quedar
                this.AddList("1", (AppForms.ReportForm).ToString() + "_Certificado", ccCertificadosType, true, "1");
                this.AddMaster("2", AppMasters.ccCliente, true, null);
                #endregion
                #region Configurar Filtros);
                this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                this.periodoFilter1.monthCB.SelectedIndex = 0;

                this.btnFilter.Enabled = true;
                this.btnResetFilter.Enabled = true;
                this.btnFilter.Visible = true;
                this.btnResetFilter.Visible = true;
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentosYCertificados.cs", "InitReport"));
            }

        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                if (this._libranza == null)
                {
                    if (this._creditosXCliente.Count != 0)
                    {
                        this._libranza = this._creditosXCliente[0].Libranza.Value.Value.ToString();
                    }
                }
                if (this._libranza == null)
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_NoCredit);
                    MessageBox.Show(msg);
                    return;
                }

                if (this._report == AppTemplates.cc_PazYSalvo)
                {
                    DTO_ccCreditoDocu creditoInfo = new DTO_ccCreditoDocu();
                    creditoInfo = this._bc.AdministrationModel.GetCreditoByLibranza(Convert.ToInt32(this._libranza));
                    if (!creditoInfo.CanceladoInd.Value.Value)
                    {
                        string msg = this._bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_NoPayedCredit);
                        MessageBox.Show(msg);
                    }
                    else
                    {
                        Templates tem = new Templates();
                        List<string> list = new List<string>();
                        DTO_ccPagaduria pagaduria = (DTO_ccPagaduria)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccPagaduria, false, creditoInfo.PagaduriaID.Value, true);

                        list.Add(this._bc.AdministrationModel.Empresa.EmpresaGrupoID_.Value);
                        list.Add(this._dtoCliente.ID.Value);
                        list.Add(this._dtoCliente.Descriptivo.Value);
                        list.Add(pagaduria.Descriptivo.Value);
                        list.Add(creditoInfo.Libranza.Value.Value.ToString());
                        list.Add(DateTime.Now.Day.ToString());
                        list.Add(CurrencyFormater.GetStringVal_ESP(DateTime.Now.Day));
                        list.Add(String.Format(Convert.ToString(DateTime.Now.ToString("MMMM"))));
                        list.Add(DateTime.Now.Year.ToString());
                        list.Add("Bogotá D.C.");
                        tem.GenerarPlantilla(this._report, list);
                    }
                }
                if (this._report == AppTemplates.cc_CertificadoDeuda || this._report == null)
                {
                    Templates tem = new Templates();
                    List<string> list = new List<string>();
                     
                    DTO_ccCertificadoDeuda deudaCert = this._bc.AdministrationModel.Report_Cc_CertificadoDeuda(this._fechaIni, Convert.ToInt32(this._libranza));
                    if (!string.IsNullOrWhiteSpace(deudaCert.mensaje))
                    {
                        string msg = deudaCert.mensaje;
                        MessageBox.Show(msg);
                    }
                    else
                    {
                        list.Add(deudaCert.Descriptivo.Value);
                        list.Add(deudaCert.ClienteId.Value);
                        list.Add(deudaCert.NroPagare.Value.Value.ToString());
                        list.Add(deudaCert.Pagadria.Value);
                        list.Add(deudaCert.EC_SaldoPend.Value.Value.ToString());
                        list.Add(CurrencyFormater.GetCurrencyString("ES1", "COP", deudaCert.EC_SaldoPend.Value.Value));
                        list.Add(Convert.ToString(deudaCert.EC_FechaLimite.Value.Value.ToString("dd-MMMM-yyyy")));
                        list.Add(String.Format(Convert.ToString(this._fechaIni.ToString("MMMM"))));
                        list.Add(deudaCert.Correo.Value);

                        list.Add(String.Format(Convert.ToString(deudaCert.FechaLiquida.Value.Value.ToString("dd-MMMM-yyyy"))));
                        list.Add(deudaCert.VlrLibranza.Value.Value.ToString());
                        list.Add(deudaCert.Plazo.Value.Value.ToString());
                        list.Add(deudaCert.VlrCuota.Value.Value.ToString());
                        if (deudaCert.isPazYSalvo.Value == false)
                            list.Add(this._bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_BehindCredit));
                        else
                            list.Add(this._bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_Updated));
                        list.Add(DateTime.Now.Day.ToString());
                        list.Add(String.Format(Convert.ToString(DateTime.Now.ToString("MMMM"))));
                        list.Add(DateTime.Now.Year.ToString());
                        list.Add(DateTime.Now.Year.ToString());
                        list.Add("Bogotá D.C.");
                        list.Add(this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_SupervisorCertificados));
                        tem.GenerarPlantilla(this._report, list);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-DocumentosYCertificados.cs", "Report"));
            }
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Changes in the form depending on user's operations
        /// </summary>
        protected override void ListValueChanged(string option, object sender)
        {
            Dictionary<string, string[]> reportParameters = this.GetValues();
            ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];
            #region Tipo Reporte
            if (option.Equals("1"))
            {
                this._report = listReportType.SelectedListItem.Key;
                if (listReportType.SelectedListItem.Key == AppTemplates.cc_CertificadoDeuda)
                {
                    this._report = AppTemplates.cc_CertificadoDeuda;
                    #region Configurar Filtros);
                    this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                    this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                    this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                    this.periodoFilter1.monthCB.SelectedIndex = 0;

                    this.btnFilter.Enabled = true;
                    this.btnResetFilter.Enabled = true;
                    this.btnFilter.Visible = true;
                    this.btnResetFilter.Visible = true;
                    #endregion
                }

                if (listReportType.SelectedListItem.Key == AppTemplates.cc_PazYSalvo)
                {
                    this._report = AppTemplates.cc_PazYSalvo;

                    #region Configurar Filtros

                    this.periodoFilter1.Visible = false;
                    this.btnFilter.Enabled = true;
                    this.btnResetFilter.Enabled = true;

                    #endregion
                }
            }
            #endregion
            #region MasterFind
            if (option.Equals("2"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                this._dtoCliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, master.Value, true);
                List<ReportParameterListItem> items = new List<ReportParameterListItem>();
                if (!string.IsNullOrWhiteSpace(this._dtoCliente.ID.Value))
                {
                    List<ReportParameterListItem> ccCreditosxClienteType = new List<ReportParameterListItem>();
                    this._creditosXCliente = this._bc.AdministrationModel.GetCreditoByCliente(this._dtoCliente.ID.Value);
                    if (this._creditosXCliente.Count != 0)
                    {
                        foreach (DTO_ccCreditoDocu credito in this._creditosXCliente)
                        {
                            ReportParameterListItem itemCredito = new ReportParameterListItem() { Key = credito.Libranza.Value.Value.ToString(), Desc = credito.Libranza.Value.Value.ToString() };
                            items.Add(itemCredito);
                        }
                        ccCreditosxClienteType.AddRange(items);
                        //Determina el nombre del combo y el item donde debe quedar
                        this.AddList("3", (AppForms.ReportForm).ToString() + "_CreditosCliente", ccCreditosxClienteType, true, "1");
                    }
                }
                else
                {
                    string msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_NoCredit);
                    MessageBox.Show(msg);
                }
            }
            #endregion
            #region Combo Libranza
            if (option.Equals("3"))
            {
                listReportType = (ReportParameterList)this.RPControls["3"];
                this._libranza = listReportType.SelectedListItem.Key;
            }
            #endregion
        }

        #endregion

        #region Filtros

        /// <summary>
        /// Funcion que captura el valor del filtro seleccionado
        /// </summary>
        /// <param name="consulta">Dto con la info de la consulta</param>
        /// <param name="fields">nombre de las columnas o campos</param>
        void IFiltrable.SetConsulta(DTO_glConsulta consulta, List<SentenceTransformer.ConsultasFields> fields)
        {
            //foreach (var fil in consulta.Filtros)
            //    this._filtro = fil.ValorFiltro;
        }
        #endregion
    }
}
