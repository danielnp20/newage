using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public partial class Report_Cc_AutorizacionDescuento : XtraReport
    {

        #region Variable
        // variable para convertir en letras los valores
        private string mdaLocal = string.Empty;
        //Variable de error
        string Libranzavalidation = string.Empty;
        protected ModuloGlobal _moduloGlobal;
        protected ModuloCartera _moduloCartera;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected DTO_glEmpresa _empresa;
        protected int _userID;
        protected ModuloBase _moduloBase;
        protected ExportFormatType _formatType;
        protected ReportProvider reportProvider;
        protected int? numeroDoc;
        protected string loggerConnectionStr;
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

        public Report_Cc_AutorizacionDescuento()
        {
        }
        public Report_Cc_AutorizacionDescuento(string loggerConn, SqlConnection conn, SqlTransaction tx, DTO_glEmpresa emp, int userID, ExportFormatType formatType, int? numDoc = null)
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

        public Report_Cc_AutorizacionDescuento(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        #region Funciones
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
        /// Inicializa el origen de datos del reporte
        /// </summary>
        public string GenerateReport(string libranza)
        {
            // Convierte Valor en Letras
            this.mdaLocal = this._moduloGlobal.GetControlValueByCompany(ModulesPrefix.co, AppControl.co_MonedaLocal);
            this._moduloCartera = new ModuloCartera(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);

            #region Trae la ciudad de Exp
            List<DTO_ccSolicitudDatosPersonales> datos = this._moduloCartera.RegistroSolicitud_GetByLibranza(Convert.ToInt32(libranza));
            foreach (DTO_ccSolicitudDatosPersonales d in datos)
            {
                DTO_coTercero tercero = (DTO_coTercero)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Simple, AppMasters.coTercero, d.TerceroID.Value, true, false);
                DTO_glLugarGeografico ciudad = (DTO_glLugarGeografico)this._moduloGlobal.GetMasterDTO(AppMasters.MasterType.Hierarchy, AppMasters.glLugarGeografico, tercero != null ? tercero.LugarGeograficoID.Value : d.CiudadExpDoc.Value, true, false);
                if (d.TipoPersona.Value == 1 && ciudad != null)
                {
                    this.lblCiudadExp.Text = ciudad.Descriptivo.Value;
                    this.label7.Text = ciudad.Descriptivo.Value;
                }
                else if (d.TipoPersona.Value == 2 && ciudad != null)
                {
                    this.xrLabel17.Text = ciudad.Descriptivo.Value;
                    this.label20.Text = ciudad.Descriptivo.Value;
                }
            }

            #endregion

            if (this.lblPLazoLetra != null)
                this.lblPLazoLetra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPLazoLetra_BeforePrint);
            if (this.lblValorCuotaLetra != null)
                this.lblValorCuotaLetra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblValorCuotaLetra_BeforePrint);

            if (this.lblPlazo2 != null)
                this.lblPlazo2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPLazo2_BeforePrint);
            if (this.lblValor2 != null)
                this.lblValor2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblValor2_BeforePrint);

            // Parametros para trabajar sobre el Query
            this.DataSourceLibranza.Queries[0].Parameters[0].Value = this.Empresa.ID.Value;
            this.DataSourceLibranza.Queries[0].Parameters[1].Value = !string.IsNullOrEmpty(libranza.ToString()) ? libranza.ToString() : null;

            this.ConfigureConnection(this.DataSourceLibranza);
            this.ShowPreview();
            //this.CreateReport();
            return this.ReportName;

        } 
        #endregion

        #region Eventos

        private void lblPLazoLetra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel Plazo = FindControl("lblPlazo", true) as XRLabel;
            if (!string.IsNullOrEmpty(Plazo.Text))
            {
                Decimal NumeroCuotas = Convert.ToDecimal(Plazo.Text);
                this.lblPLazoLetra.Text = CurrencyFormater.GetCurrencyString("ES", "num", NumeroCuotas);
            }

        }

        private void lblValorCuotaLetra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel VlrCuota = FindControl("lblVlrCuota", true) as XRLabel;
            if (!string.IsNullOrEmpty(VlrCuota.Text))
            {
                Decimal ValorCuotas = Convert.ToDecimal(VlrCuota.Text);
                this.lblValorCuotaLetra.Text = CurrencyFormater.GetCurrencyString("ES", "cop", ValorCuotas);
            }
        }

        private void lblPLazo2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel Plazo = FindControl("lblPlazo", true) as XRLabel;
            if (!string.IsNullOrEmpty(Plazo.Text))
            {
                Decimal NumeroCuotas = Convert.ToDecimal(Plazo.Text);
                this.lblPlazo2.Text = CurrencyFormater.GetCurrencyString("ES", "num", NumeroCuotas);
            }

        }

        private void lblValor2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel VlrCuota = FindControl("lblVlrCuota", true) as XRLabel;
            if (!string.IsNullOrEmpty(VlrCuota.Text))
            {
                Decimal ValorCuotas = Convert.ToDecimal(VlrCuota.Text);
                this.lblValor2.Text = CurrencyFormater.GetCurrencyString("ES", "cop", ValorCuotas);
            }
        }

        #endregion

    }
}
