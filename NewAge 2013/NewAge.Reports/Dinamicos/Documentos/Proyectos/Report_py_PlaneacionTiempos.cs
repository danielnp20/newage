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
    public partial class Report_py_PlaneacionTiempos : XtraReport
    {
        #region Variables

        private DTO_SolicitudTrabajo _solicitud = new DTO_SolicitudTrabajo();
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

        #region Contructores
        public Report_py_PlaneacionTiempos()
        {

        }

        public Report_py_PlaneacionTiempos(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_py_PlaneacionTiempos(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc = null)
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
            #region Recursos
            this.lblPage.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblPage");
            this.lblFecha.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblDate");
            this.lblUser.Text = this._moduloGlobal.GetResource(LanguageTypes.Messages, "msg_lblUser");

            this.reportProvider = new ReportProvider(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this.reportProvider.LoadResources(this.AllControls<XRControl>());
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
        public string GenerateReport(DTO_SolicitudTrabajo cotizacion)
        {
            try
            {
                List<DTO_SolicitudTrabajo> list = new List<DTO_SolicitudTrabajo>();
                this._solicitud = cotizacion;

                #region Asigna Valores Generales
                DTO_faCliente cliente = (DTO_faCliente)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.faCliente, this._solicitud.Header.ClienteID.Value, true, false);
                this._solicitud.Header.ClienteDesc.Value = cliente != null ? cliente.Descriptivo.Value : this._solicitud.Header.EmpresaNombre.Value;

                DTO_seUsuario user = (DTO_seUsuario)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.seUsuario, this._solicitud.Header.ResponsableEMP.Value, true, false);
                this._solicitud.CorreoUsuario.Value = user != null ? user.CorreoElectronico.Value : string.Empty;
                this._solicitud.TelefonoUsuario.Value = user != null ? user.Telefono.Value : string.Empty;
                #endregion

                #region Asigna Otros valores segun Tipo Presupuesto
                DTO_pyClaseProyecto claseproy = (DTO_pyClaseProyecto)this._moduloBase.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.pyClaseProyecto, this._solicitud.Header.ClaseServicioID.Value, true, false);
                if (claseproy != null && claseproy.TipoPresupuesto.Value == (byte)TipoPresupuestoProy.Otros) // Interno
                {
                    string marcaxDef = this._moduloBase.GetControlValueByCompany(ModulesPrefix.@in, AppControl.in_MarcaxDef);
                    this._solicitud.Detalle = this._solicitud.Detalle.OrderBy(x => x.CapituloTareaID.Value).ToList(); //Ordena por CapituloID
                    int count = 1;
                    foreach (var tarea in this._solicitud.Detalle)
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

                this._solicitud.Header.PorClienteADM.Value = this._solicitud.Header.PorClienteADM.Value ?? 0;
                this._solicitud.Header.PorEmpresaADM.Value = this._solicitud.Header.PorEmpresaADM.Value ?? 0;
                this._solicitud.Header.PorClienteIMP.Value = this._solicitud.Header.PorClienteIMP.Value ?? 0;
                this._solicitud.Header.PorEmpresaIMP.Value = this._solicitud.Header.PorEmpresaIMP.Value ?? 0;
                this._solicitud.Header.PorClienteUTI.Value = this._solicitud.Header.PorClienteUTI.Value ?? 0;
                this._solicitud.Header.PorEmpresaUTI.Value = this._solicitud.Header.PorEmpresaUTI.Value ?? 0;
                this._solicitud.Header.ValorAdminAIU.Value = this._solicitud.Header.ValorAdminAIU.Value ?? 0;
                this._solicitud.Header.ValorImprAIU.Value = this._solicitud.Header.ValorImprAIU.Value ?? 0;
                this._solicitud.Header.ValorUtilAIU.Value = this._solicitud.Header.ValorUtilAIU.Value ?? 0;
                this._solicitud.DetalleProyecto.OrderBy(x => x.Consecutivo.Value);
               list.Add(this._solicitud);  

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
            #region Declara variables
            decimal totalTareasEmp = 0;
            decimal totalTareasAdicEmp = 0;
            decimal vlrAdmin = 0;
            decimal vlrImpr = 0;
            decimal vlrUtil = 0;
            decimal vlrIVA = 0;
            #endregion

            #region Calcula Totales
            if (this._solicitud.Detalle.Count > 0)
                totalTareasEmp = this._solicitud.Detalle.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoTotalML.Value.Value);
            if (this._solicitud.DetalleTareasAdic.Count > 0)
                totalTareasAdicEmp = this._solicitud.DetalleTareasAdic.FindAll(x => x.DetalleInd.Value.Value).Sum(x => x.CostoTotalML.Value.Value);

            if (!this._solicitud.Header.APUIncluyeAIUInd.Value.Value)
            {
                vlrAdmin = totalTareasEmp * (this._solicitud.Header.PorEmpresaADM.Value.Value / 100);
                vlrImpr = totalTareasEmp * (this._solicitud.Header.PorEmpresaIMP.Value.Value / 100);
                vlrUtil = totalTareasEmp * (this._solicitud.Header.PorEmpresaUTI.Value.Value / 100);
            }
            vlrIVA = vlrUtil * Convert.ToDecimal(16 * 0.01);
            #endregion            
        }

        #endregion
    }
}
