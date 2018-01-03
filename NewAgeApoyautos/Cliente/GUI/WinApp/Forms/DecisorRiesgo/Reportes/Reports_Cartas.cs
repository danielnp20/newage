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
    public class Reports_Cartas : ReportParametersForm
    {
        #region Variables
        //Variables del hilo
        private DateTime _fechaIniCorte;
        //Filtro
        private string _libranza;
        //MasterFind
        private string _clienteID = null;
        private List<DTO_ccSolicitudDocu> _solicitudesXCliente;
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public Reports_Cartas() { }

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
                    new ReportParameterListItem() { Key = AppReports.drCartaEnvioPrendas.ToString(), Desc = "CARTA ENVIO PRENDAS" },
                    new ReportParameterListItem() { Key = AppReports.drRevocacionAprobacion.ToString(), Desc = "REVOCACION APROBACION" },
                    new ReportParameterListItem() { Key = AppReports.drTrasladoCuenta.ToString(), Desc = "TRASLADO DE CUENTA" },
                    new ReportParameterListItem() { Key = AppReports.drVencimientoTerminos.ToString(), Desc = "VENCIMIENTO DE TERMINOS" },
                    new ReportParameterListItem() { Key = AppReports.drAprobacionDirectaSinDoc.ToString(), Desc = "APROBACION DIRECTA NO REQUIERE DOCUMENTOS" },
                    new ReportParameterListItem() { Key = AppReports.drAprobacionDirectaConDoc.ToString(), Desc = "APROBACION DIRECTA SUJETA A DOCUMENTOS" },
                    new ReportParameterListItem() { Key = AppReports.drNoViable.ToString(), Desc = "NO VIABLE" },
                    new ReportParameterListItem() { Key = AppReports.drPreAprobacion.ToString(), Desc = "PREAPROBACION" },
                    new ReportParameterListItem() { Key = AppReports.drRatificacion.ToString(), Desc = "RATIFICACION" }
                };
                //Determina el nombre del combo y el item donde debe quedar
                this.AddList("1", (AppForms.ReportForm).ToString() + "_Certificado", ccCertificadosType, true, "1");
                this.AddMaster("2", AppMasters.ccCliente, true, null);
                this.AddList("3", (AppForms.ReportForm).ToString() + "_SolicitudCliente", null, false, "-",false);
                this.documentReportID = AppReports.drCartaEnvioPrendas;

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
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Cartas.cs", "InitReport"));
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
                    Dictionary<string, string> list = new Dictionary<string, string>();
                    int consecutivo = 0;
                     DTO_ccSolicitudDocu solicitudSelect =null;
                    string terceroEmpresa = this._bc.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                    string responsFirma = this._bc.GetControlValueByCompany(ModulesPrefix.cc, AppControl.cc_ResponsableFirmaCertif);
                    DTO_coTercero dtoFirma = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, responsFirma, true);
                    DTO_ccCliente dtoCliente = (DTO_ccCliente)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.ccCliente, false, cliente.Value, true);
                    DTO_glLugarGeografico dtoLugarGeo = (DTO_glLugarGeografico)_bc.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, false,dtoCliente.ResidenciaCiudad.Value, true);

                    //Si no ha seleccionado libranza elige la primera por defecto
                    if (string.IsNullOrEmpty(this._libranza))
                    {
                        if (this._solicitudesXCliente != null && this._solicitudesXCliente.Count != 0)
                            this._libranza = this._solicitudesXCliente[0].Libranza.Value.ToString();
                    }
                    //Si no hay libranza no permite abrir reporte
                    if (string.IsNullOrEmpty(this._libranza))
                    {
                        string msg = _bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Ac_noFilterSelected);
                        MessageBox.Show(msg);
                        return;
                    }
                    else
                        solicitudSelect =  _solicitudesXCliente.Find(x => x.Libranza.Value == Convert.ToInt32(this._libranza));//this._bc.AdministrationModel.GetCreditoByLibranza(Convert.ToInt32(this._libranza));
                    #endregion
                    #region Asigna Codeudores
                    string codeudorCedulas = string.Empty;
                    string codeudorNombres = string.Empty;
                    string codeudorAll = string.Empty;
                    DTO_coTercero codeudor = null;
                    if (!string.IsNullOrEmpty(solicitudSelect.Codeudor1.Value) && solicitudSelect.Codeudor1.Value != "0")
                    {
                        codeudor = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, solicitudSelect.Codeudor1.Value, true);
                        if(codeudor != null)
                        {
                            codeudorNombres = codeudor.Descriptivo.Value;
                            codeudorCedulas = solicitudSelect.Codeudor1.Value;
                            codeudorAll = codeudor.Descriptivo.Value + " identificado(a) con cédula No. " + solicitudSelect.Codeudor1.Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(solicitudSelect.Codeudor2.Value) && solicitudSelect.Codeudor2.Value != "0")
                    {
                        codeudor = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, solicitudSelect.Codeudor2.Value, true);
                        if (codeudor != null)
                        {
                            codeudorCedulas += "," + solicitudSelect.Codeudor2.Value;
                            codeudorNombres += "," + codeudor.Descriptivo.Value;
                            codeudorAll += ", " + codeudor.Descriptivo.Value + " identificado(a) con cédula No. " + solicitudSelect.Codeudor2.Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(solicitudSelect.Codeudor3.Value) && solicitudSelect.Codeudor3.Value != "0")
                    {
                        codeudor = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, solicitudSelect.Codeudor3.Value, true);
                        if (codeudor != null)
                        {
                            codeudorCedulas += "," + solicitudSelect.Codeudor3.Value;
                            codeudorNombres += "," + codeudor.Descriptivo.Value;
                            codeudorAll += ", " + codeudor.Descriptivo.Value + " identificado(a) con cédula No. " + solicitudSelect.Codeudor3.Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(solicitudSelect.Codeudor4.Value) && solicitudSelect.Codeudor4.Value != "0")
                    {
                        codeudor = (DTO_coTercero)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, false, solicitudSelect.Codeudor4.Value, true);
                        if (codeudor != null)
                        {
                            codeudorCedulas += "," + solicitudSelect.Codeudor4.Value;
                            codeudorNombres += codeudor != null ? "," + codeudor.Descriptivo.Value : string.Empty;
                            codeudorAll += ", " + codeudor.Descriptivo.Value + " identificado(a) con cédula No. " + solicitudSelect.Codeudor4.Value;
                        }
                    }

                    #endregion                    

                    //Datos generales   
                    list.Add("%FECHACORTE%", this._fechaIniCorte.ToString("dd") + " de " + this._fechaIniCorte.ToString("MMMM") + " de " + this._fechaIniCorte.Year.ToString());//%3%  Fecha Corte 
                    if (this.documentReportID == AppReports.drCartaEnvioPrendas)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data,list);
                    }
                    else if (this.documentReportID == AppReports.drRevocacionAprobacion)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
                    }
                    else if (this.documentReportID == AppReports.drTrasladoCuenta)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
                    }
                    else if (this.documentReportID == AppReports.drVencimientoTerminos)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
                    }
                    else if (this.documentReportID == AppReports.drAprobacionDirectaSinDoc)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
                    }
                    else if (this.documentReportID == AppReports.drAprobacionDirectaConDoc)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
                    }
                    else if (this.documentReportID == AppReports.drNoViable)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
                    }
                    else if (this.documentReportID == AppReports.drPreAprobacion)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
                    }
                    else if (this.documentReportID == AppReports.drRatificacion)
                    {
                        DTO_DigitaSolicitudDecisor data = _bc.AdministrationModel.DigitacionSolicitud_GetBySolicitud(Convert.ToInt32(this._libranza));
                        this._bc.AdministrationModel.Report_Cc_CartaCustom(this.documentReportID, data, list);
                    }
                }
                else
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_ClientObligated));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_Cartas.cs", "Report"));
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
                if (listReportType.SelectedListItem.Key == AppReports.drCartaEnvioPrendas.ToString())
                {
                    this.documentReportID = AppReports.drCartaEnvioPrendas;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.drRevocacionAprobacion.ToString())
                {
                    this.documentReportID = AppReports.drRevocacionAprobacion;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.drTrasladoCuenta.ToString())
                {
                    this.documentReportID = AppReports.drTrasladoCuenta;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.drVencimientoTerminos.ToString())
                {
                    this.documentReportID = AppReports.drVencimientoTerminos;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.drAprobacionDirectaSinDoc.ToString())
                {
                    this.documentReportID = AppReports.drAprobacionDirectaSinDoc;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.drAprobacionDirectaConDoc.ToString())
                {
                    this.documentReportID = AppReports.drAprobacionDirectaConDoc;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.drNoViable.ToString())
                {
                    this.documentReportID = AppReports.drNoViable;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.drPreAprobacion.ToString())
                {
                    this.documentReportID = AppReports.drPreAprobacion;
                }
                else if (listReportType.SelectedListItem.Key == AppReports.drRatificacion.ToString())
                {
                    this.documentReportID = AppReports.drRatificacion;
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
                    List<ReportParameterListItem> ccSolicitudxClienteType = new List<ReportParameterListItem>();
                    this._solicitudesXCliente = this._bc.AdministrationModel.GetSolicitudesByCliente(master.Value);
                    if (this._solicitudesXCliente.Count > 0)
                    {
                        foreach (DTO_ccSolicitudDocu sol in this._solicitudesXCliente)
                        {
                            ReportParameterListItem itemSol = new ReportParameterListItem() { Key = sol.Libranza.Value.Value.ToString(), Desc = sol.Libranza.Value.Value.ToString() };
                            if(!items.Exists(x=>x.Key ==itemSol.Key))
                                items.Add(itemSol);
                        }
                        ccSolicitudxClienteType.AddRange(items);

                        ReportParameterList libranza = (ReportParameterList)this.RPControls["3"];
                        libranza.SetItems((AppForms.ReportForm).ToString() + "_SolicitudCliente", ccSolicitudxClienteType);
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
