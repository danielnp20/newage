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
using System.Linq;

namespace NewAge.Cliente.GUI.WinApp.Forms
{
    public class Reports_Certificados : ReportParametersForm
    {
        #region Variables
        //Variables del hilo
        private DateTime _fechaIniCorte;
        //Filtro
        private string _libranza;
        //MasterFind
        private string _clienteID = null;
        private List<DTO_ccCreditoDocu> _creditosXCliente;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Certificados() { }

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
                    new ReportParameterListItem() { Key = AppReports.ccCertificadoDeuda.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CertificadoDeuda) }, 
                    new ReportParameterListItem() { Key = AppReports.ccCertificadoPazYSalvo.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_PazYSalvo) },
                    new ReportParameterListItem() { Key = AppReports.ccCertificadoPagosAlDia.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CertificadoPagosAlDia) },
                    new ReportParameterListItem() { Key = AppReports.ccCertificadoRelacionPagos.ToString(), Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CertificadoRelacionPagos) },
                    new ReportParameterListItem() { Key = AppReports.ccCertificadoRelacionPagosXCuota.ToString(), Desc = "Certificado Relación Pagos por Cuota" }
                };
                //Determina el nombre del combo y el item donde debe quedar
                this.AddList("1", (AppForms.ReportForm).ToString() + "_Certificado", ccCertificadosType, true, "1");
                this.AddMaster("2", AppMasters.ccCliente, true, null);
                this.AddList("3", (AppForms.ReportForm).ToString() + "_CreditosCliente", null, false, "-",false);
                this.documentReportID = AppReports.ccCertificadoDeuda;

                #endregion
                #region Configurar Filtros);
                this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonthDay;
                this.periodoFilter1.txtYear.Text = DateTime.Now.Year.ToString();
                this.periodoFilter1.monthCB.Items.Add(DateTime.Now.Month);
                this.periodoFilter1.monthCB.SelectedIndex = 0;
                this.periodoFilter1.monthCB1.SelectedItem = DateTime.Now.Day;

                this.btnFilter.Enabled = true;
                this.btnResetFilter.Enabled = true;
                this.btnFilter.Visible = false;
                this.btnResetFilter.Visible = false;
                //this.btnExportToPDF.Text = "Generar Word";
                //this.btnExportToPDF.Image = global::NewAge.Properties.Resources.Word;
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-reports_Certificados.cs", "InitReport"));
            }

        }

        /// <summary>
        /// Collects data for the report from the form
        /// </summary>
        protected override void Report_PDF()
        {
            try
            {
                uc_MasterFind cliente = (uc_MasterFind)this.RPControls["2"];
                if (cliente.ValidID)
                {
                    this.periodoFilter1.Enabled = false;
                    this.btnExportToPDF.Enabled = false;
                    this.periodoFilter1.txtYear1.Visible = false;

                    //Fecha de Corte
                    this.periodoFilter1.Year[0].ToString();
                    string fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0] + " / " + this.periodoFilter1.monthCB1.SelectedItem;
                    this._fechaIniCorte = Convert.ToDateTime(fechaIniString);
                    #region Asigna datos 
                    Dictionary<int, string> list = new Dictionary<int, string>();
                    int consecutivo = 0;
                     DTO_ccCreditoDocu credidotSelect =null;
                    string terceroEmpresa = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    string responsFirma = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ResponsableFirmaCertif);
                    DTO_coTercero dtoFirma = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, responsFirma, true);
                    DTO_ccCliente dtoCliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, cliente.Value, true);
                    DTO_glLugarGeografico dtoLugarGeo = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false,dtoCliente.ResidenciaCiudad.Value, true);

                    //Si no ha seleccionado libranza elige la primera por defecto
                    if (string.IsNullOrEmpty(this._libranza))
                    {
                        if (this._creditosXCliente != null && this._creditosXCliente.Count != 0)
                            this._libranza = this._creditosXCliente[0].Libranza.Value.ToString();
                    }
                    //Si no hay libranza no permite abrir reporte
                    if (string.IsNullOrEmpty(this._libranza))
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_noFilterSelected);
                        MessageBox.Show(msg);
                        return;
                    }
                    else
                        credidotSelect =  _creditosXCliente.Find(x => x.Libranza.Value == Convert.ToInt32(this._libranza));//this._bc.AdministrationModel.GetCreditoByLibranza(Convert.ToInt32(this._libranza));
                    #endregion
                    #region Asigna Codeudores
                    string codeudorCedulas = string.Empty;
                    string codeudorNombres = string.Empty;
                    string codeudorAll = string.Empty;
                    DTO_coTercero codeudor = null;
                    if (!string.IsNullOrEmpty(credidotSelect.Codeudor1.Value) && credidotSelect.Codeudor1.Value != "0")
                    {
                        codeudor = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, credidotSelect.Codeudor1.Value, true);
                        if(codeudor != null)
                        {
                            codeudorNombres = codeudor.Descriptivo.Value;
                            codeudorCedulas = credidotSelect.Codeudor1.Value;
                            codeudorAll = codeudor.Descriptivo.Value + " identificado(a) con cédula No. " + credidotSelect.Codeudor1.Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(credidotSelect.Codeudor2.Value) && credidotSelect.Codeudor2.Value != "0")
                    {
                        codeudor = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, credidotSelect.Codeudor2.Value, true);
                        if (codeudor != null)
                        {
                            codeudorCedulas += "," + credidotSelect.Codeudor2.Value;
                            codeudorNombres += "," + codeudor.Descriptivo.Value;
                            codeudorAll += ", " + codeudor.Descriptivo.Value + " identificado(a) con cédula No. " + credidotSelect.Codeudor2.Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(credidotSelect.Codeudor3.Value) && credidotSelect.Codeudor3.Value != "0")
                    {
                        codeudor = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, credidotSelect.Codeudor3.Value, true);
                        if (codeudor != null)
                        {
                            codeudorCedulas += "," + credidotSelect.Codeudor3.Value;
                            codeudorNombres += "," + codeudor.Descriptivo.Value;
                            codeudorAll += ", " + codeudor.Descriptivo.Value + " identificado(a) con cédula No. " + credidotSelect.Codeudor3.Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(credidotSelect.Codeudor4.Value) && credidotSelect.Codeudor4.Value != "0")
                    {
                        codeudor = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, credidotSelect.Codeudor4.Value, true);
                        if (codeudor != null)
                        {
                            codeudorCedulas += "," + credidotSelect.Codeudor4.Value;
                            codeudorNombres += codeudor != null ? "," + codeudor.Descriptivo.Value : string.Empty;
                            codeudorAll += ", " + codeudor.Descriptivo.Value + " identificado(a) con cédula No. " + credidotSelect.Codeudor4.Value;
                        }
                    }

                    #endregion                    

                    //Datos generales
                    list.Add(0, this._bc.AdministrationModel.Empresa.Descriptivo.Value);        //%0%  Nombre Empresa Actual  
                    list.Add(1, terceroEmpresa);                                                //%1%  Nit Empresa     
                    list.Add(2, DateTime.Now.Day.ToString() + " de " + DateTime.Now.ToString("MMMM") + " de " + DateTime.Now.Year.ToString());      //%2%  Fecha Actual   
                    list.Add(3, this._fechaIniCorte.Day.ToString() + " de " + this._fechaIniCorte.ToString("MMMM") + " de " + this._fechaIniCorte.Year.ToString());//%3%  Fecha Corte 
                    list.Add(4, dtoCliente.Descriptivo.Value);                                  //%4%  Nombre Cliente
                    list.Add(5, dtoCliente.ID.Value);                                           //%5%  Cedula Cliente    
                    list.Add(6, dtoLugarGeo != null?dtoLugarGeo.Descriptivo.Value:string.Empty);//%6%  Ciudad Cliente    
                    list.Add(7, dtoCliente.ResidenciaDir.Value);                                //%7%  Direccion Cliente                                                
                    list.Add(8, this._libranza.ToString());                                     //%8%  Credito/Libranza                         
                    list.Add(9, codeudorCedulas);                                               //%9%  Codeudores Cedulas
                    list.Add(10, codeudorNombres);                                              //%10%  Codeudores Nombres
                    list.Add(11, codeudorAll);                                                  //%11%  Codeudores Todos
                    list.Add(12, DateTime.Now.Day.ToString());                                  //%12%  Dia Actual(numero)
                    list.Add(13, DateTime.Now.ToString("MMMM"));                                //%13%  Mes Actual(Letras)
                    list.Add(14, DateTime.Now.Year.ToString());                                 //%14%  Año Actual (numero)   
                    list.Add(99, dtoFirma != null?dtoFirma.Descriptivo.Value:responsFirma);     //%99%  Nombre o cedula de la firma responsable                                //%11}

                    //Datos particulares
                    if (this.documentReportID == AppReports.ccCertificadoPazYSalvo)
                    {
                        #region Paz y Salvo
                        if (!credidotSelect.CanceladoInd.Value.Value)
                        {
                            string msg = this._bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_NoPayedCredit);
                            MessageBox.Show(msg);
                            return;
                        }
                        else
                        {
                            string consecPazySal = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConsecutivoPazySalvo);
                            consecutivo = !string.IsNullOrEmpty(consecPazySal) ? Convert.ToInt32(consecPazySal) : 0;   
                            list.Add(15, consecutivo.ToString());                            //%15%  Consecutivo Deuda                         
                        }
                        #endregion
                    }
                    else if (this.documentReportID == AppReports.ccCertificadoDeuda)
                    {
                        #region Certificado Deuda   
                        string consecCertDeuda = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ConsecutivoCertifDeuda);
                        consecutivo = !string.IsNullOrEmpty(consecCertDeuda) ? Convert.ToInt32(consecCertDeuda) : 0;

                        if (credidotSelect.DocEstadoCuenta.Value != null)
                        {
                            DTO_ccEstadoCuentaHistoria estadoCta = this._bc.AdministrationModel.EstadoCuenta_GetHistoria(credidotSelect.DocEstadoCuenta.Value.Value);
                            decimal saldoCred = estadoCta != null ? estadoCta.EC_ValorPago.Value.Value : 0;
                            if (saldoCred > 0)
                            {
                                list[3] =  estadoCta.EC_Fecha.Value.Value.Day.ToString() + " de " + estadoCta.EC_Fecha.Value.Value.ToString("MMMM") + " de " + estadoCta.EC_Fecha.Value.Value.Year.ToString();//%3%  Fecha Corte 
                                list.Add(15, credidotSelect.Plazo.Value.Value.ToString());                      //%15%    Plazo(numero)
                                list.Add(16, credidotSelect.VlrCuota.Value.Value.ToString("n0"));               //%16%    Vlr Cuota(numero)
                                list.Add(17, estadoCta.EC_ValorPago.Value.Value.ToString("n0"));                //%17%    Saldo Pendiente(numero)
                                list.Add(18, CurrencyFormater.GetCurrencyString("ES1", "COP", saldoCred));      //%18}    Saldo Pendiente(letras)         
                                list.Add(19, consecutivo.ToString());                                           //%19%    Consecutivo Deuda  
                            }
                            else
                            {
                                MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "El cliente no tiene deuda pendiente"));
                                return;
                            } 
                        }
                        else
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "El cliente no tiene estado de cuenta"));
                            return;
                        }
                        #endregion
                    }
                    else if (this.documentReportID == AppReports.ccCertificadoPagosAlDia)
                    {
                        #region Certificado Pagos al dia
                        DTO_InfoCredito saldoCred = this._bc.AdministrationModel.GetSaldoCredito(credidotSelect.NumeroDoc.Value.Value, this._fechaIniCorte, true, false, true);
                        saldoCred.PlanPagos = saldoCred.PlanPagos.OrderBy(x => x.FechaCuota.Value).ToList();
                        if (saldoCred.PlanPagos.Count == 0)
                        { 
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "El cliente no tiene pagos pendientes, si desea consulte el certificado de Paz y Salvo"));
                            return;
                        }
                        if (saldoCred.PlanPagos.First().FechaCuota.Value <= this._fechaIniCorte)//Valida si esta atrasado
                        {
                            MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, "El cliente no está al día en sus pagos"));
                            return;
                        }
                        else 
                        {
                            //saldoCred.PlanPagos = saldoCred.PlanPagos.FindAll(x=>x.FechaCuota.Value >this._fechaIniCorte).OrderBy(x => x.FechaCuota.Value).ToList();
                            DateTime proximoPago = saldoCred.PlanPagos.Count > 0 ? saldoCred.PlanPagos.First().FechaCuota.Value.Value : this._fechaIniCorte;
                            list.Add(15, proximoPago.Day.ToString() + " de " + proximoPago.ToString("MMMM") + " de " + proximoPago.Year.ToString());  //%15%  Fecha proximo pago

                        }                       

                        #endregion
                    }
                    else if (this.documentReportID == AppReports.ccCertificadoRelacionPagos)
                    {
                        #region Certificado Relacion de pagos 
                        list.Add(15, credidotSelect.FechaLiquida.Value.Value.Day.ToString() + " de " + credidotSelect.FechaLiquida.Value.Value.ToString("MMMM") +
                            " de " + credidotSelect.FechaLiquida.Value.Value.Year.ToString());      //%15%  Fecha Inicial del credito
                       
                        string reportPagos = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(AppReports.ccRepAnalisisPagos, 2, this._fechaIniCorte, this._fechaIniCorte, cliente.Value, Convert.ToInt32(this._libranza), string.Empty, string.Empty,
                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null, null);
                        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportPagos);
                        Process.Start(fileURl);

                        #endregion
                    }
                    else if (this.documentReportID == AppReports.ccCertificadoRelacionPagosXCuota)
                    {
                        #region Certificado Relacion de pagos 
                        list.Add(15, credidotSelect.FechaLiquida.Value.Value.Day.ToString() + " de " + credidotSelect.FechaLiquida.Value.Value.ToString("MMMM") +
                            " de " + credidotSelect.FechaLiquida.Value.Value.Year.ToString());      //%15%  Fecha Inicial del credito

                        string reportPagos = this._bc.AdministrationModel.Report_Cc_CarteraByParameter(AppReports.ccRepAnalisisPagos, 2, this._fechaIniCorte, this._fechaIniCorte, cliente.Value, Convert.ToInt32(this._libranza), string.Empty, string.Empty,
                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, null, true);
                        string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportPagos);
                        Process.Start(fileURl);

                        #endregion
                    }

                    if (list.Count > 0)
                    {
                        #region Genera el Reporte y actualiza control
                        string reportName = this._bc.AdministrationModel.Report_Cc_Certificados(this.documentReportID, list);
                        if (!string.IsNullOrEmpty(reportName))
                        {
                            // Actualiza el  glControl
                            string empNro = this._bc.AdministrationModel.Empresa.NumeroControl.Value;
                            string _modId = ((int)ModulesPrefix.cc).ToString();
                            if (_modId.Length == 1) _modId = "0" + _modId;
                            DTO_glControl ctrlConsec = new DTO_glControl();

                            if (this.documentReportID == AppReports.ccCertificadoPazYSalvo)
                            {
                                ctrlConsec.glControlID.Value = Convert.ToInt32((empNro + _modId + AppControl.cc_ConsecutivoPazySalvo));
                                ctrlConsec.Descriptivo.Value = "Consecutivo de Paz y salvo";
                            }
                            else if (this.documentReportID == AppReports.ccCertificadoDeuda)
                            {
                                ctrlConsec.glControlID.Value = Convert.ToInt32((empNro + _modId + AppControl.cc_ConsecutivoCertifDeuda));
                                ctrlConsec.Descriptivo.Value = "Consecutivo Certificado de Deuda";
                            }
                            ctrlConsec.Data.Value = (consecutivo + 1).ToString();
                            this._bc.AdministrationModel.glControl_Update(ctrlConsec);
                            this._bc.AdministrationModel.ControlList = _bc.AdministrationModel.glControl_GetByNumeroEmpresa(false, empNro).ToList();

                            string fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName);
                            Process.Start(fileURl);
                        }  
                        #endregion
                    }
                }
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_ClientObligated));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-reports_Certificados.cs", "Report"));
            }
            finally
            {
                this.periodoFilter1.Enabled = true;
                this.btnExportToPDF.Enabled = true;
                this.btnExportToXLS.Enabled = true;
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
                if (listReportType.SelectedListItem.Key == AppReports.ccCertificadoDeuda.ToString())
                {
                    this.documentReportID = AppReports.ccCertificadoDeuda;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.ccCertificadoPazYSalvo.ToString())
                {
                    this.documentReportID = AppReports.ccCertificadoPazYSalvo;                    
                }
                else if (listReportType.SelectedListItem.Key == AppReports.ccCertificadoPagosAlDia.ToString())
                {
                    this.documentReportID = AppReports.ccCertificadoPagosAlDia;                    
                }
                else if (listReportType.SelectedListItem.Key == AppReports.ccCertificadoRelacionPagos.ToString())
                {
                    this.documentReportID = AppReports.ccCertificadoRelacionPagos;                    
                }
                else if (listReportType.SelectedListItem.Key == AppReports.ccCertificadoRelacionPagosXCuota.ToString())
                {
                    this.documentReportID = AppReports.ccCertificadoRelacionPagosXCuota;
                }
            }
            #endregion
            #region Cliente
            if (option.Equals("2"))
            {
                uc_MasterFind master = (uc_MasterFind)sender;
                List<ReportParameterListItem> items = new List<ReportParameterListItem>();
                if (master.ValidID)
                {
                    this._clienteID  = master.Value;
                    List<ReportParameterListItem> ccCreditosxClienteType = new List<ReportParameterListItem>();
                    this._creditosXCliente = this._bc.AdministrationModel.GetCreditoByCliente(master.Value);
                    if (this._creditosXCliente.Count > 0)
                    {
                        foreach (DTO_ccCreditoDocu credito in this._creditosXCliente)
                        {
                            ReportParameterListItem itemCredito = new ReportParameterListItem() { Key = credito.Libranza.Value.Value.ToString(), Desc = credito.Libranza.Value.Value.ToString() };
                            if(!items.Exists(x=>x.Key ==itemCredito.Key))
                                items.Add(itemCredito);
                        }
                        ccCreditosxClienteType.AddRange(items);

                        ReportParameterList libranza = (ReportParameterList)this.RPControls["3"];
                        libranza.SetItems((AppForms.ReportForm).ToString() + "_CreditosCliente", ccCreditosxClienteType);
                        libranza.RefreshList();
                        libranza.Visible = true;
                    }
                    else
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, MasterMessages.Cc_Cliente_NoCredit);
                        MessageBox.Show(msg);
                    }
                }
                else
                {
                    ReportParameterList libranza = (ReportParameterList)this.RPControls["3"];
                    libranza.Visible = false;
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
    }
}
