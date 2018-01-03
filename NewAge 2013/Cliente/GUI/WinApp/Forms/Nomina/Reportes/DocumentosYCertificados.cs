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
    public class DocumentosYCertificados : ReportParametersForm, IFiltrable
    {
        #region Hilos

        protected override void LoadReportMethod_PDF()
        {
            try
            {
                string reportName = string.Empty;
                string fileURl;

                if (this._report == "6")
                {
                    string empleadoID = this._dtoEmpleado != null ? this._dtoEmpleado.ID.Value : string.Empty;  
                    reportName = this._bc.AdministrationModel.Report_No_BoletaPago(empleadoID, this._mes, this._año, this._documentoNomina,this._quincena, ExportFormatType.pdf,null);                    
                }

                if (!string.IsNullOrEmpty(reportName))
                {
                    fileURl = this._bc.UrlDocumentFile(TipoArchivo.Temp, null, null, reportName.ToString());
                    Process.Start(fileURl);
                }
                else
                {
                    MessageBox.Show(this._bc.GetResource(LanguageTypes.Messages, DictionaryMessages.Rpt_gl_ReporteNotDataFound));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_DetalleNomina.cs", "LoadReportMethod"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
            }
        }

        #endregion

        #region Variables
        private string _report;
        //Variables del hilo
        private DateTime _fechaIni;
        private DateTime _fechaFin;
        // Reporte Boleta de Pago parametros
        private int _mes = 0;
        private int _año = 0;
        private string _documentoNomina = "81";
        private string _quincena = "1";        

        //Filtro
        private string _filtro;
        //MasterFind
        private DTO_noEmpleado _dtoEmpleado = null;
        //Filtros Cheks
        private bool _carteraPropia = false;
        private bool _cedida = false;
        private bool _toda = false;
        private bool _checkAll = false;

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Form Constructer (for Saldos Report)
        /// </summary>
        public DocumentosYCertificados() { }

        /// <summary>
        /// Funcion para configurar filtros y opciones iniciales del reporte
        /// </summary>
        protected override void InitReport()
        {
            this.Module = ModulesPrefix.no;
            this.documentReportID = AppReports.noDocumentosYCertificados;
            this.Text = _bc.GetResource(LanguageTypes.Forms, (AppReports.noDocumentosYCertificados).ToString());
            try
            {

                #region Configurar Filtros
                //Se establece la lista del combo con sus respectivos valores
                List<ReportParameterListItem> noFondoCajaCompensacionReportType = new List<ReportParameterListItem>()
            {                
                new ReportParameterListItem() { Key = AppTemplates.no_ContratoTrabajador, Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_ContratoTrabajador) }, 
                new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CertificadoLaboral) }, 
                new ReportParameterListItem() { Key = "3", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CertificadoLaboralHistorico) }, 
                new ReportParameterListItem() { Key = "4", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_IngresosRetenciones) }, 
                new ReportParameterListItem() { Key = "5", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_CertificadoSalud) }, 
                new ReportParameterListItem() { Key = "6", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_BoletaPago) },
            };

                // Combo para filtrar por documento para la Boleta de Pago
                List<ReportParameterListItem> DocNomina = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "81", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Nomina)},
                    new ReportParameterListItem() { Key = "82", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Vacaciones)},
                    new ReportParameterListItem() { Key = "83", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Prima)},
                    new ReportParameterListItem() { Key = "84", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Rpt_Liquidacion)},
                };

                // Combo de Quincena
                List<ReportParameterListItem> Quincena = new List<ReportParameterListItem>()
                {
                    new ReportParameterListItem() { Key = "1", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Primera_Quincena)},
                    new ReportParameterListItem() { Key = "2", Desc = _bc.GetResource(LanguageTypes.Tables, DictionaryTables.Segunda_Quincena)},
                };

                //Determina el nombre del combo y el item donde debe quedar
                this.AddList    ("1", (AppForms.ReportForm).ToString() + "_TipoCertificado", noFondoCajaCompensacionReportType, true, "1",true);
                this.AddMaster  ("2", AppMasters.noEmpleado, true, null);
                this.AddList    ("3", (AppForms.ReportForm).ToString() + "_DocumentoNomina", DocNomina,true, "1",false);
                this.AddList    ("4", (AppForms.ReportForm).ToString() + "_quincena", Quincena,true, "1",false);
                //this.AddCheck   ("4", _bc.GetResource(LanguageTypes.Forms, AppForms.ReportForm + "_chkGetAll")); 
                #endregion

                if (string.IsNullOrWhiteSpace(this._report))
                {
                    this._report = AppTemplates.no_ContratoTrabajador;

                    #region Configurar Filtros
                    this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                    this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                    this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                    this.periodoFilter1.monthCB.SelectedIndex = 0;

                    this.btnFilter.Enabled = true;
                    this.btnResetFilter.Enabled = true;
                    this.btnFilter.Visible = true;
                    this.btnResetFilter.Visible = true;

                    List<ConsultasFields> fieldsNRT = new List<ConsultasFields>();
                    fieldsNRT.Add(new ConsultasFields("PagaduriaID", "Pagaduria", typeof(string)));
                    mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
                    mq.SetFK("PagaduriaID", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));

                    #endregion
                }
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
            string fechaIniString = string.Empty;
            string fechaFinString = string.Empty;

            this.btnFilter.Enabled = false;
            this.btnResetFilter.Enabled = false;
            this.periodoFilter1.Enabled = false;
            this.btnExportToPDF.Enabled = false;

            string nitEmpresa = this._bc.GetNitEmpresa();

            #region Validar Fecha para reporte Boleta de Pago
            this.periodoFilter1.Year[0].ToString();
            fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
            this._fechaIni = Convert.ToDateTime(fechaIniString);

            this._mes = this.periodoFilter1.Months[0];
            this._año = Convert.ToInt32(this.periodoFilter1.txtYear.Text); 
            #endregion

            switch (this._report)
            {
                case "2":
                    {
                        this.periodoFilter1.Year[0].ToString();
                        fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                        fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                        this._fechaIni = Convert.ToDateTime(fechaIniString);
                        this._fechaFin = Convert.ToDateTime(fechaFinString);
                        break;
                    }
                default:
                    {
                        this.periodoFilter1.Year[0].ToString();
                        fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                        this._fechaIni = Convert.ToDateTime(fechaIniString);
                        break;
                    }
            }

            Thread process = new Thread(this.LoadReportMethod_PDF);
            process.Start();

            #region Pendiente
                    /*
                    #region Asignación de parametros

                    Templates tem = new Templates();
                    List<string> list = new List<string>();


                    list.Add(this._dtoEmpleado.LugarGeograficoDesc.Value);
                    list.Add(DateTime.Now.ToShortDateString());
                    list.Add(this._dtoEmpleado.EmpresaGrupoID.Value);
                    list.Add(this._bc.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto));
                    list.Add(" ");
                    list.Add(this._dtoEmpleado.Descriptivo.Value);
                    list.Add(this._dtoEmpleado.ID.Value);
                    list.Add(this._dtoEmpleado.LugarNacimiento.Value);
                    list.Add(this._dtoEmpleado.FechaNacimiento.Value.Value.ToShortDateString());
                    list.Add(this._dtoEmpleado.DireccionResidencia.Value);
                    list.Add(this._dtoEmpleado.PeriodoPago.Value.Value.ToString());
                    list.Add(this._dtoEmpleado.CargoEmpDesc.Value);
                    list.Add(" ");

                    tem.GenerarPlantilla(this._report, list);

                    #endregion
                    */

                #endregion            
        }

        /// <summary>
        /// Funcion que se encarga de generar el reporte en XLS
        /// </summary>
        protected override void Report_XLS()
        {
            try
            {
                string fechaIniString = string.Empty;
                string fechaFinString = string.Empty;

                this.btnFilter.Enabled = false;
                this.btnResetFilter.Enabled = false;
                this.periodoFilter1.Enabled = false;
                this.btnExportToPDF.Enabled = false;

                string nitEmpresa = this._bc.GetNitEmpresa();

                this.periodoFilter1.Year[0].ToString();
                fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                this._fechaIni = Convert.ToDateTime(fechaIniString);

                this._mes = this.periodoFilter1.Months[0];
                this._año = Convert.ToInt32(this.periodoFilter1.txtYear.Text);

                switch (this._report)
                {
                    case "2":
                        {
                            this.periodoFilter1.Year[0].ToString();
                            fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                            fechaFinString = this.periodoFilter1.txtYear1.Text + " / " + this.periodoFilter1.Months[1];
                            this._fechaIni = Convert.ToDateTime(fechaIniString);
                            this._fechaFin = Convert.ToDateTime(fechaFinString);
                            break;
                        }
                    default:
                        {
                            this.periodoFilter1.Year[0].ToString();
                            fechaIniString = this.periodoFilter1.txtYear.Text + " / " + this.periodoFilter1.Months[0];
                            this._fechaIni = Convert.ToDateTime(fechaIniString);
                            break;
                        }
                }
                string empleadoID = this._dtoEmpleado != null ? this._dtoEmpleado.ID.Value : string.Empty;

                this._Query = this._bc.AdministrationModel.Reportes_No_NominaToExcel(this.documentReportID, null, this._fechaIni, null, empleadoID, this._quincena,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, this._documentoNomina, null, null);

                if (this._Query.Rows.Count != 0)
                {
                    ReportExcelBase frm = new ReportExcelBase(this._Query, this.documentReportID);
                    frm.Show();
                }
                else
                    MessageBox.Show(_bc.GetResource(LanguageTypes.Messages, DictionaryMessages.NoDataFound));
            }
            catch (Exception ex)
            {
                MessageBox.Show(_bc.GetResourceForException(ex, "WinApp-Reports_AportesNomina.cs", "Report_XLS"));
            }
            finally
            {
                this.Invoke(this.EndGenerarDelegate);
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

            // Valor del combo de Documento
            ReportParameterList VlDoc = (ReportParameterList)this.RPControls["3"];
            this._documentoNomina = VlDoc.SelectedListItem.Key;

            // Valor de la quincena
            if (option.Equals("4"))
            {
                ReportParameterList listReportType = (ReportParameterList)this.RPControls["4"];
                this._quincena = listReportType.SelectedListItem.Key;
            };

            #region Ocultar combo documento si no es Boleta de Pago
            // Valor Combo Quincena
            if (option.Equals("1"))
            {
                ReportParameterList Boleta3 = (ReportParameterList)this.RPControls["1"]; ;
                ReportParameterList Boleta = (ReportParameterList)this.RPControls["3"];
                ReportParameterList Boleta2 = (ReportParameterList)this.RPControls["4"];

                switch (reportParameters["1"][0])
                {
                    case "_TipoCertificado":
                        Boleta.Visible = false;
                        Boleta2.Visible = false;
                        Boleta.Enabled = false;
                        Boleta2.Enabled = false;
                        break;

                    case "2":
                        Boleta.Visible = false;
                        Boleta2.Visible = false;
                        Boleta.Enabled = false;
                        Boleta2.Enabled = false;
                        break;

                    case "3":
                        Boleta.Visible = false;
                        Boleta2.Visible = false;
                        Boleta.Enabled = false;
                        Boleta2.Enabled = false;
                        break;

                    case "4":
                        Boleta.Visible = false;
                        Boleta2.Visible = false;
                        Boleta.Enabled = false;
                        Boleta2.Enabled = false;
                        break;

                    case "5":
                        Boleta.Visible = false;
                        Boleta2.Visible = false;
                        Boleta.Enabled = false;
                        Boleta2.Enabled = false;
                        break;

                    case "6":
                        Boleta3.DefaultKey = "6";
                        Boleta3.RefreshList();

                        Boleta.Visible = true;
                        Boleta2.Visible = true;
                        Boleta.Enabled = true;
                        Boleta2.Enabled = true;
                        break;
                }
            }
            #endregion

            #region Valor y Ocultar combo de la Quincena
            // Valor Combo Quincena
            if (option.Equals("3"))
            {
                ReportParameterList quincena1 = (ReportParameterList)this.RPControls["3"];
                ReportParameterList quincena2 = (ReportParameterList)this.RPControls["4"];

                switch (reportParameters["3"][0])
                {
                    case "81":
                        quincena1.DefaultKey = "1";
                        quincena1.RefreshList();

                        quincena2.Visible = true;
                        quincena2.Enabled = true;
                        break;

                    case "82":
                        quincena2.Visible = false;
                        quincena2.Enabled = false;
                        break;

                    case "83":
                        quincena2.Visible = false;
                        quincena2.Enabled = false;
                        break;

                    case "84":
                        quincena2.Visible = false;
                        quincena2.Enabled = false;
                        break;
                }
            }
            #endregion
            
            //Reporte
            switch (option)
            {
                case "1":
                    {
                        ReportParameterList listReportType = (ReportParameterList)this.RPControls["1"];
                        this._report = listReportType.SelectedListItem.Key;

                        if (listReportType.SelectedListItem.Key == AppTemplates.no_ContratoTrabajador)
                        {
                            #region Configurar Filtros
                            this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearMonth;
                            this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                            this.periodoFilter1.monthCB.Items.Add(this.periodo.Date.Month);
                            this.periodoFilter1.monthCB.SelectedIndex = 0;

                            this.btnFilter.Enabled = true;
                            this.btnResetFilter.Enabled = true;
                            this.btnFilter.Visible = true;
                            this.btnResetFilter.Visible = true;
                            List<ConsultasFields> fieldsNRT = new List<ConsultasFields>();

                            fieldsNRT.Add(new ConsultasFields("PagaduriaID", "Pagaduria", typeof(string)));
                            mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
                            mq.SetFK("PagaduriaID", AppMasters.ccPagaduria, _bc.CreateFKConfig(AppMasters.ccPagaduria));
                            #endregion
                        }
                        else
                        {
                            switch (this._report)
                            {
                                case "2":
                                    {
                                        #region Configurar Filtros
                                        this.periodoFilter1.FilterOptions = PeriodFilterOptions.YearSpanMonthSpan;
                                        this.periodoFilter1.txtYear.Text = (this.periodo.Date.Year).ToString();
                                        this.periodoFilter1.txtYear1.Text = (this.periodo.Date.Year).ToString();
                                        this.periodoFilter1.monthCB.Items.Add(1);
                                        this.periodoFilter1.monthCB.SelectedIndex = 0;
                                        this.periodoFilter1.monthCB1.Items.Add(this.periodo.Date.Month);
                                        this.periodoFilter1.monthCB1.SelectedIndex = 0;

                                        List<ConsultasFields> fieldsNRT = new List<ConsultasFields>();
                                        fieldsNRT.Add(new ConsultasFields("EmpleadoID", "Cedula", typeof(string)));
                                        mq = new MasterQuery(this, AppReports.noDetalleNomina, this._bc.AdministrationModel.User.ReplicaID.Value.Value, true, fieldsNRT);
                                        mq.SetFK("EmpleadoID", AppMasters.noEmpleado, _bc.CreateFKConfig(AppMasters.noEmpleado));
                                        #endregion
                                        break;
                                    }
                            }
                        }    
                        break;
                    }
                case "2" :
                    {
                        uc_MasterFind master = (uc_MasterFind)sender;
                        this._dtoEmpleado = (DTO_noEmpleado)_bc.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.noEmpleado, false, master.Value, true);
                        break;
                    }
                case "3":
                    {
                        this._checkAll = true;
                        break;
                    }
            }          

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
            foreach (var fil in consulta.Filtros)
                this._filtro = fil.ValorFiltro;
        } 
        #endregion
    }
}
