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
    public partial class Report_py_PlaneacionCostosDetallado : XtraReport
    {
        #region Variables

        private DTO_SolicitudTrabajo solicitud = new DTO_SolicitudTrabajo();
        protected ModuloGlobal _moduloGlobal;
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

        public Report_py_PlaneacionCostosDetallado()
        {

        }

        public Report_py_PlaneacionCostosDetallado(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_py_PlaneacionCostosDetallado(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc = null)
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
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(DTO_SolicitudTrabajo solicitud, byte tipoReport)
        {
            try
            {
                List<DTO_SolicitudTrabajo> list = new List<DTO_SolicitudTrabajo>();
                this.solicitud = solicitud;

                #region Asigna Valores Generales
                string monedaLocal = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
                DTO_faCliente cliente = (DTO_faCliente)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, this.solicitud.Header.ClienteID.Value, true, false);
                this.solicitud.Header.ClienteDesc.Value = cliente != null ? cliente.Descriptivo.Value : this.solicitud.Header.EmpresaNombre.Value;
                if (tipoReport == 1) 
                    this.solicitud.Header.Valor.Value = this.solicitud.Header.Valor.Value + this.solicitud.Header.ValorIVA.Value + this.solicitud.Header.ValorOtros.Value;
                else if (tipoReport == 2) //Reporte con Valores Cliente
                     this.solicitud.Header.Valor.Value = this.solicitud.Header.ValorCliente.Value + this.solicitud.Header.ValorIVA.Value + this.solicitud.Header.ValorOtros.Value;

                DTO_seUsuario user = (DTO_seUsuario)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.seUsuario, this.solicitud.Header.ResponsableEMP.Value, true, false);
                this.solicitud.CorreoUsuario.Value = user != null ? user.CorreoElectronico.Value : string.Empty;
                this.solicitud.TelefonoUsuario.Value = user != null ? user.Telefono.Value : string.Empty;
                #endregion

                #region Asigna Otros valores segun Tipo Presupuesto
                DTO_pyClaseProyecto claseproy = (DTO_pyClaseProyecto)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, this.solicitud.Header.ClaseServicioID.Value, true, false);
                if (claseproy != null && claseproy.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) // Interno
                {
                    string marcaxDef = this._moduloBase.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_MarcaxDef);
                    this.solicitud.Detalle = this.solicitud.Detalle.OrderBy(x => x.CapituloTareaID.Value).ToList(); //Ordena por CapituloID
                    int count = 1;
                    foreach (var tarea in this.solicitud.Detalle)
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

                #region Asigna valores segun el Tipo de Reporte

                #region Calcula valores del Multiplicador
                foreach (DTO_pyPreProyectoTarea tarea in solicitud.Detalle)
                {
                    #region Convierte los valores a la Moneda requerida si es necesario
                    if (!string.IsNullOrEmpty(solicitud.Header.MonedaPresupuesto.Value) && solicitud.Header.MonedaPresupuesto.Value != monedaLocal && solicitud.Header.TasaCambio.Value != 0)
                    {
                        tarea.CostoTotalML.Value = tarea.CostoTotalML.Value / solicitud.Header.TasaCambio.Value;
                        tarea.CostoTotalUnitML.Value = tarea.CostoTotalUnitML.Value / solicitud.Header.TasaCambio.Value;
                        tarea.CostoLocalCLI.Value = tarea.CostoLocalCLI.Value / solicitud.Header.TasaCambio.Value;
                        tarea.CostoLocalUnitCLI.Value = tarea.CostoLocalUnitCLI.Value / solicitud.Header.TasaCambio.Value;
                        tarea.CostoDiferenciaML.Value = Math.Abs(tarea.CostoLocalCLI.Value.Value - tarea.CostoTotalML.Value.Value);
                        foreach (DTO_pyPreProyectoDeta det in tarea.Detalle)
                        {
                            det.CostoLocalTOT.Value = det.CostoLocalTOT.Value / solicitud.Header.TasaCambio.Value;
                            det.CostoLocal.Value = det.CostoLocal.Value / solicitud.Header.TasaCambio.Value;
                        }
                    } 
                    #endregion
                    foreach (DTO_pyPreProyectoDeta det in tarea.Detalle)
                    {
                        det.CantidadTOT.Value = tarea.Cantidad.Value * det.FactorID.Value;
                        det.CostoLocalTOT.Value = tarea.Cantidad.Value * det.CostoLocalTOT.Value;
                        det.CostoLocalTOTCLI.Value = det.CostoLocalTOT.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                        det.CostoLocalCLI.Value = det.CostoLocal.Value * (solicitud.Header.PorMultiplicadorPresup.Value / 100);
                        det.CostoLocalCLI.Value -= (det.CostoLocalCLI.Value * (tarea.PorDescuento.Value / 100)); //Quita descuento
                        det.CostoLocalTOTCLI.Value -= (det.CostoLocalTOTCLI.Value * (tarea.PorDescuento.Value / 100)); //Quita descuento
                        det.CostoLocalDiferencia.Value = Math.Abs(det.CostoLocalTOT.Value.Value - det.CostoLocalTOTCLI.Value.Value);
                    }
                }
                #endregion
                #endregion

                list.Add(solicitud);
             
                this.DataSource = list;
                this.ShowPreview();
                return this.ReportName;
            }
            catch (Exception ex)
            {
                var exception = new Exception(DictionaryMessages.Err_GettingData, ex);
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, exception, this._userID.ToString(), "tblCellDetalleInd_BeforePrint");
                return string.Empty;
            }
        } 
        #endregion

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

        #region Eventos

        /// <summary>
        /// valida datos antes de imprimar para configurar diseño y totales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblCellVlrSubTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //#region Declara variables
            //decimal totalTareasEmp = 0;
            //decimal totalTareasAdicEmp = 0;
            //decimal vlrAdmin = 0;
            //decimal vlrImpr = 0;
            //decimal vlrUtil = 0;
            //decimal vlrIVA = 0;
            //#endregion

            //#region Calcula Totales
            //if (this.solicitud.Detalle.Count > 0)
            //    totalTareasEmp = this.solicitud.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoTotalML.Value.Value);
            //if (this.solicitud.DetalleTareasAdic.Count > 0)
            //    totalTareasAdicEmp = this.solicitud.DetalleTareasAdic.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoTotalML.Value.Value);

            //if (!this.solicitud.Header.APUIncluyeAIUInd.Value.Value)
            //{
            //    vlrAdmin = totalTareasEmp * (this.solicitud.Header.PorEmpresaADM.Value.Value / 100);
            //    vlrImpr = totalTareasEmp * (this.solicitud.Header.PorEmpresaIMP.Value.Value / 100);
            //    vlrUtil = totalTareasEmp * (this.solicitud.Header.PorEmpresaUTI.Value.Value / 100);
            //}
            //vlrIVA = vlrUtil * Convert.ToDecimal(16 * 0.01);
            //#endregion
        }
        #endregion
    }
}
