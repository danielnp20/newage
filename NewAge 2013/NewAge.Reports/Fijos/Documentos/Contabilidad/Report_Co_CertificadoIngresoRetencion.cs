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
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Reports.Fijos.Documentos.Cartera
{
    public partial class Report_Co_CertificadoIngresoRetencion : XtraReport
    {
        #region Variables

        protected ModuloGlobal _moduloGlobal;

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

        #region Constructor
        public Report_Co_CertificadoIngresoRetencion()
        {

        }

        public Report_Co_CertificadoIngresoRetencion(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
        {
            InitializeComponent();

            this._connection = conn;
            this._transaction = tx;
            this._empresa = emp;
            this._userID = userID;
            this._formatType = formatType;
            this.loggerConnectionStr = loggerConn;

            this.numeroDoc = numDoc;
            this.SetInitParameters();
        }

        public Report_Co_CertificadoIngresoRetencion(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }
        
        #endregion

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(byte tipoRep, DateTime fechaIni, DateTime fechaFin, string tercero)
        {
            try
            {                
                string terceroEmpresa = this._moduloBase.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_TerceroXDefecto);
                int num = 10000;
                #region Asigna Filtros
                this.QueriesDataSource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
                this.QueriesDataSource.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(tercero) ? tercero : null;
                this.QueriesDataSource.Queries[0].Parameters[2].Value = num;
                //this.QueriesDataSource.Queries[0].Parameters[3].Value = fechaIni.Month;
                //this.QueriesDataSource.Queries[0].Parameters[4].Value = fechaFin.Month;
                //this.QueriesDataSource.Queries[0].Parameters[5].Value = !string.IsNullOrEmpty(impuesto1) ? impuesto1 : null;
                //this.QueriesDataSource.Queries[0].Parameters[6].Value = !string.IsNullOrEmpty(impuesto2) ? impuesto2 : null;
                //this.QueriesDataSource.Queries[0].Parameters[7].Value = !string.IsNullOrEmpty(tercero) ? tercero : null;
                #endregion        

                this.ConfigureConnection(this.QueriesDataSource);
                this.CreateReport();        

                return this.ReportName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        
        private void Detail1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var a = this.QueriesDataSource.Queries[0];
        }

        #region Funciones Virtuales

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
        }

        /// <summary>
        /// Inicializa objetos y parametros iniciales
        /// </summary>
        protected virtual void SetInitParameters()
        {
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Nombre del reporte

            string repName;
            string filesPath = this._moduloGlobal.GetControlValue(AppControl.RutaFisicaArchivos);
            string docsPath = this._moduloGlobal.GetControlValue(AppControl.RutaTemporales);
            string ext = this.GetExtention();

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

            this.ReportName = repName.ToString() + ext;
            this.Path = filesPath + docsPath + ReportName;

            #endregion
            #region Recusos
            this.reportProvider = new ReportProvider(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this.reportProvider.LoadResources(this.AllControls<XRControl>());

            #endregion
        }

        /// <summary>
        /// Función que exporta de acuerdo al tipo de formato seleccionado por el usuario
        /// </summary>
        protected void CreateReport()
        {
            try
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
            catch (Exception ex)
            {
                Mentor_Exception.LogException_Local(this.loggerConnectionStr, ex, this._userID.ToString(), "CreateReport");
                throw ex;
            }
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

        #region Funciones Privadas

        /// <summary>
        /// Funcion que de acuerdo al ExportType devuelve la extensión
        /// </summary>
        private string GetExtention()
        {
            string extension = string.Empty;
            switch (this._formatType)
            {
                case ExportFormatType.pdf:
                    extension = ".pdf";
                    break;
                case ExportFormatType.xls:
                    extension = ".xls";
                    break;
                case ExportFormatType.xlsx:
                    extension = ".xlsx";
                    break;
                case ExportFormatType.html:
                    extension = ".html";
                    break;
            }
            return extension;
        }
        #endregion

        private void DetailPersonasDep_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //if (this.DetailBienesPoseidos.RowCount == 1)
            //    this.DetailPersonasDep.ReportPrintOptions.BlankDetailCount = 10;
            //else if (this.DetailBienesPoseidos.RowCount == 2)
            //    this.DetailPersonasDep.ReportPrintOptions.BlankDetailCount = 9;
            //else if (this.DetailBienesPoseidos.RowCount == 3)
            //    this.DetailPersonasDep.ReportPrintOptions.BlankDetailCount = 8;
            //else if (this.DetailBienesPoseidos.RowCount == 4)
            //    this.DetailPersonasDep.ReportPrintOptions.BlankDetailCount = 7;
            //else if (this.DetailBienesPoseidos.RowCount == 5)
            //    this.DetailPersonasDep.ReportPrintOptions.BlankDetailCount = 6;
            //else if (this.DetailBienesPoseidos.RowCount == 6)
            //    this.DetailPersonasDep.ReportPrintOptions.BlankDetailCount = 5;
            //else if (this.DetailBienesPoseidos.RowCount == 7)
            //    this.DetailPersonasDep.ReportPrintOptions.BlankDetailCount = 4;
            //else if (this.DetailBienesPoseidos.RowCount == 8)
            //    this.DetailPersonasDep.ReportPrintOptions.BlankDetailCount = 3;
        }

        private void lblNroForm_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string nit = this.lblNit.Text.Trim();
            string cedula = this.lblCed.Text.Trim();
            string newText = string.Empty;

            if (cedula.Contains(" "))
                return;

            for (int i = 0; i < nit.Length; i++)
                newText = newText + (nit[i] + "   ");
            this.lblNit.Text = newText;

            newText = string.Empty;
            for (int i = 0; i < cedula.Length; i++)
                newText = newText + (cedula[i] + "   ");
            this.lblCed.Text = newText;
        }
    }
}
