﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using NewAge.DTO.Negocio;
using NewAge.Librerias.Project;
using NewAge.Negocio;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_Cc_CobroJurHistorico : XtraReport
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
        protected Image logoFactura = null;
        protected string letrasValor = string.Empty;

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

        public Report_Cc_CobroJurHistorico()
        {
        }

        public Report_Cc_CobroJurHistorico(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_Cc_CobroJurHistorico(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
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
        #endregion

        #region Funciones

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
        protected virtual void SetUserParameters()
        {
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);


            #region Header
            if (usuario != null)
                this.lblUserName.Text = usuario.Descriptivo.Value;
           

            #endregion
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
        protected virtual void SetInitParameters()
        {
            this._moduloGlobal = new ModuloGlobal(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            this._moduloGlobal.Empresa = this.Empresa;
            this._moduloBase = new ModuloBase(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
            DTO_seUsuario usuario = this._moduloGlobal.seUsuario_GetUserByReplicaID(this._userID);

            #region Header

            if (usuario != null)
                this.lblUserName.Text = usuario.Descriptivo.Value;          

            #endregion
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
        protected void CreateReport()
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

        /// <summary>
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(int numeroDocCred, byte claseDeuda)
        {
            #region Asigna Filtros
            this.QueriesDatasource.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.QueriesDatasource.Queries[0].Parameters[1].Value = numeroDocCred;

            this.QueriesDatasource.Queries[1].Parameters[0].Value = this.Empresa.ID.Value;
            this.QueriesDatasource.Queries[1].Parameters[1].Value = numeroDocCred;

            if (claseDeuda == 1) // Imprime Principal
                this.DetailReportPoliza.Visible = false;
            else if (claseDeuda == 2) // Imprime Poliza
            {
                this.tabla1.Visible = false;
                this.tabla1Titulo.Visible = false;
                this.tabla2.Visible = false;
                this.Tabla0Titulo.Visible = false;
                this.lineaCruzada.Visible = false;
                this.DetailReportPrincipal.Visible = false;
            }
            else if (claseDeuda == 4) // Imprime Juzgado
            {
                this.tabla1.Visible = false;
                this.tabla1Titulo.Visible = false;
                this.tabla2.Visible = false;
                this.DetailReportPoliza.Visible = false;
                this.Tabla0Titulo.Visible = false;
                this.xrLabel1.Visible = false;
                this.lineaCruzada.Visible = false;
                this.DetailReportPoliza.Visible = false;
            }
            #endregion

            this.ConfigureConnection(this.QueriesDatasource);
            this.ShowPreview();// this.CreateReport();
            return this.ReportName;
        }
    }
}