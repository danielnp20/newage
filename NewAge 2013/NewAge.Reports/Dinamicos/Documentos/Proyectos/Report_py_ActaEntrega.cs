using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Linq;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_py_ActaEntrega : XtraReport
    {
        #region Variables

        private DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();
        protected ModuloGlobal _moduloGlobal;
        protected ModuloProyectos _moduloProyecto;
        protected ModuloContabilidad _moduloContab;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected DTO_glEmpresa _empresa;
        protected int _userID;
        protected ModuloBase _moduloBase;
        protected ExportFormatType _formatType;
        protected int? numeroDoc;
        protected string loggerConnectionStr;
        protected Image logoFactura = null;
        protected ReportProvider reportProvider;
        #endregion

        #region Propiedades

        /// <summary>
        /// Empresa
        /// </summary>
        internal DTO_glEmpresa Empresa
        {
            get { return this._empresa; }
            set { this._empresa = value; }
        }

        /// <summary>
        /// Nombre del Reporte
        /// </summary>
        public string ReportName
        {
            get;
            set;
        }

        /// <summary>
        /// Ruta del reporte
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        #endregion

        public Report_py_ActaEntrega()
        {

        }

        public Report_py_ActaEntrega(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_py_ActaEntrega(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc = null)
        {
            this.InitializeComponent();

            this._connection = c;
            this._transaction = tx;
            this._empresa = empresa;
            this._userID = userId;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.numeroDoc = numDoc;
            this.SetInitParameters();
        }
        
        #region Funciones publicas
	    /// <summary>
        /// Inicia las variables básicas para el reporte del usuario
        /// </summary>
        /// <param name="conn">conexion a base datos</param>
        /// <param name="tx">transaccion</param>
        /// <param name="emp">empresa</param>
        /// <param name="userID">identificador del usuario</param>
        public void InitUserReport(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
        {
            this._connection = conn;
            this._transaction = tx;
            this._empresa = emp;
            this._userID = userID;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.numeroDoc = numDoc;
            this.SetUserParameters();
        }
        
        /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        public void SetUserParameters()
        {
            //this.InitializeComponent();
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Nombre del reporte

            string repName;
            string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);

            if (this.numeroDoc.HasValue)
            {
                // Reporte de documento
                string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
                repName = string.Format(fileFormat, this.numeroDoc.ToString());
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
            }
            else
            {
                // Reporte temporal
                repName = Guid.NewGuid().ToString();
                docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            }

            this.ReportName = repName.ToString() + ".pdf";
            this.Path = filesPath + docsPath + ReportName;

            #endregion
            #region Header

            //this.lblNombreEmpresa.Text = this.Empresa.Descriptivo.Value;
            //this.lblUserName.Text = usuario.Descriptivo.Value;

            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
               // this.Image = logoImage;
            }
            catch { ; }

            #endregion
            #region Recusos
            this.lblPage.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblPage");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblDate");
            this.lblUser.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblUser");
            #endregion
        }

         /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        public virtual void SetInitParameters()
        {
            //this.InitializeComponent();
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Nombre del reporte

            //string repName;
            //string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            //string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            //string ext = this.GetExtention();
            
            //if (this.numeroDoc.HasValue)
            //{
            //    // Reporte de documento
            //    string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
            //    repName = string.Format(fileFormat, this.numeroDoc.ToString());
            //    docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
            //}
            //else
            //{
            //    // Reporte temporal
            //    repName = Guid.NewGuid().ToString();
            //    docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            //}

            //this.ReportName = repName.ToString() + ext;
            //this.Path = filesPath + docsPath + ReportName;

            #endregion
            #region Header

            this.lblNombreEmpresa.Text = this.Empresa.Descriptivo.Value;
            this.lblUserName.Text = usuario.Descriptivo.Value;

            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
               // this.imgLogoEmpresa.Image = logoImage;
            }
            catch (Exception ex) 
            { }

            #endregion
            #region Recusos
            this.lblPage.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblPage");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblDate");
            this.lblUser.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblUser");

            this.reportProvider = new ReportProvider(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this.reportProvider.LoadResources(this.AllControls<XRControl>());

            #endregion
        }

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DTO_SolicitudTrabajo solicitud,int actaNroActual, DateTime fechaActa)
        {
            try
            {
                this._moduloProyecto = new ModuloProyectos(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                ModuloFacturacion moduloFact= new ModuloFacturacion(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);

                List<DTO_SolicitudTrabajo> list = new List<DTO_SolicitudTrabajo>();
                this.solicitud = solicitud;
                List<DTO_pyPreProyectoTarea> listTareasCotiz = this._moduloProyecto.pyPreProyectoTarea_Get(this.solicitud.HeaderProyecto.DocSolicitud.Value, string.Empty, string.Empty);

                #region Asigna Valores Generales
                this.lblFechaActa.Text = fechaActa.ToShortDateString();
                this.lblTerceroIDEmpresa.Text  = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co,AppControl.co_TerceroXDefecto);
                string servicioCtaCobro = this._moduloBase.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_ServicioCtaCobro);
                string tipoFacturaCtaCobro = this._moduloBase.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_TipoFacturaCtaCobro);

                decimal iva = (this.solicitud.HeaderProyecto.PorIVA.Value.Value / 100);
                DTO_faCliente cliente = (DTO_faCliente)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, this.solicitud.HeaderProyecto.ClienteID.Value, true, false);
                this.solicitud.HeaderProyecto.ClienteDesc.Value = cliente != null ? cliente.Descriptivo.Value : this.solicitud.HeaderProyecto.EmpresaNombre.Value;

                this.solicitud.HeaderProyecto.ValorCliente.Value = this.solicitud.DetalleProyecto.Sum(x=>x.CostoLocalCLI.Value);
                this.solicitud.HeaderProyecto.ValorIVA.Value = this.solicitud.HeaderProyecto.ValorCliente.Value * iva;
                this.solicitud.HeaderProyecto.ValorIVA.Value = Math.Round(this.solicitud.HeaderProyecto.ValorIVA.Value.Value,0);
                this.solicitud.HeaderProyecto.ValorOtros.Value = 0;
                this.solicitud.HeaderProyecto.Valor.Value = Math.Round(this.solicitud.HeaderProyecto.ValorCliente.Value.Value + this.solicitud.HeaderProyecto.ValorIVA.Value.Value + this.solicitud.HeaderProyecto.ValorOtros.Value.Value,0);

                DTO_seUsuario user = (DTO_seUsuario)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.seUsuario, this.solicitud.HeaderProyecto.ResponsableEMP.Value, true, false);
                this.solicitud.CorreoUsuario.Value = user != null ? user.CorreoElectronico.Value : string.Empty;
                this.solicitud.TelefonoUsuario.Value = user != null ? user.Telefono.Value : string.Empty;
                #endregion
                #region Asigna Otros valores segun Tipo Presupuesto
                DTO_pyClaseProyecto claseproy = (DTO_pyClaseProyecto)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, this.solicitud.HeaderProyecto.ClaseServicioID.Value, true, false);
                if (claseproy != null && claseproy.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) // Interno
                {
                    string marcaxDef = this._moduloBase.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_MarcaxDef);
                    this.solicitud.DetalleProyecto = this.solicitud.DetalleProyecto.OrderBy(x => x.CapituloTareaID.Value).ToList(); //Ordena por CapituloID
                    int count = 1;
                    foreach (var tarea in this.solicitud.DetalleProyecto)
                    {
                        DTO_inReferencia refer = (DTO_inReferencia)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inReferencia, tarea.TareaID.Value, true, false);
                        DTO_MasterBasic marca =  (DTO_MasterBasic)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.inMarca, refer != null ? refer.MarcaInvID.Value : string.Empty, true, false);
                        tarea.RefProveedor.Value = refer != null ? refer.RefProveedor.Value : string.Empty;
                        if (refer != null && !refer.MarcaInvID.Value.Equals(marcaxDef))
                            tarea.MarcaInvID.Value = marca != null ? marca.Descriptivo.Value : string.Empty;
                        tarea.TareaCliente.Value = count.ToString();
                        count++;
                    }
                }
                #endregion

                DTO_pyActaEntregaDeta filterActa = new DTO_pyActaEntregaDeta();
                filterActa.NumDocProyecto.Value = solicitud.DocCtrl.NumeroDoc.Value;
                List<DTO_pyActaEntregaDeta> actas = this._moduloProyecto.pyActaEntregaDeta_GetByParameter(filterActa);
                actas = actas.OrderByDescending(x => x.DocumentoNro.Value).ToList();

                int actUltimaNoAprobada = actas.Count > 0? actas.FirstOrDefault().ConsActaProy.Value.Value : 0;
                actUltimaNoAprobada = !actas.Exists(x => x.Estado.Value == 1 || x.Estado.Value == 2) ? actUltimaNoAprobada + 1 : actUltimaNoAprobada;
                this.lblReporteNombre.Text = "Acta Entrega No." + (actUltimaNoAprobada == actaNroActual? actUltimaNoAprobada.ToString() : actaNroActual.ToString());
                foreach (DTO_pyProyectoTarea tar in solicitud.DetalleProyecto)
                {
                    if (actUltimaNoAprobada == actaNroActual)
                    {
                        tar.CantActaAnterior.Value = actas.Where(x => x.ConsActaProy.Value != actUltimaNoAprobada && x.TareaEntregable.Value == tar.TareaEntregable.Value && x.Estado.Value == 3).Sum(x => x.Cantidad.Value);
                        tar.VlrActaAnterior.Value = actas.Where(x => x.ConsActaProy.Value != actUltimaNoAprobada && x.TareaEntregable.Value == tar.TareaEntregable.Value && x.Estado.Value == 3).Sum(x => x.ValorFactura.Value);
                        tar.CantActaActual.Value = actas.Where(x => x.ConsActaProy.Value == actUltimaNoAprobada && x.TareaEntregable.Value == tar.TareaEntregable.Value && x.Estado.Value != 3).Sum(x => x.Cantidad.Value);
                        tar.VlrActaActual.Value = actas.Where(x => x.ConsActaProy.Value == actUltimaNoAprobada && x.TareaEntregable.Value == tar.TareaEntregable.Value && x.Estado.Value != 3).Sum(x => x.ValorFactura.Value);
                        tar.CantActaAcumulado.Value = tar.CantActaAnterior.Value + tar.CantActaActual.Value;
                        tar.VlrActaAcumulado.Value = tar.VlrActaAnterior.Value + tar.VlrActaActual.Value;
                        tar.PorcentajeAvance.Value = actas.Where(x => x.TareaEntregable.Value == tar.TareaEntregable.Value).Sum(x => x.PorEntregado.Value);
                    }
                    else
                    {
                        tar.CantActaAnterior.Value = actas.Where(x => x.ConsActaProy.Value < actaNroActual && x.TareaEntregable.Value == tar.TareaEntregable.Value && x.Estado.Value == 3).Sum(x => x.Cantidad.Value);
                        tar.VlrActaAnterior.Value = actas.Where(x => x.ConsActaProy.Value < actaNroActual && x.TareaEntregable.Value == tar.TareaEntregable.Value && x.Estado.Value == 3).Sum(x => x.ValorFactura.Value);
                        tar.CantActaActual.Value = actas.Where(x => x.ConsActaProy.Value == actaNroActual && x.TareaEntregable.Value == tar.TareaEntregable.Value ).Sum(x => x.Cantidad.Value);
                        tar.VlrActaActual.Value = actas.Where(x => x.ConsActaProy.Value == actaNroActual && x.TareaEntregable.Value == tar.TareaEntregable.Value).Sum(x => x.ValorFactura.Value);
                        tar.CantActaAcumulado.Value = tar.CantActaAnterior.Value + tar.CantActaActual.Value;
                        tar.VlrActaAcumulado.Value = tar.VlrActaAnterior.Value + tar.VlrActaActual.Value;
                        tar.PorcentajeAvance.Value = actas.Where(x => x.TareaEntregable.Value == tar.TareaEntregable.Value).Sum(x => x.PorEntregado.Value);

                    }
                    tar.CostoAdicionalInd.Value = false;
                    if (listTareasCotiz.Any(x => x.TareaID.Value == tar.TareaID.Value && x.Descriptivo.Value == tar.Descriptivo.Value))
                    {
                        tar.Cantidad.Value = listTareasCotiz.Find(x => x.TareaID.Value == tar.TareaID.Value && x.Descriptivo.Value == tar.Descriptivo.Value).Cantidad.Value;
                        tar.CostoLocalUnitCLI.Value = listTareasCotiz.Find(x => x.TareaID.Value == tar.TareaID.Value && x.Descriptivo.Value == tar.Descriptivo.Value).CostoLocalUnitCLI.Value;
                        tar.CostoLocalCLI.Value = listTareasCotiz.Find(x => x.TareaID.Value == tar.TareaID.Value && x.Descriptivo.Value == tar.Descriptivo.Value).CostoLocalCLI.Value;
                    }
                    else
                    {
                        tar.CantActaOtroSi.Value = tar.Cantidad.Value;
                        tar.VlrActaOtroSi.Value = tar.CostoLocalCLI.Value;
                        tar.Cantidad.Value = 0;
                        tar.CostoLocalCLI.Value = 0;
                        tar.CostoAdicionalInd.Value = true;
                    }
                }

                this.solicitud.HeaderProyecto.VlrIVAActaAnterior = (solicitud.DetalleProyecto.Sum(x => x.VlrActaAnterior.Value) * iva);
                this.solicitud.HeaderProyecto.VlrIVAActaActual = (solicitud.DetalleProyecto.Sum(x => x.VlrActaActual.Value) * iva);
                this.solicitud.HeaderProyecto.VlrIVAActaOtrosSi = (solicitud.DetalleProyecto.Sum(x => x.VlrActaOtroSi.Value) * iva);
                this.solicitud.HeaderProyecto.VlrIVAActaAcumulado = (solicitud.DetalleProyecto.Sum(x => x.VlrActaAcumulado.Value) * iva);
                this.solicitud.HeaderProyecto.TotalActaAnterior = (solicitud.DetalleProyecto.Sum(x => x.VlrActaAnterior.Value)) + this.solicitud.HeaderProyecto.VlrIVAActaAnterior;
                this.solicitud.HeaderProyecto.TotalActaActual = (solicitud.DetalleProyecto.Sum(x => x.VlrActaActual.Value) ) + this.solicitud.HeaderProyecto.VlrIVAActaActual;
                this.solicitud.HeaderProyecto.TotalActaOtroSi = (solicitud.DetalleProyecto.Sum(x => x.VlrActaOtroSi.Value)) + this.solicitud.HeaderProyecto.VlrIVAActaOtrosSi;
                this.solicitud.HeaderProyecto.TotalActaAcumulado = (solicitud.DetalleProyecto.Sum(x => x.VlrActaAcumulado.Value)) + this.solicitud.HeaderProyecto.VlrIVAActaAcumulado;
                this.solicitud.HeaderProyecto.PorcRteGarantia.Value = this.solicitud.HeaderProyecto.PorcRteGarantia.Value ?? 0;
                this.cellPorRteGarantia.Text = this.solicitud.HeaderProyecto.PorcRteGarantia.Value.Value.ToString("n0") + "%";

                #region Trae las Facturas de venta y Amortizaciones del proyecto
                DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();
                filter.DatoAdd4.Value = solicitud.DocCtrl.NumeroDoc.Value.ToString();
                filter.ProyectoID.Value = solicitud.DocCtrl.ProyectoID.Value;
                filter.DocumentoID.Value = AppDocuments.FacturaVenta;
                List<DTO_glMovimientoDeta> mvtos = this._moduloGlobal.glMovimientoDeta_GetByParameter(filter, false);
                mvtos.AddRange(this._moduloGlobal.glMovimientoDeta_GetByParameter(filter, true));
                this.solicitud.HeaderProyecto.VlrAnticipoFactVenta = mvtos.FindAll(x => x.FacturaTipoID.Value == tipoFacturaCtaCobro && x.ServicioID.Value == servicioCtaCobro && !x.DatoAdd5.Value.Equals("INV")).Sum(x => x.Valor1LOC.Value.Value);
                this.solicitud.HeaderProyecto.VlrPorcAnticipo = this.solicitud.HeaderProyecto.ValorCliente.Value != 0 ? (this.solicitud.HeaderProyecto.VlrAnticipoFactVenta * 100 / this.solicitud.HeaderProyecto.ValorCliente.Value) : 0;
                this.solicitud.HeaderProyecto.PorcEjecucionActaAnt = this.solicitud.HeaderProyecto.Valor.Value != 0 ? (this.solicitud.HeaderProyecto.TotalActaAnterior / this.solicitud.HeaderProyecto.Valor.Value) * 100 : 0;
                this.solicitud.HeaderProyecto.VlrAmortizaPresenteActa = (this.solicitud.HeaderProyecto.VlrPorcAnticipo / 100) * solicitud.DetalleProyecto.Sum(x => x.VlrActaActual.Value);
                this.solicitud.HeaderProyecto.VlrRetegarantia = solicitud.DetalleProyecto.Sum(x => x.VlrActaActual.Value) * (solicitud.HeaderProyecto.PorcRteGarantia.Value / 100);
                this.solicitud.HeaderProyecto.SaldoCancelarPresenteActa = solicitud.DetalleProyecto.Sum(x => x.VlrActaActual.Value) - this.solicitud.HeaderProyecto.VlrAmortizaPresenteActa - this.solicitud.HeaderProyecto.VlrRetegarantia;

                mvtos = mvtos.FindAll(x => x.DocSoporte.Value.HasValue && !x.DatoAdd5.Value.Equals("INV"));//Filtra las tareas relacionadas
                decimal vlrAmortizado = 0;
                List<int> facturas = mvtos.Select(x => x.NumeroDoc.Value.Value).Distinct().ToList();
                foreach (int numDoc in facturas)
                {
                    DTO_faFacturaDocu docu = moduloFact.faFacturaDocu_Get(numDoc);
                    docu.Retencion10.Value = docu.Retencion10.Value ?? 0;
                    vlrAmortizado += (docu.Retencion10.Value.Value);
                }
                this.solicitud.HeaderProyecto.VlrAmortizadoFact = vlrAmortizado;
                if (actUltimaNoAprobada == actaNroActual)
                    this.solicitud.HeaderProyecto.SaldoxAmortizar = this.solicitud.HeaderProyecto.VlrAnticipoFactVenta - this.solicitud.HeaderProyecto.VlrAmortizaPresenteActa - vlrAmortizado;
                else
                    this.solicitud.HeaderProyecto.SaldoxAmortizar = this.solicitud.HeaderProyecto.VlrAnticipoFactVenta - vlrAmortizado;
                #endregion
                
                list.Add(solicitud);
                this.DataSource = list;
                #region Crea el reporte
                string repName;
                string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
                string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
                this.numeroDoc = actas.FirstOrDefault().NumeroDoc.Value.Value;
                if (this.numeroDoc.HasValue)
                {
                    // Reporte de documento
                    string fileFormat = this._moduloGlobal.GetControlValue(AppControl.NombreArchivoDocumentos);
                    repName = string.Format(fileFormat, this.numeroDoc.ToString());
                    docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaDocumentos);
                }
                else
                {
                    // Reporte temporal
                    repName = Guid.NewGuid().ToString();
                    docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
                }
                this.ReportName = repName.ToString() + ".pdf";
                this.Path = filesPath + docsPath + ReportName;

                this.ShowPreview();
                //this.CreateReport();
                #endregion
                return this.Path;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this._userID.ToString(), "GenerateReport");
                return this.Path;
            }
        }        

        /// <summary>
        /// Función que exporta de acuerdo al tipo de formato seleccionado por el usuario
        /// </summary>
        private void CreateReport()
        {
            switch (this._formatType)
            {
                case ExportFormatType.pdf:
                    this.ExportToPdf(Path);
                    break;
                case ExportFormatType.xls:
                    this.ExportToXls(Path);
                    break;
                case ExportFormatType.xlsx:
                    this.ExportToXlsx(Path);
                    break;
                case ExportFormatType.html:
                    this.ExportToHtml(Path);
                    break;
            }

            if (this.numeroDoc.HasValue)
                this.ReportName = this.numeroDoc.ToString();
        }

        /// <summary>
        /// Obtiene el saldo del tercero para Anticipos
        /// </summary>
        /// <returns></returns>
        private decimal GetSaldoAnticipo()
        {
            try
            {
                decimal saldoAntic = 0;
                this._moduloContab = new ModuloContabilidad(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                string cuentaAntic = this._moduloBase.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_CuentaAnticiposMdaLocal);
                string libroFunc = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TipoBalanceFuncional);
                DateTime periodoTesor = Convert.ToDateTime(this._moduloBase.GetControlValueByCompany(ModulesPrefix.fa, AppControl.fa_Periodo));
                DTO_faCliente cliente = this.solicitud != null ? (DTO_faCliente)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente,this.solicitud.HeaderProyecto.ClienteID.Value,true, false) : null;

                DTO_coCuentaSaldo saldoFilter = new DTO_coCuentaSaldo();
                saldoFilter.PeriodoID.Value = periodoTesor;
                saldoFilter.BalanceTipoID.Value = libroFunc;
                saldoFilter.CuentaID.Value = cuentaAntic;
                saldoFilter.TerceroID.Value = cliente.TerceroID.Value;
                saldoFilter.ProyectoID.Value = this.solicitud.DocCtrl.ProyectoID.Value;
                if (!string.IsNullOrEmpty(saldoFilter.CuentaID.Value))
                {
                    List<DTO_coCuentaSaldo> saldosList = this._moduloContab.Saldos_GetByParameter(saldoFilter);
                    saldoAntic = Math.Abs(saldosList.Sum(x => x.DbOrigenLocML.Value.Value + x.DbOrigenExtML.Value.Value + x.CrOrigenLocML.Value.Value +
                        x.CrOrigenExtML.Value.Value + x.DbSaldoIniLocML.Value.Value + x.DbSaldoIniExtML.Value.Value + x.CrSaldoIniLocML.Value.Value + x.CrSaldoIniExtML.Value.Value));
                }
                return saldoAntic;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region Eventos

        /// <summary>
        /// valida datos antes de imprimar para configurar diseño
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblCellDetalleInd_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                ////Se valida si es Detalle para no resaltar
                //if (this.tblCellDetalleInd.Text.Equals("True") || this.tblCellDetalleInd.Text.Equals("true"))
                //{
                //    this.tblRowTareas.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                //    this.tblCellDesc.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                //}

                //else
                //{
                //    this.tblRowTareas.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                //    this.tblCellDesc.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                //    this.tblCellUnd.Text = string.Empty;
                //    this.tblCellCant.Text = string.Empty;
                //    this.tblCellCostoUnit.Text = string.Empty;

                //    //Valida si es Titulo
                //    if (this.tblCellNivel.Text.Equals("0"))
                //    {
                //        this.tblCellTarea.Text = string.Empty;
                //        this.tblCellCostoTotal.Text = string.Empty;
                //    }
                //}
                ////Valida si oculta el codigo de Tarea Cliente
                //if (this.tblCellImprimirTarea.Text.Equals("False") || this.tblCellImprimirTarea.Text.Equals("false"))
                //    this.tblCellTarea.Text = string.Empty;

                ////Valida si resalta el item como Titulo
                //if (this.tblCellTituloNeg.Text.Equals("True") || this.tblCellTituloNeg.Text.Equals("true"))
                //{
                //    this.tblRowTareas.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                //    this.tblCellDesc.Font = new Font("Arial Narrow", 9, FontStyle.Bold);
                //    //this.tblCellUnd.Text = string.Empty;
                //    //this.tblCellCant.Text = string.Empty;
                //    //this.tblCellCostoUnit.Text = string.Empty;
                //}
                //else
                //{
                //    this.tblRowTareas.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                //    this.tblCellDesc.Font = new Font("Arial Narrow", 8, FontStyle.Regular);
                //}

            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this._userID.ToString(), "tblCellDetalleInd_BeforePrint");
            }
        }

        #endregion
    }
}
