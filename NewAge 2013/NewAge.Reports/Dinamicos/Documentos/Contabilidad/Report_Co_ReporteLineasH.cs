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

namespace NewAge.Reports.Dinamicos
{
    public partial class Report_Co_ReporteLineasH : XtraReport
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
        public Report_Co_ReporteLineasH()
        {

        }

        public Report_Co_ReporteLineasH(DTO_glEmpresa emp)
        {
            this.InitializeComponent();
            this._empresa = emp;
        }

        public Report_Co_ReporteLineasH(string loggerConn, SqlConnection c, SqlTransaction tx, DTO_glEmpresa empresa, int userId, ExportFormatType formatType, int? numDoc = null)
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
            catch {; }

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

            //this.lblNombreEmpresa.Text = this.Empresa.Descriptivo.Value;
            //this.lblUserName.Text = usuario.Descriptivo.Value;

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
        public string GenerateReport(int documentReport, string reporteID, byte tipoReport, DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                #region Variables
                ModuloContabilidad modContable = new ModuloContabilidad(this._connection, this._transaction, this._empresa, this._userID, this.loggerConnectionStr);
                #endregion

                this.lblFechas.Text = fechaFinal.ToString("'A' dd 'de' MMMM 'de' yyyy").ToUpper();
                List<DTO_coReporteLinea> final = new List<DTO_coReporteLinea>();
                DTO_coReporteLinea first = new DTO_coReporteLinea();
                first.ReporteID.Value = reporteID;

                //Obtiene saldos contables segun el tipo de Reporte
                List<DTO_coReporteLinea> lineas = modContable.Saldo_GetSaldosByLineaReporte(reporteID, fechaInicial,fechaFinal,string.Empty,string.Empty,string.Empty, string.Empty); //Parametrizar el reporte 
                if (documentReport == AppReports.coReporteSituacionFinanciero)
                {
                    int i = 1;
                    foreach (DTO_coReporteLinea lin in lineas)
                    {
                        switch (i)
                        {
                            case 1:
                                first.DescLin1 = lin.Descriptivo.Value;
                                first.VlrLin1 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 2:
                                first.DescLin2 = lin.Descriptivo.Value;
                                first.VlrLin2 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 3:
                                first.DescLin3 = lin.Descriptivo.Value;
                                first.VlrLin3= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 4:
                                first.DescLin4 = lin.Descriptivo.Value;
                                first.VlrLin4= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 5:
                                first.DescLin5 = lin.Descriptivo.Value;
                                first.VlrLin5= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 6:
                                first.DescLin6 = lin.Descriptivo.Value;
                                first.VlrLin6= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 7:
                                first.DescLin7 = lin.Descriptivo.Value;
                                first.VlrLin7 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 8:
                                first.DescLin8 = lin.Descriptivo.Value;
                                first.VlrLin8= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 9:
                                first.DescLin9 = lin.Descriptivo.Value;
                                first.VlrLin9= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 10:
                                first.DescLin10 = lin.Descriptivo.Value;
                                first.VlrLin10 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 11:
                                first.DescLin11 = lin.Descriptivo.Value;
                                first.VlrLin11 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 12:
                                first.DescLin12 = lin.Descriptivo.Value;
                                first.VlrLin12 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 13:
                                first.DescLin13 = lin.Descriptivo.Value;
                                first.VlrLin13 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 14:
                                first.DescLin14 = lin.Descriptivo.Value;
                                first.VlrLin14 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 15:
                                first.DescLin15 = lin.Descriptivo.Value;
                                first.VlrLin15 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 16:
                                first.DescLin16 = lin.Descriptivo.Value;
                                first.VlrLin16 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 17:
                                first.DescLin17 = lin.Descriptivo.Value;
                                first.VlrLin17 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 18:
                                first.DescLin18 = lin.Descriptivo.Value;
                                first.VlrLin18= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 19:
                                first.DescLin19 = lin.Descriptivo.Value;
                                first.VlrLin19 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 20:
                                first.DescLin20 = lin.Descriptivo.Value;
                                first.VlrLin20 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 21:
                                first.DescLin21 = lin.Descriptivo.Value;
                                first.VlrLin21 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 22:
                                first.DescLin22 = lin.Descriptivo.Value;
                                first.VlrLin22 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 23:
                                first.DescLin23 = lin.Descriptivo.Value;
                                first.VlrLin23 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 24:
                                first.DescLin24 = lin.Descriptivo.Value;
                                first.VlrLin24 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 25:
                                first.DescLin25 = lin.Descriptivo.Value;
                                first.VlrLin25 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 26:
                                first.DescLin26 = lin.Descriptivo.Value;
                                first.VlrLin26 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 27:
                                first.DescLin27 = lin.Descriptivo.Value;
                                first.VlrLin27 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 28:
                                first.DescLin28 = lin.Descriptivo.Value;
                                first.VlrLin28 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 29:
                                first.DescLin29 = lin.Descriptivo.Value;
                                first.VlrLin29 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 30:
                                first.DescLin30 = lin.Descriptivo.Value;
                                first.VlrLin30 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 31:
                                first.DescLin31 = lin.Descriptivo.Value;
                                first.VlrLin31 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 32:
                                first.DescLin32 = lin.Descriptivo.Value;
                                first.VlrLin32= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 33:
                                first.DescLin33 = lin.Descriptivo.Value;
                                first.VlrLin33 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 34:
                                first.DescLin34 = lin.Descriptivo.Value;
                                first.VlrLin34 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 35:
                                first.DescLin35 = lin.Descriptivo.Value;
                                first.VlrLin35 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 36:
                                first.DescLin36 = lin.Descriptivo.Value;
                                first.VlrLin36 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 37:
                                first.DescLin37 = lin.Descriptivo.Value;
                                first.VlrLin37 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 38:
                                first.DescLin38 = lin.Descriptivo.Value;
                                first.VlrLin38 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 39:
                                first.DescLin39 = lin.Descriptivo.Value;
                                first.VlrLin39 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 40:
                                first.DescLin40 = lin.Descriptivo.Value;
                                first.VlrLin40 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 41:
                                first.DescLin41 = lin.Descriptivo.Value;
                                first.VlrLin41 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 42:
                                first.DescLin42 = lin.Descriptivo.Value;
                                first.VlrLin42 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 43:
                                first.DescLin43 = lin.Descriptivo.Value;
                                first.VlrLin43 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 44:
                                first.DescLin44 = lin.Descriptivo.Value;
                                first.VlrLin44= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 45:
                                first.DescLin45 = lin.Descriptivo.Value;
                                first.VlrLin45= lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 46:
                                first.DescLin46 = lin.Descriptivo.Value;
                                first.VlrLin46 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 47:
                                first.DescLin47 = lin.Descriptivo.Value;
                                first.VlrLin47 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 48:
                                first.DescLin48 = lin.Descriptivo.Value;
                                first.VlrLin48 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 49:
                                first.DescLin49 = lin.Descriptivo.Value;
                                first.VlrLin49 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                            case 50:
                                first.DescLin50 = lin.Descriptivo.Value;
                                first.VlrLin50 = lin.VlrTotalMLRepLinea.Value.Value;
                                break;
                        }          
                        i++;
                    }                    
                }
                final.Add(first);
                DataSource = final;
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
