using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using System.Linq;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Reports.Fijos
{
    public partial class Report_Cc_GestionCobranzaDia : XtraReport
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
        public Report_Cc_GestionCobranzaDia()
        {

        }

        public Report_Cc_GestionCobranzaDia(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_Cc_GestionCobranzaDia(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc = null)
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

            this.lblUserName.Text = usuario.Descriptivo.Value;

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

        /// <summary>
        /// Funcion para crear los paramatros de conexion
        /// </summary>
        protected void ConfigureConnection(SqlDataSource source)
        {
            try
            {
                MsSqlConnectionParameters connParameters = new MsSqlConnectionParameters();
                SqlConnectionStringBuilder credentials = new SqlConnectionStringBuilder(this.loggerConnectionStr);
                connParameters.DatabaseName = this._connection.Database;
                connParameters.ServerName = this._connection.DataSource;
                connParameters.AuthorizationType = credentials.IntegratedSecurity ? MsSqlAuthorizationType.Windows : MsSqlAuthorizationType.SqlServer;
                connParameters.UserName = credentials.UserID;
                connParameters.Password = credentials.Password;
                source.ConnectionName = "NewAgeConnection";
                source.ConnectionParameters = connParameters;
            }
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this._userID.ToString(), "ConfigureConnection");
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string tipoReporte, DateTime fechaCorte)
        {            
            try
            {
                byte tipoRep = Convert.ToByte(tipoReporte);
              
                //Valida si es reporte Diario
                if (tipoRep == 1)
                {
                    this.tblCellFecha.DataBindings.Clear();
                    this.tblCellFecha.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] { new DevExpress.XtraReports.UI.XRBinding("Text", null, "Cartera_ReporteGestionDiariaCobranza.fecha", "{0:dd}") });
                    this.lblMes.Text = fechaCorte.ToString("MMMM");
                }
                else if (tipoRep == 2)
                {
                    this.lblTitulo.Text = "GESTIÓN COBRANZA ANUAL";
                    this.lblMes.Visible = false;
                    this.lblDia.Text = "Mes";
                }
                #region Asigna Filtros
                this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDatasource.Queries[0].Parameters[1].Value = fechaCorte;
                this.QueriesDatasource.Queries[0].Parameters[2].Value = tipoRep;
                #endregion

              

                this.ConfigureConnection(this.QueriesDatasource);
                this.CreateReport();
                return this.ReportName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

    }
}
