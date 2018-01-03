using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Linq;

namespace NewAge.Reports.Fijos
{
    public partial class Report_py_EjecucionPresupuesto : XtraReport
    {
        #region Variables
        protected ModuloProyectos _moduloProyectos;
        protected ModuloGlobal _moduloGlobal;
        protected ModuloFacturacion _moduloFacturacion;
        protected ModuloProveedores _moduloProveedores;
        protected ModuloCuentasXPagar _moduloCxP;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected DTO_glEmpresa _empresa;
        protected int _userID;
        protected ModuloBase _moduloBase;
        protected ExportFormatType _formatType;
        protected int? numeroDoc;
        protected string loggerConnectionStr;
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

        #region Constructores
        public Report_py_EjecucionPresupuesto()
        {

        }

        public Report_py_EjecucionPresupuesto(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_py_EjecucionPresupuesto(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc = null)
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
        
        #endregion

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

            //this.lblNombreEmpresa.Text = this.Empresa.Descriptivo.Value;
            //this.lblUserName.Text = usuario.Descriptivo.Value;

            byte[] logo = this._moduloGlobal.glEmpresaLogo();
            try
            {
                object img = Utility.ByteArrayToObject(logo);
                Image logoImage = (Image)img;
                this.imgLogoEmpresa.Image = logoImage;
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
                this.imgLogoEmpresa.Image = logoImage;
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
        #endregion

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string proyecto, string cliente, string prefijo, int? docNro)
        {            
            try
            {
                #region Variables
                this._moduloProyectos = new ModuloProyectos(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                this._moduloFacturacion = new ModuloFacturacion(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                this._moduloProveedores = new ModuloProveedores(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                this._moduloCxP = new ModuloCuentasXPagar(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                ModuloContabilidad modContable = new ModuloContabilidad(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);

                Dictionary<int, DTO_faFacturaDocu> cacheFactVenta = new Dictionary<int, DTO_faFacturaDocu>();
                List<DTO_SolicitudTrabajo> data = new List<DTO_SolicitudTrabajo>();
                List<DTO_QueryTrazabilidad> trazabDetallada = new List<DTO_QueryTrazabilidad>();
                string monedaLocal = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                string recursoConfig = this._moduloBase.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_RecursoConfiguracionProy);

                string factor4x1000Str =  this._moduloBase.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_Factor4x1000CtrlCostos);
                string vlrFinancierosStr = this._moduloBase.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_VlrServiciosFinancierosCtrlCostos);
                string factorGarantiaStr = this._moduloBase.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_FactorGarantiaCtrlCostos);
                string factorComisionStr = this._moduloBase.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_FactorComisionCtrlCostos);
                string factorVlrNetoStr = this._moduloBase.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_FactorVlrNetoCtrlCostos);
                string codigoReporteIDStr = this._moduloBase.GetControlValueByCompany(ModulesPrefix.py, AppControl.py_CodigoReporteIDCtrlCostos);

                decimal? vlrBase4x1000 = 0;
                decimal? vlrRecursoConfig = 0;

                decimal factor4x1000 = !string.IsNullOrEmpty(factor4x1000Str)? Convert.ToDecimal(factor4x1000Str) : 0;
                decimal vlrFinancieros = !string.IsNullOrEmpty(vlrFinancierosStr)? Convert.ToDecimal(vlrFinancierosStr) : 0;
                decimal factorGarantia = !string.IsNullOrEmpty(factorGarantiaStr)? Convert.ToDecimal(factorGarantiaStr) : 0;
                decimal factorComision = !string.IsNullOrEmpty(factorComisionStr)? Convert.ToDecimal(factorComisionStr) : 0;
                decimal factorVlrNeto = !string.IsNullOrEmpty(factorVlrNetoStr)? Convert.ToDecimal(factorVlrNetoStr) : 0;

                #endregion

                DTO_SolicitudTrabajo proy = this._moduloProyectos.SolicitudProyecto_Load(AppDocuments.Proyecto, prefijo, docNro, null, string.Empty, proyecto, false, true, false,false, true);    
                foreach (DTO_QueryTrazabilidad traz in proy.ResumenTrazabilidad)
                    trazabDetallada.AddRange(traz.Detalle);

                //Trae las tareas del preproyecto
                proy.Detalle = this._moduloProyectos.pyPreProyectoTarea_Get(proy.HeaderProyecto.DocSolicitud.Value.Value,string.Empty,string.Empty,false);

                //Trae los recibidos que no son controlados excluyendo los Detalle4ID
                List<DTO_prDetalleDocu> detProveedor = this._moduloProveedores.prDetalleDocu_GetSolicitudByProyecto(AppDocuments.Recibido, null, proy.DocCtrl.ProyectoID.Value);
                detProveedor.RemoveAll(x => x.Detalle4ID.Value.HasValue);

                //Trae los mvtos de Facturas de Venta o Inventarios realizados
                DTO_glMovimientoDeta filter = new DTO_glMovimientoDeta();
                filter.DatoAdd4.Value = proy.DocCtrl.NumeroDoc.Value.ToString();
                filter.ProyectoID.Value = proy.DocCtrl.ProyectoID.Value;
                List<DTO_glMovimientoDeta> mvtos = this._moduloGlobal.glMovimientoDeta_GetByParameter(filter, false);

                #region Asigna Valores Generales              
                DTO_MasterBasic dtoProyecto = (DTO_MasterBasic)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.coProyecto, proy.DocCtrl.ProyectoID.Value, true, false);
                proy.DocCtrl.ProyectoDesc.Value = dtoProyecto != null ? proy.DocCtrl.ProyectoID.Value + "-" + dtoProyecto.Descriptivo.Value : proy.DocCtrl.ProyectoID.Value;
                DTO_faCliente dtoCliente = (DTO_faCliente)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, proy.HeaderProyecto.ClienteID.Value, true, false);
                proy.HeaderProyecto.ClienteDesc.Value = dtoCliente != null ? dtoCliente.Descriptivo.Value : proy.HeaderProyecto.EmpresaNombre.Value;
                decimal porIVAProy = proy.HeaderProyecto.PorIVA.Value.HasValue? proy.HeaderProyecto.PorIVA.Value.Value/100 : 0.19m;
                #endregion
                #region Ordena items segun Tipo Presupuesto
                DTO_pyClaseProyecto claseproy = (DTO_pyClaseProyecto)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, proy.HeaderProyecto.ClaseServicioID.Value, true, false);
                if (claseproy != null && claseproy.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) // Interno
                    proy.DetalleProyecto = proy.DetalleProyecto.OrderBy(x => x.CapituloTareaID.Value).ToList(); //Ordena por CapituloID
                #endregion
                #region Calcula Valores Detallados
                int nroCxP = 0;
                proy.HeaderProyecto.VlrUtilBruta = 0; 
                foreach (DTO_pyProyectoTarea tarea in  proy.DetalleProyecto)
                {
                    tarea.CantidadEjec.Value = 0;
                    tarea.VlrEjecutado.Value = 0;
                    tarea.VlrEjecutadoParcial.Value = 0;
                    tarea.CantidadTarea.Value = proy.Detalle.FindAll(x => x.TareaID.Value == tarea.TareaID.Value && x.Descriptivo.Value == tarea.Descriptivo.Value).Sum(x => x.Cantidad.Value);

                    #region Convierte los valores a la Moneda requerida si es necesario
                    if (!string.IsNullOrEmpty(proy.HeaderProyecto.MonedaPresupuesto.Value) &&  proy.HeaderProyecto.MonedaPresupuesto.Value != monedaLocal && proy.Header.TasaCambio.Value != 0)
                    {
                        tarea.CostoTotalML.Value = tarea.CostoTotalML.Value /  proy.HeaderProyecto.TasaCambio.Value;
                        tarea.CostoTotalUnitML.Value = tarea.CostoTotalUnitML.Value /  proy.HeaderProyecto.TasaCambio.Value;
                        tarea.CostoLocalCLI.Value = tarea.CostoLocalCLI.Value /  proy.HeaderProyecto.TasaCambio.Value;
                        tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value /  proy.HeaderProyecto.TasaCambio.Value;
                        tarea.CostoDiferenciaML.Value = Math.Abs(tarea.CostoLocalCLI.Value.Value - tarea.CostoTotalML.Value.Value);
                        foreach (DTO_pyProyectoDeta det in tarea.Detalle)
                        {
                            det.CostoLocalTOT.Value = det.CostoLocalTOT.Value /  proy.HeaderProyecto.TasaCambio.Value;
                            det.CostoLocal.Value = det.CostoLocal.Value /  proy.HeaderProyecto.TasaCambio.Value;
                        }
                    }
                    #endregion
                  
                    //Asigna datos de movimientos al detalle de cada tarea
                    foreach (DTO_pyProyectoDeta d in tarea.Detalle)
                    {
                        //Variables por detalle
                        List<DTO_QueryTrazabilidad> trazabxDet = trazabDetallada.FindAll(x => x.ConsecDeta.Value == d.Consecutivo.Value);
                        List<DTO_glDocumentoControl> ctrls = new List<DTO_glDocumentoControl>();                     
                        List<DTO_prOrdenCompraFooter> provDetalle = new List<DTO_prOrdenCompraFooter>();
                        List<DTO_ComprobanteFooter> cxpDetalle = new List<DTO_ComprobanteFooter>();

                        #region Calcula  valores del cliente segun %Multiplicador
                        //Asigna totales del Preproyecto
                        d.CantPreproyecto.Value = trazabxDet.Count > 0? trazabxDet.First().CantPreproyecto.Value : 0;//Cant Contratada
                        d.VlrUnitPreproyecto.Value = trazabxDet.FindAll(x => x.ConsecTarea.Value == tarea.Consecutivo.Value).Count > 0 ? trazabxDet.FindAll(x => x.ConsecTarea.Value == tarea.Consecutivo.Value).First().VlrUnitPreproyecto.Value : 0;
                        d.VlrTotPreproyecto.Value = trazabxDet.FindAll(x => x.ConsecTarea.Value == tarea.Consecutivo.Value).Count > 0 ? trazabxDet.FindAll(x => x.ConsecTarea.Value == tarea.Consecutivo.Value).First().VlrTotPreproyecto.Value : 0;
                        d.VlrUnitCLIPreproyecto.Value = trazabxDet.FindAll(x => x.ConsecTarea.Value == tarea.Consecutivo.Value).Count > 0 ? trazabxDet.FindAll(x => x.ConsecTarea.Value == tarea.Consecutivo.Value).First().VlrUnitCLIPreproyecto.Value : 0;
                        d.VlrTotCLIPreproyecto.Value = trazabxDet.FindAll(x => x.ConsecTarea.Value == tarea.Consecutivo.Value).Count > 0 ? trazabxDet.FindAll(x => x.ConsecTarea.Value == tarea.Consecutivo.Value).First().VlrTotCLIPreproyecto.Value : 0;
                        //Valida si la tarea tiene descuento
                        DTO_pyPreProyectoTarea tareaCotiz = proy.Detalle.Find(x => x.TareaID.Value == tarea.TareaID.Value && x.Descriptivo.Value == tarea.Descriptivo.Value);
                        d.VlrUnitCLIPreproyecto.Value -= tareaCotiz != null? (d.VlrUnitCLIPreproyecto.Value * (tareaCotiz.PorDescuento.Value / 100)) : 0;//Quita descuento
                        d.VlrTotCLIPreproyecto.Value -= tareaCotiz != null ? (d.VlrTotCLIPreproyecto.Value * (tareaCotiz.PorDescuento.Value / 100)) : 0;//Quita descuento

                        //Asigna totales del Proyecto
                        d.CantidadTOT.Value = tarea.Cantidad.Value * d.FactorID.Value;
                        d.CostoLocalTOT.Value = d.CantidadTOT.Value * d.CostoLocal.Value;
                        d.CostoLocalCLI.Value = d.CostoLocal.Value * (proy.HeaderProyecto.PorMultiplicadorPresup.Value / 100);
                        d.CostoLocalTOTCLI.Value = d.CostoLocalTOT.Value * (proy.HeaderProyecto.PorMultiplicadorPresup.Value / 100);
                        //Valida si la tarea tiene descuento                      
                        d.CostoLocalCLI.Value -= (d.CostoLocalCLI.Value * (tarea.PorDescuento.Value / 100)); //Quita descuento
                        d.CostoLocalTOTCLI.Value -= (d.CostoLocalTOTCLI.Value * (tarea.PorDescuento.Value / 100)); //Quita descuento
                        //Asigna valores unitarios
                        d.VlrUnitProyecto.Value = d.CostoLocal.Value;
                        d.VlrUnitCLIProyecto.Value = d.CostoLocalCLI.Value;

                        d.CostoLocalDiferencia.Value = Math.Abs(d.CostoLocalTOT.Value.Value - d.CostoLocalTOTCLI.Value.Value);
                        #endregion
                        #region Segun la trazabilidad asigna datos del Estado de los items
                        d.NroFactura = string.Empty;
                        d.NroOrdCompra = string.Empty;
                        #region Obtiene Documentos relacionados
                        foreach (DTO_QueryTrazabilidad tz in trazabxDet)
                        {
                            ctrls.AddRange(this._moduloProyectos.pyProyectoMvto_GetDocsAnexo(tz.ConsecMvto.Value));                           
                            if (ctrls.Count > 0)
                            {
                                //Obtiene info de CxP
                                foreach (DTO_glDocumentoControl ctrl in ctrls.FindAll(x => x.DocumentoID.Value == AppDocuments.CausarFacturas))
                                {
                                    if (!d.NroFactura.Contains(ctrl.DocumentoTercero.Value.ToString()))
                                    {
                                        d.NroFactura += (!string.IsNullOrEmpty(d.NroFactura) ? "/" + ctrl.DocumentoTercero.Value : ctrl.DocumentoTercero.Value);
                                        DTO_CuentaXPagar cxp = this._moduloCxP.CuentasXPagar_GetForCausacion(AppDocuments.CausarFacturas, ctrl.TerceroID.Value, ctrl.DocumentoTercero.Value);
                                        if (cxp != null && cxp.Comp != null)
                                            cxpDetalle.AddRange(cxp.Comp.Footer);
                                    }
                                }
                                //Obtiene info de Proveedores
                                foreach (DTO_glDocumentoControl ctrl in ctrls.FindAll(x => x.DocumentoID.Value == AppDocuments.OrdenCompra))
                                {
                                    if (!d.NroOrdCompra.Contains(ctrl.DocumentoNro.Value.ToString()))
                                    {
                                        d.NroOrdCompra += (!string.IsNullOrEmpty(d.NroOrdCompra) ? "/" + ctrl.DocumentoNro.Value : ctrl.DocumentoNro.Value.ToString());
                                        DTO_prOrdenCompra ord = this._moduloProveedores.OrdenCompra_Load(AppDocuments.OrdenCompra, ctrl.PrefijoID.Value, ctrl.DocumentoNro.Value.Value);
                                        if (ord != null)
                                        {
                                            provDetalle.AddRange(ord.Footer);
                                            d.ProveedorDesc += (!string.IsNullOrEmpty(d.ProveedorDesc) ? "/" + ord.HeaderOrdenCompra.ProveedorDesc.Value : ord.HeaderOrdenCompra.ProveedorDesc.Value);
                                        }
                                    }
                                } 
                            }
                        } 
                        #endregion
                        #region Calcula Costos Proyecto segun trazabilidad
                        //Asigna valores de Ejecucion
                        d.CantidadEjec.Value = 0;
                        d.CostoLocalEjec.Value= 0;
                        d.SubTotalLocalEjec.Value = 0;
                        d.CostoLocalIVATOTEjec.Value =0;
                        d.CostoLocalTOTEjec.Value = 0;
                        d.CantidadConsumoInt.Value = 0;
                        d.CostoLocalConsumoInt.Value = 0;
                        d.CantidadPrestamo.Value = 0;
                        d.CostoLocalPrestamo.Value = 0;
                        d.CantidadTotalTraslados.Value = 0;
                        d.CostoTotalTraslados.Value = 0;
                        d.CantidadTotalFINAL.Value = 0;
                        d.CostoTotalFINAL.Value = 0;
                        if (trazabxDet.Sum(x => x.CantFacturado.Value) > 0)
                        {
                            if (cxpDetalle.Count > 0)
                                d.Estado = "FACTURADO";
                            else
                                d.Estado = "RADICADO";
                            d.CantidadEjec.Value += trazabxDet.Sum(x => x.CantFacturado.Value);
                            d.CostoLocalEjec.Value += (d.CantidadEjec.Value != 0 ? trazabxDet.Sum(x => x.VlrFacturado.Value.Value) / d.CantidadEjec.Value.Value: 0);
                            d.SubTotalLocalEjec.Value += trazabxDet.Sum(x => x.VlrFacturado.Value.Value);
                            d.CostoLocalIVATOTEjec.Value += cxpDetalle.FindAll(x => x.DatoAdd1.Value == AuxiliarDatoAdd1.IVA.ToString()).Sum(y => y.vlrMdaLoc.Value);
                            if (d.Estado == "RADICADO")
                            {
                                foreach (DTO_QueryTrazabilidad t in trazabxDet)
                                    d.CostoLocalIVATOTEjec.Value += provDetalle.FindAll(x => x.DetalleDocu.Detalle4ID.Value == t.ConsecMvto.Value).Sum(y => y.DetalleDocu.IvaTotML.Value);
                            }
                            d.CostoLocalTOTEjec.Value += d.SubTotalLocalEjec.Value + d.CostoLocalIVATOTEjec.Value;
                            proy.HeaderProyecto.VlrUtilBruta += d.SubTotalLocalEjec.Value;
                            vlrBase4x1000 += d.SubTotalLocalEjec.Value;
                            nroCxP ++;
                        }
                        else if (trazabxDet.Sum(x => x.CantRecibido.Value) > 0)
                        {
                            d.Estado = "RECIBIDO";
                            d.CantidadEjec.Value += trazabxDet.Sum(x => x.CantRecibido.Value);
                            d.CostoLocalEjec.Value += (d.CantidadEjec.Value != 0 ?trazabxDet.Sum(x => x.VlrRecibido.Value.Value) / d.CantidadEjec.Value.Value : 0);
                            d.SubTotalLocalEjec.Value += trazabxDet.Sum(x => x.VlrRecibido.Value.Value);
                            foreach (DTO_QueryTrazabilidad t in trazabxDet)
                                d.CostoLocalIVATOTEjec.Value += provDetalle.FindAll(x => x.DetalleDocu.Detalle4ID.Value == t.ConsecMvto.Value).Sum(y => y.DetalleDocu.IvaTotML.Value);
                            d.CostoLocalTOTEjec.Value += d.SubTotalLocalEjec.Value + d.CostoLocalIVATOTEjec.Value;
                            proy.HeaderProyecto.VlrUtilBruta += d.SubTotalLocalEjec.Value;
                        }
                        else if (trazabxDet.Sum(x => x.CantComprado.Value) > 0)
                        {
                            d.Estado = "ODC APROBADA";
                            d.CantidadEjec.Value += trazabxDet.Sum(x => x.CantComprado.Value);
                            d.CostoLocalEjec.Value += (d.CantidadEjec.Value != 0 ? trazabxDet.Sum(x => x.VlrComprado.Value.Value) / d.CantidadEjec.Value.Value : 0);
                            d.SubTotalLocalEjec.Value += trazabxDet.Sum(x => x.VlrComprado.Value.Value);
                            foreach (DTO_QueryTrazabilidad t in trazabxDet)
                                d.CostoLocalIVATOTEjec.Value += provDetalle.FindAll(x => x.DetalleDocu.Detalle4ID.Value == t.ConsecMvto.Value).Sum(y => y.DetalleDocu.IvaTotML.Value);
                            d.CostoLocalTOTEjec.Value += d.SubTotalLocalEjec.Value + d.CostoLocalIVATOTEjec.Value;
                            proy.HeaderProyecto.VlrUtilBruta += d.SubTotalLocalEjec.Value;
                        }
                        else if (trazabxDet.Sum(x => x.CantPreComprado.Value) > 0)
                        {
                            d.Estado = "ODC EN PROCESO";
                            d.CantidadEjec.Value += trazabxDet.Sum(x => x.CantPreComprado.Value);
                            d.CostoLocalEjec.Value += (d.CantidadEjec.Value != 0 ? trazabxDet.Sum(x => x.VlrPreComprado.Value.Value) / d.CantidadEjec.Value.Value : 0);
                            d.SubTotalLocalEjec.Value += trazabxDet.Sum(x => x.VlrPreComprado.Value.Value);
                            foreach (DTO_QueryTrazabilidad t in trazabxDet)
                                d.CostoLocalIVATOTEjec.Value += provDetalle.FindAll(x=>x.DetalleDocu.Detalle4ID.Value == t.ConsecMvto.Value).Sum(y => y.DetalleDocu.IvaTotML.Value);
                            d.CostoLocalTOTEjec.Value += d.SubTotalLocalEjec.Value + d.CostoLocalIVATOTEjec.Value;
                            proy.HeaderProyecto.VlrUtilBruta += d.SubTotalLocalEjec.Value;
                        }
                        else if (trazabxDet.Sum(x => x.CantSolicitado.Value) > 0)
                        {
                            d.Estado = "SOLICITADO";
                            d.CantidadEjec.Value += (d.CantidadTOT.Value != 0? trazabxDet.Sum(x => x.CantSolicitado.Value) : 0);
                            d.CostoLocalEjec.Value += d.CostoLocal.Value;
                            d.SubTotalLocalEjec.Value += d.CantidadEjec.Value * d.CostoLocalEjec.Value;
                            d.CostoLocalIVATOTEjec.Value = 0;
                            d.CostoLocalTOTEjec.Value += d.SubTotalLocalEjec.Value + d.CostoLocalIVATOTEjec.Value;
                            proy.HeaderProyecto.VlrUtilBruta += d.SubTotalLocalEjec.Value;
                        }
                        else
                        {
                            d.Estado = "PRESUPUESTADO";
                            d.CantidadEjec.Value += (d.CantidadTOT.Value != 0 ? trazabxDet.Sum(x => x.CantPresupuestado.Value) : 0);
                            d.CostoLocalEjec.Value += d.CostoLocal.Value;
                            d.SubTotalLocalEjec.Value += d.CantidadEjec.Value * d.CostoLocalEjec.Value;
                            d.CostoLocalIVATOTEjec.Value = 0;
                            d.CostoLocalTOTEjec.Value += d.SubTotalLocalEjec.Value + d.CostoLocalIVATOTEjec.Value;
                            proy.HeaderProyecto.VlrUtilBruta += d.SubTotalLocalEjec.Value;
                        }
                        d.CantidadTotalFINAL.Value = !d.Estado.Equals("PRESUPUESTADO") ? d.CantidadEjec.Value : 0; 
                        d.CostoTotalFINAL.Value = !d.Estado.Equals("PRESUPUESTADO")? d.SubTotalLocalEjec.Value : 0;
                        #endregion
                        #endregion      
                        #region Si es recurso de configuracion suma el valor 
                        if (d.RecursoID.Value == recursoConfig)
                            vlrRecursoConfig += d.CostoLocalTOTCLI.Value;
                        #endregion
                    }

                    #region Agrega items de recibidos con solicitud de OC NO controlados 
                    foreach (DTO_prDetalleDocu recibido in detProveedor.FindAll(x => x.Detalle5ID.Value == tarea.Consecutivo.Value))
                    {
                        DTO_glDocumentoControl ctrlRecibido = this._moduloGlobal.glDocumentoControl_GetByID(recibido.NumeroDoc.Value.Value);

                        if (ctrlRecibido != null && (ctrlRecibido.Estado.Value != (byte)EstadoDocControl.Revertido || ctrlRecibido.Estado.Value != (byte)EstadoDocControl.Anulado))
                        {
                            string filtro = !string.IsNullOrEmpty(recibido.inReferenciaID.Value) ? recibido.inReferenciaID.Value : recibido.CodigoBSID.Value;
                            DTO_pyRecurso recurso = (DTO_pyRecurso)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, filtro, true, false);

                            #region Crea el detalle
                            DTO_pyProyectoDeta nuevo = new DTO_pyProyectoDeta();
                            nuevo.RecursoID.Value = filtro;
                            nuevo.RecursoDesc.Value = recurso != null ? recurso.Descriptivo.Value : string.Empty;
                            nuevo.UnidadInvID.Value = recurso != null ? recurso.UnidadInvID.Value : string.Empty;
                            if (tarea.Detalle.Exists(x => x.RecursoID.Value == nuevo.RecursoID.Value))
                                nuevo.Consecutivo.Value = tarea.Detalle.Find(x => x.RecursoID.Value == nuevo.RecursoID.Value).Consecutivo.Value;
                            else
                                nuevo.Consecutivo.Value = 9999999;
                            nuevo.ConsecTarea.Value = tarea.Consecutivo.Value;
                            nuevo.CantPreproyecto.Value = 0;
                            nuevo.VlrUnitPreproyecto.Value = 0;
                            nuevo.VlrTotPreproyecto.Value = 0;
                            nuevo.VlrUnitCLIPreproyecto.Value = 0;
                            nuevo.VlrTotCLIPreproyecto.Value = 0;
                            nuevo.Cantidad.Value = 0;
                            nuevo.CantidadTOT.Value = 0;
                            nuevo.CostoLocal.Value = 0;
                            nuevo.CostoLocalTOT.Value = 0;
                            nuevo.CostoLocalCLI.Value = 0;
                            nuevo.CostoLocalTOTCLI.Value = 0;
                            nuevo.CostoLocalDiferencia.Value = 0;
                            #endregion
                            #region Trae info de la OC
                            DTO_glDocumentoControl ctrlOC = this._moduloGlobal.glDocumentoControl_GetByID(recibido.OrdCompraDocuID.Value.Value);
                            if (ctrlOC != null)
                            {
                                nuevo.Estado = "SOLICITUD DIRECTA";
                                nuevo.NroOrdCompra += ctrlOC.DocumentoNro.Value;
                                DTO_prOrdenCompraDocu ord = this._moduloProveedores.prOrdenCompraDocu_Get(recibido.OrdCompraDocuID.Value.Value);
                                nuevo.ProveedorDesc += ord != null ? ord.ProveedorDesc.Value : string.Empty;
                            }

                            //Trae info de Factura
                            if (recibido.FacturaDocuID.Value.HasValue)
                            {
                                DTO_glDocumentoControl ctrlFact = this._moduloGlobal.glDocumentoControl_GetByID(recibido.FacturaDocuID.Value.Value);
                                nuevo.NroFactura += ctrlFact != null ? ctrlFact.DocumentoTercero.Value : string.Empty;
                            }
                            #endregion

                            #region Asigna Campos de Ejecucion
                            nuevo.CantidadEjec.Value = recibido.CantidadRec.Value;
                            nuevo.CostoLocalEjec.Value = recibido.ValorUni.Value;
                            nuevo.SubTotalLocalEjec.Value = recibido.ValorTotML.Value;
                            nuevo.CostoLocalIVATOTEjec.Value = recibido.IvaTotML.Value;
                            nuevo.CostoLocalTOTEjec.Value = nuevo.SubTotalLocalEjec.Value + nuevo.CostoLocalIVATOTEjec.Value;
                            proy.HeaderProyecto.VlrUtilBruta += nuevo.SubTotalLocalEjec.Value;
                            tarea.Detalle.Add(nuevo);
                            #endregion 
                        }
                    } 
                    #endregion 
                    tarea.Detalle = tarea.Detalle.OrderBy(x => x.Consecutivo.Value).ToList();
                }
                #endregion

                #region Agrega Items de Inventarios de Consumo Interno
                //Filtra y recorre los consumos internos de inventario con tareas relacionadas,que esten aprobados y sean entradas
                foreach (DTO_glMovimientoDeta mvto in mvtos.FindAll(x => x.DocumentoID.Value == AppDocuments.TransaccionManual && x.EstadoDocCtrl.Value == (Int16)EstadoDocControl.Aprobado && 
                                                                    x.DocSoporte.Value.HasValue))
                {
                    //Trae la tarea
                    DTO_pyProyectoMvto mvtoProy = proy.Movimientos.Find(x => x.Consecutivo.Value == mvto.DocSoporte.Value);
                    DTO_pyProyectoTarea tareaProy = proy.DetalleProyecto.Find(x => x.Consecutivo.Value == (mvtoProy != null? mvtoProy.ConsecTarea.Value : 0));
                    if (tareaProy != null)
                    {
                        DTO_pyRecurso recurso = (DTO_pyRecurso)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, mvto.inReferenciaID.Value, true, false);

                        #region Trae el detalle
                        DTO_pyProyectoDeta exist = tareaProy.Detalle.Find(x => x.RecursoID.Value == mvto.inReferenciaID.Value);
                        #endregion

                        //Modifica Campos
                        if (mvto.TipoTrasladoInv.Value == (byte)TipoTraslado.Traslado || mvto.TipoTrasladoInv.Value == (byte)TipoTraslado.ConsumoInterno)
                        {
                            exist.CantidadConsumoInt.Value += mvto.CantidadUNI.Value;
                            exist.CostoLocalConsumoInt.Value += mvto.Valor1LOC.Value;
                        }
                        else if (mvto.TipoTrasladoInv.Value == (byte)TipoTraslado.PrestamoProyecto)
                        {
                            exist.CantidadPrestamo.Value -= mvto.CantidadUNI.Value;
                            exist.CostoLocalPrestamo.Value -= mvto.Valor1LOC.Value;
                        }
                        else if (mvto.TipoTrasladoInv.Value == (byte)TipoTraslado.DevolucionProyecto)
                        {
                            exist.CantidadPrestamo.Value += mvto.CantidadUNI.Value;
                            exist.CostoLocalPrestamo.Value += mvto.Valor1LOC.Value;
                        }
                        exist.CantidadTotalTraslados.Value = exist.CantidadConsumoInt.Value + exist.CantidadPrestamo.Value;
                        exist.CostoTotalTraslados.Value = exist.CostoLocalConsumoInt.Value + exist.CostoLocalPrestamo.Value;

                        //COSTOS FINALES
                        //Cuando el consumo interno fue total asigna valores del consumo int
                        if (exist.CantidadTotalTraslados.Value == exist.CantidadEjec.Value)
                        {
                            exist.CantidadEjec.Value = exist.CantidadTotalTraslados.Value;
                            exist.CantidadTotalFINAL.Value = exist.CantidadTotalTraslados.Value;
                            exist.CostoLocalTOTEjec.Value = exist.CostoTotalTraslados.Value;
                            exist.CostoLocalEjec.Value = Math.Round(exist.CostoTotalTraslados.Value.Value / exist.CantidadTotalTraslados.Value.Value, 2);
                            exist.SubTotalLocalEjec.Value = exist.CostoLocalTOTEjec.Value;
                            exist.CostoTotalFINAL.Value = exist.CostoTotalTraslados.Value;
                            proy.HeaderProyecto.VlrUtilBruta += exist.CostoTotalTraslados.Value;
                            exist.Estado = "CONSUMO INTERNO";
                        }
                        //Cuando el consumo interno fue parcial asigna valores de combinados
                        else if (exist.CantidadTotalTraslados.Value > 0 && exist.CantidadTotalTraslados.Value != exist.CantidadEjec.Value)
                        {
                            decimal? cantFinal = exist.CantidadEjec.Value - exist.CantidadTotalTraslados.Value;
                            decimal? costoCantFinal = cantFinal * exist.CostoLocalEjec.Value;
                            decimal? costoFinal  = costoCantFinal + exist.CostoTotalTraslados.Value;
                            exist.CostoTotalFINAL.Value = costoFinal;
                            proy.HeaderProyecto.VlrUtilBruta += costoFinal;
                           // if(exist.Estado.Equals("SOLICITADO"))
                                exist.Estado += "-CONSUMO INTERNO";
                        }                        
                    }
                }
                #endregion

                #region Valida si recibio prestamos 
                filter = new DTO_glMovimientoDeta();
                filter.EntradaSalida.Value =1;
                filter.TipoTrasladoInv.Value = (byte)TipoTraslado.PrestamoProyecto;
                filter.ProyectoID.Value = proy.DocCtrl.ProyectoID.Value;
                List<DTO_glMovimientoDeta> mvtosPrestamos = this._moduloGlobal.glMovimientoDeta_GetByParameter(filter, false);
                //Filtra y recorre los consumos internos de inventario con tareas relacionadas,que esten aprobados y sean entradas
                foreach (DTO_glMovimientoDeta mvto in mvtosPrestamos.FindAll(x => x.DocumentoID.Value == AppDocuments.TransaccionManual && x.EstadoDocCtrl.Value == (Int16)EstadoDocControl.Aprobado &&
                                                                    x.DocSoporte.Value.HasValue))
                {
                    //Trae la tarea
                    DTO_pyProyectoMvto mvtoProy = proy.Movimientos.Find(x => x.inReferenciaID.Value == mvto.inReferenciaID.Value);
                    DTO_pyProyectoTarea tareaProy = proy.DetalleProyecto.Find(x => x.Consecutivo.Value == (mvtoProy != null ? mvtoProy.ConsecTarea.Value : 0));
                    if (tareaProy != null)
                    {
                        DTO_pyRecurso recurso = (DTO_pyRecurso)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyRecurso, mvto.inReferenciaID.Value, true, false);

                        #region Crea el detalle
                        DTO_pyProyectoDeta exist = tareaProy.Detalle.Find(x => x.RecursoID.Value == mvto.inReferenciaID.Value);

                        if (mvto.TipoTrasladoInv.Value == (byte)TipoTraslado.PrestamoProyecto)
                        {
                            exist.CantidadPrestamo.Value += mvto.CantidadUNI.Value;
                            exist.CostoLocalPrestamo.Value += mvto.Valor1LOC.Value;
                        }

                        exist.CantidadTotalTraslados.Value = exist.CantidadConsumoInt.Value + exist.CantidadPrestamo.Value;
                        exist.CostoTotalTraslados.Value = exist.CostoLocalConsumoInt.Value + exist.CostoLocalPrestamo.Value;

                        //COSTOS FINALES
                        //if (exist.CantidadTotalTraslados.Value > 0)
                        //{
                        //    exist.CantidadEjec.Value = exist.CantidadTotalTraslados.Value;
                        //    exist.CantidadTotalFINAL.Value = exist.CantidadTotalTraslados.Value;
                        //    exist.CostoLocalTOTEjec.Value = exist.CostoTotalTraslados.Value;
                        //    exist.CostoLocalEjec.Value = Math.Round(exist.CostoTotalTraslados.Value.Value / exist.CantidadTotalTraslados.Value.Value, 2);
                        //    exist.SubTotalLocalEjec.Value = exist.CostoLocalTOTEjec.Value;
                        //    exist.CostoTotalFINAL.Value = exist.CostoTotalTraslados.Value;
                        //    solicitud.HeaderProyecto.VlrUtilOperacional += exist.CostoTotalTraslados.Value;
                        //    exist.Estado = "CONSUMO INTERNO";
                        //}
                      
                        #endregion
                    }
                }
                #endregion

                #region Trae las facturas de venta para Asignar Ejecucion
                //Filtra y recorre las Facturas de Venta con tareas relacionadas,que esten aprobadas y No sean de inventario
                foreach (DTO_glMovimientoDeta mvto in  mvtos.FindAll(x => x.DocumentoID.Value == AppDocuments.FacturaVenta && x.EstadoDocCtrl.Value == (Int16)EstadoDocControl.Aprobado && 
                                                                     x.DocSoporte.Value.HasValue && !x.DatoAdd5.Value.Equals("INV")))
                {
                    //Trae la tarea
                    DTO_pyProyectoTarea tareasProy = proy.DetalleProyecto.Find(x => x.Consecutivo.Value == mvto.DocSoporte.Value);
                    if (tareasProy != null)
                    {
                        //Calcula cant y vlr Ejecutado de la tarea
                        tareasProy.CantidadEjec.Value += mvto.CantidadUNI.Value;
                        tareasProy.VlrEjecutado.Value += mvto.Valor1LOC.Value;
                        if (!cacheFactVenta.ContainsKey(mvto.NumeroDoc.Value.Value))
                        {
                            DTO_faFacturaDocu docu = this._moduloFacturacion.faFacturaDocu_Get(mvto.NumeroDoc.Value.Value);
                            docu.Administracion.Value = docu.Administracion.Value ?? 0;
                            docu.Imprevistos.Value = docu.Imprevistos.Value ?? 0;
                            docu.Utilidad.Value = docu.Utilidad.Value ?? 0;
                            docu.Retencion4.Value = docu.Retencion4.Value ?? 0;
                            docu.Retencion10.Value = docu.Retencion10.Value ?? 0;
                            cacheFactVenta.Add(mvto.NumeroDoc.Value.Value, docu);                            
                        }
                    }
                }
                #endregion

                #region Calcula sumas por tarea
                foreach (DTO_pyProyectoTarea tarea in proy.DetalleProyecto)
                {
                    //Trae valores del preproyecto
                    if (proy.Detalle.Exists(x => x.TareaID.Value == tarea.TareaID.Value && x.Descriptivo.Value == tarea.Descriptivo.Value))
                    {
                        DTO_pyPreProyectoTarea tareaPre = proy.Detalle.Find(x => x.TareaID.Value == tarea.TareaID.Value && x.Descriptivo.Value == tarea.Descriptivo.Value);
                        tarea.VlrUnitPreproyecto.Value = tareaPre.CostoTotalUnitML.Value; // tarea.Detalle.Sum(x => x.VlrUnitPreproyecto.Value);
                        tarea.VlrTotPreproyecto.Value = tareaPre.CostoTotalML.Value;// tarea.Detalle.Sum(x => x.VlrTotPreproyecto.Value);
                        tarea.VlrUnitCLIPreproyecto.Value = tareaPre.CostoLocalUnitCLI.Value;// tarea.Detalle.Sum(x => x.VlrUnitCLIPreproyecto.Value);
                        tarea.VlrTotCLIPreproyecto.Value = tareaPre.CostoLocalCLI.Value;// tarea.Detalle.Sum(x => x.VlrTotCLIPreproyecto.Value);
                    }
                    else
                    {
                        tarea.VlrUnitPreproyecto.Value = 0;
                        tarea.VlrTotPreproyecto.Value = 0;
                        tarea.VlrUnitCLIPreproyecto.Value = 0;
                        tarea.VlrTotCLIPreproyecto.Value = 0;
                    }
                    tarea.CostoTotalTraslados.Value = tarea.Detalle.Sum(x => x.CostoTotalTraslados.Value);
                    tarea.CostoTotalFINAL.Value = tarea.Detalle.Sum(x => x.CostoTotalFINAL.Value);
                    tarea.VlrEjecutadoParcial.Value = tarea.VlrEjecutado.Value != 0? tarea.VlrEjecutado.Value : tarea.CostoLocalCLI.Value;
                    tarea.CantidadEjecParcial.Value = tarea.CantidadEjec.Value != 0 ? tarea.CantidadEjec.Value : tarea.Cantidad.Value;
                    tarea.SubTotalLocalEjec.Value = tarea.Detalle.Sum(x => x.SubTotalLocalEjec.Value);
                    tarea.CostoLocalIVATOTEjec.Value = tarea.Detalle.Sum(x => x.CostoLocalIVATOTEjec.Value);
                    tarea.CostoLocalTOTEjec.Value = tarea.Detalle.Sum(x => x.CostoLocalTOTEjec.Value);
                    if (claseproy != null && claseproy.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Construccion)
                        tarea.TareaID.Value = tarea.TareaCliente.Value;
                }
                #endregion

                #region Valores Resumen y Totales
                decimal vlrVentaTOTAL = proy.DetalleProyecto.Sum(x => x.VlrEjecutado.Value.Value);
                List<DTO_coReporteLinea> lineas = modContable.Saldo_GetSaldosByLineaReporte(codigoReporteIDStr, null, null,string.Empty,proy.DocCtrl.ProyectoID.Value,string.Empty,string.Empty); //Trae info del reporte 
                proy.HeaderProyecto.VlrCostosMenores = lineas.FindAll(x => x.RepLineaID.Value.Equals("4")).Sum(x => x.VlrTotalMLRepLinea.Value);   // 4 Costos Menores
                proy.HeaderProyecto.VlrUtilBruta = vlrVentaTOTAL - proy.DetalleProyecto.Sum(x => x.CostoTotalFINAL.Value) - proy.HeaderProyecto.VlrCostosMenores;             
                proy.HeaderProyecto.VlrEstimadoAdmin = vlrVentaTOTAL * (0.11m);     //Traer glControl      FALTA DEFINIR CONTROL
                proy.HeaderProyecto.VlrEstimadoNomina = vlrVentaTOTAL * (0.06m);    //Traer glControl FALTA DEFINIR CONTROL
                proy.HeaderProyecto.VlrICA = vlrVentaTOTAL * (6.9m / 1000);         // cacheFactVenta.Sum(x => x.Value.Retencion3.Value);
                proy.HeaderProyecto.VlrPoliza = lineas.FindAll(x => x.RepLineaID.Value.Equals("2")).Sum(x => x.VlrTotalMLRepLinea.Value);     // 2 Poliza
                proy.HeaderProyecto.VlrGarSoporte = vlrRecursoConfig * (0.015m);    //Traer glControl con item de configuracion que se defina(RecursoID)            
                proy.HeaderProyecto.VlrComision = vlrVentaTOTAL * (0.01m);          //Traer glControl FALTA DEFINIR CONTROL
                proy.HeaderProyecto.VlrUtilOperacional = proy.HeaderProyecto.VlrUtilBruta -(proy.HeaderProyecto.VlrEstimadoNomina + proy.HeaderProyecto.VlrEstimadoAdmin +
                                                                                            proy.HeaderProyecto.VlrICA +proy.HeaderProyecto.VlrPoliza +
                                                                                            proy.HeaderProyecto.VlrGarSoporte+ proy.HeaderProyecto.VlrComision);
                proy.HeaderProyecto.Vlr4x1000 = (vlrVentaTOTAL+ cacheFactVenta.Values.Sum(x => x.Iva.Value)) * (4/1000); //Traer glControl FALTA DEFINIR CONTROL
                proy.HeaderProyecto.VlrFinancieros = vlrVentaTOTAL * (0.015m);      //Traer glControl nroCxP * (3000);     FALTA DEFINIR CONTROL     
                proy.HeaderProyecto.VlrIntereses = lineas.FindAll(x => x.RepLineaID.Value.Equals("3")).Sum(x => x.VlrTotalMLRepLinea.Value);   // 3 Intereses
                proy.HeaderProyecto.VlrUtilSinImpuestos = proy.HeaderProyecto.VlrUtilOperacional - (proy.HeaderProyecto.Vlr4x1000 + proy.HeaderProyecto.VlrFinancieros+ proy.HeaderProyecto.VlrIntereses);
                proy.HeaderProyecto.VlrImpuestoRenta = proy.HeaderProyecto.VlrUtilOperacional *0.4m;
                proy.HeaderProyecto.VlrUtilNETO = proy.HeaderProyecto.VlrUtilSinImpuestos - proy.HeaderProyecto.VlrImpuestoRenta;

                //Valores Totales
                #region AIU
                if (proy.HeaderProyecto.APUIncluyeAIUInd.Value.Value)
                {
                    this.rowAdmin.Visible = true;
                    this.rowImprev.Visible = true;
                    this.rowUtilidad.Visible = true;
                    proy.HeaderProyecto.VlrAIUVentasAdmin = cacheFactVenta.Values.Sum(x => x.Administracion.Value);
                    proy.HeaderProyecto.VlrAIUVentasImpr = cacheFactVenta.Values.Sum(x => x.Imprevistos.Value);
                    proy.HeaderProyecto.VlrAIUVentasUtil = cacheFactVenta.Values.Sum(x => x.Utilidad.Value);
                    if (proy.HeaderProyecto.TipoRedondeo.Value == 2)//Redonde hacia arriba
                    {
                        proy.HeaderProyecto.VlrAIUPreProyAdmin = Math.Ceiling(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaADM.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUPreProyImpr = Math.Ceiling(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaIMP.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUPreProyUtil = Math.Ceiling(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaUTI.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyAdmin = Math.Ceiling(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaADM.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyImpr = Math.Ceiling(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaIMP.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyUtil = Math.Ceiling(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaUTI.Value.Value / 100));
                    }
                    else if (proy.HeaderProyecto.TipoRedondeo.Value == 3) //Redondea hacia abajo
                    {
                        proy.HeaderProyecto.VlrAIUPreProyAdmin = Math.Floor(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaADM.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUPreProyImpr = Math.Floor(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaIMP.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUPreProyUtil = Math.Floor(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaUTI.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyAdmin = Math.Floor(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaADM.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyImpr = Math.Floor(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaIMP.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyUtil = Math.Floor(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaUTI.Value.Value / 100));
                    }
                    else //Redondea Normal
                    {
                        proy.HeaderProyecto.VlrAIUPreProyAdmin = Math.Round(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaADM.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUPreProyImpr = Math.Round(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaIMP.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUPreProyUtil = Math.Round(proy.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaUTI.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyAdmin = Math.Round(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaADM.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyImpr = Math.Round(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaIMP.Value.Value / 100));
                        proy.HeaderProyecto.VlrAIUProyUtil = Math.Round(proy.DetalleProyecto.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoLocalCLI.Value.Value) * (proy.HeaderProyecto.PorEmpresaUTI.Value.Value / 100));
                    }
                }
                else
                {
                    proy.HeaderProyecto.VlrAIUPreProyAdmin = 0;
                    proy.HeaderProyecto.VlrAIUPreProyImpr = 0;
                    proy.HeaderProyecto.VlrAIUPreProyUtil = 0;
                    proy.HeaderProyecto.VlrAIUProyAdmin = 0;
                    proy.HeaderProyecto.VlrAIUProyImpr = 0;
                    proy.HeaderProyecto.VlrAIUProyUtil = 0;
                    proy.HeaderProyecto.VlrAIUVentasAdmin = 0;
                    proy.HeaderProyecto.VlrAIUVentasImpr = 0;
                    proy.HeaderProyecto.VlrAIUVentasUtil = 0;
                }
                #endregion
                #region IVA
                proy.HeaderProyecto.VlrIVAProyVentas = cacheFactVenta.Values.Sum(x => x.Iva.Value);// (vlrVentaTOTAL * porIVAProy); 
                if (proy.HeaderProyecto.APUIncluyeAIUInd.Value.Value)
                {
                    proy.HeaderProyecto.VlrIVAPreProy = 0;// Math.Round(proy.HeaderProyecto.VlrAIUPreProyUtil.Value * porIVAProy); 
                    proy.HeaderProyecto.VlrIVAPreProyCLI = Math.Round(proy.HeaderProyecto.VlrAIUPreProyUtil.Value * porIVAProy);
                    proy.HeaderProyecto.VlrIVAProy = 0;// Math.Round(proy.HeaderProyecto.VlrAIUPreProyUtil.Value * porIVAProy);
                    proy.HeaderProyecto.VlrIVAProyCLI = Math.Round(proy.HeaderProyecto.VlrAIUProyUtil.Value * porIVAProy);
                    proy.HeaderProyecto.VlrIVAProyDif = Math.Abs(proy.HeaderProyecto.VlrIVAProyCLI.Value - proy.HeaderProyecto.VlrIVAProy.Value);
                }
                else
                {
                    proy.HeaderProyecto.VlrIVAPreProy = proy.Detalle.Sum(x => x.CostoTotalML.Value) * porIVAProy;
                    proy.HeaderProyecto.VlrIVAPreProyCLI = proy.Detalle.Sum(x => x.CostoLocalCLI.Value) * porIVAProy;
                    proy.HeaderProyecto.VlrIVAProy = proy.DetalleProyecto.Sum(x => x.CostoTotalML.Value) * porIVAProy;
                    proy.HeaderProyecto.VlrIVAProyCLI = proy.DetalleProyecto.Sum(x => x.CostoLocalCLI.Value) * porIVAProy;
                    proy.HeaderProyecto.VlrIVAProyDif = Math.Abs(proy.HeaderProyecto.VlrIVAProyCLI.Value - proy.HeaderProyecto.VlrIVAProy.Value);
                }
                #endregion
                #region Totales
                proy.HeaderProyecto.VlrTotalPreProy = proy.Detalle.Sum(x => x.CostoTotalML.Value) + proy.HeaderProyecto.VlrIVAPreProy;
                proy.HeaderProyecto.VlrTotalPreProyCLI = proy.Detalle.Sum(x => x.CostoLocalCLI.Value) + proy.HeaderProyecto.VlrIVAPreProyCLI + proy.HeaderProyecto.VlrAIUPreProyAdmin + proy.HeaderProyecto.VlrAIUPreProyImpr + proy.HeaderProyecto.VlrAIUPreProyUtil;
                proy.HeaderProyecto.VlrTotalProy = proy.DetalleProyecto.Sum(x => x.CostoTotalML.Value) + proy.HeaderProyecto.VlrIVAProy;
                proy.HeaderProyecto.VlrTotalProyCLI = proy.DetalleProyecto.Sum(x => x.CostoLocalCLI.Value) + proy.HeaderProyecto.VlrIVAProyCLI + proy.HeaderProyecto.VlrAIUProyAdmin + proy.HeaderProyecto.VlrAIUProyImpr + proy.HeaderProyecto.VlrAIUProyUtil;
                proy.HeaderProyecto.VlrTotalProyDif = Math.Abs(proy.HeaderProyecto.VlrTotalProyCLI.Value - proy.HeaderProyecto.VlrTotalProy.Value);
                proy.HeaderProyecto.VlrTotalProyVentas = vlrVentaTOTAL + proy.HeaderProyecto.VlrIVAProyVentas + proy.HeaderProyecto.VlrAIUVentasAdmin + proy.HeaderProyecto.VlrAIUVentasImpr + proy.HeaderProyecto.VlrAIUVentasUtil;

                #endregion                //Asigna valores
                this.cellIVA.Text = "IVA(" + (porIVAProy*100).ToString("n0") + "%)";
                this.cellPorcEstAdmin.Text = "11%";
                this.cellPorcEstNominas.Text = "6%";
                this.cellPorcICA.Text = "6.9/1000";
                this.cellPorcGarant.Text = "1.5%";
                this.cellPorcComision.Text = "1%";
                this.cellPorc4x1000.Text = "4/1000";
                this.cellPorcFInancieros.Text = "1.5%";
                this.cellPorcRenta.Text = "40%";
                #endregion
                data.Add(proy);
                DataSource = data;
                this.ShowPreview();
                return this.ReportName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}
