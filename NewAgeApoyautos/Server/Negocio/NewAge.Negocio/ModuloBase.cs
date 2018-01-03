using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using NewAge.Cliente.GUI.WinApp.Reports;
using NewAge.DTO.Negocio;
using NewAge.ADO;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using System.Reflection;
using NewAge.Librerias.ExceptionHandler;
using NewAge.DTO.Reportes;
using NewAge.ADO.Reportes;
using NewAge.ReportesComunes;
using System.Collections;
using System.IO;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Resultados;
using SentenceTransformer;

namespace NewAge.Negocio
{
    public class ModuloBase
    {
        #region Variables

        /// <summary>
        /// Dicccionario para el manejo de instancias de modelos
        /// </summary>
        private Dictionary<string, object> _instanciasModelos = new Dictionary<string, object>();
        private Dictionary<LanguageTypes, Dictionary<string, string>> rsx = new Dictionary<LanguageTypes, Dictionary<string, string>>();

        //DALS
        private DAL_aplReporte _dal_aplReporte = null;
        private DAL_glControl _dal_glControl = null;
        private DAL_glEmpresa _dal_glEmpresa = null;
        private DAL_seUsuario _dal_seUsuario = null;
        private DAL_Comprobante _dal_Comprobante = null;
        private DAL_OperacionesDocumentos _dal_OperacionesDocumentos = null;
        private DAL_glActividadControl _dal_glActividadControl = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Get or sets the connection
        /// </summary>
        internal SqlConnection _mySqlConnection
        {
            get;
            set;
        }

        /// <summary>
        /// Get or sets the connection Tx
        /// </summary>
        public SqlTransaction _mySqlConnectionTx
        {
            get;
            set;
        }

        /// <summary>
        /// Get or sets Empresa
        /// </summary>
        public DTO_glEmpresa Empresa
        {
            get;
            set;
        }
                
        /// <summary>
        /// Usuario de la aplicación
        /// </summary>
        public int UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Get or sets the logger connection string
        /// </summary>
        public string loggerConnectionStr
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ModuloBase(SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, string loggerConn)
        {
            this._mySqlConnection = conn;
            this._mySqlConnectionTx = tx;
            this.Empresa = emp;
            this.UserId = userID;
            this.loggerConnectionStr = loggerConn;
        }

        #region Funciones Privadas

        /// <summary>
        /// Determina si se modifican las parametros del constructor de la instacion solicitada
        /// </summary>
        /// <param name="tipo">tipo de objeto</param>
        /// <param name="obj">objeto</param>
        /// <param name="args">arreglo de parametros del constructor</param>
        /// <returns></returns>
        private object getParametersInstance(Type tipo, object obj, params object[] args)
        {
            SqlTransaction tx = (SqlTransaction)args.Where(x => x != null && x.GetType() == typeof(SqlTransaction)).FirstOrDefault();
            if (tx != null)
            {
                if (tx.IsolationLevel == IsolationLevel.ReadCommitted)
                {
                    PropertyInfo[] properties = obj.GetType().GetProperties();
                    PropertyInfo pTx = properties.Where(x => x.PropertyType.FullName == typeof(SqlTransaction).ToString()).FirstOrDefault();
                    if (pTx != null)
                    {
                        pTx.SetValue(obj, tx, null);
                        this._instanciasModelos.Remove(tipo.ToString());
                        this._instanciasModelos.Add(tipo.ToString(), obj);
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// Calcula una nueva fecha a partir de un periodo de tiempo
        /// </summary>
        /// <param name="un">Unidad de tiempo</param>
        /// <param name="cantTiempo">Tiempo para incrementar en la fecha</param>
        /// <returns>Retorna la nueva fecha</returns>
        private DateTime CalculateAlarmDate(UnidadTiempo un, int cantTiempo)
        {
            DateTime res = DateTime.Now;
            switch (un)
            {
                case UnidadTiempo.Minuto:
                    res = res.AddMinutes(cantTiempo);
                    break;
                case UnidadTiempo.Hora:
                    res = res.AddHours(cantTiempo);
                    break;
                case UnidadTiempo.Dia:
                    res = res.AddDays(cantTiempo);
                    break;
                case UnidadTiempo.Semana:
                    int days = cantTiempo * 7;
                    res = res.AddDays(days);
                    break;
                case UnidadTiempo.Mes:
                    res = res.AddMonths(cantTiempo);
                    break;
            }

            return res;
        }

        #endregion

        #region Funciones Internas

        /// <summary>
        /// funcion para instanciar un objeto del diccionario de objetos
        /// </summary>
        /// <param name="type">type de objeto</param>
        /// <param name="args">argumentos del constructor</param>
        /// <returns>instancia objeto</returns>
        internal object GetInstance(Type tipo, params object[] args)
        {
            object obj = null;
            try
            {
                if (this._instanciasModelos.ContainsKey(tipo.ToString()))
                {
                    obj = this._instanciasModelos[tipo.ToString()];
                    obj = this.getParametersInstance(tipo, obj, args);
                }
                else
                {
                    obj = Activator.CreateInstance(tipo, args);
                    this._instanciasModelos.Add(tipo.ToString(), obj);
                }
            }
            catch(Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "ModuloBase.cs-GetInstance");
            }

            return obj;
        }

        /// <summary>
        /// Dice si el usuario esta trabajando en multimoneda
        /// </summary>
        /// <returns></returns>
        internal bool Multimoneda()
        {
            string multimoneda = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_IndMultimoneda);
            return multimoneda == "1" ? true : false;
        }

        #region Archivos

        /// <summary>
        /// funcion para obtener la ruta del archivo
        /// </summary>
        /// <param name="fileName">nombre del archivo</param>
        public string GetFileLocalPath(string fileName, TipoArchivo fileType)
        {
            int ruta;
            switch (fileType)
            {
                case TipoArchivo.Documentos:
                    ruta = AppControl.RutaDocumentos;
                    break;
                case TipoArchivo.Imagenes:
                    ruta = AppControl.RutaImagenes;
                    break;
                case TipoArchivo.Mails:
                    ruta = AppControl.RutaMails;
                    break;
                case TipoArchivo.Temp:
                    ruta = AppControl.RutaTemporales;
                    break;
                default:
                    ruta = AppControl.RutaDocumentos;
                    break;
            }

            //Llaves para el nomrbe del archivo
            string filesPath = this.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this.GetControlValue(ruta);

            string ext = this.GetControlValue(AppControl.ExtensionDocumentos);
            string fileFormat = this.GetControlValue(AppControl.NombreArchivoDocumentos);

            string completePath = filesPath + docsPath + string.Format(fileFormat, fileName) + ext;
            return completePath;
        }

        /// <summary>
        /// funcion para obtener la ruta del archivo
        /// </summary>
        /// <param name="fileName">nombre del archivo</param>
        internal string GetFileRemotePath(string fileName, TipoArchivo fileType)
        {
            int ruta;
            switch (fileType)
            {
                case TipoArchivo.Documentos:
                    ruta = AppControl.RutaDocumentos;
                    break;
                case TipoArchivo.Imagenes:
                    ruta = AppControl.RutaImagenes;
                    break;
                case TipoArchivo.Mails:
                    ruta = AppControl.RutaMails;
                    break;
                default:
                    ruta = AppControl.RutaDocumentos;
                    break;
            }

            //Llaves para el nomrbe del archivo
            string filesPath = this.GetControlValue(AppControl.RutaVirtualArchivos);
            string docsPath = this.GetControlValue(ruta);

            string filePath = filesPath + docsPath.Replace("\\", "/");
            string ext = this.GetControlValue(AppControl.ExtensionDocumentos);
            string fileFormat = this.GetControlValue(AppControl.NombreArchivoDocumentos);

            string completePath = filePath + string.Format(fileFormat, fileName) + ext;
            return completePath;
        }

        /// <summary>
        /// Genera un reporte
        /// </summary>
        /// <param name="numeroDoc">Numero del documento con el cual se salva el archivo</param>
        /// <param name="userId">Id del usuario/param>
        /// <param name="data">Datos para generar el reporte</param>
        internal void GenerarArchivo(int documentID, int numeroDoc, object data)
        {
            try
            {
                #region Variables generales
                this._dal_glEmpresa = (DAL_glEmpresa)this.GetInstance(typeof(DAL_glEmpresa), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_seUsuario = (DAL_seUsuario)this.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_seUsuario user = this._dal_seUsuario.DAL_seUsuario_GetUserByReplicaID(this.UserId);
                string terceroPorDefecto = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);

                string idiomaID = user.IdiomaID.Value;
                DAL_ReportDataSupplier suppl = new DAL_ReportDataSupplier(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, idiomaID, this.loggerConnectionStr);

                //Info de la empresa
                DAL_glEmpresa dalempr = (DAL_glEmpresa)this.GetInstance(typeof(DAL_glEmpresa), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                byte[] logo = dalempr.DAL_glEmpresaLogo();

                suppl.NombreEmpresa = this.Empresa.Descriptivo.Value;
                suppl.LogoEmpresa = logo;
                suppl.UserName = user.ID.Value;
                suppl.NitEmpresa = terceroPorDefecto;

                //Info de rutas y nombres de archivos

                string file = this.GetFileLocalPath(numeroDoc.ToString(), TipoArchivo.Documentos);

                //Info de los datos
                BaseCommonReport rep = null;                
                Type dataType = data.GetType();
                ArrayList fieldList = new ArrayList();

                if (File.Exists(file))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex) { }
                }

                bool multimoneda = this.Multimoneda();
                #endregion
                #region Generacion del reporte


                if (dataType == typeof(DTO_ReportComprobante2))
                {
                    #region Reporte de comprobante manual
                    DTO_ReportComprobante2 comp = (DTO_ReportComprobante2)data;
                    fieldList.AddRange(ColumnsInfo.ComprobanteFields);

                    if (multimoneda)
                    {
                        fieldList.Add("DebitoME");
                        fieldList.Add("CreditoME");
                    };

                    rep = new ComprobanteReport(AppDocuments.ComprobanteManual, new List<DTO_ReportComprobante2>() { comp }, multimoneda, fieldList, (comp.Estado.Trim() == (EstadoDocControl.Aprobado).ToString().Trim()) ? false : true, suppl, new List<string>());
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportDocumentoContable))
                {
                    #region Reportes de Documento contable y Ajuste de saldos
                    this._dal_aplReporte = (DAL_aplReporte)this.GetInstance(typeof(DAL_aplReporte), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    DTO_ReportDocumentoContable docCont = (DTO_ReportDocumentoContable)data;

                    byte[] arr = this._dal_aplReporte.DAL_aplReporte_GetByID(documentID);
                    if (arr != null)
                    {
                        //ReportForm ribbonForm = new ReportForm();
                        XtraReport customReport = new ReporteDinamico(suppl);
                        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                            customReport.LoadLayout(memoryStream);

                        customReport.DataSource = new List<DTO_ReportDocumentoContable>() { docCont };
                        rep = (ReporteDinamico)customReport;
                    }
                    else
                    {
                        fieldList.AddRange(ColumnsInfo.ComprobanteFields);
                        if (multimoneda)
                        {
                            fieldList.Add("DebitoME");
                            fieldList.Add("CreditoME");
                        };

                        rep = new DocumentoContableReport(Convert.ToInt32(docCont.coDocumentoID) == AppDocuments.DocumentoContable ? AppReports.coDocumentoContable : AppReports.coAjusteSaldos, new List<DTO_ReportDocumentoContable>() { docCont }, multimoneda, fieldList, suppl);
                    }

                    #endregion
                }
                else if (dataType == typeof(DTO_ReportAnticipo))
                {
                    #region Reporte de anticipos
                    DTO_ReportAnticipo reportAnticipo = (DTO_ReportAnticipo)data;

                    rep = new AnticipoReport(AppReports.cpAnticipo, new List<DTO_ReportAnticipo>() { reportAnticipo }, reportAnticipo.EstadoInd, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportAnticipoViaje))
                {
                    #region Reporte de anticipos
                    DTO_ReportAnticipoViaje reportAnticipoViaje = (DTO_ReportAnticipoViaje)data;

                    rep = new AnticipoViajeReport(AppReports.cpAnticipo, new List<DTO_ReportAnticipoViaje>() { reportAnticipoViaje }, reportAnticipoViaje.EstadoInd, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportAutorizacionDeGiro))
                {
                    #region Reporte de causacion factura
                    DTO_ReportAutorizacionDeGiro reportAutorGiro = (DTO_ReportAutorizacionDeGiro)data;
                    fieldList = ColumnsInfo.AutorGiroFields;
                    if (this.Multimoneda() && !fieldList.Contains("ValorME"))
                        fieldList.Add("ValorME");
                    rep = new AutorizacionDeGiroReport(AppReports.cpAutorizacionDeGiro, new List<DTO_ReportAutorizacionDeGiro>() { reportAutorGiro }, fieldList, this.Multimoneda(), reportAutorGiro.EstadoInd, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportReciboCaja))
                {
                    #region Reporte de recibo de caja
                    DTO_ReportReciboCaja reportReciboCaja = (DTO_ReportReciboCaja)data;
                    fieldList = ColumnsInfo.ReciboCajaFields;
                    rep = new ReciboCajaReport(AppReports.coReciboCaja, new List<DTO_ReportReciboCaja>() { reportReciboCaja }, fieldList, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportCajaMenor))
                {
                    #region Reporte de Caja Menor
                    DTO_ReportCajaMenor reportCajaMenor = (DTO_ReportCajaMenor)data;
                    fieldList = ColumnsInfo.CajaMenorFields;
                    rep = new CajaMenorReport(AppReports.cpCajaMenor, new List<DTO_ReportCajaMenor>() { reportCajaMenor }, fieldList, reportCajaMenor.EstadoInd, suppl);
                    #endregion
                }                
                else if (dataType == typeof(DTO_ReportPagoFacturas))
                {
                    #region Reporte de Pago Facturas

                    //this._dal_aplReporte = (DAL_aplReporte)this.GetInstance(typeof(DAL_aplReporte), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    //DTO_ReportPagoFacturas reportCheque = (DTO_ReportPagoFacturas)data;

                    //byte[] arr = this._dal_aplReporte.DAL_aplReporte_GetByID(documentID);
                    //if (arr != null)
                    //{
                    //    //ReportForm ribbonForm = new ReportForm();
                    //    XtraReport customReport = new ReporteDinamico(suppl);
                    //    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(arr))
                    //        customReport.LoadLayout(memoryStream);

                    //    customReport.DataSource = new List<DTO_ReportPagoFacturas>() { reportCheque };
                    //    rep = (ReporteDinamico)customReport;
                    //}
                    //else
                    //{
                    //    fieldList = ColumnsInfo.ChequeFields;
                    //    rep = new PagoFacturasReport(AppReports.PagoFacturas, new List<DTO_ReportPagoFacturas>() { reportCheque }, fieldList, suppl);
                    //}
                    #endregion
                }                    
                else if (dataType == typeof(DTO_ReportLegalizacionGastos))
                {
                    #region Reporte de Legalizacion Gastos
                    DTO_ReportLegalizacionGastos reportLegalizacion = (DTO_ReportLegalizacionGastos)data;
                    ArrayList footerFieldList = ColumnsInfo.LegaGastosFooterFields;
                    ArrayList detailFieldList = ColumnsInfo.LegaGastosDetailFields;
                    rep = new LegalizacionGastosReport(AppReports.cpLegalizacionGastos, new List<DTO_ReportLegalizacionGastos>() { reportLegalizacion }, footerFieldList, detailFieldList, reportLegalizacion.Header.EstadoInd, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportTransaccionManual))
                {
                    #region reporte Transaccion Manual
                    DTO_ReportTransaccionManual reportTransaccionMnl = (DTO_ReportTransaccionManual)data;
                    ArrayList footerFieldList = ColumnsInfo.FormTransacMnlFooterFields;
                    ArrayList detailFieldList = ColumnsInfo.FormTransacMnlDetailFields;
                    rep = new TransaccionManualReport(AppReports.inTransaccionManual, new List<DTO_ReportTransaccionManual>() { reportTransaccionMnl }, footerFieldList, detailFieldList, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportNotaEnvio))
                {
                    #region reporte Nota de Envio
                    DTO_ReportNotaEnvio reportNotaE = (DTO_ReportNotaEnvio)data;
                    ArrayList footerFieldList = ColumnsInfo.FormNotaEFooterFields;
                    ArrayList detailFieldList = ColumnsInfo.FormNotaEDetailFields;
                    rep = new NotaEnvioReport(AppReports.inNotaEnvio, new List<DTO_ReportNotaEnvio> { reportNotaE }, footerFieldList, detailFieldList, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportSolicitud))
                {
                    #region reporte Nota de Envio
                    DTO_ReportSolicitud reportSolic = (DTO_ReportSolicitud)data;
                    ArrayList detailFieldList = ColumnsInfo.FormSoliciDetailFields;
                    rep = new SolicitudReport(AppReports.prSolicitudOld, new List<DTO_ReportSolicitud> { reportSolic }, detailFieldList, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportNoPreNomina))
                {
                    #region reporte Prenomina
                    DTO_ReportNoPreNomina reportNoPreNomina = (DTO_ReportNoPreNomina)data;
                    ArrayList detailFieldList = ColumnsInfo.FormPreNominaDetailFields;
                    ArrayList footerFieldList = ColumnsInfo.FormPreNominaFooterFields;
                    rep = new PreNominaReport(AppReports.noPreNomina, new List<DTO_ReportNoPreNomina> { reportNoPreNomina }, detailFieldList, footerFieldList, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportNoVacaciones))
                {
                    #region reporte nominaVacaciones
                    DTO_ReportNoVacaciones reportNoVacaciones = (DTO_ReportNoVacaciones)data;
                    ArrayList detailFieldList1 = ColumnsInfo.FormPreVacacionesFields1;
                    ArrayList detailFieldList2 = ColumnsInfo.FormPreVacacionesFields2;
                    rep = new VacacionesReport(AppReports.noVacaciones, new List<DTO_ReportNoVacaciones> { reportNoVacaciones }, detailFieldList1, detailFieldList2, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportInvFisico))
                {
                    #region reporte Inventario Físico
                    DTO_ReportInvFisico reportInvFisico = (DTO_ReportInvFisico)data;
                    switch (reportInvFisico.TipoReport)
                    {
                        case InventarioFisicoReportType.Conteo:
                            ArrayList detailFieldList1 = ColumnsInfo.FormReportInvFisicoConteo;
                            rep = new InventarioFisicoReport(AppReports.inFisicoInventario, new List<DTO_ReportInvFisico> { reportInvFisico }, detailFieldList1, suppl, InventarioFisicoReportType.Conteo);
                            break;
                        case InventarioFisicoReportType.Fisico:
                            ArrayList detailFieldList2 = ColumnsInfo.FormReportInvFisicoFisico;
                            rep = new InventarioFisicoReport(AppReports.inFisicoInventario, new List<DTO_ReportInvFisico> { reportInvFisico }, detailFieldList2, suppl, InventarioFisicoReportType.Fisico);
                            break;
                        case InventarioFisicoReportType.Diferencia:
                            ArrayList detailFieldList3 = ColumnsInfo.FormReportInvFisicoDiferencia;
                            rep = new InventarioFisicoReport(AppReports.inFisicoInventario, new List<DTO_ReportInvFisico> { reportInvFisico }, detailFieldList3, suppl, InventarioFisicoReportType.Diferencia);
                            break;
                    }

                    #endregion
                }
                else if (dataType == typeof(DTO_ReportLiquidacionCredito2))
                {
                    #region reporte LiquidacionCredito
                    DTO_ReportLiquidacionCredito2 reportLiquidacionCredito = (DTO_ReportLiquidacionCredito2)data;
                    ArrayList detailFieldList1 = ColumnsInfo.FormLiquidcacionCreditoDetailFields;
                    ArrayList footerFieldList1 = ColumnsInfo.FormLiquidcacionCreditoFooterFields;
                    rep = new LiquidcacionCreditoReport(AppReports.ccLiquidacionCredito, new List<DTO_ReportLiquidacionCredito2> { reportLiquidacionCredito }, detailFieldList1, footerFieldList1, suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportNominaDetalleCxE))
                {
                    #region reporte LiquidacionCredito
                    DTO_ReportNominaDetalleCxE ReportNominaDetalleCxE = (DTO_ReportNominaDetalleCxE)data;
                    ArrayList detailFieldList1 = ColumnsInfo.FormReportNominaDetalleCxE;
                    rep = new NominaDetailExCReport(AppReports.noDetalleNomina, new List<DTO_ReportNominaDetalleCxE> { ReportNominaDetalleCxE }, detailFieldList1, "Det.Empleado Concepto", suppl);
                    #endregion
                }
                else if (dataType == typeof(DTO_ReportNominaInfoEmpleado))
                {
                    #region reporte LiquidacionCredito
                    DTO_ReportNominaInfoEmpleado ReportNominaDetalleExE = (DTO_ReportNominaInfoEmpleado)data;
                    ArrayList detailFieldList1 = ColumnsInfo.FormReportNominaDetalleCxE;
                    rep = new NominaDetailExEReport(AppReports.noDetalleNomina, new List<DTO_ReportNominaInfoEmpleado> { ReportNominaDetalleExE }, detailFieldList1, "DtoReportNominaDetail_ExE", suppl);
                    #endregion
                }
                
                #endregion
                #region Exporta el reporte al formato deseado
                string ext = this.GetControlValue(AppControl.ExtensionDocumentos);
                switch (ext)
                {
                    case ".pdf":
                        rep.ExportToPdf(file);
                        break;
                    case ".html":
                        rep.ExportToHtml(file);
                        break;
                    case ".xls":
                        rep.ExportToXls(file);
                        break;
                    case ".xlsx":
                        rep.ExportToXlsx(file);
                        break;
                }
                #endregion;
            }
            catch (Exception ex1)
            {
                var exception = new Exception(DictionaryMessages.Err_ReportCreate, ex1);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloBase.cs-GenerarArchivo");
                throw exception;
            }
        }

        /// <summary>
        /// Exporta un archivo a csv
        /// </summary>
        /// <param name="tipo">Tipo de dato</param>
        /// <param name="list">Lista de datos a exportar</param>
        internal string GetCvsName(ExportType type)
        {
            try
            {
                string filesPath = this.GetControlValue(AppControl.RutaFisicaArchivos);
                string tempPath = this.GetControlValue(AppControl.RutaTemporales);
                string fileName = Guid.NewGuid().ToString();
                string ext = string.Empty;

                switch (type)
                {
                    case ExportType.Csv:
                        ext = ".csv";
                        break;
                    case ExportType.Txt:
                        ext = ".txt";
                        break;
                    default:
                        ext = ".csv";
                        break;
                }
                string file = filesPath + tempPath + fileName +  ext;

                if (File.Exists(file))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex) { }
                }

                return file;
            }
            catch (Exception ex1)
            {
                var exception = new Exception(DictionaryMessages.Err_ReportCreate, ex1);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloBase.cs-GetCvsName");
                throw exception;
            }
        }

        /// <summary>
        /// Funcion que retorna el nombre del archivo temporal
        /// </summary>
        /// <param name="type">Tipo de dato</param>
        /// <param name="fileName">Nombre del archivo temporal generado</param>
        internal string GetCvsName(ExportType type, out string fileName)
        {
            try
            {
                string filesPath = this.GetControlValue(AppControl.RutaFisicaArchivos);
                string tempPath = this.GetControlValue(AppControl.RutaTemporales);
                fileName = Guid.NewGuid().ToString();
                string ext = string.Empty;

                switch (type)
                {
                    case ExportType.Csv:
                        ext = ".csv";
                        break;
                    case ExportType.Txt:
                        ext = ".txt";
                        break;
                    default:
                        ext = ".csv";
                        break;
                }
                string file = filesPath + tempPath + fileName + ext;
                
                if (File.Exists(file))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex) { }
                }
                fileName = fileName + ext;

                return file;
            }
            catch (Exception ex1)
            {
                var exception = new Exception(DictionaryMessages.Err_ReportCreate, ex1);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "ModuloBase.cs-GetCvsName");
                throw exception;
            }
        }

        #endregion

        #region Control

        /// <summary>
        /// Obtiene el valor del campo data en la tabla glControl segun el control
        /// </summary>
        /// <param name="ctrl">control</param>
        /// <returns></returns>
        public string GetControlValue(int ctrl)
        {
            this._dal_glControl = (DAL_glControl)this.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glControl.DAL_glControl_GetById(ctrl).Data.Value;
        }

        /// <summary>
        /// Obtiene el valor del campo descripcion en la tabla glControl segun el control
        /// </summary>
        /// <param name="ctrl">control</param>
        /// <returns></returns>
        public string GetControlDescripcionValue(int ctrl)
        {
            this._dal_glControl = (DAL_glControl)this.GetInstance(typeof(DAL_glControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_glControl.DAL_glControl_GetById(ctrl).Descriptivo.Value;
        }
        /// <summary>
        /// Devuelve in valor segun la empresa
        /// </summary>
        /// <param name="mod">Modulo del sistema</param>
        /// <param name="ctrl">Control que se desea consultar</param>
        /// <returns>Retorna el valor buscado</returns>
        public string GetControlValueByCompany(ModulesPrefix mod, string ctrl)
        {
            string modValue = ((int)mod).ToString();

            if (modValue.Length == 1)
                modValue = "0" + modValue;

            string empValue = this.Empresa.NumeroControl.Value;
            string key = empValue + modValue + ctrl;

            string result = this.GetControlValue(Convert.ToInt32(key));
            return result;
        }
        /// <summary>
        /// Devuelve in valor segun la empresa
        /// </summary>
        /// <param name="mod">Modulo del sistema</param>
        /// <param name="ctrl">Control que se desea consultar</param>
        /// <returns>Retorna la descripcion del valor buscado</returns>
        public string GetControlDescripcionValueByCompany(ModulesPrefix mod, string ctrl)
        {
            string modValue = ((int)mod).ToString();

            if (modValue.Length == 1)
                modValue = "0" + modValue;

            string empValue = this.Empresa.NumeroControl.Value;
            string key = empValue + modValue + ctrl;

            string result = this.GetControlDescripcionValue(Convert.ToInt32(key));
            return result;
        }


        #endregion

        #region Documentos

        internal string GetFactura(DateTime periodo, int numeroDoc)
        {
            string numDoc = numeroDoc.ToString();
            string resultDoc = string.Empty;

            if (numDoc.Length == 4)
                resultDoc = numDoc;
            else if (numDoc.Length < 4)
            {
                for (int i = numDoc.Length; i < 4; ++i)
                    resultDoc += "0";

                resultDoc += numDoc;
            }
            else
                resultDoc = numDoc.Substring(numDoc.Length - 4);

            return periodo.Month.ToString() + "_" + resultDoc;
        }

        /// <summary>
        /// Trae la informacion de un documetno que envia para aprobacion
        /// </summary>
        /// <param name="periodo">Periodo</param>
        /// <param name="comprobanteID">Codigo del comprobante</param>
        /// <param name="compNro">Consecutivo del comprobante</param>
        public DTO_Alarma GetFirstMailInfo(int numeroDoc, bool hasReport)
        {
            DAL_Alarmas alarmasDAL = new DAL_Alarmas(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_Alarma alarma = alarmasDAL.DAL_Alarma_GetDocumentMail(numeroDoc);
            alarma.NumeroDoc = numeroDoc.ToString();
            if (hasReport)
                alarma.FileName =  this.GetFileRemotePath(numeroDoc.ToString(), TipoArchivo.Documentos);

            return alarma;
        }

        /// <summary>
        /// Trae el consecutivo para un numero de documento
        /// Si no existe crea uno y lo inicia en 1
        /// </summary>
        /// <param name="prefijoID">Identificador del prefijo</param>
        /// <param name="onlyGet">Indica si solo puede traer la info o tambien crear un nuevo numero</param>
        /// <returns>Retorna el consecutivo</returns>
        public int GenerarDocumentoNro(int documentID, string prefijoID, bool onlyGet = false)
        {
            this._dal_OperacionesDocumentos = (DAL_OperacionesDocumentos)this.GetInstance(typeof(DAL_OperacionesDocumentos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_OperacionesDocumentos.GenerateNumeroDoc(documentID, prefijoID, onlyGet);
        }

        /// <summary>
        /// Genera un numero de comprobante
        /// </summary>
        /// <param name="comprobante">Identificador del comprobante</param>
        /// <param name="prefijoID">Identificador del prefijo del documento que se esta trabajando</param>
        /// <param name="documentoID">Identificador del documento sobre el cual se esta trabajando</param>
        /// <param name="periodo">Periodo sobre el cual se esta trabajando</param>
        /// <param name="documentoNro">Numero de documento (documentoNro) de glDocumentoControl</param>
        /// <returns>Retorna en numero de comprobante asignado</returns>
        internal int GenerarComprobanteNro(DTO_coComprobante comprobante, string prefijoID, DateTime periodo, int documentoNro, bool onlyGet = false)
        {
            this._dal_Comprobante = (DAL_Comprobante)this.GetInstance(typeof(DAL_Comprobante), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            return this._dal_Comprobante.DAL_Comprobante_GenerarNumeroComprobante(comprobante, prefijoID, periodo, documentoNro, onlyGet);
        }

        /// <summary>
        /// Obtiene el prefijo de un documento
        /// </summary>
        /// <param name="documentID">Documento que ejecuta la transaccion (obtiene el tipo de prefijo)</param>
        /// <param name="prefDocID">Documento del cual se obtiene el prefijo en glConsPrefijoDoc</param>
        /// <returns></returns>
        internal string GetPrefijoByDocumento(int documentID)
        {
            this._dal_seUsuario = (DAL_seUsuario)this.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_seUsuario user = this._dal_seUsuario.DAL_seUsuario_GetUserByReplicaID(this.UserId);

            DTO_glDocumento dtoDoc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento, documentID.ToString(), true, true);
            TipoPrefijo_Documento tipoPref = (TipoPrefijo_Documento)Enum.Parse(typeof(TipoPrefijo_Documento), dtoDoc.PrefijoTipo.Value.Value.ToString());
            switch (tipoPref)
            {
                case TipoPrefijo_Documento.Fijo:
                    return this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_PrefijoXDefecto);
                case TipoPrefijo_Documento.AreaFuncional:
                    this._dal_OperacionesDocumentos = (DAL_OperacionesDocumentos)this.GetInstance(typeof(DAL_OperacionesDocumentos), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                    return this._dal_OperacionesDocumentos.PrefijoDocumento_Get(user.AreaFuncionalID.Value, documentID);
                case TipoPrefijo_Documento.Documento:
                    return string.Empty;
                case TipoPrefijo_Documento.Digitado:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Obtiene el area funcional del usuario actual
        /// </summary>
        /// <returns>Retorna el area funcional del usuario</returns>
        internal string GetAreaFuncionalByUser()
        {
            this._dal_seUsuario = (DAL_seUsuario)this.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_seUsuario user = this._dal_seUsuario.DAL_seUsuario_GetUserByReplicaID(this.UserId);

            return user.AreaFuncionalID.Value;
        }

        /// <summary>
        /// Crea el detalle de un comprobante a partir de un documento control
        /// </summary>
        /// <param name="ctrl">Documento Control</param>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="cta">Cuenta</param>
        /// <param name="concCargoXdef">Concepto cargo por defecto</param>
        /// <param name="lgXdef">Lugar geografico por defecto</param>
        /// <param name="lineaXdef">Linea presupuestal X Def</param>
        /// <param name="vlrLoc">Valor local</param>
        /// <param name="vlrExt">Valor extranjero</param>
        /// <param name="isContra">Indica si es el registro de contrapartida</param>
        /// <returns>Retorna el detalle de un comprobante</returns>
        internal DTO_ComprobanteFooter CrearComprobanteFooter(DTO_glDocumentoControl ctrl, DTO_coPlanCuenta cta, DTO_glConceptoSaldo cSaldo, decimal? tc, 
            string concCargoXdef, string lgXdef, string lineaXdef, decimal vlrLoc, decimal vlrExt, bool isContra)
        {
            string proyectoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            string centroCtoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);

            DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
            detalle.CuentaID.Value =  cta.ID.Value;
            detalle.ProyectoID.Value = (string.IsNullOrWhiteSpace(ctrl.ProyectoID.Value) || !cta.ProyectoInd.Value.Value)? proyectoxDef : ctrl.ProyectoID.Value;
            detalle.CentroCostoID.Value = (string.IsNullOrWhiteSpace(ctrl.CentroCostoID.Value) || !cta.CentroCostoInd.Value.Value)? centroCtoxDef : ctrl.CentroCostoID.Value;
            detalle.LineaPresupuestoID.Value = (string.IsNullOrWhiteSpace(ctrl.LineaPresupuestoID.Value) || !cta.LineaPresupuestalInd.Value.Value)? lineaXdef : ctrl.LineaPresupuestoID.Value;
            detalle.LugarGeograficoID.Value = (string.IsNullOrWhiteSpace(ctrl.LugarGeograficoID.Value) || !cta.LugarGeograficoInd.Value.Value)? lgXdef : ctrl.LugarGeograficoID.Value;
            detalle.PrefijoCOM.Value = ctrl.PrefijoID.Value;
            detalle.TerceroID.Value = ctrl.TerceroID.Value;

            detalle.ConceptoCargoID.Value = (string.IsNullOrWhiteSpace(cta.ConceptoCargoID.Value) || !cta.ConceptoCargoInd.Value.Value)? concCargoXdef : cta.ConceptoCargoID.Value;
            detalle.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;
            detalle.TasaCambio.Value = tc.HasValue ? tc.Value : ctrl.TasaCambioCONT.Value;
            detalle.DocumentoCOM.Value = cSaldo.coSaldoControl.Value == (byte)SaldoControl.Doc_Interno ? ctrl.DocumentoNro.Value.Value.ToString() : ctrl.DocumentoTercero.Value;
            detalle.vlrMdaLoc.Value = vlrLoc;
            detalle.vlrMdaExt.Value = vlrExt;
            detalle.vlrMdaOtr.Value = 0; 
            detalle.vlrBaseML.Value = 0;
            detalle.vlrBaseME.Value = 0;

            //Calcula el identificadorTR
            detalle.IdentificadorTR.Value = 0;
            if (cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Componente_Documento
                || cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Externo
                || cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Interno)
            {
                detalle.IdentificadorTR.Value = ctrl.NumeroDoc.Value;
            }
            else if (cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Componente_Tercero)
                detalle.IdentificadorTR.Value = Convert.ToInt64(ctrl.TerceroID.Value);

            if (isContra)
                detalle.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

            return detalle;
        }

        /// <summary>
        /// Crea el detalle de un comprobante a partir de un documento control
        /// </summary>
        /// <param name="ctrl">Documento Control</param>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="cta">Cuenta</param>
        /// <param name="concCargoXdef">Concepto cargo por defecto</param>
        /// <param name="lgXdef">Lugar geografico por defecto</param>
        /// <param name="lineaXdef">Linea presupuestal X Def</param>
        /// <param name="vlrLoc">Valor local</param>
        /// <param name="vlrExt">Valor extranjero</param>
        /// <param name="isContra">Indica si es el registro de contrapartida</param>
        /// <returns>Retorna el detalle de un comprobante</returns>
        internal DTO_ComprobanteFooter CrearComprobanteFooter(DTO_glDocumentoControl ctrl, decimal? tc, string concCargoXdef, string lgXdef, string lineaXdef,
            decimal vlrLoc, decimal vlrExt, bool isContra)
        {
            string proyectoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_ProyectoXDefecto);
            string centroCtoxDef = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_CentroCostoXDefecto);

            DTO_coPlanCuenta cta = (DTO_coPlanCuenta)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coPlanCuenta, ctrl.CuentaID.Value, true, false);
            DTO_ComprobanteFooter detalle = new DTO_ComprobanteFooter();
            detalle.CuentaID.Value =  ctrl.CuentaID.Value;
            detalle.ProyectoID.Value = (string.IsNullOrWhiteSpace(ctrl.ProyectoID.Value) || !cta.ProyectoInd.Value.Value) ? proyectoxDef : ctrl.ProyectoID.Value;
            detalle.CentroCostoID.Value = (string.IsNullOrWhiteSpace(ctrl.CentroCostoID.Value) || !cta.CentroCostoInd.Value.Value) ? centroCtoxDef : ctrl.CentroCostoID.Value;
            detalle.LineaPresupuestoID.Value = (string.IsNullOrWhiteSpace(ctrl.LineaPresupuestoID.Value) || !cta.LineaPresupuestalInd.Value.Value)? lineaXdef : ctrl.LineaPresupuestoID.Value;
            detalle.LugarGeograficoID.Value = (string.IsNullOrWhiteSpace(ctrl.LugarGeograficoID.Value) || !cta.LugarGeograficoInd.Value.Value)? lgXdef : ctrl.LugarGeograficoID.Value;
            detalle.PrefijoCOM.Value = ctrl.PrefijoID.Value;
            detalle.TerceroID.Value = ctrl.TerceroID.Value;
            detalle.Descriptivo.Value = ctrl.Descripcion.Value;

            detalle.ConceptoCargoID.Value = (string.IsNullOrWhiteSpace(cta.ConceptoCargoID.Value) || !cta.ConceptoCargoInd.Value.Value)? concCargoXdef : cta.ConceptoCargoID.Value;
            detalle.ConceptoSaldoID.Value = cta.ConceptoSaldoID.Value;
            detalle.TasaCambio.Value = tc.HasValue ? tc.Value : ctrl.TasaCambioCONT.Value;
            detalle.DocumentoCOM.Value = ctrl.DocumentoTipo.Value == (byte)DocumentoTipo.DocInterno ? ctrl.DocumentoNro.Value.Value.ToString() : ctrl.DocumentoTercero.Value;
            detalle.vlrMdaLoc.Value = vlrLoc;
            detalle.vlrMdaExt.Value = vlrExt;
            detalle.vlrMdaOtr.Value = 0;
            detalle.vlrBaseML.Value = 0;
            detalle.vlrBaseME.Value = 0;

            //Calcula el identificadorTR
            detalle.IdentificadorTR.Value = 0;

            DTO_glConceptoSaldo cSaldo = (DTO_glConceptoSaldo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glConceptoSaldo, cta.ConceptoSaldoID.Value, true, false);
            if (cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Componente_Documento
                || cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Externo
                || cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Interno)
            {
                detalle.IdentificadorTR.Value = ctrl.NumeroDoc.Value;
            }
            else if (cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Componente_Tercero)
                detalle.IdentificadorTR.Value = Convert.ToInt64(ctrl.TerceroID.Value);

            if (isContra)
                detalle.DatoAdd4.Value = AuxiliarDatoAdd4.Contrapartida.ToString();

            return detalle;
        }

        /// <summary>
        /// Crea el detalle de un comprobante a partir de un documento control
        /// </summary>
        /// <param name="ctrl">Documento Control</param>
        /// <param name="tc">Tasa de cambio</param>
        /// <param name="cta">Cuenta</param>
        /// <param name="concCargoXdef">Concepto cargo por defecto</param>
        /// <param name="lgXdef">Lugar geografico por defecto</param>
        /// <param name="lineaXdef">Linea presupuestal X Def</param>
        /// <param name="vlrLoc">Valor local</param>
        /// <param name="vlrExt">Valor extranjero</param>
        /// <param name="isContra">Indica si es el registro de contrapartida</param>
        /// <returns>Retorna el detalle de un comprobante</returns>
        internal long GetIdentificadorTR(DTO_glDocumentoControl ctrl, DTO_glConceptoSaldo cSaldo)
        {
            long idTR = 0;

            if (cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Componente_Documento
                || cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Externo
                || cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Doc_Interno)
            {
                idTR = ctrl.NumeroDoc.Value.Value;
            }
            else if (cSaldo.coSaldoControl.Value.Value == (int)SaldoControl.Componente_Tercero)
                idTR = Convert.ToInt64(ctrl.TerceroID.Value);


            return idTR;
        }

        /// <summary>
        /// Cambia los signos de los valores en detalle de comprobante
        /// </summary>
        /// <param name="footer">detalle de comprobante</param>        
        /// <returns>Retorna detalle de comprobante con cambios</returns>
        internal List<DTO_ComprobanteFooter> CambiarSignoComprobante(List<DTO_ComprobanteFooter> footer)
        {
            foreach (DTO_ComprobanteFooter footerItem in footer)
            {
                footerItem.vlrBaseML.Value *= (-1);
                footerItem.vlrBaseME.Value *= (-1);
                footerItem.vlrMdaExt.Value *= (-1);
                footerItem.vlrMdaLoc.Value *= (-1);
                footerItem.vlrMdaOtr.Value *= (-1);
            }
            return footer;
        }

        /// <summary>
        /// Inserta un registro en la bitacora de tareas (glTareasControl)
        /// </summary>
        internal void AgregarActividadControl(DTO_glActividadControl actividadCtrl)
        {
            try
            {
                DTO_glDocumento doc = (DTO_glDocumento)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDocumento,
                    actividadCtrl.DocumentoID.Value.Value.ToString(), false, true);
                ModulesPrefix mod = (ModulesPrefix)Enum.Parse(typeof(ModulesPrefix), doc.ModuloID.Value.ToLower());

                string periodoStr = string.Empty;
                if (mod == ModulesPrefix.gl)
                { 
                    periodoStr = this.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_Periodo); 
                }
                else if (mod == ModulesPrefix.cf)
                {
                    periodoStr = this.GetControlValueByCompany(ModulesPrefix.cc, AppControl.co_Periodo);
                }
                else
                {
                    periodoStr = this.GetControlValueByCompany(mod, AppControl.co_Periodo);
                }

                DateTime periodo = Convert.ToDateTime(periodoStr);
                actividadCtrl.Periodo.Value = periodo;

                this._dal_glActividadControl = (DAL_glActividadControl)this.GetInstance(typeof(DAL_glActividadControl), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                this._dal_glActividadControl.AgregarTareasControl(actividadCtrl);
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_AddData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "AgregarActividadControl");
                throw exception;
            }
        }

        /// <summary>
        /// Busca un flujo de actividades y activa las alarmas correspondientes
        /// </summary>
        /// <param name="documentoID">Documento que esta ejecutando la transaccion</param>
        /// <param name="numeroDoc">Numero de documento que ejecuta el procedimiento</param>
        /// <param name="actividadPadreID">Identificador de la actividad padre</param>
        /// <param name="rechazoInd">Indica si no se esta aprobando sino rechazando la actividad</param>
        /// <param name="obs">Observaciones</param>
        /// <returns>Retorna true si esa actividad termina con el proceso</returns>
        internal DTO_TxResult AsignarFlujo(int documentoID, int numeroDoc, string actividadPadreID, bool rechazoInd, string obs, string actividadFuerza=null)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = false.ToString();

            try
            {
                DAL_glActividadEstado _dal_glActividadEstado = new DAL_glActividadEstado(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DAL_Alarmas alarmasDAL = (DAL_Alarmas)this.GetInstance(typeof(DAL_Alarmas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glActividadFlujo actPadre = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actividadPadreID, true, false);
                bool finaliza = false;

                #region Guarda en la bitacora de documentos (glActividadControl)

                DTO_glActividadControl actCtrl = new DTO_glActividadControl();
                actCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                actCtrl.NumeroDoc.Value = numeroDoc;
                actCtrl.DocumentoID.Value = documentoID;
                actCtrl.ActividadFlujoID.Value = actividadPadreID;
                actCtrl.UsuarioID.Value = this.UserId;
                actCtrl.Fecha.Value = DateTime.Now;
                actCtrl.Observacion.Value = obs;
                actCtrl.AlarmaInd.Value = false;

                this.AgregarActividadControl(actCtrl);

                #endregion
                #region Trae la lista de actividades hijas

                //Filtro
                DTO_glConsulta q = new DTO_glConsulta();
                List<DTO_glConsultaFiltro> filtros = new List<DTO_glConsultaFiltro>();
                DTO_glConsultaFiltro filtro = new DTO_glConsultaFiltro()
                {
                    CampoFisico = "ActividadPadre",
                    ValorFiltro = actividadPadreID,
                    OperadorFiltro = OperadorFiltro.Igual,
                    OperadorSentencia = "OR"
                };
                filtros.Add(filtro);
                q.Filtros = filtros;

                //Trae la lista actividades del flujo
                DAL_MasterComplex complexDAL = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                complexDAL.DocumentID = AppMasters.glProcedimientoFlujo;

                long count = complexDAL.DAL_MasterComplex_Count(q, true);
                List<DTO_MasterComplex> list = complexDAL.DAL_MasterComplex_GetPaged(count, 1, q, true).ToList();
                List<DTO_glProcedimientoFlujo> nuevasAct = list.Cast<DTO_glProcedimientoFlujo>().ToList();

                #endregion
                #region Revisa si finaliza el proceso
               
                if (actPadre.FinalizaProcesoInd.Value.Value)
                {
                    // Se pone para saber que termina una tarea y poder matar un flujo a las malas (cxp, comp manual y doc contable) 
                    result.ResultMessage = true.ToString(); 

                    finaliza = true;
                    bool countAct = _dal_glActividadEstado.DAL_glActividadEstado_TieneActividadesPendientes(numeroDoc, actividadPadreID);
                    if (countAct)
                    {
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Gl_ProcessWithActivities;

                        return result;
                    }
                }

                //Desactiva la alarma
                _dal_glActividadEstado.DAL_glActividadEstado_UpdateAlarmStatus(numeroDoc, actividadPadreID, false, null);

                #endregion
                #region Revisa si rechazando el proceso
                if (rechazoInd)
                {
                    // Se pone para saber que termina una tarea y poder matar un flujo a las malas (cxp, comp manual y doc contable) 
                    result.ResultMessage = true.ToString();

                    //Desactiva la alarma
                    _dal_glActividadEstado.DAL_glActividadEstado_UpdateAlarmStatus(numeroDoc, actividadPadreID, false, null);
                }

                #endregion
                if (actividadFuerza == null)
                {
                    #region Asigna las alarmas de las hijas
                    foreach (DTO_glProcedimientoFlujo f in nuevasAct)
                    {
                        bool asignarAlarma = true;
                        DTO_glActividadFlujo actDTO = new DTO_glActividadFlujo();
                        DTO_glActividadEstado actEstado = new DTO_glActividadEstado();
                        #region Revisa si se esta rechazando una actividad
                        if (rechazoInd)
                        {
                            if (!string.IsNullOrWhiteSpace(f.ActividadHijaRechazo.Value) && actividadPadreID != f.ActividadHijaRechazo.Value)
                                actDTO = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, f.ActividadHijaRechazo.Value, true, false);
                            else
                                asignarAlarma = false;
                        }
                        else
                        {
                            if (finaliza)
                                asignarAlarma = false;
                            else
                                actDTO = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, f.ActividadHija.Value, true, false);
                        }

                        #endregion

                        if (asignarAlarma)
                        {
                            bool hasAlarm = _dal_glActividadEstado.DAL_glActividadEstado_HasAlarm(numeroDoc, actDTO.ID.Value);

                            #region Asigna las alarmas
                            UnidadTiempo un = (UnidadTiempo)Enum.Parse(typeof(UnidadTiempo), actDTO.UnidadTiempo.Value.Value.ToString());

                            //Alarma 1
                            if (actDTO.Alarma1Ind.Value.Value && un != UnidadTiempo.NoAplica)
                                actEstado.FechaAlarma1.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo1.Value.Value);
                            else
                                actEstado.FechaAlarma1.Value = null;

                            //Alarma 2
                            if (actDTO.Alarma2Ind.Value.Value && un != UnidadTiempo.NoAplica)
                                actEstado.FechaAlarma2.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo2.Value.Value);
                            else
                                actEstado.FechaAlarma2.Value = null;

                            //Alarma 3
                            if (actDTO.Alarma3Ind.Value.Value && un != UnidadTiempo.NoAplica)
                                actEstado.FechaAlarma3.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo3.Value.Value);
                            else
                                actEstado.FechaAlarma3.Value = null;

                            #endregion
                            if (hasAlarm)
                            {
                                #region habilita una alarma una alarma (glActividadEstado)
                                bool enabled = actividadPadreID.Trim() != actDTO.ID.Value.Trim() ? true : false;
                                _dal_glActividadEstado.DAL_glActividadEstado_UpdateAlarmStatus(numeroDoc, actDTO.ID.Value, enabled, actEstado);
                                #endregion
                            }
                            else
                            {
                                #region Crea una alarma (glActividadEstado)

                                actEstado.NumeroDoc.Value = numeroDoc;
                                actEstado.ActividadFlujoID.Value = actDTO.ID.Value;
                                actEstado.seUsuarioID.Value = this.UserId;
                                actEstado.AlarmaInd.Value = true;
                                actEstado.FechaInicio.Value = DateTime.Now;
                                actEstado.UsuarioAlarma1.Value = string.Empty;
                                actEstado.UsuarioAlarma2.Value = string.Empty;
                                actEstado.UsuarioAlarma3.Value = string.Empty;
                                actEstado.Observaciones.Value = string.Empty;
                                actEstado.CerradoInd.Value = false;

                                _dal_glActividadEstado.DAL_glActividadEstado_Add(actEstado);
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    bool asignarAlarma = true;
                    DTO_glActividadFlujo actDTO = new DTO_glActividadFlujo();
                    DTO_glActividadEstado actEstado = new DTO_glActividadEstado();

                    actDTO = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actividadFuerza, true, false);
                    if (asignarAlarma)
                    {
                        bool hasAlarm = _dal_glActividadEstado.DAL_glActividadEstado_HasAlarm(numeroDoc, actDTO.ID.Value);

                        #region Asigna las alarmas
                        UnidadTiempo un = (UnidadTiempo)Enum.Parse(typeof(UnidadTiempo), actDTO.UnidadTiempo.Value.Value.ToString());

                        //Alarma 1
                        if (actDTO.Alarma1Ind.Value.Value && un != UnidadTiempo.NoAplica)
                            actEstado.FechaAlarma1.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo1.Value.Value);
                        else
                            actEstado.FechaAlarma1.Value = null;

                        //Alarma 2
                        if (actDTO.Alarma2Ind.Value.Value && un != UnidadTiempo.NoAplica)
                            actEstado.FechaAlarma2.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo2.Value.Value);
                        else
                            actEstado.FechaAlarma2.Value = null;

                        //Alarma 3
                        if (actDTO.Alarma3Ind.Value.Value && un != UnidadTiempo.NoAplica)
                            actEstado.FechaAlarma3.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo3.Value.Value);
                        else
                            actEstado.FechaAlarma3.Value = null;

                        #endregion
                        if (hasAlarm)
                        {
                            #region habilita una alarma una alarma (glActividadEstado)
                            bool enabled = actividadPadreID.Trim() != actDTO.ID.Value.Trim() ? true : false;
                            _dal_glActividadEstado.DAL_glActividadEstado_UpdateAlarmStatus(numeroDoc, actDTO.ID.Value, enabled, actEstado);
                            #endregion
                        }
                        else
                        {
                            #region Crea una alarma (glActividadEstado)

                            actEstado.NumeroDoc.Value = numeroDoc;
                            actEstado.ActividadFlujoID.Value = actDTO.ID.Value;
                            actEstado.seUsuarioID.Value = this.UserId;
                            actEstado.AlarmaInd.Value = true;
                            actEstado.FechaInicio.Value = DateTime.Now;
                            actEstado.UsuarioAlarma1.Value = string.Empty;
                            actEstado.UsuarioAlarma2.Value = string.Empty;
                            actEstado.UsuarioAlarma3.Value = string.Empty;
                            actEstado.Observaciones.Value = string.Empty;
                            actEstado.CerradoInd.Value = false;

                            _dal_glActividadEstado.DAL_glActividadEstado_Add(actEstado);
                            #endregion
                        }
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "AsignarFlujo");
                throw exception;
            }
        }

        /// <summary>
        /// Revierte el flujo a la actividad especificada
        /// </summary>
        /// <param name="documentoID">Documento que esta ejecutando la transaccion</param>
        /// <param name="numeroDoc">Numero de documento que ejecuta el procedimiento</param>
        /// <param name="actividadActual">Actividad en la cual se esta realizando la reversion</param>
        /// <param name="actividadReversion">Actividad a la cual se desea revertir el flujo</param>
        /// <param name="obs">Observaciones</param>
        /// <returns>Retorna true si esa actividad termina con el proceso</returns>
        internal DTO_TxResult ActualizarReversionFlujo(int documentoID, int numeroDoc, string actividadActual, string actividadReversion, string obs)
        {
            DTO_TxResult result = new DTO_TxResult();
            result.Result = ResultValue.OK;
            result.ResultMessage = false.ToString();

            try
            {
                DAL_glActividadEstado _dal_glActividadEstado = new DAL_glActividadEstado(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DAL_Alarmas alarmasDAL = (DAL_Alarmas)this.GetInstance(typeof(DAL_Alarmas), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glActividadFlujo actActual = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actividadActual, true, false);
                DTO_glActividadFlujo actReversion = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actividadReversion, true, false);

                #region Guarda en la bitacora de documentos (glActividadControl)
                DTO_glActividadControl actCtrl = new DTO_glActividadControl();
                actCtrl.EmpresaID.Value = this.Empresa.ID.Value;
                actCtrl.NumeroDoc.Value = numeroDoc;
                actCtrl.DocumentoID.Value = documentoID;
                actCtrl.ActividadFlujoID.Value = actividadActual;
                actCtrl.UsuarioID.Value = this.UserId;
                actCtrl.Fecha.Value = DateTime.Now;
                actCtrl.Observacion.Value = obs;
                actCtrl.AlarmaInd.Value = false;

                this.AgregarActividadControl(actCtrl);
                #endregion
                #region Desactiva la alarma de la actividad actual
                _dal_glActividadEstado.DAL_glActividadEstado_UpdateAlarmStatus(numeroDoc, actividadActual, false, null);
                #endregion
                #region Asigna la alarma de la actividad a revertir
                DTO_glActividadEstado actEstado = new DTO_glActividadEstado();
                bool hasAlarm = _dal_glActividadEstado.DAL_glActividadEstado_HasAlarm(numeroDoc, actReversion.ID.Value);                
                if (hasAlarm)
                {
                    #region Asigna las alarmas
                    UnidadTiempo un = (UnidadTiempo)Enum.Parse(typeof(UnidadTiempo), actReversion.UnidadTiempo.Value.Value.ToString());

                    //Alarma 1
                    if (actReversion.Alarma1Ind.Value.Value && un != UnidadTiempo.NoAplica)
                        actEstado.FechaAlarma1.Value = this.CalculateAlarmDate(un, actReversion.AlarmaPeriodo1.Value.Value);
                    else
                        actEstado.FechaAlarma1.Value = null;

                    //Alarma 2
                    if (actReversion.Alarma2Ind.Value.Value && un != UnidadTiempo.NoAplica)
                        actEstado.FechaAlarma2.Value = this.CalculateAlarmDate(un, actReversion.AlarmaPeriodo2.Value.Value);
                    else
                        actEstado.FechaAlarma2.Value = null;

                    //Alarma 3
                    if (actReversion.Alarma3Ind.Value.Value && un != UnidadTiempo.NoAplica)
                        actEstado.FechaAlarma3.Value = this.CalculateAlarmDate(un, actReversion.AlarmaPeriodo3.Value.Value);
                    else
                        actEstado.FechaAlarma3.Value = null;

                    #endregion
                    //Actualiza el estado de la actividad a la cual se va a revertir
                    _dal_glActividadEstado.DAL_glActividadEstado_UpdateAlarmStatus(numeroDoc, actReversion.ID.Value, true, actEstado);
                }
                else
                {
                    #region Crea una alarma (glActividadEstado)
                    actEstado.NumeroDoc.Value = numeroDoc;
                    actEstado.ActividadFlujoID.Value = actReversion.ID.Value;
                    actEstado.seUsuarioID.Value = this.UserId;
                    actEstado.AlarmaInd.Value = true;
                    actEstado.FechaInicio.Value = DateTime.Now;
                    actEstado.UsuarioAlarma1.Value = string.Empty;
                    actEstado.UsuarioAlarma2.Value = string.Empty;
                    actEstado.UsuarioAlarma3.Value = string.Empty;
                    actEstado.Observaciones.Value = string.Empty;
                    actEstado.CerradoInd.Value = false;
                    _dal_glActividadEstado.DAL_glActividadEstado_Add(actEstado);
                    #endregion
                }
                #endregion
                return result;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_UpdateData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this.UserId.ToString(), "AsignarFlujo");
                throw exception;
            }
        }

        /// <summary>
        /// Elimina un registro
        /// </summary>
        /// <param name="numeroDoc">Identificador unico del documento</param>
        /// <param name="actividadFlujoID">Identificador de la actividad</param>
        internal void AsignarAlarma(int numeroDoc, string actividadFlujoID, bool alarmaInd)
        {
            try
            {
                DAL_glActividadEstado _dal_glActividadEstado = new DAL_glActividadEstado(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                DTO_glActividadEstado actEstado = new DTO_glActividadEstado();
                DTO_glActividadFlujo actDTO = null;
                #region Asigna las alarmas
                if (!string.IsNullOrWhiteSpace(actividadFlujoID))
                {
                    actDTO = (DTO_glActividadFlujo)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glActividadFlujo, actividadFlujoID, true, false);
                    UnidadTiempo un = (UnidadTiempo)Enum.Parse(typeof(UnidadTiempo), actDTO.UnidadTiempo.Value.Value.ToString());

                    //Alarma 1
                    if (actDTO.Alarma1Ind.Value.Value && un != UnidadTiempo.NoAplica)
                        actEstado.FechaAlarma1.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo1.Value.Value);
                    else
                        actEstado.FechaAlarma1.Value = null;

                    //Alarma 2
                    if (actDTO.Alarma2Ind.Value.Value && un != UnidadTiempo.NoAplica)
                        actEstado.FechaAlarma2.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo2.Value.Value);
                    else
                        actEstado.FechaAlarma2.Value = null;

                    //Alarma 3
                    if (actDTO.Alarma3Ind.Value.Value && un != UnidadTiempo.NoAplica)
                        actEstado.FechaAlarma3.Value = this.CalculateAlarmDate(un, actDTO.AlarmaPeriodo3.Value.Value);
                    else
                        actEstado.FechaAlarma3.Value = null;
                }
                #endregion

                _dal_glActividadEstado.DAL_glActividadEstado_UpdateAlarmStatus(numeroDoc, actividadFlujoID, alarmaInd, actEstado);
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "AsignarAlarma");
                throw ex;
            }
        }

        /// <summary>
        /// Elimina un registro
        /// </summary>
        /// <param name="numeroDoc">Identificador unico del documento</param>
        /// <param name="actividadFlujoID">Identificador de la actividad</param>
        internal void DeshabilitarAlarma(int numeroDoc, string actividadFlujoID)
        {
            DAL_glActividadEstado _dal_glActividadEstado = new DAL_glActividadEstado(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            _dal_glActividadEstado.DAL_glActividadEstado_UpdateAlarmStatus(numeroDoc, actividadFlujoID, false, null);
        }

        /// <summary>
        /// Trae el libro por defecto de un comprobante
        /// </summary>
        /// <param name="comprobanteID"></param>
        /// <returns></returns>
        internal string GetLibroComprobante(DTO_ComprobanteHeader header)
        {
            try
            {
                string libroID = header.LibroID.Value;
                if (string.IsNullOrWhiteSpace(libroID))
                {
                    DTO_coComprobante comp = (DTO_coComprobante)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coComprobante, header.ComprobanteID.Value, true, false);
                    libroID = comp.BalanceTipoID.Value;
                }

                return libroID;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "GetLibroXDefecto");
                throw ex;
            }
        }
        #endregion

        #region Impuestos

        /// <summary>
        /// Llena la informacion de las cuentas de impuestos
        /// </summary>
        /// <param name="reteFteInd">Indicador si el tercero tiene excepcion para retefuente</param>
        /// <param name="ctaDTO">Cuenta</param>
        /// <param name="valor">Valor (base)</param>
        /// <param name="lg">Lugar geografico</param>
        /// <param name="impReteFte">Identificador del impuesto de retefuente</param>
        /// <param name="periodo">periodo de consulta</param>
        /// <returns>Retorna el impuesto</returns>
        internal DTO_SerializedObject Impuesto_LlenaInfoCuenta(DTO_coPlanCuenta ctaDTO, decimal valor, string lg, DateTime periodo, bool reteFteInd, string impReteFte, List<DTO_noReteFuenteBasica> reteFtes)
        {
            try
            {
                DTO_SerializedObject impuesto = new DTO_SerializedObject();

                bool updVal = valor >= 0 ? false : true;

                string impIVA = this.GetControlValueByCompany(ModulesPrefix.cp, AppControl.cp_CodigoIVA);
                NaturalezaCuenta nat = (NaturalezaCuenta)Enum.Parse(typeof(NaturalezaCuenta), ctaDTO.Naturaleza.Value.Value.ToString());

                decimal vlrCta = 0;
                decimal vlrBase = 0;

                vlrBase = valor;

                if (reteFteInd && ctaDTO.ImpuestoTipoID.Value == impReteFte)
                {
                    #region Excepcion para retefuente
                    DTO_glDatosAnuales datos = (DTO_glDatosAnuales)this.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.glDatosAnuales, periodo.Date.Year.ToString(), true, false);
                    if (datos == null)
                    {
                        DTO_TxResult result = new DTO_TxResult();
                        result.Result = ResultValue.NOK;
                        result.ResultMessage = DictionaryMessages.Err_Co_NoDatosAnuales;
                        return result;
                    }

                    decimal vlrUVT = datos.Valor1.Value.Value;
                    decimal numUVT = valor / vlrUVT;
                    int nBase = 0;
                    int nAdd = 0;
                    decimal nPorc = 0;

                    #region Saca la info de noReteFuenteBasica
                    int i = 0;
                    DTO_noReteFuenteBasica dto;
                    for (i = 0; i < reteFtes.Count; ++i)
                    {
                        dto = reteFtes[i];
                        nBase = Convert.ToInt32(dto.BaseUVT.Value);//Probar conversion con puntos y comas
                        if (numUVT < nBase)
                        {
                            nAdd = dto.AdicionUVT.Value.Value;
                            nPorc = dto.PorcentajeID.Value.Value;

                            break;
                        }
                    }

                    dto = i == 0 ? reteFtes[i] : reteFtes[i - 1];
                    nBase = i == 0 ? 0 : dto.AdicionUVT.Value.Value;
                    #endregion

                    vlrCta = (vlrUVT - nBase) * nPorc / 100;
                    #endregion
                }
                else
                {
                    #region Carga la base y el valor
                    if (ctaDTO.ImpuestoPorc != null && ctaDTO.ImpuestoPorc.Value.HasValue && valor >= ctaDTO.MontoMinimo.Value)
                        vlrCta = valor * ctaDTO.ImpuestoPorc.Value.Value / 100;
                    else
                        vlrCta = 0;
                    #endregion
                }

                #region Revisa la naturaleza y el signo del valor
                if (valor < 0)
                {
                    vlrCta *= -1;
                    vlrBase *= -1;
                }

                if (nat == NaturalezaCuenta.Credito)
                    vlrCta *= -1;
                #endregion
                #region carga el DTO
                DTO_CuentaValor ctaVal = new DTO_CuentaValor(ctaDTO.ID.Value, vlrCta, vlrBase, lg, ctaDTO.ImpuestoTipoID.Value);

                if (ctaDTO.ImpuestoTipoID.Value == impIVA)
                    ctaVal.TarifaCosto = ctaDTO.ImpuestoPorc.Value.Value;

                #endregion

                return ctaVal;
            }
            catch (Exception ex)
            {
                DTO_TxResult result = new DTO_TxResult();
                result.Result = ResultValue.NOK;
                result.ResultMessage = Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this.UserId.ToString(), "Impuesto_LlenaInfoCuenta");

                return result;
            }
        }

        #endregion

        #region Maestras

        /// <summary>
        /// Obtiene le el objeto de la maestra que se este consultado de acuerdo al AppMaster y al valor
        /// </summary>
        /// <param name="masterID">id de la maestra</param>
        /// <param name="masterValue">valor</param>
        /// <param name="isActivo">si esta activa</param>
        /// <param name="isInt">si el Id es entero</param>
        /// <returns>objeto DTO_MastarSimple</returns>
        public Object GetMasterDTO(AppMasters.MasterType type, int masterID, string masterValue, bool isActivo, bool isInt, List<DTO_glConsultaFiltro> filtros = null)
        {
            Object maestra = null;
            UDT_BasicID basic = new UDT_BasicID(true) { Value = masterValue, IsInt = isInt };

            if (type == AppMasters.MasterType.Simple)
            {
                DAL_MasterSimple simpleDAL = (DAL_MasterSimple)this.GetInstance(typeof(NewAge.ADO.DAL_MasterSimple), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                simpleDAL.DocumentID = masterID;
                maestra = simpleDAL.DAL_MasterSimple_GetByID(basic, isActivo, filtros);
            }
            else if (type == AppMasters.MasterType.Hierarchy)
            {
                DAL_MasterHierarchy hierarchyDAL = (DAL_MasterHierarchy)this.GetInstance(typeof(NewAge.ADO.DAL_MasterHierarchy), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
                hierarchyDAL.DocumentID = masterID;
                maestra = hierarchyDAL.DAL_MasterSimple_GetByID(basic, isActivo, filtros);
            }
            return maestra;
        }

        /// <summary>
        /// Retorna un tipo de dato complejo
        /// </summary>
        /// <param name="docId">documentoID</param>
        /// <param name="pks">diccionario de llaves primarias</param>
        /// <param name="active"></param>
        /// <returns></returns>
        public DTO_MasterComplex GetMasterComplexDTO(int docId, Dictionary<string, string> pks, bool active)
        {
            DAL_MasterComplex dalMasterComplex = new DAL_MasterComplex(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            dalMasterComplex.DocumentID = docId;

            return dalMasterComplex.DAL_MasterComplex_GetByID(pks, active);
        }

        /// <summary>
        /// Trae el grupo de empresas de acuerdo al un documento
        /// </summary>
        /// <param name="documentID">Documento</param>
        /// <returns>Retorna el grupo de empresas</returns>
        internal string GetMaestraEmpresaGrupoByDocumentID(int documentID, DTO_glEmpresa emp, string egControl)
        {
            DTO_aplMaestraPropiedades prop = StaticMethods.GetParameters(this._mySqlConnection, this._mySqlConnectionTx, documentID, this.loggerConnectionStr);
            GrupoEmpresa seguridad = prop.TipoSeguridad;

            string empGrupo = string.Empty;
            if (seguridad == GrupoEmpresa.Automatico)
            {
                empGrupo = emp.ID.Value;
            }
            else if (seguridad == GrupoEmpresa.Individual)
            {
                empGrupo = emp.EmpresaGrupoID_.Value;
            }
            else
            {
                empGrupo = egControl;
            }

            return empGrupo;
        }

        #endregion

        #region Recursos

        /// <summary>
        /// Funcion para obtener los recursos
        /// </summary>
        /// <param name="t">Tipo Lenguaje</param>
        /// <param name="v"></param>
        /// <returns>Recurso</returns>
        public string GetResource(LanguageTypes t, string v)
        {
            this._dal_glEmpresa = (DAL_glEmpresa)this.GetInstance(typeof(DAL_glEmpresa), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            this._dal_seUsuario = (DAL_seUsuario)this.GetInstance(typeof(DAL_seUsuario), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DAL_aplIdiomaTraduccion dal = (DAL_aplIdiomaTraduccion)this.GetInstance(typeof(DAL_aplIdiomaTraduccion), this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);
            DTO_seUsuario user = this._dal_seUsuario.DAL_seUsuario_GetUserByReplicaID(this.UserId);

            string idiomaID = user.IdiomaID.Value;
            dal = new DAL_aplIdiomaTraduccion(this._mySqlConnection, this._mySqlConnectionTx, this.Empresa, this.UserId, this.loggerConnectionStr);

            Dictionary<string, string> d;
            if (rsx.TryGetValue(t, out d))
            {
                string res = "";
                if (d.TryGetValue(v, out res))
                {
                    return res;
                }
                else
                {
                    Dictionary<string, string> temp = dal.DAL_aplIdiomaTraduccion_GetRsxByKeysDict(idiomaID, t, new List<string>() { v });
                    if (temp.TryGetValue(v, out res))
                    {
                        d.Add(v, res);
                        return res;
                    }
                }
            }
            else
            {
                d = dal.DAL_aplIdiomaTraduccion_GetRsxByKeysDict(idiomaID, t, new List<string>() { v });
                rsx.Add(t, d);
                string res = "";
                if (d.TryGetValue(v, out res))
                {
                    return res;
                }
            }
            return v;
        }

        #endregion

        #endregion

    }
}
